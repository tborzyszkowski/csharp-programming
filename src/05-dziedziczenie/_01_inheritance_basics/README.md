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
│ - secretField: int      │ ⚠️ istnieje w obiekcie, ale jest niedostępny
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
- Wszystkie klasy dziedziczą z `object` (jawnie lub niejawnie)
- Konstruktory **nie są dziedziczone** - każda klasa pochodna definiuje własne (omówione w temacie 3)

Dziedziczenie oznacza relację **is-a**, a nie tylko możliwość ponownego użycia kodu. `Dog` jest `Animal`, więc można przekazać go do metody przyjmującej `Animal`. Jeśli relacja brzmi raczej „ma” (`Car` ma `Engine`), kompozycja zwykle będzie lepszym rozwiązaniem niż kolejna klasa bazowa.

Prywatne pola klasy bazowej nie znikają: stan bazowy istnieje w obiekcie pochodnym, ale kod klasy pochodnej nie ma do niego bezpośredniego dostępu. Odwołuje się do niego przez publiczne/protected API klasy bazowej. Dlatego dokładniej mówimy, że prywatne składowe są *niedostępne* w klasie pochodnej, a nie że „nie istnieją”.

## C# vs Java - Porównanie

| Aspekt | C# | Java |
|--------|-----|------|
| Słowo kluczowe | `:` | `extends` |
| Interface | `:` | `implements` |
| Baza wszystkiego | `object` | `Object` |
| Access modifiers | `public`, `private`, `protected`, `internal`, `protected internal`, `private protected` | `public`, `private`, `protected`, brak modyfikatora (package-private) |

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

Można sprawdzić zgodność z typem bazowym operatorem `is`, ale rzutowanie powinno mieć sens w modelu domeny:

```csharp
Animal animal = new Dog { Name = "Rex" };
if (animal is Dog dog)
    dog.Bark();
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

## 🧭 Nawigacja

[← Moduł 05: dziedziczenie i polimorfizm](../README.md) · [Modyfikatory dostępu →](../_02_access_modifiers/README.md)

## 📚 Referencje

- [Microsoft Learn: Inheritance](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [C# Player's Guide - Chapter 13](https://csharpplayersguide.com/)
- [Effective C# - Item 36: Base Class Design](https://www.informit.com/store/effective-c-plus-items-1-50-9780136834830)
