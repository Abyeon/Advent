using System.CommandLine;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Text;
using Advent.Lib;

var solutions = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(ISolution).IsAssignableFrom(t))
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

var testOpt = new Option<bool>(name: "--test", ["-t"])
{
    Description = "Run using provided test inputs",
    DefaultValueFactory = _ => false
};

root.Add(yearOpt);
root.Add(dayOpt);
root.Add(allOpt);
root.Add(testOpt);

root.SetAction(async (result) =>
{
    int year = result.GetValue(yearOpt);
    int day = result.GetValue(dayOpt);
    bool runAll = result.GetValue(allOpt);
    bool test = result.GetValue(testOpt);
    
    List<ISolution> toRun = [];
    foreach (var type in solutions)
    {
        var solution = (ISolution)Activator.CreateInstance(type)!;
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

    Console.WriteLine($"\nRunning {toRun.Count} {(toRun.Count == 1 ? "puzzle" : "puzzles")} for year: {year}, day: {(runAll ? "ALL" : day)}\n");

    long start = Stopwatch.GetTimestamp();
    foreach (var solution in toRun)
    {
        Console.WriteLine($"Day {solution.Day()}: {solution.Name().FgColor(Color.CornflowerBlue)}");
        
        string path = Path.Combine(currentDir, $@"inputs\{solution.Year()}\");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Console.WriteLine($"Created: {path}");
        }
    
        path = Path.Combine(path, $"{solution.Day()}.txt");
        
        // Grab the input no matter what, in case we haven't cached yet
        string input = await AdventOfCode.GetInput(solution.Year(), solution.Day(), cookie, path);
        
        // Swap the input to the test input if desired
        if (test)
        {
            input = solution.TestInput();
        }
    
        string[] lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
        solution.Solve(lines);
    }

    var elapsed = Stopwatch.GetElapsedTime(start);
    
    Console.WriteLine($"Finished processing {(toRun.Count == 1 ? "puzzle" : "puzzles")}!");
    Console.WriteLine($"Total processing time: {Utils.GetReadableTimeSpan(elapsed).FgColor(Color.PaleGreen)}\n");
});

Console.OutputEncoding = Encoding.UTF8;
root.Parse(args).Invoke();