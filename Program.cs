using OpenCvSharp;

partial class MarchingSquares
{
    // Settings
    const int WIDTH = 1200;
    const int HEIGHT = 1000;
    const int CELL_SIZE = 100; 
    const int MIN = 0;
    const int MAX = 1;
    const float THRESHOLD = 0.5f;
    const bool IS_DISPLAY_TEXT = true; // Either show text or dot for points. Ideally set it to false at below 40 cell size
    const bool IS_REMOVE_SQUARE = false; // Remove 1x1 squares


    static readonly Random RANDOM = new Random();
    const int ESC = 27;

    static void Main(string[] args)
    {
        int cols = WIDTH / CELL_SIZE;   // Number of cells horizontally
        int rows = HEIGHT / CELL_SIZE;  // Number of cells vertically

        // Create a window
        using Mat canvas = new Mat(HEIGHT, WIDTH, MatType.CV_8UC3);

        while (true)
        {
            float[,] scalarField = CreateScalarField(cols, rows);

            ResetScreen(canvas);
            DrawGrid(canvas, scalarField, CELL_SIZE);
            
            bool isDisplayText = IS_DISPLAY_TEXT;
            if (isDisplayText == true)
                DrawVertexNumbers(canvas, scalarField, CELL_SIZE);
            else
                DrawVertexDots(canvas, scalarField, CELL_SIZE, THRESHOLD);
        
            RefreshScreen(canvas);
            if (Cv2.WaitKey(0) == ESC) break;

            PerformMarchingSquares(canvas, scalarField, CELL_SIZE, THRESHOLD);
            RefreshScreen(canvas);
            if (Cv2.WaitKey(0) == ESC) break;

            ResetScreen(canvas);
            PerformMarchingSquares(canvas, scalarField, CELL_SIZE, THRESHOLD);

            RefreshScreen(canvas);
            if (Cv2.WaitKey(0) == ESC) break;

            bool isRemoveSquare = IS_REMOVE_SQUARE;
            if (isRemoveSquare == true)
            {
                ResetScreen(canvas);
                RemoveSmallSquares(scalarField, THRESHOLD);
                PerformMarchingSquares(canvas, scalarField, CELL_SIZE, THRESHOLD);

                RefreshScreen(canvas);
                if (Cv2.WaitKey(0) == ESC) break;
            }
        }

        Cv2.DestroyAllWindows();
    }
}
