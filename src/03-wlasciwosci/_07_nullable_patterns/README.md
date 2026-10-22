# Nullable Reference Types (C# 8+)

## 🎯 Cel rozdziału

Zrozumienie Nullable Reference Types - system typów w C# 8+ który pomaga uniknąć `NullReferenceException`.

## 📚 Spis treści

1. [Co to NullReferenceException?](#co-to-nullreferenceexception)
2. [Nullable Annotations](#nullable-annotations)
3. [Non-nullable by Default](#non-nullable-by-default)
4. [Best Practices](#best-practices)

---

## Co to NullReferenceException?

**Najczęstszy błąd w C#** - próba dostępu do właściwości na null:

```csharp
string? name = null;
Console.WriteLine(name.Length);  // BŁĄD - NullReferenceException!
```

**Tradycyjnie nie było ochrony:**
```csharp
public class Person
{
    public string Name { get; set; }  // Może być null!
}

var person = new Person();
// person.Name = null;  // Akceptuje!
// var len = person.Name.Length;  // Runtime crash!
```

---

## Nullable Annotations (C# 8+)

**Nullable reference types** pozwala oznaczyć które typy mogą być null:

```csharp
#nullable enable  // Włącz sprawdzanie

public class Person
{
    public string Name { get; set; } = string.Empty;  // Non-nullable
    public string? Phone { get; set; }  // Nullable
}

var person = new Person { Name = "John" };
Console.WriteLine(person.Name.Length);  // OK - Name nie może być null
// person.Phone.Length;  // BŁĄD - Phone jest nullable!

if (person.Phone != null)
{
    Console.WriteLine(person.Phone.Length);  // OK - after null-check
}
```

Adnotacje nullable nie zmieniają reprezentacji obiektu ani nie dodają automatycznego sprawdzania w runtime. Są informacją dla kompilatora i narzędzi, która opisuje kontrakt API. Wartość z zewnętrznego źródła (JSON, baza, użytkownik) nadal trzeba sprawdzić podczas działania programu.

---

## Non-nullable by Default

W C# 8+ **referencje są non-nullable by default**:

```csharp
#nullable enable

public class Config
{
    public string? ConnectionString { get; set; }  // Nullable
    public int Timeout { get; set; }  // Non-nullable
    public string ApiKey { get; set; } = string.Empty;  // Non-nullable + init
}

// Compiler warns:
var config = new Config();  // ConnectionString = null (warning!)
```

Właściwość typu `string` powinna zostać zainicjalizowana w deklaracji, konstruktorze albo oznaczona jako `required`. `string?` komunikuje, że brak wartości jest przewidzianym przypadkiem i wymusza obsłużenie go przed użyciem.

```csharp
string? input = GetInput();
string normalized = input?.Trim() ?? "brak danych";

static string? GetInput() => null;
```

Operator `?.` zatrzymuje wywołanie przy `null`, a `??` dostarcza wartość zastępczą. Nie używaj operatora `!` jako sposobu na usunięcie problemu bez dowodu, że wartość rzeczywiście nie jest pusta - wyłącza on tylko ostrzeżenie kompilatora.

---

## Best Practices

✅ **Enable `#nullable enable`** na górze pliku

✅ **`string?` dla opcjonalnych** pól

✅ **`string` z inicjalizacją** dla required

✅ **Null-checks** przed użyciem nullable

✅ **Null-coalescing** operator `??`

```csharp
string displayName = person.Phone ?? "N/A";
```

---

## Levels

| Level | Znaczenie |
|-------|-----------|
| `#nullable enable` | Pełne sprawdzanie |
| `#nullable disable` | Brak sprawdzania |
| `#nullable safeonly` | Tylko read |

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)

## 🧭 Nawigacja

[← Init Properties](../_06_init_properties/README.md) · [Moduł 03](../README.md) · [Następny moduł: 04-statyczne →](../../04-statyczne/README.md)
