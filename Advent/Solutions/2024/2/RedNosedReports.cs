using Advent.Lib;

namespace Advent.Solutions._2024._2;

public class RedNosedReports : ISolution
{
    bool CheckSafe(int[] line)
    {
        int delta = 0;

        bool safe = true;

        for (int i = 0; i < line.Length - 1; i++) {
            int temp = line[i] - line[i + 1];

            if ((Math.Abs(temp) > 3 || Math.Abs(temp) < 1) || (Math.Sign(temp) != Math.Sign(delta) && delta != 0)) {
                safe = false;
            }

            delta += temp;
        }

        return safe;
    }
    
    [Test("2", "631")]
    public string PartOne(string[] input)
    {
        var safeLines = 0;
        
        foreach (string line in input)
        {
            int[] nums = Array.ConvertAll(line.Split(' '), int.Parse);

            bool safe = CheckSafe(nums);

            if (safe) {
                safeLines++;
            }
        }
        
        return safeLines.ToString();
    }

    [Test("4", "665")]
    public string PartTwo(string[] input)
    {
        var safeLines = 0;
        var saveable = 0;
        
        foreach (string line in input)
        {
            int[] nums = Array.ConvertAll(line.Split(' '), int.Parse);

            bool safe = CheckSafe(nums); // Helper function that uses part 1 solution to check any array for safeness

            if (safe) {
                safeLines++;
            } else {
                // Check for a safe version of the line, by removing each index from the array and checking if that version is safe.
                for (var i = 0; i < nums.Length; i++) {
                    int[] newNums = nums.Where((source, index) => index != i).ToArray();

                    if (!CheckSafe(newNums)) continue;
                    saveable++;
                    break;
                }
            }
        }
        
        return (safeLines + saveable).ToString();
    }

    public string TestInputA() => """
                                 7 6 4 2 1
                                 1 2 7 8 9
                                 9 7 6 2 1
                                 1 3 2 4 5
                                 8 6 4 4 1
                                 1 3 6 7 9
                                 """;
    
    public string TestInputB() => TestInputA();
}