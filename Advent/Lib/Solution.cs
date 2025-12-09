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
        TimeSpan one = TimeSpan.Zero;
        TimeSpan two = TimeSpan.Zero;
        
        try
        {
            var analysis = Analyzer.Analyze(solution.PartOne, input);
            one = analysis.Elapsed;
            Console.WriteLine(analysis.ToString());
        }
        catch (Exception e)
        {
            Analyzer.HandleErrors(e, solution);
        }

        try
        {
            var analysis = Analyzer.Analyze(solution.PartTwo, input);
            two = analysis.Elapsed;
            Console.WriteLine(analysis.ToString());
        }
        catch (Exception e)
        {
            Analyzer.HandleErrors(e, solution);
        }
        
        return (one + two).Ticks;
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