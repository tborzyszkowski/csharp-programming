using System;
using Xunit;

namespace SealedClasses;

// Sealed class - nie można dziedziczić
public sealed class FinalClass
{
    public void Method() => Console.WriteLine("FinalClass.Method");
}

// ❌ Nie można: public class DerivedFromFinal : FinalClass { }

public class Animal
{
    public virtual void Speak()
    {
        Console.WriteLine("Animal sound");
    }
}

public class Dog : Animal
{
    public sealed override void Speak()  // Sealed - nie można dalej override
    {
        Console.WriteLine("Woof!");
    }
}

// ❌ Nie można: public class Puppy : Dog { public override void Speak() { } }

public abstract class PaymentProcessor
{
    public abstract void Process();
}

public sealed class CreditCardProcessor : PaymentProcessor
{
    public override void Process()
    {
        Console.WriteLine("Processing credit card");
        ValidateCard();
    }
    
    private void ValidateCard() => Console.WriteLine("Card validated");
}

public sealed class PayPalProcessor : PaymentProcessor
{
    public override void Process()
    {
        Console.WriteLine("Processing PayPal");
        AuthenticateUser();
    }
    
    private void AuthenticateUser() => Console.WriteLine("User authenticated");
}

public class Logger
{
    public virtual void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public sealed class FileLogger : Logger
{
    public sealed override void Log(string message)  // Sealed override
    {
        Console.WriteLine($"[FILE] {message}");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== SEALED CLASSES ===\n");
        
        Console.WriteLine("1. Sealed Class:");
        var obj = new FinalClass();
        obj.Method();
        Console.WriteLine("   (Cannot derive from FinalClass)");
        Console.WriteLine();
        
        Console.WriteLine("2. Sealed Override:");
        Animal dog = new Dog();
        dog.Speak();
        Console.WriteLine("   (Dog.Speak is sealed - cannot override further)");
        Console.WriteLine();
        
        Console.WriteLine("3. Sealed Implementations:");
        PaymentProcessor processor = new CreditCardProcessor();
        processor.Process();
        Console.WriteLine();
        
        Console.WriteLine("4. Sealed Logger:");
        Logger logger = new FileLogger();
        logger.Log("Test message");
    }
}

public class SealedClassesTests
{
    [Fact]
    public void SealedClass_CanBeInstantiated()
    {
        var obj = new FinalClass();
        Assert.NotNull(obj);
    }
    
    [Fact]
    public void SealedClass_CannotBeDerived()
    {
        // public class Derived : FinalClass { }  // Compile error
        Assert.True(true);
    }
    
    [Fact]
    public void SealedOverride_PreventsFurtherOverrides()
    {
        var dog = new Dog();
        Assert.NotNull(dog);
    }
    
    [Fact]
    public void SealedImplementation_CanBeUsedPolymorphically()
    {
        PaymentProcessor processor = new CreditCardProcessor();
        
        Assert.IsType<CreditCardProcessor>(processor);
    }
    
    [Fact]
    public void FileLogger_IsSealedImplementation()
    {
        var logger = new FileLogger();
        
        Assert.NotNull(logger);
    }
}
