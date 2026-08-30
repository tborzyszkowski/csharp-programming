# Zadania - Auto Properties

## 📝 Zadanie 1: Klasa Product

Stwórz klasę `Product` z auto properties:
- `Name` (string)
- `Price` (decimal)
- `Stock` (int)

```csharp
var product = new Product { Name = "Phone", Price = 999 };
Assert.Equal("Phone", product.Name);
```

## 📝 Zadanie 2: Asymetryczne accessory dla ID

Stwórz `Book` z `Id` (read-only), `Title` (read-write), inicjalizuj domyślnie.

## 📝 Zadanie 3: Backing Field z walidacją

Stwórz `Temperature` z właściwością `Celsius` (backing field) z walidacją -273 <= C <= 1000.

---

## ✅ Rozwiązania w `code/Program.cs`
