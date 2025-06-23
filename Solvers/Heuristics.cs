using System;
using System.Collections.Generic;

namespace KillerSudoku;

public class Heuristics : ISolver
{
    int[,] board = new int[9, 9];
    List<Cage> cages; 
    List<IConstraint> constraints;
    DotTreeLogger logger;

    public Heuristics(List<Cage> cages, DotTreeLogger logger)
    {
        this.cages = cages;
        this.logger = logger;
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
        bool solved = SolveIternal();
        logger.Close();
        return solved;
    }

    public bool SolveIternal()
    {
        var variable = GetMRVVariable();
        if (variable == null)
        {
            logger.PushNode("Solved");
            logger.PopNode();
            return true;
        }

        int row = variable.Value.row;
        int col = variable.Value.col;

        for (int domain = 1; domain <= 9; domain++)
        {
            if (IsValid(row, col, domain))
            {
                string label = $"({row},{col})={domain}";
                logger.PushNode(label);

                board[row, col] = domain;
                if (SolveIternal()) return true;
                board[row, col] = 0;

                logger.PopNode();
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
}