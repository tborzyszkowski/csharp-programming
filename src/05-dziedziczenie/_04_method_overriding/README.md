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
│ - Ukrywает metodę bazową            │
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

## Ważna Zasada: Base Musi Być Virtual

```csharp
public class Animal
{
    // ❌ Błąd - nie można override bez virtual!
    public void Speak() { }
}

public class Dog : Animal
{
    // ❌ BŁĄD: cannot override non-virtual method
    // public override void Speak() { }
    
    // ✅ OK - nowe ukrycie
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

❌ **Unikaj new** (zwykle oznacza złą hierarchię):
```csharp
// ZŁO - new wskazuje na problem designu
public class Base { public void Do() { } }
public class Derived : Base { public new void Do() { } }

// DOBRZE - używaj virtual+override
public class Base { public virtual void Do() { } }
public class Derived : Base { public override void Do() { } }
```

## Abstract Methods

```csharp
public abstract class Animal
{
    // Pochodne MUSZĄ implementować
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
