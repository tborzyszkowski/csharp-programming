using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InterfacesIntro;

// ============================================================================
// CZĘŚĆ 1: Basics - Interface Definition
// ============================================================================

/// <summary>
/// C# Interface - kontrakt, WSZYSTKIE elementy są public
/// W C# interfejs może mieć: metody, properties, events
/// W Java: interfejs może mieć TYLKO metody (do Java 8)
/// </summary>
public interface ILogger
{
    // ✅ Public methods
    void Log(string message);
    void Error(string message);
    
    // ✅ Public properties (C# feature, not in older Java)
    string Name { get; set; }
}

/// <summary>
/// ✅ Implementacja interfejsu
/// </summary>
public class ConsoleLogger : ILogger
{
    public string Name { get; set; }
    
    public ConsoleLogger()
    {
        Name = "ConsoleLogger";
    }
    
    public void Log(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }
    
    public void Error(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }
}

public class FileLogger : ILogger
{
    public string Name { get; set; }
    
    public FileLogger()
    {
        Name = "FileLogger";
    }
    
    public void Log(string message)
    {
        Console.WriteLine($"[FILE] Logging to file: {message}");
    }
    
    public void Error(string message)
    {
        Console.WriteLine($"[FILE-ERROR] Logging error to file: {message}");
    }
}

// ============================================================================
// CZĘŚĆ 2: Dependency Injection - Loose Coupling
// ============================================================================

/// <summary>
/// ❌ TIGHT COUPLING - depends directly on ConsoleLogger
/// </summary>
public class TightlyCoupledApp
{
    private ConsoleLogger _logger = new();  // Direct dependency
    
    public void Start()
    {
        _logger.Log("Starting tightly coupled app...");
    }
}

/// <summary>
/// ✅ LOOSE COUPLING - depends on ILogger interface
/// </summary>
public class LooselyConfiguredApp
{
    private readonly ILogger _logger;  // Depend on interface
    
    // ✅ Constructor Injection - provide logger at runtime
    public LooselyConfiguredApp(ILogger logger)
    {
        _logger = logger;
    }
    
    public void Start()
    {
        _logger.Log("Starting loosely coupled app...");
    }
}

// ============================================================================
// CZĘŚĆ 3: Multiple Implementations
// ============================================================================

/// <summary>
/// Interface for data access
/// </summary>
public interface IRepository
{
    void Save(string data);
    string? Load();
}

public class DatabaseRepository : IRepository
{
    public void Save(string data)
    {
        Console.WriteLine($"Saving to database: {data}");
    }
    
    public string? Load()
    {
        Console.WriteLine("Loading from database...");
        return "Database data";
    }
}

public class FileRepository : IRepository
{
    public void Save(string data)
    {
        Console.WriteLine($"Saving to file: {data}");
    }
    
    public string? Load()
    {
        Console.WriteLine("Loading from file...");
        return "File data";
    }
}

/// <summary>
/// Service that depends on interface, not concrete implementation
/// </summary>
public class DataService
{
    private readonly IRepository _repository;
    
    public DataService(IRepository repository)
    {
        _repository = repository;
    }
    
    public void SaveData(string data)
    {
        _repository.Save(data);
    }
    
    public void LoadData()
    {
        var data = _repository.Load();
        Console.WriteLine($"Using data: {data}");
    }
}

// ============================================================================
// CZĘŚĆ 4: Multiple Interface Implementation
// ============================================================================

public interface IStorable
{
    void Save();
    void Load();
}

public interface IComparable
{
    int CompareTo(object? other);
}

/// <summary>
/// ✅ Class can implement multiple interfaces
/// </summary>
public class Document : IStorable, IComparable
{
    public string Content { get; set; }
    public string FileName { get; set; }
    
    public Document(string content, string fileName)
    {
        Content = content;
        FileName = fileName;
    }
    
    public void Save()
    {
        Console.WriteLine($"Saving {FileName}...");
    }
    
    public void Load()
    {
        Console.WriteLine($"Loading {FileName}...");
    }
    
    public int CompareTo(object? other)
    {
        if (other is Document doc)
            return Content.Length.CompareTo(doc.Content.Length);
        return 0;
    }
}

// ============================================================================
// CZĘŚĆ 5: Interfaces vs Classes - C# vs Java
// ============================================================================

