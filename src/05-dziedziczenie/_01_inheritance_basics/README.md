# Podstawowe Pojęcia Dziedziczenia (Inheritance Basics)

## 🎯 Cel

Zrozumienie fundamentalnych koncepcji **dziedziczenia** - mechanizmu umożliwiającego klasom pochodnym odziedziczenie cech klasy bazowej.

## Definicja

**Dziedziczenie** to relacja `is-a` między klasami:
- **Klasa bazowa (base class)** - klasa, z której inne klasy dziedziczą
- **Klasa pochodna (derived class)** - klasa, która dziedziczy z innej klasy

```csharp
public class Animal          // Klasa bazowa
{
    public string Name { get; set; }
    public void Eat() => Console.WriteLine("Eating...");
}

public class Dog : Animal    // Klasa pochodna
{
    public void Bark() => Console.WriteLine("Woof!");
}

// Użycie
var dog = new Dog { Name = "Rex" };
dog.Eat();   // Odziedziczona metoda z Animal
dog.Bark();  // Własna metoda Dog
```

## Co się dziedziczy?

```
┌─────────────────────────┐
│      Animal (Base)      │
├─────────────────────────┤
│ + Name: string          │ ✅ DZIEDZICZY SIĘ
│ + Eat(): void           │ ✅ DZIEDZICZY SIĘ
│ - secretField: int      │ ❌ NIE (private)
└─────────────────────────┘
            ▲
            │ Dziedziczy
            │
┌─────────────────────────┐
│    Dog : Animal         │
├─────────────────────────┤
│ + Name: string          │ (odziedziczone)
│ + Eat(): void           │ (odziedziczone)
│ + Bark(): void          │ (własne)
└─────────────────────────┘
```

## Korzyści Dziedziczenia

### 1. **Reusability (Wielokrotne Użycie)**
```csharp
public class Animal
{
    public string Name { get; set; }
    public int Age { get; set; }
    public void Sleep() => Console.WriteLine("Zzz...");
}

public class Dog : Animal
{
    // Mamy Name, Age, Sleep() bez przepisywania
    public void Bark() => Console.WriteLine("Woof!");
}

public class Cat : Animal
{
    // Mamy Name, Age, Sleep() bez przepisywania
    public void Meow() => Console.WriteLine("Meow!");
}
```

### 2. **Hierarchia (Taxonomy)**
```
        Animal
        /  |  \
      Dog Cat Bird
      /      |      \
   Puppy  Kitten  Parrot
```

### 3. **Polimorfizm (tego będzie więcej w późniejszych tematach)**
```csharp
List<Animal> animals = new()
{
    new Dog { Name = "Rex" },
    new Cat { Name = "Whiskers" },
    new Bird { Name = "Tweety" }
};

foreach (var animal in animals)
{
    animal.Sleep();  // Każde zwierze śpi tak samo
}
```

## Składnia

```csharp
public class ClassName : BaseClass
{
    // Klasa pochodna
}
```

- Każda klasa może mieć **max jedną bezpośrednią klasę bazową** (single inheritance)
- C# nie wspiera multiple inheritance (ale są interfejsy)
- Wszystkie klasy dziedziczy z `object` (jawnie lub niejawnie)

## C# vs Java - Porównanie

| Aspekt | C# | Java |
|--------|-----|------|
| Słowo kluczowe | `:` | `extends` |
| Interface | `:` | `implements` |
| Baza wszystkiego | `object` | `Object` |
| Access modifiers | `public`, `private`, `protected`, `internal` | `public`, `private`, `protected` |

```csharp
// C#
public class Dog : Animal { }
public class ServiceImpl : IService { }

// Java
public class Dog extends Animal { }
public class ServiceImpl implements IService { }
```

## Wszystkie klasy dziedziczą z `object`

```csharp
public class Animal { }

// To jest tożsame z:
public class Animal : object { }

// Stąd każda klasa ma metody z object:
var dog = new Animal();
dog.GetType();        // object method
dog.ToString();       // object method
dog.Equals(other);    // object method
dog.GetHashCode();    // object method
```

## Łańcuch Dziedziczenia

```csharp
public class Animal { }
public class Mammal : Animal { }
public class Dog : Mammal { }

var dog = new Dog();
// dog jest: Dog, Mammal, Animal, object
```

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

## 📚 Referencje

- [Microsoft Learn: Inheritance](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [C# Player's Guide - Chapter 13](https://csharpplayersguide.com/)
- [Effective C# - Item 36: Base Class Design](https://www.informit.com/store/effective-c-plus-items-1-50-9780136834830)

