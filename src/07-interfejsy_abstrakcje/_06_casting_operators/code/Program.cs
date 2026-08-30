using System;
using System.Collections.Generic;
using Xunit;

namespace CastingOperators;

// ============================================================================
// CZĘŚĆ 1: Basic Types
// ============================================================================

public abstract class AnimalBase
{
    public string Name { get; set; }
    public AnimalBase(string name) => Name = name;
    public abstract void Speak();
}

public class Dog : AnimalBase
{
    public Dog(string name) : base(name) { }
    public override void Speak() => Console.WriteLine("Woof!");
    public void Fetch() => Console.WriteLine("Fetching ball...");
}

public class Cat : AnimalBase
{
    public Cat(string name) : base(name) { }
    public override void Speak() => Console.WriteLine("Meow!");
    public void Scratch() => Console.WriteLine("Scratching...");
}

// ============================================================================
// CZĘŚĆ 2: Cast Operator ()
// ============================================================================

/// <summary>
/// ✅ Cast - assume type is specific type
/// ❌ Throws InvalidCastException if wrong type
/// </summary>
public class CastDemo
{
    public static void Demo()
    {
        AnimalBase animal = new Dog("Buddy");
        
        // ✅ Correct type - works
        var dog = (Dog)animal;
        dog.Fetch();
        
        // ❌ Wrong type - throws exception!
        // var cat = (Cat)animal;  // InvalidCastException!
    }
}

// ============================================================================
// CZĘŚĆ 3: is Operator
// ============================================================================

/// <summary>
/// ✅ is - check if object IS of specific type
/// Returns true/false, does NOT throw
/// Introduced: Pattern matching in C# 7.0+
/// </summary>
public class IsOperatorDemo
{
    public static void Demo()
    {
        List<AnimalBase> animals = new()
        {
            new Dog("Buddy"),
            new Cat("Whiskers"),
            new Dog("Max")
        };
        
        foreach (var animal in animals)
        {
            // ✅ Check type safely
            if (animal is Dog)
            {
                Console.WriteLine($"{animal.Name} is a Dog");
            }
            else if (animal is Cat)
            {
                Console.WriteLine($"{animal.Name} is a Cat");
            }
        }
    }
}

// ============================================================================
// CZĘŚĆ 4: as Operator
// ============================================================================

/// <summary>
/// ✅ as - attempt to cast, returns null if fails
/// Safe cast - does NOT throw exception
/// Good for optional conversions
/// </summary>
public class AsOperatorDemo
{
    public static void Demo()
    {
        AnimalBase animal1 = new Dog("Buddy");
        AnimalBase animal2 = new Cat("Whiskers");
        
        // ✅ Successful cast
        var dog = animal1 as Dog;
        if (dog != null)
        {
            dog.Fetch();
        }
        
        // ✅ Failed cast - returns null (doesn't throw)
        var cat = animal1 as Cat;
        if (cat is null)
        {
            Console.WriteLine("animal1 is not a Cat");
        }
    }
}

// ============================================================================
// CZĘŚĆ 5: Pattern Matching (C# 7.0+)
// ============================================================================

/// <summary>
/// Modern approach: is + pattern matching
/// More readable than separate checks
/// </summary>
public class PatternMatchingDemo
{
    public static void Demo()
    {
        List<AnimalBase> animals = new()
        {
            new Dog("Buddy"),
            new Cat("Whiskers")
        };
        
        foreach (var animal in animals)
        {
            // ✅ Modern pattern matching with is
            if (animal is Dog dog)  // Declares 'dog' variable if match
            {
                Console.WriteLine($"{dog.Name} is a dog");
                dog.Fetch();
            }
            else if (animal is Cat cat)
            {
                Console.WriteLine($"{cat.Name} is a cat");
                cat.Scratch();
            }
        }
    }
}

// ============================================================================
// CZĘŚĆ 6: Type Patterns (C# 9.0+)
// ============================================================================

/// <summary>
/// Not pattern - negation
/// And/Or patterns - combinations
/// </summary>
public class NotPatternDemo
{
    public static void Demo()
    {
        AnimalBase animal = new Dog("Buddy");
        
        // ✅ Not pattern
        if (animal is not Cat)
        {
            Console.WriteLine("This is not a cat");
        }
        
        // ✅ Type pattern with null check
        if (animal is not null)
        {
            Console.WriteLine("Animal is not null");
        }
    }
}

// ============================================================================
// CZĘŚĆ 7: Real-World: Payment Processing
// ============================================================================

public interface IPayment
{
    decimal Amount { get; }
    void Process();
}

public class CreditCardPayment : IPayment
{
    public decimal Amount { get; set; }
    public string CardNumber { get; set; }
    
    public CreditCardPayment(decimal amount, string cardNumber)
    {
        Amount = amount;
        CardNumber = cardNumber;
    }
    
    public void Process()
    {
        Console.WriteLine($"Processing credit card: {CardNumber}");
    }
    
