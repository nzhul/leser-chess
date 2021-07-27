public static class PathfindingHelper
{
    public const int GRID_SIZE = 8;

    /// <summary>
    /// Checks movement in all directions. Which is combination of `Rook` and `Bishop` checks.
    /// </summary>
    public static void CheckAll(ref bool[,] matrix, int x, int y, int speed)
    {
        CheckRook(ref matrix, x, y, speed);
        CheckBishop(ref matrix, x, y, speed);
    }

    /// <summary>
    /// Checks `Rook` (♜) like movement.
    /// Rook movement allows movement in the following directions: right, left, up and down.
    /// </summary>
    public static void CheckRook(ref bool[,] matrix, int x, int y, int speed)
    {
        CheckRight(ref matrix, x, y, speed);
        CheckLeft(ref matrix, x, y, speed);
        CheckUp(ref matrix, x, y, speed);
        CheckDown(ref matrix, x, y, speed);
    }

    /// <summary>
    /// Checks `Bishop` (♝) like movement.
    /// Bishop movement allows movement in the following directions: top-left, top-right, down-left and down-right.
    /// </summary>
    public static void CheckBishop(ref bool[,] matrix, int x, int y, int speed)
    {
        CheckTopLeft(ref matrix, x, y, speed);
        CheckTopRight(ref matrix, x, y, speed);
        CheckDownLeft(ref matrix, x, y, speed);
        CheckDownRight(ref matrix, x, y, speed);
    }

    /// <summary>
    /// Checks `Knight` (♞) like movement.
    /// </summary>
    public static void CheckKnight(ref bool[,] matrix, int x, int y, int speed)
    {
        // UpLeft
        CheckPosition(x - 1, y + 2, ref matrix);

        // UpRight
        CheckPosition(x + 1, y + 2, ref matrix);

        // RightUp
        CheckPosition(x + 2, y + 1, ref matrix);

        // RightDown
        CheckPosition(x + 2, y - 1, ref matrix);

        // DownLeft
        CheckPosition(x - 1, y - 2, ref matrix);

        // DownRight
        CheckPosition(x + 1, y - 2, ref matrix);

        // LeftUp
        CheckPosition(x - 2, y + 1, ref matrix);

        // LeftDown
        CheckPosition(x - 2, y - 1, ref matrix);
    }

    public static void CheckPosition(int x, int y, ref bool[,] r)
    {
        if (x >= 0 && x < GRID_SIZE && y >= 0 && y < GRID_SIZE)
        {
            var piece = BoardManager.Instance.Pieces[x, y];
            if (piece == null)
            {
                r[x, y] = true;
            }
        }
    }

    public static void CheckRight(ref bool[,] matrix, int x, int y, int speed)
    {
        int index = x;
        int maxI = x + speed;

        while (true)
        {
            index++;
            if (index > maxI || index >= GRID_SIZE)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[index, y];
            if (piece == null)
            {
                matrix[index, y] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckLeft(ref bool[,] matrix, int x, int y, int speed)
    {
        int index = x;
        int maxI = x - speed;

        while (true)
        {
            index--;
            if (index < maxI || index < 0)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[index, y];
            if (piece == null)
            {
                matrix[index, y] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckUp(ref bool[,] matrix, int x, int y, int speed)
    {
        int index = y;
        int maxI = y + speed;

        while (true)
        {
            index++;
            if (index > maxI || index >= GRID_SIZE)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[x, index];
            if (piece == null)
            {
                matrix[x, index] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckDown(ref bool[,] matrix, int x, int y, int speed)
    {
        int index = y;
        int maxI = y - speed;

        while (true)
        {
            index--;
            if (index < maxI || index < 0)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[x, index];
            if (piece == null)
            {
                matrix[x, index] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckTopLeft(ref bool[,] matrix, int x, int y, int speed)
    {
        int i = x;
        int j = y;
        int maxI = x - speed;

        while (true)
        {
            i--;
            j++;

            if (i < maxI || i < 0 || j >= GRID_SIZE)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[i, j];
            if (piece == null)
            {
                matrix[i, j] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckTopRight(ref bool[,] matrix, int x, int y, int speed)
    {
        int i = x;
        int j = y;
        int maxI = x + speed;

        while (true)
        {
            i++;
            j++;
            if (i > maxI || i >= GRID_SIZE || j >= GRID_SIZE)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[i, j];
            if (piece == null)
            {
                matrix[i, j] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckDownLeft(ref bool[,] matrix, int x, int y, int speed)
    {
        int i = x;
        int j = y;
        int maxI = x - speed;

        while (true)
        {
            i--;
            j--;
            if (i < maxI || i < 0 || j < 0)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[i, j];
            if (piece == null)
            {
                matrix[i, j] = true;
            }
            else
            {
                break;
            }
        }
    }

    public static void CheckDownRight(ref bool[,] matrix, int x, int y, int speed)
    {
        int i = x;
        int j = y;
        int maxI = x + speed;

        while (true)
        {
            i++;
            j--;
            if (i > maxI || i >= GRID_SIZE || j < 0)
            {
                break;
            }

            var piece = BoardManager.Instance.Pieces[i, j];
            if (piece == null)
            {
                matrix[i, j] = true;
            }
            else
            {
                break;
            }
        }
    }
}