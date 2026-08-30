using System;
using System.Collections.Generic;
using Xunit;

namespace AbstractClassesIntro;

// ============================================================================
// CZĘŚĆ 1: Basics of Abstract Classes
// ============================================================================

/// <summary>
/// Klasa abstrakcyjna - nie może być instantiowana bezpośrednio
/// Konwencja: dodajemy przyrostek "Base" do nazwy
/// </summary>
public abstract class AnimalBase
{
    public string Name { get; set; }
    
    public AnimalBase(string name)
    {
        Name = name;
    }
    
    // Konkretna metoda w klasie abstrakcyjnej
    public void Sleep()
    {
        Console.WriteLine($"{Name} is sleeping...");
    }
    
    // Abstrakcyjna metoda - musi być zaimplementowana w klasach pochodnych
    public abstract void Speak();
    public abstract string Describe();
}

/// <summary>
/// Klasa konkretna dziedzicząca po klasie abstrakcyjnej
/// </summary>
public class Dog : AnimalBase
{
    public Dog(string name) : base(name) { }
    
    // ✅ Musimy implementować abstrakcyjne metody
    public override void Speak()
    {
        Console.WriteLine($"{Name}: Woof! Woof!");
    }
    
    public override string Describe()
    {
        return $"Dog named {Name}";
    }
}

public class Cat : AnimalBase
{
    public Cat(string name) : base(name) { }
    
    public override void Speak()
    {
        Console.WriteLine($"{Name}: Meow!");
    }
    
    public override string Describe()
    {
        return $"Cat named {Name}";
    }
}

// ============================================================================
// CZĘŚĆ 2: Shape Library - Realistic Example
// ============================================================================

/// <summary>
/// Abstrakcyjna klasa bazowa dla wszystkich figur geometrycznych
/// </summary>
public abstract class ShapeBase
{
    public abstract string Name { get; }
    public abstract double GetArea();
    public abstract double GetPerimeter();
    
    public virtual string GetDescription()
    {
        return $"{Name} - Area: {GetArea():F2}, Perimeter: {GetPerimeter():F2}";
    }
}

public class Circle : ShapeBase
{
    private readonly double _radius;
    
    public override string Name => "Circle";
    
    public Circle(double radius)
    {
        _radius = radius;
    }
    
    public override double GetArea() => Math.PI * _radius * _radius;
    public override double GetPerimeter() => 2 * Math.PI * _radius;
}

public class Rectangle : ShapeBase
{
    private readonly double _width;
    private readonly double _height;
    
    public override string Name => "Rectangle";
    
    public Rectangle(double width, double height)
    {
        _width = width;
        _height = height;
    }
    
    public override double GetArea() => _width * _height;
    public override double GetPerimeter() => 2 * (_width + _height);
}

public class Triangle : ShapeBase
{
    private readonly double _a, _b, _c;
    
    public override string Name => "Triangle";
    
    public Triangle(double a, double b, double c)
    {
        _a = a;
        _b = b;
        _c = c;
    }
    
    public override double GetArea()
    {
        // Heron's formula
        double s = (_a + _b + _c) / 2;
        return Math.Sqrt(s * (s - _a) * (s - _b) * (s - _c));
    }
    
    public override double GetPerimeter() => _a + _b + _c;
}

// ============================================================================
// CZĘŚĆ 3: Real-World Example - Payment Processing
// ============================================================================

/// <summary>
/// Abstract base class for all payment methods
/// </summary>
public abstract class PaymentMethodBase
{
    public string TransactionId { get; protected set; }
    
    public abstract void Authorize(decimal amount);
    public abstract void Charge(decimal amount);
    public abstract void Refund(decimal amount);
    
    // Concrete method shared by all payment methods
    public virtual void PrintReceipt(decimal amount, string status)
    {
        Console.WriteLine($"Receipt: {TransactionId} - Amount: ${amount} - Status: {status}");
    }
}

public class CreditCardPayment : PaymentMethodBase
{
    private readonly string _cardNumber;
    private decimal _authorizedAmount = 0;
    
