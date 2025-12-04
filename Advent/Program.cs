using System.CommandLine;
using System.Diagnostics;
using System.Reflection;
using Advent.Lib;

var solutions = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solution).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

var root = new RootCommand("A tool for Advent of Code solutions.");

var yearOpt = new Option<int>(name: "--year", ["-y"])
{
    Description = "The year that you want to run.",
    DefaultValueFactory = _ => DateTime.Today.Year
};

var dayOpt = new Option<int>(name: "--day", ["-d"])
{
    Description = "The day that you want to run.",
    DefaultValueFactory = _ => DateTime.Today.Day
};

var allOpt = new Option<bool>(name: "--all", ["-a"])
{
    Description = "Run all days in the specified year.",
    DefaultValueFactory = _ => false
};

root.Add(yearOpt);
root.Add(dayOpt);
root.Add(allOpt);

root.SetAction(async (result) =>
{
    int year = result.GetValue(yearOpt);
    int day = result.GetValue(dayOpt);
    bool runAll = result.GetValue(allOpt);
    
    List<Solution> toRun = [];
    foreach (var type in solutions)
    {
        var solution = (Solution)Activator.CreateInstance(type)!;
        if (runAll)
        {
            toRun.Add(solution);
            continue;
        }

        if (solution.Year() != year) continue;
        if (solution.Day() == -1)
        {
            toRun.Add(solution);
            continue;
        }

        if (solution.Day() == day)
        {
            toRun.Add(solution);
        }
    }
    
    string currentDir = Directory.GetCurrentDirectory();
    string cookie = await AdventOfCode.GetCookie(Path.Combine(currentDir, "cookie.txt"));

    Console.WriteLine($"Running {toRun.Count} {(toRun.Count == 1 ? "puzzle" : "puzzles")} for year: {year}, day: {(runAll ? "ALL" : day)}");

    long start = Stopwatch.GetTimestamp();
    foreach (var solution in toRun)
    {
        Console.WriteLine($"Day {solution.Day()}: {solution.Name()}");
        
        string path = Path.Combine(currentDir, $@"inputs\{solution.Year()}\");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Console.WriteLine($"Created: {path}");
        }
    
        path = Path.Combine(path, $"{solution.Day()}.txt");
        string input = await AdventOfCode.GetInput(solution.Year(), solution.Day(), cookie, path);
    
        string[] lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        solution.Solve(lines);
    }

    var elapsed = Stopwatch.GetElapsedTime(start);
    
    Console.WriteLine("Finished processing puzzles!");
    Console.WriteLine($"Total processing time: {Utils.GetReadableTimeSpan(elapsed)}\n");
});

root.Parse(args).Invoke();