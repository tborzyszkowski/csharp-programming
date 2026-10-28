# Modyfikatory Dostępu w Dziedziczeniu (Access Modifiers)

## 🎯 Cel

Zrozumienie jak **modyfikatory dostępu** (`public`, `private`, `protected`, `internal`) wpływają na dziedziczenie i dostęp do członków klasy.

## Modyfikatory Dostępu

| Modyfikator | Ta sama klasa | Pochodna (to samo assembly) | Inny kod (to samo assembly) | Pochodna (inne assembly) | Inny kod (inne assembly) |
|-------------|:---:|:---:|:---:|:---:|:---:|
| `public` | ✅ | ✅ | ✅ | ✅ | ✅ |
| `protected` | ✅ | ✅ | ❌ | ✅ | ❌ |
| `internal` | ✅ | ✅ | ✅ | ❌ | ❌ |
| `protected internal` | ✅ | ✅ | ✅ | ✅ | ❌ |
| `private protected` | ✅ | ✅ | ❌ | ❌ | ❌ |
| `private` | ✅ | ❌ | ❌ | ❌ | ❌ |

Dostęp „pochodna” w przypadku `protected` dotyczy odwołań przez `this` lub przez referencję typu klasy pochodnej (patrz niżej).

## Przykład: Czym Różni Się protected w C# i Java?

### Java: protected = dostęp z other classes w tym samym package

```java
// Java - same package
package com.example;

public class Animal {
    protected void eat() { }
}

public class Dog extends Animal {
    // OK - są w tym samym package
    void callEat() { this.eat(); }
}

public class OtherClass {
    // OK w Javie - ten sam package
    void method() {
        new Dog().eat();  // Działa!
    }
}
```

### C#: protected = dostęp TYLKO z klas pochodnych

```csharp
// C# - INNA logika!
public class Animal
{
    protected void Eat() { }
}

public class Dog : Animal
{
    void CallEat() { this.Eat(); }  // OK
    void CallOther(Animal other) 
    { 
        // other.Eat();  // ❌ BŁĄD (CS1540): dostęp przez referencję typu bazowego
    }

    void CallDog(Dog other)
    {
        other.Eat();  // ✅ OK - referencja typu Dog (lub jego pochodnej)
    }
}

public class OtherClass
{
    void Method()
    {
        // new Dog().Eat();  // ❌ BŁĄD! Brak dostępu z zewnątrz hierarchii
    }
}
```

## Praktyczny Przykład

```csharp
namespace MyNamespace;

public class Vehicle
{
    // Public - dostęp wszędzie
    public string Make { get; set; } = "";
    
    // Private - dostęp TYLKO w tej klasie
    private double fuelTank = 50.0;
    
    // Protected - dostęp TYLKO w tej klasie i klasach pochodnych
    protected int MaxSpeed { get; set; } = 100;
    
    // Internal - dostęp w całym assembly
    internal string VIN { get; set; } = "";
}

public class Car : Vehicle
{
    public void Drive()
    {
        // ✅ OK - public
        Console.WriteLine(Make);
        
        // ❌ BŁĄD - private
        // Console.WriteLine(fuelTank);
        
        // ✅ OK - protected
        Console.WriteLine(MaxSpeed);
        
        // ✅ OK - internal
        Console.WriteLine(VIN);
    }
}

public class Other
{
    public void Test()
    {
        var car = new Car();
        
        // ✅ OK - public
        car.Make = "Toyota";
        
        // ❌ BŁĄD - private
        // car.fuelTank = 40;
        
        // ❌ BŁĄD - protected
        // car.MaxSpeed = 120;
        
        // ✅ OK - internal
        car.VIN = "123ABC";
    }
}
```

## Domyślny Modyfikator

```csharp
class TopLevel { }                   // internal (typ najwyższego poziomu)

class Outer
{
    class Nested { }                 // private (typ zagnieżdżony)
    void Method() { }                // private (składowa klasy)
    int field;                       // private
}

public class Explicit { }            // jawnie public
public void Method() { }             // jawnie public (jako składowa)
```

Gdy nie podasz modyfikatora, typ najwyższego poziomu jest `internal`, a składowe klasy i typy zagnieżdżone są `private`. W kodzie dydaktycznym i bibliotecznym warto pisać modyfikator jawnie.

