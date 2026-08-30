# Modyfikatory Dostępu w Dziedziczeniu (Access Modifiers)

## 🎯 Cel

Zrozumienie jak **modyfikatory dostępu** (`public`, `private`, `protected`, `internal`) wpływają na dziedziczenie i dostęp do członków klasy.

## Modyfikatory Dostępu

| Modyfikator | Klasa | Assembly | Pochodna | Inne |
|-------------|-------|----------|----------|------|
| `public` | ✅ | ✅ | ✅ | ✅ |
| `protected` | ✅ | ❌ | ✅ | ❌ |
| `internal` | ✅ | ✅ | ❌* | ❌ |
| `private` | ✅ | ❌ | ❌ | ❌ |
| `protected internal` | ✅ | ✅ | ✅ | ❌ |
| `private protected` | ✅ | ❌ | ✅* | ❌ |

*Tylko w ramach tego assembly

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
        other.Eat();  // ❌ BŁĄD! Nawet na Animal
    }
}

public class OtherClass
{
    void Method()
    {
        new Dog().Eat();  // ❌ BŁĄD! Nie do dostępu z zewnątrz
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
class MyClass { }                    // private (jeśli wewnątrz innej klasy)
public class MyClass { }             // public

void Method() { }                    // private
public void Method() { }             // public
```

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
| `protected` | `protected` | Dostęp w klasach pochodnych + same package |
| `internal` | `(default)` | Dostęp w assembly/package |
| `protected internal` | N/A | Dostęp w pochodnych ORAZ w assembly |

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
        new Derived().Method();  // ❌ BŁĄD - protected, nawet w derived
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

✅ **Protected dla Interface'ów**:
```csharp
public abstract class Shape
{
    protected double width;
    protected double height;
    
    // Protected - dla klas pochodnych
    protected double CalculateArea() => width * height;
}

public class Rectangle : Shape
{
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
        get { return age; }
        set { if (value > 0) age = value; }
    }
}
```

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
