# Temat 7: Operatory Relacyjne

## 🎯 Cel

Porównania: `==`, `!=`, `<`, `>`, `<=`, `>=`

---

## 📖 Operatory Relacyjne

```csharp
public static bool operator ==(T a, T b) => /* porównanie */;
public static bool operator !=(T a, T b) => !(a == b);
public static bool operator <(T a, T b) => /* porównanie */;
public static bool operator >(T a, T b) => a > b;
public static bool operator <=(T a, T b) => a < b || a == b;
public static bool operator >=(T a, T b) => a > b || a == b;
```

---

## 📝 Przykład

```csharp
public record Temperature(double Celsius)
{
    public static bool operator ==(Temperature a, Temperature b)
        => a.Celsius == b.Celsius;
    
    public static bool operator !=(Temperature a, Temperature b)
        => !(a == b);
    
    public static bool operator <(Temperature a, Temperature b)
        => a.Celsius < b.Celsius;
    
    public static bool operator >(Temperature a, Temperature b)
        => a.Celsius > b.Celsius;
}
```

---

## ⚠️ Reguła

**Zawsze przeciażaj porównania w kompletnychzestawach:**
- `==` i `!=`
- `<`, `>`, `<=`, `>=`
