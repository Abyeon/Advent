using Advent.Lib;

namespace Advent.Solutions._2023._1;

public class Trebuchet : ISolution
{
    public string PartOne(string[] input)
    {
        long total = 0;
        foreach (var line in input)
        {
            List<char> digits = [];
            digits.AddRange(line.Where(char.IsDigit));

            char[] newString =
            [
                digits[0],
                digits[^1]
            ];
            
            total += long.Parse(new string(newString.ToArray()));
        }
        
        return total.ToString();
    }

    public string PartTwo(string[] input)
    {
        throw new NotImplementedException();
    }

    public string TestInput() => """
                                 1abc2
                                 pqr3stu8vwx
                                 a1b2c3d4e5f
                                 treb7uchet
                                 """;
}