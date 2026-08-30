using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintHeader();

// Example 1: Basic Stopwatch Benchmark
await RunExample1();

// Example 2: File I/O Benchmark (Sync vs Async)
await RunExample2();

// Example 3: Network-like Operations Benchmark
await RunExample3();

// Example 4: Memory and Allocation Benchmark
await RunExample4();

PrintFooter();

async Task RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic Stopwatch Benchmark ===");
    
    var sw = Stopwatch.StartNew();
    
    // Sync operation - Thread blocked
    await Task.Delay(100);
    
    sw.Stop();
    Console.WriteLine($"  Operation completed in {sw.ElapsedMilliseconds}ms");
    Console.WriteLine($"  Timestamp: {sw.ElapsedTicks} ticks");
}

async Task RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: File I/O Benchmark (Sync vs Async) ===");
    
    const string testFile = "benchmark_test.txt";
    const string testContent = "Hello, Benchmarking World!";
    const int iterations = 100;
    
    try
    {
        // Sync write benchmark
        var swSync = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            File.WriteAllText(testFile, testContent + i);
        }
        swSync.Stop();
        
        // Async write benchmark
        var swAsync = Stopwatch.StartNew();
        var tasks = new Task[iterations];
        for (int i = 0; i < iterations; i++)
        {
            int index = i;
            tasks[i] = File.WriteAllTextAsync(testFile, testContent + index);
        }
        await Task.WhenAll(tasks);
        swAsync.Stop();
        
        Console.WriteLine($"  Sync {iterations} writes:  {swSync.ElapsedMilliseconds}ms");
        Console.WriteLine($"  Async {iterations} writes: {swAsync.ElapsedMilliseconds}ms");
        Console.WriteLine($"  Improvement: {(double)swSync.ElapsedMilliseconds / swAsync.ElapsedMilliseconds:F2}x faster");
        
        File.Delete(testFile);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"  Error: {ex.Message}");
    }
}

async Task RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Simulated Network Benchmark ===");
    
    const int requests = 10;
    
    // Sync approach - sequential requests
    var swSync = Stopwatch.StartNew();
    for (int i = 0; i < requests; i++)
    {
        SimulateNetworkCall(100);  // 100ms per request
    }
    swSync.Stop();
    
    // Async approach - concurrent requests
    var swAsync = Stopwatch.StartNew();
    var tasks = new Task[requests];
    for (int i = 0; i < requests; i++)
    {
        tasks[i] = SimulateNetworkCallAsync(100);  // 100ms per request
    }
    await Task.WhenAll(tasks);
    swAsync.Stop();
    
    Console.WriteLine($"  Sync  {requests} requests (sequential):  {swSync.ElapsedMilliseconds}ms");
    Console.WriteLine($"  Async {requests} requests (concurrent):  {swAsync.ElapsedMilliseconds}ms");
    Console.WriteLine($"  Improvement: {(double)swSync.ElapsedMilliseconds / swAsync.ElapsedMilliseconds:F2}x faster");
}

async Task RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Memory Allocation Benchmark ===");
    
    const int iterations = 10000;
    
    // Measure memory before
    var memBefore = GC.GetTotalMemory(true);
    
    // Sync operations with allocations
    var sw = Stopwatch.StartNew();
    for (int i = 0; i < iterations; i++)
    {
        var result = SyncOperation(i);
    }
    sw.Stop();
    
    var memAfter = GC.GetTotalMemory(false);
    var syncAllocation = memAfter - memBefore;
    var syncTime = sw.ElapsedMilliseconds;
    
    // Async operations with allocations
    memBefore = GC.GetTotalMemory(true);
    sw.Restart();
    var tasks = new Task<int>[iterations];
    for (int i = 0; i < iterations; i++)
    {
        int index = i;
        tasks[i] = AsyncOperation(index);
    }
    await Task.WhenAll(tasks);
    sw.Stop();
    
    memAfter = GC.GetTotalMemory(false);
    var asyncAllocation = memAfter - memBefore;
    var asyncTime = sw.ElapsedMilliseconds;
    
    Console.WriteLine($"  Sync  {iterations} operations:");
    Console.WriteLine($"    Time: {syncTime}ms");
    Console.WriteLine($"    Memory: {syncAllocation / 1024}KB allocated");
    
    Console.WriteLine($"  Async {iterations} operations:");
    Console.WriteLine($"    Time: {asyncTime}ms");
    Console.WriteLine($"    Memory: {asyncAllocation / 1024}KB allocated");
}

void SimulateNetworkCall(int delayMs)
{
    Thread.Sleep(delayMs);
}

async Task SimulateNetworkCallAsync(int delayMs)
{
    await Task.Delay(delayMs);
}

int SyncOperation(int value)
{
    // Simulate some work
    Thread.Sleep(0);
    return value * 2;
}

async Task<int> AsyncOperation(int value)
{
    await Task.Yield();
    return value * 2;
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   BENCHMARKING PERFORMANCE                                         ║");
    Console.WriteLine("║   Async vs Sync Metrics, Stopwatch, Measurement                   ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Example Completed                                              ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}
