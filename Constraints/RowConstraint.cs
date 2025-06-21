namespace KillerSudoku;

public class RowConstraint : IConstraint
{
    public bool IsValid(int[,] board, int row, int col, int num)
    {
        for (int c = 0; c < 9; c++)
            if (board[row, c] == num) return false;
        return true;
    }
}