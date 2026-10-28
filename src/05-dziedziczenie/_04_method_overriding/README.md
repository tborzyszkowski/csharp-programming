# Przesłonięcie Metod: Override vs New

## 🎯 Cel

Zrozumienie różnicy między `override` i `new` w C# - jak przesłaniać metody w klasach pochodnych.

## Podstawowe Pojęcia

### `virtual` - metoda może być przesłonięta

```csharp
public class Animal
{
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
```

### `override` - przesłonięcie (polimorfizm)

```csharp
var animal = new Animal();
animal.Speak();  // "Animal sound"

var dog = new Dog();
dog.Speak();     // "Woof!"

Animal polymorphic = dog;
polymorphic.Speak();  // "Woof!" - wywołuje Dog.Speak!
```

### `new` - ukrycie (nie polimorfizm!)

```csharp
public class Animal
{
    public void Speak()  // Nie virtual!
    {
        Console.WriteLine("Animal sound");
    }
}

public class Dog : Animal
{
    public new void Speak()  // Ukrycie, nie override
    {
        Console.WriteLine("Woof!");
    }
}

Dog dog = new Dog();
dog.Speak();  // "Woof!"

Animal polymorphic = dog;
polymorphic.Speak();  // "Animal sound" - nie "Woof!"!
```

`new` nie zmienia implementacji zapisanej w klasie bazowej. Wybór metody zależy od typu referencji, dlatego po przekazaniu obiektu do kodu bazowego może zostać wywołana inna metoda. Jeśli zachowanie ma być wybierane według rzeczywistego typu obiektu, użyj `virtual` i `override`.

## Override vs New - Porównanie

```
┌─────────────────────────────────────┐
│ override - Polimorfizm              │
├─────────────────────────────────────┤
│ - Wymaga virtual w bazie            │
│ - Wywoływana wersja pochodna        │
│ - Dynamic dispatch (runtime)        │
│                                     │
│ public virtual void Speak() { }     │
│ public override void Speak() { }    │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│ new - Ukrycie                       │
├─────────────────────────────────────┤
│ - Nie wymaga virtual                │
│ - Wywoływana wersja typu zmiennej   │
│ - Ukrywa metodę bazową              │
│                                     │
│ public void Speak() { }             │
│ public new void Speak() { }         │
└─────────────────────────────────────┘
```

## Przykład: Override

```csharp
public class Shape
{
    public virtual void Draw()
    {
        Console.WriteLine("Drawing Shape");
    }
}

public class Circle : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing Circle");
    }
}

public class Square : Shape
{
    public override void Draw()
    {
        Console.WriteLine("Drawing Square");
    }
}

// Polimorfizm
List<Shape> shapes = new() { new Circle(), new Square() };
foreach (var shape in shapes)
{
    shape.Draw();  // Circle: "Drawing Circle", Square: "Drawing Square"
}
```

## Przykład: New (Ukrycie)

```csharp
public class Base
{
    public void Method()
    {
        Console.WriteLine("Base.Method");
    }
}

public class Derived : Base
{
    public new void Method()  // Ukrywamy Method z Base
    {
        Console.WriteLine("Derived.Method");
    }
}

Derived d = new Derived();
d.Method();  // "Derived.Method"

Base b = d;
b.Method();  // "Base.Method" - nie polimorfizm!
```

## Ważna Zasada: `override` Wymaga Przesłanianego Członka

`override` można zastosować tylko wtedy, gdy członek w klasie bazowej jest `virtual`, `abstract` albo sam jest `override` (i nie jest `sealed`). Sygnatura musi być zgodna, a modyfikator dostępu - identyczny.

```csharp
public class Animal
{
    // Brak virtual
    public void Speak() { }
}

public class Dog : Animal
{
    // ❌ BŁĄD: nie można przesłonić metody niewirtualnej
    // public override void Speak() { }
    
    // ✅ Można ją co najwyżej ukryć (ostrzeżenie CS0108 bez 'new')
    public new void Speak() { }
}
```

## Best Practices

✅ **Używaj override dla polimorfizmu**:
```csharp
public class Payment
{
    public virtual void Process()
    {
        Console.WriteLine("Processing payment");
    }
}

public class CreditCardPayment : Payment
{
    public override void Process()
    {
        Console.WriteLine("Processing credit card");
    }
}

public class PayPalPayment : Payment
{
    public override void Process()
    {
        Console.WriteLine("Processing PayPal");
    }
}

// Polimorfizm
List<Payment> payments = new() 
{ 
    new CreditCardPayment(), 
    new PayPalPayment() 
};

foreach (var payment in payments)
    payment.Process();  // Każda wykonuje swoją implementację
```

⚠️ **Używaj `new` rzadko i świadomie** (najczęściej sygnalizuje problem z designem):
```csharp
// Ryzykowne - zachowanie zależy od typu referencji
public class Base { public void Do() { } }
public class Derived : Base { public new void Do() { } }

// Zwykle lepiej - virtual + override
public class Base { public virtual void Do() { } }
public class Derived : Base { public override void Do() { } }
```

Uzasadnione użycie `new` to np. ukrycie członka z biblioteki, której nie kontrolujesz, albo konflikt nazw przy ewolucji API. Zmiana typu zwracanego przy przesłanianiu nie wymaga `new` - od C# 9 `override` obsługuje kowariantne typy zwracane. W pozostałych przypadkach zastanów się, czy nie potrzebujesz `virtual`.

Kompilator ostrzega (CS0108), gdy członek klasy pochodnej ukrywa członka bazowego bez jawnego `new`. Użycie `new` powinno być świadomą decyzją, najlepiej opisaną komentarzem lub dokumentacją API.

## Abstract Methods

```csharp
public abstract class Animal
{
    // Każda konkretna (nieabstrakcyjna) klasa pochodna MUSI to zaimplementować
    public abstract void Speak();
}

public class Dog : Animal
{
    public override void Speak()
    {
        Console.WriteLine("Woof!");
    }
}
```

## Calling Base Implementation

```csharp
public class Animal
{
    public virtual void Eat()
    {
        Console.WriteLine("Eating food");
    }
}

public class Dog : Animal
{
    public override void Eat()
    {
        Console.WriteLine("Dog is eating");
        base.Eat();  // Wywołaj wersję z Animal
    }
}

var dog = new Dog();
dog.Eat();
// Dog is eating
// Eating food
```

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 📚 Referencje

- [Microsoft Learn: Polymorphism](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Override keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override)
- [Virtual keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual)

## 🧭 Nawigacja

[← Konstruktory pochodne](../_03_derived_constructors/README.md) · [Moduł 05](../README.md) · [Virtual i polimorfizm →](../_05_virtual_polymorphism/README.md)
