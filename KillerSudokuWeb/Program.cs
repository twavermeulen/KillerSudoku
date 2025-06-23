using KillerSudokuWeb.Components;
using KillerSudoku;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    
builder.Services.AddSignalR();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/api/solve", () =>
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

    // 1. Call Coloring.ColorCages to get cage-to-color mapping
    var coloring = Coloring.ColorCages(cages, 6);

    // 2. Create a 9x9 int[,] grid and fill with color indices
    int[,] cageColorsGrid = new int[9, 9];
    for (int r = 0; r < 9; r++)
        for (int c = 0; c < 9; c++)
            cageColorsGrid[r, c] = -1; // default for empty cells (if any)

    foreach (var kvp in coloring)
    {
        var cage = kvp.Key;
        int colorIndex = kvp.Value;
        foreach (var (r, c) in cage.variables)
        {
            cageColorsGrid[r, c] = colorIndex;
        }
    }

    // 3. Solve the puzzle
    ISolver solver = new ConstraintPropagation(cages);
    var sw = Stopwatch.StartNew();
    bool solved = solver.Solve();
    sw.Stop();

    // 4. Convert solution history and cageColorsGrid to jagged arrays (for JSON)
    List<int[][]> historyJagged = solver.GetHistory()
        .Select(board => BoardToJagged(board))
        .ToList();

    int[][] cageColorsJagged = ConvertToJagged(cageColorsGrid);

    // 5. Return all data in one response object
    return new
    {
        Solved = solved,
        Type = solver.GetType().Name,
        TimeSeconds = sw.Elapsed.TotalSeconds,
        History = historyJagged,
        CageColors = cageColorsJagged,
        board = BoardToJagged(solver.GetSolvedBoard()),
    };
});

static int[][] BoardToJagged(int[,] board)
{
    int rows = board.GetLength(0);
    int cols = board.GetLength(1);
    var result = new int[rows][];
    for (int r = 0; r < rows; r++)
    {
        result[r] = new int[cols];
        for (int c = 0; c < cols; c++)
            result[r][c] = board[r, c];
    }
    return result;
}

static int[][] ConvertToJagged(int[,] arr)
{
    int rows = arr.GetLength(0);
    int cols = arr.GetLength(1);
    var jagged = new int[rows][];
    for (int i = 0; i < rows; i++)
    {
        jagged[i] = new int[cols];
        for (int j = 0; j < cols; j++)
            jagged[i][j] = arr[i, j];
    }
    return jagged;
}

app.Run();
