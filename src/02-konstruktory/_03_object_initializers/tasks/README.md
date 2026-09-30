# Zadania - Inicjalizatory Obiektów

## 📝 Zadanie 1: Klasa Product z inicjalizatorem

Stwórz klasę `Product`, w której `Name` i `Price` są **wymagane** przy tworzeniu (`required`) i później tylko do odczytu (`init`), a `InStock` jest opcjonalne (domyślnie `true`).

```csharp
var product = new Product
{
    Name = "Laptop",
    Price = 5000,
    InStock = true
};
```

Odpowiedz: co się stanie po próbie utworzenia `new Product { Price = 10 }`? A po `product.Price = 20;`?

## 📝 Zadanie 2: Klasa Order z nested inicjalizatorem

Klasa `Order` zawiera listę `Items` (nazwy produktów) i `Customer` (osoba). Użyj zagnieżdżonego inicjalizatora.

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

## 📝 Zadanie 3: Dictionary, `Add` a indeksator

Stwórz słownik `warehouse` z produktami i ich ilościami, używając collection initializer. Następnie:

1. zapisz go w dwóch wariantach: `{ "Laptop", 10 }` oraz `["Laptop"] = 10`;
2. sprawdź, co się stanie, gdy klucz wystąpi dwa razy w każdym z wariantów;
3. zapisz listę nazw produktów za pomocą wyrażenia kolekcji (C# 12) z operatorem rozwinięcia `..`.

```csharp
var warehouse = new Dictionary<string, int>
{
    ["Laptop"] = 10,
    ["Mouse"] = 50,
    ["Keyboard"] = 30
};
```

## 📝 Zadanie 4: Pułapka z `null`

Poniższy kod kompiluje się, ale rzuca wyjątek w czasie działania. Wyjaśnij dlaczego i popraw go na dwa sposoby.

```csharp
public class BrokenBasket
{
    public List<string> Items { get; set; }   // brak inicjalizacji
}

var basket = new BrokenBasket { Items = { "Apple", "Pear" } };
```

---

## ✅ Zadanie 1 - Rozwiązanie

```csharp
public class Product
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
    public bool InStock { get; init; } = true;
}

var product = new Product { Name = "Laptop", Price = 5000 };
// new Product { Price = 10 };   // BŁĄD KOMPILACJI CS9035: wymagany element Name nie został ustawiony
// product.Price = 20;           // BŁĄD KOMPILACJI CS8852: właściwość init-only można ustawić tylko w inicjalizatorze/konstruktorze
```

**Wyjaśnienie:** `required` wymusza podanie wartości przy tworzeniu obiektu, a `init` zamyka możliwość zmiany po zakończeniu
inicjalizacji. Razem dają czytelną składnię inicjalizatora **i** niezmienny obiekt.

## ✅ Zadanie 2 - Rozwiązanie

```csharp
public class Customer
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
}

public class Order
{
    public string OrderId { get; set; } = "";
    public Customer Customer { get; set; } = new();
    public List<string> Items { get; } = new();   // tylko getter: kolekcja zawsze istnieje, nie da się jej podmienić
}
```

**Wyjaśnienie:** `Items = { "Laptop", ... }` wywołuje `Add` na liście utworzonej w inicjalizatorze właściwości, dlatego
właściwość może mieć tylko `get`. Dzięki temu nie istnieje stan „`Items == null`”, a klienci nie mogą podmienić całej listy.

## ✅ Zadanie 3 - Rozwiązanie

```csharp
// Wariant 1: Add(klucz, wartość)
var withAdd = new Dictionary<string, int>
{
    { "Laptop", 10 },
    { "Mouse", 50 }
};

// Wariant 2: indeksator
var withIndexer = new Dictionary<string, int>
{
    ["Laptop"] = 10,
    ["Mouse"] = 50
};

// Duplikat klucza:
// new Dictionary<string,int> { { "Laptop", 1 }, { "Laptop", 2 } };   // ArgumentException (Add)
// new Dictionary<string,int> { ["Laptop"] = 1, ["Laptop"] = 2 };      // OK - ostatnia wartość wygrywa

List<string> names = [.. withIndexer.Keys];                 // wyrażenie kolekcji + spread
string[] all = ["Headphones", .. names, "Webcam"];          // łączenie kolekcji
```

## ✅ Zadanie 4 - Rozwiązanie

**Przyczyna:** `Items = { "Apple", "Pear" }` (bez `new`) nie tworzy listy, tylko wywołuje `Add` na liście, którą
właściwość *już* przechowuje. `Items` ma domyślnie `null`, więc pojawia się `NullReferenceException`.

```csharp
// Poprawka 1: inicjalizacja właściwości
public class Basket
{
    public List<string> Items { get; set; } = new();
}

// Poprawka 2: jawne utworzenie listy w inicjalizatorze obiektu
var basket = new Basket { Items = new List<string> { "Apple", "Pear" } };
```

---

## 🧪 Testy

```csharp
[Fact]
public void Product_RequiredAndInit_AreSetByInitializer()
{
    var p = new Product { Name = "Laptop", Price = 5000 };

    Assert.Equal("Laptop", p.Name);
    Assert.True(p.InStock);   // wartość domyślna
}

[Fact]
public void Order_NestedInitializer_FillsEverything()
{
    var order = new Order
    {
        OrderId = "ORD001",
        Customer = new Customer { Name = "John", Email = "john@example.com" },
        Items = { "Laptop", "Mouse" }
    };

    Assert.Equal("John", order.Customer.Name);
    Assert.Equal(new[] { "Laptop", "Mouse" }, order.Items);
}

[Fact]
public void Warehouse_AddThrowsOnDuplicate_IndexerDoesNot()
{
    Assert.Throws<ArgumentException>(() => new Dictionary<string, int> { { "Laptop", 1 }, { "Laptop", 2 } });

    var dict = new Dictionary<string, int> { ["Laptop"] = 1, ["Laptop"] = 2 };
    Assert.Equal(2, dict["Laptop"]);
}

[Fact]
public void Basket_CollectionInitializerOnNullProperty_Throws()
{
    Assert.Throws<NullReferenceException>(() => new BrokenBasket { Items = { "Apple" } });
    Assert.Single(new Basket { Items = { "Apple" } }.Items);
}
```

(`BrokenBasket` to wersja klasy z zadania 4 (bez inicjalizacji), `Basket` – poprawiona z rozwiązania.)

