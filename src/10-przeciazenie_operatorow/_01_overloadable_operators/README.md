# Temat 1: Operatory Które Można Przeciążyć w C#

## 🎯 Cel

Poznaj pełny katalog operatorów, które można przeciążać w C#.

---

## 📖 Kategorie Operatorów

### 1️⃣ Operatory Jednoargumentowe (Unary)

```csharp
public static T operator +(T value)        // +T
public static T operator -(T value)        // -T
public static T operator !(T value)        // !T (logiczne NOT)
public static T operator ~(T value)        // ~T (bitowe NOT)
public static T operator ++(T value)       // T++
public static T operator --(T value)       // T--
public static bool operator true(T value)  // if (obj) - konwersja do bool
public static bool operator false(T value) // if (!obj)
```

### 2️⃣ Operatory Binarne (Binary)

```csharp
public static T operator +(T a, T b)       // a + b
public static T operator -(T a, T b)       // a - b
public static T operator *(T a, T b)       // a * b
public static T operator /(T a, T b)       // a / b
public static T operator %(T a, T b)       // a % b
public static T operator &(T a, T b)       // a & b (bitowe AND)
public static T operator |(T a, T b)       // a | b (bitowe OR)
public static T operator ^(T a, T b)       // a ^ b (bitowe XOR)
public static T operator <<(T a, int b)    // a << b
public static T operator >>(T a, int b)    // a >> b
```

### 3️⃣ Operatory Relacyjne

```csharp
public static bool operator ==(T a, T b)   // a == b
public static bool operator !=(T a, T b)   // a != b
public static bool operator <(T a, T b)    // a < b
public static bool operator >(T a, T b)    // a > b
public static bool operator <=(T a, T b)   // a <= b
public static bool operator >=(T a, T b)   // a >= b
```

### 4️⃣ Operatory Konwersji (Conversion)

```csharp
public static explicit operator TargetType(SourceType value)  // jawna konwersja
public static implicit operator TargetType(SourceType value)  // niejawna konwersja
```

### 5️⃣ Operatory Indeksowania i Dostępu

```csharp
public T this[int index]          // indexer: obj[index]
public T this[string key]         // indexer: obj[key]
public static T operator checked +(T a, T b)  // checked context
```

---

## 📊 Tabela Wszystkich Przeciążalnych Operatorów

| Kategoria | Operatory | Można Przeciążyć? |
|-----------|-----------|------------------|
| **Arytmetyka** | +, -, *, /, % | ✅ TAK |
| **Jednoargumentowa** | +, -, !, ~ | ✅ TAK |
| **Inkrementacja** | ++, -- | ✅ TAK |
| **Logiczne** | &&, \|\| | ❌ NIE* |
| **Bitowe** | &, \|, ^, <<, >> | ✅ TAK |
| **Porównanie** | ==, !=, <, >, <=, >= | ✅ TAK |
| **Przypisanie** | =, +=, -=, *=, /=, etc | ❌ NIE** |
| **Konwersja** | explicit, implicit | ✅ TAK |
| **Indeksowanie** | [] | ✅ TAK |
| **Dostęp do metody** | . | ❌ NIE |
| **Dereferencia** | ->, *, & | ⚠️ Ograniczone |

*Można przeciążać & i |, wtedy && i || są automatycznie obsługiwane  
**Można przeciążać operatory binarne, a przypisanie jest pośrednio obsługiwane

---

## ⚠️ Ograniczenia i Zasady

### ❌ Operatory NIEPRZE CIAŻALNE

```csharp
obj = value;           // Assignment - nie można przeciążać
obj += value;          // Compound assignment - nie można bezpośrednio
obj.Field;             // Member access - nie można przeciążać
obj?.Property;         // Null-coalescing - nie można
obj => expr;           // Lambda - nie można
obj as Type;           // Type cast - nie można
```

### ✅ Wymagania Przeciążania

```csharp
// ✓ Musi być public static
public static T operator +(T a, T b)

// ✓ Przynajmniej jeden parametr musi być typu klasy/struktury
public static Complex operator +(Complex a, int b)  // ✅ OK
public static int operator +(int a, int b)         // ❌ NIE - obie built-in

// ✓ Return type może być dowolny (zwykle T)
public static bool operator ==(T a, T b)

// ✓ Nie może mieć ref parametrów (oprócz zwracanego ref)
public static T operator +(in T a, in T b)         // ❌ NIE
```

---

## 🔗 Operatory Powiązane (Implied)

Niektóre operatory są powiązane - przeciażenie jednego zmusza do przeciążenia drugiego:

| Operator | Powiązany | Reguła |
|----------|-----------|--------|
| == | != | Zawsze przeciążaj parami |
| < | > | Zawsze przeciążaj parami |
| <= | >= | Zawsze przeciążaj parami |
| true | false | Przeprowadź razem dla bool conversion |
| && | \|\| | Przeprowadź razem z true/false |
| + | - | Semantycznie powiązane |

---

## 📝 Praktyczne Przykłady (Preview)

```csharp
public class Money
{
    public decimal Amount { get; set; }
    
    // Arytmetyka
    public static Money operator +(Money a, Money b)
        => new Money { Amount = a.Amount + b.Amount };
    
    // Porównanie
    public static bool operator ==(Money a, Money b)
        => a.Amount == b.Amount;
    
    // Konwersja
    public static implicit operator Money(decimal amount)
        => new Money { Amount = amount };
}

var m1 = new Money { Amount = 100 };
var m2 = m1 + new Money { Amount = 50 };  // [150]
Money m3 = 99.99m;  // Niejawna konwersja
```

---

## 💡 Kiedy Przeciążać Operatory?

### ✅ Dobrze Przeciażyć

- Klasy reprezentujące wartości (Complex, Vector, Money)
- Operatory mają intuicyjne matematyczne znaczenie
- Kod staje się bardziej czytelny i naturalny
- Typ ma naturalną reprezentację
- Semantyka jest jasna dla użytkownika

### ❌ Źle Przeciażyć

- Operatory zmieniają znaczenie (+ jako concatenation zamiast dodawania)
- Kod staje się trudny do zrozumienia
- Operatory nie mają oczywistego znaczenia dla tego typu
- Przeciażanie operator to zaciemnia logikę biznesu

---

## 🔗 Referencje

- [Operator Overloading - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading)
- [Overloadable Operators - MSDN](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/)
- [C# Operator Precedence](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/)

---

## ➡️ Następny Krok

**Temat 2: Operatory Przeciażane Pośrednio** - Jakie operatory są ze sobą powiązane?
