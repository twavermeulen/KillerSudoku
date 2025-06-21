using System.Diagnostics;
using KillerSudoku;


class Program
{
    static void Main()
    {
        var cages = new List<Cage>
        {
            new Cage(new List<(int,int)> { (0,0), (0,1) }, 3),
            new Cage(new List<(int,int)> { (0,2), (0,3), (0,4) }, 15),
            new Cage(new List<(int,int)> { (0,5), (1,5), (1,4), (2,4) }, 22),
            new Cage(new List<(int,int)> { (0,6), (1,6) }, 4),
            new Cage(new List<(int,int)> { (0,7), (1,7) }, 16),
            new Cage(new List<(int,int)> { (0,8), (1,8), (2,8), (3,8) }, 15),

            new Cage(new List<(int,int)> { (1,0), (1,1), (2,0), (2,1) }, 25),
            new Cage(new List<(int,int)> { (1,2), (1,3) }, 17),

            new Cage(new List<(int,int)> { (2,2), (2,3), (3,3) }, 9),
            new Cage(new List<(int,int)> { (2,5), (3,5), (4,5) }, 8),
            new Cage(new List<(int,int)> { (2,6), (2,7), (3,6) }, 20),

            new Cage(new List<(int,int)> { (3,0), (4,0) }, 6),
            new Cage(new List<(int,int)> { (3,1), (3,2) }, 14),
            new Cage(new List<(int,int)> { (3,4), (4,4), (5,4) }, 17),
            new Cage(new List<(int,int)> { (3,7), (4,7), (4,6) }, 17),

            new Cage(new List<(int,int)> { (4,1), (4,2), (5,1) }, 13),
            new Cage(new List<(int,int)> { (4,3), (5,3), (6,3) }, 20),
            new Cage(new List<(int,int)> { (4,8), (5,8) }, 12),

            new Cage(new List<(int,int)> { (5,0), (6,0), (7,0), (8,0) }, 27),
            new Cage(new List<(int,int)> { (5,2), (6,2), (6,1) }, 6),
            new Cage(new List<(int,int)> { (5,5), (6,5), (6,6) }, 20),
            new Cage(new List<(int,int)> { (5,6), (5,7) }, 6),

            new Cage(new List<(int,int)> { (6,4), (7,4), (7,3), (8,3) }, 10),
            new Cage(new List<(int,int)> { (6,7), (6,8), (7,7), (7,8) }, 14),

            new Cage(new List<(int,int)> { (7,1), (8,1) }, 8),
            new Cage(new List<(int,int)> { (7,2), (8,2) }, 16),
            new Cage(new List<(int,int)> { (7,5), (7,6) }, 15),

            new Cage(new List<(int,int)> { (8,4), (8,5), (8,6) }, 13),
            new Cage(new List<(int,int)> { (8,7), (8,8) }, 17),
        };

        
        ISolver solver = new BackTracking(cages);

        var sw = Stopwatch.StartNew();
        bool solved = solver.Solve();
        sw.Stop();
        

        Console.WriteLine($"Time to complete: {sw.Elapsed.TotalSeconds:F3} seconds");
    }
}
