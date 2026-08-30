# Zadania: Singleton Pattern

## Zadanie 1: Database Connection

Stwórz Singleton `DatabaseConnection` z prywatnym konstruktorem i metodą `GetInstance()`.

## Zadanie 2: Application Settings

Stwórz Singleton `AppSettings` z metodami:
- `GetSetting(key)`
- `SetSetting(key, value)`

## Zadanie 3: Thread-safe Singleton

Stwórz thread-safe Singleton używając `Lazy<T>`.

---

## Rozwiązania

### Zadanie 1
```csharp
public sealed class DatabaseConnection
{
    private static DatabaseConnection? _instance;
    private static readonly object _lock = new();
    
    private DatabaseConnection() { }
    
    public static DatabaseConnection GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DatabaseConnection();
                }
            }
        }
        return _instance;
    }
}
```
