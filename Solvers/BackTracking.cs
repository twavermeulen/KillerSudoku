using System;
using System.Collections.Generic;
using System.Linq;

namespace KillerSudoku;

public class Backtracking : ISolver
{
    int[,] board = new int[9, 9];
    List<Cage> cages; 
    List<IConstraint> constraints;

    public Backtracking(List<Cage> cages)
    {
        this.cages = cages;
        constraints = new List<IConstraint>
        {
            new RowConstraint(),
            new ColumnConstraint(),
            new BoxConstraint(),
            new CageConstraint(cages)
        };
    }

    public bool Solve()
    {
        return Backtrack();
    }

    private bool Backtrack()
    {
        (int row, int col)? unassigned = FindUnassigned();
        if (unassigned == null) return true;

        int row = unassigned.Value.row, col = unassigned.Value.col;
        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(row, col, num))
            {
                board[row, col] = num;
                if (AllConstraintsSatisfied(row, col))
                {
                    if (Backtrack()) return true;
                }
                board[row, col] = 0;
            }
        }
        return false;
    }

    private (int, int)? FindUnassigned()
    {
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
                if (board[r, c] == 0)
                    return (r, c);
        return null;
    }

    private bool IsValid(int row, int col, int num)
    {
        foreach (var constraint in constraints)
            if (!constraint.IsValid(board, row, col, num))
                return false;
        return true;
    }

    private bool AllConstraintsSatisfied(int row, int col)
    {
        // Use the cages field directly
        foreach (var cage in cages)
        {
            if (cage.Cells.Contains((row, col)))
            {
                bool allAssigned = cage.Cells.All(cell => board[cell.Item1, cell.Item2] != 0);
                if (allAssigned)
                {
                    int sum = 0;
                    HashSet<int> seen = new();
                    foreach (var cell in cage.Cells)
                    {
                        int r = cell.Item1;
                        int c = cell.Item2;
                        int val = board[r, c];
                        if (seen.Contains(val)) return false;
                        seen.Add(val);
                        sum += val;
                    }
                    if (sum != cage.Sum) return false;
                }
            }
        }
        return true;
    }

    public void PrintBoard()
    {
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
                Console.Write(board[r, c] + " ");
            Console.WriteLine();
        }
    }
}