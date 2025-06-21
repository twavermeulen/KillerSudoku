using System;
using System.Collections.Generic;

namespace KillerSudoku;

public class BruteForce : ISolver
{
    int[,] board = new int[9, 9];
    List<Cage> cages; 
    List<IConstraint> constraints;

    public BruteForce(List<Cage> cages)
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

    public bool Solve(int row = 0, int col = 0)
    {
        if (row == 9) return true;
        if (col == 9) return Solve(row + 1, 0);
        if (board[row, col] != 0) return Solve(row, col + 1);

        for (int num = 1; num <= 9; num++)
        {
            if (IsValid(row, col, num))
            {
                board[row, col] = num;
                if (Solve(row, col + 1)) return true;
                board[row, col] = 0;
            }
        }
        return false;
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