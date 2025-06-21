namespace KillerSudoku;

public class Solver
{
    int[,] board = new int[9, 9];  // no initial numbers for killer sudoku
    List<Cage> cages;

    public Solver(List<Cage> cages)
    {
        this.cages = cages;
        // board initialized with zeros (empty)
    }

    bool IsValid(int row, int col, int num)
    {
        // Check row and column
        for (int i = 0; i < 9; i++)
        {
            if (board[row, i] == num) return false;
            if (board[i, col] == num) return false;
        }

        // Check 3x3 box
        int boxRowStart = 3 * (row / 3);
        int boxColStart = 3 * (col / 3);
        for (int r = boxRowStart; r < boxRowStart + 3; r++)
            for (int c = boxColStart; c < boxColStart + 3; c++)
                if (board[r, c] == num) return false;

        return true;
    }

    bool IsValidCage(int row, int col, int num)
    {
        foreach (var cage in cages)
        {
            if (cage.Cells.Contains((row, col)))
            {
                int sumSoFar = 0;
                HashSet<int> usedNums = new HashSet<int>();

                foreach (var (r, c) in cage.Cells)
                {
                    int val = board[r, c];
                    if (r == row && c == col) val = num;

                    if (val != 0)
                    {
                        if (usedNums.Contains(val))
                            return false; // duplicate in cage
                        usedNums.Add(val);
                        sumSoFar += val;
                    }
                }

                if (sumSoFar > cage.Sum) return false; // sum exceeded
                if (usedNums.Count == cage.Cells.Count && sumSoFar != cage.Sum)
                    return false; // cage full but sum mismatch
            }
        }
        return true;
    }

    // Get valid numbers for a cell based on constraints
    List<int> GetValidNumbers(int row, int col)
    {
        var validNums = new List<int>();
        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(row, col, num) && IsValidCage(row, col, num))
                validNums.Add(num);
        }
        return validNums;
    }

    // Find next cell with minimum remaining values (MRV heuristic)
    (bool found, int row, int col, List<int> candidates) FindNextCellMRV()
    {
        int minCount = 10; // more than max domain size 9
        int bestRow = -1, bestCol = -1;
        List<int> bestCandidates = null;

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (board[r, c] == 0)
                {
                    var candidates = GetValidNumbers(r, c);
                    if (candidates.Count < minCount)
                    {
                        minCount = candidates.Count;
                        bestRow = r;
                        bestCol = c;
                        bestCandidates = candidates;

                        if (minCount == 1) // can’t do better than 1
                            return (true, bestRow, bestCol, bestCandidates);
                    }
                }
            }
        }

        return (bestRow != -1, bestRow, bestCol, bestCandidates);
    }

    bool Solve()
    {
        var (found, row, col, candidates) = FindNextCellMRV();
        if (!found)
            return true; // solved (no empty cells)

        foreach (int num in candidates)
        {
            board[row, col] = num;
            if (Solve())
                return true;
            board[row, col] = 0;
        }
        return false;
    }

    public void PrintBoard()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
                Console.Write(board[r, c] + " ");
            Console.WriteLine();
        }
    }

    public bool StartSolving()
    {
        return Solve();
    }
}