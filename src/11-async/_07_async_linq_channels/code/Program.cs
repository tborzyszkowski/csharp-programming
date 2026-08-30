using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AsyncLinqChannels;

public class Example1_AsyncEnumerable
{
    public static async IAsyncEnumerable<int> GenerateAsync()
    {
        for (int i = 0; i < 5; i++)
        {
            await Task.Delay(200);
            yield return i;
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Async Enumerable (C# 8+) ===");
        
        await foreach (var item in GenerateAsync())
        {
            Console.WriteLine($"  Item: {item}");
        }
    }
}

public class Example2_AsyncLINQ
{
    public static async IAsyncEnumerable<int> GetNumbersAsync()
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(100);
            yield return i;
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Async LINQ Filtering ===");

        var evenNumbers = new List<int>();
        await foreach (var n in GetNumbersAsync())
        {
            if (n % 2 == 0)
                evenNumbers.Add(n);
        }

        Console.WriteLine($"  Even numbers: {string.Join(", ", evenNumbers)}");
    }
}

public class Example3_ProducerConsumer
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Producer-Consumer with Channels ===");

        var channel = Channel.CreateUnbounded<int>();

        // Producer
        var producer = Task.Run(async () =>
        {
            for (int i = 0; i < 5; i++)
            {
                await channel.Writer.WriteAsync(i);
                await Task.Delay(200);
            }
            channel.Writer.Complete();
        });

        // Consumer
        var consumer = Task.Run(async () =>
        {
            await foreach (var item in channel.Reader.ReadAllAsync())
            {
                Console.WriteLine($"  Consumed: {item}");
            }
        });

        await Task.WhenAll(producer, consumer);
    }
}

public class Example4_AsyncLinqSelect
{
    public static async IAsyncEnumerable<int> GetDataAsync()
    {
        for (int i = 1; i <= 3; i++)
        {
            await Task.Delay(100);
            yield return i;
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Async Transform ===");

        await foreach (var doubled in DoubleAsync())
        {
            Console.WriteLine($"  Doubled: {doubled}");
        }
    }

    public static async IAsyncEnumerable<int> DoubleAsync()
    {
        await foreach (var item in GetDataAsync())
        {
            yield return item * 2;
        }
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ASYNC LINQ & CHANNELS                                   ║");
        Console.WriteLine("║   IAsyncEnumerable, Channels, Producer-Consumer           ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_AsyncEnumerable.Run();
        await Example2_AsyncLINQ.Run();
        await Example3_ProducerConsumer.Run();
        await Example4_AsyncLinqSelect.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
