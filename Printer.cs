namespace KillerSudoku;

using System;
using System.Collections.Generic;
using System.Threading;

public static class Printer
{
    static Dictionary<Cage, int> cageColoring;
    static List<ConsoleColor> cageColors = new()
    {
        ConsoleColor.DarkBlue,
        ConsoleColor.DarkGreen,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkRed
    };

    public static void InitializeColoring(List<Cage> cages)
    {
        cageColoring = Coloring.ColorCages(cages, cageColors.Count);
    }

    public static void Print(int[,] board, List<Cage> cages)
    {
        if (cageColoring == null) InitializeColoring(cages);

        Console.SetCursorPosition(0, 0); 

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                var cage = cages.Find(cg => cg.variables.Contains((r, c)));
                if (cage != null && cageColoring.TryGetValue(cage, out int colorIdx))
                {
                    var bg = cageColors[colorIdx % cageColors.Count];
                    Console.BackgroundColor = bg;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write(board[r, c].ToString().PadLeft(2).PadRight(3));
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Thread.Sleep(50);
    }
}