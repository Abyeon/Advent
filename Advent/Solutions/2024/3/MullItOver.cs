using System.Text.RegularExpressions;
using Advent.Lib;

namespace Advent.Solutions._2024._3;

public partial class MullItOver : ISolution
{
    [Test("161", "174561379")]
    public string PartOne(string[] input)
    {
        var ins  = MultiplyInstructions();

        var total = 0;

        for (var i = 0; i < input.Length; i++)
        {
            string line = input[i];
            var multi = ins.Matches(line);

            foreach (Match match in multi)
            {
                int input1 = int.Parse(match.Groups[1].Value);
                int input2 = int.Parse(match.Groups[2].Value);

                total += input1 * input2;
            }
        }

        return total.ToString();
    }

    [Test("48", "106921067")]
    public string PartTwo(string[] input)
    {
        var ins  = MultiplyInstructions();
        var cond = ConditionalStatements();
        var lastConditional = Match.Empty;

        var total = 0;
        
        foreach (string line in input) {
            var multi  = ins.Matches(line);
            var conditionals = cond.Matches(line).ToArray();

            foreach (Match match in multi) {
                lastConditional = conditionals.LastOrDefault(conditional => conditional.Index <= match.Index, lastConditional);
                if (lastConditional.Value != "do()" && lastConditional != Match.Empty) continue;
                
                Console.WriteLine(match.Value + " " + lastConditional);
                
                int input1 = int.Parse(match.Groups[1].Value);
                int input2 = int.Parse(match.Groups[2].Value);

                total += input1 * input2;
            }
        }

        return total.ToString();
    }

    public string TestInputA() => """
                                 xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
                                 """;
    
    public string TestInputB() => TestInputA();
    
    [GeneratedRegex(@"mul\(([1-9][0-9][0-9]|[1-9][0-9]|[0-9]),([1-9][0-9][0-9]|[1-9][0-9]|[0-9])\)", RegexOptions.Multiline)]
    private static partial Regex MultiplyInstructions();
    
    [GeneratedRegex(@"(do\(\)|don't\(\))", RegexOptions.Multiline)]
    private static partial Regex ConditionalStatements();
}