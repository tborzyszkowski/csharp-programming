# Zadania: Konstruktor Statyczny

## Zadanie 1: Konfiguracja Z Zmiennych Środowiskowych

Stwórz klasę `AppConfig` która czyta zmienne środowiskowe w konstruktorze statycznym:
- `ApiUrl` (default: "http://localhost")
- `Port` (default: 8080)
- `IsProduction` (default: false)

## Zadanie 2: Cache Inicjalizacja

Stwórz klasę `Cache` z Dictionary'iem inicjalizowanym w konstruktorze statycznym:
- Wstępnie załaduj 5 kluczy: "key1", "key2", itp.

## Zadanie 3: Singleton Logger

Stwórz `Logger` z prywatnymi polami inicjalizowanymi w konstruktorze statycznym.

---

## Rozwiązania

### Zadanie 1
```csharp
public class AppConfig
{
    public static string ApiUrl;
    public static int Port;
    public static bool IsProduction;
    
    static AppConfig()
    {
        ApiUrl = Environment.GetEnvironmentVariable("API_URL") ?? "http://localhost";
        Port = int.Parse(Environment.GetEnvironmentVariable("PORT") ?? "8080");
        IsProduction = bool.Parse(Environment.GetEnvironmentVariable("IS_PRODUCTION") ?? "false");
    }
}
```
