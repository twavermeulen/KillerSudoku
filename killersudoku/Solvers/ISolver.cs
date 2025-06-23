namespace KillerSudoku;

public interface ISolver
{
    public List<int[,]> GetHistory();
    public int[,] GetSolvedBoard();
    bool Solve();

}