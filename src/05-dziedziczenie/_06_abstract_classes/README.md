# Klasy Abstrakcyjne (Abstract Classes)

## 🎯 Cel

Zrozumienie **abstract classes** - klas które definiują interface dla klas pochodnych ale nie mogą być instancjonowane.

## Czym Jest Abstract Class?

```csharp
public abstract class Animal  // Nie można: new Animal()
{
    // Abstract member - MUSI być implementowany w klasach pochodnych
    public abstract void Speak();
    
    // Virtual member - może być implementowany
    public virtual void Eat() => Console.WriteLine("Eating");
    
    // Regular member - zwykła metoda
    public void Sleep() => Console.WriteLine("Sleeping");
}

public class Dog : Animal
{
    // MUSI implementować abstract members!
    public override void Speak() => Console.WriteLine("Woof!");
}

// new Animal();  // ❌ BŁĄD
var dog = new Dog();  // ✅ OK
```

## Abstract vs Virtual

| Abstract | Virtual |
|----------|---------|
| Brak ciała (tylko deklaracja) | Zawiera domyślną implementację |
| Każda konkretna klasa pochodna MUSI zrobić `override` | Klasa pochodna MOŻE zrobić `override` |
| Może być tylko w klasie abstrakcyjnej | Może być w dowolnej niezapieczętowanej klasie |
| Wymusza kontrakt | Udostępnia domyślne zachowanie |

```csharp
public abstract class Shape
{
    // Abstract - MUSI override w konkretnej klasie pochodnej
    public abstract double CalculateArea();
    
    // Virtual - może override
    public virtual string Describe() => "Shape";
}

public class Circle : Shape
{
    // ❌ BŁĄD KOMPILACJI, jeśli konkretna klasa nie implementuje CalculateArea()
    public override double CalculateArea() => Math.PI * 1 * 1;
    
    // ✅ OK - opcjonalne
    // public override string Describe() => "Circle";
}
```

Klasa pochodna od klasy abstrakcyjnej może sama pozostać abstrakcyjna i odłożyć implementację abstrakcyjnych członków do swoich potomków.

## Abstract Members

```csharp
public abstract class Vehicle
{
    // Abstract property - MUSI implementować
    public abstract string Model { get; set; }
    
    // Abstract method - MUSI implementować
    public abstract void Start();
    
    // Virtual property - może implementować
    public virtual int MaxSpeed { get; set; } = 100;
    
    // Regular method - dostępna dla wszystkich
    public void Drive() => Console.WriteLine("Driving");
}

public class Car : Vehicle
{
    // MUSI implementować
    public override string Model { get; set; } = "";
    public override void Start() => Console.WriteLine("Engine started");
}
```

## Praktyczny Przykład: Database

```csharp
public abstract class Database
{
    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void ExecuteQuery(string query);
    
    public virtual void LogConnection()
    {
        Console.WriteLine("Connected to database");
    }
}

public class SqlDatabase : Database
{
    public override void Connect() => Console.WriteLine("SQL: Connecting");
    public override void Disconnect() => Console.WriteLine("SQL: Disconnecting");
    public override void ExecuteQuery(string query) => Console.WriteLine($"SQL: {query}");
}

public class MongoDatabase : Database
{
    public override void Connect() => Console.WriteLine("MongoDB: Connecting");
    public override void Disconnect() => Console.WriteLine("MongoDB: Disconnecting");
    public override void ExecuteQuery(string query) => Console.WriteLine($"Mongo: {query}");
}

// Polimorfizm
Database db = new SqlDatabase();
db.Connect();
db.ExecuteQuery("SELECT * FROM Users");
```

## Abstract vs Interface

