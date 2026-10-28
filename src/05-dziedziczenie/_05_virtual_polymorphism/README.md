# Virtual Methods i Polimorfizm (Virtual & Polymorphism)

## 🎯 Cel

Głębokie zrozumienie **virtual** słowa kluczowego i **polimorfizmu** - jak C# decyduje którą metodę wywołać w runtime.

## Co to jest Polimorfizm?

**Polimorfizm** (z greckiego: "wiele form") - ta sama operacja pracuje z różnymi typami.

```csharp
public class Animal { public virtual void Speak() { } }
public class Dog : Animal { public override void Speak() => Console.WriteLine("Woof!"); }
public class Cat : Animal { public override void Speak() => Console.WriteLine("Meow!"); }

// Jeden kod - wiele zachowań!
Animal animal = new Dog();   animal.Speak();  // "Woof!"
animal = new Cat();          animal.Speak();  // "Meow!"
```

## Virtual - Włączanie Polimorfizmu

```csharp
public class Base
{
    // Virtual = może być przesłonięta w klasach pochodnych
    public virtual void Method()
    {
        Console.WriteLine("Base");
    }
}

public class Derived : Base
{
    public override void Method()
    {
        Console.WriteLine("Derived");
    }
}
```

## Dynamic Dispatch (Runtime Type Checking)

C# używa **dynamic dispatch** - metodę wybraną na podstawie typu obiektu, nie typu zmiennej.

```csharp
var derived = new Derived();
derived.Method();           // "Derived" - typ: Derived

Base baseRef = derived;
baseRef.Method();           // "Derived" - typ runtime: Derived!

Base baseRef2 = new Base();
baseRef2.Method();          // "Base" - typ runtime: Base
```

Typ zmiennej określa, jakie członki są dostępne w czasie kompilacji, natomiast typ obiektu decyduje o implementacji metody wirtualnej w czasie działania. To pozwala pisać kod pracujący z `List<Payment>` bez znajomości każdej konkretnej metody płatności.

## Bez Virtual = Static Dispatch

```csharp
public class Base
{
    // Bez virtual!
    public void Method() => Console.WriteLine("Base");
}

public class Derived : Base
{
    public new void Method() => Console.WriteLine("Derived");  // new, nie override!
}

Base baseRef = new Derived();
baseRef.Method();  // "Base" - typ zmiennej = Base!
```

## Virtual Properties

```csharp
public class Person
{
    private string name = "";
    
    public virtual string Name
    {
        get { return name; }
        set { name = value; }
    }
}

public class Employee : Person
{
    public override string Name
    {
        get { return base.Name.ToUpper(); }
        set { base.Name = value; }
    }
}

Person p = new Employee();
p.Name = "john";
Console.WriteLine(p.Name);  // "JOHN"
```

## Is i As Operatory (Type Checking & Casting)

```csharp
Animal animal = new Dog();

// is - Type checking
if (animal is Dog)
{
    Console.WriteLine("It's a Dog");
    Dog dog = (Dog)animal;  // Casting
    dog.Bark();
}

// as - bezpieczne rzutowanie (zwraca null, jeśli typ się nie zgadza)
Dog? dog2 = animal as Dog;
if (dog2 != null)
{
    dog2.Bark();
}

// Pattern matching (C# 7+) - sprawdzenie i rzutowanie w jednej operacji
if (animal is Dog dog)
{
    dog.Bark();
}
```

Rzutowanie jawne `(Dog)animal` zgłosi `InvalidCastException`, gdy obiekt nie jest `Dog`. Preferuj pattern matching `animal is Dog dog` nad osobnym `is` i rzutowaniem, bo sprawdzenie i uzyskanie typowanej zmiennej odbywa się w jednej operacji. `as` zwraca `null` przy niezgodnym typie i działa tylko dla typów, które mogą przyjąć `null`.

## Praktyczny Przykład: Payment System

```csharp
public abstract class Payment
{
    public virtual void Process()
    {
        Console.WriteLine("Processing...");
    }
}

public class CreditCard : Payment
{
    public override void Process()
    {
        Console.WriteLine("Processing credit card");
        ValidateCard();
    }
    
    private void ValidateCard() { }
}

public class PayPal : Payment
{
    public override void Process()
    {
        Console.WriteLine("Processing PayPal");
        AuthenticateUser();
    }
    
    private void AuthenticateUser() { }
}

// Polimorfizm
List<Payment> payments = new()
{
    new CreditCard(),
    new PayPal(),
    new CreditCard()
};

foreach (var payment in payments)
{
    payment.Process();  // Virtual dispatch
}
```

## Call Chain

```csharp
public class Animal
{
    public virtual void Eat()
    {
        Console.WriteLine("Animal eats");
    }
}

public class Dog : Animal
{
    public override void Eat()
    {
        Console.WriteLine("Dog eats");
        base.Eat();      // Może wywołać bazę
    }
}

var dog = new Dog();
dog.Eat();
// Output:
// Dog eats
// Animal eats
```

## Koszt Wywołań Wirtualnych

```csharp
// Wywołanie wirtualne wymaga wyboru implementacji w czasie działania
public virtual void Method() { }

// Wywołanie niewirtualne może być rozstrzygnięte wcześniej i łatwiej inline'owane
public void Method() { }
```

Wywołanie wirtualne może być nieco droższe niż niewirtualne, ale JIT potrafi je optymalizować (np. devirtualizacja, gdy znany jest dokładny typ). Skala różnicy zależy od platformy i scenariusza - jeśli ma znaczenie, zmierz ją benchmarkiem (np. BenchmarkDotNet), zamiast zakładać konkretną liczbę nanosekund.

Nie wybieraj `virtual` ani jego braku wyłącznie na podstawie mikrooptymalizacji. Ważniejsze są kontrakt klasy i możliwość rozszerzania. Z kolei publiczne metody wirtualne są częścią API, więc ich zachowanie i kompatybilność trzeba utrzymywać.

## Best Practices

✅ **Virtual dla inheritance hierarchies**:
```csharp
public abstract class Shape
{
    public abstract double CalculateArea();  // Wymuś implementację
    public virtual string Describe() => "Shape";  // Opcjonalne przesłonięcie
}
```

> Nie nazywaj własnej metody `GetType()` - ukryłaby niewirtualne `object.GetType()`.

✅ **Type checking przed casting**:
```csharp
if (animal is Dog dog)
{
    dog.Bark();  // Safe
}
```

❌ **Unikaj mieszania `virtual` w bazie z `new` w pochodnej**:
```csharp
// Mylące - metoda bazowa jest wirtualna, a pochodna ją tylko ukrywa
public class Base { public virtual void Method() { } }
public class Derived : Base { public new void Method() { } }

// Jasny zamiar
public class Base { public virtual void Method() { } }
public class Derived : Base { public override void Method() { } }
```

## Records z Polimorfizmem (C# 9+)

```csharp
public abstract record Shape(string Name)
{
    public abstract double Area();
}

public record Circle(string Name, double Radius) : Shape(Name)
{
    public override double Area() => Math.PI * Radius * Radius;
}
```

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 📚 Referencje

- [Microsoft Learn: Polymorphism](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Virtual keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual)
- [Is operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-conversion#is-operator)

## 🧭 Nawigacja

[← Override vs New](../_04_method_overriding/README.md) · [Moduł 05](../README.md) · [Klasy abstrakcyjne →](../_06_abstract_classes/README.md)
