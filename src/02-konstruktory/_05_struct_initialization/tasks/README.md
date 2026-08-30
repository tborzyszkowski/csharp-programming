# Zadania - Struktury: Inicjalizacja

## 📝 Zadanie 1: Struct Temperature

Stwórz struct `Temperature` z `init` properties dla Celsjusza, konwersją na Fahrenheit.

```csharp
public struct Temperature
{
    public double Celsius { get; init; }
    public double Fahrenheit => Celsius * 9 / 5 + 32;
}

var temp = new Temperature { Celsius = 25 };
Console.WriteLine(temp.Fahrenheit);  // 77
```

## 📝 Zadanie 2: Struct Date z konstruktorem

Stwórz struct `Date` z konstruktorem inicjalizującym day/month/year i metodą `AddDays(n)`.

---

## ✅ Rozwiązania w `code/Program.cs`
