# Temat 9: Operatory Konwersji (Casting Operators)

## 🎯 Cel

Nauczysz się definiować jawne i niejawne konwersje typów.

---

## 📖 Konwersje

### Jawna Konwersja (Explicit)

```csharp
public static explicit operator TargetType(SourceType source)
{
    // Konwersja
    return target;
}
```

Użycie:
```csharp
Temperature t = new Temperature(25);
int celsius = (int)t;  // Jawna konwersja
```

### Niejawna Konwersja (Implicit)

```csharp
public static implicit operator TargetType(SourceType source)
{
    // Konwersja
    return target;
}
```

Użycie:
```csharp
Temperature t = 25.0;  // Niejawna konwersja
```

---

## ⚠️ Wytyczne

- **Implicit:** Zawsze bezpieczna, nigdy nie traci danych
- **Explicit:** Może być niebezpieczna lub tracić dane

```csharp
public class Percent
{
    public double Value { get; set; }

    // Jawna - może się nie mieścić w %
    public static explicit operator Percent(double d)
        => new() { Value = d };

    // Niejawna - zawsze bezpieczna
    public static implicit operator double(Percent p)
        => p.Value;
}

Percent p = (Percent)150.0;  // Explicit
double d = p;                 // Implicit
```

---

## 📋 Praktyczne Przypadki

```csharp
// Konwersja Temperature
public record Temperature(double Celsius)
{
    // Niejawna z double
    public static implicit operator Temperature(double celsius)
        => new(celsius);
    
    // Jawna na int (zaokrąglenie)
    public static explicit operator int(Temperature t)
        => (int)Math.Round(t.Celsius);
    
    // Jawna na Fahrenheit (double)
    public static explicit operator double(Temperature t)
        => (t.Celsius * 9/5) + 32;
}
```

---

## 📖 Referencje

- [User-defined conversion operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators)
