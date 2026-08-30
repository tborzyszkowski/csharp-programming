using System;
using System.Collections.Generic;
using Xunit;

namespace StaticConstructor;

public class Database
{
    public static string ConnectionString;
    public static int Timeout;
    
    static Database()
    {
        ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "localhost";
        Timeout = int.Parse(Environment.GetEnvironmentVariable("DB_TIMEOUT") ?? "30");
    }
}

public class AppSettings
{
    public static Dictionary<string, string> Values = new();
    
    static AppSettings()
    {
        Values["app_name"] = "MyApp";
        Values["version"] = "1.0.0";
        Values["debug"] = "true";
    }
}

public class Logger
{
    private static Logger _instance = null!;
    
    static Logger()
    {
        _instance = new Logger();
    }
    
    public static Logger Instance => _instance;
    private List<string> logs = new();
    
    public void Log(string message) => logs.Add(message);
}

public class Counter
{
    public static int Count = 0;
    
    static Counter()
    {
        Count = 100;
    }
}

public class InitOrder
{
    public static string Name = "Field";
    
    static InitOrder()
    {
        Name = "StaticConstructor";
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== KONSTRUKTOR STATYCZNY ===\n");
        
        Console.WriteLine("1. Database config:");
        Console.WriteLine($"   ConnectionString: {Database.ConnectionString}");
        Console.WriteLine($"   Timeout: {Database.Timeout}");
        Console.WriteLine();
        
        Console.WriteLine("2. App Settings:");
        foreach (var kvp in AppSettings.Values)
            Console.WriteLine($"   {kvp.Key}: {kvp.Value}");
        Console.WriteLine();
        
        Console.WriteLine("3. Logger Singleton:");
        var logger = Logger.Instance;
        logger.Log("Test");
        Console.WriteLine($"   Logger instance created by static constructor");
        Console.WriteLine();
        
        Console.WriteLine("4. Init Order:");
        Console.WriteLine($"   InitOrder.Name: {InitOrder.Name}");
        Console.WriteLine();
        
        Console.WriteLine("5. Counter:");
        Console.WriteLine($"   Counter.Count: {Counter.Count}");
    }
}

public class StaticConstructorTests
{
    [Fact]
    public void Database_InitializedByStaticConstructor()
    {
        Assert.NotNull(Database.ConnectionString);
        Assert.True(Database.Timeout > 0);
    }
    
    [Fact]
    public void AppSettings_PopulatedByStaticConstructor()
    {
        Assert.Equal(3, AppSettings.Values.Count);
        Assert.Equal("MyApp", AppSettings.Values["app_name"]);
    }
    
    [Fact]
    public void Logger_CreatedByStaticConstructor()
    {
        Assert.NotNull(Logger.Instance);
    }
    
    [Fact]
    public void Counter_InitializedTo100()
    {
        Assert.Equal(100, Counter.Count);
    }
    
    [Fact]
    public void InitOrder_FieldOverriddenByStaticConstructor()
    {
        Assert.Equal("StaticConstructor", InitOrder.Name);
    }
}
