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
    public static void Solve(this ISolution solution, string[] input)
    {
        long start = Stopwatch.GetTimestamp();
        string partOne = solution.PartOne(input);
        var elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part One: {partOne} → {Utils.GetReadableTimeSpan(elapsed).FgColor(Color.PaleGreen)}");
        
        start = Stopwatch.GetTimestamp();
        string partTwo = solution.PartTwo(input);
        elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part Two: {partTwo} → {Utils.GetReadableTimeSpan(elapsed).FgColor(Color.PaleGreen)}");
        Console.WriteLine();
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