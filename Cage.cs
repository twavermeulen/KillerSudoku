namespace KillerSudoku;

public class Cage
{
    public List<(int r, int c)> Cells;
    public int Sum;

    public Cage(List<(int r, int c)> cells, int sum)
    {
        Cells = cells;
        Sum = sum;
    }
}