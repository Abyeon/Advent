using System.Runtime.CompilerServices;
using Advent.Lib;

namespace Advent.Solutions._2025._9;

public class MovieTheater : ISolution
{
    private readonly struct Point(long x, long y)
    {
        public readonly long X = x;
        public readonly long Y = y;
    }
    
    [Test("50", "4786902990")]
    public string PartOne(string[] input)
    {
        var points = new Point[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            var s = input[i].AsSpan();
            int comma = s.IndexOf(',');
            points[i] = new Point(int.Parse(s[..comma]), int.Parse(s[(comma + 1)..]));
        }

        var largestArea = 0L;
        for (var i = 0; i < points.Length - 1; i++)
        {
            var first = points[i];
            for (var j = i + 1; j < points.Length; j++)
            {
                var second = points[j];
                long width  = second.X > first.X ? second.X - first.X : first.X - second.X;
                long height = second.Y > first.Y ? second.Y - first.Y : first.Y - second.Y;
                
                long area = (width + 1) * (height + 1);

                if (area > largestArea) largestArea = area;
            }
        }

        return largestArea.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool PointInsidePolygon(Point point, Point[] vertices)
    {
        var inside = false;

        var a = vertices[^1];
        long px = point.X;
        long py = point.Y;

        for (var i = 0; i < vertices.Length; i++)
        {
            var b = vertices[i];

            long ax = a.X, ay = a.Y;
            long bx = b.X, by = b.Y;

            // Vertex hit
            if (bx == px && by == py)
                return true;

            // Horizontal edge check
            if (ay == by && py == ay)
            {
                if ((ax <= px && px <= bx) || (bx <= px && px <= ax))
                    return true;
            }

            // Straddling test
            bool straddles = (by < py) ^ (ay < py);
            if (straddles)
            {
                long dy = ay - by;
                long lhs = (py - by) * (ax - bx);
                long rhs = (px - bx) * dy;
                
                if ((dy > 0 && lhs <= rhs) || (dy < 0 && lhs >= rhs))
                    inside = !inside;
            }

            a = b;
        }

        return inside;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool SegmentsIntersect(Point p1, Point p2, Point p3, Point p4)
    {
        long d1 = Cross(p1, p2, p3);
        long d2 = Cross(p1, p2, p4);
        long d3 = Cross(p3, p4, p1);
        long d4 = Cross(p3, p4, p2);
        
        return ((d1 > 0 && d2 < 0) || (d1 < 0 && d2 > 0)) &&
               ((d3 > 0 && d4 < 0) || (d3 < 0 && d4 > 0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static long Cross(Point a, Point b, Point c)
            => (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
    }
    
    private static bool RectInsidePolygon(Point topLeft, Point bottomRight, (Point A, Point B)[] edges, Point[] vertices)
    {
        // Inline rectangle values
        long left   = topLeft.X;
        long right  = bottomRight.X;
        long top    = topLeft.Y;
        long bottom = bottomRight.Y;

        // Corner checks
        if (!PointInsidePolygon(new Point(left,  top),    vertices)) return false;
        if (!PointInsidePolygon(new Point(right, top),    vertices)) return false;
        if (!PointInsidePolygon(new Point(left,  bottom), vertices)) return false;
        if (!PointInsidePolygon(new Point(right, bottom), vertices)) return false;
        
        var r1 = new Point(left,  top);
        var r2 = new Point(right, top);
        var r3 = new Point(right, bottom);
        var r4 = new Point(left,  bottom);
        
        foreach (var (a, b) in edges)
        {
            long minX = a.X < b.X ? a.X : b.X;
            long minY = a.Y < b.Y ? a.Y : b.Y;
            long maxX = a.X > b.X ? a.X : b.X;
            long maxY = a.Y > b.Y ? a.Y : b.Y;

            if (maxX < left || minX > right ||
                maxY < bottom || minY > top)
                continue;
            
            if (SegmentsIntersect(r1, r2, a, b)) return false; // top
            if (SegmentsIntersect(r2, r3, a, b)) return false; // right
            if (SegmentsIntersect(r3, r4, a, b)) return false; // bottom
            if (SegmentsIntersect(r4, r1, a, b)) return false; // left
        }
        
        return true;
    }

    [Test("24", "1571016172")]
    public string PartTwo(string[] input)
    {
        var points = new Point[input.Length];
        
        for (var i = 0; i < input.Length; i++)
        {
            var s = input[i].AsSpan();
            int comma = s.IndexOf(',');
            points[i] = new Point(int.Parse(s[..comma]), int.Parse(s[(comma + 1)..]));
        }

        var edges = new (Point A, Point B)[points.Length];
        for (var i = 0; i < points.Length; i++)
        {
            edges[i] = (points[i], points[(i + 1) % points.Length]);
        }

        var largestArea = 0L;
        Parallel.For<long>(0, points.Length - 1, () => 0L, (i, _, localMax) =>
        {
            var first = points[i];
            
            for (int j = i + 1; j < points.Length; j++)
            {
                var second = points[j];
                
                long width  = second.X > first.X ? second.X - first.X : first.X - second.X;
                long height = second.Y > first.Y ? second.Y - first.Y : first.Y - second.Y;
                
                long possibleArea = (width + 1) * (height + 1);
                if (possibleArea <= localMax) continue;
                
                var topLeft  = new Point(Math.Min(first.X, second.X), Math.Max(first.Y, second.Y));
                var botRight = new Point(Math.Max(first.X, second.X), Math.Min(first.Y, second.Y));
        
                if (!RectInsidePolygon(topLeft, botRight, edges, points)) continue;
                
                localMax = possibleArea;
            }
        
            return localMax;
        }, localMax =>
        {
            InterlockedExtensions.Max(ref largestArea, localMax);
        });

        return largestArea.ToString();
    }

    public string TestInputA() => """
                                 7,1
                                 11,1
                                 11,7
                                 9,7
                                 9,5
                                 2,5
                                 2,3
                                 7,3
                                 """;
    
    public string TestInputB() => TestInputA();
}