namespace KillerSudoku;

public class ColumnConstraint : IConstraint
{
    public bool IsValid(int[,] board, int row, int col, int num)
    {
        for (int r = 0; r < 9; r++)
            if (board[r, col] == num) return false;
        return true;
    }
}