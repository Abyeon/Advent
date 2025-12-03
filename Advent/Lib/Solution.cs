using System.Diagnostics;

namespace Advent.Lib;

public interface Solution
{
    string PartOne(string[] input);
    string PartTwo(string[] input);
}

public static class SolutionExtensions
{
    private static string PrettifyTicks(long ticks)
    {
        var diff = ticks * 1000.0 / Stopwatch.Frequency;
        return $"{diff:F3} ms";
    }
    
    public static void Solve(this Solution solution, string[] input)
    {
        var partOne = "";
        var partTwo = "";
        
        var watch = Stopwatch.StartNew();
        partOne = solution.PartOne(input);
        watch.Stop();
        Console.WriteLine("Part One: " + PrettifyTicks(watch.ElapsedTicks));
        Console.WriteLine("  Output: " + partOne);
        
        watch.Restart();
        partTwo = solution.PartTwo(input);
        watch.Stop();
        Console.WriteLine("Part Two: " + PrettifyTicks(watch.ElapsedTicks));
        Console.WriteLine("  Output: " + partTwo);
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