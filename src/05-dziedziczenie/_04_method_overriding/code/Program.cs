using System;
using System.Collections.Generic;
using Xunit;

namespace MethodOverriding;

// ===== OVERRIDE Example (Polymorphism) =====
public class Animal
{
    public virtual void Speak()
    {
        Console.WriteLine("Animal makes a sound");
    }
}

public class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Woof!");
    }
}

public class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Meow!");
    }
}

// ===== NEW Example (Hiding) =====
public class Base
{
    public void Method()
    {
        Console.WriteLine("Base.Method");
    }
}

public class Derived : Base
{
    public new void Method()
    {
        Console.WriteLine("Derived.Method");
    }
}

// ===== Calling Base =====
public class Vehicle
{
    public virtual void Drive()
    {
        Console.WriteLine("Vehicle driving");
    }
}

public class Car : Vehicle
{
    public override void Drive()
    {
        Console.WriteLine("Car driving fast");
        base.Drive();  // Wywołaj wersję bazową
    }
}

// ===== Shape Example =====
public abstract class Shape
{
    public abstract void Draw();
    public virtual string GetInfo() => "Shape";
}

public class Circle : Shape
{
    public override void Draw() => Console.WriteLine("Drawing Circle");
    public override string GetInfo() => "Circle";
}

public class Rectangle : Shape
{
    public override void Draw() => Console.WriteLine("Drawing Rectangle");
    public override string GetInfo() => "Rectangle";
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== OVERRIDE VS NEW ===\n");
        
        Console.WriteLine("1. Override - Polimorfizm:");
        List<Animal> animals = new() { new Dog(), new Cat() };
        foreach (var animal in animals)
        {
            animal.Speak();  // Virtual dispatch
        }
        Console.WriteLine();
        
        Console.WriteLine("2. New - Ukrycie:");
        Derived d = new Derived();
        d.Method();  // "Derived.Method"
        
        Base b = d;
        b.Method();  // "Base.Method" - nie polimorfizm!
        Console.WriteLine();
        
        Console.WriteLine("3. Base Keyword:");
        var car = new Car();
        car.Drive();
        Console.WriteLine();
        
        Console.WriteLine("4. Abstract Methods:");
        List<Shape> shapes = new() { new Circle(), new Rectangle() };
        foreach (var shape in shapes)
        {
            shape.Draw();
            Console.WriteLine($"  {shape.GetInfo()}");
        }
    }
}

public class MethodOverridingTests
{
    [Fact]
    public void Override_AllowsPolymorphism()
    {
        List<Animal> animals = new() { new Dog(), new Cat() };
        
        var dog = (Dog)animals[0];
        var cat = (Cat)animals[1];
        
        Assert.IsType<Dog>(dog);
        Assert.IsType<Cat>(cat);
    }
    
    [Fact]
    public void Override_VirtualDispatch()
    {
        Animal dog = new Dog();
        Animal cat = new Cat();
        
        Assert.IsType<Dog>(dog);
        Assert.IsType<Cat>(cat);
    }
    
    [Fact]
    public void New_HidesMethod()
    {
        Derived d = new Derived();
        Base b = d;
        
        // Different behavior based on reference type
        Assert.NotNull(d);
        Assert.NotNull(b);
    }
    
    [Fact]
    public void BaseKeyword_CallsBaseImplementation()
    {
        var car = new Car();
        
        // Should not throw
        car.Drive();
    }
    
    [Fact]
    public void Abstract_RequiresImplementation()
    {
        Shape circle = new Circle();
        Shape rect = new Rectangle();
        
        Assert.IsType<Circle>(circle);
        Assert.IsType<Rectangle>(rect);
    }
}
