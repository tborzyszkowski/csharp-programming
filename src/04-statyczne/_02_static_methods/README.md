# Metody Statyczne (Static Methods)

## 🎯 Cel rozdziału

Zrozumienie **metod statycznych** - funkcji należących do klasy, a nie do instancji. Można je wywoływać bez tworzenia obiektu.

## 📚 Spis treści

1. [Co to metoda statyczna?](#co-to-metoda-statyczna)
2. [Metoda statyczna vs Metoda instancji](#metoda-statyczna-vs-metoda-instancji)
3. [Zastosowania](#zastosowania)
4. [Best Practices](#best-practices)

---

## Co to metoda statyczna?

**Metoda statyczna** to funkcja należąca do klasy, którą można wywoływać bez tworzenia instancji:

```csharp
public class Math2D
{
    public static double CircleArea(double radius)
    {
        return 3.14159 * radius * radius;
    }
}

// Nie trzeba tworzyć instancji!
double area = Math2D.CircleArea(5);  // Bezpośredni dostęp do klasy
Console.WriteLine(area);  // 78.54975
```

**Kluczowe cechy**:
- ✅ Należy do **klasy**, nie do instancji
- ✅ Brak dostępu do **pól instancji** (`this` nie istnieje)
- ✅ Może dostępować **statycznych pól**
- ✅ Wywoływana przez **nazwę klasy**
- ✅ Przydatna dla **utility functions** (Math, String, File itp)

---

## Metoda statyczna vs Metoda instancji

| Aspekt | Metoda Statyczna | Metoda Instancji |
|--------|---|---|
| Deklaracja | `public static void Method()` | `public void Method()` |
| Należy do | Klasy | Instancji |
| Dostęp | `ClassName.Method()` | `instance.Method()` |
| Dostęp do `this` | ❌ Nie | ✅ Tak |
| Dostęp do pól instancji | ❌ Nie | ✅ Tak |
| Dostęp do pól statycznych | ✅ Tak | ✅ Tak |

**Przykład porównania**:

```csharp
public class Temperature
{
    public double Celsius { get; set; }
    
    // METODA INSTANCJI - pracuje z polami instancji
    public double GetFahrenheit()
    {
        return (Celsius * 9/5) + 32;
    }
    
    // METODA STATYCZNA - funkcja utility
    public static double CelsiusToFahrenheit(double celsius)
    {
        return (celsius * 9/5) + 32;
    }
}

// Metoda instancji - potrzeba obiektu
var temp = new Temperature { Celsius = 25 };
double f1 = temp.GetFahrenheit();  // Używa pola instancji

// Metoda statyczna - bez obiektu
double f2 = Temperature.CelsiusToFahrenheit(25);  // Funkcja utility
```

---

## Zastosowania

### 1. Utility Functions
```csharp
public class StringHelper
{
    public static bool IsNullOrEmpty(string? str)
    {
        return string.IsNullOrEmpty(str);
    }
    
    public static string Reverse(string str)
    {
        char[] chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

// Użycie
StringHelper.IsNullOrEmpty("test");  // false
StringHelper.Reverse("hello");  // "olleh"
```

### 2. Factory Methods
```csharp
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    private Person(string first, string last)
    {
        FirstName = first;
        LastName = last;
    }
    
    // Factory method
    public static Person CreateFromFullName(string fullName)
    {
        var parts = fullName.Split(' ');
        return new Person(parts[0], parts[1]);
    }
}

// Użycie
var person = Person.CreateFromFullName("John Doe");
```

### 3. Walidacja
```csharp
public class Email
{
    public string Address { get; set; }
    
    public static bool IsValid(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
        
        return email.Contains("@") && email.Contains(".");
    }
}

if (Email.IsValid("user@example.com"))
    Console.WriteLine("Email valid");
```

### 4. Konwersje
```csharp
public class DataConverter
{
    public static int StringToInt(string str)
    {
        return int.TryParse(str, out int result) ? result : 0;
    }
    
    public static double KilometersToMiles(double km)
    {
        return km * 0.621371;
    }
}

// Użycie
int num = DataConverter.StringToInt("42");
double miles = DataConverter.KilometersToMiles(100);
```

---

## Best Practices

### ✅ DO:

- **Używaj dla utility functions**:
```csharp
public static class MathHelper
{
    public static double Square(double x) => x * x;
    public static double Cube(double x) => x * x * x;
}
```

- **Factory methods**:
```csharp
public static User CreateAdmin(string name)
{
    return new User { Name = name, Role = "Admin" };
}
```

- **Gdy nie potrzebujesz stanu instancji**:
```csharp
public static bool IsPrime(int number)
{
    // Nie pracuje z polami instancji
    if (number < 2) return false;
    // ... sprawdzanie
}
```

### ❌ AVOID:

- **Nie mieszaj logiki instancji ze statyczną**:
```csharp
// ❌ ŹLE
public class User
{
    public string Name { get; set; }
    
    public static void PrintUserInfo(User user)
    {
        Console.WriteLine(user.Name);  // Użycie statycznej dla instancji
    }
}

// ✅ DOBRZE
public class User
{
    public string Name { get; set; }
    
    public void PrintInfo()
    {
        Console.WriteLine(Name);  // Metoda instancji
    }
}
```

- **Unikaj zbyt wiele statycznych metod w klasie biznesowej**:
```csharp
// ❌ Zła organizacja
public class Product
{
    public string Name { get; set; }
    
    public static void PrintAllProducts() { }
    public static decimal CalculateTax(decimal price) { }
    public static void DeleteAllProducts() { }
}

// ✅ Lepsze: Utility class
public static class ProductService
{
    public static void PrintAll() { }
    public static void DeleteAll() { }
}

public static class TaxCalculator
{
    public static decimal Calculate(decimal price) { }
}
```

---

## 📚 Referencje

- [Microsoft Learn: Static Members](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-members)
- [C# Static Methods](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members)

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
