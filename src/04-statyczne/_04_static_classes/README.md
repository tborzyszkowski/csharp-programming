# Klasy Statyczne

## 🎯 Cel

**Klasa statyczna** zawiera tylko statyczne członkowie. Nie można tworzyć instancji.

## Definicja

```csharp
public static class MathHelper
{
    public static double PI = 3.14159;
    
    public static double CircleArea(double radius)
    {
        return PI * radius * radius;
    }
}

// Użycie - bezpośredni dostęp do klasy
double area = MathHelper.CircleArea(5);

// Nie można: new MathHelper()  - BŁĄD!
```

## Cechy

- ✅ Tylko statyczne członkowie
- ✅ Nie można instancjonować
- ✅ Niejawnie sealed
- ✅ Brak konstruktora instancji

## Zastosowania

1. **Utility funkcje** - Math, String, File operations
2. **Globalna konfiguracja** - Settings, Config
3. **Helper metody** - Converters, Validators

## Przykłady Z .NET

- `System.Math` - operacje matematyczne
- `System.String` - operacje na stringach
- `System.IO.File` - operacje na plikach

## Best Practices

✅ Używaj dla kolekacji utility funkcji
✅ Jasne nazwy (Helper, Service, Util)

❌ Nie przechowuj stanu mutable
❌ Unikaj przesady - grupy logiczne

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```
