using System;
using Xunit;

namespace AutoProperties;

// ============ AUTO PROPERTIES ============
public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
}

// ============ AUTO PROPERTIES Z INICJALIZACJĄ ============
public class Config
{
    public string AppName { get; set; } = "MyApp";
    public int MaxConnections { get; set; } = 100;
    public bool EnableLogging { get; set; } = true;
}

// ============ ASYMETRYCZNE ACCESSORY ============
public class User
{
    public int Id { get; private set; }  // Read-only externally
    public string Username { get; set; } = string.Empty;
    
    public User(int id)
    {
        Id = id;
    }
}

// ============ BACKING FIELD + LOGIKA ============
public class Product
{
    private decimal _price;
    
    public string Name { get; set; } = string.Empty;
    
    public decimal Price
    {
        get { return _price; }
        set
        {
            if (value >= 0)
                _price = value;
        }
    }
}

// ============ OBJECT INITIALIZER Z AUTO PROPERTIES ============
public class Employee
{
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 2: AUTO PROPERTIES ===\n");
        
        Console.WriteLine("1. PROSTE AUTO PROPERTIES:");
        var person = new Person { Name = "John", Age = 30, Email = "john@example.com" };
        Console.WriteLine($"{person.Name} ({person.Age}) - {person.Email}");
        Console.WriteLine();
        
        Console.WriteLine("2. AUTO PROPERTIES Z INICJALIZACJĄ:");
        var config = new Config();
        Console.WriteLine($"AppName: {config.AppName}");
        Console.WriteLine($"MaxConnections: {config.MaxConnections}");
        Console.WriteLine($"EnableLogging: {config.EnableLogging}");
        Console.WriteLine();
        
        Console.WriteLine("3. ASYMETRYCZNE ACCESSORY:");
        var user = new User(42) { Username = "john_doe" };
        Console.WriteLine($"User ID: {user.Id} ({user.Username})");
        // user.Id = 100;  // BŁĄD - private setter
        Console.WriteLine();
        
        Console.WriteLine("4. BACKING FIELD + LOGIKA:");
        var product = new Product { Name = "Laptop", Price = 5000 };
        Console.WriteLine($"{product.Name}: ${product.Price}");
        product.Price = -100;  // Backing field z walidacją
        Console.WriteLine($"Price after invalid set: ${product.Price}");
        Console.WriteLine();
        
        Console.WriteLine("5. OBJECT INITIALIZER:");
        var emp = new Employee
        {
            Name = "Alice",
            Department = "IT",
            Salary = 6000
        };
        Console.WriteLine($"{emp.Name} ({emp.Department}) - ${emp.Salary}");
    }
}

public class AutoPropertiesTests
{
    [Fact]
    public void AutoProperty_StoresValue()
    {
        var person = new Person { Name = "John", Age = 30 };
        
        Assert.Equal("John", person.Name);
        Assert.Equal(30, person.Age);
    }
    
    [Fact]
    public void Config_HasDefaultValues()
    {
        var config = new Config();
        
        Assert.Equal("MyApp", config.AppName);
        Assert.Equal(100, config.MaxConnections);
    }
    
    [Fact]
    public void AsymmetricAccessor_CannotSetExternally()
    {
        var user = new User(42);
        
        // user.Id = 100;  // Compilation error
        Assert.Equal(42, user.Id);
    }
    
    [Fact]
    public void BackingField_ValidatesPrice()
    {
        var product = new Product { Price = 1000 };
        product.Price = -100;  // Invalid
        
        Assert.Equal(1000, product.Price);
    }
    
    [Fact]
    public void ObjectInitializer_SetsAllProperties()
    {
        var emp = new Employee
        {
            Name = "Alice",
            Department = "IT",
            Salary = 5000
        };
        
        Assert.Equal("Alice", emp.Name);
        Assert.Equal("IT", emp.Department);
        Assert.Equal(5000, emp.Salary);
    }
}
