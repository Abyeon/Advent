using System.Reflection;
using Advent.Lib;

var solutions = Assembly.GetEntryAssembly()!.GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(Solution).IsAssignableFrom(t))
    .OrderBy(t => t.FullName)
    .ToArray();

foreach (var type in solutions)
{
    var solution = (Solution)Activator.CreateInstance(type)!;
    Console.WriteLine($"Name: {type.Name} Year: {solution.Year()} Day: {solution.Day()}");
    solution.Solve(" ");
}