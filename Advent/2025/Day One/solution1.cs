/*
const string path = "D:/Programming/Advent/Advent/2025/Day One/input.txt";

try
{
    int totalZero = 0;
    int pos = 50;
    foreach (var line in File.ReadAllLines(path))
    {
        var sign = 1;
        if (line[..1].Equals("L")) sign = -1;
        
        int amount = int.Parse(line[1..]);
        pos = (pos + (sign * amount)) % 100;
        if (pos < 0) pos += 100;
        
        if (pos == 0) totalZero++;
        
        Console.WriteLine(pos);
    }
    
    Console.WriteLine($"Total Zeroes: {totalZero}");
}
catch (FileNotFoundException)
{
    Console.WriteLine("File not found");
}
catch (IOException ex)
{
    Console.WriteLine($"An I/O error occurred: {ex.Message}");
}

Console.WriteLine("Hello, World!");*/