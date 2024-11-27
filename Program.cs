using OpenCvSharp;

partial class MarchingSquares
{
    // Settings
    const int WIDTH = 1200;
    const int HEIGHT = 1000;
    const int CELLSIZE = 100; 
    const float THRESHOLD = 0.5f;
    const bool IS_DISPLAY_TEXT = true;

    static void Main(string[] args)
    {
        int cols = WIDTH / CELLSIZE;   // Number of cells horizontally
        int rows = HEIGHT / CELLSIZE;  // Number of cells vertically

        // Create a window
        using var canvas = new Mat(HEIGHT, WIDTH, MatType.CV_8UC3);

        while (true)
        {
            float[,] scalarField = CreateScalarField(cols, rows);

            ResetScreen(canvas);

            // Draw grid and numbers
            DrawGrid(canvas, cols, rows, CELLSIZE);
            if (IS_DISPLAY_TEXT == true)
                DrawVertexNumbers(canvas, scalarField, cols, rows, CELLSIZE);
            else
                DrawVertexDots(canvas, scalarField, cols, rows, CELLSIZE, THRESHOLD);
        
            RefreshScreen(canvas);
            int key = Cv2.WaitKey(0);
            if (key == 'q' || key == 81)
                break;

            PerformMarchingSquares(canvas, scalarField, cols, rows, CELLSIZE, THRESHOLD);
            RefreshScreen(canvas);
            key = Cv2.WaitKey(0);
            if (key == 'q' || key == 81)
                break;

            ResetScreen(canvas);
            PerformMarchingSquares(canvas, scalarField, cols, rows, CELLSIZE, THRESHOLD);

            RefreshScreen(canvas);
            key = Cv2.WaitKey(0);
            if (key == 'q' || key == 81)
                break;
        }

        Cv2.DestroyAllWindows();
    }
}
