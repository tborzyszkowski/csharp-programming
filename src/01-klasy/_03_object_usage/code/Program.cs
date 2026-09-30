using System;
using System.Collections.Generic;
using Xunit;

namespace ObjectUsage;

/// <summary>
/// TEMAT 3: Tworzenie i Korzystanie z Obiektów
/// 
/// Demonstracja:
/// - Operatora new
/// - Referencji i wartości
/// - Inicjalizatorów
/// - Zarządzania pamięcią
/// </summary>

public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    public string City { get; set; } = "";
    
    public override bool Equals(object? obj)
    {
        if (obj is not Person other) return false;
        return Name == other.Name && Age == other.Age && City == other.City;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Age, City);
    }
    
    public override string ToString() => $"{Name}, {Age} lat, {City}";
}

// STRUKTURA (typ wartościowy) - przypisanie kopiuje całą wartość.
// Zmienna struktura (set) jest tu celowa, żeby pokazać semantykę kopiowania;
// w produkcyjnym kodzie preferuj struktury niezmienne (readonly struct).
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Point other) return false;
        return X == other.X && Y == other.Y;
    }
    
    public override int GetHashCode() => HashCode.Combine(X, Y);
    
    public override string ToString() => $"({X}, {Y})";
}

public class BankAccount
{
    private decimal balance;
    
    public string Owner { get; set; }
    public decimal Balance => balance;
    
    public BankAccount(string owner, decimal initialBalance = 0)
    {
        Owner = owner;
        balance = initialBalance;
    }
    
    public void Deposit(decimal amount)
    {
        if (amount > 0)
            balance += amount;
    }
    
    public override string ToString() => $"{Owner}: {balance:C}";
}

/// <summary>
/// Metody pomocnicze pokazujące, że do metody przekazywana jest KOPIA referencji.
/// </summary>
public static class ReferenceDemo
{
    // Zmiana stanu obiektu jest widoczna u wołującego (obie referencje wskazują na ten sam obiekt)
    public static void Rename(Person person) => person.Name = "Zmieniony";

    // Przypisanie nowej wartości do parametru zmienia tylko lokalną kopię referencji
    public static void Replace(Person person) => person = new Person { Name = "Nowy" };

    // Modyfikator ref przekazuje samą zmienną - teraz podmiana jest widoczna u wołującego
    public static void ReplaceByRef(ref Person person) => person = new Person { Name = "Nowy" };
}

