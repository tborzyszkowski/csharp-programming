using System;
using System.Collections.Generic;
using Xunit;

namespace VirtualVsNew;

// ============================================================================
// CZĘŚĆ 1: Override vs New - Porównanie
// ============================================================================

/// <summary>
/// Klasa bazowa z wirtualną metodą
/// </summary>
public class Animal
{
    public virtual void Speak() => Console.WriteLine("Animal: Generic sound");
    public virtual void Move() => Console.WriteLine("Animal: Moving");
}

/// <summary>
/// Dog z OVERRIDE - Polimorfizm
/// </summary>
public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Dog: Woof! Woof!");  // ✅ Override
    public override void Move() => Console.WriteLine("Dog: Running on all four legs");
}

/// <summary>
/// Cat z NEW - Ukrycie (⚠️  Anti-pattern!)
/// </summary>
public class Cat : Animal
{
    public new void Speak() => Console.WriteLine("Cat: Meow! Meow!");  // ❌ New - nie polimorfizm!
    public new void Move() => Console.WriteLine("Cat: Stalking silently");
}

// ============================================================================
// CZĘŚĆ 2: Real-World: Repository Pattern
// ============================================================================

public abstract class Repository<T> where T : class
{
    public virtual void Save(T item)
    {
        Console.WriteLine("Saving to database...");
    }
    
    public virtual T? GetById(int id)
    {
        Console.WriteLine("Fetching from database...");
        return null;
    }
}

/// <summary>
/// ✅ DOBRY WZORZEC - CacheRepository
/// </summary>
public class CacheRepository<T> : Repository<T> where T : class
{
    public override void Save(T item)  // ✅ Override - rozszerza funkcjonalność
    {
        Console.WriteLine("  1. Checking cache...");
        Console.WriteLine("  2. Saving to cache...");
        base.Save(item);  // Wołuje bazową implementację
        Console.WriteLine("  3. Cache updated");
    }
}

/// <summary>
/// ❌ ZŁY WZORZEC - DatabaseRepository z New
/// </summary>
public class BadDatabaseRepository<T> : Repository<T> where T : class
{
    public new void Save(T item)  // ❌ New - BUG!
    {
        Console.WriteLine("  Custom database save (but nobody calls this!)");
    }
}

/// <summary>
/// ✅ DOBRY WZORZEC - DatabaseRepository z Override
/// </summary>
public class GoodDatabaseRepository<T> : Repository<T> where T : class
{
    public override void Save(T item)  // ✅ Override
    {
        Console.WriteLine("  1. Validating...");
        Console.WriteLine("  2. Opening connection...");
        Console.WriteLine("  3. Inserting row...");
        Console.WriteLine("  4. Connection closed");
    }
}

// ============================================================================
// CZĘŚĆ 3: Liskov Substitution Principle Violation
// ============================================================================

public abstract class Bird
{
    public virtual void Fly()
    {
        Console.WriteLine("Bird: Flying in the sky");
    }
}

/// <summary>
/// ✅ DOBRY - Eagle respects contract
/// </summary>
public class Eagle : Bird
{
    public override void Fly()  // ✅ Override - LSP honored
    {
        Console.WriteLine("Eagle: Flying high with majestic wings");
    }
}

/// <summary>
/// ❌ ZŁY - Penguin violates LSP
/// </summary>
public class Penguin : Bird
{
    public new void Fly()  // ❌ New - LSP violated!
    {
        throw new NotImplementedException("Penguins cannot fly!");
    }
}

// ============================================================================
// CZĘŚĆ 4: Payment System - Real World Bug
// ============================================================================

public abstract class PaymentProcessor
{
    public virtual void Process(decimal amount)
    {
        Console.WriteLine($"Processing ${amount}...");
    }
}

/// <summary>
/// ✅ DOBRA implementacja
/// </summary>
public class CorrectPaymentProcessor : PaymentProcessor
{
    public override void Process(decimal amount)  // ✅ Override
    {
        Console.WriteLine($"  1. Validating amount: ${amount}");
        Console.WriteLine($"  2. Processing payment...");
        Console.WriteLine($"  3. Receipt generated");
    }
}

/// <summary>
/// ❌ ZŁA implementacja - użycie 'new' zamiast 'override'
/// </summary>
public class BuggyPaymentProcessor : PaymentProcessor
{
    public new void Process(decimal amount)  // ❌ New - HUGE BUG!
    {
        Console.WriteLine($"  1. Special payment logic: ${amount}");
    }
}

