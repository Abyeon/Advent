using Advent.Lib;

namespace Advent.Solutions._2025._2;

public class GiftShop : ISolution
{
    [Test("1227775554", "34826702005")]
    public string PartOne(string[] input)
    {
        string[] lines = input[0].Split(',');
        
        long total = 0;
        foreach (string line in lines)
        {
            string[] pieces = line.Split('-');
            long min = long.Parse(pieces[0]);
            long max = long.Parse(pieces[1]);

            for (long i = min; i <= max; i++)
            {
                var current = i.ToString();

                if (current.Length % 2 != 0) continue;
                string firstHalf = current[..(current.Length / 2)];
                string secondHalf = current[(current.Length / 2)..];
                if (firstHalf != secondHalf) continue;
                
                total += long.Parse(current);
            }
        }
        
        return total.ToString();
    }
    
    [Test("4174379265", "43287141963")]
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
    
    public string TestInputA() => "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
    public string TestInputB() => TestInputA();
}