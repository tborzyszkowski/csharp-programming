# Zadania - Właściwości vs Pola

## 📝 Zadanie 1: Pracownik z walidacją

Stwórz klasę `Employee` z właściwościami:
- `Name` (string, nie może być pusty)
- `Salary` (decimal, musi być > 0)
- `Department` (string)

```csharp
var emp = new Employee { Name = "John", Salary = 5000 };
emp.Salary = -100;  // Zignorowane
Assert.Equal(5000, emp.Salary);
```

## 📝 Zadanie 2: Read-only ID

Stwórz `Product` z read-only `Id` (ustawiany tylko w konstruktorze).

## 📝 Zadanie 3: Właściwość obliczona

Stwórz `Rectangle` z właściwościami `Width` i `Height`, oraz **getter** dla `Area` (obliczona).

---

## ✅ Rozwiązania w `code/Program.cs`
