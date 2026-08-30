# Zadania: Konstruktory w Klasach Pochodnych

## Zadanie 1: Base Constructor Chaining

Stwórz:
- `Person(string name, int age)`
- `Employee(string name, int age, string position) : Person`
- `Manager(string name, int age, string position, int subordinates) : Employee`

## Zadanie 2: Brak Domyślnego Konstruktora

Stwórz klasę `Base` bez konstruktora bezparametrowego, następnie klasa pochodna MUSI używać `base()`.

## Zadanie 3: Multiple Constructors

Klasa pochodna powinna mieć 2+ konstruktory, każdy wywoływujący inny konstruktor bazy.

---

## Rozwiązania

### Zadanie 1
```csharp
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

public class Employee : Person
{
    public string Position { get; set; } = "";
    
    public Employee(string name, int age, string position) 
        : base(name, age)
    {
        Position = position;
    }
}

public class Manager : Employee
{
    public int Subordinates { get; set; }
    
    public Manager(string name, int age, string position, int subordinates)
        : base(name, age, position)
    {
        Subordinates = subordinates;
    }
}
```
