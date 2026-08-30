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
