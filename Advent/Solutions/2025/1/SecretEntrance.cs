using Advent.Lib;

namespace Advent.Solutions._2025._1;

public class SecretEntrance : Solution
{
    const string path = "D:\\Programming\\Advent\\Advent\\Solutions\\2025\\1\\input.txt";
    
    public void PartOne(string input)
    {
        int totalZero = 0;
        int pos = 50;
        foreach (var line in File.ReadAllLines(path))
        {
            var sign = 1;
            if (line[..1].Equals("L")) sign = -1;
        
            int amount = int.Parse(line[1..]);
            pos = (pos + (sign * amount)) % 100;
            if (pos < 0) pos += 100;
        
            if (pos == 0) totalZero++;
        }
    
        Console.WriteLine($"Total Zeroes: {totalZero}");
    }

    public void PartTwo(string input)
    {
        var totalZero = 0;
        var pos = 50;
        foreach (var line in File.ReadAllLines(path))
        {
            var sign = 1;
            if (line[..1].Equals("L")) sign = -1;
        
            var amount = int.Parse(line[1..]);
            for (var i = 0; i < amount; i++)
            {
                pos = (pos + sign) % 100;
                if (pos < 0) pos += 100;
                if (pos == 0) totalZero++;
            }
        }
    
        Console.WriteLine($"Total Zeroes: {totalZero}");
    }
}