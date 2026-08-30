# Zadania - Klasy Częściowe

## 📝 Zadanie 1: Student z partial classes

Rozbij klasę Student na 3 części:
- Część 1: Właściwości (Name, Id, GPA)
- Część 2: Metody (UpdateGPA, IsExcellent)
- Część 3: Walidacja (IsValid)

---

## 📝 Zadanie 2: Logger system

Stwórz klasę `Logger` rozproszoną na 2 części:
- Część 1: Metody logowania (Log, Error, Warning)
- Część 2: Formatowanie (GetTimestamp, GetLevel)

---

## 📝 Zadanie 3: Configuration builder

Utwórz klasę `AppConfig` (partial) ze:
- Częścią 1: Pola (AppName, Version, Settings)
- Częścią 2: Metody Load/Save
- Częścią 3: Validacja

---

## ✅ Zadanie 1 - Rozwiązanie: Student

### Kod - Część 1 (Właściwości)

```csharp
public partial class Student
{
    private string name;
    private int id;
    private double gpa;
    
    public Student(string name, int id, double gpa = 0.0)
    {
        this.name = name;
        this.id = id;
        this.gpa = gpa;
    }
    
    public string Name => name;
    public int Id => id;
    public double GPA => gpa;
}
```

### Kod - Część 2 (Metody)

```csharp
public partial class Student
{
    public void UpdateGPA(double newGPA)
    {
        if (newGPA >= 0 && newGPA <= 4.0)
            gpa = newGPA;
    }
    
    public bool IsExcellent() => gpa >= 3.5;
    
    public string GetGrade()
    {
        return gpa switch
        {
            >= 3.5 => "A",
            >= 3.0 => "B",
            >= 2.0 => "C",
            _ => "F"
        };
    }
}
```

### Kod - Część 3 (Walidacja)

```csharp
public partial class Student
{
    public bool IsValid() => !string.IsNullOrEmpty(name) && id > 0 && gpa >= 0;
    
    public override string ToString() => $"{name} (ID:{id}, GPA:{gpa:F2}, Grade:{GetGrade()})";
}

// Test
var student = new Student("Anna", 123, 3.7);
Console.WriteLine($"Student: {student}");
Console.WriteLine($"Excellent: {student.IsExcellent()}");
student.UpdateGPA(3.9);
Console.WriteLine($"After update: {student}");
```

### Wyjaśnienie

- **Część 1**: Pola i konstruktor - definicja struktury
- **Część 2**: Metody biznesowe - działania na obiekcie
- **Część 3**: Walidacja i ToString - Ochrona danych i prezentacja
- Są one **logicznie osobne** ale fizycznie (dla kompilatora) **jeden plik**
- Zaleta: Łatwiej organizować dużo kodu

---

## ✅ Zadanie 2 - Rozwiązanie: Logger System

### Kod - Część 1 (Metody Logowania)

```csharp
public partial class Logger
{
    public void Log(string message)
    {
        Console.WriteLine($"[{GetTimestamp()}] [INFO] {message}");
    }
    
    public void Error(string message)
    {
        Console.WriteLine($"[{GetTimestamp()}] [ERROR] {message}");
    }
    
    public void Warning(string message)
    {
        Console.WriteLine($"[{GetTimestamp()}] [WARN] {message}");
    }
}
```

### Kod - Część 2 (Formatowanie)

```csharp
public partial class Logger
{
    private string GetTimestamp() => DateTime.Now.ToString("HH:mm:ss");
    
    private string GetLevel(string level) => level.ToUpper();
}

// Test
var logger = new Logger();
logger.Log("Aplikacja uruchomiona");
logger.Warning("Mało pamięci");
logger.Error("Błąd połączenia");
```

### Wyjaśnienie

- Metody publiczne w **Części 1**
- Metody prywatne/helper w **Części 2**
- Czytelniej rozdzielić interfejs public od implementacji
- Łatwiej testować każdą część osobno

---

## ✅ Zadanie 3 - Rozwiązanie: Configuration Builder

### Kod - Część 1 (Pola i Konstruktor)

```csharp
public partial class AppConfig
{
    private string appName;
    private string version;
    private Dictionary<string, string> settings;
    
    public AppConfig(string name, string version)
    {
        this.appName = name;
        this.version = version;
        this.settings = new Dictionary<string, string>();
    }
    
    public string AppName => appName;
    public string Version => version;
    public Dictionary<string, string> Settings => settings;
}
```

### Kod - Część 2 (Load/Save)

```csharp
public partial class AppConfig
{
    public void AddSetting(string key, string value)
    {
        settings[key] = value;
    }
    
    public string? GetSetting(string key)
    {
        return settings.TryGetValue(key, out var value) ? value : null;
    }
    
    public void ClearSettings()
    {
        settings.Clear();
    }
}
```

### Kod - Część 3 (Validacja)

```csharp
public partial class AppConfig
{
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(appName) &&
               !string.IsNullOrEmpty(version) &&
               settings.Count > 0;
    }
    
    public override string ToString()
    {
        return $"{appName} v{version} ({settings.Count} settings)";
    }
}

// Test
var config = new AppConfig("MyApp", "1.0");
config.AddSetting("Database", "localhost");
config.AddSetting("Port", "5432");
Console.WriteLine($"Config: {config}");
Console.WriteLine($"Valid: {config.IsValid()}");
```

### Wyjaśnienie

- **3 części**: Struktura → Operacje → Walidacja
- Każda część ma **jedną odpowiedzialność** (Single Responsibility Principle)
- Łatwo dodawać nowe metody do każdej części
- Kod jest bardziej **organizowany i czytelny**

---

## 🧪 Testy

```csharp
[Fact]
public void PartialClass_Student_Works()
{
    var student = new Student("Anna", 123, 3.8);
    Assert.Equal("Anna", student.Name);
    Assert.True(student.IsExcellent());
    Assert.Equal("A", student.GetGrade());
}

[Fact]
public void PartialClass_Logger_Works()
{
    var logger = new Logger();
    logger.Log("Test message");  // Powinno się wypisać
}

[Fact]
public void PartialClass_Config_Works()
{
    var config = new AppConfig("TestApp", "1.0");
    config.AddSetting("Key1", "Value1");
    Assert.Equal("Value1", config.GetSetting("Key1"));
    Assert.True(config.IsValid());
}
```

---

## 📚 Zasoby Edukacyjne

**Pojęcia kluczowe**:
- Klasy częściowe umożliwiają rozłożyć definicję klasy na wiele plików
- Każda część musi mieć słowo kluczowe `partial`
- Kompilator łączy wszystkie części w jedną klasę
- Przydatne dla: wielkich klas, generowanego kodu, podziału pracy

**YouTube - Partial Classes in C#**:
- https://www.youtube.com/results?search_query=C%23+partial+classes+tutorial

**Microsoft Docs**:
- https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods
