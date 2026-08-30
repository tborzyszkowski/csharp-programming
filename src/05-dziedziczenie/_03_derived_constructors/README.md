# Inicjalizacja Klas Pochodnych (Derived Constructors)

## 🎯 Cel

Zrozumienie jak konstruktory pracują w hierarchii klas i jak używać słowa kluczowego `base`.

## Słowo kluczowe `base`

```csharp
public class Animal
{
    public string Name { get; set; } = "";
    
    public Animal(string name)
    {
        Name = name;
        Console.WriteLine("Animal constructor");
    }
}

public class Dog : Animal
{
    public string Breed { get; set; } = "";
    
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
        Console.WriteLine("Dog constructor");
    }
}

// Użycie
var dog = new Dog("Rex", "Labrador");
// Wynik:
// Animal constructor
// Dog constructor
```

## Kolejność Inicjalizacji

```
new Dog("Rex", "Labrador")
    ↓
Dog constructor code: base(name)
    ↓
Animal constructor code
    ↓
Powrót do Dog constructor
    ↓
Dog constructor - pozostały kod
```

## Brak jawnego `base` - Domyślny Konstruktor

```csharp
public class Vehicle
{
    public string Make { get; set; } = "";
    
    public Vehicle() 
    {
        Console.WriteLine("Vehicle default constructor");
    }
}

public class Car : Vehicle
{
    // Niejawnie: : base()
    public Car(string make) 
    {
        Make = make;
        Console.WriteLine("Car constructor");
    }
}

var car = new Car("Toyota");
// Wynik:
// Vehicle default constructor  <- automatycznie
// Car constructor
```

## Jeśli Baza Nie Ma Domyślnego Konstruktora

```csharp
public class Animal
{
    public Animal(string name) { }  // Tylko ten konstruktor
    // Brak parameterless!
}

public class Dog : Animal
{
    public Dog(string name) : base(name)  // MUSI używać base()
    {
    }
    
    // public Dog() { }  // ❌ BŁĄD - gdzie base?
}
```

## `this` vs `base`

```csharp
public class Animal
{
    protected int age;
    
    public Animal(int age)
    {
        this.age = age;  // this = ten sam obiekt
    }
}

public class Dog : Animal
{
    public Dog(int age) : base(age)  // base = klasa bazowa
    {
    }
    
    public void CallBothConstructors()
    {
        // this() - inny konstruktor tej samej klasy
        // base() - konstruktor klasy bazowej (tylko w initializer!)
    }
}
```

## Praktyczny Przykład: Łańcuch Konstruktorów

```csharp
public class Shape
{
    protected string Name { get; set; } = "";
    
    public Shape(string name) => Name = name;
}

public class Polygon : Shape
{
    protected int Sides { get; set; }
    
    public Polygon(string name, int sides) : base(name)
    {
        Sides = sides;
    }
}

public class Triangle : Polygon
{
    public Triangle(string name) : base(name, 3) { }
}

// Użycie
var triangle = new Triangle("MyTriangle");
// Inicjalizacja: Shape → Polygon → Triangle
```

## Konstruktor Bezparametrowy vs Parametrowy

```csharp
public class Base
{
    public Base() => Console.WriteLine("Base()");
    public Base(int x) => Console.WriteLine($"Base({x})");
}

public class Derived : Base
{
    // Wybiera jawnie (musi być jawne!)
    public Derived() : base() { }
    
    public Derived(int x) : base(x) { }
}

new Derived();    // Base()
new Derived(5);   // Base(5)
```

## Best Practices

✅ **Zawsze inicjalizuj klasy bazowe**:
```csharp
public class Employee
{
    protected string name;
    public Employee(string name) { this.name = name; }
}

public class Manager : Employee
{
    public Manager(string name) : base(name) { }  // ✅ OK
}
```

✅ **Przesyłaj dane do base**:
```csharp
public class Person
{
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    
    public Person(string first, string last)
    {
        FirstName = first;
        LastName = last;
    }
}

public class Customer : Person
{
    public string CustomerID { get; set; }
    
    public Customer(string first, string last, string id) 
        : base(first, last)
    {
        CustomerID = id;
    }
}
```

❌ **Nie inicjalizuj ręcznie wartości bazowe**:
```csharp
// ZŁO
public class Dog : Animal
{
    public Dog(string name)
    {
        Name = name;  // ❌ Powinno być w base
    }
}

// DOBRZE
public class Dog : Animal
{
    public Dog(string name) : base(name) { }
}
```

## Porównanie C# i Java

Składnia jest prawie taka sama!

```csharp
// C#
public class Dog : Animal
{
    public Dog(string name) : base(name) { }
}

// Java
public class Dog extends Animal {
    public Dog(String name) {
        super(name);
    }
}
```

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 📚 Referencje

- [Microsoft Learn: Constructors](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constructors)
- [C# base keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/base)
