using System.Reflection;
using Advent.Lib;

var solutions = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solution).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

var currentDir = Directory.GetCurrentDirectory();

var cookie = await AdventOfCode.GetCookie(Path.Combine(currentDir, "cookie.txt"));

foreach (var type in solutions)
{
    var solution = (Solution)Activator.CreateInstance(type)!;
    Console.WriteLine($"Name: {type.Name} Year: {solution.Year()} Day: {solution.Day()}");

    var path = Path.Combine(currentDir, $"inputs/{solution.Year()}/");
    if (!Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
        Console.WriteLine($"Created: {path}");
    }
    
    path = Path.Combine(path, $"{solution.Day()}.txt");
    var input = await AdventOfCode.GetInput(solution.Year(), solution.Day(), cookie, path);
    
    var lines = input.Split(['\r', '\n'], StringSplitOptions.None);
    solution.Solve(lines);
}