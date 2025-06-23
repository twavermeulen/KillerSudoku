namespace KillerSudoku;

public class Cage
{
    public List<(int r, int c)> variables;
    public int sum;

    public Cage(List<(int r, int c)> variables, int sum)
    {
        this.variables = variables;
        this.sum = sum;
    }
}