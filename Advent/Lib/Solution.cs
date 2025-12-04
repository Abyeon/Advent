using System.Diagnostics;

namespace Advent.Lib;

public interface Solution
{
    string PartOne(string[] input);
    string PartTwo(string[] input);
}

public static class SolutionExtensions
{
    // private static string PrettifyTicks(long ticks)
    // {
    //     double diff = ticks * 1000.0 / Stopwatch.Frequency;
    //     return $"{diff:F3} ms";
    // }

    private static string GetReadableTimeSpan(TimeSpan ts)
    {
        if (ts.TotalSeconds >= 1)
        {
            return $"{ts.TotalSeconds} seconds";
        }
        
        if (ts.TotalMilliseconds >= 1)
        {
            return $"{ts.TotalMilliseconds} ms";
        }
        
        return (ts.Ticks / 10) + " µs";
    }
    
    public static void Solve(this Solution solution, string[] input)
    {
        long start = Stopwatch.GetTimestamp();
        string partOne = solution.PartOne(input);
        var elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part One: {partOne} in {GetReadableTimeSpan(elapsed)}");
        
        start = Stopwatch.GetTimestamp();
        string partTwo = solution.PartTwo(input);
        elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part Two: {partTwo} in {GetReadableTimeSpan(elapsed)}");
        Console.WriteLine();
    }

    public static int Year(this Solution solution)
    {
        string? fullName = solution.GetType().FullName;
        if (fullName is null) return -1;
        
        return int.Parse(fullName.Split('.')[2][1..]);
    }

    public static int Day(this Solution solution)
    {
        string? fullName = solution.GetType().FullName;
        if (fullName is null) return -1;
        
        return int.Parse(fullName.Split('.')[3][1..]);
    }
}