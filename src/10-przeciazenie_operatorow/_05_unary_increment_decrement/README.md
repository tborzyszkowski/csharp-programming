# Temat 5: Operatory Inkrementacji i Dekrementacji

## 🎯 Cel

Przeciążanie `++` i `--` (pre i post)

---

## 📖 ++/-- Operatory

```csharp
// Pre-increment
public static T operator ++(T value)
{
    value.Increment();
    return value;
}

// Pre-decrement
public static T operator --(T value)
{
    value.Decrement();
    return value;
}
```

- C# nie ma specjalnej składni dla post-increment
- Zazwyczaj obie wersje robią to samo

---

## 📝 Przykład

```csharp
public class Counter
{
    public int Value { get; set; }

    public static Counter operator ++(Counter c)
    {
        c.Value++;
        return c;
    }

    public static Counter operator --(Counter c)
    {
        c.Value--;
        return c;
    }
}

var c = new Counter { Value = 5 };
++c;  // c.Value = 6
--c;  // c.Value = 5
```
