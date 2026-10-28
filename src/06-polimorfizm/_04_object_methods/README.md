# Temat 4: Metody Klasy Object - ToString, Equals, GetHashCode

## 📌 Cel Tematu

Zrozumienie trzech kluczowych metod dziedziczonych z `object`:
- `ToString()` - reprezentacja tekstowa
- `Equals()` - porównanie równości
- `GetHashCode()` - hash dla kolekcji (Dict, Set)

## 🎯 Fundamenty

### Wszystko Dziedziczy z Object

```csharp
public class MyClass { }
// Równoważnie:
public class MyClass : object { }
```

**Każda klasa w C# ma dostęp do:**
- `ToString()` - zwraca string reprezentację (wirtualna)
- `Equals(object)` - porównuje dwie instancje (wirtualna)
- `GetHashCode()` - zwraca int hash (wirtualna)
- `GetType()` - zwraca typ obiektu w czasie działania (**niewirtualna**, nie można jej przesłonić)
- `MemberwiseClone()` - tworzy płytką kopię (`protected`, dostępna tylko wewnątrz klasy i jej pochodnych)

To właśnie `ToString`, `Equals` i `GetHashCode` są wirtualne - przesłaniasz je słowem `override`, tak jak każdą inną metodę wirtualną (temat 1).

---

## 📝 ToString()

### Domyślna Implementacja

```csharp
public class Person
{
    public string Name { get; set; }
}

var p = new Person { Name = "John" };
Console.WriteLine(p.ToString());  // Output: "Namespace.Person"
```

**Domyślnie**: Zwraca pełną nazwę klasy (namespace + class)

### Przesłanianie ToString()

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public override string ToString()
    {
        return $"Person: {Name}, Age {Age}";
    }
}

var p = new Person { Name = "John", Age = 30 };
Console.WriteLine(p.ToString());  // Output: "Person: John, Age 30"
```

### Real-World: E-Commerce

```csharp
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public override string ToString()
    {
        return $"[{Name}] ${Price:F2}";
    }
}

var products = new List<Product>
{
    new() { Name = "Laptop", Price = 999.99m },
    new() { Name = "Mouse", Price = 29.99m }
};

foreach (var p in products)
{
    Console.WriteLine(p);  // Automatycznie wołuje ToString()!
}
// Output:
// [Laptop] $999.99
// [Mouse] $29.99
```

---

## 🔍 Equals() i GetHashCode()

### Domyślna Implementacja Equals()

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

var user1 = new User { Id = 1, Name = "John" };
var user2 = new User { Id = 1, Name = "John" };

Console.WriteLine(user1.Equals(user2));  // false!
// Domyślnie: porównuje referencje, nie wartości!
```

**Domyślnie dla typów referencyjnych**: `Equals()` z `object` porównuje tożsamość referencji. Typy wartościowe i niektóre typy frameworka mają własne implementacje równości.

### Przesłanianie Equals() i GetHashCode()

```csharp
public class User : IEquatable<User>
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not User other)
            return false;
        
        return Id == other.Id && Name == other.Name;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}

var user1 = new User { Id = 1, Name = "John" };
var user2 = new User { Id = 1, Name = "John" };

Console.WriteLine(user1.Equals(user2));  // true!
Console.WriteLine(user1.GetHashCode() == user2.GetHashCode());  // true!
```

---

## ⚡ GetHashCode() - Dlaczego to Ważne?

### Kolekcje HashSet i Dictionary

```csharp
var users = new HashSet<User>
{
    new() { Id = 1, Name = "John" },
    new() { Id = 1, Name = "John" }  // Duplikat?
};

Console.WriteLine(users.Count);  // Bez przesłonięcia Equals/GetHashCode: 2 (to różne referencje)
                                 // Z poprawnymi Equals i GetHashCode: 1 (duplikat rozpoznany)
```

### Jak Działa HashSet

1. Wołuje `GetHashCode()` - wybiera "kubełek" w tablicy mieszającej
2. Dla elementów z tego samego kubełka wołuje `Equals()`
3. Jeśli `Equals()` zwraca true - duplikat!

**REGUŁA**: Jeśli przesłaniasz `Equals()`, MUSISZ przesłonić `GetHashCode()` (kompilator zgłosi ostrzeżenie `CS0659`, jeśli o tym zapomnisz).

> **Uwaga na mutowalne klucze:** jeśli obiekt zmieni pole używane przez `Equals`
> lub `GetHashCode` po dodaniu do `Dictionary`/`HashSet`, kolekcja może nie znaleźć
> tego obiektu. Klucz powinien być niemutowalny albo nie należy zmieniać jego pól
> tożsamości podczas przechowywania w kolekcji.

