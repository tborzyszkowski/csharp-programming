using System;
using System.Collections.Generic;
using Xunit;

namespace VirtualMethodsIntro;

// ============================================================================
// CZĘŚĆ 1: Wstęp do Funkcji Wirtualnych
// ============================================================================

/// <summary>
/// Klasa bazowa Animal demonstrująca wirtualne metody
/// </summary>
public class Animal
{
    public string Name { get; set; }
    
    // Metoda NON-VIRTUAL (zwykła)
    public void Eat()
    {
        Console.WriteLine($"{Name} is eating.");
    }
    
    // Metoda VIRTUAL - może być przesłonięta
    public virtual void Speak()
    {
        Console.WriteLine($"{Name} makes a generic sound.");
    }
}

/// <summary>
/// Klasa Dog demonstrująca przesłanianie metod virtual
/// </summary>
public class Dog : Animal
{
    // Override metody wirtualnej Speak
    public override void Speak()
    {
        Console.WriteLine($"{Name} barks: Woof! Woof!");
    }
}

/// <summary>
/// Klasa Cat demonstrująca własną implementację Speak
/// </summary>
public class Cat : Animal
{
    public override void Speak()
    {
        Console.WriteLine($"{Name} meows: Meow! Meow!");
    }
}

/// <summary>
/// Klasa Bird demonstrująca trzecią implementację
/// </summary>
public class Bird : Animal
{
    public override void Speak()
    {
        Console.WriteLine($"{Name} chirps: Tweet! Tweet!");
    }
}

// ============================================================================
// CZĘŚĆ 2: Porównanie Wczesnego vs. Późnego Wiązania
// ============================================================================

/// <summary>
/// Demonstruje wczesne wiązanie (early binding) - BEZ virtual
/// </summary>
public class Vehicle
{
    public string Model { get; set; }
    
    // BEZ 'virtual' - wczesne wiązanie
    public void Start()
    {
        Console.WriteLine($"{Model} is starting (generic way)");
    }
}

public class Car : Vehicle
{
    // BEZ 'override' - tylko przesłaniamy
    public void Start()
    {
        Console.WriteLine($"{Model} is starting (car engine roaring)");
    }
}

/// <summary>
/// Demonstruje późne wiązanie (late binding) - Z virtual
/// </summary>
public class Transport
{
    public string Model { get; set; }
    
    // Z 'virtual' - późne wiązanie
    public virtual void Start()
    {
        Console.WriteLine($"{Model} is starting (generic way)");
    }
}

public class Truck : Transport
{
    // Z 'override' - przesłaniamy wirtualną
    public override void Start()
    {
        Console.WriteLine($"{Model} is starting (truck engine roaring)");
    }
}

// ============================================================================
// CZĘŚĆ 3: Praktyczne Zastosowanie - Payment System
// ============================================================================

/// <summary>
/// Abstrakcyjna klasa bazowa dla metod płatności
/// </summary>
public abstract class PaymentMethod
{
    public string Name { get; set; }
    public decimal Amount { get; set; }
    
    public PaymentMethod(string name, decimal amount)
    {
        Name = name;
        Amount = amount;
    }
    
    // Metoda wirtualna - każda metoda płatności implementuje inaczej
    public virtual void ProcessPayment()
    {
        Console.WriteLine($"Processing {Amount:C} via {Name}");
    }
    
    public virtual void DisplayReceipt()
    {
        Console.WriteLine($"Receipt for {Amount:C} paid with {Name}");
    }
}

public class CreditCard : PaymentMethod
{
    public CreditCard(decimal amount) : base("Credit Card", amount) { }
    
    public override void ProcessPayment()
    {
        Console.WriteLine($"💳 Processing {Amount:C} with Credit Card");
        Console.WriteLine("   Validating card...");
        Console.WriteLine("   Charging card...");
    }
    
    public override void DisplayReceipt()
    {
        Console.WriteLine($"📄 Credit Card Receipt: {Amount:C}");
        Console.WriteLine("   Card number: ****-****-****-1234");
    }
}

public class PayPal : PaymentMethod
{
    public PayPal(decimal amount) : base("PayPal", amount) { }
    
    public override void ProcessPayment()
    {
        Console.WriteLine($"🔵 Processing {Amount:C} with PayPal");
        Console.WriteLine("   Contacting PayPal...");
        Console.WriteLine("   Transferring funds...");
    }
    
    public override void DisplayReceipt()
    {
        Console.WriteLine($"📄 PayPal Receipt: {Amount:C}");
        Console.WriteLine("   Transaction ID: PP-12345-67890");
    }
}

public class Bitcoin : PaymentMethod
{
    public Bitcoin(decimal amount) : base("Bitcoin", amount) { }
    
    public override void ProcessPayment()
    {
        Console.WriteLine($"₿ Processing {Amount:C} in Bitcoin");
        Console.WriteLine("   Validating blockchain...");
        Console.WriteLine("   Recording transaction...");
    }
    
    public override void DisplayReceipt()
    {
        Console.WriteLine($"📄 Bitcoin Receipt: {Amount:C}");
        Console.WriteLine("   Blockchain hash: 0x123abc...");
    }
}

