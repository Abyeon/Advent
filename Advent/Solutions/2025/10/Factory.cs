using Advent.Lib;
using ZLinq.Linq;

namespace Advent.Solutions._2025._10;

public class Factory : ISolution
{
    private static int Solve(ulong target, ulong[] flips)
    {
        const ulong start = 0L;
        
        var frontA = new Dictionary<ulong, int>();
        var frontB = new Dictionary<ulong, int>();

        var qA = new Queue<ulong>();
        var qB = new Queue<ulong>();

        frontA[start] = 0;
        frontB[target] = 0;
        
        qA.Enqueue(start);
        qB.Enqueue(target);

        while (qA.Count > 0 && qB.Count > 0)
        {
            if (qA.Count <= qB.Count)
            {
                if (Expand(qA, frontA, frontB, flips, out int result))
                    return result;
            }
            else
            {
                if (Expand(qB, frontB, frontA, flips, out int result))
                    return result;
            }
        }

        return -1;
    }

    private static bool Expand(
        Queue<ulong> queue,
        Dictionary<ulong, int> mine,
        Dictionary<ulong, int> other,
        ulong[] flips, out int answer)
    {
        
        int i = queue.Count;
        while (i-- > 0)
        {
            ulong cur = queue.Dequeue();
            int depth = mine[cur];

            foreach (ulong flip in flips)
            {
                ulong next = cur ^ flip;

                if (mine.ContainsKey(next)) continue;

                if (other.TryGetValue(next, out int otherDepth))
                {
                    answer = depth + 1 + otherDepth;
                    return true;
                }
                
                mine[next] = depth + 1;
                queue.Enqueue(next);
            }
        }

        answer = -1;
        return false;
    }
    
    [Test("7", "491")]
    public string PartOne(string[] input)
    {
        var total = 0;
        foreach (string line in input)
        {
            int bracketIndex = line.IndexOf(']');
            int curlIndex = line.IndexOf('{');
            char[] diagram = line[1..bracketIndex].ToCharArray();
            ulong toMask = 0;

            for (var i = 0; i < diagram.Length; i++)
            {
                char c = diagram[i];
                if (c == '#') toMask |= 1UL << i;
            }

            string schematicsLine = line[(bracketIndex + 1)..(curlIndex - 1)];
            string[] schematicsSplit = schematicsLine.Split(['(', ')', ' '], StringSplitOptions.RemoveEmptyEntries);

            List<ulong> schematics = [];
            foreach (string numList in schematicsSplit)
            {
                string[] numSplit = numList.Split(',');
                ulong mask = 0;
                
                foreach (string num in numSplit)
                {
                    int index = int.Parse(num);
                    mask |= (1UL << index);
                }
                
                schematics.Add(mask);
            }
            
            total += Solve(toMask, schematics.ToArray());
        }

        return total.ToString();
    }

    [Test("33", "20617")]
    public string PartTwo(string[] input)
    {
        // I cheated for part two (seriously it was this or Z3... I'll come back after learning some Z3) :(
        var total = Day10Solver.Solve(input.AsEnumerable());
        return total.Part2.ToString();
    }

    public string TestInputA() => """
                                 [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
                                 [...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
                                 [.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
                                 """;
    
    public string TestInputB() => TestInputA();
}