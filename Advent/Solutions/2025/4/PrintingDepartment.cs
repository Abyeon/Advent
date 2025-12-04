using Advent.Lib;

namespace Advent.Solutions._2025._4;

public class PrintingDepartment : ISolution
{
    public string PartOne(string[] input)
    {
        long total = 0;
        int height = input.Length - 1;
        
        for (var i = 0; i < input.Length; i++)
        {
            for (var j = 0; j < input[i].Length; j++)
            {
                if (input[i][j] == '.') continue;
                int width = input[i].Length - 1;
                var neighbors = 0;
                
                for (int x = Math.Max(0, i - 1); x <= Math.Min(i+1, width); x++)
                {
                    for (int y = Math.Max(0, j - 1); y <= Math.Min(j + 1, height); y++)
                    {
                        if (x == i && y == j) continue;
                        if (input[x][y] == '@') neighbors++;
                    }
                }
                
                if (neighbors < 4) total++;
            }
        }
        
        return total.ToString();
    }
    
    public string PartTwo(string[] input)
    {
        long total = 0;
        int height = input.Length - 1;

        List<(int, int)> toRemove = [];

        while (true)
        {
            var found = false;
            for (var i = 0; i < input.Length; i++)
            {
                for (var j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == '.') continue;
                    int width = input[i].Length - 1;
                    var neighbors = 0;
                
                    for (int x = Math.Max(0, i - 1); x <= Math.Min(i+1, width); x++)
                    {
                        for (int y = Math.Max(0, j - 1); y <= Math.Min(j + 1, height); y++)
                        {
                            if (x == i && y == j) continue;
                            if (input[x][y] == '@') neighbors++;
                        }
                    }

                    if (neighbors >= 4) continue;
                    found = true;
                
                    total++;
                    toRemove.Add(new ValueTuple<int, int>(i, j));
                }
            }
            
            if (!found) break;

            foreach (var coords in toRemove)
            {
                char[] line = input[coords.Item1].ToCharArray();
                line[coords.Item2] = '.';
                input[coords.Item1] = new string(line);
            }
            
            toRemove.Clear();
        }
        
        return total.ToString();
    }

    public string TestInput() => """
                                 ..@@.@@@@.
                                 @@@.@.@.@@
                                 @@@@@.@.@@
                                 @.@@@@..@.
                                 @@.@@@@.@@
                                 .@@@@@@@.@
                                 .@.@.@.@@@
                                 @.@@@.@@@@
                                 .@@@@@@@@.
                                 @.@.@@@.@.
                                 """;
}