| Klasa abstrakcyjna | Interfejs |
|---|---|
| Może zawierać stan (pola instancji) | Nie ma pól instancji (mogą być składowe statyczne) |
| Może mieć konstruktory (wywoływane przez klasy pochodne) | Nie ma konstruktorów instancji |
| Może mieć składowe o dowolnym dostępie (`private`, `protected` itd.) | Składowe są domyślnie publiczne (od C# 8 możliwe też inne modyfikatory dla członków z implementacją) |
| Może mieć metody abstrakcyjne i z implementacją | Od C# 8 może mieć domyślne implementacje, ale służy głównie jako kontrakt |
| Klasa dziedziczy po jednej klasie bazowej | Klasa może implementować wiele interfejsów |

Klasa abstrakcyjna jest właściwa, gdy typy mają wspólny stan lub implementację oraz silną relację bazową. Interfejs opisuje zdolność albo kontrakt, który może zostać dodany do klasy mającej już inną klasę bazową. C# pozwala dziedziczyć tylko po jednej klasie, ale implementować wiele interfejsów.

```csharp
// Abstract class - implementacja + interface
public abstract class Animal
{
    protected string name;  // Field
    
    public Animal(string name) { this.name = name; }  // Constructor
    
    public abstract void Speak();  // Abstract
    public virtual void Sleep() => Console.WriteLine("Zzz");  // Implementacja
}

// Interface - tylko kontrakt
public interface IFlying
{
    void Fly();
    void Land();
}

// Klasa pochodna - może dziedziczyć z abstract i implementować interface
public class Bird : Animal, IFlying
{
    public Bird(string name) : base(name) { }
    
    public override void Speak() => Console.WriteLine("Chirp!");
    public void Fly() => Console.WriteLine("Flying");
    public void Land() => Console.WriteLine("Landing");
}
```

## Best Practices

✅ **Definiuj kontrakt w klasie abstrakcyjnej** (gdy typy mają też wspólny stan lub logikę):
```csharp
public abstract class DataProvider
{
    public abstract IEnumerable<T> GetAll<T>();
    public abstract T? GetById<T>(int id);
    public abstract void Save<T>(T item);
}
```

✅ **Udostępniaj wspólną implementację w metodach bazowych/virtual**:
```csharp
public abstract class Entity
{
    public int Id { get; set; }
    
    public virtual void Validate()
    {
        if (Id <= 0) throw new InvalidOperationException("Invalid ID");
    }
}
```

> Klasa abstrakcyjna nie może być `sealed` ani `static` - te modyfikatory wykluczają się z `abstract`. Konstruktor klasy abstrakcyjnej zwykle jest `protected` (lub `public`, co nie ma znaczenia, bo i tak nie utworzysz instancji bezpośrednio).

❌ **Unikaj zbyt abstrakcyjnych abstrakcji**:
```csharp
// ZŁO - zbyt ogólne
public abstract class AbstractBase { }

// DOBRZE - konkretne
public abstract class Animal { public abstract void Speak(); }
```

Metoda abstrakcyjna wymusza implementację w każdej konkretnej klasie pochodnej. Metoda wirtualna daje domyślne zachowanie, które można zastąpić. Wspólne reguły i walidację warto trzymać w metodach bazowych, a szczegóły zależne od typu pozostawić metodom abstrakcyjnym.

## Hierarchia Abstrakcji

Płaska hierarchia - jedna abstrakcyjna baza:

```
        Animal (abstract)
        /              \
      Dog            Cat
    (concrete)     (concrete)
```

Wielopoziomowa hierarchia - klasy pośrednie mogą pozostać abstrakcyjne:

```
        Animal (abstract)
             |
        Mammal (abstract)
        /              \
      Dog            Cat
    (concrete)     (concrete)
```

Dodawaj poziomy pośrednie tylko wtedy, gdy mają realny wspólny stan lub zachowanie - głębokie hierarchie trudno utrzymać.

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 📚 Referencje

- [Microsoft Learn: Abstract Classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)
- [Abstract keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract)
- [Interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces)

## 🧭 Nawigacja

[← Virtual i polimorfizm](../_05_virtual_polymorphism/README.md) · [Moduł 05](../README.md) · [Klasy sealed →](../_07_sealed_classes/README.md)
