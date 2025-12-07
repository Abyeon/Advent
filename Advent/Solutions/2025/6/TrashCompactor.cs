using System.Numerics;
using Advent.Lib;

namespace Advent.Solutions._2025._6;

public class TrashCompactor : ISolution
{
    [Test("4277556", "4771265398012")]
    public string PartOne(string[] input)
    {
        string[] instructions = input[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        
        int height = input.Length - 1;
        int width = instructions.Length;
        var blocks = new int[height, width];
        
        long total = 0;

        for (var i = 0; i < height; i++)
        {
            string[] cols = input[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (var j = 0; j < width; j++)
            {
                blocks[i, j] = int.Parse(cols[j]);
            }
        }

        for (var i = 0; i < width; i++)
        {
            string instruction = instructions[i];
            if (instruction == "+")
            {
                for (var j = 0; j < height; j++)
                {
                    total += blocks[j, i];
                }
            }
            else
            {
                long temp = blocks[0, i];
                for (var j = 1; j < height; j++)
                {
                    temp *= blocks[j, i];
                }
                total += temp;
            }
        }
        
        return total.ToString();
    }

    private struct Pos (int x, int y)
    {
        public int X { get; set; } = x;
        public int Y { get; set; } = y;
        public void Iterate(int heightConstraint)
        {
            if (Y + 1 == heightConstraint)
            {
                Y = 0;
                X--;
                return;
            }

            Y++;
        }
    }

    //private IDictionary<char, Action> instructionMap = [];

    [Test("3263827", "10695785245101")]
    public string PartTwo(string[] input)
    {
        int width = input[0].Length - 1;
        int height = input.Length;
        
        long total = 0;
        
        Queue<int> workingInts = new();
        var curr = 0;
        
        // instructionMap.Add('+', () =>
        // { 
        //     while (workingInts.Count != 0) total += workingInts.Dequeue();
        // });
        //
        // instructionMap.Add('*', () =>
        // { 
        //     long temp = workingInts.Dequeue();
        //     while (workingInts.Count != 0) temp *= workingInts.Dequeue();
        //     total += temp;
        // });
        
        
        for (var pos = new Pos(width, 0); pos.X >= 0; pos.Iterate(height))
        {
            char c = input[pos.Y][pos.X];
            //instructionMap[c]();
            
            switch (c)
            {
                case '+':
                    AddCurr();
                    while (workingInts.Count != 0) total += workingInts.Dequeue();
                    pos.X--;
                    break;
                case '*':
                    AddCurr();
                    long temp = workingInts.Dequeue();
                    while (workingInts.Count != 0) temp *= workingInts.Dequeue();
                    total += temp;
                    pos.X--;
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