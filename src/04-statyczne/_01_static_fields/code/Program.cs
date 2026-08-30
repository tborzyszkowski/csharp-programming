using System;
using System.Collections.Generic;
using Xunit;

namespace StaticFields;

/// <summary>
/// 1. Pole statyczne - licznik instancji
/// </summary>
public class Counter
{
    public static int TotalCount = 0;
    public int Id { get; set; }
    
    public Counter()
    {
        TotalCount++;
        Id = TotalCount;
    }
}

/// <summary>
/// 2. Konfiguracja aplikacji (statyczne pola)
/// </summary>
public class AppConfig
{
    public static string AppName = "MyApplication";
    public static bool IsDebugMode = true;
    public static int MaxRetries = 3;
    public static readonly string Version = "1.0.0";  // readonly static
}

/// <summary>
/// 3. Konto bankowe z wspólną stopą procentową
/// </summary>
public class BankAccount
{
    public static decimal InterestRate = 0.05m;
    public decimal Balance { get; set; }
    
    public BankAccount(decimal initialBalance)
    {
        Balance = initialBalance;
    }
    
    public decimal CalculateInterest()
    {
        return Balance * InterestRate;
    }
}

/// <summary>
/// 4. Repozytorium z cache'em (static dictionary)
/// </summary>
public class UserRepository
{
    public static Dictionary<int, string> Cache = new();
    public static int CacheHits = 0;
    
    public string? GetUser(int id)
    {
        if (Cache.ContainsKey(id))
        {
            CacheHits++;
            return Cache[id];
        }
        
        return null;
    }
    
    public void AddUser(int id, string name)
    {
        Cache[id] = name;
    }
}

/// <summary>
/// 5. Logger z licznikiem logów
/// </summary>
public class Logger
{
    public static int LogCount = 0;
    private static List<string> Logs = new();
    
    public static void Log(string message)
    {
        LogCount++;
        Logs.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
    }
    
    public static void PrintAllLogs()
    {
        foreach (var log in Logs)
        {
            Console.WriteLine(log);
        }
    }
    
    public static void ClearLogs()
    {
        Logs.Clear();
        LogCount = 0;
    }
}

/// <summary>
/// Program demonstracyjny
/// </summary>
public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== POLA STATYCZNE (STATIC FIELDS) ===\n");
        
        // 1. Licznik instancji
        Console.WriteLine("1. LICZNIK INSTANCJI:");
        var c1 = new Counter();
        var c2 = new Counter();
        var c3 = new Counter();
        Console.WriteLine($"   TotalCount: {Counter.TotalCount}");
        Console.WriteLine($"   c1.Id: {c1.Id}, c2.Id: {c2.Id}, c3.Id: {c3.Id}");
        Console.WriteLine();
        
        // 2. Konfiguracja
        Console.WriteLine("2. KONFIGURACJA APLIKACJI:");
        Console.WriteLine($"   AppName: {AppConfig.AppName}");
        Console.WriteLine($"   IsDebugMode: {AppConfig.IsDebugMode}");
        Console.WriteLine($"   MaxRetries: {AppConfig.MaxRetries}");
        Console.WriteLine($"   Version: {AppConfig.Version}");
        Console.WriteLine();
        
        // 3. Stopa procentowa wspólna dla wszystkich kont
        Console.WriteLine("3. KONTA BANKOWE (wspólna stopa procentowa):");
        var acc1 = new BankAccount(1000);
        var acc2 = new BankAccount(2000);
        Console.WriteLine($"   BankAccount.InterestRate: {BankAccount.InterestRate}");
        Console.WriteLine($"   acc1 ({acc1.Balance}): {acc1.CalculateInterest()} odsetek");
        Console.WriteLine($"   acc2 ({acc2.Balance}): {acc2.CalculateInterest()} odsetek");
        
        BankAccount.InterestRate = 0.10m;  // Zmiana dla WSZYSTKICH kont!
        Console.WriteLine($"   Po zmianie stopy na 0.10m:");
        Console.WriteLine($"   acc1: {acc1.CalculateInterest()} odsetek");
        Console.WriteLine($"   acc2: {acc2.CalculateInterest()} odsetek");
        Console.WriteLine();
        
        // 4. Cache'owanie
        Console.WriteLine("4. REPOZYTORIUM Z CACHE'M:");
        var repo = new UserRepository();
        repo.AddUser(1, "Alice");
        repo.AddUser(2, "Bob");
        
        var user1 = repo.GetUser(1);  // Cache hit
        var user1Again = repo.GetUser(1);  // Cache hit
        var user3 = repo.GetUser(3);  // Cache miss
        
        Console.WriteLine($"   CacheHits: {UserRepository.CacheHits}");
        Console.WriteLine($"   Cached users: {UserRepository.Cache.Count}");
        Console.WriteLine();
        
        // 5. Logger
        Console.WriteLine("5. LOGGER Z LICZNIKIEM:");
        Logger.Log("Aplikacja uruchomiona");
        Logger.Log("Załadowano konfigurację");
        Logger.Log("Nawiązano połączenie z bazą");
        Console.WriteLine($"   Liczba logów: {Logger.LogCount}");
        Logger.PrintAllLogs();
    }
}

/// <summary>
/// Testy xUnit
/// </summary>
public class StaticFieldsTests
{
    [Fact]
    public void Counter_IncrementsTotalCountForEachInstance()
    {
        Counter.TotalCount = 0;  // Reset
        
        var c1 = new Counter();
        var c2 = new Counter();
        
        Assert.Equal(2, Counter.TotalCount);
        Assert.Equal(1, c1.Id);
        Assert.Equal(2, c2.Id);
    }
    
    [Fact]
    public void AppConfig_StaticFieldsAccessible()
    {
        Assert.Equal("MyApplication", AppConfig.AppName);
        Assert.True(AppConfig.IsDebugMode);
        Assert.Equal(3, AppConfig.MaxRetries);
    }
    
    [Fact]
    public void BankAccount_InterestRateSharedBetweenInstances()
    {
        BankAccount.InterestRate = 0.05m;
        
        var account1 = new BankAccount(1000);
        var account2 = new BankAccount(2000);
        
        // Zmiana dla jednego wpływa na wszystkie
        BankAccount.InterestRate = 0.10m;
        
        Assert.Equal(100, account1.CalculateInterest());
        Assert.Equal(200, account2.CalculateInterest());
    }
    
    [Fact]
    public void UserRepository_CacheTracksHits()
    {
        UserRepository.Cache.Clear();
        UserRepository.CacheHits = 0;
        
        var repo = new UserRepository();
        repo.AddUser(1, "Alice");
        
        repo.GetUser(1);  // Hit
        repo.GetUser(1);  // Hit
        repo.GetUser(2);  // Miss
        
        Assert.Equal(2, UserRepository.CacheHits);
    }
    
    [Fact]
    public void Logger_CountsLogMessages()
    {
        Logger.ClearLogs();
        
        Logger.Log("Message 1");
        Logger.Log("Message 2");
        Logger.Log("Message 3");
        
        Assert.Equal(3, Logger.LogCount);
    }
}
