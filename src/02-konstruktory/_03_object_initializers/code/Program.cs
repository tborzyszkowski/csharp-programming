using System;
using System.Collections.Generic;
using Xunit;

namespace ObjectInitializers;

/// <summary>
/// Temat 3: Inicjalizatory Obiektów
/// Demonstracja object initializers i collection initializers
/// </summary>

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public string City { get; set; } = "Unknown";
}

public class Car
{
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public int Mileage { get; set; }
}

public class Team
{
    public string Name { get; set; } = string.Empty;
    public List<string> Members { get; set; } = new();
    public int BudgetInThousands { get; set; }
}

public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}

public class Employee
{
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    public List<string> Skills { get; set; } = new();
}

// init + required: wymuszone przy tworzeniu i niezmienne później
public class User
{
    public required string Email { get; init; }
    public string? Nick { get; init; }
}

// Setter ze skutkiem ubocznym - dowodzi, że inicjalizator wywołuje settery
public class AuditedItem
{
    public int SetterCalls { get; private set; }
    
    private string name = "";
    public string Name
    {
        get => name;
        set { name = value; SetterCalls++; }
    }
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== TEMAT 3: INICJALIZATORY OBIEKTÓW ===\n");
        
        // 1. Object Initializer - prosty
        Console.WriteLine("1. OBJECT INITIALIZER - PROSTY");
        var person1 = new Person
        {
            Name = "Anna",
            Age = 30,
            Email = "anna@example.com",
            City = "Warszawa"
        };
        Console.WriteLine($"{person1.Name}, {person1.Age} lat, email: {person1.Email}");
        Console.WriteLine();
        
        // 2. Collection Initializer - lista
        Console.WriteLine("2. COLLECTION INITIALIZER - LISTA");
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        Console.WriteLine($"Numbers: {string.Join(", ", numbers)}");
        Console.WriteLine();
        
        // 3. Collection Initializer - dictionary
        Console.WriteLine("3. COLLECTION INITIALIZER - DICTIONARY");
        var ages = new Dictionary<string, int>
        {
            ["Alice"] = 25,
            ["Bob"] = 30,
            ["Charlie"] = 28
        };
        foreach (var kvp in ages)
        {
            Console.WriteLine($"  {kvp.Key}: {kvp.Value} lat");
        }
        Console.WriteLine();
        
        // 4. Nested Initializer - obiekt z kolekcją
        Console.WriteLine("4. NESTED INITIALIZER - OBIEKT Z KOLEKCJĄ");
        var team = new Team
        {
            Name = "Backend Team",
            Members = { "Alice", "Bob", "Charlie" },  // Collection initializer
            BudgetInThousands = 50
        };
        Console.WriteLine($"Team: {team.Name}");
        Console.WriteLine($"Members: {string.Join(", ", team.Members)}");
        Console.WriteLine();
        
        // 5. Deeply Nested Initializer
        Console.WriteLine("5. DEEPLY NESTED INITIALIZER");
        var employee = new Employee
        {
            Name = "John Doe",
            Department = "IT",
            Address = new Address
            {
                Street = "Piotrkowska 100",
                City = "Łódź",
                PostalCode = "90-001"
            },
            Skills = { "C#", "SQL", "Azure" }
        };
        Console.WriteLine($"{employee.Name} z {employee.Department}");
        Console.WriteLine($"Skills: {string.Join(", ", employee.Skills)}");
        Console.WriteLine($"Adres: {employee.Address.Street}, {employee.Address.City}");
        Console.WriteLine();
        
        // 6. Target-typed initializer (C# 9+)
        Console.WriteLine("6. TARGET-TYPED INITIALIZER");
        Car car = new()
        {
            Brand = "Tesla",
            Model = "Model 3",
            Year = 2024
        };
        Console.WriteLine($"{car.Brand} {car.Model} ({car.Year})");
        Console.WriteLine();
        
        // 7. Wyrażenia kolekcji (C# 12)
        Console.WriteLine("7. WYRAŻENIA KOLEKCJI (C# 12)");
        List<int> list = [1, 2, 3];
        int[] more = [..list, 4, 5];
        Console.WriteLine($"list: {string.Join(", ", list)}; more: {string.Join(", ", more)}");
    }
}

