using Advent.Lib;

namespace Advent.Solutions._2025._5;

public class Cafeteria : ISolution
{
    private class Range(long min, long max)
    {
        public long Min = min;
        public long Max = max;

        public bool WithinRange(long value)
        {
            return value >= Min && value <= Max;
        }
    }
    
    public string PartOne(string[] input)
    {
        List<Range> ranges = [];
        var i = 0;
        for (;input[i].Contains('-'); i++)
        {
            string[] range = input[i].Split('-');
            long min = long.Parse(range[0]);
            long max = long.Parse(range[1]);
            ranges.Add(new Range(min, max));
        }
        
        long total = 0;
        for (;i < input.Length; i++)
        {
            long num = long.Parse(input[i]);
            if (ranges.Any(x => x.WithinRange(num))) total++;
        }

        return total.ToString();
    }

    public string PartTwo(string[] input)
    {
        List<Range> ranges = [];
        var i = 0;
        for (;input[i].Contains('-'); i++)
        {
            string[] range = input[i].Split('-');
            long min = long.Parse(range[0]);
            long max = long.Parse(range[1]);
            ranges.Add(new Range(min, max));
        }

        ranges = ranges.OrderBy(x => x.Min).ToList();
        List<Range> merged = [ranges[0]];

        for (var j = 1; j < ranges.Count; j++)
        {
            var current = ranges[j];
            var last = merged[^1];

            if (current.Min <= last.Max)
            {
                last.Max = Math.Max(last.Max, current.Max);
            }
            else
            {
                merged.Add(current);
            }
        }

        long unique = merged.Sum(range => (range.Max - range.Min + 1));
        return unique.ToString();
    }

    public string TestInput() => """
                                 3-5
                                 10-14
                                 16-20
                                 12-18

                                 1
                                 5
                                 8
                                 11
                                 17
                                 32
                                 """;
}