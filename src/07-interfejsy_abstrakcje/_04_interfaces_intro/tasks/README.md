# 📝 Zadania do Topic 4: Interfejsy - Wstęp

## Zadanie 1: Implement Logging Interface

### Problem
Masz interfejs `ILogger`:
```csharp
public interface ILogger
{
    void Log(string message);
    void Error(string error);
}
```

Chcesz obsługiwać różne rodzaje logowania:
- **ConsoleLogger** - wyświetl w konsoli
- **FileLogger** - zapisz do pliku
- **DatabaseLogger** - zapisz do bazy danych (mock)

### Wymagania
1. **Implementuj DatabaseLogger : ILogger**
   - Przechowuj logi w List<string> (simulacja DB)
   - `Log()`: Dodaj "[LOG] {message}" do listy
   - `Error()`: Dodaj "[ERROR] {error}" do listy
   - `GetAllLogs()`: Zwróć wszystkie logi

2. **Utwórz Logger Factory**:
   ```csharp
   public class LoggerFactory
   {
       public static ILogger CreateLogger(string type)
       {
           return type switch
           {
               "console" => new ConsoleLogger(),
               "database" => new DatabaseLogger(),
               _ => throw new ArgumentException("Unknown type")
           };
       }
   }
   ```

3. **Użyj DI w konsumujących klasach**

### Starter Code
```csharp
public class DatabaseLogger : ILogger
{
    private List<string> _logs = new();
    
    public void Log(string message)
    {
        // TODO: Dodaj log
    }
    
    public void Error(string error)
    {
        // TODO: Dodaj error
    }
    
    public List<string> GetAllLogs() => _logs;
}
```

### Oczekiwany Rezultat
```csharp
var dbLogger = LoggerFactory.CreateLogger("database");
dbLogger.Log("User logged in");
dbLogger.Error("Invalid password");

var all = ((DatabaseLogger)dbLogger).GetAllLogs();
Console.WriteLine($"Total logs: {all.Count}");  // 2
```

---

## Zadanie 2: Repository Pattern with Multiple Backends

### Problem
Masz `IRepository<T>` interfejs:
```csharp
public interface IRepository<T>
{
    void Add(T item);
    void Remove(T item);
    List<T> GetAll();
    T? GetById(int id);
}
```

### Wymagania
1. **Implementuj FileRepository<T>**
   - Przechowuj dane w List<T> (simulacja pliku)
   - Implementuj wszystkie metody

2. **Implementuj InMemoryRepository<T>**
   - Szybka pamięć (bez IO)
   - Implementuj wszystkie metody

3. **Użyj w DataService**:
   ```csharp
   public class UserService
   {
       private readonly IRepository<User> _repo;
       
       public UserService(IRepository<User> repo)
       {
           _repo = repo;  // ← Dependency Injection
       }
       
       public void RegisterUser(User user) => _repo.Add(user);
       public List<User> GetAllUsers() => _repo.GetAll();
   }
   ```

4. **Swap implementations bez zmiany kodu UserService**:
   ```csharp
   var service1 = new UserService(new FileRepository<User>());
   var service2 = new UserService(new InMemoryRepository<User>());
   // Obie działają identycznie!
   ```

### Starter Code
```csharp
public class InMemoryRepository<T> : IRepository<T>
{
    private List<T> _items = new();
    private int _nextId = 1;
    
    public void Add(T item)
    {
        // TODO: Dodaj item
    }
    
    public void Remove(T item)
    {
        // TODO: Usuń item
    }
    
    // ... getters
}

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
}
```

### Oczekiwany Rezultat
```csharp
var repo = new InMemoryRepository<User>();
var service = new UserService(repo);

service.RegisterUser(new User { Email = "alice@example.com" });
service.RegisterUser(new User { Email = "bob@example.com" });

var users = service.GetAllUsers();
Assert.Equal(2, users.Count);
```

---

## Zadanie 3 (Advanced): Multi-Interface Service

### Problem
Masz dwa interfejsy:
- `IDataProvider<T>` - dostęp do danych
- `IDataValidator<T>` - walidacja danych

### Wymagania
1. Implementuj klasę obsługującą oba interfejsy
2. W konstruktorze zaakceptuj dwa interfejsy:
   ```csharp
   public class SmartDataService<T>
   {
       public SmartDataService(IDataProvider<T> provider, IDataValidator<T> validator)
       {
           // Przechowaj
       }
   }
   ```

