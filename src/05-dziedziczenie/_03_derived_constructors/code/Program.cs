using System;
using Xunit;

namespace DerivedConstructors;

public class Animal
{
    public string Name { get; set; } = "";
    protected int Age { get; set; }
    
    public Animal(string name)
    {
        Name = name;
        Age = 0;
        Console.WriteLine($"Animal constructor: {name}");
    }
    
    public Animal(string name, int age)
    {
        Name = name;
        Age = age;
        Console.WriteLine($"Animal constructor: {name}, age {age}");
    }
}

public class Dog : Animal
{
    public string Breed { get; set; } = "";
    
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
        Console.WriteLine($"Dog constructor: {breed}");
    }
    
    public Dog(string name, int age, string breed) : base(name, age)
    {
        Breed = breed;
        Console.WriteLine($"Dog constructor (with age): {breed}");
    }
}

public class Shape
{
    protected string ShapeName { get; set; } = "";
    
    public Shape(string name) => ShapeName = name;
}

public class Polygon : Shape
{
    protected int Sides { get; set; }
    
    public Polygon(string name, int sides) : base(name)
    {
        Sides = sides;
    }
}

public class Triangle : Polygon
{
    public Triangle(string name) : base(name, 3) { }
}

public class Vehicle
{
    public string Make { get; set; } = "";
    
    public Vehicle()  // Default constructor
    {
        Console.WriteLine("Vehicle default constructor");
    }
}

public class Car : Vehicle
{
    public Car(string make)
    {
        Make = make;
        Console.WriteLine("Car constructor");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== INICJALIZACJA KLAS POCHODNYCH ===\n");
        
        Console.WriteLine("1. Simple base constructor:");
        var dog1 = new Dog("Rex", "Labrador");
        Console.WriteLine();
        
        Console.WriteLine("2. Base constructor with multiple parameters:");
        var dog2 = new Dog("Max", 5, "Golden Retriever");
        Console.WriteLine();
        
        Console.WriteLine("3. Chain of constructors:");
        var triangle = new Triangle("MyTriangle");
        Console.WriteLine();
        
        Console.WriteLine("4. Default base constructor (implicit):");
        var car = new Car("Toyota");
        Console.WriteLine();
    }
}

public class DerivedConstructorsTests
{
    [Fact]
    public void DerivedConstructor_CallsBaseConstructor()
    {
        var dog = new Dog("Rex", "Labrador");
        
        Assert.Equal("Rex", dog.Name);
        Assert.Equal("Labrador", dog.Breed);
    }
    
    [Fact]
    public void DerivedConstructor_WithAge_CallsBaseWithAge()
    {
        var dog = new Dog("Max", 5, "Shepherd");
        
        Assert.Equal("Max", dog.Name);
        Assert.Equal("Shepherd", dog.Breed);
    }
    
    [Fact]
    public void ChainOfConstructors_WorksCorrectly()
    {
        var triangle = new Triangle("MyTriangle");
        
        Assert.NotNull(triangle);
    }
    
    [Fact]
    public void DerivedClass_CanHaveMultipleConstructors()
    {
        var dog1 = new Dog("Rex", "Labrador");
        var dog2 = new Dog("Max", 5, "Shepherd");
        
        Assert.NotEqual(dog1.Name, dog2.Name);
    }
    
    [Fact]
    public void ImplicitDefaultBase_CallsBaseDefaultConstructor()
    {
        var car = new Car("Toyota");
        
        Assert.Equal("Toyota", car.Make);
    }
}
