using System.Net;

namespace Advent.Lib;

public abstract class AdventOfCode
{
    /// <summary>
    /// Tries to get the user's cookie from the specified file, if not found prompt the user to provide it.
    /// </summary>
    /// <param name="filename">Path for the file containing the cookie</param>
    /// <returns>Task for the cookie</returns>
    /// <exception cref="Exception">Throws if user does not provide a cookie when prompted</exception>
    public static async Task<string> GetCookie(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("Please paste your Advent of Code cookie here: ");
            string? response = Console.ReadLine();
            
            if (response == null) throw new Exception("No Advent of Code cookie found");
            
            await File.WriteAllTextAsync(filename, response);
            return response;
        }
        
        string cookie = await File.ReadAllTextAsync(filename);
        return cookie;
    }

    /// <summary>
    /// Get the Advent of Code input for year and day
    /// </summary>
    /// <param name="year">Year of puzzle</param>
    /// <param name="day">Day of puzzle</param>
    /// <param name="cookie">User's Advent of Code cookie</param>
    /// <param name="path">Path to save the file</param>
    /// <returns>Task for the desired input</returns>
    public static async Task<string> GetInput(int year, int day, string cookie, string path)
    {
        // File exists, just get the data
        if (File.Exists(path)) return File.ReadAllTextAsync(path).Result;
        
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
        await using var file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
        await using var stream = await response.Content.ReadAsStreamAsync();
        await stream.CopyToAsync(file);
        
        Console.WriteLine($"Puzzle input saved to {path}");
            
        // Finally return the data
        return response.Content.ReadAsStringAsync().Result;
    }
}