3. Dodaj metodę: `public List<T> GetValidatedData()`
   - Pobierz z providera
   - Waliduj każdy element
   - Zwróć tylko poprawne

---

## Rozwiązanie: Zadanie 1

```csharp
public class DatabaseLogger : ILogger
{
    private List<string> _logs = new();
    
    public void Log(string message)
    {
        _logs.Add($"[LOG] {DateTime.Now:HH:mm:ss} - {message}");
    }
    
    public void Error(string error)
    {
        _logs.Add($"[ERROR] {DateTime.Now:HH:mm:ss} - {error}");
    }
    
    public List<string> GetAllLogs() => _logs;
}

public class LoggerFactory
{
    public static ILogger CreateLogger(string type)
    {
        return type.ToLower() switch
        {
            "console" => new ConsoleLogger(),
            "file" => new FileLogger(),
            "database" => new DatabaseLogger(),
            _ => throw new ArgumentException($"Unknown logger: {type}")
        };
    }
}
```

---

## Rozwiązanie: Zadanie 2

```csharp
public class InMemoryRepository<T> : IRepository<T>
{
    private List<T> _items = new();
    
    public void Add(T item) => _items.Add(item);
    
    public void Remove(T item) => _items.Remove(item);
    
    public List<T> GetAll() => new List<T>(_items);  // Kopia
    
    public T? GetById(int id)
    {
        if (id >= 0 && id < _items.Count)
            return _items[id];
        return default;
    }
}

public class UserService
{
    private readonly IRepository<User> _repo;
    
    public UserService(IRepository<User> repo) => _repo = repo;
    
    public void RegisterUser(User user) => _repo.Add(user);
    
    public List<User> GetAllUsers() => _repo.GetAll();
}

// Użycie
var repo = new InMemoryRepository<User>();
var service = new UserService(repo);
service.RegisterUser(new User { Email = "test@example.com" });
```

---

## Kluczowe Koncepty

### Dependency Injection (DI)
```csharp
// ❌ Tight coupling - zła praktyka
public class UserService
{
    private ILogger _logger = new ConsoleLogger();  // Hardcoded
}

// ✅ Loose coupling - dobra praktyka
public class UserService
{
    private ILogger _logger;
    
    public UserService(ILogger logger)  // ← Dependency injection
    {
        _logger = logger;
    }
}
```

### Multiple Implementations
```csharp
// Jeden interfejs, wiele implementacji
IRepository<User> repo1 = new FileRepository<User>();
IRepository<User> repo2 = new InMemoryRepository<User>();
IRepository<User> repo3 = new DatabaseRepository<User>();

// Kod klienta nie zmienia się!
var service = new UserService(repo1);
var service2 = new UserService(repo2);
```

### Factory Pattern
```csharp
// Zamiast tworzenia bezpośrednio:
var logger = new ConsoleLogger();  // ← Klient wie o konkretnym typie

// Użyj fabryki:
var logger = LoggerFactory.CreateLogger("console");  // ← Abstrakcja
```

---

## Testy (XUnit)

```csharp
[Fact]
public void DatabaseLoggerPersistsLogs()
{
    var logger = new DatabaseLogger();
    logger.Log("Test message");
    logger.Error("Test error");
    
    var logs = logger.GetAllLogs();
    Assert.Equal(2, logs.Count);
    Assert.Contains("[LOG]", logs[0]);
    Assert.Contains("[ERROR]", logs[1]);
}

[Fact]
public void DifferentRepositoriesWorkIdentically()
{
    var fileRepo = new FileRepository<User>();
    var memoryRepo = new InMemoryRepository<User>();
    
    var user = new User { Email = "test@test.com" };
    
    fileRepo.Add(user);
    memoryRepo.Add(user);
    
    Assert.Equal(1, fileRepo.GetAll().Count);
    Assert.Equal(1, memoryRepo.GetAll().Count);
}
```

---

## Podsumowanie Nauki

Po ukończeniu tych zadań powinieneś:
- ✅ Rozumieć interfejsy jako kontrakty
- ✅ Implementować DI (Dependency Injection)
- ✅ Tworzyć wiele implementacji jednego interfejsu
- ✅ Używać Factory Pattern
- ✅ Zwracać loose coupling zamiast tight coupling
- ✅ Pisać kod łatwy do testowania

**Główna Idea**: Interface pozwala "odłączyć" kod klienta od konkretnych implementacji!

---

*Czas do ukończenia: 30-45 minut*  
*Poziom: Intermediate*  
*Koncepty: Interfaces, DI, Factory Pattern, Loose Coupling*
