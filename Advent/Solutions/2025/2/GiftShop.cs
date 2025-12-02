using Advent.Lib;

namespace Advent.Solutions._2025._2;

public class GiftShop : Solution
{
    public void PartOne(string[] input)
    {
        var lines = input[0].Split(',');
        
        long total = 0;
        foreach (var line in lines)
        {
            var min = long.Parse(line.Split('-')[0]);
            var max = long.Parse(line.Split('-')[1]);

            for (var i = min; i <= max; i++)
            {
                var current = i.ToString();

                var firstHalf = current[..(current.Length / 2)];
                var secondHalf = current[(current.Length / 2)..];
                if (firstHalf != secondHalf) continue;
                
                total += long.Parse(current);
            }
        }
        
        Console.WriteLine($"Total is: {total}");
    }

    private static List<string> SplitInParts(string s, int partLength)
    {
        var list = new List<string>(s.Length / partLength);
        for (var i = 0; i < s.Length; i += partLength)
            list.Add(s.Substring(i, Math.Min(partLength, s.Length - i)));
        
        return list;
    }
    
    public void PartTwo(string[] input)
    {
        long total = 0;
        foreach (var line in input[0].Split(','))
        {
            var pieces = line.Split('-');
            var min = long.Parse(pieces[0]);
            var max = long.Parse(pieces[1]);

            for (var i = min; i <= max; i++)
            {
                var current = i.ToString();

                for (var j = 1; j < current.Length; j++)
                {
                    if (current.Length % j != 0) continue;
                    
                    var split = SplitInParts(current, j);
                    if (split.Any(x => !x.Equals(split.First()))) continue;
                    
                    total += long.Parse(current);
                    break;
                }
            }
        }
        
        Console.WriteLine($"Total is: {total}");
    }
}