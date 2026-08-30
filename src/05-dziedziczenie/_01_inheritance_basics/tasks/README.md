# Zadania: Podstawowe Pojęcia Dziedziczenia

## Zadanie 1: Hierarchia Pojazdów

Stwórz hierarchię klas:
- `Vehicle` (bazowa) - properties: Make, Model, Year
  - metody: Start(), Stop()
- `Car : Vehicle` - właściwe dla samochodu
  - property: NumberOfDoors
  - metoda: OpenTrunk()
- `Motorcycle : Vehicle` - właściwe dla motoru
  - metoda: DoWheelie()

## Zadanie 2: Łańcuch Dziedziczenia

Stwórz łańcuch 3 klas:
```
Shape (bazowa)
  ├── Polygon
  │     └── Triangle
```

Każda klasa powinna mieć:
- Shape: property Name, metoda GetDescription()
- Polygon: property NumberOfSides
- Triangle: override GetDescription() z specjalnym opisem

## Zadanie 3: Rozróżnianie Typów

Dla klasy Dog z podstawowego przykładu, napisz kod sprawdzający:
- czy dog jest Animal
- czy dog jest Cat
- czy dog jest object
- jaki typ ma dog

---

## Rozwiązania

### Zadanie 1
```csharp
public class Vehicle
{
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    public int Year { get; set; }
    
    public void Start() => Console.WriteLine("Engine started");
    public void Stop() => Console.WriteLine("Engine stopped");
}

public class Car : Vehicle
{
    public int NumberOfDoors { get; set; }
    public void OpenTrunk() => Console.WriteLine("Trunk opened");
}

public class Motorcycle : Vehicle
{
    public void DoWheelie() => Console.WriteLine("Doing a wheelie!");
}
```

### Zadanie 2
```csharp
public class Shape
{
    public string Name { get; set; } = "";
    public virtual string GetDescription() => $"Shape: {Name}";
}

public class Polygon : Shape
{
    public int NumberOfSides { get; set; }
    public override string GetDescription() => $"Polygon with {NumberOfSides} sides";
}

public class Triangle : Polygon
{
    public Triangle() => NumberOfSides = 3;
    public override string GetDescription() => "A triangle with 3 sides";
}
```
