# Metody Rozszerzające (Extension Methods)

## 🎯 Cel

**Klasyczne metody rozszerzające** to statyczne metody, które można wywołać składnią przypominającą metodę instancji istniejącego typu.

## Definicja

```csharp
public static class StringExtensions
{
    public static int CountWords(this string str)
    {
        return string.IsNullOrEmpty(str)
            ? 0
            : str.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// Użycie jak metoda instancji!
string text = "Hello World Test";
int count = text.CountWords();  // 3
```

## Składnia

```csharp
public static <return-type> <method-name>(this <type> <parameter>, <other-params>)
{
    // Implementacja
}
```

**Kluczowe**: Słowo kluczowe `this` jako pierwszy parametr!

Kompilator traktuje wywołanie `text.CountWords()` jako skrót dla `StringExtensions.CountWords(text)`. Metoda rozszerzająca nie zmienia typu i nie ma uprzywilejowanego dostępu do jego prywatnych pól. Jeśli typ ma już pasujący członek instancji, ma on pierwszeństwo przed metodą rozszerzającą. Klasyczna składnia metod rozszerzających wymaga statycznej metody w niegenerycznej, statycznej klasie najwyższego poziomu. C# 14 wprowadza również bloki rozszerzeń; nie są one częścią przykładów w tym module.

## Zastosowania

### 1. Rozszerzanie String
```csharp
public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);
        if (str.Length == 0)
            return str;

        return char.ToUpperInvariant(str[0]) + str[1..].ToLowerInvariant();
    }
    
    public static bool IsValidEmail(this string str)
    {
        return str.Contains("@");
    }
}

string text = "hello";
text.Capitalize();  // "Hello"
text.IsValidEmail();  // false
```

`IsValidEmail` jest tu tylko uproszczonym przykładem rozszerzenia, a nie pełną walidacją adresu e-mail.

### 2. Rozszerzanie Int
```csharp
public static class IntExtensions
{
    public static bool IsEven(this int num)
    {
        return num % 2 == 0;
    }
    
    public static int Square(this int num)
    {
        return num * num;
    }
}

int x = 5;
x.IsEven();  // false
x.Square();  // 25
```

### 3. LINQ (Chain metod)
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var result = numbers.Where(x => x > 2)
                    .Select(x => x * 2)
                    .ToList();
```

## Ograniczenia

❌ Klasyczna metoda rozszerzająca jest wywoływana na odbiorcy jako instancji; nie dodaje członków statycznych do typu
❌ Nie przesłania ani nie zastępuje członków typu
❌ Nie mają dostępu do private pól
❌ Muszą być w statycznej klasie

Argument oznaczony `this` może być `null` tylko wtedy, gdy metoda jawnie to obsługuje. Dla typów referencyjnych warto użyć `string?` i określić zachowanie dla braku wartości, zamiast doprowadzać do `NullReferenceException`.

## Best Practices

✅ Umieszczaj w namespace pokrewnym typowi
✅ Jasne nazwy (raczej `Capitalize` niż `Cap`)
✅ Dokumentuj intent
✅ Unikaj zbyt wielu - może być zamieszanie

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 🧭 Nawigacja

[← Klasy statyczne](../_04_static_classes/README.md) · [Moduł 04](../README.md) · [Singleton →](../_06_singleton_pattern/README.md)