    public void ApplyDiscount(decimal percent)
    {
        Amount *= (1 - percent);
    }
}

public class PayPalPayment : IPayment
{
    public decimal Amount { get; set; }
    public string Email { get; set; }
    
    public PayPalPayment(decimal amount, string email)
    {
        Amount = amount;
        Email = email;
    }
    
    public void Process()
    {
        Console.WriteLine($"Processing PayPal: {Email}");
    }
}

public class PaymentProcessor
{
    public void ProcessPayment(IPayment payment)
    {
        // ✅ Pattern matching with property
        if (payment is CreditCardPayment { Amount: > 1000 } cc)
        {
            Console.WriteLine($"High value card payment: {cc.Amount}");
            cc.ApplyDiscount(0.05m);  // 5% discount
        }
        else if (payment is PayPalPayment pp)
        {
            Console.WriteLine($"PayPal payment: {pp.Amount}");
        }
        
        payment.Process();
    }
}

// ============================================================================
// CZĘŚĆ 8: Comparison Table
// ============================================================================

/*
┌─────────────┬─────────────────────┬─────────────┬──────────────────────┐
│ Operator    │ Syntax              │ Result      │ Throws Exception?    │
├─────────────┼─────────────────────┼─────────────┼──────────────────────┤
│ Cast ()     │ (Dog)animal         │ Dog or Exc  │ ✅ YES - careful!    │
│ is          │ animal is Dog       │ bool        │ ❌ NO - safe         │
│ as          │ animal as Dog       │ Dog or null │ ❌ NO - safe         │
│ is + cast   │ animal is Dog dog   │ bool + var  │ ❌ NO - modern       │
│ not pattern │ animal is not Cat   │ bool        │ ❌ NO - C# 9.0+      │
│ prop pattern│ pay is CC { Amount} │ bool + var  │ ❌ NO - C# 8.0+      │
└─────────────┴─────────────────────┴─────────────┴──────────────────────┘
*/

// ============================================================================
// TESTS
// ============================================================================

public class CastingOperatorsTests
{
    [Fact]
    public void CastOperatorSuccessful()
    {
        AnimalBase animal = new Dog("Buddy");
        var dog = (Dog)animal;
        
        Assert.NotNull(dog);
        Assert.Equal("Buddy", dog.Name);
    }
    
    [Fact]
    public void CastOperatorThrowsException()
    {
        AnimalBase animal = new Dog("Buddy");
        
        // ❌ Throws InvalidCastException
        Assert.Throws<InvalidCastException>(() =>
        {
            var cat = (Cat)animal;
        });
    }
    
    [Fact]
    public void IsOperatorTypeCheck()
    {
        AnimalBase dog = new Dog("Buddy");
        AnimalBase cat = new Cat("Whiskers");
        
        Assert.True(dog is Dog);
        Assert.False(dog is Cat);
        
        Assert.True(cat is Cat);
        Assert.False(cat is Dog);
    }
    
    [Fact]
    public void AsOperatorSafeCast()
    {
        AnimalBase animal = new Dog("Buddy");
        
        // ✅ Successful
        var dog = animal as Dog;
        Assert.NotNull(dog);
        
        // ✅ Failed - returns null
        var cat = animal as Cat;
        Assert.Null(cat);
    }
    
    [Fact]
    public void PatternMatchingWithCast()
    {
        List<AnimalBase> animals = new()
        {
            new Dog("Buddy"),
            new Cat("Whiskers"),
            new Dog("Max")
        };
        
        int dogCount = 0;
        int catCount = 0;
        
        foreach (var animal in animals)
        {
            if (animal is Dog)
                dogCount++;
            else if (animal is Cat)
                catCount++;
        }
        
        Assert.Equal(2, dogCount);
        Assert.Equal(1, catCount);
    }
    
    [Fact]
    public void ModernPatternMatchingWithVariable()
    {
        AnimalBase animal = new Dog("Buddy");
        
        // ✅ is + cast in one expression
        if (animal is Dog dog)
        {
            Assert.Equal("Buddy", dog.Name);
            dog.Fetch();
        }
        else
        {
            Assert.True(false);  // Should not reach here
        }
    }
    
    [Fact]
    public void NotPatternCSharp9()
    {
        AnimalBase animal = new Dog("Buddy");
        
        Assert.True(animal is not Cat);
        Assert.True(animal is not null);
        Assert.False(animal is not Dog);
    }
    
    [Fact]
    public void PaymentProcessingWithPatterns()
    {
        IPayment card = new CreditCardPayment(1500m, "1234-5678");
        IPayment paypal = new PayPalPayment(500m, "user@paypal.com");
        
        var processor = new PaymentProcessor();
        processor.ProcessPayment(card);
        processor.ProcessPayment(paypal);
        
        Assert.NotNull(card);
        Assert.NotNull(paypal);
    }
}
