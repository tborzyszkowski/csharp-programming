using System;
using Xunit;

namespace StaticPropertiesLazy;

public class AppConfig
{
    public static string ApiUrl { get; private set; } = "http://localhost";
    public static int Timeout { get; private set; } = 30;
    public static bool IsDebug { get; private set; } = true;
    public static readonly string Version = "1.0.0";

    public static void Configure(string apiUrl, int timeout, bool isDebug)
    {
        if (string.IsNullOrWhiteSpace(apiUrl))
            throw new ArgumentException("API URL cannot be empty.", nameof(apiUrl));
        if (timeout <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeout));

        ApiUrl = apiUrl;
        Timeout = timeout;
        IsDebug = isDebug;
    }
}

public class ExpensiveResource
{
    public int Id { get; } = new Random().Next();
    
    private ExpensiveResource()
    {
        Console.WriteLine("Expensive initialization");
    }
    
    private static readonly Lazy<ExpensiveResource> _instance =
        new(() => new ExpensiveResource());
    
    public static ExpensiveResource Instance => _instance.Value;
}

public class Database
{
    private static Lazy<Database> _lazyInstance = new(() => new Database());
    
    public static Database Instance => _lazyInstance.Value;
    
    private Database()
    {
        Console.WriteLine("Database initialized");
    }
    
    public string Connect() => "Connected";
}

public class CachedData
{
    private static readonly Lazy<System.Collections.Generic.Dictionary<string, string>> _cache =
        new(() =>
        {
            var dict = new System.Collections.Generic.Dictionary<string, string>
            {
                { "key1", "value1" },
                { "key2", "value2" },
                { "key3", "value3" }
            };
            Console.WriteLine("Cache initialized");
            return dict;
        });
    
    public static System.Collections.Generic.IReadOnlyDictionary<string, string> Cache => _cache.Value;
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== STATIC PROPERTIES I LAZY<T> ===\n");
        
        Console.WriteLine("1. Static Properties:");
        Console.WriteLine($"   ApiUrl: {AppConfig.ApiUrl}");
        Console.WriteLine($"   Timeout: {AppConfig.Timeout}");
        Console.WriteLine($"   Version: {AppConfig.Version}");
        Console.WriteLine();
        
        Console.WriteLine("2. Lazy<T> - Inicjalizacja przy pierwszym dostępie:");
        Console.WriteLine("   Accessing ExpensiveResource.Instance...");
        var res1 = ExpensiveResource.Instance;
        var res2 = ExpensiveResource.Instance;
        Console.WriteLine($"   res1.Id == res2.Id: {res1.Id == res2.Id}");
        Console.WriteLine();
        
        Console.WriteLine("3. Database Singleton z Lazy<T>:");
        Console.WriteLine("   Accessing Database.Instance...");
        var db = Database.Instance;
        Console.WriteLine($"   Result: {db.Connect()}");
        Console.WriteLine();
        
        Console.WriteLine("4. Cached Data z Lazy<T>:");
        Console.WriteLine("   Accessing CachedData.Cache...");
        var cache = CachedData.Cache;
        Console.WriteLine($"   Cache items: {cache.Count}");
    }
}

public class StaticPropertiesLazyTests
{
    [Fact]
    public void AppConfig_DefaultValues()
    {
        Assert.Equal("http://localhost", AppConfig.ApiUrl);
        Assert.Equal(30, AppConfig.Timeout);
        Assert.True(AppConfig.IsDebug);
    }
    
    [Fact]
    public void AppConfig_CanChangeProperties()
    {
        string original = AppConfig.ApiUrl;
        
        AppConfig.Configure("http://newapi.com", AppConfig.Timeout, AppConfig.IsDebug);
        Assert.Equal("http://newapi.com", AppConfig.ApiUrl);
        
        AppConfig.Configure(original, AppConfig.Timeout, AppConfig.IsDebug);
    }
    
    [Fact]
    public void AppConfig_ReadonlyProperty()
    {
        Assert.Equal("1.0.0", AppConfig.Version);
        // AppConfig.Version = "2.0.0";  // Compilation error
    }
    
    [Fact]
    public void ExpensiveResource_Lazy_ReturnsSameInstance()
    {
        var res1 = ExpensiveResource.Instance;
        var res2 = ExpensiveResource.Instance;
        
        Assert.Same(res1, res2);
        Assert.Equal(res1.Id, res2.Id);
    }
    
    [Fact]
    public void Database_Lazy_Initialized()
    {
        var db = Database.Instance;
        
        Assert.NotNull(db);
        Assert.Equal("Connected", db.Connect());
    }
    
    [Fact]
    public void CachedData_Lazy_InitializedOnce()
    {
        var cache1 = CachedData.Cache;
        var cache2 = CachedData.Cache;
        
        Assert.Same(cache1, cache2);
        Assert.Equal(3, cache1.Count);
    }
}