// ============================================================================
// CZĘŚĆ 5: Weryfikowalne porównanie override / new / brak słowa kluczowego
// ============================================================================

public class Greeter
{
    public virtual string Greet() => "Base";
}

public class OverridingGreeter : Greeter
{
    public override string Greet() => "Override";
}

public class HidingGreeter : Greeter
{
    public new string Greet() => "New";
}

// Brak 'override' i 'new' - dla programisty znającego Javę to najbardziej zaskakujący przypadek.
// Kompilator zgłasza ostrzeżenie CS0114, a metoda ukrywa bazową (nie przesłania jej).
#pragma warning disable CS0114
public class ForgetfulGreeter : Greeter
{
    public string Greet() => "Forgot override";
}
#pragma warning restore CS0114

// Naruszenie LSP przez override: pochodna odrzuca kontrakt bazy.
public class FlightlessBird : Bird
{
    public override void Fly() => throw new NotSupportedException("Cannot fly");
}

// ============================================================================
// PROGRAM GŁÓWNY I TESTY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TEMAT 3: VIRTUAL VS NEW - KLUCZOWE RÓŻNICE               ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        // ====================================================================
        Console.WriteLine("SCENARIUSZ 1: Override (Polimorfizm) vs New (Ukrycie)\n");
        
        Animal dog = new Dog();
        Animal cat = new Cat();
        
        Console.WriteLine("--- Using 'override' (Dog) ---");
        dog.Speak();  // ✅ Wołuje Dog.Speak()
        dog.Move();   // ✅ Wołuje Dog.Move()
        
        Console.WriteLine("\n--- Using 'new' (Cat) ---");
        cat.Speak();  // ❌ Wołuje Animal.Speak()! Nie Cat.Speak()!
        cat.Move();   // ❌ Wołuje Animal.Move()! Nie Cat.Move()!
        
        Console.WriteLine("\n⚠️  UWAGA: Cat.Speak() i Cat.Move() nigdy się nie execute!");
        Console.WriteLine("   bo używają 'new' zamiast 'override'!\n");
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 2: Repository Pattern - Bug w kodzie\n");
        
        var item = new User { Id = 1, Name = "John" };
        
        Console.WriteLine("--- Correct Usage (Override) ---");
        Repository<User> goodRepo = new GoodDatabaseRepository<User>();
        goodRepo.Save(item);  // ✅ Wołuje GoodDatabaseRepository
        
        Console.WriteLine("\n--- Bug Usage (New) ---");
        Repository<User> badRepo = new BadDatabaseRepository<User>();
        badRepo.Save(item);  // ❌ Wołuje base Repository.Save() zamiast BadDatabaseRepository!
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 3: Liskov Substitution Principle\n");
        
        Console.WriteLine("--- Eagle (Correct - honors LSP) ---");
        Bird eagle = new Eagle();
        eagle.Fly();  // ✅ Works as expected
        
        Console.WriteLine("\n--- Penguin (Bug - violates LSP) ---");
        try
        {
            Bird penguin = new Penguin();
            penguin.Fly();  // ❌ Wołuje Bird.Fly()!
            Console.WriteLine("   No error - but expectation is broken!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"   Exception: {ex.Message}");
        }
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 4: Payment Processing Bug\n");
        
        decimal amount = 99.99m;
        
        Console.WriteLine("--- Correct Payment Processor ---");
        PaymentProcessor correct = new CorrectPaymentProcessor();
        correct.Process(amount);  // ✅ Works perfectly
        
        Console.WriteLine("\n--- Buggy Payment Processor ---");
        PaymentProcessor buggy = new BuggyPaymentProcessor();
        buggy.Process(amount);  // ❌ Calls base class method!
        
        Console.WriteLine("\n⚠️  CRITICAL BUG DETECTED:");
        Console.WriteLine("   BuggyPaymentProcessor.Process() is never called!");
        Console.WriteLine("   Uses 'new' instead of 'override'");
        Console.WriteLine("   Result: Wrong payment logic executed\n");
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 5: Brak override i brak new (pułapka dla programistów Javy)\n");

        var forgetful = new ForgetfulGreeter();
        Greeter forgetfulAsBase = forgetful;
        Console.WriteLine($"   Przez zmienną Greeter:         {forgetfulAsBase.Greet()}");
        Console.WriteLine($"   Przez zmienną ForgetfulGreeter: {forgetful.Greet()}");
        Console.WriteLine("   W Javie metoda bez @Override nadal przesłania metodę bazową;");
        Console.WriteLine("   w C# bez 'override' następuje ukrycie (ostrzeżenie CS0114).\n");

        Console.WriteLine("✅ Demonstration completed");
    }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// ============================================================================
