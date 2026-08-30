using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection;

public interface IDataService
{
    Task<string> GetDataAsync();
}

public class DataService : IDataService
{
    private string? _data;

    public async Task InitializeAsync()
    {
        await Task.Delay(500);
        _data = "Initialized data";
    }

    public async Task<string> GetDataAsync()
    {
        await Task.Delay(100);
        return _data ?? "No data";
    }
}

public class Example1_SimpleAsyncInit
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Simple Async Initialization ===");

        var service = new DataService();
        await service.InitializeAsync();

        var data = await service.GetDataAsync();
        Console.WriteLine($"  Data: {data}");
    }
}

public class Example2_DependencyInjection
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Dependency Injection ===");

        var services = new ServiceCollection();
        services.AddScoped<IDataService, DataService>();
        var provider = services.BuildServiceProvider();

        var service = provider.GetRequiredService<IDataService>();
        var data = await service.GetDataAsync();
        Console.WriteLine($"  Data: {data}");
    }
}

public class Example3_LazyAsync
{
    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Lazy<Task<T>> Pattern ===");

        var lazyService = new Lazy<Task<DataService>>(async () =>
        {
            Console.WriteLine("    Lazy: Creating service...");
            var service = new DataService();
            await service.InitializeAsync();
            return service;
        });

        Console.WriteLine("  Service not created yet");
        
        var service = await lazyService.Value;
        Console.WriteLine("  Service created on first access");

        var data = await service.GetDataAsync();
        Console.WriteLine($"  Data: {data}");
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   DEPENDENCY INJECTION + ASYNC                            ║");
        Console.WriteLine("║   Async Init, Lazy<Task<T>>, Microsoft.Extensions.DI      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        await Example1_SimpleAsyncInit.Run();
        await Example2_DependencyInjection.Run();
        await Example3_LazyAsync.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
