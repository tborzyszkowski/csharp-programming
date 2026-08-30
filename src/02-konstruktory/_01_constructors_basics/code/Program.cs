using System;
using Xunit;

namespace ConstructorsBasics;

/// <summary>
/// Temat 1: Konstruktory - o co chodzi?
/// Demonstracja różnych typów konstruktorów i ich zastosowania
/// </summary>

// ============ 1. KONSTRUKTOR PARAMETROWY ============

public class Person
{
    private readonly string name;
    private readonly int age;
    
    /// <summary>
    /// Konstruktor parametrowy - inicjalizuje dane osoby
    /// </summary>
    public Person(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Imię nie może być puste");
        
        if (age < 0 || age > 150)
            throw new ArgumentException("Wiek musi być między 0 a 150");
        
        this.name = name;
        this.age = age;
    }
    
    public string Name => name;
    public int Age => age;
    
    public void Introduce() => Console.WriteLine($"Cześć! Jestem {name}, mam {age} lat");
}

// ============ 2. KONSTRUKTOR DOMYŚLNY ============

public class Car
{
    public string Brand { get; set; }
    public string Model { get; set; }
    
    // Domyślny konstruktor (bez parametrów)
    public Car()
    {
        Brand = "Unknown";
        Model = "Unknown";
    }
    
    public Car(string brand, string model)
    {
        Brand = brand;
        Model = model;
    }
    
    public override string ToString() => $"{Brand} {Model}";
}

// ============ 3. KONSTRUKTOR STATICZNY ============

public class Logger
{
    private static int instanceCount = 0;
    private static readonly string logFilePath;
    
    // Konstruktor statyczny - uruchamia się raz, przed pierwszą instancją
    static Logger()
    {
        logFilePath = "application.log";
        Console.WriteLine($"[STATIC CONSTRUCTOR] Logger zainicjalizowany, log file: {logFilePath}");
    }
    
    public Logger(string name)
    {
        instanceCount++;
        Console.WriteLine($"[CONSTRUCTOR #{instanceCount}] Logger '{name}' created");
    }
    
    public static int GetInstanceCount() => instanceCount;
    public static string GetLogFile() => logFilePath;
}

// ============ 4. KONSTRUKTOR PRYWATNY - SINGLETON ============

public class Database
{
    private static Database? instance = null;
    
    // Prywatny konstruktor - nie można tworzyć z zewnątrz
    private Database()
    {
        Console.WriteLine("[DATABASE] Singleton instance created");
    }
    
    public static Database GetInstance()
    {
        if (instance == null)
        {
            instance = new Database();
        }
        return instance;
    }
    
    public void Query(string sql) => Console.WriteLine($"Executing: {sql}");
}

// ============ 5. KONSTRUKTOR PRYWATNY - FACTORY ============

public class Document
{
    public string Type { get; }
    public string Content { get; }
    
    private Document(string type, string content)
    {
        Type = type;
        Content = content;
    }
    
    public static Document CreatePDF(string content)
    {
        Console.WriteLine("[FACTORY] Creating PDF document");
        return new Document("PDF", content);
    }
    
    public static Document CreateWord(string content)
    {
        Console.WriteLine("[FACTORY] Creating WORD document");
        return new Document("Word", content);
    }
    
    public override string ToString() => $"Document[{Type}]: {Content[..Math.Min(30, Content.Length)]}...";
}

// ============ 6. KONSTRUKTOR Z WALIDACJĄ ============

public class BankAccount
{
    private readonly string accountNumber;
    private decimal balance;
    
    public BankAccount(string accountNumber, decimal initialBalance)
    {
        if (string.IsNullOrEmpty(accountNumber))
            throw new ArgumentException("Account number cannot be empty");
        
        if (initialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative");
        
        this.accountNumber = accountNumber;
        this.balance = initialBalance;
    }
    
    public string AccountNumber => accountNumber;
    public decimal Balance => balance;
    
    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive");
        
        balance += amount;
    }
    
    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdraw amount must be positive");
        
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds");
        
        balance -= amount;
    }
}