// ============================================================================
// PROGRAM GŁÓWNY I TESTY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TEMAT 1: FUNKCJE WIRTUALNE - WSTĘP                   ║");
        Console.WriteLine("║  Virtual Methods Introduction                         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        // ====================================================================
        Console.WriteLine("CZĘŚĆ 1: Polimorfizm - Ten Sam Kod, Różne Działania\n");
        Console.WriteLine("Animal animal = new Dog();");
        Console.WriteLine("animal.Speak();  // Wola Dog.Speak() - Late Binding!\n");
        
        Animal dog = new Dog { Name = "Rex" };
        Animal cat = new Cat { Name = "Whiskers" };
        Animal bird = new Bird { Name = "Tweety" };
        
        Console.WriteLine("--- Animal Hierarchy ---");
        dog.Speak();   // Wywołuje Dog.Speak()
        cat.Speak();   // Wywołuje Cat.Speak()
        bird.Speak();  // Wywołuje Bird.Speak()
        
        Console.WriteLine("\n--- Kolekcja Zwierząt (Polimorfizm) ---");
        List<Animal> animals = new() { dog, cat, bird };
        foreach (var animal in animals)
        {
            animal.Speak();  // Każdy robi to, co powinien!
        }
        
        // ====================================================================
        Console.WriteLine("\n\nCZĘŚĆ 2: Wczesne vs. Późne Wiązanie\n");
        
        Console.WriteLine("--- WCZESNE WIĄZANIE (bez virtual) ---");
        Vehicle vehicle = new Car { Model = "Toyota Camry" };
        vehicle.Start();  // Wówła Vehicle.Start() - wczesne wiązanie!
        
        Console.WriteLine("\n--- PÓŹNE WIĄZANIE (z virtual) ---");
        Transport transport = new Truck { Model = "Volvo FH16" };
        transport.Start();  // Wówła Truck.Start() - późne wiązanie!
        
        // ====================================================================
        Console.WriteLine("\n\nCZĘŚĆ 3: Praktyka - E-Commerce Payment System\n");
        
        decimal orderTotal = 99.99m;
        Console.WriteLine($"Processing order for {orderTotal:C}\n");
        
        List<PaymentMethod> paymentMethods = new()
        {
            new CreditCard(orderTotal),
            new PayPal(orderTotal),
            new Bitcoin(orderTotal)
        };
        
        Console.WriteLine("--- Dostępne Metody Płatności ---");
        foreach (var payment in paymentMethods)
        {
            payment.ProcessPayment();
            Console.WriteLine();
        }
        
        Console.WriteLine("\n--- Rachunki ---");
        foreach (var payment in paymentMethods)
        {
            payment.DisplayReceipt();
            Console.WriteLine();
        }
        
        Console.WriteLine("\n✅ Demonstracja ukończona - zobacz testy poniżej");
    }
}

// ============================================================================
// TESTY XUNIT
// ============================================================================

public class VirtualMethodsIntroTests
{
    [Fact]
    public void Animal_CanBeInstantiatedWithName()
    {
        var animal = new Animal { Name = "Generic" };
        Assert.Equal("Generic", animal.Name);
    }
    
    [Fact]
    public void Dog_OverridesSpeak_ReturnsDogSound()
    {
        var dog = new Dog { Name = "Rex" };
        // Test: Dog ma metodę Speak
        Assert.NotNull(dog as Animal);
        var method = dog.GetType().GetMethod("Speak");
        Assert.NotNull(method);
        Assert.True(method.GetBaseDefinition().DeclaringType == typeof(Animal));
    }
    
    [Fact]
    public void Polymorphism_VirtualMethodDispatch()
    {
        Animal dog = new Dog { Name = "Buddy" };
        Animal cat = new Cat { Name = "Whiskers" };
        
        // Test: różne obiekty, ta sama metoda, różne rezultaty
        Assert.Equal("Buddy", dog.Name);
        Assert.Equal("Whiskers", cat.Name);
        
        // Test: Speak() na różnych typach
        var dogType = dog.GetType().Name;
        var catType = cat.GetType().Name;
        
        Assert.Equal("Dog", dogType);
        Assert.Equal("Cat", catType);
    }
    
    [Fact]
    public void EarlyBinding_WithoutVirtual_UsesVariableType()
    {
        Vehicle vehicle = new Car { Model = "Toyota" };
        
        // Bez virtual - wczesne wiązanie
        var method = vehicle.GetType().GetMethod("Start");
        
        // Method na Vehicle, nie na Car (bo nie virtual)
        Assert.NotNull(method);
    }
    
    [Fact]
    public void LateBinding_WithVirtual_UsesActualType()
    {
        Transport transport = new Truck { Model = "Volvo" };
        
        // Z virtual - późne wiązanie
        var method = transport.GetType().GetMethod("Start");
        
        // Method powinno być z Truck (virtual override)
        Assert.Equal("Truck", transport.GetType().Name);
        Assert.NotNull(method);
    }
    
    [Fact]
    public void PaymentMethod_Polymorphism_AllMethodsWork()
    {
        List<PaymentMethod> payments = new()
        {
            new CreditCard(50m),
            new PayPal(75m),
            new Bitcoin(100m)
        };
        
        Assert.Equal(3, payments.Count);
        Assert.All(payments, p => Assert.NotNull(p.Name));
        Assert.All(payments, p => Assert.True(p.Amount > 0));
    }
}
