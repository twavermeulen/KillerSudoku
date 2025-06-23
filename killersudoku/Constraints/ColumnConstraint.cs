namespace KillerSudoku;

public class ColumnConstraint : IConstraint
{
    public bool IsValid(int[,] board, int row, int col, int domain)
    {
        for (int r = 0; r < 9; r++)
            if (board[r, col] == domain) return false;
        return true;
    }
}