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
            int domain = board[row, col];
            if (domain < 1 || domain > 9) return false;
            foreach (var constraint in constraints)
                if (!constraint.IsValid(board, row, col, domain))
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

        for (int domain = 1; domain <= 9; domain++)
        {
            board[row, col] = domain;
            Printer.Print(board, cages);
            if (GenerateAndTest(idx + 1))
                return true;
        }
        board[row, col] = 0;
        return false;
    }
}