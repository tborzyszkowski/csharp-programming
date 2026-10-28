# Konstruktor Statyczny

## 🎯 Cel

Zrozumienie **konstruktora statycznego** - specjalnej metody inicjalizacji dla statycznych pól i logiki.

## Definicja

Konstruktor statyczny to inicjalizator typu uruchamiany przez runtime **najwyżej raz**, przed pierwszą operacją wymagającą inicjalizacji typu:

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

- ✅ Uruchamiany najwyżej raz podczas inicjalizacji typu
- ✅ Bez parametrów
- ✅ Nie ma modyfikatora dostępu i nie można wywołać go bezpośrednio
- ✅ Inicjalizuje statyczne pola
- ✅ Należy do klasy, nie do instancji

CLR uruchamia konstruktor statyczny przed pierwszym użyciem typu wymagającym jego inicjalizacji, a inicjalizacja typu jest synchronizowana. Konstruktor nie jest wywoływany przy każdym utworzeniu instancji. Jeśli zgłosi wyjątek, typ jest oznaczony jako niezainicjalizowany i kolejne użycia kończą się `TypeInitializationException` (z wyjątkiem pierwotnym jako `InnerException`).

Typ bez jawnego konstruktora statycznego może być oznaczony jako `beforefieldinit`; wtedy runtime może wykonać inicjalizację wcześniej niż bezpośrednio przed pierwszym odczytem pola statycznego.

Nie należy umieszczać w nim długich operacji I/O ani logiki zależnej od kolejności startu aplikacji. Warto też pamiętać, że proste inicjalizatory pól często wystarczają i są czytelniejsze.

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

✅ Użyj inicjalizatora pola, jeśli wystarcza; konstruktor statyczny dodaj, gdy potrzebna jest bardziej złożona inicjalizacja
✅ Czytaj konfigurację z environment variables
✅ Inicjalizuj cache'e i kolekcje

❌ Nie rób zbyt skomplikowanej logiki
❌ Unikaj ryzykownej inicjalizacji, która może rzucić wyjątek — runtime nie ponawia inicjalizacji tego typu w tym samym kontekście załadowania

❌ Nie zakładaj, że konstruktor statyczny wykona się dokładnie w momencie uruchomienia procesu.

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 🧭 Nawigacja

[← Metody statyczne](../_02_static_methods/README.md) · [Moduł 04](../README.md) · [Klasy statyczne →](../_04_static_classes/README.md)
