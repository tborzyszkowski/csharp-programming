# Tworzenie Obiektu na Podstawie Wzorca Prototyp

## 🎯 Cel rozdziału

Zrozumienie Prototype Pattern - tworzenie nowych obiektów przez klonowanie istniejących zamiast konstruowania od zera.

## 📚 Spis treści

1. [Czym jest Prototype Pattern?](#czym-jest-prototype-pattern)
2. [ICloneable interface](#icloneable-interface)
3. [Shallow Copy vs Deep Copy](#shallow-copy-vs-deep-copy)
4. [Praktyczne przykłady](#praktyczne-przykłady)

---

## Czym jest Prototype Pattern?

**Prototype Pattern** to creational pattern, który tworzy nowe obiekty przez klonowanie (kopiowanie) istniejącego obiektu ("prototypu") zamiast konstruowania od zera.

### Kiedy używać?

- Tworzenie obiektów jest kosztowne (CPU, I/O)
- Chcesz niezależną kopię obiektu
- Chcesz unikn ąć podklas dla każdej możliwej konfiguracji

---

## ICloneable Interface

```csharp
public interface ICloneable
{
    object Clone();  // Zwraca kopię obiektu
}

public class Person : ICloneable
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public object Clone()
    {
        return this.MemberwiseClone();  // Shallow copy
    }
}

var person1 = new Person { Name = "John", Age = 30 };
var person2 = (Person)person1.Clone();

person2.Name = "Jane";
Console.WriteLine(person1.Name);  // John (zmienił się tylko clone)
Console.WriteLine(person2.Name);  // Jane
```

---

## Shallow Copy vs Deep Copy

### Shallow Copy

```csharp
public class Address
{
    public string City { get; set; } = string.Empty;
}

public class Employee : ICloneable
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    
    public object Clone()
    {
        return this.MemberwiseClone();  // Shallow copy!
    }
}

var emp1 = new Employee
{
    Name = "John",
    Address = new Address { City = "Warszawa" }
};

var emp2 = (Employee)emp1.Clone();  // Shallow copy

emp2.Address.City = "Kraków";  // UWAGA: zmienia też emp1!
Console.WriteLine(emp1.Address.City);  // Kraków (!!)
Console.WriteLine(emp2.Address.City);  // Kraków
```

### Deep Copy

```csharp
public class Employee : ICloneable
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    
    public object Clone()
    {
        var cloned = (Employee)this.MemberwiseClone();
        cloned.Address = new Address { City = this.Address.City };  // Deep copy Address!
        return cloned;
    }
}

var emp1 = new Employee
{
    Name = "John",
    Address = new Address { City = "Warszawa" }
};

var emp2 = (Employee)emp1.Clone();  // Deep copy

emp2.Address.City = "Kraków";  // Zmienia tylko emp2!
Console.WriteLine(emp1.Address.City);  // Warszawa
Console.WriteLine(emp2.Address.City);  // Kraków
```

---

## Praktyczne Przykłady

### Factory z Prototypem

```csharp
public class PrototypeFactory
{
    private Dictionary<string, (ICloneable Template)> prototypes = new();
    
    public void RegisterPrototype(string key, ICloneable template)
    {
        prototypes[key] = (template);
    }
    
    public ICloneable Create(string key)
    {
        return prototypes[key].Template.Clone() as ICloneable ??
            throw new InvalidOperationException();
    }
}

var factory = new PrototypeFactory();
factory.RegisterPrototype("standard-doc", new Document { Type = "PDF" });

var doc1 = factory.Create("standard-doc");  // Klon prototypu
var doc2 = factory.Create("standard-doc");  // Inny klon
```

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
