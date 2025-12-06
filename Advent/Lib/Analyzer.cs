using System.Diagnostics;
using System.Drawing;
using System.Reflection;

namespace Advent.Lib;

public static class Analyzer
{
    public static bool TestMode = false;
    
    public static Analysis Analyze(Func<string[], string> func, string[] input)
    {
        long start = Stopwatch.GetTimestamp();
        string output = func(input);
        var elapsed = Stopwatch.GetElapsedTime(start);
        
        var methodInfo = func.GetMethodInfo();
        var test = methodInfo.GetCustomAttributes(false).OfType<Test>().FirstOrDefault();
        var passed = false;

        if (test != null)
        {
            passed = test.TestPassed(output);
        }
        
        return new Analysis(methodInfo.Name, elapsed, output, passed);
    }
}

public class Analysis(string name, TimeSpan elapsed, string output, bool testsPassed)
{
    public string Name => Utils.CamelCase().Replace(name, " $1");
    public TimeSpan Elapsed => elapsed;
    public string Output => output;
    public bool TestsPassed => testsPassed;

    public override string ToString()
    {
        string tempOutput = Output.PadRight(30 - Name.Length);
        if (TestsPassed)
        {
            tempOutput = tempOutput.FgRainbow();
        }
        
        return $"{Name}: {tempOutput} → {Utils.GetColoredTimeSpan(Elapsed)}";
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