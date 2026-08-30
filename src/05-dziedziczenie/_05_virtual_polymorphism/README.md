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

// as - Safe casting (zwraca null jeśli fail)
if (animal is Dog dog)  // Pattern matching (C# 7+)
{
    dog.Bark();
}

Dog? dog2 = animal as Dog;  // null jeśli nie Dog
if (dog2 != null)
{
    dog2.Bark();
}
```

## Prakticzny Przykład: Payment System

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

## Performance: Virtual vs Non-Virtual

```csharp
// Virtual - runtime lookup (nieco wolniej)
public virtual void Method() { }

// Non-virtual - compile-time (szybciej)
public void Method() { }

// W praktyce: różnica nieznaczna (~nanosekunda)
// Używaj virtual gdy logika tego wymaga!
```

## Best Practices

✅ **Virtual dla inheritance hierarchies**:
```csharp
public abstract class Shape
{
    public abstract double CalculateArea();  // Force override
    public virtual string GetType() => "Shape";  // Optional override
}
```

✅ **Type checking przed casting**:
```csharp
if (animal is Dog dog)
{
    dog.Bark();  // Safe
}
```

❌ **Nie mieszaj virtual i new**:
```csharp
// ZŁO - mylące!
public class Base { public virtual void Method() { } }
public class Derived : Base { public new void Method() { } }

// DOBRZE - jasne zamiaru
public class Base { public virtual void Method() { } }
public class Derived : Base { public override void Method() { } }
```

## C# 9+: Records z Polimorfizmem

```csharp
public abstract record Shape(string Name)
{
    public abstract double Area();
}

public record Circle(string Name, double Radius) : Shape(Name)
{
    public override double Area() => 3.14 * Radius * Radius;
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
