namespace KillerSudoku;

public interface IConstraint
{
    bool IsValid(int[,] board, int row, int col, int domain);
}