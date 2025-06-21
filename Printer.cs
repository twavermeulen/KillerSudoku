namespace KillerSudoku;

using System;
using System.Threading;

public static class Printer
{
    public static void Print(int[,] board)
    {
        Console.Clear();
        for (int r = 0; r < 9; r++)
        {
            for (int c = 0; c < 9; c++)
                Console.Write(board[r, c] + " ");
            Console.WriteLine();
        }
        Thread.Sleep(50);
    }
}