/// ============================================
/// KLASA GŁÓWNA - DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TWORZENIE I KORZYSTANIE Z OBIEKTÓW                   ║");
        Console.WriteLine("║  Operator new, referencje, inicjalizatory             ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        DemonstrateObjectCreation();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateReferencesVsValues();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateInitializers();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateEqualityAndIdentity();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateNullAndParameterPassing();
    }
    
    private static void DemonstrateObjectCreation()
    {
        Console.WriteLine("🎯 Tworzenie Obiektów za Pomocą Operatora new");
        Console.WriteLine("──────────────────────────────────────────────\n");
        
        // Tradycyjne tworzenie
        Person person1 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        Console.WriteLine($"Person 1: {person1}");
        
        // Z konstruktorem
        BankAccount account1 = new BankAccount("Jan Kowalski", 1000);
        Console.WriteLine($"Account 1: {account1}");
        
        // Każde new tworzy nowy obiekt
        Person person2 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        Console.WriteLine($"Person 2: {person2}");
        Console.WriteLine($"ReferenceEquals(person1, person2)? {ReferenceEquals(person1, person2)}");
        Console.WriteLine($"Person 1.Equals(Person 2)? {person1.Equals(person2)}");
    }
    
    private static void DemonstrateReferencesVsValues()
    {
        Console.WriteLine("🔗 Referencje vs Wartości");
        Console.WriteLine("─────────────────────────\n");
        
        // TYPY REFERENCYJNE - klasy
        Console.WriteLine(">>> Typ referencyjny (klasa Person):");
        var person1 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person2 = person1;  // Obie zmienne wskazują na TEN SAM OBIEKT
        
        Console.WriteLine($"  person1: {person1}");
        Console.WriteLine($"  person2: {person2}");
        Console.WriteLine($"  ReferenceEquals? {ReferenceEquals(person1, person2)}");
        
        person2.Name = "Maria";
        Console.WriteLine($"\n  Po zmianie person2.Name = 'Maria':");
        Console.WriteLine($"  person1.Name: {person1.Name} (ZMIENIŁ SIĘ!)");
        Console.WriteLine($"  person2.Name: {person2.Name}");
        
        // TYPY WARTOŚCIOWE - struktury
        Console.WriteLine("\n>>> Typ wartościowy (struktura Point):");
        var point1 = new Point { X = 10, Y = 20 };
        var point2 = point1;  // KOPIA wartości
        
        Console.WriteLine($"  point1: {point1}");
        Console.WriteLine($"  point2: {point2}");
        
        point2.X = 100;
        Console.WriteLine($"\n  Po zmianie point2.X = 100:");
        Console.WriteLine($"  point1.X: {point1.X} (bez zmian!)");
        Console.WriteLine($"  point2.X: {point2.X}");
    }
    
    private static void DemonstrateInitializers()
    {
        Console.WriteLine("✨ Inicjalizatory Obiektów");
        Console.WriteLine("──────────────────────────\n");
        
        // Object initializer
        Console.WriteLine("1. Object initializer:");
        var person = new Person
        {
            Name = "Anna",
            Age = 28,
            City = "Kraków"
        };
        Console.WriteLine($"   {person}");
        
        // Collection initializer
        Console.WriteLine("\n2. Collection initializer:");
        var people = new List<Person>
        {
            new Person { Name = "Jan", Age = 30, City = "Warszawa" },
            new Person { Name = "Maria", Age = 25, City = "Kraków" },
            new Person { Name = "Piotr", Age = 35, City = "Gdańsk" }
        };
        
        foreach (var p in people)
            Console.WriteLine($"   {p}");
        
        // Target-typed new (C# 9+)
        Console.WriteLine("\n3. Target-typed new (C# 9+):");
        Person personTargeted = new() { Name = "Sławek", Age = 40, City = "Poznań" };
        Console.WriteLine($"   {personTargeted}");
        
        List<BankAccount> accounts = new()
        {
            new("Adam", 500),
            new("Beata", 1000),
            new("Czesław", 2000)
        };
        
        foreach (var acc in accounts)
            Console.WriteLine($"   {acc}");
    }
    
    private static void DemonstrateEqualityAndIdentity()
    {
        Console.WriteLine("⚖️  Równość i Identyczność");
        Console.WriteLine("──────────────────────────\n");
        
        var person1 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person2 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person3 = person1;
        
        Console.WriteLine("Obiekty:");
        Console.WriteLine($"  person1: {person1}");
        Console.WriteLine($"  person2: {person2} (identyczna zawartość)");
        Console.WriteLine($"  person3: {person3} (referencja do person1)");
        
        Console.WriteLine("\nIdentyczność (czy to ten sam obiekt?):");
        Console.WriteLine($"  ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}");
        Console.WriteLine($"  ReferenceEquals(person1, person3): {ReferenceEquals(person1, person3)}");
        
        Console.WriteLine("\nRówność (czy mają tę samą zawartość?):");
        Console.WriteLine($"  person1.Equals(person2): {person1.Equals(person2)}");
        Console.WriteLine($"  person1 == person2: {person1 == person2} (operator == domyślnie porównuje referencje)");
        Console.WriteLine($"  person1.Equals(person3): {person1.Equals(person3)}");
        
        Console.WriteLine("\nJeśli person2.Name zmieni się:");
        person2.Name = "Maria";
        Console.WriteLine($"  person2: {person2}");
        Console.WriteLine($"  person1.Equals(person2): {person1.Equals(person2)} (już nie równe)");
    }
    
    private static void DemonstrateNullAndParameterPassing()
    {
        Console.WriteLine("🔄 null i przekazywanie obiektów do metod");
        Console.WriteLine("──────────────────────────────────────\n");
        
        // null = zmienna nie wskazuje na żaden obiekt
        Person? nobody = null;
        Console.WriteLine($"nobody?.Name ?? \"brak\": {nobody?.Name ?? "brak"}");
        // Console.WriteLine(nobody.Name);  // NullReferenceException w czasie działania!
        
        var person = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        
        ReferenceDemo.Rename(person);
        Console.WriteLine($"\nPo Rename(person):  {person.Name}  (obiekt został zmieniony)");
        
        person.Name = "Jan";
        ReferenceDemo.Replace(person);
        Console.WriteLine($"Po Replace(person): {person.Name}  (zmienna wołającego bez zmian)");
        
        ReferenceDemo.ReplaceByRef(ref person);
        Console.WriteLine($"Po ReplaceByRef(ref person): {person.Name}  (zmienna podmieniona)");
    }
}

