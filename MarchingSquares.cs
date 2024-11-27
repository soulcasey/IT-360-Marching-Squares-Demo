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

    // Display numbers (1 or 0) at vertices of the grid
    static void DrawVertexNumbers(Mat canvas, float[,] scalarField, int cols, int rows, int cellSize, float threshold)
    {
        for (int y = 0; y <= rows; y++)
        {
            for (int x = 0; x <= cols; x++)
            {
                float value = scalarField[x, y];

                // Get the position of the vertex
                Point vertexPosition = new Point(x * cellSize, y * cellSize);

                // Draw the number at the vertex
                Cv2.PutText(canvas, value.ToString(), new Point(vertexPosition.X - 10, vertexPosition.Y + 10), HersheyFonts.HersheyScriptSimplex, 1, Scalar.White, 2);
            }
        }
    }

    // Perform Marching Squares algorithm and draw contours
    static void PerformMarchingSquares(Mat canvas, float[,] scalarField, int cols, int rows, int cellSize, float threshold)
    {
        Scalar red = new Scalar(0, 0, 255); // Red color for contours

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
                Point2f top = new Point2f((x + 0.5f) * cellSize, y * cellSize);
                Point2f bottom = new Point2f((x + 0.5f) * cellSize, (y + 1) * cellSize);
                Point2f left = new Point2f(x * cellSize, (y + 0.5f) * cellSize);
                Point2f right = new Point2f((x + 1) * cellSize, (y + 0.5f) * cellSize);

                // Draw contours based on case index
                switch (caseIndex)
                {
                    case 0: // No contour
                        break;
                    case 1: DrawLine(canvas, left, bottom, red); break;
                    case 2: DrawLine(canvas, right, bottom, red); break;
                    case 3: DrawLine(canvas, left, right, red); break;
                    case 4: DrawLine(canvas, top, right, red); break;
                    case 5: DrawLine(canvas, left, top, red); DrawLine(canvas, bottom, right, red); break;
                    case 6: DrawLine(canvas, top, bottom, red); break;
                    case 7: DrawLine(canvas, left, top, red); break;
                    case 8: DrawLine(canvas, top, left, red); break;
                    case 9: DrawLine(canvas, top, bottom, red); break;
                    case 10: DrawLine(canvas, left, bottom, red); DrawLine(canvas, top, right, red); break;
                    case 11: DrawLine(canvas, top, right, red); break;
                    case 12: DrawLine(canvas, left, right, red); break;
                    case 13: DrawLine(canvas, bottom, right, red); break;
                    case 14: DrawLine(canvas, left, bottom, red); break;
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
    static void DrawLine(Mat canvas, Point2f start, Point2f end, Scalar color)
    {
        Cv2.Line(canvas, new Point((int)start.X, (int)start.Y), new Point((int)end.X, (int)end.Y), color, 2);
    }
}
