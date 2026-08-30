using System;
using System.Collections.Generic;
using Xunit;

namespace ReadonlyConst;

// ============ CONST - COMPILE TIME ============
public class MathConstants
{
    public const double PI = 3.14159265359;
    public const double E = 2.71828182846;
    public const int DAYS_IN_WEEK = 7;
    public const string APP_VERSION = "1.0.0";
}

// ============ READONLY - RUNTIME ============
public class Configuration
{
    public readonly string ConfigPath;
    public readonly int MaxRetries;
    public readonly bool EnableLogging;
    
    public Configuration(string path, int retries = 3, bool logging = true)
    {
        ConfigPath = path;
        MaxRetries = retries;
        EnableLogging = logging;
    }
}

// ============ STATIC READONLY ============
public class Database
{
    public static readonly Database Instance = new();
    
    private Database()
    {
        Console.WriteLine("[DB] Singleton created");
    }
    
    public void Query(string sql)
    {
        Console.WriteLine($"[DB] Executing: {sql}");
    }
}

// ============ READONLY COLLECTION ============
public class AppSettings
{
    public readonly List<string> AllowedHosts;
    public readonly Dictionary<string, string> AppConfig;
    
    public AppSettings()
    {
        AllowedHosts = new List<string> { "localhost", "127.0.0.1" };
        AppConfig = new Dictionary<string, string>
        {
            { "theme", "dark" },
            { "language", "pl" }
        };
    }
}

// ============ CONST vs READONLY COMPARISON ============
public class ComparisonExample
{
    public const int ConstValue = 42;  // Compile-time
    public readonly int ReadonlyValue;  // Runtime
    
    public ComparisonExample(int value)
    {
        ReadonlyValue = value;
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 5: READONLY vs CONST ===\n");
        
        Console.WriteLine("1. CONST (COMPILE-TIME):");
        Console.WriteLine($"PI = {MathConstants.PI}");
        Console.WriteLine($"E = {MathConstants.E}");
        Console.WriteLine($"DAYS_IN_WEEK = {MathConstants.DAYS_IN_WEEK}");
        Console.WriteLine($"APP_VERSION = {MathConstants.APP_VERSION}");
        // MathConstants.PI = 3.14;  // BŁĄD - const
        Console.WriteLine();
        
        Console.WriteLine("2. READONLY (RUNTIME):");
        var config = new Configuration("C:\\app.json", 5);
        Console.WriteLine($"ConfigPath: {config.ConfigPath}");
        Console.WriteLine($"MaxRetries: {config.MaxRetries}");
        Console.WriteLine($"EnableLogging: {config.EnableLogging}");
        // config.ConfigPath = "D:\\other.json";  // BŁĄD - readonly
        Console.WriteLine();
        
        Console.WriteLine("3. STATIC READONLY (SINGLETON):");
        var db1 = Database.Instance;
        var db2 = Database.Instance;
        Console.WriteLine($"Same instance: {ReferenceEquals(db1, db2)}");
        db1.Query("SELECT * FROM Users");
        Console.WriteLine();
        
        Console.WriteLine("4. READONLY COLLECTION:");
        var settings = new AppSettings();
        Console.WriteLine($"Hosts: {string.Join(", ", settings.AllowedHosts)}");
        Console.WriteLine($"Theme: {settings.AppConfig["theme"]}");
        settings.AllowedHosts.Add("example.com");  // Można zmienić zawartość!
        Console.WriteLine($"After adding: {string.Join(", ", settings.AllowedHosts)}");
        // settings.AllowedHosts = new List<string>();  // BŁĄD - readonly
        Console.WriteLine();
        
        Console.WriteLine("5. PORÓWNANIE:");
        var example1 = new ComparisonExample(100);
        var example2 = new ComparisonExample(200);
        Console.WriteLine($"ComparisonExample.ConstValue: {ComparisonExample.ConstValue}");
        Console.WriteLine($"example1.ReadonlyValue: {example1.ReadonlyValue}");
        Console.WriteLine($"example2.ReadonlyValue: {example2.ReadonlyValue}");
        Console.WriteLine("(Notice: ConstValue is static, so both instances have same value)");
    }
}

public class ReadonlyConstTests
{
    [Fact]
    public void Const_CannotModify()
    {
        // MathConstants.PI = 3.14;  // Compilation error
        Assert.Equal(3.14159265359, MathConstants.PI, precision: 10);
    }
    
    [Fact]
    public void Readonly_CanBeSetInConstructor()
    {
        var config = new Configuration("test.json", 5);
        
        Assert.Equal("test.json", config.ConfigPath);
        Assert.Equal(5, config.MaxRetries);
    }
    
    [Fact]
    public void Readonly_CannotModifyAfterConstructor()
    {
        var config = new Configuration("test.json");
        
        // config.ConfigPath = "other.json";  // Compilation error
        Assert.Equal("test.json", config.ConfigPath);
    }
    
    [Fact]
    public void StaticReadonly_IsSingleton()
    {
        var db1 = Database.Instance;
        var db2 = Database.Instance;
        
        Assert.Same(db1, db2);
    }
    
    [Fact]
    public void ReadonlyCollection_CanModifyContents()
    {
        var settings = new AppSettings();
        var initialCount = settings.AllowedHosts.Count;
        
        settings.AllowedHosts.Add("newhost.com");
        
        Assert.Equal(initialCount + 1, settings.AllowedHosts.Count);
    }
    
    [Fact]
    public void ConstValues_StaticByDefault()
    {
        var ex1 = new ComparisonExample(100);
        var ex2 = new ComparisonExample(200);
        
        // Const is static - access via type, not instance
        Assert.Equal(42, ComparisonExample.ConstValue);
        
        // Different readonly values (runtime) - per instance
        Assert.NotEqual(ex1.ReadonlyValue, ex2.ReadonlyValue);
    }
}
