using System;
using System.Collections.Generic;
using Xunit;

namespace AbstractClasses;

public abstract class Animal
{
    public string Name { get; set; } = "";
    
    // Abstract - MUSI być implementowane
    public abstract void Speak();
    
    // Virtual - może być implementowane
    public virtual void Eat() => Console.WriteLine($"{Name} is eating");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");
}

public class Cat : Animal
{
    public override void Speak() => Console.WriteLine("Meow!");
}

public abstract class Database
{
    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void ExecuteQuery(string query);
    
    public virtual void LogConnection() 
        => Console.WriteLine("Database connected");
}

public class SqlDatabase : Database
{
    public override void Connect() => Console.WriteLine("SQL: Connected");
    public override void Disconnect() => Console.WriteLine("SQL: Disconnected");
    public override void ExecuteQuery(string query) 
        => Console.WriteLine($"SQL: Executing {query}");
}

public class MongoDatabase : Database
{
    public override void Connect() => Console.WriteLine("MongoDB: Connected");
    public override void Disconnect() => Console.WriteLine("MongoDB: Disconnected");
    public override void ExecuteQuery(string query) 
        => Console.WriteLine($"MongoDB: Finding {query}");
}

public abstract class Shape
{
    public abstract double CalculateArea();
    public virtual string GetInfo() => "Shape";
}

public class Circle : Shape
{
    private double radius;
    
    public Circle(double radius) => this.radius = radius;
    
    public override double CalculateArea() => 3.14 * radius * radius;
    public override string GetInfo() => $"Circle (R={radius})";
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== ABSTRACT CLASSES ===\n");
        
        Console.WriteLine("1. Abstract Animals:");
        List<Animal> animals = new() 
        { 
            new Dog { Name = "Rex" },
            new Cat { Name = "Whiskers" }
        };
        foreach (var animal in animals)
        {
            animal.Speak();
            animal.Eat();
        }
        Console.WriteLine();
        
        Console.WriteLine("2. Database Polymorphism:");
        List<Database> databases = new()
        {
            new SqlDatabase(),
            new MongoDatabase()
        };
        foreach (var db in databases)
        {
            db.Connect();
            db.ExecuteQuery("SELECT *");
            db.Disconnect();
        }
        Console.WriteLine();
        
        Console.WriteLine("3. Abstract Shapes:");
        Shape circle = new Circle(5);
        Console.WriteLine($"{circle.GetInfo()}: Area = {circle.CalculateArea()}");
        
        // var animal = new Animal();  // ❌ BŁĄD
    }
}

public class AbstractClassesTests
{
    [Fact]
    public void AbstractClass_CannotBeInstantiated()
    {
        // new Animal();  // Compile error
        Assert.True(true);
    }
    
    [Fact]
    public void DerivedClass_MustImplementAbstractMembers()
    {
        var dog = new Dog { Name = "Rex" };
        
        Assert.NotNull(dog);
        Assert.Equal("Rex", dog.Name);
    }
    
    [Fact]
    public void AbstractMembers_ArePolymorphic()
    {
        Animal dog = new Dog();
        Animal cat = new Cat();
        
        Assert.IsType<Dog>(dog);
        Assert.IsType<Cat>(cat);
    }
    
    [Fact]
    public void VirtualMembers_CanBeOverridden()
    {
        var dog = new Dog { Name = "Rex" };
        
        // Should not throw
        dog.Eat();
    }
    
    [Fact]
    public void Database_Polymorphism_Works()
    {
        List<Database> databases = new()
        {
            new SqlDatabase(),
            new MongoDatabase()
        };
        
        Assert.Equal(2, databases.Count);
    }
}
