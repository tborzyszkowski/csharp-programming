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

Przy `new Dog("Rex", "Labrador")` konstruktory są *wywoływane* od klasy pochodnej do bazowej, ale ich *ciała* wykonują się od bazowej do pochodnej:

```
new Dog("Rex", "Labrador")
    ↓
Dog constructor: wywołanie base(name)
    ↓
Animal constructor - ciało
    ↓
Powrót do Dog constructor - pozostały kod
```

W C# pełna kolejność dla jednego obiektu wygląda tak:

1. Inicjalizatory pól i właściwości **klasy pochodnej** (od najbardziej pochodnej)
2. Inicjalizatory pól i właściwości **klasy bazowej**
3. Ciało konstruktora **klasy bazowej**
4. Ciało konstruktora **klasy pochodnej**

```csharp
public class BaseInit
{
    private readonly string _baseField = Log("Base field initializer");
    public BaseInit() => Log("Base constructor");
}

public class DerivedInit : BaseInit
{
    private readonly string _derivedField = Log("Derived field initializer");
    public DerivedInit() => Log("Derived constructor");
}

// new DerivedInit() zapisuje kolejno:
// Derived field initializer
// Base field initializer
// Base constructor
// Derived constructor
```

> **C# vs Java:** w Javie inicjalizatory pól klasy pochodnej wykonują się dopiero *po* konstruktorze bazowym. W C# są wykonywane przed nim, dlatego pola pochodne z inicjalizatorem są już ustawione, gdy działa kod klasy bazowej. Pola przypisywane dopiero w ciele konstruktora pochodnej jeszcze takie nie są.

### ⚠️ Wywołanie metody wirtualnej w konstruktorze

```csharp
public class Base
{
    public Base() => Describe();         // wywołanie wirtualne w konstruktorze
    public virtual void Describe() { }
}

public class Derived : Base
{
    private string? _name;

    public Derived(string name) => _name = name;

    public override void Describe()
        => Console.WriteLine(_name ?? "(jeszcze null)");   // konstruktor Derived jeszcze się nie wykonał
}
```

Gdy konstruktor bazowy wywołuje metodę wirtualną, uruchomi się wersja z klasy pochodnej, choć jej konstruktor jeszcze nie zakończył pracy. Obiekt może więc być w częściowo zainicjalizowanym stanie. Unikaj tego wzorca w konstruktorach.

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
        // base(...) i this(...) są dozwolone TYLKO w inicjalizatorze konstruktora,
        // nie w zwykłych metodach.
    }
}

public class Puppy : Dog
{
    public Puppy() : this(0) { }               // this(...) - inny konstruktor tej samej klasy

    public Puppy(int age) : base(age) { }      // base(...) - konstruktor klasy bazowej
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
    public Derived() { }                  // niejawnie : base()
    public Derived(int x) : base(x) { }   // jawny wybór przeciążenia
}

new Derived();    // Base()
new Derived(5);   // Base(5)
```

Jawny `base(...)` jest konieczny tylko wtedy, gdy chcesz wybrać inne przeciążenie niż bezparametrowe albo gdy klasa bazowa nie ma dostępnego konstruktora bezparametrowego.

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

Konstruktor klasy pochodnej powinien przekazać do `base(...)` wszystkie dane niezbędne do utworzenia poprawnego obiektu bazowego. Dzięki temu po zakończeniu konstruktora bazowego obiekt spełnia swój kontrakt, niezależnie od tego, która klasa pochodna jest tworzona. Klasa pochodna może korzystać z chronionych członków bazowych, ale nie powinna omijać reguł walidacji z konstruktora bazowego. Konstruktory nie są dziedziczone, więc każda klasa pochodna musi zdefiniować własne.

## Porównanie C# i Java

Składnia jest podobna (`: base(...)` w C#, `super(...)` w Javie), ale kolejność inicjalizatorów pól różni się - patrz uwaga w sekcji „Kolejność Inicjalizacji”.

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

## 🧭 Nawigacja

[← Modyfikatory dostępu](../_02_access_modifiers/README.md) · [Moduł 05](../README.md) · [Override vs New →](../_04_method_overriding/README.md)
