namespace Advent.Lib;

public static class StringExtensions
{
    public static IEnumerable<string> SplitInParts(this string s, int partLength) {
        for (var i = 0; i < s.Length; i += partLength)
            yield return s.Substring(i, Math.Min(partLength, s.Length - i));
    }
}