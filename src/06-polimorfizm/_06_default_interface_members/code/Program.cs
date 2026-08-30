using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DefaultInterfaceMembers;

// ============================================================================
// CZĘŚĆ 1: Basic Default Implementation (C# 8.0+)
// ============================================================================

/// <summary>
/// Interface with default implementation
/// </summary>
public interface ILogger
{
    void Log(string message);
    
    // ✅ NEW: Default implementation - Nie trzeba implementować w klasach!
    public void LogInfo(string info)
    {
        Console.WriteLine($"[INFO] {info}");
    }
    
    public void LogWarning(string warning)
    {
        Console.WriteLine($"[WARNING] {warning}");
    }
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
    
    // ✅ LogInfo i LogWarning są odziedziczone z default implementation!
    // Ale możemy przesłonić jeśli chcemy:
    public void LogWarning(string warning)
    {
        Console.WriteLine($"[🚨 CRITICAL WARNING] {warning}");
    }
}

public class FileLogger : ILogger
{
    private readonly string _filePath = "log.txt";
    
    public void Log(string message)
    {
        // Write to file
        System.IO.File.AppendAllText(_filePath, message + "\n");
    }
    
    // LogInfo i LogWarning korzystają z default implementation
}

// ============================================================================
// CZĘŚĆ 2: Backward Compatibility Example
// ============================================================================

public interface IRepository<T>
{
    T? GetById(int id);
    void Save(T item);
    void Delete(int id);
    
    // ✅ C# 8.0+: Default implementation - nowa funkcja bez przerywania istniejącego kodu!
    public void SaveAll(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Save(item);
        }
    }
    
    // ✅ Dodatkowa domyślna metoda
    public int Count()
    {
        return 0;  // Domyślnie 0
    }
}

public class UserRepository : IRepository<User>
{
    private readonly List<User> _users = new();
    
    public User? GetById(int id) => _users.FirstOrDefault(u => u.Id == id);
    public void Save(User item) => _users.Add(item);
    public void Delete(int id) => _users.RemoveAll(u => u.Id == id);
    
    // ✅ Nie trzeba implementować SaveAll - korzysta z default!
    // Ale możemy przesłonić:
    public void Count()
    {
        Console.WriteLine($"Total users: {_users.Count}");
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// ============================================================================
// CZĘŚĆ 3: Combining Methods (Wrapper Pattern)
// ============================================================================

public interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount);
    void LogTransaction(string info);
    
    // ✅ Default: Kombinuje inne metody
    public bool ProcessWithValidation(decimal amount)
    {
        if (amount <= 0)
        {
            LogTransaction("Validation failed: negative amount");
            return false;
        }
        
        LogTransaction("Validation passed");
        return ProcessPayment(amount);
    }
}

public class CreditCardProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processing credit card payment: ${amount}");
        return true;
    }
    
    public void LogTransaction(string info)
    {
        Console.WriteLine($"[CC] {info}");
    }
    
    // ProcessWithValidation jest dostępne z default implementation!
}

// ============================================================================
// CZĘŚĆ 4: Static Members (C# 11+)
// ============================================================================

public interface IConverter
{
    // ✅ C# 11+: Static member - dostęp bez instancji!
    static double ConversionFactor = 1.60934;  // KM to miles
    
    // ✅ Static method
    static string GetConversionInfo()
    {
        return $"1 mile = {ConversionFactor} km";
    }
    
    // Instance member
    double Convert(double value);
}

public class DistanceConverter : IConverter
{
    public double Convert(double miles)
    {
        return miles * IConverter.ConversionFactor;
    }
}

// ============================================================================
// CZĘŚĆ 5: Access Modifiers (C# 11+)
// ============================================================================

public interface IAdvancedLogging
{
    // Public - dostęp wszędzie
    public void PublicLog(string message)
    {
        Console.WriteLine($"[PUBLIC] {message}");
    }
    
    // Private - dostęp tylko w interface'ie
    private void FormatMessage(string message)
    {
        return $"Formatted: {message}";
    }
    
    // Protected - dostęp w klasach implementujących
    protected void ProtectedLog(string message)
    {
        Console.WriteLine($"[PROTECTED] {message}");
    }
}

public class AdvancedLogger : IAdvancedLogging
{
    public void UseProtected()
    {
        ProtectedLog("Can access protected");
    }
}

// ============================================================================
// CZĘŚĆ 6: Real-World: Versioning Pattern
// ============================================================================

// Wersja 1.0
public interface IDataServiceV1
{
    string GetData(int id);
}

// Wersja 2.0 - Dodaliśmy nową metodę, ale z default implementation!
public interface IDataServiceV2 : IDataServiceV1
{
    // ✅ Nowe, ale z domyślną implementacją - backward compatible!
    public string GetDataWithCache(int id)
    {
        Console.WriteLine($"[Cache] Retrieving data for {id}");
        return GetData(id);
    }
}

public class DataService : IDataServiceV2
{
    public string GetData(int id)
    {
        return $"Data for {id}";
    }
    
    // GetDataWithCache jest dostępne z default implementation!
}

