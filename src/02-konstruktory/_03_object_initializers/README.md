# Inicjalizatory Obiektów

## 🎯 Cel rozdziału

Zrozumienie inicjalizatorów obiektów i kolekcji - składni pozwalającej na eleganckie tworzenie i inicjalizowanie obiektów bez jawnego wywoływania metod setter-ów.

## 📚 Spis treści

1. [Object Initializers](#object-initializers)
2. [Collection Initializers](#collection-initializers)
3. [Nested Initializers](#nested-initializers)
4. [Target-typed expressions](#target-typed-expressions)
5. [Best Practices](#best-practices)

---

## Object Initializers

**Object Initializer** to składnia pozwalająca na inicjalizowanie publicznych właściwości obiektu bezpośrednio po konstruktorze:

```csharp
// Tradycyjnie
var person = new Person("Jan", 30);
person.City = "Warszawa";
person.Email = "jan@example.com";

// Z initializer - czystsze!
var person = new Person("Jan", 30)
{
    City = "Warszawa",
    Email = "jan@example.com"
};
```

### Przykład

```csharp
public class Car
{
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
}

// Stosując initializer
var car = new Car
{
    Brand = "Tesla",
    Model = "Model 3",
    Year = 2024
};
```

### Inicjalizator bez konstruktora z parametrami

```csharp
var car = new Car()  // Konstruktor domyślny
{
    Brand = "BMW",
    Model = "X5",
    Year = 2023
};

// Nawet krótej - () jest opcjonalne
var car = new Car
{
    Brand = "BMW",
    Model = "X5",
    Year = 2023
};
```

---

## Collection Initializers

**Collection Initializer** to inicjalizowanie kolekcji elementów:

```csharp
// Tradycyjnie
var numbers = new List<int>();
numbers.Add(1);
numbers.Add(2);
numbers.Add(3);

// Z initializer - zwięźle!
var numbers = new List<int> { 1, 2, 3 };

// Dictionary
var ages = new Dictionary<string, int>
{
    { "Anna", 30 },
    { "Jan", 25 },
    { "Maria", 28 }
};

// Lub z property initializer (C# 6+)
var ages = new Dictionary<string, int>
{
    ["Anna"] = 30,
    ["Jan"] = 25,
    ["Maria"] = 28
};
```

---

## Nested Initializers

Kombinowanie object i collection initializers:

```csharp
public class Person
{
    public string Name { get; set; }
    public List<string> PhoneNumbers { get; set; } = new();
    public Address Address { get; set; }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

// Zagnieżdżone initializers
var person = new Person
{
    Name = "Anna",
    PhoneNumbers = { "123456789", "987654321" },  // Collection initializer
    Address = new Address  // Nested object initializer
    {
        Street = "Piotrkowska 10",
        City = "Łódź"
    }
};
```

---

## Target-typed Expressions

C# 9+ - możliwość pominięcia typu gdy jest znany z kontekstu:

```csharp
// C# 8 - trzeba podać typ
List<int> numbers = new List<int> { 1, 2, 3 };

// C# 9+ - typ jest znany z kontekstu
List<int> numbers = new() { 1, 2, 3 };

// W parametrach metod
public void PrintNumbers(List<int> numbers) { }

PrintNumbers(new() { 1, 2, 3 });  // Typ znany z sygnatury metody
```

---

## Best Practices

### ✅ Dobre praktyki

1. **Używaj initializers dla czytelności**:
   ```csharp
   // ✅ Dobrze - jasne co się ustawia
   var config = new AppConfig
   {
       Debug = true,
       MaxConnections = 100,
       Timeout = 30
   };
   ```

2. **Łącz konstruktory z initializers**:
   ```csharp
   // ✅ Obowiązkowe w konstruktorze + opcjonalne w initializer
   var user = new User("john@example.com")  // Obowiązkowy email
   {
       FirstName = "John",  // Opcjonalne
       LastName = "Doe"
   };
   ```

3. **Kolekcje w initializer**:
   ```csharp
   var team = new Team
   {
       Name = "Development",
       Members = new List<string> { "Alice", "Bob", "Charlie" }
   };
   ```

---

## Diagrama koncepcji

```mermaid
graph LR
    A["Object Initializer<br/>new Class { prop = value }"]
    B["Collection Initializer<br/>new List<T> { item1, item2 }"]
    C["Nested Initializer<br/>new A { B = new B { } }"]
    D["Target-typed<br/>new() { }"]
    
    A --> A1["Inicjalizuje właściwości<br/>Po konstruktorze"]
    B --> B1["Inicjalizuje elementy<br/>Wywołuje Add"]
    C --> C1["Łączy oba podejścia<br/>Czystszy kod"]
    D --> D1["C# 9+<br/>Typ z kontekstu"]
    
    style A fill:#e3f2fd
    style B fill:#f3e5f5
    style C fill:#e8f5e9
    style D fill:#fff3e0
