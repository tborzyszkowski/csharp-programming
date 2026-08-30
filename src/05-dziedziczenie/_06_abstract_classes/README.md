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
| MUSI być implementowane w pochodnych | Może być zaimplementowane |
| Klasa pochodna MUSI override | Klasa pochodna MOŻE override |
| Nie może mieć ciała (zwykle) | Zwykle ma ciało |
| Wymusza interface | Udostępnia default implementację |

```csharp
public abstract class Shape
{
    // Abstract - MUSI override
    public abstract double CalculateArea();
    
    // Virtual - może override
    public virtual string GetType() => "Shape";
}

public class Circle : Shape
{
    // ❌ BŁĄD jeśli nie implementujesz CalculateArea()
    public override double CalculateArea() => 3.14;
    
    // ✅ OK - opcjonalne
    // public override string GetType() { }
}
```

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

| Abstract Class | Interface |
|---|---|
| Zawiera implementację | Tylko sygnatury |
| Może mieć fields | Nie może (C# 8+ może) |
| Może mieć constructors | Nie (z wyjątkiem C# 11+) |
| Może być private/protected | Zwykle public |
| Jedna bazowa klasa | Wiele interfejsów |

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

✅ **Definiuj interface w abstract class**:
```csharp
public abstract class DataProvider
{
    public abstract IEnumerable<T> GetAll<T>();
    public abstract T GetById<T>(int id);
    public abstract void Save<T>(T item);
}
```

✅ **Udostępniaj common implementation w virtual**:
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

❌ **Unikaj zbyt abstrakcyjnych abstrakcji**:
```csharp
// ZŁO - zbyt ogólne
public abstract class AbstractBase { }

// DOBRZE - konkretne
public abstract class Animal { public abstract void Speak(); }
```

## Hierarchia Abstrakcji

```
        Animal (abstract)
        /              \
      Dog            Cat
    (concrete)     (concrete)

vs

        Mammal (abstract)
        /              \
      Dog            Cat
    (abstract)     (abstract)
    /              /
  Puppy        Kitten
(concrete)    (concrete)
```

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
