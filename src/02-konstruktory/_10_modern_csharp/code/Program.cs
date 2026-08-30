using System;
using Xunit;

namespace ModernCSharp;

// ============ RECORD (C# 9+) ============
public record Point(double X, double Y);

public record Person(string Name, int Age)
{
    // Można dodawać własne metody
    public bool IsAdult => Age >= 18;
}

// ============ INIT-ONLY PROPERTIES ============
public class Configuration
{
    public required string AppName { get; init; }
    public required string Version { get; init; }
    public int MaxConnections { get; init; }
}

// ============ PRIMARY CONSTRUCTOR (C# 12+) ============
public class Employee(string name, string department, decimal salary)
{
    public string Name => name;
    public string Department => department;
    public decimal Salary => salary;
    
    public override string ToString() => $"{Name} ({Department}) - ${Salary}";
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 10: NOWOCZESNE C# ===\n");
        
        Console.WriteLine("1. RECORD - Positional:");
        var p1 = new Point(3, 4);
        var p2 = new Point(3, 4);
        Console.WriteLine($"p1: {p1}");
        Console.WriteLine($"p1 == p2: {p1 == p2}");
        Console.WriteLine();
        
        Console.WriteLine("2. RECORD - With-expression:");
        var person1 = new Person("John", 30);
        var person2 = person1 with { Age = 31 };
        Console.WriteLine($"person1: {person1}");
        Console.WriteLine($"person2: {person2}");
        Console.WriteLine($"person1.IsAdult: {person1.IsAdult}");
        Console.WriteLine();
        
        Console.WriteLine("3. INIT-ONLY PROPERTIES:");
        var config = new Configuration
        {
            AppName = "MyApp",
            Version = "1.0",
            MaxConnections = 100
        };
        Console.WriteLine($"AppName: {config.AppName}");
        Console.WriteLine($"Version: {config.Version}");
        Console.WriteLine();
        
        Console.WriteLine("4. PRIMARY CONSTRUCTOR:");
        var emp = new Employee("Alice", "IT", 5000);
        Console.WriteLine(emp);
    }
}

public class ModernCSharpTests
{
    [Fact]
    public void Record_EqualsByValue()
    {
        var p1 = new Point(3, 4);
        var p2 = new Point(3, 4);
        
        Assert.Equal(p1, p2);  // Value equality!
    }
    
    [Fact]
    public void Record_WithExpression_CreatesNewInstance()
    {
        var person1 = new Person("John", 30);
        var person2 = person1 with { Age = 31 };
        
        Assert.NotEqual(person1.Age, person2.Age);
        Assert.Equal("John", person2.Name);
    }
    
    [Fact]
    public void InitProperty_CannotChangeAfterInit()
    {
        var config = new Configuration
        {
            AppName = "App",
            Version = "1.0",
            MaxConnections = 100
        };
        
        // config.MaxConnections = 200;  // Compilation error!
        Assert.Equal(100, config.MaxConnections);
    }
    
    [Fact]
    public void PrimaryConstructor_InitializesProperties()
    {
        var emp = new Employee("Bob", "Finance", 4000);
        
        Assert.Equal("Bob", emp.Name);
        Assert.Equal("Finance", emp.Department);
        Assert.Equal(4000, emp.Salary);
    }
}
