namespace KillerSudoku;

using System;
using System.Collections.Generic;
using System.Threading;

public static class Printer
{
    public static void Print(int[,] board, List<Cage> cages)
    {
        Console.SetCursorPosition(0, 0);
        var cageColors = new List<ConsoleColor>
        {
            ConsoleColor.DarkBlue, ConsoleColor.DarkGreen, ConsoleColor.DarkCyan,
            ConsoleColor.DarkRed, ConsoleColor.DarkMagenta, ConsoleColor.DarkYellow,
            ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Blue,
            ConsoleColor.Green, ConsoleColor.Cyan, ConsoleColor.Red,
            ConsoleColor.Magenta, ConsoleColor.Yellow, ConsoleColor.White
        };

        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
            {
                int cageIndex = cages.FindIndex(cg => cg.Cells.Contains((r, c)));
                if (cageIndex != -1)
                {
                    var bg = cageColors[cageIndex % cageColors.Count];
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