/*
RÓŻNICE: Interfejs w C# vs Java

┌──────────────────┬────────────────────────┬─────────────────────────┐
│ Cecha            │ C# Interface           │ Java Interface          │
├──────────────────┼────────────────────────┼─────────────────────────┤
│ Metody           │ ✅ (public)            │ ✅ (public)             │
│ Properties       │ ✅ (public)            │ ❌ Brak                 │
│ Fields           │ ❌ Brak (only const)   │ ❌ Brak (implicitly pub)│
│ Default impl     │ ✅ C# 8.0+             │ ✅ Java 8+              │
│ Static members   │ ✅ C# 11+              │ ✅ Java 8+              │
│ Access modifiers │ ✅ C# 11+ (private)    │ ✅ Java 9+ (private)    │
│ Constructor      │ ❌ Brak                │ ❌ Brak                 │
│ Multiple inherit | ✅ (interfaces)        │ ✅ (interfaces)         │
└──────────────────┴────────────────────────┴─────────────────────────┘
*/

// ============================================================================
// CZĘŚĆ 6: Real-World DI Example
// ============================================================================

public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
    bool IsAvailable();
}

public class StripeProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"[Stripe] Processing ${amount}");
    }
    
    public bool IsAvailable() => true;
}

public class PayPalProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"[PayPal] Processing ${amount}");
    }
    
    public bool IsAvailable() => true;
}

/// <summary>
/// Depends on interface - can work with any processor
/// </summary>
public class OrderProcessor
{
    private readonly IPaymentProcessor _payment;
    private readonly ILogger _logger;
    
    public OrderProcessor(IPaymentProcessor payment, ILogger logger)
    {
        _payment = payment;
        _logger = logger;
    }
    
    public void ProcessOrder(decimal amount)
    {
        _logger.Log($"Processing order for ${amount}");
        
        if (_payment.IsAvailable())
        {
            _payment.ProcessPayment(amount);
            _logger.Log("Order processed successfully");
        }
        else
        {
            _logger.Error("Payment processor unavailable");
        }
    }
}

// ============================================================================
// TESTS
// ============================================================================

public class InterfacesTests
{
    [Fact]
    public void CanImplementInterface()
    {
        ILogger logger = new ConsoleLogger();
        logger.Log("Test message");
        
        Assert.Equal("ConsoleLogger", logger.Name);
    }
    
    [Fact]
    public void MultipleImplementations()
    {
        ILogger console = new ConsoleLogger();
        ILogger file = new FileLogger();
        
        List<ILogger> loggers = new() { console, file };
        
        foreach (var logger in loggers)
        {
            logger.Log("Same message");
        }
        
        Assert.Equal(2, loggers.Count);
    }
    
    [Fact]
    public void DependencyInjection()
    {
        // ✅ Can inject different implementations
        ILogger logger1 = new ConsoleLogger();
        ILogger logger2 = new FileLogger();
        
        var app1 = new LooselyConfiguredApp(logger1);
        var app2 = new LooselyConfiguredApp(logger2);
        
        app1.Start();
        app2.Start();
        
        Assert.NotNull(app1);
        Assert.NotNull(app2);
    }
    
    [Fact]
    public void RepositoryPattern()
    {
        IRepository db = new DatabaseRepository();
        IRepository file = new FileRepository();
        
        var service1 = new DataService(db);
        var service2 = new DataService(file);
        
        service1.SaveData("DB data");
        service2.SaveData("File data");
        
        service1.LoadData();
        service2.LoadData();
        
        Assert.NotNull(service1);
        Assert.NotNull(service2);
    }
    
    [Fact]
    public void MultipleInterfaceImplementation()
    {
        Document doc = new("Hello World", "doc.txt");
        
        // Can use as IStorable
        IStorable storable = doc;
        storable.Save();
        
        // Can use as IComparable
        IComparable comparable = doc;
        int result = comparable.CompareTo(null);
        
        Assert.Equal(11, doc.Content.Length);
    }
    
    [Fact]
    public void PaymentProcessorDI()
    {
        IPaymentProcessor stripe = new StripeProcessor();
        IPaymentProcessor paypal = new PayPalProcessor();
        ILogger logger = new ConsoleLogger();
        
        var order1 = new OrderProcessor(stripe, logger);
        var order2 = new OrderProcessor(paypal, logger);
        
        order1.ProcessOrder(100m);
        order2.ProcessOrder(200m);
        
        Assert.NotNull(order1);
        Assert.NotNull(order2);
    }
    
    [Fact]
    public void LooseCoupling()
    {
        // Can swap implementations without changing app code
        var app1 = new LooselyConfiguredApp(new ConsoleLogger());
        var app2 = new LooselyConfiguredApp(new FileLogger());
        
        app1.Start();
        app2.Start();
        
        Assert.NotNull(app1);
        Assert.NotNull(app2);
    }
}
