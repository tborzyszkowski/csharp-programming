using System;
using System.Threading.Tasks;

namespace BreakfastConcurrent;

public class Example1_Sequential
{
    private static async Task<string> CookEggsAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Eggs";
    }

    private static async Task<string> ToastBreadAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Toast";
    }

    private static async Task<string> BrewCoffeeAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Coffee";
    }

    private static async Task<string> CookBaconAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Bacon";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Sequential Breakfast ===");
        Console.WriteLine("  Timeline:");
        Console.WriteLine("    Eggs   [====3000ms====]");
        Console.WriteLine("    Bread      [==2000ms==]");
        Console.WriteLine("    Coffee        [=1500ms=]");
        Console.WriteLine("    Bacon          [1000ms]");
        Console.WriteLine("  Expected: 7500ms total");

        var start = DateTime.Now;
        
        var eggs = await CookEggsAsync(3000);
        var bread = await ToastBreadAsync(2000);
        var coffee = await BrewCoffeeAsync(1500);
        var bacon = await CookBaconAsync(1000);

        var elapsed = (DateTime.Now - start).TotalSeconds;
        
        Console.WriteLine($"  Result: {eggs}, {bread}, {coffee}, {bacon}");
        Console.WriteLine($"  Actual time: {elapsed:F1}s");
        Console.WriteLine($"  ❌ INEFFICIENT - waited for each task sequentially!");
    }
}

public class Example2_Concurrent
{
    private static async Task<string> CookEggsAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Eggs";
    }

    private static async Task<string> ToastBreadAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Toast";
    }

    private static async Task<string> BrewCoffeeAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Coffee";
    }

    private static async Task<string> CookBaconAsync(int timeMs)
    {
        await Task.Delay(timeMs);
        return "Bacon";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Concurrent Breakfast ===");
        Console.WriteLine("  Timeline:");
        Console.WriteLine("    Eggs   [====3000ms====]");
        Console.WriteLine("    Bread  [==2000ms==]");
        Console.WriteLine("    Coffee [=1500ms=]");
        Console.WriteLine("    Bacon  [1000ms]");
        Console.WriteLine("  Expected: 3000ms total (only longest task)");

        var start = DateTime.Now;

        // Start all tasks IMMEDIATELY (don't await yet)
        var eggsTask = CookEggsAsync(3000);
        var breadTask = ToastBreadAsync(2000);
        var coffeeTask = BrewCoffeeAsync(1500);
        var baconTask = CookBaconAsync(1000);

        // Now wait for ALL to complete
        await Task.WhenAll(eggsTask, breadTask, coffeeTask, baconTask);

        var elapsed = (DateTime.Now - start).TotalSeconds;

        Console.WriteLine($"  Result: {eggsTask.Result}, {breadTask.Result}, {coffeeTask.Result}, {baconTask.Result}");
        Console.WriteLine($"  Actual time: {elapsed:F1}s");
        Console.WriteLine($"  ✅ EFFICIENT - all tasks ran concurrently!");
    }
}

public class Example3_Comparison
{
    private static async Task<string> OperationAsync(string name, int timeMs)
    {
        await Task.Delay(timeMs);
        return $"{name}({timeMs}ms)";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Sequential vs Concurrent Comparison ===");

        // Sequential
        var start = DateTime.Now;
        var r1 = await OperationAsync("Op1", 1000);
        var r2 = await OperationAsync("Op2", 1000);
        var r3 = await OperationAsync("Op3", 1000);
        var seqTime = (DateTime.Now - start).TotalSeconds;
        
        Console.WriteLine($"  Sequential: {seqTime:F1}s (sum of all)");

        // Concurrent
        start = DateTime.Now;
        var t1 = OperationAsync("Op1", 1000);
        var t2 = OperationAsync("Op2", 1000);
        var t3 = OperationAsync("Op3", 1000);
        await Task.WhenAll(t1, t2, t3);
        var concTime = (DateTime.Now - start).TotalSeconds;

        Console.WriteLine($"  Concurrent: {concTime:F1}s (max of all)");
        Console.WriteLine($"  Speedup: {seqTime/concTime:F1}x faster!");
    }
}

public class Example4_PartialWait
{
    private static async Task<string> FetchAsync(string source, int delayMs)
    {
        await Task.Delay(delayMs);
        return $"Data from {source}";
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Task.WhenAny - First Completed ===");

        var start = DateTime.Now;

        var task1 = FetchAsync("Source1", 3000);
        var task2 = FetchAsync("Source2", 1000);  // Fastest
        var task3 = FetchAsync("Source3", 2000);

        // Wait only for first to complete
        var completedTask = await Task.WhenAny(task1, task2, task3);

        Console.WriteLine($"  First result: {completedTask.Result}");
        Console.WriteLine($"  Time to first: {(DateTime.Now - start).TotalSeconds:F1}s");

        // Then wait for all others
        await Task.WhenAll(task1, task2, task3);
        Console.WriteLine($"  Time to all: {(DateTime.Now - start).TotalSeconds:F1}s");
    }
}

public class Example5_RealWorldScenario
{
    public class BreakfastService
    {
        public async Task<string> CookAsync(string item, int timeMs)
        {
            await Task.Delay(timeMs);
            return item;
        }

        public async Task<string> PrepareAsync()
        {
            Console.WriteLine("  Starting breakfast preparation...");
            var start = DateTime.Now;

            var eggs = CookAsync("Scrambled Eggs", 2000);
            var toast = CookAsync("Toasted Bread", 1500);
            var juice = CookAsync("Orange Juice", 1000);
            var bacon = CookAsync("Crispy Bacon", 2500);

            await Task.WhenAll(eggs, toast, juice, bacon);

            var elapsed = (DateTime.Now - start).TotalSeconds;

            return $"Breakfast ready! ({elapsed:F1}s)\n" +
                   $"  - {eggs.Result}\n" +
                   $"  - {toast.Result}\n" +
                   $"  - {juice.Result}\n" +
                   $"  - {bacon.Result}";
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Real-World Breakfast Service ===");

        var service = new BreakfastService();
        var result = await service.PrepareAsync();
        Console.WriteLine($"  {result}");
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   BREAKFAST: SEQUENTIAL vs CONCURRENT                      ║");
        Console.WriteLine("║   Practical Example of Async Patterns                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_Sequential.Run();
        await Example2_Concurrent.Run();
        await Example3_Comparison.Run();
        await Example4_PartialWait.Run();
        await Example5_RealWorldScenario.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
