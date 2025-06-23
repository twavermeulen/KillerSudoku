using System;
using System.Collections.Generic;
using System.Linq;

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

    public bool Solve()
    {
        return Backtrack();
    }

    bool Backtrack()
    {
        var unassigned = variables.Where(kv => board[kv.Key.Item1, kv.Key.Item2] == 0)
                                  .OrderBy(kv => kv.Value.Count)
                                  .FirstOrDefault();
        if (unassigned.Key == default && unassigned.Value == null)
            return true; // Solved

        var (row, col) = unassigned.Key;
        foreach (var value in unassigned.Value.ToList())
        {
            if (IsValid(row, col, value))
            {
                var snapshot = CloneDomains();
                board[row, col] = value;
                variables[(row, col)] = new HashSet<int> { value };

                if (AC3((row, col)))
                {
                    if (Backtrack())
                        return true;
                }

                board[row, col] = 0;
                variables = snapshot;
            }
        }
        return false;
    }

    bool AC3((int, int) pos)
    {
        var queue = new Queue<(int, int)>();
        queue.Enqueue(pos);

        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            foreach (var peer in GetPeers(cell))
            {
                if (Revise(cell, peer))
                {
                    if (variables[peer].Count == 0)
                        return false;
                    if (variables[peer].Count == 1)
                        queue.Enqueue(peer);
                }
            }
        }
        return true;
    }

    bool Revise((int, int) cell, (int, int) peer)
    {
        bool revised = false;
        if (variables[peer].Count == 1)
        {
            int val = variables[peer].First();
            if (variables[cell].Remove(val))
                revised = true;
        }
        return revised;
    }

    IEnumerable<(int, int)> GetPeers((int, int) cell)
    {
        var (row, col) = cell;
        var peers = new HashSet<(int, int)>();
        
        for (int i = 0; i < 9; i++)
        {
            if (i != col) peers.Add((row, i));
            if (i != row) peers.Add((i, col));
        }
        
        int boxRow = row / 3 * 3, boxCol = col / 3 * 3;
        for (int r = boxRow; r < boxRow + 3; r++)
            for (int c = boxCol; c < boxCol + 3; c++)
                if ((r, c) != cell) peers.Add((r, c));
  
        foreach (var cage in cages)
            if (cage.variables.Contains(cell))
                foreach (var c in cage.variables)
                    if (c != cell) peers.Add(c);

        return peers;
    }

    bool IsValid(int row, int col, int value)
    {
        foreach (var constraint in constraints)
            if (!constraint.IsValid(board, row, col, value))
                return false;
        return true;
    }

    Dictionary<(int, int), HashSet<int>> CloneDomains()
    {
        return variables.ToDictionary(
            kv => kv.Key,
            kv => new HashSet<int>(kv.Value)
        );
    }
}