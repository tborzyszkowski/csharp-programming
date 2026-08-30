# Sealed Classes (Sealed Classes & Members)

## 🎯 Cel

Zrozumienie **sealed** słowa kluczowego - jak zapobiegać dalszemu dziedziczeniu i przesłanianiu metod.

## Sealed Class - Nie Można Dziedziczić

```csharp
public sealed class FinalClass  // Nie można: class Derived : FinalClass
{
    public void Method() { }
}

// ❌ BŁĄD
// public class Derived : FinalClass { }

// ✅ OK - można instancjonować
var obj = new FinalClass();
```

## Sealed Method - Nie Można Przesłonić

```csharp
public class Base
{
    public virtual void Method()
    {
        Console.WriteLine("Base");
    }
}

public class Derived : Base
{
    public sealed override void Method()  // Sealed - nie można przesłonić dalej
    {
        Console.WriteLine("Derived");
    }
}

// ❌ BŁĄD
public class FurtherDerived : Derived
{
    // public override void Method() { }  // ❌ Sealed!
}
```

## Kiedy Używać Sealed?

### 1. Performance - Compiler Optimization

```csharp
// Compiler wie że nie będzie virtual dispatch
public sealed class String
{
    // ...
}
```

### 2. Security - Zapobieganie Nadużyciu

```csharp
public sealed class EncryptionKey
{
    // Nie chcemy aby ktoś nadpisywał metodę bezpieczeństwa
    public sealed void Encrypt(string data) { }
}
```

### 3. Design - Zapobieganie Złemu Dziedziczeniu

```csharp
public class Base
{
    public virtual void Setup() { }
}

public class Intermediate : Base
{
    // Setup musi być implementowany dokładnie tak
    public sealed override void Setup()
    {
        Console.WriteLine("Critical setup");
    }
}
```

## Sealed vs Abstract

| Sealed | Abstract |
|--------|----------|
| Nie można dziedziczić | Musi być dziedziczona |
| Nie można override (jeśli sealed override) | MUSI implementować abstract |
| Zwykle dla finalnych klas | Dla class hierarchy |

```csharp
// Sealed - koniec hierarchii
public sealed class FinalImplementation : Base { }

// Abstract - musi być dziedziczona
public abstract class BaseInterface { }
```

## Sealed Override

```csharp
public class Animal
{
    public virtual void Speak() => Console.WriteLine("Sound");
}

public class Dog : Animal
{
    public sealed override void Speak()  // Sealed - nie można dalej override
    {
        Console.WriteLine("Woof!");
    }
}

public class Puppy : Dog
{
    // ❌ BŁĄD - nie można override sealed
    // public override void Speak() { }
}
```

## Praktyczne Przykłady

### String Class (Sealed)

```csharp
// C# String jest sealed - nie można dziedziczić
// public class MyString : string { }  // ❌ BŁĄD

var str = "Hello";  // OK
```

### Sealing Final Classes

```csharp
public class Logger { }
public sealed class FileLogger : Logger  // Ostateczna implementacja
{
    public void LogToFile(string message) { }
}

// ❌ Nie można dalej dziedziczić
// public class CustomFileLogger : FileLogger { }
```

## Best Practices

✅ **Seal jeśli to jest final implementation**:
```csharp
public abstract class PaymentProcessor
{
    public abstract void Process();
}

public sealed class CreditCardProcessor : PaymentProcessor
{
    public override void Process() => Console.WriteLine("Card payment");
}
```

✅ **Seal method jeśli nie ma sensu dalsze override'owanie**:
```csharp
public class Base
{
    public virtual void Critical()
    {
        // Critical implementation
    }
}

public class Derived : Base
{
    public sealed override void Critical()  // Nie zmieniaj tego!
    {
        // ...
    }
}
```

❌ **Nie overuse seal** (może ograniczyć testowanie):
```csharp
// ZŁO - zbyt restrykcyjne
public sealed class IRepository { }

// DOBRZE - interface dla testów
public interface IRepository { }
```

## Hierarchia Z Sealed

```
        Animal
        /    \
      Dog    Cat
      (sealed)  (open)
      
Dog nie może być
dalej dziedziczona
```

## Performance Impact

```csharp
// Non-sealed - virtual dispatch (lookup w runtime)
public class Base { public virtual void M() { } }
var b = new Derived() as Base;
b.M();  // Runtime lookup

// Sealed - compile-time (szybciej)
public sealed class Final : Base { public sealed override void M() { } }
var f = new Final();
f.M();  // Direct call
```

W praktyce: różnica ~nanosekunda, ale ważna dla hot-path code.

## Records i Sealed (C# 9+)

```csharp
public abstract record Shape
{
    public abstract double Area();
}

public sealed record Circle(double Radius) : Shape
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

- [Microsoft Learn: Sealed keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed)
- [Abstract and Sealed Classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)
