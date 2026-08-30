# Pole Statyczne (Static Fields)

## 🎯 Cel rozdziału

Zrozumienie **pól statycznych** - zmiennych należących do klasy, a nie do instancji. Każda instancja dzieli tę samą wartość.

## 📚 Spis treści

1. [Co to pole statyczne?](#co-to-pole-statyczne)
2. [Pole statyczne vs Pole instancji](#pole-statyczne-vs-pole-instancji)
3. [Inicjalizacja](#inicjalizacja)
4. [Zastosowania](#zastosowania)
5. [Best Practices](#best-practices)

---

## Co to pole statyczne?

**Pole statyczne** (static field) to zmienna należąca do klasy, nie do konkretnej instancji:

```csharp
public class Counter
{
    public static int TotalCount = 0;  // Pole statyczne
    public int Id { get; set; }        // Pole instancji
    
    public Counter()
    {
        TotalCount++;
    }
}

var c1 = new Counter();  // TotalCount = 1
var c2 = new Counter();  // TotalCount = 2
var c3 = new Counter();  // TotalCount = 3

Console.WriteLine(Counter.TotalCount);  // 3 - WSZYSTKIE instancje dzielą tę wartość!
```

**Kluczowe cechy**:
- ✅ Należy do **klasy**, nie do instancji
- ✅ Dzielona między **wszystkie instancje**
- ✅ Dostęp przez **nazwę klasy** `Counter.TotalCount`
- ✅ Inicjalizowana raz przy pierwszym użyciu
- ✅ Przechowuje się w **statycznym obszarze pamięci**

---

## Pole statyczne vs Pole instancji

| Aspekt | Pole Statyczne | Pole Instancji |
|--------|---|---|
| Należy do | Klasy | Instancji |
| Deklaracja | `static int field;` | `int field;` |
| Dostęp | `ClassName.field` | `instance.field` |
| Pamięć | Jedna kopia dla całej klasy | Oddzielna kopia dla każdej instancji |
| Inicjalizacja | Raz przy pierwszym użyciu | Za każdym razem w konstruktorze |
| Dziedziczenie | Nie dziedziczone | Dziedziczone |

**Przykład porównania**:

```csharp
public class BankAccount
{
    public static decimal InterestRate = 0.05m;  // Statyczne - wspólne dla wszystkich
    public decimal Balance { get; set; }         // Instancji - dla każdego konta
    
    public decimal CalculateInterest()
    {
        return Balance * InterestRate;  // Wspólna stopa procentowa
    }
}

var account1 = new BankAccount { Balance = 1000 };
var account2 = new BankAccount { Balance = 2000 };

Console.WriteLine(account1.CalculateInterest());  // 50
Console.WriteLine(account2.CalculateInterest());  // 100

BankAccount.InterestRate = 0.10m;  // Zmiana dla OBU kont!

Console.WriteLine(account1.CalculateInterest());  // 100
Console.WriteLine(account2.CalculateInterest());  // 200
```

---

## Inicjalizacja

### Inicjalizacja na miejscu

```csharp
public class Logger
{
    public static string LogFilePath = "log.txt";
    public static int MaxLogSize = 1000;
}
```

### Inicjalizacja w konstruktorze statycznym

```csharp
public class Database
{
    public static string ConnectionString;
    
    static Database()  // Konstruktor statyczny
    {
        ConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") 
                          ?? "DefaultConnection";
    }
}
```

**Kolejność inicjalizacji**:
1. Inicjalizacja wartości na miejscu (jeśli istnieje)
2. Konstruktor statyczny (jeśli istnieje)
3. Pierwsze użycie klasy

```csharp
public class InitOrder
{
    public static string Name = "InitialName";  // Krok 1
    
    static InitOrder()  // Krok 2
    {
        Name = "ConstructorName";
    }
}

// Przy pierwszym użyciu:
var x = InitOrder.Name;  // "ConstructorName"
```

---

## Zastosowania

### 1. Licznik (Counter)
```csharp
public class User
{
    public static int TotalUsers = 0;
    public string Name { get; set; }
    
    public User(string name)
    {
        Name = name;
        TotalUsers++;
    }
}

User u1 = new User("Alice");
User u2 = new User("Bob");
Console.WriteLine(User.TotalUsers);  // 2
```

### 2. Konfiguracja
```csharp
public class AppConfig
{
    public static string AppName = "MyApp";
    public static bool IsProduction = false;
    public static int MaxConnections = 100;
}

// Użycie
if (AppConfig.IsProduction)
    Console.WriteLine("Production mode");
```

### 3. Stałe wspólne
```csharp
public class Math2D
{
    public static double PI = 3.14159265359;
    public static double E = 2.71828182846;
    
    public static double CircleArea(double radius)
    {
        return PI * radius * radius;
    }
}
```

### 4. Singleton cache
```csharp
public class UserRepository
{
    public static Dictionary<int, User> Cache = new();
    
    public User? GetUser(int id)
    {
        if (Cache.ContainsKey(id))
            return Cache[id];
        
        // Pobranie z bazy...
        return null;
    }
}
```

---

## Best Practices

### ✅ DO:

- **Używaj `readonly`** dla pól statycznych jeśli nie zmienią się:
```csharp
public static readonly string AppName = "MyApp";
```

- **Inicjalizuj wartości sensowne**:
```csharp
public static int Counter = 0;  // Jasne zamiast null
```

- **Dokumentuj cel** pola statycznego:
```csharp
/// <summary>
/// Całkowita liczba utworzonych instancji tej klasy.
/// </summary>
public static int InstanceCount = 0;
```

### ❌ AVOID:

- **Nie modyfikuj zbyt często**:
```csharp
// ❌ ŹLE - logowanie zmiany przy każdym dostępie
public static int BadCounter = 0;  // Jeśli będzie zmieniane tysiące razy
```

- **Nie przechowuj mutable state bez synchronizacji**:
```csharp
// ❌ ŹLE - thread safety issues
public static List<string> Items = new();  // Wiele wątków może modyfikować
```

- **Nie ukrywaj tego że pole jest statyczne**:
```csharp
// ✅ DOBRZE - jasne że to static
public class Counter
{
    public static int Count = 0;
}

// ❌ ŹLE - mylące
public class Person
{
    public int SharedCount = 0;  // Looks like instance field!
}
```

---

## Thread Safety

Pola statyczne mogą być niebezpieczne w wielowątkowych aplikacjach:

```csharp
public class ThreadUnsafeCounter
{
    public static int Count = 0;
    
    public static void Increment()
    {
        Count++;  // ⚠️ RACE CONDITION!
    }
}

// Wielowątkowe wywołania mogą dać zły wynik
```

**Rozwiązanie**:
```csharp
public class ThreadSafeCounter
{
    private static readonly object lockObj = new();
    public static int Count { get; private set; } = 0;
    
    public static void Increment()
    {
        lock (lockObj)
        {
            Count++;  // ✅ Bezpieczne
        }
    }
}
```

---

## 📚 Referencje

- [Microsoft Learn: Static Members](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-members)
- [C# Operators and Expressions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/)
- [Thread Safety in .NET](https://learn.microsoft.com/en-us/dotnet/standard/threading/overview-of-synchronization-primitives)

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
