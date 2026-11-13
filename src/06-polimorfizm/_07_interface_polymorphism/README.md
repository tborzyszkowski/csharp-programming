# Temat 7: Polimorfizm oparty na interfejsach

**Moduł:** [06-polimorfizm](../README.md)
**Kod:** [`code/`](code/) (`InterfacePolymorphism.csproj`, `Program.cs` z demo i testami xUnit)

---

## 🎯 Idea

**Polimorfizm oparty na interfejsach** (interface-based polymorphism) polega na tym, że kod operuje na typie interfejsu (`IShape`), a nie na konkretnej klasie (`Circle`, `Rectangle`). Każda klasa implementująca interfejs dostarcza własną wersję jego składowych, a środowisko uruchomieniowe wybiera właściwą implementację na podstawie **rzeczywistego typu obiektu** (late binding).

W odróżnieniu od polimorfizmu opartego na dziedziczeniu (`virtual`/`override`, zob. [Temat 1](../_01_virtual_methods_intro/README.md)):

| Cecha | Dziedziczenie (`virtual`/`abstract`) | Interfejs |
|-------|--------------------------------------|-----------|
| Relacja | „jest rodzajem" (`Circle` jest `Shape`) | „potrafi" (`Circle` umie `GetArea()`) |
| Liczba źródeł | Jedna klasa bazowa | Wiele interfejsów na jednej klasie |
| Współdzielony stan / kod | Tak (pola, konstruktor bazy) | Nie (tylko kontrakt; od C# 8 domyślne implementacje) |
| Typowe zastosowanie | Hierarchia z wspólną logiką | Kontrakt niezależny od hierarchii, testowalność, DI |

Kluczowa zaleta: klasy z zupełnie różnych hierarchii (np. `Circle` i `Logger`) mogą wspólnie implementować np. `IDisposable` lub `IShape`, a kod klienta zależy tylko od kontraktu.

---

## 🧩 Przykład

```csharp
public interface IShape
{
    double GetArea();
    double GetPerimeter();
}

public class Circle : IShape
{
    public double Radius { get; }
    public Circle(double radius) => Radius = radius;

    public double GetArea() => Math.PI * Radius * Radius;
    public double GetPerimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : IShape
{
    public double Width { get; }
    public double Height { get; }
    public Rectangle(double width, double height) => (Width, Height) = (width, height);

    public double GetArea() => Width * Height;
    public double GetPerimeter() => 2 * (Width + Height);
}

// Klasyczny przykład: losowe kształty, jeden kod dla wszystkich
var random = new Random(2024);
List<IShape> shapes = new();
for (int i = 0; i < 5; i++)
{
    shapes.Add(random.Next(2) == 0
        ? new Circle(random.NextDouble() * 10)
        : new Rectangle(random.NextDouble() * 10, random.NextDouble() * 10));
}

foreach (IShape shape in shapes)
    Console.WriteLine($"{shape.GetType().Name}: pole = {shape.GetArea():F2}");
```

`shape.GetArea()` jest jednym wywołaniem w pętli, ale dla każdego obiektu wykonuje się inny kod. Pełna wersja z testami: [`code/Program.cs`](code/Program.cs).

---

## ⚖️ Kiedy wybrać interfejs

- Potrzebujesz kontraktu, który mogą spełnić klasy z różnych hierarchii.
- Chcesz zależeć od abstrakcji (np. w DI, mockowaniu w testach).
- Nie chcesz narzucać klasom wspólnej klasy bazowej ani współdzielonego stanu.

Gdy klasy mają wspólny stan lub wspólną logikę, lepszym wyborem bywa klasa abstrakcyjna lub połączenie obu podejść (zob. [Moduł 7: Interfejsy i Abstrakcja](../../07-interfejsy_abstrakcje/README.md)).

---

## ✅ Zadania kontrolne

- [ ] Dodaj klasę `Triangle : IShape` (pole ze wzoru Herona) i sprawdź, że `CreateRandomShapes` nadal działa bez zmian w kodzie klienta.
- [ ] Wyjaśnij, dlaczego `shape.GetType().Name` pokazuje typ konkretny, mimo że zmienna ma typ `IShape`.
- [ ] Porównaj to podejście z `abstract class Shape` z [Modułu 5](../../05-dziedziczenie/README.md) — co zmieniłoby się w kodzie, gdyby `Circle` i `Rectangle` już dziedziczyły po innej klasie?