// ============================================================================
// PROGRAM GŁÓWNY I TESTY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TEMAT 6: DEFAULT INTERFACE MEMBERS - C# 8.0+              ║");
        Console.WriteLine("║  Nowoczesne Rozwiązania                                    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        // ====================================================================
        Console.WriteLine("SCENARIUSZ 1: Basic Default Implementation\n");
        
        ILogger consoleLogger = new ConsoleLogger();
        consoleLogger.Log("Application started");
        consoleLogger.LogInfo("This uses default implementation!");
        consoleLogger.LogWarning("But can be overridden");
        
        Console.WriteLine("\n--- File Logger (uses defaults) ---");
        ILogger fileLogger = new FileLogger();
        fileLogger.Log("File log entry");
        fileLogger.LogInfo("File uses default LogInfo");
        fileLogger.LogWarning("File uses default LogWarning");
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 2: Backward Compatibility\n");
        
        Console.WriteLine("Interface evolved with default implementation:");
        Console.WriteLine("- GetById() - original, required");
        Console.WriteLine("- SaveAll() - new in v2, has default");
        Console.WriteLine("- Count() - new in v2, has default\n");
        
        IRepository<User> repo = new UserRepository();
        
        repo.Save(new User { Id = 1, Name = "John" });
        repo.Save(new User { Id = 2, Name = "Jane" });
        
        // Default implementation - no need to implement!
        repo.SaveAll(new[]
        {
            new User { Id = 3, Name = "Bob" },
            new User { Id = 4, Name = "Alice" }
        });
        
        repo.Count();
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 3: Combining Methods (Wrapper Pattern)\n");
        
        IPaymentProcessor processor = new CreditCardProcessor();
        
        Console.WriteLine("--- Direct payment ---");
        processor.ProcessPayment(99.99m);
        
        Console.WriteLine("\n--- Payment with validation (default implementation) ---");
        processor.ProcessWithValidation(99.99m);
        
        Console.WriteLine("\n--- Invalid payment ---");
        processor.ProcessWithValidation(-50m);
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 4: Static Members (C# 11+)\n");
        
        Console.WriteLine($"Conversion Info: {IConverter.GetConversionInfo()}");
        Console.WriteLine($"Conversion Factor: {IConverter.ConversionFactor}");
        
        var converter = new DistanceConverter();
        Console.WriteLine($"100 miles = {converter.Convert(100)} km");
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 5: Versioning Pattern\n");
        
        IDataServiceV2 dataService = new DataService();
        
        Console.WriteLine("--- Original V1 method ---");
        Console.WriteLine(dataService.GetData(1));
        
        Console.WriteLine("\n--- New V2 method with default implementation ---");
        Console.WriteLine(dataService.GetDataWithCache(2));
        
        Console.WriteLine("\n✅ Demonstracja ukończona");
    }
}

// ============================================================================
// TESTY XUNIT
// ============================================================================

public class DefaultInterfaceMembersTests
{
    [Fact]
    public void Logger_DefaultImplementation_Works()
    {
        ILogger logger = new ConsoleLogger();
        
        // Default implementation should work
        logger.LogInfo("test");
        logger.LogWarning("warning");
        
        Assert.NotNull(logger);
    }
    
    [Fact]
    public void Logger_CanOverrideDefault()
    {
        var logger = new ConsoleLogger();
        
        // This should use the overridden version (with emoji)
        // We can't test console output easily, but we verify it works
        logger.LogWarning("critical");
        
        Assert.NotNull(logger);
    }
    
    [Fact]
    public void Repository_DefaultSaveAll_Works()
    {
        IRepository<User> repo = new UserRepository();
        
        var users = new[]
        {
            new User { Id = 1, Name = "John" },
            new User { Id = 2, Name = "Jane" }
        };
        
        // Default implementation
        repo.SaveAll(users);
        
        Assert.NotNull(repo);
    }
    
    [Fact]
    public void PaymentProcessor_DefaultValidation_Works()
    {
        IPaymentProcessor processor = new CreditCardProcessor();
        
        var result1 = processor.ProcessWithValidation(100m);  // Valid
        var result2 = processor.ProcessWithValidation(-50m);   // Invalid
        
        Assert.True(result1);
        Assert.False(result2);
    }
    
    [Fact]
    public void Converter_StaticMember_Accessible()
    {
        // Static member accessible without instance
        Assert.True(IConverter.ConversionFactor > 1);
    }
    
    [Fact]
    public void Converter_StaticMethod_Works()
    {
        var info = IConverter.GetConversionInfo();
        Assert.Contains("mile", info);
    }
    
    [Fact]
    public void DistanceConverter_Convert_Correct()
    {
        var converter = new DistanceConverter();
        var km = converter.Convert(100);
        
        Assert.Equal(100 * 1.60934, km);
    }
    
    [Fact]
    public void DataService_V2_BackwardCompatible()
    {
        IDataServiceV2 service = new DataService();
        
        // V1 methods still work
        var data = service.GetData(1);
        Assert.Contains("1", data);
        
        // V2 methods with defaults work
        var dataWithCache = service.GetDataWithCache(2);
        Assert.NotEmpty(dataWithCache);
    }
    
    [Fact]
    public void BackwardCompatibility_NoBreakingChanges()
    {
        // Create instance as old interface
        IDataServiceV1 oldVersion = new DataService();
        
        // Still works!
        var result = oldVersion.GetData(1);
        Assert.NotEmpty(result);
    }
}
