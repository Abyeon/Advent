using Advent.Lib;

namespace Advent.Solutions._2025._7;

public class Laboratories : ISolution
{
    [Test("21", "1698")]
    public string PartOne(string[] input)
    {
        long total = 0;
        int start = input[0].IndexOf('S');
        
        HashSet<int> beams = [start];
        for (var i = 2; i < input.Length; i += 2)
        {
            char[] line = input[i].ToCharArray();
            
            foreach (int col in new HashSet<int>(beams))
            {
                if (line[col] != '^') continue;
                
                total++;
                beams.Remove(col);
                beams.Add(col - 1);
                beams.Add(col + 1);
            }
        }
        
        return total.ToString();
    }

    [Test("40", "95408386769474")]
    public string PartTwo(string[] input)
    {
        int start = input[0].IndexOf('S');
        
        var nodeCache = new long[input.Length, input[0].Length];
        long total = BuildTree(0, start);
        return total.ToString();
        
        long BuildTree(int row, int col)
        {
            long left = BuildChild(row, col - 1);
            long right = BuildChild(row, col + 1);
            return left + right;
        }

        long BuildChild(int row, int col)
        {
            long sum = 0;
            for (int i = row + 2; i < input.Length; i += 2)
            {
                if (input[i][col] != '^') continue;
                
                long cached = nodeCache[row, col];
                if (cached > 0)
                {
                    sum = cached;
                }
                else
                {
                    sum = BuildTree(i, col);
                    nodeCache[row, col] = sum;
                }
                break;
            }

            return sum != 0 ? sum : 1;
        }
    }

    public string TestInput() => """
                                 .......S.......
                                 ...............
                                 .......^.......
                                 ...............
                                 ......^.^......
                                 ...............
                                 .....^.^.^.....
                                 ...............
                                 ....^.^...^....
                                 ...............
                                 ...^.^...^.^...
                                 ...............
                                 ..^...^.....^..
                                 ...............
                                 .^.^.^.^.^...^.
                                 ...............
                                 """;
}