using Advent.Lib;

namespace Advent.Solutions._2025._3;

public class Lobby : ISolution
{
    public string PartOne(string[] input)
    {
        var total = 0;
        foreach (string line in input)
        {
            char[] bank = line.ToArray();
            char max = bank.Max();
            int index = bank.IndexOf(max);

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
        
        return total.ToString();
    }

    public string PartTwo(string[] input)
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
        
        return total.ToString();
    }

    public string TestInput() => """
                                 987654321111111
                                 811111111111119
                                 234234234234278
                                 818181911112111
                                 """;
}