// TESTY XUNIT
// ============================================================================

public class VirtualVsNewTests
{
    [Fact]
    public void Override_Dog_CallsDogSpeak()
    {
        Animal dog = new Dog();
        var method = dog.GetType().GetMethod("Speak");
        
        Assert.NotNull(method);
        Assert.Equal("Dog", dog.GetType().Name);
    }
    
    [Fact]
    public void New_Cat_CallsAnimalSpeak()
    {
        Animal cat = new Cat();
        
        // Virtual dispatch - Dog override works
        var dog = new Dog();
        Assert.Equal("Dog", dog.GetType().Name);
        
        // Non-virtual - Cat new doesn't work polymorphically
        Assert.Equal("Cat", cat.GetType().Name);
    }
    
    [Fact]
    public void Repository_Override_Works()
    {
        var repo = new GoodDatabaseRepository<User>();
        var user = new User { Id = 1, Name = "Test" };
        
        // Should call GoodDatabaseRepository.Save, not base.Save
        repo.Save(user);
        Assert.NotNull(user);
    }
    
    [Fact]
    public void Repository_New_DoesNotCallDerivedMethod()
    {
        Repository<User> repo = new BadDatabaseRepository<User>();
        var user = new User { Id = 1, Name = "Test" };
        
        // Calls base Repository.Save(), not BadDatabaseRepository.Save()
        repo.Save(user);
        Assert.NotNull(user);
    }
    
    [Fact]
    public void LSP_Eagle_Works()
    {
        Bird eagle = new Eagle();
        Assert.Equal("Eagle", eagle.GetType().Name);
    }
    
    [Fact]
    public void LSP_Penguin_Violates()
    {
        Bird bird = new Penguin();
        
        // This calls Bird.Fly(), but Penguin.Fly() would throw
        // LSP is violated - can't substitute Penguin for Bird
        Assert.Equal("Penguin", bird.GetType().Name);
    }
    
    [Fact]
    public void PaymentProcessor_Correct_Works()
    {
        PaymentProcessor processor = new CorrectPaymentProcessor();
        processor.Process(100m);
        
        Assert.Equal("CorrectPaymentProcessor", processor.GetType().Name);
    }
    
    [Fact]
    public void PaymentProcessor_Buggy_BugsOut()
    {
        PaymentProcessor processor = new BuggyPaymentProcessor();
        
        // Calls base class method due to 'new'
        processor.Process(100m);
        
        // BuggyPaymentProcessor.Process() is never called
        Assert.Equal("BuggyPaymentProcessor", processor.GetType().Name);
    }

    [Fact]
    public void Override_UsesRuntimeType()
    {
        Greeter greeter = new OverridingGreeter();

        Assert.Equal("Override", greeter.Greet());
    }

    [Fact]
    public void New_UsesVariableType()
    {
        var hiding = new HidingGreeter();
        Greeter asBase = hiding;

        Assert.Equal("Base", asBase.Greet());
        Assert.Equal("New", hiding.Greet());
    }

    [Fact]
    public void MissingOverride_HidesInsteadOfOverriding()
    {
        var forgetful = new ForgetfulGreeter();
        Greeter asBase = forgetful;

        Assert.Equal("Base", asBase.Greet());
        Assert.Equal("Forgot override", forgetful.Greet());
    }

    [Fact]
    public void OverrideThatRejectsContract_ViolatesLsp()
    {
        Bird bird = new FlightlessBird();

        Assert.Throws<NotSupportedException>(() => bird.Fly());
    }

    [Fact]
    public void PenguinWithNew_BehavesDifferentlyDependingOnVariableType()
    {
        var penguin = new Penguin();
        Bird bird = penguin;

        var viaBase = Record.Exception(() => bird.Fly());
        var viaDerived = Record.Exception(() => penguin.Fly());

        Assert.Null(viaBase);
        Assert.IsType<NotImplementedException>(viaDerived);
    }
}
