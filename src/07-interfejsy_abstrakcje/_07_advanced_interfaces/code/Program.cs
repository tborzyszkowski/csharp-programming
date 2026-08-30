using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdvancedInterfaces;

// ============================================================================
// CZĘŚĆ 1: Default Interface Members (C# 8.0+)
// ============================================================================

/// <summary>
/// C# 8.0: Interface can have default implementation
/// Adding new method to interface doesn't break existing implementations
/// Backward compatibility!
/// </summary>
public interface ILoggerV1
{
    void Log(string message);
    
    // ✅ C# 8.0+: Default implementation in interface
    public void LogInfo(string info)
    {
        Console.WriteLine($"[INFO] {info}");
    }
    
    public void LogWarning(string warning)
    {
        Console.WriteLine($"[WARN] {warning}");
    }
}

public class ConsoleLogger : ILoggerV1
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
    
    // ✅ Does NOT need to implement LogInfo/LogWarning
    // Uses default implementation from interface
}

public class FileLogger : ILoggerV1
{
    public void Log(string message)
    {
        Console.WriteLine($"[FILE] {message}");
    }
    
    // ✅ Can override default implementation if needed
    public void LogInfo(string info)
    {
        Console.WriteLine($"[FILE-INFO] {info}");
    }
}

// ============================================================================
// CZĘŚĆ 2: Interface Versioning - Adding Methods Safely
// ============================================================================

/// <summary>
/// Problem: Adding new method to interface breaks all existing implementations
/// Solution: Provide default implementation
/// </summary>

// Version 1 - Old interface
public interface IRepositoryV1
{
    List<string> GetAll();
    string? GetById(int id);
}

// Version 2 - New interface with additional method
// ❌ OLD WAY: All existing implementations must implement GetCount
// public interface IRepositoryV2 : IRepositoryV1
// {
//     int GetCount();  // ← breaks all existing code!
// }

// ✅ NEW WAY: Provide default implementation
public interface IRepositoryV2 : IRepositoryV1
{
    // ✅ Default implementation - existing classes don't break
    public int GetCount()
    {
        var items = GetAll();
        return items.Count;
    }
    
    // ✅ Another new method with default implementation
    public bool Exists()
    {
        return GetCount() > 0;
    }
}

public class UserRepository : IRepositoryV2
{
    private List<string> _users = new() { "Alice", "Bob", "Charlie" };
    
    public List<string> GetAll() => _users;
    public string? GetById(int id) => id >= 0 && id < _users.Count ? _users[id] : null;
    
    // ✅ Inherits GetCount() and Exists() from interface
}

// ============================================================================
// CZĘŚĆ 3: Static Abstract Members (C# 11+)
// ============================================================================

/// <summary>
/// C# 11.0: Interfaces can have static abstract members
/// Forces implementing classes to have static methods
/// </summary>
public interface IConverterV3
{
    // ✅ Static abstract method (C# 11+)
    static abstract double Convert(double value);
    
    // Static method implementations
    static virtual double ConvertBack(double value)
    {
        return value;  // Default implementation
    }
}

public class FahrenheitToCelsius : IConverterV3
{
    // ✅ Must implement static abstract method
    public static double Convert(double fahrenheit)
    {
        return (fahrenheit - 32) * 5 / 9;
    }
    
    public static double ConvertBack(double celsius)
    {
        return celsius * 9 / 5 + 32;
    }
}

public class MilesToKilometers : IConverterV3
{
    public static double Convert(double miles)
    {
        return miles * 1.60934;
    }
    
    // Uses default implementation of ConvertBack
}

// Generic method using static abstract interface
public static class Converter
{
    public static double ConvertValue<T>(double value) where T : IConverterV3
    {
        return T.Convert(value);
    }
}

// ============================================================================
// CZĘŚĆ 4: Access Modifiers in Interfaces (C# 11+)
// ============================================================================

/// <summary>
/// C# 11.0: Interface members can be private (or protected, internal)
/// Allows internal helper methods
/// </summary>
public interface IDataProcessor
{
    void ProcessData();
    
    // ✅ Private helper method (C# 11+)
    private void LogMessage(string message)
    {
        Console.WriteLine($"[INTERNAL] {message}");
    }
    
    // ✅ Protected method (C# 11+)
    protected void ValidateData()
    {
        Console.WriteLine("Validating...");
    }
    
    // ✅ Public default implementation
    public void Process()
    {
        LogMessage("Processing started");
        ValidateData();
        ProcessData();
        LogMessage("Processing completed");
    }
}

public class DataProcessor : IDataProcessor
{
    public void ProcessData()
    {
        Console.WriteLine("Processing data...");
    }
    
    // ✅ Inherits private LogMessage and protected ValidateData
    // ✅ Inherits public Process()
}

// ============================================================================
// CZĘŚĆ 5: Real-World: Plugin System
// ============================================================================

public interface IPluginV2
{
    string Name { get; }
    
    // ✅ Default implementation
    public void Initialize()
    {
        Console.WriteLine($"Initializing {Name}...");
    }
    
    // ✅ Abstract method - must override
    void Execute();
    
    // ✅ Static configuration
    static virtual int Priority => 0;
    
    // ✅ Private helper
    private void Log(string msg)
    {
        Console.WriteLine($"[{Name}] {msg}");
    }
}

public class LoggingPlugin : IPluginV2
{
    public string Name => "LoggingPlugin";
    public static int Priority => 10;  // Higher priority
    
    public void Execute()
    {
        Console.WriteLine("Logging plugin executed");
    }
    
    // Uses default Initialize()
}

public class MetricsPlugin : IPluginV2
{
    public string Name => "MetricsPlugin";
    // Uses default Priority (0)
    
