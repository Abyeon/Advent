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

var repeatOpt = new Option<int>(name: "--repeat", ["-r"])
{
    Description = "Repeat processing each day for n times.",
    DefaultValueFactory = _ => 0
};

root.Add(yearOpt);
root.Add(dayOpt);
root.Add(allOpt);
root.Add(testOpt);
root.Add(repeatOpt);

root.SetAction(async (result) =>
{
    int year = result.GetValue(yearOpt);
    int day = result.GetValue(dayOpt);
    bool runAll = result.GetValue(allOpt);
    bool test = result.GetValue(testOpt);
    int repeat = result.GetValue(repeatOpt) + 1;
    
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

    long time = 0;
    foreach (var solution in toRun)
    {
        Console.WriteLine($"Day {solution.Day()}: {solution.Name().FgColor(Color.CornflowerBlue)}");
        
        string input;
        if (test)
        {
            input = solution.TestInput();
        }
        else
        {
            string path = Path.Combine(currentDir, $@"inputs\{solution.Year()}\");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine($"Created: {path}");
            }
    
            path = Path.Combine(path, $"{solution.Day()}.txt");
            input = await AdventOfCode.GetInput(solution.Year(), solution.Day(), cookie, path);
        }

        var originalOut = Console.Out;
        if (repeat > 1) Console.SetOut(TextWriter.Null);

        long start = time;
        for (var i = 0; i < repeat; i++)
        {
            string[] lines = input.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries);
            time += solution.Solve(lines);
        }

        var elapsed = TimeSpan.FromTicks(time - start);
        var average = elapsed / repeat;
        
        if (repeat > 1) Console.SetOut(originalOut);
        
        Console.WriteLine("Total".PadRight(30) + $" → {Utils.GetColoredTimeSpan(elapsed)}");
        if (repeat > 1) Console.WriteLine("Average".PadRight(30) + $" → {Utils.GetColoredTimeSpan(average)}");
        Console.WriteLine();
    }
    
    Console.WriteLine($"Finished processing {(toRun.Count == 1 ? "puzzle" : "puzzles")}!");
    
    if (toRun.Count > 1) Console.WriteLine($"Total processing time: {Utils.GetColoredTimeSpan(TimeSpan.FromTicks(time))}");
    Console.WriteLine();
});

Console.OutputEncoding = Encoding.UTF8;
root.Parse(args).Invoke();