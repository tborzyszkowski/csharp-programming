using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RealWorld;

public class ApiClient
{
    private readonly HttpClient _client = new();
    private readonly Random _random = new();

    public async Task<List<string>> FetchPostsAsync(int count)
    {
        var tasks = Enumerable.Range(1, count)
            .Select(i => FetchPostAsync(i))
            .ToList();

        return (await Task.WhenAll(tasks)).ToList();
    }

    private async Task<string> FetchPostAsync(int id)
    {
        try
        {
            await Task.Delay(_random.Next(100, 500));
            return $"Post {id}: Sample content";
        }
        catch
        {
            return $"Post {id}: Failed to fetch";
        }
    }
}

public class Example1_CompleteSystem
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Complete Real-World System ===");

        var client = new ApiClient();
        var posts = await client.FetchPostsAsync(5);

        Console.WriteLine("  Fetched posts:");
        foreach (var post in posts)
        {
            Console.WriteLine($"    - {post}");
        }
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   REAL-WORLD INTEGRATION                                  ║");
        Console.WriteLine("║   API Client, Error Handling, Logging                     ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_CompleteSystem.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ Example Completed                                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
