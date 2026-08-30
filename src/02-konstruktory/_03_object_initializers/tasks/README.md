# Zadania - Inicjalizatory Obiektów

## 📝 Zadanie 1: Klasa Product z inicjalizatorem

Stwórz klasę `Product` z właściwościami (readonly fields lub auto-properties) i zademonstruj inicjalizator.

```csharp
var product = new Product
{
    Name = "Laptop",
    Price = 5000,
    InStock = true
};
```

## 📝 Zadanie 2: Klasa Order z nested inicjalizatorem

Klasa `Order` zawiera listę `Items` (produkty) i `Customer` (osoba). Użyj zagnieżdżonego initializer.

```csharp
var order = new Order
{
    OrderId = "ORD001",
    Customer = new Customer
    {
        Name = "John",
        Email = "john@example.com"
    },
    Items = { "Laptop", "Mouse", "Keyboard" }
};
```

## 📝 Zadanie 3: Dictionary i collection initializers

Stwórz słownik `warehouse` z produktami i ich ilościami, używając collection initializer.

```csharp
var warehouse = new Dictionary<string, int>
{
    ["Laptop"] = 10,
    ["Mouse"] = 50,
    ["Keyboard"] = 30
};
```

---

## ✅ Rozwiązania

Patrz: `code/Program.cs`