// ============ MAIN PROGRAM ============

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 1: KONSTRUKTORY - O CO CHODZI? ===\n");
        
        // 1. Konstruktor parametrowy
        Console.WriteLine("1. KONSTRUKTOR PARAMETROWY");
        var person = new Person("Anna", 30);
        person.Introduce();
        Console.WriteLine();
        
        // 2. Konstruktor domyślny
        Console.WriteLine("2. KONSTRUKTOR DOMYŚLNY");
        var car1 = new Car();
        var car2 = new Car("Tesla", "Model 3");
        Console.WriteLine($"Car 1: {car1}");
        Console.WriteLine($"Car 2: {car2}");
        Console.WriteLine();
        
        // 3. Konstruktor statyczny
        Console.WriteLine("3. KONSTRUKTOR STATYCZNY");
        var log1 = new Logger("Main");
        var log2 = new Logger("Service");
        var log3 = new Logger("Database");
        Console.WriteLine($"Total instances: {Logger.GetInstanceCount()}");
        Console.WriteLine($"Log file: {Logger.GetLogFile()}");
        Console.WriteLine();
        
        // 4. Singleton pattern
        Console.WriteLine("4. SINGLETON - KONSTRUKTOR PRYWATNY");
        var db1 = Database.GetInstance();
        var db2 = Database.GetInstance();
        Console.WriteLine($"Same instance? {ReferenceEquals(db1, db2)}");
        db1.Query("SELECT * FROM Users");
        Console.WriteLine();
        
        // 5. Factory pattern
        Console.WriteLine("5. FACTORY - KONSTRUKTOR PRYWATNY");
        var pdf = Document.CreatePDF("My PDF content");
        var word = Document.CreateWord("My Word content");
        Console.WriteLine(pdf);
        Console.WriteLine(word);
        Console.WriteLine();
        
        // 6. Bank account z walidacją
        Console.WriteLine("6. WALIDACJA W KONSTRUKTORZE");
        var account = new BankAccount("123456", 5000);
        Console.WriteLine($"Account: {account.AccountNumber}, Balance: ${account.Balance}");
        account.Deposit(1000);
        Console.WriteLine($"After deposit: ${account.Balance}");
        account.Withdraw(500);
        Console.WriteLine($"After withdrawal: ${account.Balance}");
    }
}

// ============ TESTY XUNIT ============

public class ConstructorTests
{
    [Fact]
    public void PersonConstructor_WithValidData_SetsNameAndAge()
    {
        var person = new Person("John", 25);
        
        Assert.Equal("John", person.Name);
        Assert.Equal(25, person.Age);
    }
    
    [Fact]
    public void PersonConstructor_WithInvalidAge_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Person("John", -1));
        Assert.Throws<ArgumentException>(() => new Person("John", 151));
    }
    
    [Fact]
    public void PersonConstructor_WithEmptyName_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Person("", 25));
        Assert.Throws<ArgumentException>(() => new Person(null!, 25));
    }
    
    [Fact]
    public void CarDefaultConstructor_InitializesWithUnknown()
    {
        var car = new Car();
        
        Assert.Equal("Unknown", car.Brand);
        Assert.Equal("Unknown", car.Model);
    }
    
    [Fact]
    public void CarParameterConstructor_SetsBrandAndModel()
    {
        var car = new Car("BMW", "X5");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal("X5", car.Model);
    }
    
    [Fact]
    public void StaticConstructor_RunsOnce()
    {
        var log1 = new Logger("First");
        var log2 = new Logger("Second");
        
        Assert.Equal(2, Logger.GetInstanceCount());
        Assert.Equal("application.log", Logger.GetLogFile());
    }
    
    [Fact]
    public void Singleton_ReturnsAlwaysSameInstance()
    {
        var db1 = Database.GetInstance();
        var db2 = Database.GetInstance();
        
        Assert.True(ReferenceEquals(db1, db2));
    }
    
    [Fact]
    public void Factory_CreatesDifferentInstances()
    {
        var pdf = Document.CreatePDF("content");
        var word = Document.CreateWord("content");
        
        Assert.Equal("PDF", pdf.Type);
        Assert.Equal("Word", word.Type);
        Assert.NotEqual(pdf.Type, word.Type);
    }
    
    [Fact]
    public void BankAccount_Constructor_ValidatesBalance()
    {
        Assert.Throws<ArgumentException>(() => new BankAccount("123", -100));
    }
    
    [Fact]
    public void BankAccount_Deposit_IncreasesBalance()
    {
        var account = new BankAccount("123", 1000);
        account.Deposit(500);
        
        Assert.Equal(1500, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_ThrowsWhenInsufficientFunds()
    {
        var account = new BankAccount("123", 100);
        
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(200));
    }
}
