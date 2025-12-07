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
            for (var j = 0; j < line.Length; j++)
            {
                if (line[j] != '^') continue;
                if (!beams.Contains(j)) continue;
                
                total++;
                beams.Remove(j);
                beams.Add(j - 1);
                beams.Add(j + 1);
            }
        }
        
        return total.ToString();
    }

    private class Node (int row, int col)
    {
        public int Row { get; } = row;
        public int Column { get; } = col;
        public long Total { get; set; } = 1;
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