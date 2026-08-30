using System;
using Xunit;

namespace SingletonPattern;

// Klasyczne - Double-checked locking
public class Database
{
    private static Database? _instance;
    private static readonly object _lock = new();
    
    private Database() { }
    
    public static Database GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }
            }
        }
        return _instance;
    }
    
    public string Connect() => "Connected to Database";
}

// Nowoczesne - Lazy<T>
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = 
        new(() => new Logger());
    
    private Logger() { }
    
    public static Logger Instance => _instance.Value;
    
    public void Log(string message) 
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

// Static constructor pattern
public sealed class ConfigManager
{
    private static ConfigManager? _instance;
    
    static ConfigManager()
    {
        _instance = new ConfigManager();
    }
    
    public static ConfigManager Instance => _instance!;
    
    public string GetConfig(string key) => $"Value of {key}";
}

// Property pattern
public sealed class Cache
{
    private static readonly Lazy<Cache> _lazy = 
        new(() => new Cache());
    
    public static Cache Instance => _lazy.Value;
    
    private Cache() { }
    
    public void Set(string key, string value) 
    { 
        Console.WriteLine($"Cache: {key} = {value}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== SINGLETON PATTERN ===\n");
        
        Console.WriteLine("1. Database (Double-checked locking):");
        var db1 = Database.GetInstance();
        var db2 = Database.GetInstance();
        Console.WriteLine($"   db1 == db2: {ReferenceEquals(db1, db2)}");
        Console.WriteLine();
        
        Console.WriteLine("2. Logger (Lazy<T>):");
        var logger1 = Logger.Instance;
        var logger2 = Logger.Instance;
        logger1.Log("Test message");
        Console.WriteLine($"   logger1 == logger2: {ReferenceEquals(logger1, logger2)}");
        Console.WriteLine();
        
        Console.WriteLine("3. ConfigManager (Static constructor):");
        var config = ConfigManager.Instance;
        Console.WriteLine($"   Config: {config.GetConfig("db_host")}");
        Console.WriteLine();
        
        Console.WriteLine("4. Cache (Property pattern):");
        var cache = Cache.Instance;
        cache.Set("user_1", "John");
    }
}

public class SingletonPatternTests
{
    [Fact]
    public void Database_ReturnsSameInstance()
    {
        var db1 = Database.GetInstance();
        var db2 = Database.GetInstance();
        
        Assert.Same(db1, db2);
    }
    
    [Fact]
    public void Logger_ReturnsSameInstance()
    {
        var logger1 = Logger.Instance;
        var logger2 = Logger.Instance;
        
        Assert.Same(logger1, logger2);
    }
    
    [Fact]
    public void ConfigManager_ReturnsSameInstance()
    {
        var config1 = ConfigManager.Instance;
        var config2 = ConfigManager.Instance;
        
        Assert.Same(config1, config2);
    }
    
    [Fact]
    public void Cache_ReturnsSameInstance()
    {
        var cache1 = Cache.Instance;
        var cache2 = Cache.Instance;
        
        Assert.Same(cache1, cache2);
    }
    
    [Fact]
    public void Database_CannotCreateDirectly()
    {
        // new Database();  // Private constructor - cannot compile
        Assert.NotNull(Database.GetInstance());
    }
}