    public CreditCardPayment(string cardNumber)
    {
        _cardNumber = cardNumber;
        TransactionId = Guid.NewGuid().ToString();
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"[CC] Authorizing ${amount} on card ending with {_cardNumber[^4..]}");
        _authorizedAmount = amount;
    }
    
    public override void Charge(decimal amount)
    {
        if (amount <= _authorizedAmount)
        {
            Console.WriteLine($"[CC] Charging ${amount}");
        }
        else
        {
            Console.WriteLine($"[CC] ❌ Charge amount exceeds authorized amount!");
        }
    }
    
    public override void Refund(decimal amount)
    {
        Console.WriteLine($"[CC] Refunding ${amount} to card ending with {_cardNumber[^4..]}");
        _authorizedAmount -= amount;
    }
}

public class PayPalPayment : PaymentMethodBase
{
    private readonly string _email;
    
    public PayPalPayment(string email)
    {
        _email = email;
        TransactionId = Guid.NewGuid().ToString();
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"[PayPal] Authorizing ${amount} for {_email}");
    }
    
    public override void Charge(decimal amount)
    {
        Console.WriteLine($"[PayPal] Charging ${amount}");
    }
    
    public override void Refund(decimal amount)
    {
        Console.WriteLine($"[PayPal] Refunding ${amount} to {_email}");
    }
}

// ============================================================================
// TESTS
// ============================================================================

public class AbstractClassesTests
{
    [Fact]
    public void CannotInstantiateAbstractClass()
    {
        // ❌ This would not compile:
        // var animal = new AnimalBase("Generic");  // Compiler error!
        
        // ✅ Must instantiate concrete class instead
        var dog = new Dog("Buddy");
        Assert.NotNull(dog);
    }
    
    [Fact]
    public void AbstractMethodsMustBeImplemented()
    {
        var dog = new Dog("Rex");
        dog.Speak();  // Should print "Rex: Woof! Woof!"
        
        var description = dog.Describe();
        Assert.Contains("Dog", description);
    }
    
    [Fact]
    public void ConcreteMethodsInheritedFromAbstractClass()
    {
        var cat = new Cat("Whiskers");
        cat.Sleep();  // Inherited concrete method
        
        Assert.Equal("Whiskers", cat.Name);
    }
    
    [Fact]
    public void PolymorphismWithAbstractClasses()
    {
        // ✅ CAN use abstract class as type for references!
        List<AnimalBase> animals = new()
        {
            new Dog("Buddy"),
            new Cat("Whiskers")
        };
        
        foreach (var animal in animals)
        {
            animal.Speak();  // Calls overridden method
        }
        
        Assert.Equal(2, animals.Count);
    }
    
    [Fact]
    public void ShapesGeometry()
    {
        // Shapes library
        List<ShapeBase> shapes = new()
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(3, 4, 5)
        };
        
        foreach (var shape in shapes)
        {
            Console.WriteLine(shape.GetDescription());
        }
        
        // Calculate total area
        double totalArea = 0;
        foreach (var shape in shapes)
        {
            totalArea += shape.GetArea();
        }
        
        Assert.True(totalArea > 0);
    }
    
    [Fact]
    public void PaymentProcessing()
    {
        // Abstract base class as reference type
        PaymentMethodBase payment = new CreditCardPayment("1234-5678-9012-3456");
        
        payment.Authorize(100m);
        payment.Charge(50m);
        payment.PrintReceipt(50m, "Completed");
        payment.Refund(10m);
        
        Assert.NotNull(payment.TransactionId);
    }
    
    [Fact]
    public void MultiplePaymentMethods()
    {
        List<PaymentMethodBase> methods = new()
        {
            new CreditCardPayment("1234-5678-9012-3456"),
            new PayPalPayment("user@paypal.com")
        };
        
        foreach (var method in methods)
        {
            method.Authorize(100m);
            method.Charge(100m);
            method.PrintReceipt(100m, "Completed");
        }
        
        Assert.Equal(2, methods.Count);
    }
    
    [Fact]
    public void VirtualMethodsInAbstractClass()
    {
        Circle circle = new Circle(5);
        
        // Virtual method can be overridden
        string description = circle.GetDescription();
        Assert.Contains("Circle", description);
        Assert.Contains("Area", description);
    }
}
