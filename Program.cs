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

    static readonly Random RANDOM = new Random();
    const int ESC = 27;
    const int SPACE = 32;

    static void Main(string[] args)
    {
        int cols = WIDTH / CELL_SIZE;   // Number of cells horizontally
        int rows = HEIGHT / CELL_SIZE;  // Number of cells vertically

        // Create a window
        using Mat canvas = new Mat(HEIGHT, WIDTH, MatType.CV_8UC3);

        bool isShowGrid = true; // Q 
        bool isShowMarchingSquares = false; // W
        bool isRemoveSquare = false; // E
        bool isFill = false; // R

        float[,] scalarField = CreateScalarField(cols, rows, MIN, MAX);
        float[,] scalarFieldWithoutSquare = RemoveSmallSquares(scalarField, THRESHOLD, MIN, MAX);

        while (true)
        {
            float[,] targetScalarField = isRemoveSquare ? scalarFieldWithoutSquare : scalarField;

            ResetScreen(canvas);

            if (isShowGrid)
            {
                DrawGrid(canvas, targetScalarField, CELL_SIZE);

                bool isShowNumber = CELL_SIZE >= 40; // Set numbers to dots instead when cell size gets too small
                if (isShowNumber)
                    DrawVertexNumbers(canvas, targetScalarField, CELL_SIZE);
                else
                    DrawVertexDots(canvas, targetScalarField, CELL_SIZE, THRESHOLD);
            }

            if (isShowMarchingSquares)
                PerformMarchingSquares(canvas, targetScalarField, CELL_SIZE, THRESHOLD, isFill);

            RefreshScreen(canvas);

            int key = Cv2.WaitKey();
            switch (key)
            {
                case ESC:
                    Cv2.DestroyAllWindows();
                    return; // Exit the program

                case 'Q':
                case 'q':
                    isShowGrid = !isShowGrid; // Toggle showing grid
                    break;

                case 'W':
                case 'w':
                    isShowMarchingSquares = !isShowMarchingSquares; // Toggle showing grid
                    break;

                case 'E':
                case 'e':
                    isRemoveSquare = !isRemoveSquare; // Toggle marching squares
                    break;
                
                case 'R':
                case 'r':
                    isFill = !isFill; // Toggle fill
                    break;

                case SPACE:
                    scalarField = CreateScalarField(cols, rows, MIN, MAX);
                    scalarFieldWithoutSquare = RemoveSmallSquares(scalarField, THRESHOLD, MIN, MAX);
                    break;


            }
        }
    }
}