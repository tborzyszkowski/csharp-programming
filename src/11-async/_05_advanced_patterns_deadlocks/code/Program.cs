using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedPatterns;

public class Example1_CancellationToken
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: CancellationToken ===");

        var cts = new CancellationTokenSource();
        cts.CancelAfter(2000);  // Cancel after 2 seconds

        try
        {
            await LongOperationAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("  Operation was cancelled");
        }
    }

    private static async Task LongOperationAsync(CancellationToken ct)
    {
        for (int i = 0; i < 10; i++)
        {
            ct.ThrowIfCancellationRequested();
            Console.WriteLine($"  Step {i+1}...");
            await Task.Delay(500, ct);
        }
    }
}

public class Example2_ConfigureAwait
{
    private static async Task<string> GetDataAsync()
    {
        await Task.Delay(500);
        return "Data";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: ConfigureAwait(false) ===");

        // In library code - ConfigureAwait(false)
        var data = await GetDataAsync().ConfigureAwait(false);
        Console.WriteLine($"  Data: {data}");
        Console.WriteLine("  ✓ No context switching overhead");
    }
}

public class Example3_Timeout
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Timeout Handling ===");

        var cts = new CancellationTokenSource(2000);  // 2 second timeout

        try
        {
            Console.WriteLine("  Starting long operation with 2s timeout...");
            await SlowOperationAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("  ⏱️ Operation timed out!");
        }
    }

    private static async Task SlowOperationAsync(CancellationToken ct)
    {
        await Task.Delay(5000, ct);  // 5 seconds - will timeout
    }
}

public class Example4_ParallelWithCancellation
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Parallel Tasks with Cancellation ===");

        var cts = new CancellationTokenSource();
        cts.CancelAfter(1500);

        try
        {
            var t1 = Task.Delay(1000, cts.Token);
            var t2 = Task.Delay(2000, cts.Token);
            var t3 = Task.Delay(3000, cts.Token);

            await Task.WhenAll(t1, t2, t3);
            Console.WriteLine("  All completed");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("  Some tasks cancelled");
        }
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ADVANCED PATTERNS & DEADLOCKS                           ║");
        Console.WriteLine("║   CancellationToken, ConfigureAwait, Timeout              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_CancellationToken.Run();
        await Example2_ConfigureAwait.Run();
        await Example3_Timeout.Run();
        await Example4_ParallelWithCancellation.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
