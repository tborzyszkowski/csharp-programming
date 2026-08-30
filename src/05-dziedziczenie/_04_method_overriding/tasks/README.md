# Zadania: Przesłonięcie Metod

## Zadanie 1: Override z Polimorfizmem

Stwórz hierarchię:
- `Shape(virtual Draw())`
- `Triangle : Shape(override Draw())`
- `Square : Shape(override Draw())`

Następnie polimorfizm w liście Shape'ów.

## Zadanie 2: Base Keyword

Stwórz klasę pochodną która:
- Przesłania metodę
- Wywoła wersję bazową przy `base.Method()`

## Zadanie 3: Override vs New

Pokaż różnicę - gdy wołasz metodę przez zmienną bazową, override daje inny wynik niż new.

---

## Rozwiązania

### Zadanie 1
```csharp
public abstract class Shape
{
    public abstract void Draw();
}

public class Triangle : Shape
{
    public override void Draw() => Console.WriteLine("Drawing Triangle");
}

public class Square : Shape
{
    public override void Draw() => Console.WriteLine("Drawing Square");
}

List<Shape> shapes = new() { new Triangle(), new Square() };
foreach (var shape in shapes) shape.Draw();
```
