using System;
using Xunit;

#nullable enable

namespace NullablePatterns;

// ============ NON-NULLABLE BY DEFAULT ============
public class Person
{
    public string Name { get; set; } = string.Empty;  // Non-nullable
    public int Age { get; set; }
    public string? Phone { get; set; }  // Nullable
    public string? Email { get; set; }
}

// ============ NULLABLE REFERENCE TYPES ============
public class User
{
    public required string Username { get; init; }
    public string? Email { get; init; }
    public string? Bio { get; init; }
    
    public string GetDisplayName()
    {
        return Email ?? Username;
    }
}

// ============ OPTIONAL PROPERTIES ============
public class Config
{
    public required string AppName { get; init; }
    public required string ConnectionString { get; init; }
    public string? ProxyUrl { get; init; }
    public int Timeout { get; init; } = 30;
}

// ============ DEFENSIVE PROGRAMMING ============
public class Order
{
    public string OrderId { get; init; } = string.Empty;
    public string? CustomerName { get; init; }
    public string? ShippingAddress { get; init; }
    
    public string GetDisplayInfo()
    {
        var customer = CustomerName ?? "Anonymous";
        var address = ShippingAddress ?? "N/A";
        return $"{customer} -> {address}";
    }
}

// ============ NULLABLE VALUE TYPES ============
public class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int? StockQuantity { get; set; }  // Nullable int
    public DateTime? LastRestockDate { get; set; }
    
    public bool IsInStock => StockQuantity.HasValue && StockQuantity.Value > 0;
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 7: NULLABLE REFERENCE TYPES (C# 8+) ===\n");
        
        Console.WriteLine("1. NON-NULLABLE BY DEFAULT:");
        var person = new Person { Name = "John", Age = 30 };
        Console.WriteLine($"Name: {person.Name}");
        // person.Name = null;  // BŁĄD - compiler warning/error
        Console.WriteLine($"Phone: {person.Phone ?? "N/A"}");
        Console.WriteLine();
        
        Console.WriteLine("2. NULLABLE REFERENCE TYPES:");
        var user = new User
        {
            Username = "john_doe",
            Email = "john@example.com"
        };
        Console.WriteLine($"Username: {user.Username}");
        Console.WriteLine($"DisplayName: {user.GetDisplayName()}");
        Console.WriteLine();
        
        Console.WriteLine("3. OPTIONAL PROPERTIES:");
        var config = new Config
        {
            AppName = "MyApp",
            ConnectionString = "Server=localhost"
        };
        Console.WriteLine($"AppName: {config.AppName}");
        Console.WriteLine($"ProxyUrl: {config.ProxyUrl ?? "None"}");
        Console.WriteLine();
        
        Console.WriteLine("4. DEFENSIVE PROGRAMMING:");
        var order = new Order
        {
            OrderId = "ORD-001",
            CustomerName = "Alice"
        };
        Console.WriteLine($"Order: {order.GetDisplayInfo()}");
        
        var order2 = new Order { OrderId = "ORD-002" };
        Console.WriteLine($"Order2: {order2.GetDisplayInfo()}");
        Console.WriteLine();
        
        Console.WriteLine("5. NULLABLE VALUE TYPES:");
        var product = new Product
        {
            Name = "Laptop",
            Price = 999.99m,
            StockQuantity = 5
        };
        Console.WriteLine($"Product: {product.Name}");
        Console.WriteLine($"In Stock: {product.IsInStock}");
        
        var product2 = new Product
        {
            Name = "Phone",
            Price = 599.99m
            // StockQuantity = null (unknown)
        };
        Console.WriteLine($"Product2 In Stock: {product2.IsInStock}");
    }
}

public class NullablePatternsTests
{
    [Fact]
    public void NonNullable_HasDefault()
    {
        var person = new Person { Name = "John" };
        
        Assert.NotNull(person.Name);
        Assert.Equal("John", person.Name);
    }
    
    [Fact]
    public void Nullable_CanBeNull()
    {
        var person = new Person { Name = "John", Phone = null };
        
        Assert.Null(person.Phone);
    }
    
    [Fact]
    public void NullCoalescing_ProvidesFallback()
    {
        var user = new User
        {
            Username = "john_doe",
            Email = null
        };
        
        var displayName = user.GetDisplayName();
        Assert.Equal("john_doe", displayName);
    }
    
    [Fact]
    public void OptionalProperties_AreNull()
    {
        var config = new Config
        {
            AppName = "Test",
            ConnectionString = "Server=test"
        };
        
        Assert.Null(config.ProxyUrl);
    }
    
    [Fact]
    public void DefensiveProgramming_HandlesNull()
    {
        var order = new Order { OrderId = "ORD-001" };
        var display = order.GetDisplayInfo();
        
        Assert.Contains("Anonymous", display);
        Assert.Contains("N/A", display);
    }
    
    [Fact]
    public void NullableValueType_HasValue()
    {
        var product = new Product
        {
            Name = "Laptop",
            Price = 999,
            StockQuantity = 5
        };
        
        Assert.True(product.IsInStock);
    }
    
    [Fact]
    public void NullableValueType_NoValue()
    {
        var product = new Product
        {
            Name = "Phone",
            Price = 599
            // StockQuantity = null
        };
        
        Assert.False(product.IsInStock);
    }
}
