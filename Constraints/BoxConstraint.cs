namespace KillerSudoku;

public class BoxConstraint : IConstraint
{
    public bool IsValid(int[,] board, int row, int col, int domain)
    {
        int boxRow = 3 * (row / 3), boxCol = 3 * (col / 3);
        for (int r = boxRow; r < boxRow + 3; r++)
        for (int c = boxCol; c < boxCol + 3; c++)
            if (board[r, c] == domain) return false;
        return true;
    }
}