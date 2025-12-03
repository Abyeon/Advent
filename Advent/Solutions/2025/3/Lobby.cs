using System.Collections.Immutable;
using System.Text;
using Advent.Lib;
using ZLinq;

namespace Advent.Solutions._2025._3;

public class Lobby : Solution
{
    public void PartOne(string[] input)
    {
        var total = 0;
        foreach (var line in input)
        {
            var bank = line.ToArray();
            var max = bank.Max();
            var index = bank.IndexOf(max);

            char secondMax;
            if (index == bank.Length - 1)
            {
                secondMax = bank[..^1].Max();
                total += int.Parse([secondMax, max]);
                continue;
            }
            
            secondMax = bank[(index + 1)..].Max();
            total += int.Parse([max, secondMax]);
        }
        
        Console.WriteLine(total);
    }

    public void PartTwo(string[] input)
    {
        ulong total = 0;
        foreach (string line in input)
        {
            var bank = line.Select((value, index) => new {Value = value, Index = index})
                .ToDictionary(x => x.Index, x => x.Value);
            
            int furthestDigit = -1;
            var output = new char[12];
            for (var i = 0; i < 12; i++)
            {
                var max = bank.ToArray()[(furthestDigit + 1)..^(11 - i)].OrderByDescending(x => x.Value).First();
                furthestDigit = max.Key;
                output[i] = max.Value;
            }
            
            total += ulong.Parse(output);
        }
        
        Console.WriteLine($"Total: {total}");
    }
}