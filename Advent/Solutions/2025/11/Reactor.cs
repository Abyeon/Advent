using Advent.Lib;
using NetTopologySuite.Utilities;

namespace Advent.Solutions._2025._11;

public class Reactor : ISolution
{
    [Test("5", "758")]
    public string PartOne(string[] input)
    {
        var nodeDict = new Dictionary<string, string[]>(input.Length);

        // Build node dictionary
        for (var i = 0; i < input.Length; i++)
        {
            string schematic = input[i];
            string name = schematic[..3];
            string[] children = schematic[4..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            nodeDict.Add(name, children);
        }
        
        // Start at "you"
        Queue<string> queue = [];
        queue.Enqueue("you");

        // Dequeue until we have explored all paths
        var total = 0;
        while (queue.Count > 0)
        {
            string curr = queue.Dequeue();
            string[] children = nodeDict[curr];

            foreach (string child in children)
            {
                if (child == "out")
                {
                    total++;
                    continue;
                }
                
                queue.Enqueue(child);
            }
        }

        return total.ToString();
    }

    [Test("2", "490695961032000")]
    public string PartTwo(string[] input)
    {
        int count = input.Length;
        var nodeDict = new Dictionary<string, string[]>(count);

        // Build node dictionary
        for (var i = 0; i < count; i++)
        {
            string schematic = input[i];
            string name = schematic[..3];
            string[] children = schematic[4..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            nodeDict.Add(name, children);
        }
        
        // Memoization
        Dictionary<(string, bool, bool), long> nodeMemo = [];
        long total = FindPaths("svr", false, false);
        
        return total.ToString();

        long FindPaths(string root, bool visitedFft, bool visitedDac)
        {
            var fft = visitedFft || root == "fft";
            var dac = visitedDac || root == "dac";
            
            if (nodeMemo.TryGetValue((root, fft, dac), out long value))
            {
                return value;
            }
            
            if (root == "out")
            {
                long result = (fft && dac) ? 1 : 0;
                nodeMemo[(root, fft, dac)] = result;
                return result;
            }

            long localTotal = 0;
            string[] children = nodeDict[root];
            foreach (string child in children)
            {
                localTotal += FindPaths(child, fft, dac);
            }
            
            nodeMemo[(root, fft, dac)] = localTotal;
            return localTotal;
        }
    }

    public string TestInput() => """
                                 svr: aaa bbb
                                 aaa: fft
                                 fft: ccc
                                 bbb: tty
                                 tty: ccc
                                 ccc: ddd eee
                                 ddd: hub
                                 hub: fff
                                 eee: dac
                                 dac: fff
                                 fff: ggg hhh
                                 ggg: out
                                 hhh: out
                                 """;

    // public string TestInput() => """
    //                              aaa: you hhh
    //                              you: bbb ccc
    //                              bbb: ddd eee
    //                              ccc: ddd eee fff
    //                              ddd: ggg
    //                              eee: out
    //                              fff: out
    //                              ggg: out
    //                              hhh: ccc fff iii
    //                              iii: out
    //                              """;
}