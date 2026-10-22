# Wzorzec Singleton

## 🎯 Cel

**Singleton** - gwarantuje że klasa ma tylko jedną instancję.

## Klasyczna Implementacja

```csharp
public class Database
{
    private static Database? _instance;
    private static readonly object _lock = new();
    
    private Database() { }  // Prywatny konstruktor
    
    public static Database GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Database();
                }
            }
        }
        return _instance;
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

`Lazy<T>` domyślnie zapewnia bezpieczną inicjalizację w środowisku wielowątkowym i tworzy obiekt dopiero przy pierwszym odczycie `Value`. Nie rozwiązuje jednak problemu globalnego stanu ani nie ułatwia testowania zależności.

W aplikacjach korzystających z kontenera DI singleton zwykle konfiguruje się w kontenerze, np. jako `AddSingleton<T>`. Dzięki temu kod otrzymuje interfejs, można podmienić implementację w testach, a zakres życia obiektu jest jawny.

## Zastosowania

- Database connections
- Logger
- Cache
- Configuration
- File managers

## Best Practices

✅ Prywatny konstruktor
✅ Thread-safe
✅ Lazy initialization
✅ Sealed class (prevent inheritance)

❌ Nie używaj for mutable state
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
