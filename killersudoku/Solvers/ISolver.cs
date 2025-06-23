namespace KillerSudoku;

public interface ISolver
{
    public List<int[,]> GetHistory();
    bool Solve();
}