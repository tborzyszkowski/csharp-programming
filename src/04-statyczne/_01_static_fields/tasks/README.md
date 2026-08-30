# Zadania: Pola Statyczne

## Zadanie 1: Licznik ID produktów

**Cel**: Zaimplementuj system automatycznego ID dla produktów.

**Wymagania**:
- Klasa `Product` z polem instancji `Name`
- Statyczne pole `NextId` zaczynające się od 1
- Konstruktor przypisuje statyczne `Id` każdemu produktowi i inkrementuje `NextId`
- Testy sprawdzające że każdy produkt ma unikalne ID

**Starter code**:
```csharp
public class Product
{
    public static int NextId = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    
    public Product(string name)
    {
        // TODO: Implementuj
    }
}
```

**Wskazówka**: Użyj `Id = NextId++;` w konstruktorze.

**Testy**:
```csharp
[Fact]
public void Product_AssignsUniqueIds()
{
    Product.NextId = 1;  // Reset
    
    var p1 = new Product("Phone");
    var p2 = new Product("Laptop");
    var p3 = new Product("Monitor");
    
    Assert.Equal(1, p1.Id);
    Assert.Equal(2, p2.Id);
    Assert.Equal(3, p3.Id);
}
```

---

## Zadanie 2: Konfiguracja aplikacji

**Cel**: Stwórz system konfiguracji z polami statycznymi.

**Wymagania**:
- Klasa `Configuration` z polami statycznymi readonly
- Pola: `DatabaseUrl`, `ApiTimeout`, `MaxConnections`, `LogLevel`
- Inicjalizuj sensowne wartości domyślne
- Metoda statyczna `ResetToDefaults()`

**Starter code**:
```csharp
public class Configuration
{
    public static readonly string DatabaseUrl = "localhost";
    public static readonly int ApiTimeout = 30;
    public static readonly int MaxConnections = 100;
    public static readonly string LogLevel = "INFO";
    
    public static void ResetToDefaults()
    {
        // TODO: Jak zresetować readonly pola?
        // (Wskazówka: Cannot modify readonly - to celowe!)
    }
}
```

**Wskazówka**: Readonly pola statyczne nie mogą być zmieniane po inicjalizacji. To jest feature!

**Testy**:
```csharp
[Fact]
public void Configuration_HasSensibleDefaults()
{
    Assert.Equal("localhost", Configuration.DatabaseUrl);
    Assert.Equal(30, Configuration.ApiTimeout);
    Assert.Equal(100, Configuration.MaxConnections);
}

[Fact]
public void Configuration_FieldsAreReadonly()
{
    // Configuration.DatabaseUrl = "newhost";  // Błąd kompilacji!
    Assert.True(true);  // Test że readonly zapobiega modyfikacji
}
```

---

## Zadanie 3: Licznik działań użytkownika

**Cel**: Śledź statystyki działań wszystkich użytkowników.

**Wymagania**:
- Klasa `User` z polem instancji `Name`
- Statyczne pola: `TotalLogins`, `TotalLogouts`, `ActiveUsers`
- Metody instancji: `Login()`, `Logout()`
- Metoda statyczna: `PrintStatistics()`

**Starter code**:
```csharp
public class User
{
    public static int TotalLogins = 0;
    public static int TotalLogouts = 0;
    public static int ActiveUsers = 0;
    
    public string Name { get; set; }
    public bool IsLoggedIn { get; set; }
    
    public User(string name)
    {
        Name = name;
        IsLoggedIn = false;
    }
    
    public void Login()
    {
        if (!IsLoggedIn)
        {
            IsLoggedIn = true;
            TotalLogins++;
            ActiveUsers++;
        }
    }
    
    public void Logout()
    {
        if (IsLoggedIn)
        {
            IsLoggedIn = false;
            TotalLogouts++;
            ActiveUsers--;
        }
    }
    
    public static void PrintStatistics()
    {
        // TODO: Wydrukuj statystyki
    }
}
```

**Testy**:
```csharp
[Fact]
public void User_TrackLoginLogoutStatistics()
{
    User.TotalLogins = 0;
    User.TotalLogouts = 0;
    User.ActiveUsers = 0;
    
    var u1 = new User("Alice");
    var u2 = new User("Bob");
    
    u1.Login();
    u2.Login();
    
    Assert.Equal(2, User.TotalLogins);
    Assert.Equal(0, User.TotalLogouts);
    Assert.Equal(2, User.ActiveUsers);
    
    u1.Logout();
    
    Assert.Equal(2, User.TotalLogins);
    Assert.Equal(1, User.TotalLogouts);
    Assert.Equal(1, User.ActiveUsers);
}
```

---

## Rozwiązania

### Zadanie 1 - Rozwiązanie

```csharp
public class Product
{
    public static int NextId = 1;
    public int Id { get; set; }
    public string Name { get; set; }
    
    public Product(string name)
    {
        Name = name;
        Id = NextId++;  // Przypisz bieżący, potem inkrementuj
    }
}
```

**Wyjaśnienie**: 
- `NextId++` zwraca bieżącą wartość (którą przypisujemy do `Id`), potem inkrementuje `NextId`
- Każda nowa instancja dostaje unikalne ID

---

### Zadanie 2 - Rozwiązanie

```csharp
public class Configuration
{
    public static readonly string DatabaseUrl = "localhost";
    public static readonly int ApiTimeout = 30;
    public static readonly int MaxConnections = 100;
    public static readonly string LogLevel = "INFO";
    
    public static void ResetToDefaults()
    {
        // Readonly pola nie mogą być zmieniane - to feature!
        // Jeśli chcesz zmieniane pole, nie rób go readonly:
        Console.WriteLine("Resetting to defaults (readonly fields cannot be modified)");
    }
}
```

**Wyjaśnienie**:
- Readonly oznacza że wartość nie zmienia się po inicjalizacji
- To zabezpieczenie - config nie powinien być zmieniany w runtime
- Jeśli potrzebujesz zmieniane pola, usuń `readonly`

---

### Zadanie 3 - Rozwiązanie

```csharp
public void Login()
{
    if (!IsLoggedIn)
    {
        IsLoggedIn = true;
        TotalLogins++;
        ActiveUsers++;
    }
}

public void Logout()
{
    if (IsLoggedIn)
    {
        IsLoggedIn = false;
        TotalLogouts++;
        ActiveUsers--;
    }
}

public static void PrintStatistics()
{
    Console.WriteLine($"Total Logins: {TotalLogins}");
    Console.WriteLine($"Total Logouts: {TotalLogouts}");
    Console.WriteLine($"Active Users: {ActiveUsers}");
}
```

**Wyjaśnienie**:
- Statyczne pola są modyfikowane przez metody instancji i statyczne
- `ActiveUsers` rośnie przy `Login()`, maleje przy `Logout()`
- Metoda statyczna `PrintStatistics()` dostęp do statycznych pól

---

## 💡 Key Takeaways

1. **Statyczne pola są dzielane** między wszystkie instancje
2. **Inicjalizuj je na miejscu** lub w konstruktorze statycznym
3. **Używaj `readonly`** aby zapobiec niekontrolowanym zmianom
4. **Unikaj zbyt dużo statycznych pól** - mogą komplikować testowanie

---

## Dodatkowe Wyzwania

- Jak śledził byś statystyki per godzinę?
- Jak automatycznie resetować liczniki o północy?
- Jak bezpiecznie modyfikować pola statyczne w wielowątkowym środowisku?