```csharp
public class Product
{
    public int Id { get; set; }
    
    public override bool Equals(object? obj)
    {
        return obj is Product p && p.Id == Id;
    }
    
    public override int GetHashCode()  // ✅ ZAWSZE razem z Equals!
    {
        return Id.GetHashCode();
    }
}
```

---

## 📊 Diagram: Equals & GetHashCode Flow

```mermaid
graph TD
    A["user1.Equals(user2)"]
    B{"Override<br/>Equals?"}
    C["Porównanie<br/>referencji"]
    D["Porównanie<br/>wartości"]
    E["Return false<br/>Inne obiekty"]
    F["Return true<br/>Równe wartości"]
    
    A --> B
    B -->|Nie| C --> E
    B -->|Tak| D --> F
    
    style D fill:#ccffcc
    style F fill:#ffcccc
```

---

## 💰 Real-World: E-Commerce Product Equality

```csharp
public class Product : IEquatable<Product>
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    
    // Dwa produkty są równe jeśli mają ten sam ID
    public override bool Equals(object? obj) => Equals(obj as Product);
    
    public bool Equals(Product? other)
    {
        return other != null && ProductId == other.ProductId;
    }
    
    public override int GetHashCode()
    {
        return ProductId.GetHashCode();
    }
    
    public override string ToString()
    {
        return $"Product#{ProductId}: {Name}";
    }
}

// Użycie
var cart1 = new HashSet<Product>
{
    new() { ProductId = 1, Name = "Laptop" },
    new() { ProductId = 2, Name = "Mouse" }
};

var cart2 = new HashSet<Product>
{
    new() { ProductId = 1, Name = "Laptop" },  // Duplikat
    new() { ProductId = 3, Name = "Keyboard" }
};

var combined = new HashSet<Product>(cart1);
combined.UnionWith(cart2);

Console.WriteLine($"Unique products: {combined.Count}");  // 3
foreach (var product in combined)
{
    Console.WriteLine($"  - {product}");
}
```

---

## 🎓 Wytyczne

| Metoda | Kiedy | Jak |
|--------|-------|-----|
| **ToString()** | Zawsze (debug, logs, UI) | Zwróć user-friendly string |
| **Equals()** | Gdy porównywacie wartości | Porównaj pola, nie referencje |
| **GetHashCode()** | Razem z Equals() | Musi być konsystentny z Equals() |

---

## ⚠️  Reguły

1. **Jeśli przesłonisz `Equals()`, przesłoń `GetHashCode()`**
2. **Jeśli `a.Equals(b)`, to `a.GetHashCode() == b.GetHashCode()`.** Odwrotnie nie musi być prawdą: różne obiekty *mogą* mieć ten sam hash (kolizja), więc nie zakładaj, że różne wartości zawsze dają różne hashe.
3. **`Equals` musi być zwrotne, symetryczne i przechodnie, a `x.Equals(null)` zwraca `false`**
4. **`ToString()` to dla ludzi, nie dla maszyn**
5. **Równość wartości to nie to samo co tożsamość** (`ReferenceEquals`)
6. **`==` nie jest automatycznie powiązany z `Equals`**: dla klas bez przeciążonego operatora `==` porównuje referencje (wyjątek: `string`, który ma własne `==`). Jeśli przeciążasz `==`, przeciąż też `!=` i zachowaj spójność z `Equals`
7. **Nie utrwalaj wartości `GetHashCode()`** (w plikach, bazie): w .NET `HashCode.Combine` i `string.GetHashCode()` są losowane per proces, więc ta sama wartość może mieć inny hash po restarcie aplikacji
8. **Dla `IEquatable<T>` zaimplementuj oba:** `Equals(T)` i `Equals(object)`, żeby kolekcje generyczne nie musiały pakować typów wartościowych (boxing)

> **Typy `record`** generują `Equals`, `GetHashCode`, `ToString` i `==` automatycznie (równość wartości po wszystkich polach). W tym temacie implementujemy je ręcznie, żeby zrozumieć mechanizm.

---

## 📁 Kod Demonstracyjny

Znajduje się w katalogu `code/`:
- `Program.cs` - ToString, Equals, GetHashCode w akcji

Uruchomienie:
```bash
cd code
dotnet run
```

---

## 🔗 Referencje

- [Object.ToString()](https://learn.microsoft.com/en-us/dotnet/api/system.object.tostring)
- [Object.Equals()](https://learn.microsoft.com/en-us/dotnet/api/system.object.equals)
- [Object.GetHashCode()](https://learn.microsoft.com/en-us/dotnet/api/system.object.gethashcode)
- [IEquatable<T>](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1)

## 🧭 Nawigacja

| Przejdź do | Link |
|---|---|
| Poprzedni temat | [Temat 3: Virtual vs New](../_03_virtual_vs_new/README.md) |
| Moduł | [06-polimorfizm](../README.md) |
| Następny temat | [Temat 5: Adapter + Factory](../_05_adapter_factory/README.md) |
