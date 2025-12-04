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

    public static string GetColoredTimeSpan(TimeSpan ts)
    {
        double ms = ts.TotalMilliseconds;

        var color = ms switch
        {
            > 1000 => Color.PaleVioletRed,
            > 100 => Color.PaleGoldenrod,
            _ => Color.PaleGreen
        };

        return GetReadableTimeSpan(ts).FgColor(color);
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