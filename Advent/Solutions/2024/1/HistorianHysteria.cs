using Advent.Lib;

namespace Advent.Solutions._2024._1;

public class HistorianHysteria : ISolution
{
    [Test("11", "1941353")]
    public string PartOne(string[] input)
    {
        var left = new List<int>();
        var right = new List<int>();

        
        foreach (string line in input)
        {
            string[] nums = line.Split("   ");
            left.Add(int.Parse(nums[0]));
            right.Add(int.Parse(nums[1]));
        }

        left.Sort();
        right.Sort();
        
        int totalDistance = left.Select((t, i) => Math.Abs(t - right[i])).Sum();

        return totalDistance.ToString();
    }

    [Test("31", "22539317")]
    public string PartTwo(string[] input)
    {
        var left = new List<int>();
        var right = new List<int>();

        
        foreach (string line in input)
        {
            string[] nums = line.Split("   ");
            left.Add(int.Parse(nums[0]));
            right.Add(int.Parse(nums[1]));
        }

        left.Sort();
        right.Sort();
        
        int similarityScore = 0;

        foreach (int t in left)
        {
            int occurrences = right.Count(x => x.Equals(t));
            similarityScore += t * occurrences;
        }
        
        return similarityScore.ToString();
    }

    public string TestInputA() => """
                                 3   4
                                 4   3
                                 2   5
                                 1   3
                                 3   9
                                 3   3
                                 """;
    
    public string TestInputB() => TestInputA();
}