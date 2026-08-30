# Zadania: Static Properties i Lazy<T>

## Zadanie 1: Application Settings

Stwórz klasę `Settings` ze static properties:
- `DatabaseUrl` (default: "localhost")
- `Port` (default: 5432)
- `MaxConnections` (default: 100)

## Zadanie 2: Lazy Database Connection

Stwórz `DatabaseConnection` z `Lazy<T>` inicjalizacją.

## Zadanie 3: Configuration Cache

Stwórz `ConfigCache` z `Lazy<Dictionary<string, string>>` zawierającą domyślne wartości.

---

## Rozwiązania

### Zadanie 1
```csharp
public class Settings
{
    public static string DatabaseUrl { get; set; } = "localhost";
    public static int Port { get; set; } = 5432;
    public static int MaxConnections { get; set; } = 100;
}
```

### Zadanie 2
```csharp
public class DatabaseConnection
{
    private static readonly Lazy<DatabaseConnection> _instance =
        new(() => new DatabaseConnection());
    
    public static DatabaseConnection Instance => _instance.Value;
    
    private DatabaseConnection() { }
}
```
