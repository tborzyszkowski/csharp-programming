using System;
using Xunit;

namespace InitProperties;

// ============ INIT-ONLY PROPERTIES (C# 9+) ============
public class Person
{
    public string Name { get; init; } = string.Empty;
    public int Age { get; init; }
    public string Email { get; init; } = string.Empty;
}

// ============ REQUIRED KEYWORD (C# 11+) ============
public class User
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; } = true;
}

// ============ CONFIGURATION ============
public class ApiConfig
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
    public int Timeout { get; init; } = 30;
    public bool EnableSSL { get; init; } = true;
}

// ============ DTO (DATA TRANSFER OBJECT) ============
public class ProductDto
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public decimal Price { get; init; }
    public string? Description { get; init; }
}

// ============ BUILDER PATTERN Z INIT ============
public class AppSettings
{
    public required string AppName { get; init; }
    public string? Theme { get; init; }
    public int MaxConnections { get; init; } = 100;
    public bool EnableLogging { get; init; } = true;
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 6: INIT PROPERTIES (C# 9+) I REQUIRED (C# 11+) ===\n");
        
        Console.WriteLine("1. INIT-ONLY PROPERTIES:");
        var person = new Person { Name = "John", Age = 30, Email = "john@example.com" };
        Console.WriteLine($"{person.Name} ({person.Age}) - {person.Email}");
        // person.Name = "Jane";  // BŁĄD - init-only
        Console.WriteLine();
        
        Console.WriteLine("2. REQUIRED KEYWORD:");
        var user = new User
        {
            Email = "user@example.com",
            Username = "john_doe",
            Phone = "123-456-789"
        };
        Console.WriteLine($"User: {user.Username} ({user.Email})");
        // var invalid = new User { };  // BŁĄD - Email i Username required
        Console.WriteLine();
        
        Console.WriteLine("3. API CONFIGURATION:");
        var config = new ApiConfig
        {
            BaseUrl = "https://api.example.com",
            ApiKey = "secret123",
            Timeout = 60
        };
        Console.WriteLine($"BaseUrl: {config.BaseUrl}");
        Console.WriteLine($"Timeout: {config.Timeout}s");
        Console.WriteLine();
        
        Console.WriteLine("4. DTO (DATA TRANSFER OBJECT):");
        var product = new ProductDto
        {
            Id = 1,
            Name = "Laptop",
            Price = 999.99m,
            Description = "High-performance laptop"
        };
        Console.WriteLine($"Product {product.Id}: {product.Name} (${product.Price})");
        Console.WriteLine();
        
        Console.WriteLine("5. APP SETTINGS:");
        var settings = new AppSettings
        {
            AppName = "MyApp",
            Theme = "dark",
            MaxConnections = 200
        };
        Console.WriteLine($"AppName: {settings.AppName}");
        Console.WriteLine($"Theme: {settings.Theme}");
        Console.WriteLine($"EnableLogging: {settings.EnableLogging}");
    }
}

public class InitPropertiesTests
{
    [Fact]
    public void InitProperty_CanSetDuringInit()
    {
        var person = new Person { Name = "John", Age = 30 };
        
        Assert.Equal("John", person.Name);
        Assert.Equal(30, person.Age);
    }
    
    [Fact]
    public void InitProperty_CannotModifyAfterInit()
    {
        var person = new Person { Name = "John" };
        
        // person.Name = "Jane";  // Compilation error
        Assert.Equal("John", person.Name);
    }
    
    [Fact]
    public void RequiredProperty_MustBeSet()
    {
        // var user = new User { };  // Compilation error
        var user = new User
        {
            Email = "test@example.com",
            Username = "test"
        };
        
        Assert.Equal("test@example.com", user.Email);
    }
    
    [Fact]
    public void RequiredProperty_OptionalFieldsCanBeNull()
    {
        var user = new User
        {
            Email = "test@example.com",
            Username = "test"
            // Phone is optional - can be null
        };
        
        Assert.Null(user.Phone);
    }
    
    [Fact]
    public void ApiConfig_HasDefaults()
    {
        var config = new ApiConfig
        {
            BaseUrl = "https://api.test.com",
            ApiKey = "key123"
        };
        
        Assert.Equal(30, config.Timeout);
        Assert.True(config.EnableSSL);
    }
    
    [Fact]
    public void Dto_WorksWithOptionalFields()
    {
        var product = new ProductDto
        {
            Id = 1,
            Name = "Phone"
            // Description is optional
        };
        
        Assert.Equal(1, product.Id);
        Assert.Null(product.Description);
    }
}
