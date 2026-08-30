# Metody Rozszerzające (Extension Methods)

## 🎯 Cel

**Metody rozszerzające** to statyczne metody które wyglądają jak metody instancji na istniejących typach.

## Definicja

```csharp
public static class StringExtensions
{
    public static int CountWords(this string str)
    {
        return str.Split(' ').Length;
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

## Zastosowania

### 1. Rozszerzanie String
```csharp
public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
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

❌ Nie można rozszerzać static members
❌ Nie można nadpisywać istniejących metod
❌ Nie mają dostępu do private pól
❌ Muszą być w statycznej klasie

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
