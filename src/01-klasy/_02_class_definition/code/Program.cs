using System;
using Xunit;

namespace ClassDefinition;

/// <summary>
/// TEMAT 2: Definicja Klasy w Języku C#
/// 
/// Demonstracja:
/// - Definiowania klas
/// - Konstruktorów
/// - Pól, właściwości i metod
/// - Enkapsulacji
/// </summary>

/// <summary>
/// Przykład 1: Prosta klasa Person
/// </summary>
public class Person
{
    // Pola prywatne
    private string firstName;
    private string lastName;
    private int age;
    
    // Konstruktor bez parametrów
    public Person()
    {
        firstName = "Unknown";
        lastName = "Unknown";
        age = 0;
    }
    
    // Konstruktor z parametrami
    public Person(string firstName, string lastName, int age)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
    }
    
    // Właściwości
    public string FirstName
    {
        get { return firstName; }
        set { firstName = value ?? "Unknown"; }
    }
    
    public string LastName
    {
        get { return lastName; }
        set { lastName = value ?? "Unknown"; }
    }
    
    public int Age
    {
        get { return age; }
        set { age = value >= 0 ? value : 0; }
    }
    
    // Właściwość read-only (obliczana)
    public string FullName => $"{firstName} {lastName}";
    
    // Metody
    public void Introduce()
    {
        Console.WriteLine($"Cześć! Jestem {FullName}, mam {age} lat");
    }
    
    public bool IsAdult() => age >= 18;
    
    public override string ToString() => FullName;
}

/// <summary>
/// Przykład 2: Klasa BankAccount z enkapsulacją
/// </summary>
public class BankAccount
{
    // Pola prywatne
    private string accountNumber;
    private decimal balance;
    private int pin;
    private string accountHolder;
    
    // Konstruktor
    public BankAccount(string accountHolder, string accountNumber, int pin)
    {
        this.accountHolder = accountHolder;
        this.accountNumber = accountNumber;
        this.pin = pin;
        this.balance = 0;
    }
    
    // Właściwości
    public string AccountNumber => accountNumber;
    public string AccountHolder => accountHolder;
    public decimal Balance => balance;
    
    // Metody
    public bool Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
            Console.WriteLine($"✓ Wpłacono: {amount:C}. Nowe saldo: {balance:C}");
            return true;
        }
        Console.WriteLine("✗ Kwota musi być dodatnia!");
        return false;
    }
    
    public bool Withdraw(decimal amount, int providedPin)
    {
        if (providedPin != pin)
        {
            Console.WriteLine("✗ Błędny PIN!");
            return false;
        }
        
        if (amount <= 0)
        {
            Console.WriteLine("✗ Kwota musi być dodatnia!");
            return false;
        }
        
        if (amount > balance)
        {
            Console.WriteLine("✗ Niewystarczające środki!");
            return false;
        }
        
        balance -= amount;
        Console.WriteLine($"✓ Wypłacono: {amount:C}. Nowe saldo: {balance:C}");
        return true;
    }
    
    public bool ChangePin(int oldPin, int newPin)
    {
        if (oldPin != pin)
        {
            Console.WriteLine("✗ Stary PIN jest nieprawidłowy");
            return false;
        }
        
        pin = newPin;
        Console.WriteLine("✓ PIN zmieniony pomyślnie");
        return true;
    }
    
    public override string ToString()
    {
        return $"Konto: {accountHolder} ({accountNumber}) - Saldo: {balance:C}";
    }
}

/// <summary>
/// Przykład 3: Klasa Student z polem statycznym
/// </summary>
public class Student
{
    private string name;
    private int studentId;
    private double gpa;
    private static int totalStudents = 0;
    
    public Student(string name, double gpa)
    {
        this.name = name;
        this.gpa = gpa;
        this.studentId = ++totalStudents;
    }
    
    public string Name => name;
    public int StudentId => studentId;
    public double GPA => gpa;
    public static int TotalStudents => totalStudents;
    
    public void UpdateGPA(double newGPA)
    {
        if (newGPA >= 0 && newGPA <= 4.0)
            gpa = newGPA;
    }
    
    public override string ToString()
    {
        return $"Student: {name} (ID: {studentId}) - GPA: {gpa:F2}";
    }
}

/// <summary>
/// Przykład 4: Klasa z auto-implementowanymi właściwościami
/// </summary>
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    
    public Product(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
    }
    
    public decimal TotalValue => Price * Quantity;
    
    public override string ToString()
    {
        return $"{Name}: {Price:C} x {Quantity} = {TotalValue:C}";
    }
}

