using System.Drawing;
using System.Numerics;
using System.Text;
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

    public static Color HslToRgb(double h, double s, double l)
    {
        double r, g, b;

        if (s == 0)
        {
            r = g = b = 1;
        }
        else
        {
            double Hue2Rgb(double p, double q, double t)
            {
                if (t < 0) t += 1;
                if (t > 1) t -= 1;
                if (t < 1 / 6.0) return p + (q - p) * 6 * t;
                if (t < 1 / 2.0) return q;
                if (t < 2 / 3.0) return p + (q - p) * (2 / 3.0 - t) * 6;
                return p;
            }
            
            double q = l < 0.5 ? l * (1 + s) : l + s - l * s;
            double p = 2 * l - q;
            r = Hue2Rgb(p, q, h + 1 / 3.0);
            g = Hue2Rgb(p, q, h);
            b = Hue2Rgb(p, q, h - 1 / 3.0);
        }
        
        return Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
    }

    public static string FgRainbow(this string input)
    {
        var sb =  new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            Color color = HslToRgb((double)i / input.Length, 0.5, 0.7);
            sb.Append($"\e[38;2;{color.R};{color.G};{color.B}m{input[i]}");
        }

        sb.Append("\e[0m");
        
        return sb.ToString();
    }

    public static string BgRainbow(this string input, int offset = 0)
    {
        var sb =  new StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            Color color = HslToRgb((double)((i + offset) % input.Length)/ input.Length, 0.5, 0.7);
            sb.Append($"\e[48;2;{color.R};{color.G};{color.B}m{input[i]}");
        }

        sb.Append("\e[0m");
        
        return sb.ToString();
    }

    public static string FgColor(this string input, Color color)
    {
        return $"\e[38;2;{color.R};{color.G};{color.B}m{input}\e[0m";
    }
    
    public static string BgColor(this string input, Color color)
    {
        return $"\e[48;2;{color.R};{color.G};{color.B}m{input}\e[0m";
    }
}