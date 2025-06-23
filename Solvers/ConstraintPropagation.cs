using System;
using System.Collections.Generic;

namespace KillerSudoku;

public class ConstraintPropagation : ISolver
{
    int[,] board = new int[9, 9];
    List<Cage> cages;
    List<IConstraint> constraints;
    Dictionary<(int, int), HashSet<int>> variables;

    public ConstraintPropagation(List<Cage> cages)
    {
        this.cages = cages;
        constraints = new List<IConstraint>
        {
            new RowConstraint(),
            new ColumnConstraint(),
            new BoxConstraint(),
            new CageConstraint(cages)
        };
        variables = new Dictionary<(int, int), HashSet<int>>();
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                variables[(r, c)] = new HashSet<int>(Enumerable.Range(1, 9));
    }
    
    

    bool IsValid(int row, int col, int domain)
    {
        foreach (var constraint in constraints)
            if (!constraint.IsValid(board, row, col, domain))
                return false;
        return true;
    }

    public bool Solve()
    {
        var variable = GetMRVVariable();
        if (variable == null) return true;

        int row = variable.Value.row;
        int col = variable.Value.col;

        var possible = new List<int>(variables[(row, col)]);
        foreach (int domain in possible)
        {
            if (IsValid(row, col, domain))
            {
                var backup = CopyVariables();
                board[row, col] = domain;
                variables[(row, col)] = new HashSet<int> { domain };
                if (Propagate(row, col))
                {
                    if (Solve()) return true;
                }
                board[row, col] = 0;
                variables = backup;
            }
        }
        return false;
    }

   

    (int row, int col)? GetMRVVariable()
    {
        int minOptions = 10;
        int minRow = -1, minCol = -1;
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                if (board[r, c] != 0) continue;
                int options = variables[(r, c)].Count;
                if (options < minOptions)
                {
                    minOptions = options;
                    minRow = r;
                    minCol = c;
                }
            }
        }
        return minRow == -1 ? null : (minRow, minCol);
    }

    bool Propagate(int row, int col)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue((row, col));
        while (queue.Count > 0)
        {
            var (r, c) = queue.Dequeue();
            foreach (var neighbor in GetNeighbors(r, c))
            {
                if (board[neighbor.Item1, neighbor.Item2] != 0) continue;
                var toRemove = new List<int>();
                foreach (var val in variables[neighbor])
                {
                    if (!IsValid(neighbor.Item1, neighbor.Item2, val))
                        toRemove.Add(val);
                }
                if (toRemove.Count > 0)
                {
                    foreach (var val in toRemove)
                        variables[neighbor].Remove(val);
                    if (variables[neighbor].Count == 0)
                        return false;
                    queue.Enqueue(neighbor);
                }
            }
        }
        return true;
    }

    IEnumerable<(int, int)> GetNeighbors(int row, int col)
    {
        var neighbors = new HashSet<(int, int)>();
        for (int i = 0; i < 9; i++)
        {
            if (i != col) neighbors.Add((row, i));
            if (i != row) neighbors.Add((i, col));
        }
        int boxRow = row / 3 * 3, boxCol = col / 3 * 3;
        for (int r = boxRow; r < boxRow + 3; r++)
            for (int c = boxCol; c < boxCol + 3; c++)
                if ((r, c) != (row, col)) neighbors.Add((r, c));
        foreach (var cage in cages)
            if (cage.variables.Contains((row, col)))
                foreach (var variable in cage.variables)
                    if (variable != (row, col)) neighbors.Add(variable);
        return neighbors;
    }

    Dictionary<(int, int), HashSet<int>> CopyVariables()
    {
        var copy = new Dictionary<(int, int), HashSet<int>>();
        foreach (var kv in variables)
            copy[kv.Key] = new HashSet<int>(kv.Value);
        return copy;
    }
}