/// ============================================
/// KLASA GŁÓWNA - DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
        Console.WriteLine("║  DEFINICJA KLASY W C#                                ║");
        Console.WriteLine("║  Konstruktory, pola, właściwości i metody           ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════╝\n");
        
        DemonstratePerson();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateBankAccount();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateStudent();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateProduct();
    }
    
    private static void DemonstratePerson()
    {
        Console.WriteLine("👤 Klasa Person - Konstruktory i Właściwości");
        Console.WriteLine("──────────────────────────────────────────\n");
        
        // Konstruktor bezparametrowy
        var person1 = new Person();
        Console.WriteLine("1. Konstruktor domyślny:");
        Console.WriteLine($"   {person1.FullName}, wiek: {person1.Age}");
        
        // Konstruktor z parametrami
        var person2 = new Person("Jan", "Kowalski", 30);
        Console.WriteLine("\n2. Konstruktor z parametrami:");
        person2.Introduce();
        
        // Modyfikacja poprzez właściwości
        Console.WriteLine("\n3. Modyfikacja właściwości:");
        person2.Age = 31;
        person2.Introduce();
        
        // Sprawdzenie czy osoba jest dorosła
        Console.WriteLine($"\n4. Czy jest dorosły? {person2.IsAdult()}");
    }
    
    private static void DemonstrateBankAccount()
    {
        Console.WriteLine("🏦 Klasa BankAccount - Enkapsulacja");
        Console.WriteLine("──────────────────────────────────\n");
        
        var account = new BankAccount("Jan Nowak", "PL12345678", 1234);
        Console.WriteLine($"Konto utworzone: {account}\n");
        
        // Wpłata
        Console.WriteLine("Operacje na koncie:");
        account.Deposit(1000);
        account.Deposit(500);
        
        // Próba niewłaściwej operacji
        Console.WriteLine();
        account.Withdraw(100, 9999);  // Błędny PIN
        
        // Poprawna wypłata
        Console.WriteLine();
        account.Withdraw(300, 1234);
        
        // Zmiana PINu
        Console.WriteLine();
        account.ChangePin(1234, 5678);
    }
    
    private static void DemonstrateStudent()
    {
        Console.WriteLine("🎓 Klasa Student - Pola Statyczne");
        Console.WriteLine("─────────────────────────────────\n");
        
        var student1 = new Student("Anna", 3.8);
        var student2 = new Student("Piotr", 3.5);
        var student3 = new Student("Maria", 3.9);
        
        Console.WriteLine(student1);
        Console.WriteLine(student2);
        Console.WriteLine(student3);
        
        Console.WriteLine($"\nLiczba studentów: {Student.TotalStudents}");
        
        // Zmiana GPA
        student1.UpdateGPA(3.9);
        Console.WriteLine($"\nPo aktualizacji: {student1}");
    }
    
    private static void DemonstrateProduct()
    {
        Console.WriteLine("📦 Klasa Product - Auto-implementowane Właściwości");
        Console.WriteLine("────────────────────────────────────────────────\n");
        
        var products = new Product[]
        {
            new Product("Laptop", 2999.99m, 5),
            new Product("Monitor", 599.99m, 10),
            new Product("Klawiatura", 149.99m, 20)
        };
        
        decimal totalInventoryValue = 0;
        
        Console.WriteLine("Magazyn:");
        foreach (var product in products)
        {
            Console.WriteLine($"  {product}");
            totalInventoryValue += product.TotalValue;
        }
        
        Console.WriteLine($"\nWartość całego magazynu: {totalInventoryValue:C}");
    }
}

/// ============================================
/// TESTY JEDNOSTKOWE
/// ============================================

public class ClassDefinitionTests
{
    [Fact]
    public void Person_Constructor_InitializesFields()
    {
        var person = new Person("Jan", "Kowalski", 30);
        
        Assert.Equal("Jan", person.FirstName);
        Assert.Equal("Kowalski", person.LastName);
        Assert.Equal(30, person.Age);
        Assert.Equal("Jan Kowalski", person.FullName);
    }
    
    [Fact]
    public void Person_IsAdult_ReturnsTrueFor18OrOlder()
    {
        var adult = new Person("Jan", "Kowalski", 18);
        var minor = new Person("Anna", "Nowak", 16);
        
        Assert.True(adult.IsAdult());
        Assert.False(minor.IsAdult());
    }
    
    [Fact]
    public void Person_Age_NegativeValueSetToZero()
    {
        var person = new Person("Jan", "Kowalski", 20);
        person.Age = -5;
        
        Assert.Equal(0, person.Age);
    }
    
    [Fact]
    public void BankAccount_Deposit_IncreasesBalance()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        
        account.Deposit(1000);
        Assert.Equal(1000, account.Balance);
        
        account.Deposit(500);
        Assert.Equal(1500, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_WithCorrectPin_Succeeds()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        account.Deposit(1000);
        
        bool result = account.Withdraw(500, 1234);
        
        Assert.True(result);
        Assert.Equal(500, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_WithWrongPin_Fails()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        account.Deposit(1000);
        
        bool result = account.Withdraw(500, 9999);
        
        Assert.False(result);
        Assert.Equal(1000, account.Balance);
    }
    
    [Fact]
    public void BankAccount_ChangePin_WithCorrectOldPin_Succeeds()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        
        bool result = account.ChangePin(1234, 5678);
        Assert.True(result);
        
        // Sprawdzenie czy nowy PIN działa
        account.Deposit(100);
        bool withdraw = account.Withdraw(50, 5678);
        Assert.True(withdraw);
    }
    
    [Fact]
    public void Student_StaticField_CountsAllStudents()
    {
        // Reset - każdy test powinien być niezależny
        var student1 = new Student("Anna", 3.8);
        var student2 = new Student("Piotr", 3.5);
        
        Assert.Equal(1, student1.StudentId);
        Assert.Equal(2, student2.StudentId);
        Assert.Equal(2, Student.TotalStudents);
    }
    
    [Fact]
    public void Product_TotalValue_CalculatesCorrectly()
    {
        var product = new Product("Laptop", 1000m, 5);
        
        Assert.Equal(5000m, product.TotalValue);
    }
}
