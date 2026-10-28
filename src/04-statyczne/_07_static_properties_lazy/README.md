# Właściwości statyczne i `Lazy<T>`

## 🎯 Cel

Właściwości statyczne udostępniają stan typu przez kontrolowany interfejs. `Lazy<T>` pozwala odroczyć utworzenie wartości; domyślna strategia jest bezpieczna wątkowo dla samej inicjalizacji.

## Static Properties

```csharp
public class AppConfig
{
    public static string ApiUrl { get; private set; } = "http://localhost";
    public static int Timeout { get; private set; } = 30;
    public static bool IsDebug { get; private set; }

    public static void Configure(string apiUrl, int timeout, bool isDebug)
    {
        if (string.IsNullOrWhiteSpace(apiUrl))
            throw new ArgumentException("API URL cannot be empty.", nameof(apiUrl));
        if (timeout <= 0)
            throw new ArgumentOutOfRangeException(nameof(timeout));

        ApiUrl = apiUrl;
        Timeout = timeout;
        IsDebug = isDebug;
    }
}

// Dostęp
AppConfig.Configure("https://api.example.com", 30, isDebug: false);
Console.WriteLine(AppConfig.Timeout);
```

Publiczna statyczna właściwość z `set` jest współdzielonym, mutowalnym stanem. Każdy fragment kodu może zmienić wartość, dlatego w konfiguracji często lepiej użyć `init`, prywatnego settera albo jawnej metody aktualizacji, która sprawdza poprawność danych. Synchronizacja `Lazy<T>` nie sprawia, że późniejsze zmiany konfiguracji ani operacje na utworzonym obiekcie są bezpieczne wątkowo.

## Lazy<T> - Inicjalizacja Leniwa

```csharp
public class ExpensiveResource
{
    private static readonly Lazy<ExpensiveResource> _instance =
        new(() => new ExpensiveResource());
    
    public static ExpensiveResource Instance => _instance.Value;
    
    private ExpensiveResource()
    {
        Console.WriteLine("Expensive initialization");
    }
}

// Używanie
var resource = ExpensiveResource.Instance;  // Inicjalizuje tylko teraz
var resource2 = ExpensiveResource.Instance;  // Zwraca cached instance
```

`Lazy<T>` przechowuje wartość po pierwszym wywołaniu `Value`. Domyślna strategia jest thread-safe, ale jeżeli fabryka rzuci wyjątek, informacja o błędzie jest zapamiętana i kolejne odczyty również go zgłoszą. To ważne przy ładowaniu konfiguracji lub połączeń.

## Statyczne pola `readonly`

```csharp
public class RuntimeValues
{
    public static readonly string Version = "1.0.0";
    public static readonly DateTime ProcessStartedAt = DateTime.Now;
}

// Pola można odczytać, lecz nie można przypisać do nich poza inicjalizatorem
// lub konstruktorem statycznym.
```

`static readonly` nie oznacza stałej czasu kompilacji: wartość może być ustalona podczas inicjalizacji typu. `const` jest stałą czasu kompilacji i ma inne zasady (jest niejawnie statyczne). Przypisanie `DateTime.Now` w polu `static readonly` zapisuje czas inicjalizacji typu, a nie czas kompilacji.

## Zastosowania

1. **Konfiguracja aplikacji** - ApiUrl, Timeout, Debug mode
2. **Zasoby kosztowne** - Database, Network connections
3. **Cached data** - User sessions, Templates
4. **Wartości niezmienne po inicjalizacji** - Version, moment inicjalizacji typu

## Porównanie Podejść

| Podejście | Pros | Cons |
|-----------|------|------|
| Static field | Prosty | Publiczne pole nie zapewnia enkapsulacji |
| Static property | Kontrola dostępu przez get/set | Sama właściwość nie zapewnia synchronizacji |
| Lazy<T> | Leniwa, domyślnie synchronizowana inicjalizacja | Nie synchronizuje późniejszego użycia wartości |
| Static readonly | Nie można ponownie przypisać pola po inicjalizacji typu | Obiekt wskazywany przez pole nadal może być mutowalny |

`static readonly` chroni referencję lub wartość, ale nie zamraża mutowalnego obiektu. Dla cache'a warto udostępniać interfejs tylko do odczytu i kontrolować modyfikacje wewnątrz typu.

## Best Practices

✅ Używaj `Lazy<T>` dla kosztownych zasobów
✅ Używaj `const` dla stałych czasu kompilacji, a `static readonly` dla wartości ustalanych w czasie działania
✅ Properties dla configuracji
✅ Dokumentuj intent

❌ Nie mieszaj byle jakie state
❌ Unikaj side effects w getterach
❌ Pamiętaj o thread-safety

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 🧭 Nawigacja

[← Singleton](../_06_singleton_pattern/README.md) · [Moduł 04](../README.md) · [Następny moduł: 05-dziedziczenie →](../../05-dziedziczenie/README.md)