`protected` jest częścią kontraktu dla klas pochodnych. Zbyt duża liczba protected pól wiąże klasę bazową z implementacją pochodnych, dlatego często lepiej udostępnić protected metodę lub właściwość z kontrolą niezmienników. `internal` dotyczy całego assembly, a `private protected` łączy ograniczenie do tego samego assembly z dostępem z klas pochodnych.

## protected vs private - Diagramatycznie

```
                    Kod zewnętrzny
                           ▲
                           │ ❌ protected
                           │ ✅ public
                           │
    ┌──────────────────────┴───────────────┐
    │                                      │
    │  Klasa bazowa (Vehicle)             │
    │  ┌─────────────────────────┐        │
    │  │ public Make             │        │
    │  │ private fuelTank        │ ✅ OK  │
    │  │ protected MaxSpeed      │        │
    │  └─────────────────────────┘        │
    │                                      │
    │  Klasa pochodna (Car : Vehicle)     │
    │  ┌─────────────────────────┐        │
    │  │ Widzi:                  │        │
    │  │ - Make (✅ public)      │        │
    │  │ - MaxSpeed (✅ protected)│       │
    │  │ - fuelTank (❌ private) │        │
    │  └─────────────────────────┘        │
    │                                      │
    └──────────────────────────────────────┘
```

## C# vs Java - Porównanie

| C# | Java | Opis |
|----|------|------|
| `public` | `public` | Dostęp wszędzie |
| `private` | `private` | Dostęp tylko w klasie |
| `protected` | `protected` | C#: tylko klasa i jej pochodne. Java: pochodne **oraz** ten sam pakiet |
| `internal` | brak modyfikatora (package-private) | C#: całe assembly. Java: ten sam pakiet |
| `protected internal` | brak odpowiednika | Pochodne **lub** kod w tym assembly |
| `private protected` | brak odpowiednika | Pochodne **i** kod w tym assembly |

```csharp
// C#: protected - TYLKO klasy pochodne
public class Base {
    protected void Method() { }
}
public class Derived : Base {
    public void Call() { Method(); }  // ✅ OK
}
public class Other {
    public void Call() {
        // new Derived().Method();  // ❌ BŁĄD - protected nie jest dostępny z zewnątrz
    }
}
```

```java
// Java: protected - klasy pochodne ORAZ same package
public class Base {
    protected void method() { }
}
public class Derived extends Base {
    public void call() { method(); }  // ✅ OK
}
public class Other {  // same package
    public void call() {
        new Derived().method();  // ✅ OK - protected w Java!
    }
}
```

## Best Practices

✅ **Encapsulation**:
```csharp
public class BankAccount
{
    private double balance;  // Hidden implementation
    
    public double Balance    // Public access via property
    {
        get { return balance; }
    }
    
    public void Deposit(double amount)
    {
        if (amount > 0) balance += amount;
    }
}
```

✅ **Protected jako kontrakt dla klas pochodnych** (preferuj metody/właściwości zamiast pól):
```csharp
public abstract class Shape
{
    protected Shape(double width, double height)
    {
        if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
        if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
        Width = width;
        Height = height;
    }

    protected double Width { get; }
    protected double Height { get; }

    // Protected - tylko dla klas pochodnych
    protected double CalculateArea() => Width * Height;
}

public class Rectangle : Shape
{
    public Rectangle(double width, double height) : base(width, height) { }

    public double GetArea() => CalculateArea();  // OK
}
```

❌ **Unikaj public fields**:
```csharp
// ZŁO
public class Person
{
    public int age;  // Bez kontroli
}

// DOBRZE
public class Person
{
    private int age;
    public int Age
    {
        get => age;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            age = value;
        }
    }
}
```

Przy bibliotekach można użyć `protected internal`, gdy członek ma być dostępny klasom pochodnym także poza assembly oraz kodowi w bieżącym assembly; `private protected` ogranicza dostęp do klas pochodnych w tym samym assembly. Te modyfikatory powinny być dobierane do API, a nie do wygody pojedynczej klasy pochodnej.

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 📚 Referencje

- [Microsoft Learn: Access Modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers)
- [C# Language Specification](https://github.com/dotnet/csharpstandard)
- [Java Protected Keyword](https://docs.oracle.com/javase/tutorial/java/javaOO/accesscontrol.html)

## 🧭 Nawigacja

[← Podstawy dziedziczenia](../_01_inheritance_basics/README.md) · [Moduł 05](../README.md) · [Konstruktory pochodne →](../_03_derived_constructors/README.md)
