# Zadania: Extension Methods

## Zadanie 1: Double Extensions

Stwórz extension metody dla `double`:
- `IsPositive()` - zwraca true jeśli > 0
- `IsNegative()` - zwraca true jeśli < 0
- `ToFahrenheit()` - zamienia Celsius na Fahrenheit

## Zadanie 2: List Extensions

Stwórz extension metody dla `IEnumerable<T>`:
- `IsEmpty()` - czy pusta
- `GetFirst()` - pierwszy element
- `GetLast()` - ostatni element
- `Reverse()` - odwrócona lista

---

## Rozwiązania

### Zadanie 1
```csharp
public static class DoubleExtensions
{
    public static bool IsPositive(this double num) => num > 0;
    public static bool IsNegative(this double num) => num < 0;
    public static double ToFahrenheit(this double celsius) => (celsius * 9/5) + 32;
}
```
