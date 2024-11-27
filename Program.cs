using OpenCvSharp;

partial class MarchingSquares
{
    // Window and grid settings
    const int WIDTH = 1200;
    const int HEIGHT = 1000; // Window dimensions
    const int CELLSIZE = 40;             // Size of each cell
    const float THRESHOLD = 0.5f;        // Contour threshold

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
            DrawVertexNumbers(canvas, scalarField, cols, rows, CELLSIZE, THRESHOLD);

            RefreshScreen(canvas);
            int key = Cv2.WaitKey(0); // Wait for 1 millisecond for a key press
            if (key == 'q' || key == 81) // Check for 'q' or 'Q' key press
                break;

            PerformMarchingSquares(canvas, scalarField, cols, rows, CELLSIZE, THRESHOLD);
            RefreshScreen(canvas);
            key = Cv2.WaitKey(0); // Wait for 1 millisecond for a key press
            if (key == 'q' || key == 81) // Check for 'q' or 'Q' key press
                break;

            ResetScreen(canvas);
            PerformMarchingSquares(canvas, scalarField, cols, rows, CELLSIZE, THRESHOLD);

            RefreshScreen(canvas);
            key = Cv2.WaitKey(0); // Wait for 1 millisecond for a key press
            if (key == 'q' || key == 81) // Check for 'q' or 'Q' key press
                break;
        }

        Cv2.DestroyAllWindows();
    }
}
