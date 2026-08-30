using System;
using System.Threading.Tasks;

namespace TestingAsync;

public class DataService
{
    public async Task<string> GetDataAsync(int delayMs = 100)
    {
        await Task.Delay(delayMs);
        return "Test Data";
    }

    public async Task<int> CalculateAsync(int a, int b)
    {
        await Task.Delay(50);
        return a + b;
    }

    public async Task<string> FailingMethodAsync()
    {
        await Task.Delay(50);
        throw new InvalidOperationException("Test error");
    }
}

public class Example1_BasicAsyncTest
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Basic Async Test ===");

        var service = new DataService();
        var result = await service.GetDataAsync();

        Console.WriteLine($"  Result: {result}");
        Console.WriteLine($"  ✓ Test passed: {(result == "Test Data" ? "PASS" : "FAIL")}");
    }
}

public class Example2_MultipleTests
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Multiple Async Tests ===");

        var service = new DataService();

        // Test 1
        var data = await service.GetDataAsync();
        var test1 = data == "Test Data";

        // Test 2
        var calc = await service.CalculateAsync(5, 3);
        var test2 = calc == 8;

        Console.WriteLine($"  Test 1 (GetData): {(test1 ? "PASS" : "FAIL")}");
        Console.WriteLine($"  Test 2 (Calculate): {(test2 ? "PASS" : "FAIL")}");
    }
}

public class Example3_ExceptionTest
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Testing Exceptions ===");

        var service = new DataService();

        try
        {
            await service.FailingMethodAsync();
            Console.WriteLine("  FAIL: Should have thrown");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  PASS: Caught expected exception: {ex.Message}");
        }
    }
}

public class Example4_TimeoutTest
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Timeout Test ===");

        var service = new DataService();
        var start = DateTime.Now;

        var task = service.GetDataAsync(5000);
        var timeoutTask = Task.Delay(2000);

        var winner = await Task.WhenAny(task, timeoutTask);

        if (winner == timeoutTask)
        {
            Console.WriteLine("  PASS: Operation timed out as expected");
        }
        else
        {
            Console.WriteLine("  FAIL: Operation completed before timeout");
        }
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   TESTING ASYNC CODE                                      ║");
        Console.WriteLine("║   xUnit, Task-returning tests, Timeout Tests              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_BasicAsyncTest.Run();
        await Example2_MultipleTests.Run();
        await Example3_ExceptionTest.Run();
        await Example4_TimeoutTest.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Tests Completed                                    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
