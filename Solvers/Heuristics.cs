using System;
using System.Collections.Generic;

namespace KillerSudoku;

public class Heuristics : ISolver
{
    int[,] board = new int[9, 9];
    List<Cage> cages; 
    List<IConstraint> constraints;

    public Heuristics(List<Cage> cages)
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
        return SolveIternal();
    }

    public bool SolveIternal()
    {
        var cell = GetMRVCell();
        if (cell == null) return true; // puzzle solved

        int row = cell.Value.row;
        int col = cell.Value.col;

        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(row, col, num))
            {
                board[row, col] = num;
                if (SolveIternal()) return true;
                board[row, col] = 0;
            }
        }
        return false;
    }

    
    
    (int row, int col)? GetMRVCell()
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