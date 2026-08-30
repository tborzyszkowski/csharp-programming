# Temat 3: Metoda Definiująca Operator - Zasady Ogólne

## 🎯 Cel

Nauczysz się pisać poprawne deklaracje operatorów w C#.

---

## 📖 Składnia i Wymagania

### Podstawowa Struktura

```csharp
public static ReturnType operator SYMBOL(ParameterType operand)
{
    // Implementacja
}
```

### Wymagania

1. **Musi być `public static`**
   ```csharp
   public static Complex operator +(Complex a, Complex b)  // ✅ OK
   private static Complex operator +(Complex a, Complex b) // ❌ BŁĄD
   ```

2. **Przynajmniej jeden parametr musi być typu własnego**
   ```csharp
   public static Complex operator +(Complex a, int b)          // ✅ OK
   public static int operator +(int a, int b)                  // ❌ NIE - oba built-in
   ```

3. **Zwracany typ może być dowolny**
   ```csharp
   public static Complex operator +(Complex a, Complex b)      // Zwraca Complex
   public static bool operator ==(Money a, Money b)            // Zwraca bool
   ```

4. **Nie może mieć modyfikatorów readonly, sealed, virtual**
   ```csharp
   public virtual static operator +(T a, T b)     // ❌ BŁĄD
   public readonly static operator +(T a, T b)    // ❌ BŁĄD
   ```

5. **Parametry nie mogą mieć ref (oprócz `ref` zwracanego)**
   ```csharp
   public static T operator +(ref T a, T b)       // ❌ BŁĄD
   public static ref T operator ++(ref T a)       // ✅ OK (C# 7.0+)
   ```

---

## 📋 Tabela Operatorów - Liczba Parametrów

| Operator | Typ | # Param | Przykład |
|----------|-----|---------|----------|
| `+`, `-`, `!`, `~` | Unary | 1 | `T operator +(T a)` |
| `++`, `--` | Unary | 1 | `T operator ++(T a)` |
| `true`, `false` | Unary | 1 | `bool operator true(T a)` |
| `+`, `-`, `*`, `/` | Binary | 2 | `T operator +(T a, T b)` |
| `==`, `!=`, `<`, `>` | Binary | 2 | `bool operator ==(T a, T b)` |
| `[]` | N-ary | N | `T this[int i]` (indexer) |

---

## 💡 Best Practices

### 1. Immutability (Wzorzec Niezmienny)

```csharp
public record Vector(double X, double Y)
{
    // ✅ DOBRZE - Zwraca nowy obiekt
    public static Vector operator +(Vector a, Vector b)
        => new(a.X + b.X, a.Y + b.Y);
    
    // ❌ ŹLE - Modyfikuje istniejący obiekt
    public static Vector operator +(Vector a, Vector b)
    {
        a.X += b.X;  // Modyfikacja in-place!
        a.Y += b.Y;
        return a;
    }
}
```

### 2. Dokumentacja Operatorów

```csharp
/// <summary>
/// Dodaje dwa wektory komponentowo.
/// </summary>
/// <param name="a">Pierwszy wektor</param>
/// <param name="b">Drugi wektor</param>
/// <returns>Wektor będący sumą a + b</returns>
public static Vector operator +(Vector a, Vector b)
    => new(a.X + b.X, a.Y + b.Y);
```

### 3. Sprawdzanie Parametrów

```csharp
public static Complex operator /(Complex a, Complex b)
{
    if (b.Real == 0 && b.Imaginary == 0)
        throw new ArgumentException("Nie można dzielić przez 0");
    
    // Implementacja
    return result;
}
```

---

## 🔄 Pre i Post Increment/Decrement

```csharp
public class Counter
{
    public int Value { get; set; }

    // Pre-increment (++x)
    public static Counter operator ++(Counter c)
    {
        c.Value++;
        return c;
    }

    // Post-increment (x++) - C# nie da radzić czystego C++ reguł
    // Zazwyczaj obie wersje implementują to samo
}
```

---

## 🔗 Konwersje

```csharp
// Jawna konwersja (explicit)
public static explicit operator int(Temperature t)
    => (int)t.Celsius;

// Niejawna konwersja (implicit)
public static implicit operator Temperature(double celsius)
    => new Temperature(celsius);

var t = new Temperature(25);
int c = (int)t;         // Explicit
Temperature t2 = 30.0;  // Implicit
```

---

## 🔗 Referencje

- [Operator Overloading Syntax](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading)
- [Guidelines for Overloading](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/)

---

## ➡️ Następny Krok

**Temat 4: Operatory Jednoargumentowe** - Praktyka z +, -, !, ~
