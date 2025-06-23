namespace KillerSudoku;
using System.Collections.Generic;

public class CageConstraint : IConstraint
{
    private Dictionary<(int,int), Cage> coordinatesInCage = new Dictionary<(int, int), Cage>();

    public CageConstraint(List<Cage> cages)
    {
        foreach (var cage in cages)
        {
            foreach (var variable in cage.variables)
            {
                coordinatesInCage.Add(variable, cage);
            }
        }
    }

    public bool IsValid(int[,] board, int row, int col, int domain)
    {
        Cage cage = coordinatesInCage[(row, col)];

        int sum = 0;
        HashSet<int> seen = new();
        foreach (var (r, c) in cage.variables)
        {
            int val = board[r, c];
            if ((r, c) == (row, col)) val = domain;
            if (val != 0)
            {
                if (seen.Contains(val)) return false;
                seen.Add(val);
                sum += val;
            }
        }

        if (sum > cage.sum) return false;
        if (seen.Count == cage.variables.Count && sum != cage.sum) return false;

        return true;
    }
}