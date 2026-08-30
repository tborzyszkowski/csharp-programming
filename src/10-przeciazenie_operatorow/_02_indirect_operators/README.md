# Temat 2: Operatory Przeciążane Pośrednio (Implied Operators)

## 🎯 Cel

Zrozumieć relacje między operatorami - kiedy przeciążenie jednego wpływa na inne.

---

## 📖 Operatory Powiązane

### Grupa 1: Równość i Nierówność

```csharp
public static bool operator ==(T a, T b) ⟷ public static bool operator !=(T a, T b)
```

**Reguła:** Zawsze przeciażaj **parami**!

```csharp
public record Point(int X, int Y)
{
    public static bool operator ==(Point a, Point b)
        => a.X == b.X && a.Y == b.Y;
    
    public static bool operator !=(Point a, Point b)
        => !(a == b);  // MUSI być implementacja !=
}

var p1 = new Point(1, 2);
var p2 = new Point(1, 2);
var equal = p1 == p2;      // true
var notEqual = p1 != p2;   // false
```

### Grupa 2: Porównania Porządkowe

```csharp
public static bool operator <(T a, T b) ⟷ public static bool operator >(T a, T b)
public static bool operator <=(T a, T b) ⟷ public static bool operator >=(T a, T b)
```

**Reguła:** Zawsze przeciażaj **kompletne pary**!

```csharp
public static bool operator <(Temp a, Temp b) => a.Celsius < b.Celsius;
public static bool operator >(Temp a, Temp b) => a.Celsius > b.Celsius;
public static bool operator <=(Temp a, Temp b) => a.Celsius <= b.Celsius;
public static bool operator >=(Temp a, Temp b) => a.Celsius >= b.Celsius;
```

### Grupa 3: Operatory Logiczne (true/false)

```csharp
public static bool operator true(T value) ⟷ public static bool operator false(T value)
```

**Reguła:** Jeśli implementujesz `true`, **MUSISZ** implementować `false`!

```csharp
public class Result
{
    public bool Success { get; set; }
    
    public static bool operator true(Result r) => r.Success;
    public static bool operator false(Result r) => !r.Success;
}

var result = new Result { Success = true };
if (result) Console.WriteLine("Success!");  // Używa operator true
if (!result) Console.WriteLine("Failure!");  // Używa operator false
```

### Grupa 4: Operatory Arytmetyczne (Niejawne)

```csharp
public static Vector operator +(Vector a, Vector b)
public static Vector operator -(Vector a, Vector b)
```

**Reguła:** Semantycznie powiązane, ale nie wymagane bezpośrednio

```csharp
var v1 = new Vector(1, 2);
var v2 = new Vector(3, 4);
var sum = v1 + v2;   // [4, 6]
var diff = v1 - v2;  // [-2, -2]
```

### Grupa 5: Operatory Bitowe (Logiczne)

```csharp
public static T operator &(T a, T b)    // AND
public static T operator |(T a, T b)    // OR
public static bool operator true(T a)   // Konwersja na bool
public static bool operator false(T a)  // Konwersja na bool
```

**Reguła:** Jeśli przeciażysz & lub |, powinnaś także implementować true/false dla `&&` i `||`

```csharp
public class Flags
{
    public int Value { get; set; }
    
    public static Flags operator &(Flags a, Flags b)
        => new() { Value = a.Value & b.Value };
    
    public static Flags operator |(Flags a, Flags b)
        => new() { Value = a.Value | b.Value };
    
    public static bool operator true(Flags f)
        => f.Value != 0;
    
    public static bool operator false(Flags f)
        => f.Value == 0;
}

var f1 = new Flags { Value = 5 };
var f2 = new Flags { Value = 3 };

if (f1 & f2) { }  // && może być użyty
```

---

## ⚠️ Powiązania Niejawne

### Operatory, które NIE wymagają jawnej pary, ale są powiązane logicznie

| Operator | Powiązany Z | Typ Powiązania |
|----------|-------------|----------------|
| `==` | `!=` | WYMAGANE razem |
| `<` | `>` `<=` `>=` | WYMAGANE wszystkie |
| `++` | `--` | Semantycznie |
| `+` | `-` | Semantycznie |
| `true` | `false` | WYMAGANE razem |
| `&` | `\|` `^` | Logicznie |

---

## 📝 Praktyczne Wnioski

```csharp
// ✅ DOBRZE - Przeciażone parami
public static bool operator ==(Complex a, Complex b) => ...
public static bool operator !=(Complex a, Complex b) => ...

// ❌ ŹLE - Tylko jeden
public static bool operator ==(Complex a, Complex b) => ...
// Brakuje !=

// ✅ DOBRZE - Kompletne porównania
public static bool operator <(Money a, Money b) => ...
public static bool operator >(Money a, Money b) => ...
public static bool operator <=(Money a, Money b) => ...
public static bool operator >=(Money a, Money b) => ...

// ❌ ŹLE - Niekompletne
public static bool operator <(Money a, Money b) => ...
// Brakuje >, <=, >=
```

---

## 🔗 Referencje

- [Operator Overloading - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading)

---

## ➡️ Następny Krok

**Temat 3: Zasady i Best Practices** - Jak prawidłowo implementować operatory?
