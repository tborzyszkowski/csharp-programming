# Zadania - Konstruktory: o co chodzi?

## 📝 Zadanie 1: Klasa Rectangle (Prostokąt)

### Opis

Stwórz klasę `Rectangle` reprezentującą prostokąt. Klasa powinna:
- Mieć dwie właściwości: `Width` (szerokość) i `Height` (wysokość)
- Mieć konstruktor domyślny inicjujący kwadrat 1x1
- Mieć konstruktor parametrowy z walidacją
- Mieć metodę `GetArea()` zwracającą pole
- Mieć metodę `GetPerimeter()` zwracającą obwód

### Wymagania

- Waliduj, że wymiary są dodatnie
- Używaj `readonly` dla pól
- Dodaj XML dokumentację

### Przykład użycia

```csharp
var rect = new Rectangle(5, 3);
Console.WriteLine($"Area: {rect.GetArea()}");      // 15
Console.WriteLine($"Perimeter: {rect.GetPerimeter()}");  // 16

var square = new Rectangle();
Console.WriteLine($"Square: {square.GetArea()}");  // 1
```

### ✅ Rozwiązanie

```csharp
public class Rectangle
{
    private readonly double width;
    private readonly double height;
    
    /// <summary>
    /// Konstruktor domyślny - tworzy kwadrat 1x1
    /// </summary>
    public Rectangle() : this(1, 1) { }
    
    /// <summary>
    /// Konstruktor parametrowy
    /// </summary>
    /// <param name="width">Szerokość (musi być > 0)</param>
    /// <param name="height">Wysokość (musi być > 0)</param>
    public Rectangle(double width, double height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Wymiary muszą być dodatnie");
        
        this.width = width;
        this.height = height;
    }
    
    public double Width => width;
    public double Height => height;
    
    public double GetArea() => width * height;
    public double GetPerimeter() => 2 * (width + height);
    
    public override string ToString() => $"Rectangle({width}x{height})";
}
```

---

## 📝 Zadanie 2: Logger z Singleton Pattern

### Opis

Stwórz klasę `FileLogger` implementującą Singleton pattern:
- Powinna być tylko jedna instancja na aplikację
- Powinna trzymać ścieżkę do pliku logu (statyczna)
- Powinna mieć metodę `Log(message)` pisującą do konsoli
- Powinna posiadać statyczny konstruktor inicjujący plik logu

### Wymagania

- Prywatny konstruktor
- Statyczna metoda `GetInstance()`
- Statyczne pole dla instancji
- Statyczny konstruktor dla inicjalizacji

### Przykład użycia

```csharp
var logger1 = FileLogger.GetInstance();
var logger2 = FileLogger.GetInstance();

logger1.Log("Message from logger1");
logger2.Log("Message from logger2");

Console.WriteLine(ReferenceEquals(logger1, logger2));  // true
```

### ✅ Rozwiązanie

```csharp
public class FileLogger
{
    private static FileLogger? instance = null;
    private static readonly string logPath;
    
    static FileLogger()
    {
        logPath = "application.log";
        Console.WriteLine($"FileLogger initialized, path: {logPath}");
    }
    
    private FileLogger()
    {
        Console.WriteLine("FileLogger instance created");
    }
    
    public static FileLogger GetInstance()
    {
        instance ??= new FileLogger();
        return instance;
    }
    
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }
}
```

---

## 📝 Zadanie 3: User Factory

### Opis

Stwórz klasę `User` z metodami factory do tworzenia standardowych użytkowników:
- `CreateAdmin(name)` - tworzy administratora
- `CreateModerator(name)` - tworzy moderatora
- `CreateGuest()` - tworzy gościa
- Prywatny konstruktor - bezpośrednie tworzenie niemożliwe

### Wymagania

- Właściwości: `Name`, `Role`, `IsActive`
- Walidacja nazwy użytkownika
- Dokumentacja XML dla fabryk

### Przykład użycia

```csharp
var admin = User.CreateAdmin("Maria");
var mod = User.CreateModerator("Jan");
var guest = User.CreateGuest();

Console.WriteLine(admin.Role);   // Admin
Console.WriteLine(mod.Role);     // Moderator
Console.WriteLine(guest.Role);   // Guest
```

### ✅ Rozwiązanie

```csharp
public class User
{
    public string Name { get; }
    public string Role { get; }
    public bool IsActive { get; }
    
    private User(string name, string role)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        
        Name = name;
        Role = role;
        IsActive = true;
    }
    
    /// <summary>
    /// Tworzy administratora
    /// </summary>
    public static User CreateAdmin(string name)
    {
        Console.WriteLine($"Creating admin: {name}");
        return new User(name, "Admin");
    }
    
    /// <summary>
    /// Tworzy moderatora
    /// </summary>
    public static User CreateModerator(string name)
    {
        Console.WriteLine($"Creating moderator: {name}");
        return new User(name, "Moderator");
    }
    
    /// <summary>
    /// Tworzy gościa
    /// </summary>
    public static User CreateGuest()
    {
        Console.WriteLine("Creating guest");
        return new User("Guest", "Guest");
    }
    
    public override string ToString() => $"User[{Name}, Role={Role}]";
}
```

---

## 📚 Testy do implementacji

```csharp
[Fact]
public void Rectangle_DefaultConstructor_CreatesSquare()
{
    var rect = new Rectangle();
    Assert.Equal(1, rect.Width);
    Assert.Equal(1, rect.Height);
    Assert.Equal(1, rect.GetArea());
}

[Fact]
public void FileLogger_Singleton_ReturnsAlwaysSameInstance()
{
    var logger1 = FileLogger.GetInstance();
    var logger2 = FileLogger.GetInstance();
    Assert.True(ReferenceEquals(logger1, logger2));
}

[Fact]
public void User_Factory_CreatesUserWithCorrectRole()
{
    var admin = User.CreateAdmin("Anna");
    Assert.Equal("Admin", admin.Role);
    Assert.True(admin.IsActive);
}
```

---

## 💡 Wnioski

Po wykonaniu tych zadań powinieneś rozumieć:

- ✅ Kiedy użyć konstruktora domyślnego vs parametrowego
- ✅ Jak walidować dane w konstruktorze
- ✅ Singleton pattern i prywatne konstruktory
- ✅ Factory pattern do tworzenia obiektów
- ✅ Statyczne pola i konstruktory statyczne
