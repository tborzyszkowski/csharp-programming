# Konstruktor Statyczny

## 🎯 Cel

Zrozumienie **konstruktora statycznego** - specjalnej metody inicjalizacji dla statycznych pól i logiki.

## Definicja

Konstruktor statyczny to metoda wywoływana **raz, przed pierwszym dostępem** do klasy:

```csharp
public class Database
{
    public static string ConnectionString;
    
    static Database()  // Konstruktor statyczny
    {
        ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? "default";
    }
}
```

## Charakterystyka

- ✅ Wywoływany raz przy pierwszym użyciu klasy
- ✅ Bez parametrów
- ✅ Bez modyfikatora dostępu (zawsze private)
- ✅ Inicjalizuje statyczne pola
- ✅ Należy do klasy, nie do instancji

## Zastosowania

### 1. Inicjalizacja ze zmiennych środowiskowych
```csharp
public class Config
{
    public static string DbUrl;
    public static int Timeout;
    
    static Config()
    {
        DbUrl = Environment.GetEnvironmentVariable("DB_URL") ?? "localhost";
        Timeout = int.Parse(Environment.GetEnvironmentVariable("TIMEOUT") ?? "30");
    }
}
```

### 2. Ładowanie konfiguracji
```csharp
public class Settings
{
    public static Dictionary<string, string> Values = new();
    
    static Settings()
    {
        Values["app_name"] = "MyApp";
        Values["version"] = "1.0.0";
        Values["debug"] = "true";
    }
}
```

### 3. Singleton pattern
```csharp
public class Logger
{
    private static Logger _instance;
    
    static Logger()
    {
        _instance = new Logger();
    }
    
    public static Logger Instance => _instance;
}
```

## Kolejność Inicjalizacji

```csharp
public class InitOrder
{
    public static string Name = "Field";  // Krok 1: inicjalizacja pola
    
    static InitOrder()  // Krok 2: konstruktor statyczny
    {
        Name = "StaticConstructor";
    }
}

var x = InitOrder.Name;  // "StaticConstructor"
```

## Best Practices

✅ Inicjalizuj statyczne pola w konstruktorze statycznym
✅ Czytaj konfigurację z environment variables
✅ Zailizuj cache'i i kolekcje

❌ Nie rób zbyt skomplikowanej logiki
❌ Nie rzucaj wyjątków bez obsługi

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```
