# Static Properties i Lazy<T> (C# 9+)

## 🎯 Cel

**Static Properties** i **Lazy<T>** - nowoczesne sposoby na inicjalizację leniwa i thread-safe.

## Static Properties

```csharp
public class AppConfig
{
    public static string ApiUrl { get; set; } = "http://localhost";
    public static int Timeout { get; set; } = 30;
    public static bool IsDebug { get; set; }
}

// Dostęp
AppConfig.ApiUrl = "http://api.example.com";
Console.WriteLine(AppConfig.Timeout);
```

Publiczna statyczna właściwość z `set` jest globalnym, mutowalnym stanem. Każdy fragment kodu może zmienić wartość, dlatego w konfiguracji często lepiej użyć `init`, prywatnego settera albo jawnej metody aktualizacji, która sprawdza poprawność danych.

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

## Statyczne Readonly Properties

```csharp
public class Constants
{
    public static readonly string Version = "1.0.0";
    public static readonly DateTime BuildDate = DateTime.Now;
}

// Mogą być czytane, ale nie zmieniane
// Constants.Version = "2.0.0";  // BŁĄD!
```

## Zastosowania

1. **Konfiguracja aplikacji** - ApiUrl, Timeout, Debug mode
2. **Zasoby kosztowne** - Database, Network connections
3. **Cached data** - User sessions, Templates
4. **Stałe** - Version, Build date

## Porównanie Podejść

| Podejście | Pros | Cons |
|-----------|------|------|
| Static field | Prosty | Nie thread-safe, mutable |
| Static property | Kontrola | Wymaga getter/setter |
| Lazy<T> | Thread-safe, lazy | Bardziej skomplikowany |
| Static readonly | Immutable | Nie można zmienić |

`static readonly` chroni referencję lub wartość, ale nie zamraża mutowalnego obiektu. Dla cache'a warto udostępniać interfejs tylko do odczytu i kontrolować modyfikacje wewnątrz typu.

## Best Practices

✅ Używaj `Lazy<T>` dla kosztownych zasobów
✅ Używaj `static readonly` dla constants
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
