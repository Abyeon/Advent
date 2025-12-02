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
    
    public void PartTwo(string[] input)
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

                for (var j = 1; j < current.Length; j++)
                {
                    if (current.Length % j != 0) continue;
                    
                    var split = current.SplitInParts(j).ToList();
                    if (split.Any(x => !x.Equals(split.First()))) continue;
                    
                    total += long.Parse(current);
                    break;
                }
            }
        }
        
        Console.WriteLine($"Total is: {total}");
    }
}