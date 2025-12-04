using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Advent.Lib;

public static partial class Utils
{
    public static string GetReadableTimeSpan(TimeSpan ts)
    {
        if (ts.TotalSeconds >= 1)
        {
            return $"{ts.TotalSeconds} seconds";
        }
        
        if (ts.TotalMilliseconds >= 1)
        {
            return $"{ts.TotalMilliseconds} ms";
        }
        
        return ts.TotalMicroseconds + " µs";
    }
    
    [GeneratedRegex("(\\B[A-Z])")]
    public static partial Regex CamelCase();

    public static string FgColor(this string input, Color color)
    {
        return $"\e[38;2;{color.R};{color.G};{color.B}m{input}\e[0m";
    }
    
    public static string BgColor(this string input, Color color)
    {
        return $"\e[48;2;{color.R};{color.G};{color.B}m{input}\e[0m";
    }
}