# Zadania - Metody Częściowe

## 📝 Zadanie 1: Logger

Utwórz klasę `Logger` z metodami `Info`, `Warning`, `Error`, które zapisują wpis na liste logów, a następnie wołają
metodę częściową `OnLogAdded(string level, string message)`. Implementacja (w drugiej części klasy) wypisuje wpis
na konsolę. Sprawdź, co się stanie po **usunięciu** implementacji – czy kod nadal się kompiluje i działa?

```csharp
public partial class Logger
{
    partial void OnLogAdded(string level, string message);   // deklaracja
}

public partial class Logger
{
    partial void OnLogAdded(string level, string message)    // implementacja
    {
        Console.WriteLine($"[{level}] {message}");
    }
}
```

---

## 📝 Zadanie 2: Event handler

Stwórz `UserManager` z metodą częściową:
- Część 1: `partial void OnUserRegistered(string username)`
- Część 2: Implementacja - wypisz komunikat

---

## 📝 Zadanie 3: Validation hooks

Utwórz `Form` z metodami częściowymi:
- `partial void OnValidating()`
- `partial void OnValidationPassed()`
- `partial void OnValidationFailed(string error)`

---

## 📝 Zadanie 4 (dla chętnych): Rozszerzona metoda częściowa

Dodaj do `UserManager` metodę `public partial bool CanRegister(string username)`, która decyduje, czy
użytkownika wolno zarejestrować (np. odrzuca już istniejące nazwy). Sprawdź, jaki błąd kompilatora dostaniesz,
jeśli pominiesz implementację, i wyjaśnij dlaczego (por. README tematu).

---

## ✅ Zadanie 1 - Rozwiązanie: Logger

### Kod - Część 1 (Deklaracja)

```csharp
public partial class Logger
{
    private List<string> logs = new();
    
    // Deklaracja metody częściowej - bez ciała
    partial void OnLogAdded(string level, string message);
    
    public void Info(string message)
    {
        logs.Add($"[INFO] {message}");
        OnLogAdded("INFO", message);
    }
    
    public void Warning(string message)
    {
        logs.Add($"[WARN] {message}");
        OnLogAdded("WARN", message);
    }
    
    public void Error(string message)
    {
        logs.Add($"[ERROR] {message}");
        OnLogAdded("ERROR", message);
    }
    
    public IReadOnlyList<string> GetLogs() => logs.AsReadOnly();
}
```

### Kod - Część 2 (Implementacja)

```csharp
public partial class Logger
{
    // Implementacja metody częściowej
    partial void OnLogAdded(string level, string message)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}");
    }
}

// Test
var logger = new Logger();
logger.Info("Aplikacja uruchomiona");
logger.Warning("Mało pamięci");
logger.Error("Błąd!");
Console.WriteLine($"Total logs: {logger.GetLogs().Count}");
```

### Wyjaśnienie

- **Część 1**: Deklaruje metodę częściową `partial void OnLogAdded`
- **Część 2**: Implementuje to, co zadeklarowano w Części 1
- Jeśli implementacji **nie ma**, kompilator **usuwa** wywołanie w Części 1 (i nie oblicza jego argumentów)
- Przydatne dla: hooków, callbacków, opcjonalnego logowania w kodzie generowanym

---

## ✅ Zadanie 2 - Rozwiązanie: Event Handler

### Kod - Część 1 (Deklaracja)

```csharp
public partial class UserManager
{
    private List<string> users = new();
    
    // Metoda częściowa - hook na rejestrację
    partial void OnUserRegistered(string username);
    
    public void RegisterUser(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("Username nie może być pusty");
        
        users.Add(username);
        OnUserRegistered(username);  // Wywołanie hooku
    }
    
    public int UserCount => users.Count;
}
```

### Kod - Część 2 (Implementacja)

