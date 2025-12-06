using Advent.Lib;

namespace Advent.Solutions._2025._1;

public class SecretEntrance : ISolution
{
    [Test("3", "1139")]
    public string PartOne(string[] input)
    {
        var totalZero = 0;
        var pos = 50;
        foreach (string line in input)
        {
            var sign = 1;
            if (line[..1].Equals("L")) sign = -1;
        
            int amount = int.Parse(line[1..]);
            pos = (pos + (sign * amount)) % 100;
            if (pos < 0) pos += 100;
        
            if (pos == 0) totalZero++;
        }
    
        return totalZero.ToString();
    }

    [Test("6", "6684")]
    public string PartTwo(string[] input)
    {
        var totalZero = 0;
        var pos = 50;
        foreach (string line in input)
        {
            var sign = 1;
            if (line[..1].Equals("L")) sign = -1;
            
            int amount = int.Parse(line[1..]);
            for (var i = 0; i < amount; i++)
            {
                pos = (pos + sign) % 100;
                if (pos < 0) pos += 100;
                if (pos == 0) totalZero++;
            }
        }
    
        return totalZero.ToString();
    }
    
    public string TestInput() => """
                                 L68
                                 L30
                                 R48
                                 L5
                                 R60
                                 L55
                                 L1
                                 L99
                                 R14
                                 L82
                                 """;
}