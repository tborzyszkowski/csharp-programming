# Zadania - Walidacja Właściwości

## 📝 Zadanie 1: Strict Validation

Stwórz klasę `Product` z walidacją:
- `Name` (nie może być pusty)
- `Price` (musi być > 0, rzuć exception)

```csharp
var product = new Product { Name = "Phone" };
Assert.Throws<ArgumentException>(() => product.Price = -100);
```

## 📝 Zadanie 2: Silent Ignore

Stwórz `Temperature` z `Celsius` - ustawiaj tylko wartości w zakresie, ignoruj resztę.

## 📝 Zadanie 3: PropertyChanged Event

Stwórz `User` z event `OnNameChanged` - powiadomienie gdy się zmienia.

---

## ✅ Rozwiązania w `code/Program.cs`
