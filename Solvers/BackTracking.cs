using System;
using System.Collections.Generic;
using System.Threading;

namespace KillerSudoku;

public class BackTracking : ISolver
{
    int[,] board = new int[9, 9];
    List<Cage> cages; 
    List<IConstraint> constraints;

    public BackTracking(List<Cage> cages)
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

    bool IsValid(int row, int col, int num)
    {
        foreach (var constraint in constraints)
            if (!constraint.IsValid(board, row, col, num))
                return false;
        return true;
    }

    public bool Solve()
    {
        return SolveIternal(0, 0);
    }

    public bool SolveIternal(int row = 0, int col = 0)
    {
        if (row == 9) return true;
        if (col == 9) return SolveIternal(row + 1, 0);

        if (board[row, col] != 0)
            return SolveIternal(row, col + 1);

        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(row, col, num))
            {
                board[row, col] = num;
                Printer.Print(board, cages);
                if (SolveIternal(row, col + 1)) return true;
                board[row, col] = 0;
            }
        }
        return false;
    }
}