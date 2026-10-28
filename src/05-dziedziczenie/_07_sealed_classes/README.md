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

### 1. Zamknięcie projektu i możliwość optymalizacji

```csharp
// Kompilator i JIT wiedzą, że nie istnieją typy pochodne,
// więc mogą łatwiej zdewirtualizować wywołania.
public sealed class Parser
{
    public string Parse(string input) => input.Trim();
}
```

Najważniejszym powodem użycia `sealed` jest zwykle zamknięcie projektu klasy, a nie obietnica mierzalnego przyspieszenia. Klasa sealed nadal może implementować interfejsy i być używana polimorficznie przez typ bazowy lub interfejs. `System.String` jest przykładem typu zapieczętowanego z biblioteki standardowej.

### 2. Ochrona niezmienników

```csharp
public sealed class EncryptionKey
{
    // Nikt nie może dziedziczyć i zmienić sposobu obsługi klucza
    public byte[] GetBytes() => Array.Empty<byte>();
}
```

`sealed` ogranicza dziedziczenie, ale samo w sobie **nie jest mechanizmem bezpieczeństwa** - nie zastępuje kontroli dostępu, walidacji ani poprawnego projektu API. Modyfikator `sealed` bez `override` dotyczy tylko klas; na zwykłej metodzie jest błędem kompilacji.

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

❌ **Nie nadużywaj `sealed`** - zamknięte klasy trudniej zastąpić atrapą w testach:
```csharp
// Trudne do podmiany w testach, jeśli kod zależy bezpośrednio od tej klasy
public sealed class SqlRepository { }

// Lepiej - zależ od abstrakcji; konkretną implementację możesz zapieczętować
public interface IRepository { }
public sealed class SqlRepository : IRepository { }
```

`sealed override` zamyka tylko konkretny punkt rozszerzania, ale nie musi zamykać całej klasy. Jest przydatne, gdy klasa pochodna ustala krytyczny algorytm i dalsze override mogłoby złamać jej niezmienniki. Klasa pochodna nadal może jednak **ukryć** taką metodę przez `new`, więc `sealed override` nie gwarantuje, że przez referencję pochodną nie zostanie wywołana inna implementacja. Przez referencję bazową zawsze zostanie wywołana zapieczętowana wersja.

## Hierarchia Z Sealed

```
        Animal
        /    \
      Dog    Cat
      (sealed)  (open)
      
Dog nie może być
dalej dziedziczona
```

## Wpływ na Wydajność

```csharp
public class Base { public virtual void M() { } }

// Niezapieczętowana - wywołanie przez Base wymaga wyboru implementacji w czasie działania
Base b = new Derived();
b.M();

// Zapieczętowana - JIT może zdewirtualizować wywołanie, bo zna dokładny typ
public sealed class Final : Base { public override void M() { } }
var f = new Final();
f.M();
```

Zapieczętowanie klasy może pomóc JIT w optymalizacji, ale efekt zależy od kodu i środowiska. W typowej aplikacji biznesowej rzadko jest powodem do użycia `sealed` - jeśli wydajność ma znaczenie, zmierz ją (np. BenchmarkDotNet).

## Typy wartościowe i `sealed`

Struktury (`struct`) są niejawnie zapieczętowane i nie mogą być dziedziczone. `sealed` nie można łączyć z `abstract` ani `static`.

## Records i Sealed (C# 9+)

```csharp
public abstract record Shape
{
    public abstract double Area();
}

public sealed record Circle(double Radius) : Shape
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

- [Microsoft Learn: Sealed keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed)
- [Abstract and Sealed Classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)

## 🧭 Nawigacja

[← Klasy abstrakcyjne](../_06_abstract_classes/README.md) · [Moduł 05](../README.md) · [Następny moduł: 06-polimorfizm →](../../06-polimorfizm/README.md)
