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

    private static bool AllEqual<T>(T[] array)
    {
        if (array is not { Length: > 1 })
        {
            return true;
        }

        var firstElement = array[0];
        return array.All(element => EqualityComparer<T>.Default.Equals(element, firstElement));
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
                    var split = current.SplitInParts(j);
                    if (!AllEqual(split.ToArray())) continue;
                    
                    total += long.Parse(current);
                    break;
                }
            }
        }
        
        Console.WriteLine($"Total is: {total}");
    }
}