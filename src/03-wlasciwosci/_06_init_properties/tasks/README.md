# Zadania - Init Properties i Required

## 📝 Zadanie 1: Immutable Person

Stwórz `Person` z init properties (Name, Age, Email) - niemożliwe do zmiany po created.

```csharp
var person = new Person { Name = "John", Age = 30 };
// person.Age = 31;  // ERROR - init-only
```

## 📝 Zadanie 2: Required Configuration

Stwórz `DatabaseConfig` z required `Host` i `Port`, opcjonalne `Username`.

```csharp
var config = new DatabaseConfig
{
    Host = "localhost",
    Port = 5432
};
```

## 📝 Zadanie 3: DTO z Mix Init i Required

Stwórz `OrderDto` z required `Id`, `CustomerId`, opcjonalne `Notes`.

---

## ✅ Rozwiązania w `code/Program.cs`
