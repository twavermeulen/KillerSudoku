using System.Collections.Generic;

namespace KillerSudoku;



//TODO: ZEGGEN DAT CASHEN SNELLER KAN ZIJN. WANT NU DOORZOEKT HET HELE BOARD VOOR IEDERE CAGE VOOR DE GEGEVEN CEL
public class CageConstraint : IConstraint
{
    private readonly List<Cage> cages;

    public CageConstraint(List<Cage> cages)
    {
        this.cages = cages;
    }

    public bool IsValid(int[,] board, int row, int col, int num)
    {
        foreach (var cage in cages)
        {
            if (!cage.Cells.Contains((row, col))) continue;
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
        }
        return true;
    }
}