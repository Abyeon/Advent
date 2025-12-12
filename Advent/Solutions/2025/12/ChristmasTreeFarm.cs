using Advent.Lib;

namespace Advent.Solutions._2025._12;

public class ChristmasTreeFarm : ISolution
{
    private class Shape
    {
        public readonly int Area;
        public List<bool[,]> Variations = [];

        public Shape(bool[,] shape)
        {
            foreach (bool s in shape) if (s) Area++;

            // Add base shape
            Variations.Add(shape);
            
            // Add rotated variations
            bool[,] last = shape;
            for (var i = 0; i < 3; i++)
            {
                bool[,] rotated = RotateClockwise(last);
                last = rotated;
                
                Variations.Add(rotated);
            }
            
            // Add flipped variations
            bool[,] flipped = FlipHorizontal(shape);
            Variations.Add(flipped);
            last = flipped;
            
            for (var i = 0; i < 3; i++)
            {
                bool[,] rotated = RotateClockwise(last);
                last = rotated;
                
                Variations.Add(rotated);
            }
        }

        private static bool[,] RotateClockwise(bool[,] original)
        {
            var rotated = new bool[3, 3];

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    rotated[j, 2 - i] = original[i, j];
                }
            }

            return rotated;
        }

        private static bool[,] FlipHorizontal(bool[,] original)
        {
            var flipped = new bool[3, 3];

            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    flipped[i, j] = original[i, 2 - j];
                    flipped[i, 2 - j] = original[i, j];
                }
            }

            return flipped;
        }
    }
    
    [Test("2", "440")]
    public string PartOne(string[] input)
    {
        List<Shape> shapes = [];

        // Parse shapes
        var i = 0;
        for (; i < 4 * 6; i += 4)
        {
            var positions = new bool[3, 3];
            
            // Get rows
            for (int j = i + 1; j < i + 4; j++)
            {
                ReadOnlySpan<char> line = input[j];
                
                // Convert to bool
                for (var k = 0; k < 3; k++) positions[(j - i) - 1, k] = (line[k] == '#');
            }
            
            // Add new shape
            shapes.Add(new Shape(positions));
        }

        List<(int[] size, int[] shapeCounts)> regions = [];
        
        // Parse regions
        for (; i < input.Length; i++)
        {
            string line = input[i];
            int colon = line.IndexOf(':');
            string[] splitSize = line[..colon].Split('x');

            var size = new int[2];
            size[0] = int.Parse(splitSize[0]);
            size[1] = int.Parse(splitSize[1]);

            string[] splitShapes = line[(colon + 1)..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var shapeCounts = new int[splitShapes.Length];
            
            // Calculate the area the shapes would use
            int area = size[0] * size[1];
            long totalUsedArea = 0;
            for (var j = 0; j < splitShapes.Length; j++)
            {
                int shapeArea = shapes[j].Area;
                int multiplier = int.Parse(splitShapes[j]);
                totalUsedArea += shapeArea * multiplier;
                
                shapeCounts[j] = multiplier;
            }
            
            // Discard if impossible to fill
            if (totalUsedArea > area) continue;
            
            regions.Add((size, shapeCounts));
        }

        long total = 0;
        
        // Go through possible regions to fill
        foreach ((int[] size, int[] counts) in regions)
        {
            total++;
        }
        
        return total.ToString(); // Apparently just culling works. Lol
    }

    [Test("Advent of Code complete! :)", "Advent of Code complete! :)")]
    public string PartTwo(string[] input)
    {
        return "Advent of Code complete! :)";
    }

    public string TestInputA() => """
                                  0:
                                  ###
                                  ##.
                                  ##.
                                  
                                  1:
                                  ###
                                  ##.
                                  .##
                                  
                                  2:
                                  .##
                                  ###
                                  ##.
                                  
                                  3:
                                  ##.
                                  ###
                                  ##.
                                  
                                  4:
                                  ###
                                  #..
                                  ###
                                  
                                  5:
                                  ###
                                  .#.
                                  ###
                                  
                                  4x4: 0 0 0 0 2 0
                                  12x5: 1 0 1 0 2 2
                                  12x5: 1 0 1 0 3 2
                                  """;

    public string TestInputB() => TestInputA();
}