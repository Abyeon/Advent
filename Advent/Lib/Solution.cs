using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Advent.Lib;

public interface ISolution
{
    string PartOne(string[] input);
    string PartTwo(string[] input);
    string TestInput();
}

public static class SolutionExtensions
{
    public static long Solve(this ISolution solution, string[] input)
    {
        long start = Stopwatch.GetTimestamp();
        string partOne = solution.PartOne(input);
        var elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part One: {partOne} → {Utils.GetReadableTimeSpan(elapsed).FgColor(Color.PaleGreen)}");
        
        start = Stopwatch.GetTimestamp();
        string partTwo = solution.PartTwo(input);
        var elapsed2 = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part Two: {partTwo} → {Utils.GetReadableTimeSpan(elapsed2).FgColor(Color.PaleGreen)}");
        
        return (elapsed + elapsed2).Ticks;
    }

    public static int Year(this ISolution solution)
    {
        string? fullName = solution.GetType().FullName;
        if (fullName is null) return -1;
        
        return int.Parse(fullName.Split('.')[2][1..]);
    }

    public static int Day(this ISolution solution)
    {
        string? fullName = solution.GetType().FullName;
        if (fullName is null) return -1;
        
        return int.Parse(fullName.Split('.')[3][1..]);
    }

    public static string Name(this ISolution solution)
    {
        return Utils.CamelCase().Replace(solution.GetType().Name, " $1");
    }
}