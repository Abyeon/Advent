namespace Advent.Lib;

public static class InterlockedExtensions
{
    public static void Max(ref long target, long value)
    {
        long current;
        do
        {
            current = Volatile.Read(ref target);
            if (value <= current) return;
        }
        
        while (Interlocked.CompareExchange(ref target, value, current) != current);
    }
}