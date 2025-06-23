using System;
using System.Collections.Generic;

namespace KillerSudoku;

public class MinimumRemainingValues : ISolver
{
    List<int[,]> history = new();
    int[,] board = new int[9, 9];
    List<IConstraint> constraints;
    List<Cage> cages;

    public MinimumRemainingValues(List<Cage> cages)
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
        var variable = GetMRVVariable();
        if (variable == null) return true;

        int row = variable.Value.row;
        int col = variable.Value.col;

        for (int domain = 1; domain <= 9; domain++)
        {
            if (IsValid(row, col, domain))
            {
                board[row, col] = domain;
                history.Add(CloneBoard());
                Printer.Print(board, cages);
                if (Solve()) return true;
                board[row, col] = 0;
                history.Add(CloneBoard());
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

                int options = 0;
                for (int n = 1; n <= 9; n++)
                    if (IsValid(r, c, n)) options++;

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

    public List<int[,]> GetHistory() => history;
    public int[,] GetSolvedBoard() => board;
    private int[,] CloneBoard()
    {
        var clone = new int[9, 9];
        Array.Copy(board, clone, board.Length);
        return clone;
    }
}