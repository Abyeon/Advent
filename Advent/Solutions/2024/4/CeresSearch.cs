using System.Numerics;
using Advent.Lib;

namespace Advent.Solutions._2024._4;

public class CeresSearch : ISolution
{
    private const string Xmas = "XMAS";
    
    // Directions from coordinates to search
    private static readonly Vector2[] Directions = [
        new(1, 0),
        new(0, 1),
        new(0, -1),
        new(-1, 0),
        new(1, 1),
        new(-1, 1),
        new(1, -1),
        new(-1, -1)
    ];

    [Test("18", "2662")]
    public string PartOne(string[] input)
    {
        int total = 0;

        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            
            for (var j = 0; j < line.Length; j++) {
                char c = line[j];
                if (c == 'X') {
                    // search for XMAS horizontally, vertically, diagonally, and backwards
                    FindXmas(j, i);
                }
            }
        }
        
        return total.ToString();

        void FindXmas(int charIndex, int lineIndex)
        {
            var coords = new Vector2(lineIndex, charIndex);
            foreach (var dir in Directions) {
                for (var i = 1; i <= 3; i++) {
                    try {
                        var tempCoords = coords + (dir * i);
                        char charInDir = input[(int)tempCoords.X][(int)tempCoords.Y];

                        if (charInDir == Xmas[i]) {
                            if (i == 3) total++;
                        } else {
                            break;
                        }
                    } catch (Exception) {
                        break;
                    }
                }
            }
        }
    }

    [Test("9", "2034")]
    public string PartTwo(string[] input)
    {
        int total = 0;

        for (int i = 0; i < input.Length; i++)
        {
            string line = input[i];
            
            for (var j = 0; j < line.Length; j++) {
                char c = line[j];
                if (c == 'A') {
                    // search for XMAS horizontally, vertically, diagonally, and backwards
                    FindXOfMas(j, i);
                }
            }
        }
        
        return total.ToString();
        
        void FindXOfMas(int charIndex, int lineIndex)
        {
            Vector2 coords = new Vector2(lineIndex, charIndex);

            try {
                var unoMas = new string([input[(int)coords.X + 1][(int)coords.Y + 1], input[(int)coords.X][(int)coords.Y], input[(int)coords.X - 1][(int)coords.Y - 1]]);
                var dosMas = new string([input[(int)coords.X - 1][(int)coords.Y + 1], input[(int)coords.X][(int)coords.Y], input[(int)coords.X + 1][(int)coords.Y - 1]]);

                if (unoMas is "MAS" or "SAM" && dosMas is "MAS" or "SAM") {
                    total++;
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    public string TestInputA() => """
                                 MMMSXXMASM
                                 MSAMXMSMSA
                                 AMXSXMAAMM
                                 MSAMASMSMX
                                 XMASAMXAMM
                                 XXAMMXXAMA
                                 SMSMSASXSS
                                 SAXAMASAAA
                                 MAMMMXMMMM
                                 MXMXAXMASX
                                 """;
    
    public string TestInputB() => TestInputA();
}