using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Advent.Lib;

public interface Solution
{
    string PartOne(string[] input);
    string PartTwo(string[] input);
}

public static partial class SolutionExtensions
{
    
    public static void Solve(this Solution solution, string[] input)
    {
        long start = Stopwatch.GetTimestamp();
        string partOne = solution.PartOne(input);
        var elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part One: {partOne} in {Utils.GetReadableTimeSpan(elapsed)}");
        
        start = Stopwatch.GetTimestamp();
        string partTwo = solution.PartTwo(input);
        elapsed = Stopwatch.GetElapsedTime(start);
        
        Console.WriteLine($"Part Two: {partTwo} in {Utils.GetReadableTimeSpan(elapsed)}");
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

    public static string Name(this Solution solution)
    {
        return Utils.CamelCase().Replace(solution.GetType().Name, " $1");
    }
}