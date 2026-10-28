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
    // Plik tymczasowy - demonstracja nie zostawia śmieci w katalogu projektu
    private readonly string _filePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "dim-demo-log.txt");
    
    public void Log(string message)
    {
        System.IO.File.AppendAllText(_filePath, message + Environment.NewLine);
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
    public int Count()
    {
        return _users.Count;
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
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
// CZĘŚĆ 4: Składowe statyczne w interfejsach (C# 8+) i static abstract (C# 11+)
// ============================================================================

public interface IConverter
{
    // ✅ C# 8+: składowa statyczna interfejsu - dostęp bez instancji.
    // Uwaga: publiczne, modyfikowalne pole statyczne to współdzielony stan globalny.
    static double ConversionFactor = 1.60934;  // mile -> km
    
    // ✅ Metoda statyczna
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

// ✅ C# 11+: static abstract - kontrakt na składowe STATYCZNE implementującego typu.
// Wzorzec "self-referencing generic" (TSelf : IParsableValue<TSelf>) umożliwia wywołanie T.Parse(...).
public interface IParsableValue<TSelf> where TSelf : IParsableValue<TSelf>
{
    static abstract TSelf Parse(string text);
}

public readonly record struct Meters(double Value) : IParsableValue<Meters>
{
    public static Meters Parse(string text) =>
        new(double.Parse(text, System.Globalization.CultureInfo.InvariantCulture));
}

public static class ValueReader
{
    // Metoda generyczna wywołuje statyczną metodę typu T - bez instancji i bez refleksji
    public static T Read<T>(string text) where T : IParsableValue<T> => T.Parse(text);
}

// ============================================================================
// CZĘŚĆ 5: Modyfikatory dostępu w interfejsach (C# 8+)
// ============================================================================

public interface IAdvancedLogging
{
    // Public - dostępny przez referencję interfejsu
    public void PublicLog(string message)
    {
        Console.WriteLine($"[PUBLIC] {FormatMessage(message)}");
    }
    
    // Private - dostępny tylko wewnątrz tego interfejsu (pomocnik dla domyślnych implementacji)
    private string FormatMessage(string message)
    {
        return $"Formatted: {message}";
    }
    
    // Protected - dostępny dla interfejsów pochodnych. Klasa implementująca może taką
    // składową zaimplementować, ale nie może jej bezpośrednio wywołać.
    protected void ProtectedLog(string message)
    {
        Console.WriteLine($"[PROTECTED] {message}");
    }

    public void LogFromImplementation()
    {
        ProtectedLog("Called by an interface default member");
    }
}

public interface IExtendedLogging : IAdvancedLogging
{
    // Interfejs pochodny ma dostęp do składowej protected interfejsu bazowego
    public void LogViaDerivedInterface(string message) => ProtectedLog(message);
}

public class AdvancedLogger : IExtendedLogging
{
    public void UseProtected()
    {
        // ProtectedLog("...");   // ❌ błąd kompilacji: klasa nie widzi protected składowej interfejsu
        ((IAdvancedLogging)this).LogFromImplementation();
    }
}

// ============================================================================
// CZĘŚĆ 5b: Dwie domyślne implementacje tej samej metody ("problem diamentu")
// ============================================================================

public interface IGreeterA
{
    string Hello() => "A";
}

public interface IGreeterB
{
    string Hello() => "B";
}

// Klasa implementująca oba interfejsy MUSI wskazać własną implementację -
// inaczej kompilator zgłosi błąd (brak "najbardziej szczegółowej" implementacji).
public class BothGreeter : IGreeterA, IGreeterB
{
    public string Hello() => "Both";
}

// Interfejs pochodny może zastąpić domyślną implementację interfejsu bazowego
// (składnia jawnej implementacji interfejsu).
public interface ILoudGreeter : IGreeterA
{
    string IGreeterA.Hello() => "LOUD";
}

public class LoudGreeter : ILoudGreeter { }

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
        
        Console.WriteLine($"Total users: {repo.Count()}");
        
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
        Console.WriteLine("\n\nSCENARIUSZ 4: Składowe statyczne (C# 8+) i static abstract (C# 11+)\n");
        
        Console.WriteLine($"Conversion Info: {IConverter.GetConversionInfo()}");
        Console.WriteLine($"Conversion Factor: {IConverter.ConversionFactor}");
        
        var converter = new DistanceConverter();
        Console.WriteLine($"100 miles = {converter.Convert(100)} km");

        var distance = ValueReader.Read<Meters>("42.5");
        Console.WriteLine($"ValueReader.Read<Meters>(\"42.5\") = {distance.Value} m  (T.Parse przez static abstract)");

        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 4b: Dwie domyślne implementacje tej samej metody\n");

        IGreeterA viaA = new BothGreeter();
        IGreeterB viaB = new BothGreeter();
        IGreeterA loud = new LoudGreeter();
        Console.WriteLine($"BothGreeter przez IGreeterA: {viaA.Hello()}");
        Console.WriteLine($"BothGreeter przez IGreeterB: {viaB.Hello()}");
        Console.WriteLine($"LoudGreeter przez IGreeterA: {loud.Hello()}  (ILoudGreeter zastępuje domyślną implementację)");
        
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

        var output = CaptureOutput(() => logger.LogInfo("test"));

        Assert.Equal("[INFO] test", output);
    }
    
    [Fact]
    public void Logger_ClassImplementation_ReplacesDefault()
    {
        ILogger logger = new ConsoleLogger();

        var output = CaptureOutput(() => logger.LogWarning("critical"));

        Assert.Contains("CRITICAL WARNING", output);
        Assert.DoesNotContain("[WARNING]", output);
    }

    [Fact]
    public void Logger_ClassWithoutOverride_UsesInterfaceDefault()
    {
        ILogger logger = new FileLogger();

        var output = CaptureOutput(() => logger.LogWarning("careful"));

        Assert.Equal("[WARNING] careful", output);
    }

    [Fact]
    public void StaticAbstract_GenericMethod_CallsTypeMember()
    {
        var meters = ValueReader.Read<Meters>("12.5");

        Assert.Equal(12.5, meters.Value);
    }

    [Fact]
    public void TwoDefaults_ClassImplementation_IsUsedThroughBothInterfaces()
    {
        var greeter = new BothGreeter();

        Assert.Equal("Both", ((IGreeterA)greeter).Hello());
        Assert.Equal("Both", ((IGreeterB)greeter).Hello());
    }

    [Fact]
    public void DerivedInterface_CanReplaceBaseInterfaceDefault()
    {
        IGreeterA greeter = new LoudGreeter();

        Assert.Equal("LOUD", greeter.Hello());
    }

    [Fact]
    public void ProtectedInterfaceMember_IsReachableFromDerivedInterface()
    {
        IExtendedLogging logger = new AdvancedLogger();

        var output = CaptureOutput(() => logger.LogViaDerivedInterface("hi"));

        Assert.Equal("[PROTECTED] hi", output);
    }

    [Fact]
    public void PrivateInterfaceHelper_IsUsedByPublicDefault()
    {
        IAdvancedLogging logger = new AdvancedLogger();

        var output = CaptureOutput(() => logger.PublicLog("msg"));

        Assert.Equal("[PUBLIC] Formatted: msg", output);
    }

    private static string CaptureOutput(Action action)
    {
        var original = Console.Out;
        using var writer = new System.IO.StringWriter();
        Console.SetOut(writer);
        try
        {
            action();
        }
        finally
        {
            Console.SetOut(original);
        }

        return writer.ToString().Trim();
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
        
        Assert.Equal(2, repo.Count());
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
