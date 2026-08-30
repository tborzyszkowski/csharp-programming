using System;
using System.Threading.Tasks;

namespace CPUBound;

public class Example1_TaskRun
{
    private static int Fibonacci(int n)
    {
        if (n <= 1) return n;
        return Fibonacci(n - 1) + Fibonacci(n - 2);
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Task.Run for CPU-Bound ===");

        var start = DateTime.Now;
        var result = await Task.Run(() => Fibonacci(35));
        var elapsed = (DateTime.Now - start).TotalSeconds;

        Console.WriteLine($"  Fibonacci(35) = {result}");
        Console.WriteLine($"  Time: {elapsed:F2}s");
        Console.WriteLine($"  ✓ Main thread was free during computation!");
    }
}

public class Example2_LongCalculation
{
    private static long Calculate(int iterations)
    {
        long sum = 0;
        for (int i = 0; i < iterations; i++)
        {
            sum += Math.Sqrt(i);
        }
        return sum;
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Long Calculation ===");

        var start = DateTime.Now;

        // Offload to thread pool
        var result = await Task.Run(() => Calculate(100_000_000));

        var elapsed = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"  Result: {result}");
        Console.WriteLine($"  Time: {elapsed:F2}s");
    }
}

public class Example3_MultipleCalculations
{
    private static int SlowFib(int n)
    {
        if (n <= 1) return n;
        return SlowFib(n - 1) + SlowFib(n - 2);
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Parallel CPU-Bound Tasks ===");

        var start = DateTime.Now;

        var t1 = Task.Run(() => SlowFib(30));
        var t2 = Task.Run(() => SlowFib(30));
        var t3 = Task.Run(() => SlowFib(30));

        var results = await Task.WhenAll(t1, t2, t3);

        var elapsed = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"  Results: {string.Join(", ", results)}");
        Console.WriteLine($"  Time: {elapsed:F2}s (parallelized on multiple cores)");
    }
}

public class Example4_CancellationToken
{
    private static bool IsPrime(long number)
    {
        if (number < 2) return false;
        for (long i = 2; i * i <= number; i++)
        {
            if (number % i == 0) return false;
        }
        return true;
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Long Computation with Monitoring ===");

        var start = DateTime.Now;

        // Check if large number is prime
        var isPrime = await Task.Run(() => IsPrime(982_451_653));

        var elapsed = (DateTime.Now - start).TotalSeconds;
        Console.WriteLine($"  Is 982451653 prime? {isPrime}");
        Console.WriteLine($"  Time: {elapsed:F2}s");
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   CPU-BOUND OPERATIONS                                    ║");
        Console.WriteLine("║   Task.Run, Thread Pool, Parallel Processing              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_TaskRun.Run();
        await Example2_LongCalculation.Run();
        await Example3_MultipleCalculations.Run();
        await Example4_CancellationToken.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
