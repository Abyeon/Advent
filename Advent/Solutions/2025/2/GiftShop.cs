using Advent.Lib;

namespace Advent.Solutions._2025._2;

public class GiftShop : ISolution
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
                if ((current + current)[1..^1].Contains(current))
                {
                    total += long.Parse(current);
                }
            }
        }
        
        return total.ToString();
    }
    
    public string TestInput() => "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
}