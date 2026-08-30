# Temat 8: Operatory Binarne

## 🎯 Cel

Operatory na dwóch argumentach: `+`, `-`, `*`, `/`, `%`, `&`, `|`, `^`, `<<`, `>>`

---

## 📖 Operatory Binarne

```csharp
// Arytmetyka
public static T operator +(T a, T b) => ...;
public static T operator -(T a, T b) => ...;
public static T operator *(T a, T b) => ...;
public static T operator /(T a, T b) => ...;
public static T operator %(T a, T b) => ...;

// Bitowe
public static T operator &(T a, T b) => ...;
public static T operator |(T a, T b) => ...;
public static T operator ^(T a, T b) => ...;
public static T operator <<(T a, int b) => ...;
public static T operator >>(T a, int b) => ...;
```

---

## 📝 Praktyka

```csharp
public record Vector(double X, double Y)
{
    public static Vector operator +(Vector a, Vector b)
        => new(a.X + b.X, a.Y + b.Y);
    
    public static Vector operator -(Vector a, Vector b)
        => new(a.X - b.X, a.Y - b.Y);
    
    public static Vector operator *(Vector v, double scalar)
        => new(v.X * scalar, v.Y * scalar);
}
```
