using System;
using System.Collections.Generic;
using Xunit;

namespace VirtualPolymorphism;

public abstract class Payment
{
    public abstract void Process();
    public virtual void Log() => Console.WriteLine("Payment processed");
}

public class CreditCard : Payment
{
    public override void Process()
    {
        Console.WriteLine("Processing credit card");
        ValidateCard();
    }
    
    private void ValidateCard() => Console.WriteLine("Card validated");
}

public class PayPal : Payment
{
    public override void Process()
    {
        Console.WriteLine("Processing PayPal");
        AuthenticateUser();
    }
    
    private void AuthenticateUser() => Console.WriteLine("User authenticated");
}

public abstract class Shape
{
    protected string Name { get; set; } = "";
    
    public abstract double CalculateArea();
    public virtual string GetInfo() => $"Shape: {Name}";
}

public class Circle : Shape
{
    private double radius;
    
    public Circle(string name, double radius)
    {
        Name = name;
        this.radius = radius;
    }
    
    public override double CalculateArea() => 3.14 * radius * radius;
    public override string GetInfo() => $"Circle: {Name}, R={radius}";
}

public class Rectangle : Shape
{
    private double width, height;
    
    public Rectangle(string name, double w, double h)
    {
        Name = name;
        width = w;
        height = h;
    }
    
    public override double CalculateArea() => width * height;
    public override string GetInfo() => $"Rectangle: {Name}, {width}x{height}";
}

public class Animal
{
    public virtual string Name { get; set; } = "";
    
    public virtual void Speak()
    {
        Console.WriteLine("Animal sound");
    }
}

public class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Woof!");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== VIRTUAL METHODS & POLYMORPHISM ===\n");
        
        Console.WriteLine("1. Payment Polymorphism:");
        List<Payment> payments = new() 
        { 
            new CreditCard(), 
            new PayPal() 
        };
        foreach (var payment in payments)
        {
            payment.Process();
            payment.Log();
        }
        Console.WriteLine();
        
        Console.WriteLine("2. Shape Polymorphism:");
        List<Shape> shapes = new()
        {
            new Circle("Circ1", 5),
            new Rectangle("Rect1", 3, 4)
        };
        foreach (var shape in shapes)
        {
            Console.WriteLine($"{shape.GetInfo()}, Area={shape.CalculateArea()}");
        }
        Console.WriteLine();
        
        Console.WriteLine("3. Type Checking (is/as):");
        Animal animal = new Dog();
        if (animal is Dog dog)
        {
            dog.Speak();
        }
        Console.WriteLine();
        
        Console.WriteLine("4. Virtual Property:");
        Animal a = new Dog { Name = "Rex" };
        Console.WriteLine($"Name: {a.Name}");
    }
}

public class VirtualPolymorphismTests
{
    [Fact]
    public void Payment_Polymorphism_Works()
    {
        List<Payment> payments = new()
        {
            new CreditCard(),
            new PayPal()
        };
        
        Assert.Equal(2, payments.Count);
    }
    
    [Fact]
    public void Shape_CalculateArea_Polymorphism()
    {
        Shape circle = new Circle("C1", 5);
        Shape rect = new Rectangle("R1", 3, 4);
        
        Assert.True(circle.CalculateArea() > 0);
        Assert.True(rect.CalculateArea() > 0);
        Assert.NotEqual(circle.CalculateArea(), rect.CalculateArea());
    }
    
    [Fact]
    public void Is_Operator_TypeChecking()
    {
        Animal animal = new Dog();
        
        Assert.True(animal is Dog);
        Assert.False(animal is Cat);
    }
    
    [Fact]
    public void As_Operator_SafeCasting()
    {
        Animal animal = new Dog();
        Dog? dog = animal as Dog;
        
        Assert.NotNull(dog);
    }
    
    [Fact]
    public void VirtualProperty_Works()
    {
        Animal dog = new Dog { Name = "Rex" };
        
        Assert.Equal("Rex", dog.Name);
    }
}

// Dla testu - Cat class
public class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Meow!");
    }
}
