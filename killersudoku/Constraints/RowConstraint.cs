namespace KillerSudoku;

public class RowConstraint : IConstraint
{
    public bool IsValid(int[,] board, int row, int col, int domain)
    {
        for (int c = 0; c < 9; c++)
            if (board[row, c] == domain) return false;
        return true;
    }
}