    public void Execute()
    {
        Console.WriteLine("Metrics plugin executed");
    }
}

// ============================================================================
// CZĘŚĆ 6: Combining Features - Modern Interface Design
// ============================================================================

/// <summary>
/// Modern interface: default impl + static abstract + access modifiers
/// </summary>
public interface IAdvancedService
{
    // Concrete property
    string ServiceName { get; }
    
    // Abstract method
    void DoWork();
    
    // Default implementation
    public void Start()
    {
        LogInternal("Starting service");
        ValidateConfiguration();
    }
    
    // Static abstract (C# 11+)
    static abstract string GetVersion();
    
    // Private helper (C# 11+)
    private void LogInternal(string message)
    {
        Console.WriteLine($"[{ServiceName}] {message}");
    }
    
    // Protected helper (C# 11+)
    protected void ValidateConfiguration()
    {
        Console.WriteLine("Validating configuration...");
    }
}

public class WebService : IAdvancedService
{
    public string ServiceName => "WebService";
    
    public static string GetVersion() => "1.0.0";
    
    public void DoWork()
    {
        Console.WriteLine("Web service working...");
    }
    
    // Inherits Start(), LogInternal(), ValidateConfiguration()
}

// ============================================================================
// TESTS
// ============================================================================

public class AdvancedInterfacesTests
{
    [Fact]
    public void DefaultInterfaceImplementation()
    {
        ILoggerV1 console = new ConsoleLogger();
        ILoggerV1 file = new FileLogger();
        
        // All can call default methods
        console.Log("Console log");
        console.LogInfo("Console info");  // Default implementation
        
        file.Log("File log");
        file.LogInfo("File info");  // Overridden implementation
        
        Assert.NotNull(console);
        Assert.NotNull(file);
    }
    
    [Fact]
    public void InterfaceVersioning()
    {
        IRepositoryV2 repo = new UserRepository();
        
        // Old methods
        var users = repo.GetAll();
        Assert.Equal(3, users.Count);
        
        // New methods with default implementation
        int count = repo.GetCount();  // Default implementation from interface
        Assert.Equal(3, count);
        
        bool exists = repo.Exists();  // Default implementation from interface
        Assert.True(exists);
    }
    
    [Fact]
    public void StaticAbstractMembers()
    {
        // ✅ Can call static methods on implementing types
        double celsius = FahrenheitToCelsius.Convert(32);
        Assert.Equal(0, celsius, 1);  // ~0°C
        
        double km = MilesToKilometers.Convert(1);
        Assert.True(km > 1.6 && km < 1.7);
    }
    
    [Fact]
    public void StaticAbstractWithGenerics()
    {
        double celsius = Converter.ConvertValue<FahrenheitToCelsius>(32);
        Assert.Equal(0, celsius, 1);
        
        double km = Converter.ConvertValue<MilesToKilometers>(1);
        Assert.True(km > 1.6);
    }
    
    [Fact]
    public void PrivateMethodsInInterface()
    {
        IDataProcessor processor = new DataProcessor();
        processor.Process();  // Uses private LogMessage internally
        
        Assert.NotNull(processor);
    }
    
    [Fact]
    public void ModernPluginSystem()
    {
        List<IPluginV2> plugins = new()
        {
            new LoggingPlugin(),
            new MetricsPlugin()
        };
        
        foreach (var plugin in plugins)
        {
            plugin.Initialize();  // Default implementation
            plugin.Execute();
        }
        
        Assert.Equal(2, plugins.Count);
    }
    
    [Fact]
    public void AdvancedServiceIntegration()
    {
        IAdvancedService service = new WebService();
        
        // Uses default Start() which calls private LogInternal
        service.Start();
        service.DoWork();
        
        // Can call static abstract method
        string version = WebService.GetVersion();
        Assert.Equal("1.0.0", version);
    }
}

// ============================================================================
// MAIN
// ============================================================================

class Program
{
    static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║  Advanced Interfaces - C# 8.0+ Features   ║");
        Console.WriteLine("╚════════════════════════════════════════════╝\n");
        
        // Demo 1: Default Implementation
        Console.WriteLine("▶ Demo 1: Default Interface Members (C# 8.0+)\n");
        ILoggerV1 logger = new ConsoleLogger();
        logger.Log("Message");
        logger.LogInfo("Info message");  // Default implementation
        logger.LogWarning("Warning");
        
        // Demo 2: Interface Versioning
        Console.WriteLine("\n▶ Demo 2: Interface Versioning (Backward Compatibility)\n");
        IRepositoryV2 repo = new UserRepository();
        Console.WriteLine($"Total users: {repo.GetCount()}");
        Console.WriteLine($"Exists: {repo.Exists()}");
        
        // Demo 3: Static Abstract Members
        Console.WriteLine("\n▶ Demo 3: Static Abstract Members (C# 11+)\n");
        double c = FahrenheitToCelsius.Convert(212);
        Console.WriteLine($"212°F = {c:F1}°C");
        
        // Demo 4: Access Modifiers
        Console.WriteLine("\n▶ Demo 4: Private Methods in Interface (C# 11+)\n");
        IDataProcessor processor = new DataProcessor();
        processor.Process();
        
        // Demo 5: Plugin System
        Console.WriteLine("\n▶ Demo 5: Modern Plugin System\n");
        var logging = new LoggingPlugin();
        var metrics = new MetricsPlugin();
        
        logging.Initialize();
        logging.Execute();
        
        metrics.Initialize();
        metrics.Execute();
        
        // Demo 6: Advanced Service
        Console.WriteLine("\n▶ Demo 6: Advanced Service with All Features\n");
        IAdvancedService service = new WebService();
        service.Start();
        service.DoWork();
        
    }
}
