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

    bool IsValidBoard()
    {
        for (int row = 0; row < 9; row++)
        for (int col = 0; col < 9; col++)
        {
            int num = board[row, col];
            if (num < 1 || num > 9) return false;
            foreach (var constraint in constraints)
                if (!constraint.IsValid(board, row, col, num))
                    return false;
        }
        return true;
    }

    public bool Solve()
    {
        return GenerateAndTest(0);
    }
    
    bool GenerateAndTest(int idx)
    {
        if (idx == 81)
            return IsValidBoard();

        int row = idx / 9;
        int col = idx % 9;

        for (int num = 1; num <= 9; num++)
        {
            board[row, col] = num;
            Printer.Print(board, cages);
            if (GenerateAndTest(idx + 1))
                return true;
        }
        board[row, col] = 0;
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