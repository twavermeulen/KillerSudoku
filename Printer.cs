namespace KillerSudoku;

using System;
using System.Collections.Generic;
using System.Threading;

public static class Printer
{
    // Assigns a unique (or cycled) color to each cage, up to the number of available colors.
    public static void Print(int[,] board, List<Cage> cages)
    {
        Console.Clear();
        var cageColors = new List<ConsoleColor>
        {
            ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan,
            ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow,
            ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Blue,
            ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Red,
            ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.White,
            ConsoleColor.DarkGray, ConsoleColor.Blue, ConsoleColor.Green,
            ConsoleColor.Cyan, ConsoleColor.Red, ConsoleColor.Magenta,
            ConsoleColor.Yellow, ConsoleColor.White
        };

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                int cageIndex = cages.FindIndex(cg => cg.Cells.Contains((r, c)));
                if (cageIndex != -1)
                    Console.BackgroundColor = cageColors[cageIndex % cageColors.Count];
                else
                    Console.ResetColor();

                Console.Write(board[r, c].ToString().PadLeft(2).PadRight(3));
                Console.ResetColor();
            }
            Console.WriteLine();
        }
        Thread.Sleep(50);
    }
}