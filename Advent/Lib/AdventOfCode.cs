using System.Net;

namespace Advent.Lib;

public abstract class AdventOfCode
{
    public static async Task<string> GetCookie(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("Please paste your Advent of Code cookie here: ");
            var response = Console.ReadLine();
            
            if (response == null) throw new Exception("No Advent of Code cookie found");
            
            await File.WriteAllTextAsync(filename, response);
            return response;
        }
        
        var cookie = await File.ReadAllTextAsync(filename);
        return cookie;
    }
    
    public static async Task<string> GetInput(int year, int day, string cookie, string filename)
    {
        // File exists, just get the data
        if (File.Exists(filename)) return File.ReadAllTextAsync(filename).Result;
        
        // Fetch input from AoC using provided cookie
        var uri = new Uri("https://adventofcode.com");
        var cookies = new CookieContainer();
        cookies.Add(uri, new Cookie("session", cookie));
        
        using var handler = new HttpClientHandler();
        handler.CookieContainer = cookies;
        
        using var client = new HttpClient(handler);
        client.BaseAddress = uri;
        
        Console.WriteLine($"Fetching puzzle input from AdventOfCode...");
        
        using var response = await client.GetAsync($"/{year}/day/{day}/input");
        response.EnsureSuccessStatusCode();
        
        // Write to file
        await using var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
        await using var stream = await response.Content.ReadAsStreamAsync();
        await stream.CopyToAsync(file);
        
        Console.WriteLine($"Puzzle input saved to {filename}");
            
        // Finally return the data
        return response.Content.ReadAsStringAsync().Result;
    }
}