```csharp
public partial class UserManager
{
    partial void OnUserRegistered(string username)
    {
        Console.WriteLine($"✓ Nowy użytkownik zarejestrowany: {username}");
        Console.WriteLine($"  Czas: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    }
}

// Test
var manager = new UserManager();
manager.RegisterUser("alice");
manager.RegisterUser("bob");
manager.RegisterUser("charlie");
Console.WriteLine($"Total users: {manager.UserCount}");
```

### Wyjaśnienie

- Hook `OnUserRegistered` jest wywoływany **za każdym razem**, gdy ktoś się zarejestruje
- Implementacja w Części 2 decyduje **co się dzieje**
- Jeśli usunąć Część 2, hook nic nie robi (bez błędu!)
- Podobne do zdarzenia w .NET, ale rozstrzygane w czasie kompilacji i **lżejsze** (zero kosztu, gdy brak implementacji);
  ma jednak tylko jedną implementację i nie można jej zmieniać w czasie działania

---

## ✅ Zadanie 3 - Rozwiązanie: Validation Hooks

### Kod - Część 1 (Deklaracja)

```csharp
public partial class Form
{
    private Dictionary<string, string> fields = new();
    
    // Hooki validacji
    partial void OnValidating();
    partial void OnValidationPassed();
    partial void OnValidationFailed(string error);
    
    public void AddField(string name, string value)
    {
        fields[name] = value;
    }
    
    public bool Validate()
    {
        OnValidating();
        
        // Sprawdzenie
        foreach (var field in fields)
        {
            if (string.IsNullOrEmpty(field.Value))
            {
                OnValidationFailed($"Pole '{field.Key}' jest puste");
                return false;
            }
        }
        
        OnValidationPassed();
        return true;
    }
}
```

### Kod - Część 2 (Implementacja Hooków)

```csharp
public partial class Form
{
    partial void OnValidating()
    {
        Console.WriteLine("🔍 Walidacja w toku...");
    }
    
    partial void OnValidationPassed()
    {
        Console.WriteLine("✓ Walidacja POWIODŁA SIĘ");
    }
    
    partial void OnValidationFailed(string error)
    {
        Console.WriteLine($"✗ Błąd walidacji: {error}");
    }
}

// Test
var form = new Form();
form.AddField("Name", "Jan");
form.AddField("Email", "jan@example.com");
Console.WriteLine($"Valid: {form.Validate()}");

Console.WriteLine("\n--- Próba z pustym polem ---\n");
var form2 = new Form();
form2.AddField("Name", "");
Console.WriteLine($"Valid: {form2.Validate()}");
```

### Wyjaśnienie

- **3 hooki** dla różnych faz: start → sukces/błąd
- Implementacja w Części 2 decyduje o **logowaniu/monitoringu**
- Łatwo włączyć/wyłączyć logowanie bez zmiany logiki walidacji
- Przydatne dla: testowania, debugowania, instrumentacji kodu generowanego

---

## 🧪 Testy

```csharp
[Fact]
public void PartialMethod_Logger_Works()
{
    var logger = new Logger();
    logger.Info("Test");
    Assert.Single(logger.GetLogs());
}

[Fact]
public void PartialMethod_UserManager_Works()
{
    var manager = new UserManager();
    manager.RegisterUser("test");
    Assert.Equal(1, manager.UserCount);
}

[Fact]
public void PartialMethod_Form_Validation()
{
    var valid = new Form();
    valid.AddField("Name", "Jan");
    Assert.True(valid.Validate());

    var invalid = new Form();
    invalid.AddField("Name", "");
    Assert.False(invalid.Validate());
}
```

---

## 📚 Zasoby Edukacyjne

**Pojęcia kluczowe**:
- Metody częściowe mają deklarację i (w wersji klasycznej) opcjonalną implementację
- Jeśli implementacja nie istnieje, wywołanie jest **usuwane** przez kompilator
- Klasyczne metody częściowe zwracają `void` i są `private`; od C# 9 **rozszerzone** metody częściowe mogą
  mieć modyfikator dostępu i zwracać wartość – wtedy implementacja jest obowiązkowa
- Kompilator łączy deklarację i implementację w jedną metodę klasy

**Microsoft Docs**:
- https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
