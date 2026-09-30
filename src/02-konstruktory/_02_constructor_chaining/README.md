# Łańcuchowe Wywołanie Konstruktorów

## 🎯 Cel rozdziału

Zrozumienie, jak unikać duplikacji kodu w konstruktorach poprzez używanie słowa kluczowego `this()` do łańcuchowania konstruktorów.

## 📚 Spis treści

1. [Problem: Duplikacja kodu](#problem-duplikacja-kodu)
2. [Rozwiązanie: this()](#rozwiązanie-this)
3. [Kolejność wykonania](#kolejność-wykonania)
4. [Best Practices](#best-practices)
5. [Zaawansowane wzorce](#zaawansowane-wzorce)
6. [Diagrama koncepcji](#diagrama-koncepcji)

---

## Problem: Duplikacja kodu

Bez łańcuchowania konstruktorów, kod się powtarza:

```csharp
public class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
    
    // Konstruktor 1
    public Employee(string name)
    {
        Name = name;
        Department = "HR";
        Salary = 3000;
    }
    
    // Konstruktor 2 - DUPLIKACJA
    public Employee(string name, string department)
    {
        Name = name;              // Powtórzenie
        Department = department;
        Salary = 3000;            // Powtórzenie
    }
    
    // Konstruktor 3 - JESZCZE WIĘCEJ DUPLIKACJI
    public Employee(string name, string department, decimal salary)
    {
        Name = name;              // Powtórzenie
        Department = department;  // Powtórzenie
        Salary = salary;
    }
}
```

**Problemy:**
- ❌ Kod się powtarza
- ❌ Jeśli zmienisz inicjalizację, musisz zmienić we wszystkich konstruktorach
- ❌ Walidacja jest rozproszona

---

## Rozwiązanie: this()

Używając `this()`, jeden konstruktor może wywoływać inny konstruktor tej samej klasy:

```csharp
public class Employee
{
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
    
    // Konstruktor 1: najmniej parametrów
    public Employee(string name) 
        : this(name, "HR")  // Łańcuch do konstruktora 2
    {
    }
    
    // Konstruktor 2: średniozaawansowany
    public Employee(string name, string department)
        : this(name, department, 3000)  // Łańcuch do konstruktora 3
    {
    }
    
    // Konstruktor 3: pełny konstruktor (główny)
    public Employee(string name, string department, decimal salary)
    {
        // Tylko tutaj logika inicjalizacji
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        
        Name = name;
        Department = department;
        Salary = salary;
    }
}
```

**Korzyści:**
- ✅ Brak duplikacji kodu
- ✅ Walidacja w jednym miejscu
- ✅ Łatwo znaleźć główną inicjalizację
- ✅ Zachowanie DRY (Don't Repeat Yourself)

---

## Kolejność Wykonania

Musisz rozróżnić dwie rzeczy: **kolejność delegowania** (który konstruktor woła który) i **kolejność wykonywania ciał** konstruktorów.

```csharp
var emp = new Employee("Jan");
```

1. **Delegowanie** – od konstruktora najmniej parametrowego do głównego:
   `Employee(name)` → `Employee(name, "HR")` → `Employee(name, "HR", 3000)`
2. **Wykonywanie ciał** – w kolejności **odwrotnej**: najpierw wykonuje się ciało **konstruktora głównego**
   (ostatniego w łańcuchu), a potem, cofając się, ciała konstruktorów, które do niego delegowały.

```
new Employee("Jan")
        ↓ delegowanie (this(...))
    Employee("Jan", "HR")
        ↓
    Employee("Jan", "HR", 3000)   ← główny konstruktor

Wykonanie ciał:
    1. ciało Employee(name, dept, salary)   ← NAJPIERW (walidacja i przypisania)
    2. ciało Employee(name, dept)
    3. ciało Employee(name)                 ← NA KOŃCU
```

Dlatego w konstruktorach „pośrednich” można już bezpiecznie korzystać z w pełni zainicjowanego obiektu, a **logikę i walidację
umieszczamy w konstruktorze głównym**. Demonstruje to klasa `ChainOrderDemo` w `code/Program.cs`:

```
field initializer
body of ctor(a, b) - main
body of ctor(a)
body of ctor()
```

> **Inicjalizatory pól** (`private int x = 5;`) wykonują się tylko **raz** – w tym konstruktorze, który *nie* deleguje do `this(...)`
> (tu: w głównym), przed jego ciałem. Więcej w temacie 4 (kolejność inicjalizacji).

---

## Praktyczne Przykłady

### Przykład 1: Klasa Point

```csharp
public class Point
{
    public double X { get; }
    public double Y { get; }
    
    // Konstruktor dla punktu na osi X
    public Point(double x) : this(x, 0) { }
    
    // Konstruktor dla obu współrzędnych
    public Point(double x, double y)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
            throw new ArgumentException("Coordinates cannot be NaN");
        
        X = x;
        Y = y;
    }
    
    public double Distance() => Math.Sqrt(X * X + Y * Y);
    
    public override string ToString() => $"({X}, {Y})";
}

// Użycie
var p1 = new Point(3);        // (3, 0)
var p2 = new Point(3, 4);     // (3, 4)
```

### Przykład 2: Klasa Configuration

```csharp
public class DatabaseConfig
{
    public string Server { get; }
    public string Database { get; }
    public int Port { get; }
    public string Username { get; }
    public string Password { get; }
    
    // Domyślna konfiguracja
    public DatabaseConfig() 
        : this("localhost", "mydb") { }
    
    // Z serwerem i bazą
    public DatabaseConfig(string server, string database)
        : this(server, database, 5432, "admin", "password") { }
    
    // Pełna konfiguracja
    public DatabaseConfig(string server, string database, int port, 
                         string username, string password)
    {
        if (port <= 0 || port > 65535)
            throw new ArgumentException("Invalid port");
        
        Server = server;
        Database = database;
        Port = port;
        Username = username;
        Password = password;
    }
    
    public string GetConnectionString() 
        => $"Server={Server};Database={Database};Port={Port};User={Username}";
}
```

> ⚠️ **Bezpieczeństwo:** domyślne hasło `"password"` wpisane w kodzie to tylko uproszczenie dydaktyczne. Dane uwierzytelniające
> nigdy nie powinny być na stałe zapisane w kodzie źródłowym ani w repozytorium – czyta się je ze środowiska
> (zmienne środowiskowe, *User Secrets*, Azure Key Vault). Domyślne konto `admin` z oczywistym hasłem to klasyczna luka
> (OWASP: *Security Misconfiguration*).

// Użycie
var config1 = new DatabaseConfig();                          // Domyślna
var config2 = new DatabaseConfig("db.example.com", "shop");  // Z serwerem
var config3 = new DatabaseConfig("localhost", "test", 3306, "root", "secret");  // Pełna
```

---

## Best Practices

### ✅ Dobre praktyki

1. **Łańcuch od najmniej do najbardziej parametrowego**:
   ```csharp
   public Employee(string name) 
       : this(name, "HR") { }
   
   public Employee(string name, string dept) 
       : this(name, dept, 3000) { }
   
   public Employee(string name, string dept, decimal salary)
   {
       // Główna logika
   }
   ```

2. **Umieść logikę walidacji w głównym konstruktorze**:
   ```csharp
   public class Product
   {
       public string Name { get; }
       public decimal Price { get; }
       
       public Product(string name) 
           : this(name, 0) { }
       
       public Product(string name, decimal price)
       {
           // TUTAJ walidacja, nie w poprzednim
           if (string.IsNullOrEmpty(name))
               throw new ArgumentException();
           
           if (price < 0)
               throw new ArgumentException();
           
           Name = name;
           Price = price;
       }
   }
   ```

3. **Używaj wartości domyślnych w `this()`**:
   ```csharp
   public class Settings
   {
       public string Theme { get; }
       public bool DarkMode { get; }
       public int FontSize { get; }
       
       public Settings() 
           : this("Default") { }
       
       public Settings(string theme) 
           : this(theme, false) { }
       
       public Settings(string theme, bool darkMode)
           : this(theme, darkMode, 12) { }
       
       public Settings(string theme, bool darkMode, int fontSize)
       {
           Theme = theme;
           DarkMode = darkMode;
           FontSize = fontSize;
       }
   }
   ```

### ❌ Anty-wzorce

```csharp
// ❌ Łańcuch w złą stronę: konstruktor pełny deleguje do uboższego
public class Bad
{
    public string Name { get; }
    public string Dept { get; }
    
    public Bad(string name, string dept)
        : this(name) { }      // ZŁE: "pełny" konstruktor gubi argument dept, a logika ląduje w konstruktorze uboższym
    
    public Bad(string name)
    {
        Name = name;
        Dept = "HR";
    }
}

// ❌ Logika w wielu konstruktorach zamiast w jednym głównym
public class Bad2
{
    public Bad2(string name) 
        : this(name, "HR") { }
    
    public Bad2(string name, string dept)
    {
        // Walidacja tylko tutaj... ale jeśli ktoś doda kolejny konstruktor bez this(...), znowu będzie duplikacja
        if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");
        Name = name;
    }
    
    public string Name { get; set; }
}
```

**Ograniczenia `this(...)`:**
- Łańcuch nie może być cykliczny (A→B→A) – błąd kompilacji CS0768
- W argumentach `this(...)` nie można odwoływać się do składowych instancji (`this.x`), tylko do parametrów i składowych statycznych
- Konstruktor może mieć **jedno** wywołanie inicjalizujące: albo `this(...)`, albo `base(...)`

### Alternatywa: parametry opcjonalne

Często pojedynczy konstruktor z wartościami domyślnymi zastępuje cały łańcuch:

```csharp
public Employee(string name, string department = "HR", decimal salary = 3000)
{
    // jedna logika, jedna walidacja
}

var emp = new Employee("Jan", salary: 4000);   // argumenty nazwane
```

Wybieraj łańcuch konstruktorów, gdy domyślne wartości są **obliczane** (np. `DateTime.Now`) albo gdy konstruktory różnią się
semantyką, a nie tylko liczbą parametrów. (Uwaga: wartości domyślne parametrów są wpisywane w kod *wywołujący* w chwili
kompilacji – zmiana w bibliotece wymaga rekompilacji klientów.)

---

## Zaawansowane Wzorce

### Fluent API + łańcuch konstruktorów

```csharp
public class HttpRequest
{
    public string Url { get; }
    public string Method { get; }
    public Dictionary<string, string> Headers { get; }
    public string? Body { get; }
    
    public HttpRequest(string url) 
        : this(url, "GET") { }
    
    public HttpRequest(string url, string method)
        : this(url, method, new Dictionary<string, string>()) { }
    
    public HttpRequest(string url, string method, Dictionary<string, string> headers)
        : this(url, method, headers, null) { }
    
    public HttpRequest(string url, string method, Dictionary<string, string> headers, string? body)
    {
        Url = url;
        Method = method;
        Headers = headers;
        Body = body;
    }
    
    public HttpRequest WithHeader(string key, string value)
    {
        Headers[key] = value;
        return this;
    }
    
    public override string ToString() => $"{Method} {Url}";
}

// Użycie
var request = new HttpRequest("https://api.example.com/users")
    .WithHeader("Authorization", "Bearer token")
    .WithHeader("Content-Type", "application/json");
```

---

## Diagrama koncepcji

```mermaid
graph TB
    A["new Employee<br/>name: String"]
    B["Employee(name)<br/>Constructor 1"]
    C["Employee(name, dept)<br/>Constructor 2"]
    D["Employee(name, dept, salary)<br/>Constructor 3<br/>GŁÓWNY"]
    
    A --> B
    B -->|this-name, 'HR'| C
    C -->|this-name, dept, 3000| D
    D --> E["Inicjalizacja pól<br/>Walidacja"]
    E --> F["Zwrot obiektu"]
    
    style A fill:#e3f2fd
    style B fill:#bbdefb
    style C fill:#90caf9
    style D fill:#64b5f6
    style E fill:#42a5f5
    style F fill:#2196f3
```

---

## Podsumowanie

| Aspekt | Opis |
|--------|------|
| **Cel** | Unikanie duplikacji w konstruktorach |
| **Syntaktyka** | `public Constructor(...) : this(...) { }` |
| **Kolejność** | Delegowanie: najmniej → najbardziej parametrowy; wykonanie ciał: odwrotnie (główny konstruktor pierwszy) |
| **Walidacja** | W głównym konstruktorze |
| **Korzyści** | DRY, łatwość utrzymania, czystość kodu |

---

## 🚀 Jak pracować z tym tematem

```bash
cd code/

dotnet run
dotnet test
dotnet build
```

---

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