public class InitializerTests
{
    [Fact]
    public void ObjectInitializer_SetsAllProperties()
    {
        var person = new Person
        {
            Name = "John",
            Age = 25,
            Email = "john@example.com"
        };
        
        Assert.Equal("John", person.Name);
        Assert.Equal(25, person.Age);
        Assert.Equal("john@example.com", person.Email);
    }
    
    [Fact]
    public void CollectionInitializer_AddsAllItems()
    {
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        
        Assert.Equal(5, numbers.Count);
        Assert.Contains(3, numbers);
    }
    
    [Fact]
    public void DictionaryInitializer_SetsKeyValues()
    {
        var ages = new Dictionary<string, int>
        {
            ["Alice"] = 25,
            ["Bob"] = 30
        };
        
        Assert.Equal(25, ages["Alice"]);
        Assert.Equal(30, ages["Bob"]);
    }
    
    [Fact]
    public void NestedInitializer_SetsNestedObjects()
    {
        var employee = new Employee
        {
            Name = "Jane",
            Address = new Address
            {
                City = "Gdańsk"
            }
        };
        
        Assert.Equal("Jane", employee.Name);
        Assert.Equal("Gdańsk", employee.Address.City);
    }
    
    [Fact]
    public void NestedInitializer_WithCollections()
    {
        var team = new Team
        {
            Name = "DevOps",
            Members = { "Alice", "Bob" }
        };
        
        Assert.Equal(2, team.Members.Count);
    }
    
    [Fact]
    public void CollectionInitializer_WithoutNew_AddsToExistingCollection()
    {
        var team = new Team { Members = { "Alice" } };
        var originalList = team.Members;
        
        var other = new Team { Members = { "Bob" } };
        
        Assert.Single(team.Members);
        Assert.Single(other.Members);
        Assert.Same(originalList, team.Members);   // ta sama lista, nie nowa
        Assert.NotSame(team.Members, other.Members);
    }
    
    [Fact]
    public void DictionaryInitializer_AddThrowsOnDuplicate_IndexerOverwrites()
    {
        Assert.Throws<ArgumentException>(() => new Dictionary<string, int>
        {
            { "Anna", 1 },
            { "Anna", 2 }   // Add -> wyjątek
        });
        
        var ok = new Dictionary<string, int>
        {
            ["Anna"] = 1,
            ["Anna"] = 2    // indeksator -> nadpisanie
        };
        Assert.Equal(2, ok["Anna"]);
    }
    
    [Fact]
    public void ObjectInitializer_CallsSetters()
    {
        var item = new AuditedItem { Name = "x" };
        
        Assert.Equal(1, item.SetterCalls);
    }
    
    [Fact]
    public void ObjectInitializer_WithFailingSetter_DoesNotAssignVariable()
    {
        Person? person = null;
        
        // Wyjątek rzucony w trakcie inicjalizatora - zmienna pozostaje nieprzypisana
        Assert.Throws<InvalidOperationException>(() =>
        {
            person = new Person { Name = "Jan", Age = Fail() };
        });
        
        Assert.Null(person);
        
        static int Fail() => throw new InvalidOperationException();
    }
    
    [Fact]
    public void RequiredInitProperty_SetInInitializer_IsImmutableAfterwards()
    {
        var user = new User { Email = "jan@example.com" };
        
        Assert.Equal("jan@example.com", user.Email);
        Assert.Null(user.Nick);
        
        // init-only: setter ma modyfikator IsExternalInit
        var setter = typeof(User).GetProperty(nameof(User.Email))!.SetMethod!;
        Assert.Contains(setter.ReturnParameter.GetRequiredCustomModifiers(),
            m => m.Name == "IsExternalInit");
    }
    
    [Fact]
    public void CollectionExpression_CreatesCollectionsAndSupportsSpread()
    {
        List<int> list = [1, 2, 3];
        int[] more = [..list, 4, 5];
        
        Assert.Equal(new[] { 1, 2, 3, 4, 5 }, more);
    }
}
