namespace KillerSudoku;
using System.Collections.Generic;

public class CageConstraint : IConstraint
{
    private readonly List<Cage> cages;
    private Dictionary<(int,int), Cage> coordinatesByCage = new Dictionary<(int, int), Cage>();

    public CageConstraint(List<Cage> cages)
    {
        this.cages = cages;
        foreach (var cage in cages)
        {
            foreach (var cell in cage.Cells)
            {
                coordinatesByCage.Add(cell, cage);
            }
        }
    }

    public bool IsValid(int[,] board, int row, int col, int num)
    {
        Cage cage = coordinatesByCage[(row, col)];

        int sum = 0;
        HashSet<int> seen = new();
        foreach (var (r, c) in cage.Cells)
        {
            int val = board[r, c];
            if ((r, c) == (row, col)) val = num;
            if (val != 0)
            {
                if (seen.Contains(val)) return false;
                seen.Add(val);
                sum += val;
            }
        }

        if (sum > cage.Sum) return false;
        if (seen.Count == cage.Cells.Count && sum != cage.Sum) return false;

        return true;
    }
}