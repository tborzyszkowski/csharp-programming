# Klasy Statyczne

## 🎯 Cel

**Klasa statyczna** nie może być instancjonowana ani dziedziczona; jej zwykłe składowe są statyczne.

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

Klasa statyczna jest dobrym miejscem dla bezstanowych funkcji, ale nie jest zamiennikiem każdej usługi. Nie może implementować interfejsu ani być przekazana jako zależność przez konstruktor. Jeśli kod ma posiadać konfigurację, zależności lub stan per instancja, zwykła klasa będzie bardziej elastyczna.

## Zastosowania

1. **Utility funkcje** - Math, String, File operations
2. **Globalna konfiguracja** - Settings, Config
3. **Helper metody** - Converters, Validators

## Przykłady z .NET

- `System.Math` - operacje matematyczne
- `System.String` - typ instancji z metodami statycznymi, nie klasa statyczna
- `System.IO.File` - operacje na plikach
- `System.IO.Path` - operacje na ścieżkach

## Best Practices

✅ Używaj dla kolekcji bezstanowych funkcji utility
✅ Jasne nazwy (Helper, Service, Util)

✅ Przyjmuj dane przez parametry i zwracaj wynik zamiast modyfikować globalny stan.

❌ Nie przechowuj stanu mutable
❌ Unikaj przesady - grupy logiczne

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 🧭 Nawigacja

[← Konstruktor statyczny](../_03_static_constructor/README.md) · [Moduł 04](../README.md) · [Metody rozszerzające →](../_05_extension_methods/README.md)
