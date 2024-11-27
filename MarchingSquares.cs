using OpenCvSharp;

partial class MarchingSquares
{
    static void RefreshScreen(Mat canvas)
    {
        Cv2.ImShow("IT-360", canvas);
    }

    static void ResetScreen(Mat canvas)
    {
        Scalar backgroundColor = new Scalar(22, 3, 1);
        canvas.SetTo(backgroundColor);
    }

    static float[,] CreateScalarField(int cols, int rows, int min, int max)
    {
        float[,] scalarField = new float[cols + 1, rows + 1];

        // Fill the scalar field with random values
        for (int y = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++)
            {
                scalarField[x, y] = RANDOM.Next(min, max + 1);
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

                Scalar color = Scalar.White;

                // Draw contours based on case index
                switch (caseIndex)
                {
                    case 0: // No contour
                        break;
                    case 1: DrawLine(canvas, left, bottom, color); break;
                    case 2: DrawLine(canvas, right, bottom, color); break;
                    case 3: DrawLine(canvas, left, right, color); break;
                    case 4: DrawLine(canvas, top, right, color); break;
                    case 5: DrawLine(canvas, left, top, color); DrawLine(canvas, bottom, right, color); break;
                    case 6: DrawLine(canvas, top, bottom, color); break;
                    case 7: DrawLine(canvas, left, top, color); break;
                    case 8: DrawLine(canvas, top, left, color); break;
                    case 9: DrawLine(canvas, top, bottom, color); break;
                    case 10: DrawLine(canvas, left, bottom, color); DrawLine(canvas, top, right, color); break;
                    case 11: DrawLine(canvas, top, right, color); break;
                    case 12: DrawLine(canvas, left, right, color); break;
                    case 13: DrawLine(canvas, bottom, right, color); break;
                    case 14: DrawLine(canvas, left, bottom, color); break;
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
