# Wzorzec Singleton

## 🎯 Cel

**Singleton** - zapewnia jeden dostępny obiekt danego typu w określonym kontekście aplikacji (zwykle w obrębie procesu lub kontenera DI).

## Klasyczna Implementacja

```csharp
public class Database
{
    private static volatile Database? _instance;
    private static readonly object _lock = new();
    
    private Database() { }  // Prywatny konstruktor
    
    public static Database GetInstance()
    {
        Database? instance = _instance;
        if (instance is null)
        {
            lock (_lock)
            {
                instance = _instance;
                if (instance is null)
                {
                    instance = new Database();
                    _instance = instance;
                }
            }
        }
        return instance;
    }
}

// Użycie
var db1 = Database.GetInstance();
var db2 = Database.GetInstance();
// db1 == db2  ->  true
```

## Nowoczesna Implementacja (C#.NET)

```csharp
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = 
        new(() => new Logger());
    
    private Logger() { }
    
    public static Logger Instance => _instance.Value;
}
```

Wersja z double-checked locking wymaga nie tylko blokady, ale także bezpiecznego odczytu referencji poza blokadą; dlatego pole jest `volatile`. W praktyce preferuj `Lazy<T>` lub mechanizm zarządzania czasem życia z kontenera DI.

`Lazy<T>` z domyślną strategią zapewnia bezpieczną inicjalizację w środowisku wielowątkowym i tworzy obiekt dopiero przy pierwszym odczycie `Value`. Nie rozwiązuje jednak problemu globalnego stanu ani nie ułatwia testowania zależności. Singleton zarejestrowany w DI jest singletonem w ramach danej instancji kontenera.

W aplikacjach korzystających z kontenera DI singleton zwykle konfiguruje się w kontenerze, np. jako `AddSingleton<T>`. Dzięki temu kod otrzymuje interfejs, można podmienić implementację w testach, a zakres życia obiektu jest jawny.

## Zastosowania

- Database connections (ostrożnie z zarządzaniem pulą i cyklem życia)
- Logger
- Cache
- Configuration
- File managers

## Best Practices

✅ Prywatny konstruktor
✅ Thread-safe
✅ Lazy initialization
✅ Sealed class (prevent inheritance)

❌ Nie używaj go jako globalnego pojemnika na współdzielony, mutowalny stan
❌ Unikaj w testach (hard to mock)

❌ Nie używaj Singletona tylko po to, aby ukryć brak właściwego modelu zależności.

## Ograniczenia

- Trudne do testowania
- Globalne state
- Może maskować design issues

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 🧭 Nawigacja

[← Metody rozszerzające](../_05_extension_methods/README.md) · [Moduł 04](../README.md) · [Static Properties i Lazy<T> →](../_07_static_properties_lazy/README.md)
