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
    static void DrawGrid(Mat canvas, float[,] scalarField, int cellSize)
    {
        for (int y = 0; y < scalarField.GetLength(1); y++)
        {
            for (int x = 0; x < scalarField.GetLength(0); x++)
            {
                // Draw grid cells
                Cv2.Rectangle(canvas,
                    new Point(x * cellSize, y * cellSize),
                    new Point((x + 1) * cellSize, (y + 1) * cellSize),
                    Scalar.Gray, 1); // Thin white lines
            }
        }
    }

    static void DrawVertexNumbers(Mat canvas, float[,] scalarField, int cellSize)
    {
        for (int y = 0; y < scalarField.GetLength(1); y++)
        {
            for (int x = 0; x < scalarField.GetLength(0); x++)
            {
                float value = scalarField[x, y];
                
                Point position = new Point(x * cellSize - 10, y * cellSize + 10);

                Cv2.PutText(canvas, value.ToString(), position, HersheyFonts.HersheyScriptSimplex, 1, Scalar.White, 2);
            }
        }
    }

    static void DrawVertexDots(Mat canvas, float[,] scalarField, int cellSize, float threshold)
    {
        for (int y = 0; y < scalarField.GetLength(1); y++)
        {
            for (int x = 0; x < scalarField.GetLength(0); x++)
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
    static void PerformMarchingSquares(Mat canvas, float[,] scalarField, int cellSize, float threshold, bool isFill)
    {
        for (int y = 0; y < scalarField.GetLength(1) - 1; y++)
        {
            for (int x = 0; x < scalarField.GetLength(0) - 1; x++)
            {
                int caseIndex = GetIndex(scalarField, x, y, threshold);

                // Calculate midpoints
                Point top = new Point((x + 0.5f) * cellSize, y * cellSize);
                Point bottom = new Point((x + 0.5f) * cellSize, (y + 1) * cellSize);
                Point left = new Point(x * cellSize, (y + 0.5f) * cellSize);
                Point right = new Point((x + 1) * cellSize, (y + 0.5f) * cellSize);

                // Calculate corners of the cell
                Point topLeft = new Point(x * cellSize, y * cellSize);
                Point topRight = new Point((x + 1) * cellSize, y * cellSize);
                Point bottomLeft = new Point(x * cellSize, (y + 1) * cellSize);
                Point bottomRight = new Point((x + 1) * cellSize, (y + 1) * cellSize);

                Scalar color = Scalar.White;

                // Draw contours based on case index
                switch (caseIndex)
                {
                    case 0: // No contour
                        break;
                    case 1:
                        if (isFill) DrawPoly(canvas, [[left, bottomLeft, bottom]], color);
                        else DrawLine(canvas, left, bottom, color);
                        break;
                    case 2:
                        if (isFill) DrawPoly(canvas, [[right, bottomRight, bottom]], color);
                        else DrawLine(canvas, right, bottom, color);
                        break;
                    case 3:
                        if (isFill) DrawPoly(canvas, [[left, bottomLeft, bottomRight, right]], color);
                        else DrawLine(canvas, left, right, color);
                        break;
                    case 4:
                        if (isFill) DrawPoly(canvas, [[top, topRight, right]], color);
                        else DrawLine(canvas, top, right, color);
                        break;
                    case 5:
                        if (isFill) DrawPoly(canvas, [[bottomLeft, left, top, topRight, right, bottom]], color);
                        else
                        {
                            DrawLine(canvas, left, top, color);
                            DrawLine(canvas, bottom, right, color);
                        }
                        break;
                    case 6:
                        if (isFill) DrawPoly(canvas, [[top, topRight, bottomRight, bottom]], color);
                        else DrawLine(canvas, top, bottom, color);
                        break;
                    case 7:
                        if (isFill) DrawPoly(canvas, [[bottomLeft, left, top, topRight, bottomRight]], color);
                        else DrawLine(canvas, left, top, color);
                        break;
                    case 8:
                        if (isFill) DrawPoly(canvas, [[left, topLeft, top]], color);
                        else DrawLine(canvas, top, left, color);
                        break;
                    case 9:
                        if (isFill) DrawPoly(canvas, [[topLeft, top, bottom, bottomLeft]], color);
                        else DrawLine(canvas, top, bottom, color);
                        break;
                    case 10:
                        if (isFill) DrawPoly(canvas, [[left, topLeft, top, right, bottomRight, bottom]], color);
                        else
                        {
                            DrawLine(canvas, left, bottom, color);
                            DrawLine(canvas, top, right, color);
                        }   
                        break;
                    case 11:
                        if (isFill) DrawPoly(canvas, [[topLeft, top, right, bottomRight, bottomLeft]], color);
                        else DrawLine(canvas, top, right, color);
                        break;
                    case 12:
                        if (isFill) DrawPoly(canvas, [[topLeft, topRight, right, left]], color);
                        else DrawLine(canvas, left, right, color);
                        break;
                    case 13:
                        if (isFill) DrawPoly(canvas, [[bottomLeft, topLeft, topRight, right, bottom]], color);
                        else DrawLine(canvas, bottom, right, color);
                        break;
                    case 14:
                        if (isFill) DrawPoly(canvas, [[left, topLeft, topRight, bottomRight, bottom]], color);
                        else DrawLine(canvas, left, bottom, color);
                        break;
                    case 15:  // No contour
                        if (isFill) DrawPoly(canvas, [[topLeft, topRight, bottomRight, bottomLeft]], color);
                        break;
                }
            }
        }
    }

    // Convert square into index
    static int GetIndex(float[,] scalarField, int x, int y, float threshold)
    {
        int index = 0;

        // Get values for each corner
        // Bigger y is closer to bottom
        float topLeft = scalarField[x, y];
        float topRight = scalarField[x + 1, y];
        float bottomRight = scalarField[x + 1, y + 1]; 
        float bottomLeft = scalarField[x, y + 1]; 

        if (topLeft > threshold) index |= 8;
        if (topRight > threshold) index |= 4;
        if (bottomRight > threshold) index |= 2;
        if (bottomLeft > threshold) index |= 1;

        return index;
    }

    // Helper method to draw a line
    static void DrawLine(Mat canvas, Point start, Point end, Scalar color)
    {
        Cv2.Line(canvas, new Point(start.X, start.Y), new Point(end.X, end.Y), color, 2);
    }

    static void DrawPoly(Mat canvas, Point[][] points, Scalar color)
    {
        Cv2.FillPoly(canvas, points, color); 
    }


    // Optional remover for 1x1 squares
    static float[,] RemoveSmallSquares(float[,] scalarField, float threshold, int min, int max)
    {
        float[,] newScalarField = (float[,])scalarField.Clone(); 

        for (int y = 0; y < newScalarField.GetLength(1) - 2; y++)
        {  
            for (int x = 0; x < newScalarField.GetLength(0) - 2; x++)
            {
                // Get the index of each square based on its corner points
                int topLeftCaseIndex = GetIndex(newScalarField, x, y, threshold);
                int topRightCaseIndex = GetIndex(newScalarField, x + 1, y, threshold);
                int bottomRightCaseIndex = GetIndex(newScalarField, x + 1, y + 1, threshold);
                int bottomLeftCaseIndex = GetIndex(newScalarField, x, y + 1, threshold);

                // Arrays of case indexes where small squares should be "fixed"
                int[] bottomRightLineIndexes = new int[] { 2, 5, 13 };
                int[] bottomLeftLineIndexes = new int[] { 1, 10, 14 };
                int[] topLeftLineIndexes = new int[] { 5, 7, 8 };
                int[] topRightLineIndexes = new int[] { 4, 10, 11 };

                // Check the conditions where small squares are present
                bool isTopLeftFix = bottomRightLineIndexes.Contains(topLeftCaseIndex);
                bool isTopRightFix = bottomLeftLineIndexes.Contains(topRightCaseIndex);
                bool isBottomRightFix = topLeftLineIndexes.Contains(bottomRightCaseIndex);
                bool isBottomLeftFix = topRightLineIndexes.Contains(bottomLeftCaseIndex);

                // If all four conditions are met, count it as a small square
                if (isTopLeftFix && isTopRightFix && isBottomRightFix && isBottomLeftFix)
                {
                    newScalarField[x + 1, y + 1] = newScalarField[x + 1, y + 1] > threshold ? min : max;
                }
            }
        }

        return newScalarField;
    }
}
