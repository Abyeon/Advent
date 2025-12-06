using Advent.Lib;

namespace Advent.Solutions._2025._6;

public class TrashCompactor : ISolution
{
    [Test("4277556", "4771265398012")]
    public string PartOne(string[] input)
    {
        long total = 0;
        int height = input.Length - 1;
        int width = input[0].Split([' '], StringSplitOptions.RemoveEmptyEntries).Length;
        
        for (var i = 0; i < width; i++)
        {
            long curr = int.Parse(input[0].Split([' '], StringSplitOptions.RemoveEmptyEntries)[i]);
            bool add = input[height].Split([' '], StringSplitOptions.RemoveEmptyEntries)[i][0] == '+';
            for (var j = 1; j < height; j++)
            {
                int num = int.Parse(input[j].Split([' '], StringSplitOptions.RemoveEmptyEntries)[i]);
                if (add)
                {
                    curr += num;
                }
                else
                {
                    curr *= num;
                }
            }

            total += curr;
        }
        
        return total.ToString();
    }

    private struct Pos (int x, int y)
    {
        public int X { get; private set; } = x;
        public int Y { get; private set; } = y;
        public void Iterate(int heightConstraint)
        {
            if (Y + 1 >= heightConstraint)
            {
                Y = 0;
                X--;
                return;
            }

            Y++;
        }
    }

    [Test("3263827", "10695785245101")]
    public string PartTwo(string[] input)
    {
        int width = input[0].Length - 1;
        int height = input.Length;

        long total = 0;
        
        Queue<int> workingInts = new();
        var curr = 0;

        for (var pos = new Pos(width, 0); pos.X >= 0; pos.Iterate(height))
        {
            char c = input[pos.Y][pos.X];
            switch (c)
            {
                case '+':
                    AddCurr();
                    while (workingInts.Count > 0) total += workingInts.Dequeue();
                    break;
                case '*':
                    AddCurr();
                    long temp = workingInts.Dequeue();
                    while (workingInts.Count > 0) temp *= workingInts.Dequeue();
                    total += temp;
                    break;
                case ' ':
                    AddCurr();
                    break;
                default:
                    curr = curr * 10 + (c - 48);
                    break;
            }
        }
        
        return total.ToString();

        void AddCurr()
        {
            if (curr == 0) return;
            workingInts.Enqueue(curr);
            curr = 0;
        }
    }

    public string TestInput() => """
                                 123 328  51 64 
                                  45 64  387 23 
                                   6 98  215 314
                                 *   +   *   +  
                                 """;
}