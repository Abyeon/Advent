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
        public int Row { get; set; } = row;
        public int Column { get; set; } = col;
        public List<Node> Children { get; set; } = [];
        public long Total { get; set; } = 1;
    }

    [Test("40", "95408386769474")]
    public string PartTwo(string[] input)
    {
        int start = input[0].IndexOf('S');
        var root = new Node(0, start);
        
        Dictionary<(int row, int col), Node> nodeCache = [];
        long total = BuildTree(root);
        return total.ToString();
        
        long BuildTree(Node node)
        {
            long left = BuildChild(node, node.Column - 1);
            long right = BuildChild(node, node.Column + 1);
            return left + right;
        }

        long BuildChild(Node node, int col)
        {
            Node? child = null;

            long sum = 0;
            for (int i = node.Row + 2; i < input.Length; i += 2)
            {
                if (input[i][col] != '^') continue;

                var coords = (i, col);

                if (nodeCache.TryGetValue(coords, out var value))
                {
                    child = value;
                }
                else
                {
                    child = new Node(i, col);
                    nodeCache.Add(coords, child);
                    
                    child.Total = BuildTree(child);
                }
                
                sum += child.Total;
                
                node.Children.Add(child);
                break;
            }

            if (child == null)
            {
                node.Children.Add(new Node(input.Length, col));
                return 1;
            }

            return sum;
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