/// ============================================
/// TESTY JEDNOSTKOWE
/// ============================================

public class ObjectUsageTests
{
    [Fact]
    public void NewOperator_CreatesNewObject()
    {
        var person1 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person2 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        
        Assert.NotNull(person1);
        Assert.NotNull(person2);
        Assert.False(ReferenceEquals(person1, person2));
    }
    
    [Fact]
    public void ReferenceAssignment_PointsToSameObject()
    {
        var person1 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person2 = person1;
        
        Assert.True(ReferenceEquals(person1, person2));
        
        person2.Name = "Maria";
        Assert.Equal("Maria", person1.Name);
    }
    
    [Fact]
    public void ValueType_CopiesValue()
    {
        var point1 = new Point { X = 10, Y = 20 };
        var point2 = point1;
        
        point2.X = 100;
        Assert.Equal(10, point1.X);
        Assert.Equal(100, point2.X);
    }
    
    [Fact]
    public void ObjectInitializer_InitializesAllProperties()
    {
        var person = new Person
        {
            Name = "Anna",
            Age = 28,
            City = "Kraków"
        };
        
        Assert.Equal("Anna", person.Name);
        Assert.Equal(28, person.Age);
        Assert.Equal("Kraków", person.City);
    }
    
    [Fact]
    public void Equals_ComparesContent()
    {
        var person1 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person2 = new Person { Name = "Jan", Age = 30, City = "Warszawa" };
        var person3 = new Person { Name = "Jan", Age = 31, City = "Warszawa" };
        
        Assert.True(person1.Equals(person2));
        Assert.False(person1.Equals(person3));
    }
    
    [Fact]
    public void BankAccount_DepositIncreasesBalance()
    {
        var account = new BankAccount("John Doe", 1000);
        account.Deposit(500);
        
        Assert.Equal(1500, account.Balance);
    }

    [Fact]
    public void PassingReference_ModifyingObject_IsVisibleToCaller()
    {
        var person = new Person { Name = "Jan" };

        ReferenceDemo.Rename(person);

        Assert.Equal("Zmieniony", person.Name);
    }

    [Fact]
    public void PassingReference_ReassigningParameter_DoesNotAffectCaller()
    {
        var person = new Person { Name = "Jan" };
        var original = person;

        ReferenceDemo.Replace(person);

        Assert.Same(original, person);
        Assert.Equal("Jan", person.Name);
    }

    [Fact]
    public void PassingReference_WithRef_ReplacesCallersVariable()
    {
        var person = new Person { Name = "Jan" };
        var original = person;

        ReferenceDemo.ReplaceByRef(ref person);

        Assert.NotSame(original, person);
        Assert.Equal("Nowy", person.Name);
    }

    [Fact]
    public void NullConditionalOperator_OnNullReference_ReturnsNull()
    {
        Person? nobody = null;

        Assert.Null(nobody?.Name);
        Assert.Throws<NullReferenceException>(() => nobody!.Name.ToString());
    }
    
    [Fact]
    public void CollectionInitializer_CreatesMultipleObjects()
    {
        var people = new List<Person>
        {
            new Person { Name = "Jan", Age = 30, City = "Warszawa" },
            new Person { Name = "Maria", Age = 25, City = "Kraków" }
        };
        
        Assert.Equal(2, people.Count);
        Assert.Equal("Jan", people[0].Name);
        Assert.Equal("Maria", people[1].Name);
    }
}
