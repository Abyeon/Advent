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
    }

    [Test("40", "95408386769474")]
    public string PartTwo(string[] input)
    {
        int start = input[0].IndexOf('S');
        var root = new Node(0, start);
        
        Dictionary<(int row, int col), Node> nodeCache = [];
        BuildTree(root);
        
        Dictionary<(int row, int col), long> memo = new();
        long total = FindPaths(root);
        
        return total.ToString();
        
        void BuildTree(Node node)
        {
            BuildChild(node, node.Column - 1);
            BuildChild(node, node.Column + 1);
        }

        void BuildChild(Node node, int col)
        {
            Node? child = null;
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
                    
                    BuildTree(child);
                }
                
                node.Children.Add(child);
                break;
            }

            if (child == null)
            {
                node.Children.Add(new Node(input.Length, col));
            }
        }

        long FindPaths(Node node)
        {
            if (node.Children.Count == 0) return 1;
            
            var coords = (node.Row, node.Column);
            if (memo.TryGetValue(coords, out var value))
            {
                return value;
            }

            long sum = 0;
            foreach (var child in node.Children)
            {
                sum += FindPaths(child);
            }
            
            memo.Add(coords, sum);
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