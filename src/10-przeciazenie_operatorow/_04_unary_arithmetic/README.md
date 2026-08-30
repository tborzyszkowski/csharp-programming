# Temat 4: Operatory Jednoargumentowe - Arytmetyka i Logika

## 🎯 Cel

Nauczysz się przeciażać operatory: `+`, `-`, `!`, `~`

---

## 📖 Operatory

### Unary Plus `+`

```csharp
public static T operator +(T value)
```

- Zwraca nowy obiekt (zwykle kopia)
- Dla liczb: zwraca tę samą wartość
- Dla wektorów: kopia wektora

```csharp
var v1 = new Vector(1, 2);
var v2 = +v1;  // Kopia v1
```

### Unary Minus `-`

```csharp
public static T operator -(T value)
```

- Zwraca negację wartości
- Zmienia znak
- Dla wektorów: wskazuje przeciwny kierunek

```csharp
var m = new Money(100);
var neg = -m;  // [-100]
```

### Logical NOT `!`

```csharp
public static bool operator !(T value)
```

- Zwraca `true` lub `false`
- Zwykle: negacja warunku "bycia zerowym"

```csharp
var z = new Complex(0, 0);
if (!z) Console.WriteLine("Zero!");
```

### Bitwise NOT `~`

```csharp
public static T operator ~(T value)
```

- Bitowa negacja
- Zwraca nową wartość z odwróconymi bitami
- Dla struktur bitowych

```csharp
var flags = new Flags(0b1100);
var inverted = ~flags;  // 0b0011
```

---

## 📋 Praktyczne Przykłady

```csharp
public record Complex(double Real, double Imaginary)
{
    // Unary +
    public static Complex operator +(Complex c) => c;
    
    // Unary -
    public static Complex operator -(Complex c)
        => new(-c.Real, -c.Imaginary);
    
    // Logical NOT (czy zero?)
    public static bool operator !(Complex c)
        => c.Real == 0 && c.Imaginary == 0;
    
    // Magnitude
    public double Magnitude => Math.Sqrt(Real*Real + Imaginary*Imaginary);
}
```

---

## ➡️ Następny Krok

**Temat 5: Operatory ++/--**
