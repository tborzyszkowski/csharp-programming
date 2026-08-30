using System;
using System.Threading.Tasks;

namespace AsyncAwaitFundamentals;

// ==================== EXAMPLE 1: SYNCHRONOUS vs ASYNCHRONOUS ====================

public class Example1_SyncVsAsync
{
    // Synchronicznie (blokuje thread)
    public static void SyncVersion()
    {
        Console.WriteLine("  [Sync] Start");
        System.Threading.Thread.Sleep(2000);  // Blokuje na 2 sekundy!
        Console.WriteLine("  [Sync] After 2s");
    }

    // Asynchronicznie (nie blokuje thread)
    public static async Task AsyncVersion()
    {
        Console.WriteLine("  [Async] Start");
        await Task.Delay(2000);  // Thread wolny!
        Console.WriteLine("  [Async] After 2s");
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Synchronous vs Asynchronous ===");
        
        // Synchronicznie - 2 sekundy czekania
        Console.WriteLine("  Synchronous version:");
        var start = DateTime.Now;
        SyncVersion();
        Console.WriteLine($"  Duration: {(DateTime.Now - start).TotalSeconds:F1}s");

        // Asynchronicznie - 0 sekund czekania (thread jest wolny)
        Console.WriteLine("\n  Asynchronous version:");
        start = DateTime.Now;
        await AsyncVersion();
        Console.WriteLine($"  Duration: {(DateTime.Now - start).TotalSeconds:F1}s");
    }
}

// ==================== EXAMPLE 2: TASK<T> RETURN VALUE ====================

public class Example2_TaskWithReturnValue
{
    // Zwraca Task<string>
    public static async Task<string> FetchDataAsync(int delayMs)
    {
        await Task.Delay(delayMs);
        return $"Data fetched after {delayMs}ms";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Task<T> Return Value ===");

        Console.WriteLine("  Fetching data...");
        var data = await FetchDataAsync(1500);
        Console.WriteLine($"  Result: {data}");

        // Alternatywa: bez await (Task bez wartości)
        var task = FetchDataAsync(1000);
        Console.WriteLine("  Task created but not awaited yet");
        var result = await task;
        Console.WriteLine($"  Result: {result}");
    }
}

// ==================== EXAMPLE 3: TASK.WHENALL (CONCURRENT) ====================

public class Example3_TaskWhenAll
{
    public static async Task<string> ProcessItemAsync(string item, int delayMs)
    {
        await Task.Delay(delayMs);
        return $"Processed: {item}";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Task.WhenAll - Concurrent Execution ===");

        Console.WriteLine("  Starting 3 tasks concurrently...");
        var start = DateTime.Now;

        // Wszystkie 3 działają równocześnie!
        var task1 = ProcessItemAsync("Item 1", 1000);
        var task2 = ProcessItemAsync("Item 2", 1000);
        var task3 = ProcessItemAsync("Item 3", 1000);

        var results = await Task.WhenAll(task1, task2, task3);

        Console.WriteLine($"  Total time: {(DateTime.Now - start).TotalSeconds:F1}s");
        Console.WriteLine("  Results:");
        foreach (var result in results)
        {
            Console.WriteLine($"    - {result}");
        }
    }
}

// ==================== EXAMPLE 4: TASK.WHENANY (FIRST COMPLETED) ====================

public class Example4_TaskWhenAny
{
    public static async Task<string> FetchAsync(string source, int delayMs)
    {
        await Task.Delay(delayMs);
        return $"Data from {source}";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Task.WhenAny - First Completed ===");

        Console.WriteLine("  Fetching from 3 sources (fastest wins)...");
        var start = DateTime.Now;

        var source1 = FetchAsync("Server1", 3000);
        var source2 = FetchAsync("Server2", 1000);  // Fastest
        var source3 = FetchAsync("Server3", 2000);

        // Czeka aż pierwsze się skończy
        var completedTask = await Task.WhenAny(source1, source2, source3);
        
        Console.WriteLine($"  First completed in: {(DateTime.Now - start).TotalSeconds:F1}s");
        Console.WriteLine($"  Result: {completedTask.Result}");

        // Opcjonalnie: czekaj na pozostałe
        Console.WriteLine("  Waiting for all to complete...");
        await Task.WhenAll(source1, source2, source3);
        Console.WriteLine($"  All done in: {(DateTime.Now - start).TotalSeconds:F1}s total");
    }
}

// ==================== EXAMPLE 5: EXCEPTION HANDLING ====================

public class Example5_ExceptionHandling
{
    public static async Task<int> DivideAsync(int a, int b)
    {
        await Task.Delay(500);
        if (b == 0)
            throw new ArgumentException("Dzielenie przez zero!");
        return a / b;
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Exception Handling ===");

        // Try-Catch z async
        try
        {
            Console.WriteLine("  Attempting valid division...");
            var result = await DivideAsync(10, 2);
            Console.WriteLine($"  Result: {result}");

            Console.WriteLine("\n  Attempting invalid division...");
            result = await DivideAsync(10, 0);
            Console.WriteLine($"  Result: {result}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  Exception caught: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("  Cleanup completed");
        }
    }
}

// ==================== MAIN ====================

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ASYNC/AWAIT FUNDAMENTALS                                ║");
        Console.WriteLine("║   TAP, Task, async/await, Composition                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_SyncVsAsync.Run();
        await Example2_TaskWithReturnValue.Run();
        await Example3_TaskWhenAll.Run();
        await Example4_TaskWhenAny.Run();
        await Example5_ExceptionHandling.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
