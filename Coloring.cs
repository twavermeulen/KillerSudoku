// Add this class to a new file, e.g., KillerSudoku/CageColoring.cs
using System.Collections.Generic;
using System.Linq;

namespace KillerSudoku;

public static class Coloring
{
    public static Dictionary<Cage, int> ColorCages(List<Cage> cages, int colorCount)
    {
        var adjacency = new Dictionary<Cage, HashSet<Cage>>();
        foreach (var cage in cages)
            adjacency[cage] = new HashSet<Cage>();

        var cellToCage = new Dictionary<(int,int), Cage>();
        foreach (var cage in cages)
        foreach (var cell in cage.Cells)
            cellToCage[cell] = cage;

        foreach (var cage in cages)
        {
            foreach (var (r, c) in cage.Cells)
            {
                var neighbors = new (int,int)[] { (r-1,c), (r+1,c), (r,c-1), (r,c+1) };
                foreach (var n in neighbors)
                {
                    if (cellToCage.TryGetValue(n, out var neighborCage) && neighborCage != cage)
                        adjacency[cage].Add(neighborCage);
                }
            }
        }
        
        var coloring = new Dictionary<Cage, int>();
        bool Color(int idx)
        {
            if (idx == cages.Count) return true;
            var cage = cages[idx];
            var used = adjacency[cage].Where(coloring.ContainsKey).Select(c => coloring[c]).ToHashSet();
            for (int color = 0; color < colorCount; color++)
            {
                if (!used.Contains(color))
                {
                    coloring[cage] = color;
                    if (Color(idx + 1)) return true;
                    coloring.Remove(cage);
                }
            }
            return false;
        }
        Color(0);
        return coloring;
    }
}