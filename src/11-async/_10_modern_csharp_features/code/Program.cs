using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ModernFeatures;

public class Example1_ValueTask
{
    private static readonly Dictionary<string, int> Cache = new();

    public static ValueTask<int> GetNumberAsync(string key)
    {
        if (Cache.TryGetValue(key, out var value))
            return new ValueTask<int>(value);  // No allocation!

        return new ValueTask<int>(FetchAsync(key));
    }

    private static async Task<int> FetchAsync(string key)
    {
        await Task.Delay(100);
        var value = key.Length;
        Cache[key] = value;
        return value;
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: ValueTask (C# 7+) ===");

        var start = DateTime.Now;

        // First call - cache miss (allocates Task)
        var r1 = await GetNumberAsync("test");
        var r2 = await GetNumberAsync("test");  // Cache hit (no allocation!)

        Console.WriteLine($"  Result: {r1}, {r2}");
        Console.WriteLine($"  Time: {(DateTime.Now - start).TotalSeconds:F2}s");
    }
}

public class Example2_AsyncIterators
{
    public static async IAsyncEnumerable<int> GenerateAsync()
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(100);
            yield return i * i;
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Async Iterators (C# 8+) ===");

        await foreach (var num in GenerateAsync())
        {
            Console.WriteLine($"  Square: {num}");
        }
    }
}

public class Example3_TopLevelAsync
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Top-Level Async Main (C# 11) ===");
        Console.WriteLine("  This console app uses top-level async Main()");
        await Task.Delay(500);
        Console.WriteLine("  No need for 'static async Task Main()' boilerplate!");
    }
}

// C# 11: Top-level async Main - no boilerplate needed!
class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   MODERN C# FEATURES                                      ║");
        Console.WriteLine("║   ValueTask, Async Iterators, Top-Level Main             ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_ValueTask.Run();
        await Example2_AsyncIterators.Run();
        await Example3_TopLevelAsync.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
