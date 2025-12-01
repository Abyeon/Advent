const string path = "D:/Programming/Advent/Advent/2025/Day One/input.txt";

try
{
    var totalZero = 0;
    var pos = 50;
    foreach (var line in File.ReadAllLines(path))
    {
        var sign = 1;
        if (line[..1].Equals("L")) sign = -1;
        
        var amount = int.Parse(line[1..]);
        for (var i = 0; i < amount; i++)
        {
            pos = (pos + sign) % 100;
            if (pos < 0) pos += 100;
            if (pos == 0) totalZero++;
        }
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