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
}