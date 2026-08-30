using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace IOOperations;

public class Example1_FileIO
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: File I/O ===");

        var testFile = "test.txt";
        var content = "Hello, Async World!\nLine 2\nLine 3";

        // Write
        Console.WriteLine("  Writing file...");
        await File.WriteAllTextAsync(testFile, content);
        Console.WriteLine("  File written");

        // Read
        Console.WriteLine("  Reading file...");
        var read = await File.ReadAllTextAsync(testFile);
        Console.WriteLine($"  Content:\n    {read.Replace("\n", "\n    ")}");

        // Cleanup
        if (File.Exists(testFile))
            File.Delete(testFile);
    }
}

public class Example2_HttpClient
{
    private static readonly HttpClient _client = new();

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: HTTP Requests ===");

        try
        {
            // Fetch from public API
            var url = "https://jsonplaceholder.typicode.com/posts/1";
            Console.WriteLine($"  Fetching from {url}...");

            var response = await _client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine("  Response received:");
            Console.WriteLine($"    {json.Substring(0, Math.Min(100, json.Length))}...");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"  Error: {ex.Message}");
        }
    }
}

public class Example3_ConcurrentIO
{
    private static readonly HttpClient _client = new();

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Concurrent I/O Operations ===");

        try
        {
            var urls = new[]
            {
                "https://jsonplaceholder.typicode.com/posts/1",
                "https://jsonplaceholder.typicode.com/posts/2",
                "https://jsonplaceholder.typicode.com/posts/3"
            };

            var start = DateTime.Now;
            Console.WriteLine($"  Fetching {urls.Length} URLs concurrently...");

            // Start all requests
            var tasks = urls.Select(url => _client.GetStringAsync(url)).ToList();

            // Wait for all
            var results = await Task.WhenAll(tasks);

            var elapsed = (DateTime.Now - start).TotalSeconds;
            Console.WriteLine($"  Fetched {results.Length} responses in {elapsed:F2}s");
            Console.WriteLine($"  Average size: {results.Average(r => r.Length):F0} bytes");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Error: {ex.Message}");
        }
    }
}

public class Example4_MultipleFiles
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Multiple File Operations ===");

        var files = new[] { "file1.txt", "file2.txt", "file3.txt" };
        var content = "Test content";

        try
        {
            // Write multiple files concurrently
            Console.WriteLine("  Writing files...");
            var writeTasks = files.Select(f => File.WriteAllTextAsync(f, content)).ToList();
            await Task.WhenAll(writeTasks);

            // Read multiple files concurrently
            Console.WriteLine("  Reading files...");
            var readTasks = files.Select(f => File.ReadAllTextAsync(f)).ToList();
            var contents = await Task.WhenAll(readTasks);

            Console.WriteLine($"  Read {contents.Length} files: {string.Join(", ", contents.Select(c => c.Length))} bytes");

            // Cleanup
            foreach (var file in files)
                if (File.Exists(file))
                    File.Delete(file);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Error: {ex.Message}");
        }
    }
}

public class Example5_ReadAsStream
{
    private static readonly HttpClient _client = new();

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Streaming Large Content ===");

        try
        {
            var url = "https://jsonplaceholder.typicode.com/posts";
            Console.WriteLine("  Downloading with streaming...");

            using var response = await _client.GetAsync(url);
            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            var firstLine = await reader.ReadLineAsync();
            Console.WriteLine($"  First line: {firstLine?.Substring(0, Math.Min(60, firstLine?.Length ?? 0))}...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  Error: {ex.Message}");
        }
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   I/O OPERATIONS                                          ║");
        Console.WriteLine("║   File & Network Async Operations                         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_FileIO.Run();
        await Example2_HttpClient.Run();
        await Example3_ConcurrentIO.Run();
        await Example4_MultipleFiles.Run();
        await Example5_ReadAsStream.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
