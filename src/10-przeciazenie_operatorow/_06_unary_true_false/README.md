# Temat 6: Operatory true i false - Konwersja do bool

## 🎯 Cel

Nauczysz się robić konwersje na bool dla logicznych warunków.

---

## 📖 Operatory true/false

```csharp
// Konwersja do true
public static bool operator true(T value)
    => /* logika zwracająca true/false */;

// Konwersja do false
public static bool operator false(T value)
    => /* logika zwracająca true/false */;
```

### Zastosowanie

```csharp
var result = new Result { Success = true };

if (result)          // Używa operator true
    Console.WriteLine("OK!");

if (!result)         // Używa operator false
    Console.WriteLine("NOT OK!");
```

---

## 📝 Przykład

```csharp
public class OptionalInt
{
    public int? Value { get; set; }

    public static bool operator true(OptionalInt oi)
        => oi.Value.HasValue;

    public static bool operator false(OptionalInt oi)
        => !oi.Value.HasValue;
}

var opt = new OptionalInt { Value = 42 };
if (opt)  // true
    Console.WriteLine("Has value!");
```

---

## 📊 Implikacje dla && i ||

Jeśli zdefiniujesz true/false, możesz użyć:
- `&&` - wymaga definicji `operator &`
- `||` - wymaga definicji `operator |`
