using System.Diagnostics;

namespace Advent.Lib;

public interface Solution
{
    void PartOne(string input);
    void PartTwo(string input);
}

public static class SolutionExtensions
{
    private static string PrettifyTicks(long ticks)
    {
        var diff = ticks * 1000.0 / Stopwatch.Frequency;
        return $"{diff:F3} ms";
    }
    
    public static void Solve(this Solution solution, string input)
    {
        var watch = Stopwatch.StartNew();
        solution.PartOne(input);
        watch.Stop();
        Console.WriteLine("  Part One: " + PrettifyTicks(watch.ElapsedTicks));
        
        watch.Restart();
        solution.PartTwo(input);
        watch.Stop();
        Console.WriteLine("  Part Two: " + PrettifyTicks(watch.ElapsedTicks));
    }

    public static int Year(this Solution solution)
    {
        var fullName = solution.GetType().FullName;
        if (fullName is null) return -1;
        
        return int.Parse(fullName.Split('.')[2][1..]);
    }

    public static int Day(this Solution solution)
    {
        var fullName = solution.GetType().FullName;
        if (fullName is null) return -1;
        
        return int.Parse(fullName.Split('.')[3][1..]);
    }
}