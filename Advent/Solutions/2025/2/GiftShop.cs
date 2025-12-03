using Advent.Lib;

namespace Advent.Solutions._2025._2;

public class GiftShop : Solution
{
    public string PartOne(string[] input)
    {
        string[] lines = input[0].Split(',');
        
        long total = 0;
        foreach (string line in lines)
        {
            long min = long.Parse(line.Split('-')[0]);
            long max = long.Parse(line.Split('-')[1]);

            for (long i = min; i <= max; i++)
            {
                var current = i.ToString();

                string firstHalf = current[..(current.Length / 2)];
                string secondHalf = current[(current.Length / 2)..];
                if (firstHalf != secondHalf) continue;
                
                total += long.Parse(current);
            }
        }
        
        return total.ToString();
    }

    private static List<string> SplitInParts(string s, int partLength)
    {
        var list = new List<string>(s.Length / partLength);
        for (var i = 0; i < s.Length; i += partLength)
            list.Add(s.Substring(i, Math.Min(partLength, s.Length - i)));
        
        return list;
    }
    
    public string PartTwo(string[] input)
    {
        long total = 0;
        foreach (string line in input[0].Split(','))
        {
            string[] pieces = line.Split('-');
            long min = long.Parse(pieces[0]);
            long max = long.Parse(pieces[1]);

            for (long i = min; i <= max; i++)
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
        
        return total.ToString();
    }
}