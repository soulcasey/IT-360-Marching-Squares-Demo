using OpenCvSharp;

partial class MarchingSquares
{
    static void RefreshScreen(Mat canvas)
    {
        Cv2.ImShow("IT-360", canvas);
    }

    static void ResetScreen(Mat canvas)
    {
        canvas.SetTo(Scalar.Black);
    }

    static float[,] CreateScalarField(int cols, int rows)
    {
        Random random = new Random();
        float[,] scalarField = new float[cols + 1, rows + 1];

        // Fill the scalar field with random values
        for (int y = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++)
            {
                scalarField[x, y] = random.Next(0, 2);
            }
        }

        return scalarField;
    }

    // Draw the grid lines
    static void DrawGrid(Mat canvas, int cols, int rows, int cellSize)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Draw grid cells
                Cv2.Rectangle(canvas,
                    new Point(x * cellSize, y * cellSize),
                    new Point((x + 1) * cellSize, (y + 1) * cellSize),
                    Scalar.Gray, 1); // Thin white lines
            }
        }
    }

    static void DrawVertexNumbers(Mat canvas, float[,] scalarField, int cols, int rows, int cellSize)
    {
        for (int y = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++)
            {
                float value = scalarField[x, y];
                
                Point position = new Point(x * cellSize - 10, y * cellSize + 10);

                Cv2.PutText(canvas, value.ToString(), position, HersheyFonts.HersheyScriptSimplex, 1, Scalar.White, 2);
            }
        }
    }

    static void DrawVertexDots(Mat canvas, float[,] scalarField, int cols, int rows, int cellSize, float threshold)
    {
        for (int y = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++)
            {
                float value = scalarField[x, y];

                Point position = new Point(x * cellSize, y * cellSize);
                Scalar color = value > threshold ? Scalar.White : Scalar.Black;
                int radius = 4;

                Cv2.Circle(canvas, position, radius, color, -1);
            }
        }
    }

    // Perform Marching Squares algorithm and draw contours
    static void PerformMarchingSquares(Mat canvas, float[,] scalarField, int cols, int rows, int cellSize, float threshold)
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                // Get scalar values for each corner
                // Bigger y is closer to bottom
                float topLeft = scalarField[x, y];
                float topRight = scalarField[x + 1, y];
                float bottomRight = scalarField[x + 1, y + 1]; 
                float bottomLeft = scalarField[x, y + 1]; 

                int caseIndex = GetIndex(topLeft, topRight, bottomRight, bottomLeft, threshold);

                // Calculate midpoints
                Point top = new Point((x + 0.5f) * cellSize, y * cellSize);
                Point bottom = new Point((x + 0.5f) * cellSize, (y + 1) * cellSize);
                Point left = new Point(x * cellSize, (y + 0.5f) * cellSize);
                Point right = new Point((x + 1) * cellSize, (y + 0.5f) * cellSize);

                // Draw contours based on case index
                switch (caseIndex)
                {
                    case 0: // No contour
                        break;
                    case 1: DrawLine(canvas, left, bottom, Scalar.Red); break;
                    case 2: DrawLine(canvas, right, bottom, Scalar.Red); break;
                    case 3: DrawLine(canvas, left, right, Scalar.Red); break;
                    case 4: DrawLine(canvas, top, right, Scalar.Red); break;
                    case 5: DrawLine(canvas, left, top, Scalar.Red); DrawLine(canvas, bottom, right, Scalar.Red); break;
                    case 6: DrawLine(canvas, top, bottom, Scalar.Red); break;
                    case 7: DrawLine(canvas, left, top, Scalar.Red); break;
                    case 8: DrawLine(canvas, top, left, Scalar.Red); break;
                    case 9: DrawLine(canvas, top, bottom, Scalar.Red); break;
                    case 10: DrawLine(canvas, left, bottom, Scalar.Red); DrawLine(canvas, top, right, Scalar.Red); break;
                    case 11: DrawLine(canvas, top, right, Scalar.Red); break;
                    case 12: DrawLine(canvas, left, right, Scalar.Red); break;
                    case 13: DrawLine(canvas, bottom, right, Scalar.Red); break;
                    case 14: DrawLine(canvas, left, bottom, Scalar.Red); break;
                    case 15:  // No contour
                        break;
                }
            }
        }
    }

    static int GetIndex(float topLeft, float topRight, float bottomRight, float bottomLeft, float threshold)
    {
        int index = 0;

        if (topLeft > threshold) index |= 8;
        if (topRight > threshold) index |= 4;
        if (bottomRight > threshold) index |= 2;
        if (bottomLeft > threshold) index |= 1;

        return index;
    }


    // Helper method to draw a line
    static void DrawLine(Mat canvas, Point start, Point end, Scalar color)
    {
        Cv2.Line(canvas, new Point(start.X, start.Y), new Point((int)end.X, (int)end.Y), color, 2);
    }
}
