using System.Diagnostics;
using System.Drawing;
using System.Reflection;
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
        var analysis = Analyzer.Analyze(solution.PartOne, input);
        Console.WriteLine(analysis.ToString());
        
        var analysis2 = Analyzer.Analyze(solution.PartTwo, input);
        Console.WriteLine(analysis2.ToString());
        
        return (analysis.Elapsed + analysis2.Elapsed).Ticks;
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