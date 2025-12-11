using Advent.Lib;

namespace Advent.Solutions._2025._8;

public class Playground : ISolution
{
    private readonly record struct Point (long X, long Y, long Z)
    {
        public long Dist(Point other)
        {
            long dx = other.X - X;
            long dy = other.Y - Y;
            long dz = other.Z - Z;
            return dx * dx + dy * dy + dz * dz;
        }

        public override string ToString()
        {
            return $"(X: {X}, Y: {Y}, Z: {Z})";
        }
    }

    private struct Edge (double dist, int p1Index, int p2Index)
    {
        public double Distance = dist;
        public int P1Index = p1Index;
        public int P2Index = p2Index;
    }

    private class Dsu
    {
        private readonly int[] Parent;
        private readonly int[] Rank;
        private readonly int[] Size;
        public int Count { get; private set; }

        public Dsu(int n)
        {
            Parent = new int[n];
            Rank = new int[n];
            Size = new int[n];
            Count = n;
            
            for (int i = 0; i < n; i++)
            {
                Parent[i] = i;
                Size[i] = 1;
            }
        }
        
        public int Find(int i)
        {
            if (Parent[i] == i) return i;
            return Parent[i] = Find(Parent[i]);
        }

        public bool Union(int i, int j)
        {
            int rootI = Find(i);
            int rootJ = Find(j);

            if (rootI == rootJ) return false;
            
            if (Rank[rootI] < Rank[rootJ])
            {
                (rootI, rootJ) = (rootJ, rootI);
            }
            else if (Rank[rootI] == Rank[rootJ])
            {
                Rank[rootI]++;
            }
                
            Parent[rootJ] = rootI;
            Size[rootI] += Size[rootJ];
                
            Count--;
            return true;

        }

        public IEnumerable<int> GetSizes()
        {
            return Parent.Where((parent, index) => parent == index).Select(root => Size[root]);
        }
    }
    
    [Test("40", "80446")]
    public string PartOne(string[] input)
    {
        int count = input.Length;
        var points = new Point[count];
        var edges = new List<Edge>();

        // Parse
        for (var i = 0; i < count; i++)
        {
            string[] values = input[i].Split(',');
            points[i] = new Point(long.Parse(values[0]), long.Parse(values[1]), long.Parse(values[2]));
        }

        // Calculate edges
        for (var i = 0; i < count; i++)
        {
            for (var j = i + 1; j < count; j++)
            {
                double dist = points[i].Dist(points[j]);
                edges.Add(new Edge(dist, i, j));
            }
        }
        
        // Sort by distance
        edges.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        
        var dsu = new Dsu(count);
        int connectionsMade = 0;
        int connectionsToMake = count > 20 ? 1000 : 10;

        foreach (var edge in edges)
        {
            connectionsMade++;

            dsu.Union(edge.P1Index, edge.P2Index);
            if (connectionsMade == connectionsToMake) break;
        }

        var sizes = dsu.GetSizes().OrderByDescending(size => size).ToList();
        
        long total = sizes[0] * sizes[1] * sizes[2];
        return total.ToString();
    }

    [Test("25272", "51294528")]
    public string PartTwo(string[] input)
    {
        int count = input.Length;
        var points = new Point[count];
        var edges = new List<Edge>();

        // Parse
        for (var i = 0; i < count; i++)
        {
            string[] values = input[i].Split(',');
            points[i] = new Point(long.Parse(values[0]), long.Parse(values[1]), long.Parse(values[2]));
        }

        // Calculate edges
        for (var i = 0; i < count; i++)
        {
            for (var j = i + 1; j < count; j++)
            {
                double dist = points[i].Dist(points[j]);
                edges.Add(new Edge(dist, i, j));
            }
        }
        
        // Sort by distance
        edges.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        
        var dsu = new Dsu(count);
        
        foreach (var edge in edges)
        {
            if (dsu.Find(edge.P1Index) == dsu.Find(edge.P2Index)) continue;
            dsu.Union(edge.P1Index, edge.P2Index);

            if (dsu.Count == 1)
            {
                long lastX = points[edge.P1Index].X;
                long otherX = points[edge.P2Index].X;
                return (lastX * otherX).ToString();
            }
        }
        
        return "Something went wrong.";
    }

    public string TestInputA() => """
                                 162,817,812
                                 57,618,57
                                 906,360,560
                                 592,479,940
                                 352,342,300
                                 466,668,158
                                 542,29,236
                                 431,825,988
                                 739,650,466
                                 52,470,668
                                 216,146,977
                                 819,987,18
                                 117,168,530
                                 805,96,715
                                 346,949,466
                                 970,615,88
                                 941,993,340
                                 862,61,35
                                 984,92,344
                                 425,690,689
                                 """;
    
    public string TestInputB() => TestInputA();
}