using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace Advent.Lib;

public static class Analyzer
{
    public static bool TestMode = false;
    public static int Repeat = 0;
    
    public static Analysis Analyze(Func<string[], string> func, string[] input)
    {
        long total = 0;
        var output = "";
        for (int i = Repeat + 1; i > 0; i--)
        {
            long start = Stopwatch.GetTimestamp();
            output = func(input);
            total += Stopwatch.GetTimestamp() - start;
        }
        
        var methodInfo = func.GetMethodInfo();
        var test = methodInfo.GetCustomAttributes(false).OfType<Test>().FirstOrDefault();
        var passed = false;

        if (test != null)
        {
            passed = test.TestPassed(output);
        }

        var ts = TimeSpan.FromTicks(total);
        var avg = TimeSpan.FromTicks(total / (Repeat + 1));
        return new Analysis(methodInfo.Name, ts, avg, output, passed);
    }

    public static void HandleErrors(Exception e, ISolution solution)
    {
        if (e.GetType() == typeof(NotImplementedException))
        {
            Console.Error.WriteLine($"Solution not yet implemented.".FgColor(Color.PaleVioletRed));
        }
        else
        {
            var trace = new StackTrace(e, true);
            var frame = trace.GetFrame(0);
                
            int line = -1;
            if (frame != null)
            {
                line = frame.GetFileLineNumber();
            }

            Console.Error.WriteLine($"\nError while processing Day {solution.Day()} {(line != -1 ? $"at line {line}" : "")}:"
                .BgColor(Color.PaleVioletRed)
                .FgColor(Color.Black) + "\n");
                
            Console.Error.WriteLine(e.ToString().FgColor(Color.PaleVioletRed));
            Console.WriteLine();
        }
    }
}

public class Analysis(string name, TimeSpan elapsed, TimeSpan average, string output, bool testsPassed)
{
    public string Name => Utils.CamelCase().Replace(name, " $1");
    public TimeSpan Elapsed => elapsed;
    public TimeSpan Average => average;
    public string Output => output;
    public bool TestsPassed => testsPassed;

    public override string ToString()
    {
        string tempOutput = Output.PadRight(30 - Name.Length);
        if (TestsPassed)
        {
            tempOutput = tempOutput.FgRainbow();
        }
        
        return $"{Name}: {tempOutput} → {(Analyzer.Repeat > 0 ? $"{Utils.GetColoredTimeSpan(Average)}(avg)" : Utils.GetColoredTimeSpan(Elapsed))}";
    }
}

[AttributeUsage(AttributeTargets.Method)]
public class Test(string example, string correct = "") : Attribute
{
    public string Example { get; set; } = example;
    public string Correct { get; set; } = correct;

    public bool TestPassed(string input)
    {
        return Analyzer.TestMode ? input == Example : input == Correct;
    }
}