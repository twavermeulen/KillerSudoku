using System;
using System.Collections.Generic;
using System.Threading;

namespace KillerSudoku;

public class BacktrackingSearch : ISolver
{
    List<int[,]> history = new();
    public List<int[,]> GetHistory() => history;
    public int[,] GetSolvedBoard() => board;
    int[,] board = new int[9, 9];
    List<IConstraint> constraints;
    List<Cage> cages;

    public BacktrackingSearch(List<Cage> cages)
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

    bool IsValid(int row, int col, int domain)
    {
        foreach (var constraint in constraints)
            if (!constraint.IsValid(board, row, col, domain))
                return false;
        return true;
    }

    public bool Solve()
    {
        return SolveInternal(0, 0);
    }

    public bool SolveInternal(int row = 0, int col = 0)
    {
        if (row == 9) return true;
        if (col == 9) return SolveInternal(row + 1, 0);

        if (board[row, col] != 0)
            return SolveInternal(row, col + 1);

        for (int domain = 1; domain <= 9; domain++)
        {
            if (IsValid(row, col, domain))
            {
                board[row, col] = domain;
                history.Add(CloneBoard());
                // Printer.Print(board, cages);
                if (SolveInternal(row, col + 1)) return true;
                board[row, col] = 0;
                history.Add(CloneBoard());
            }
        }
        return false;
    }

   private int[,] CloneBoard()
    {
        var clone = new int[9, 9];
        Array.Copy(board, clone, board.Length);
        return clone;
    }
}