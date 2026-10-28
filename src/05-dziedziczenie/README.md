# MODUŁ 05-DZIEDZICZENIE: Dziedziczenie i polimorfizm w C#

## 📚 Wprowadzenie

Moduł kompleksowo omawia **dziedziczenie (inheritance)** i **polimorfizm** w C# - mechanizmy fundamentalne dla programowania obiektowego pozwalające na reużywalność kodu, hierarchie klas i flexible design'y.

Od podstawowych pojęć, przez modyfikatory dostępu i inicjalizację, aż po polimorfizm, klasy abstrakcyjne i `sealed`.

> **Jak korzystać z modułu:** każdy podrozdział jest samodzielnym projektem .NET. Przeczytaj README, uruchom `dotnet run`, zmień przykład w `Program.cs`, a następnie uruchom `dotnet test`.

---

## 📋 Tematy (7 wykładów)

| # | Temat | Opis | Status | Code | Testy | Zadania |
|---|-------|------|--------|------|-------|---------|
| 1 | [Podstawowe pojęcia](_01_inheritance_basics/README.md) | Relacja `is-a`, klasa bazowa i pochodna | ✅ | ✅ | ✅ | ✅ |
| 2 | [Modyfikatory dostępu](_02_access_modifiers/README.md) | `public`, `private`, `protected`, `internal` | ✅ | ✅ | ✅ | ✅ |
| 3 | [Konstruktory pochodne](_03_derived_constructors/README.md) | `base`, kolejność inicjalizacji i konstruktory | ✅ | ✅ | ✅ | ✅ |
| 4 | [Override vs New](_04_method_overriding/README.md) | Przesłonięcie i ukrycie metod | ✅ | ✅ | ✅ | ✅ |
| 5 | [Virtual i polimorfizm](_05_virtual_polymorphism/README.md) | Dynamic dispatch, `is`/`as`, polimorfizm | ✅ | ✅ | ✅ | ✅ |
| 6 | [Klasy abstrakcyjne](_06_abstract_classes/README.md) | `abstract`, interfejsy i kontrakty | ✅ | ✅ | ✅ | ✅ |
| 7 | [Klasy sealed](_07_sealed_classes/README.md) | Ograniczenie dziedziczenia i override | ✅ | ✅ | ✅ | ✅ |

---

## 🎯 Topologia Nauki

```
Temat 1: Podstawowe pojęcia (fundament)
    ↓
Temat 2: Modyfikatory dostępu (enkapsulacja)
    ↓
Temat 3: Konstruktory pochodne (inicjalizacja)
    ↓
Temat 4: Override vs New (metody)
    ↓
Temat 5: Virtual & Polymorfizm (advanced)
    ↓
Temat 6: Abstract Classes (wzorce)
    ↓
Temat 7: Sealed Classes (finalizacja)
```

### Nawigacja po module

| Przejdź do | Link |
|---|---|
| Temat 1 | [_01_inheritance_basics](_01_inheritance_basics/README.md) |
| Temat 2 | [_02_access_modifiers](_02_access_modifiers/README.md) |
| Temat 3 | [_03_derived_constructors](_03_derived_constructors/README.md) |
| Temat 4 | [_04_method_overriding](_04_method_overriding/README.md) |
| Temat 5 | [_05_virtual_polymorphism](_05_virtual_polymorphism/README.md) |
| Temat 6 | [_06_abstract_classes](_06_abstract_classes/README.md) |
| Temat 7 | [_07_sealed_classes](_07_sealed_classes/README.md) |
| Poprzedni moduł | [04-statyczne](../04-statyczne/README.md) |
| Następny moduł | [06-polimorfizm](../06-polimorfizm/README.md) |

Każdy podrozdział zawiera w stopce link do tej strony oraz odnośniki do poprzedniego i następnego tematu.

---

## 📖 Szczegóły Tematów

### Temat 1: Podstawowe Pojęcia Dziedziczenia

**Kategoria**: Fundamenty

**Kluczowe Koncepty**:
- Dziedziczenie jako `is-a` relacja
- Klasa bazowa (base) i pochodna (derived)
- Co się dziedziczy (public/protected) vs nie (private)
- Łańcuch dziedziczenia
- Wszystkie klasy dziedziczą z `object`

**Gdzie**: [`_01_inheritance_basics/`](_01_inheritance_basics/)

**Kod**:
```csharp
public class Animal
{
    public string Name { get; set; }
    public void Eat() { }
}

public class Dog : Animal
{
    public void Bark() { }
}

var dog = new Dog();
dog.Eat();   // Odziedziczone
dog.Bark();  // Własne
```

---

### Temat 2: Modyfikatory Dostępu w Dziedziczeniu

**Kategoria**: Enkapsulacja

**Kluczowe Koncepty**:
- `public` - dostęp wszędzie
- `protected` - dostęp w pochodnych (C# różni się od Java!)
- `private` - dostęp tylko w klasie
- `internal` - dostęp w assembly
- Tabela dostępu dla dziedziczenia
- Porównanie C# vs Java

**Gdzie**: [`_02_access_modifiers/`](_02_access_modifiers/)

**Kod**:
```csharp
public class Vehicle
{
    public string Make { get; set; }        // ✅ Wszędzie
    protected int MaxSpeed { get; set; }    // ✅ Pochodne
    private double fuel;                    // ❌ Tylko klasa
}

public class Car : Vehicle
{
    public void Drive()
    {
        Console.WriteLine(Make);       // ✅ OK
        Console.WriteLine(MaxSpeed);   // ✅ OK
        // Console.WriteLine(fuel);    // ❌ BŁĄD
    }
}
```

**Tabela C# vs Java**:

| Modyfikator | C# | Java |
|-------------|-----|------|
| public | Wszędzie | Wszędzie |
| protected | Klasa i jej pochodne | Pochodne + ten sam pakiet |
| private | Tylko klasa | Tylko klasa |
| internal | Całe assembly | brak odpowiednika (najbliższy: package-private) |
| brak modyfikatora | `internal` dla typu najwyższego poziomu, `private` dla składowych | package-private |

---

### Temat 3: Inicjalizacja Klas Pochodnych

**Kategoria**: Konstruktory

**Kluczowe Koncepty**:
- Słowo kluczowe `base` - wywoływanie konstruktora klasy bazowej
- Kolejność inicjalizacji (pola pochodne → pola bazowe → konstruktor bazowy → konstruktor pochodny)
- Brak domyślnego konstruktora w bazie - wymuszenie `base(...)`
- Łańcuch konstruktorów

**Gdzie**: [`_03_derived_constructors/`](_03_derived_constructors/)

**Kod**:
```csharp
public class Animal
{
    public Animal(string name) { Name = name; }
}

public class Dog : Animal
{
    public Dog(string name, string breed) : base(name)
    {
        Breed = breed;
    }
}

var dog = new Dog("Rex", "Labrador");
// Inicjalizacja: ciało konstruktora Animal → ciało konstruktora Dog
```

---

### Temat 4: Przesłonięcie Metod (Override vs New)

**Kategoria**: Polimorfizm

**Kluczowe Koncepty**:
- `virtual` - metoda może być przesłonięta
- `override` - przesłanianie (polimorfizm)
- `new` - ukrycie metody (nie polimorfizm!)
- `base` - wywoływanie metody bazowej

**Gdzie**: [`_04_method_overriding/`](_04_method_overriding/)

**Kod**:
```csharp
public class Animal
{
    public virtual void Speak() => Console.WriteLine("Sound");
}

public class Dog : Animal
{
    public override void Speak() => Console.WriteLine("Woof!");  // Polimorfizm
}

public class Cat : Animal
{
    public new void Speak() => Console.WriteLine("Meow!");  // Ukrycie (nie polimorfizm)
}

Animal animal = new Dog();
animal.Speak();  // "Woof!" (Dog version)
```

---

### Temat 5: Virtual Methods i Polimorfizm

**Kategoria**: Zaawansowany OOP

**Kluczowe Koncepty**:
- Dynamic dispatch - runtime type checking
- Virtual properties
- Type checking: `is` i `as` operatory
- Pattern matching
- Payment system example

**Gdzie**: [`_05_virtual_polymorphism/`](_05_virtual_polymorphism/)

**Kod**:
```csharp
List<Payment> payments = new()
{
    new CreditCard(),
    new PayPal(),
};

foreach (var payment in payments)
{
    payment.Process();  // Virtual dispatch - każdy robi swoje

    // Type checking
    if (payment is CreditCard card)
    {
        card.ValidateCard();
    }
}
```

---

### Temat 6: Klasy Abstrakcyjne

**Kategoria**: Wzorce projektowe

**Kluczowe Koncepty**:
- `abstract class` - nie można instancjonować
- `abstract member` - MUSI być implementowane
- Vs `virtual` - musi vs może
- Vs `interface` - implementacja vs kontrakt
- Database system example

**Gdzie**: [`_06_abstract_classes/`](_06_abstract_classes/)

**Kod**:
```csharp
public abstract class Database
{
    public abstract void Connect();              // MUSI implementować
    public virtual void LogConnection() { }     // MOŻE implementować
}

public class SqlDatabase : Database
{
    public override void Connect() { }  // ✅ Wymaga
}

// var db = new Database();  // ❌ BŁĄD
var sqlDb = new SqlDatabase();  // ✅ OK
```

---

### Temat 7: Sealed Classes i Members

**Kategoria**: Finalizacja

**Kluczowe Koncepty**:
- `sealed class` - koniec hierarchii
- `sealed override` - koniec przesłaniania
- Kiedy używać sealed (zamknięcie projektu, ochrona niezmienników, możliwe optymalizacje JIT)
- Sealed vs Abstract

**Gdzie**: [`_07_sealed_classes/`](_07_sealed_classes/)

**Kod**:
```csharp
public sealed class FinalImplementation : Base { }
// public class Derived : FinalImplementation { }  // ❌ BŁĄD

public class Dog : Animal
{
    public sealed override void Speak() { }  // Nie można dalej override
}

// public class Puppy : Dog { public override void Speak() { } }  // ❌ BŁĄD
```

---

## 💻 Jak Pracować

### Dla każdego tematu:

```bash
# 1. Wejdź do katalogu kodu
cd _01_inheritance_basics/code/

# 2. Uruchom demonstrację
dotnet run

# 3. Uruchom testy
dotnet test

# 4. Otwórz kod w VS Code
code Program.cs

# 5. Popatrz diagram
cat ../diagrams/*.mermaid  # Otwórz w markdown preview
```

### Struktura projektu:

```
05-dziedziczenie/
├── README.md                    # Ten plik
├── _01_inheritance_basics/
│   ├── README.md               # Wyjaśnienia
│   ├── code/
│   │   ├── InheritanceBasics.csproj
│   │   └── Program.cs          # Demo + xUnit testy
│   ├── diagrams/
│   │   └── 01-inheritance-basics.mermaid
│   └── tasks/
│       └── README.md           # Zadania dla studentów
├── _02_access_modifiers/
│   └── [identyczna struktura]
├── ... (tematy 3-7)
└── [główny README.md]
```

---

## 🛠️ Technologia

| Komponent | Wersja | Dlaczego |
|-----------|--------|---------|
| .NET SDK | 9.0+ | Nowsze API, performance |
| C# | 13 (latest) | Pełne wsparcie nowoczesnych cech |
| xUnit | 2.6.6 | Standardowy framework testów |
| Nullable | enable | Strict null checking |

---

## 📊 Statystyka Modułu

| Metrika | Wartość |
|---------|---------|
| Tematy | 7 |
| README.md | 8 (7 + 1 główny) |
| Program.cs | 7 (każdy z testami xUnit) |
| .csproj | 7 (wszystkie .NET 9.0) |
| Diagramy Mermaid | 7+ |
| Zadania | 20+ ćwiczeń |
| Testy xUnit | 40+ testów |
| Linii kodu C# | 1500+ |
| Dokumentacja | 10000+ słów (PL) |

---

## 🎓 Oczekiwane Efekty Uczenia

Po ukończeniu tego modułu student powinien:

- [ ] Rozumieć jest-a (is-a) relacja między klasami
- [ ] Poprawnie używać modyfikatorów dostępu
- [ ] Inicjalizować klasy pochodne z `base`
- [ ] Rozróżniać `override` i `new`
- [ ] Stosować polimorfizm w praktyce
- [ ] Projektować abstract hierarchie
- [ ] Używać `sealed` do finalizacji
- [ ] Rozumieć różnice między C# a Java
- [ ] Implementować real-world pattern'y (payment, database, itp.)

---

## 📚 Referencje i Zasoby

### Microsoft Learn
- [Inheritance in C#](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/inheritance)
- [Polymorphism](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Abstract and Sealed Classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)
- [Access Modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers)

### Słowa Kluczowe
- [virtual](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/virtual)
- [override](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override)
- [abstract](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/abstract)
- [sealed](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sealed)
- [base](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/base)

### Książki
- **C# Player's Guide** - RB Whitaker (Chapt. 12-15: OOP)
- **CLR via C#** - Jeffrey Richter (Chapt. 7-8: Inheritance)
- **C# 11 in a Nutshell** - Joseph Albahari (Chapt. 3: OOP)

### Online
- Stack Overflow: `[c#] inheritance` tags
- YouTube: "C# Inheritance and Polymorphism Tutorial"
- GitHub: Open source C# projects with real hierarchies

---

## ✅ Checklist dla Nauczycieli

Przed pierwszym wykładem:

- [ ] Przeczytaj README każdego tematu
- [ ] Uruchom `dotnet run` w każdym temacie
- [ ] Przejrzyj diagramy (Mermaid)
- [ ] Przygotuj pytania do studentów
- [ ] Skopiuj zadania do LMS (jeśli używasz)
- [ ] Skonfiguruj VS Code dla studentów
- [ ] Test projektor/HDMI połączenie
- [ ] Przygotuj przykłady z real-world (payment systems, itp.)

---

## ✅ Checklist dla Studentów

Po każdym temacie:

- [ ] Przeczytałem README.md
- [ ] Rozumiem diagramy i koncepty
- [ ] Zagrałem demo (`dotnet run`)
- [ ] Analizowałem kod w detail
- [ ] Zrobiłem przynajmniej 2 zadania
- [ ] Uruchomiłem testy (`dotnet test`)
- [ ] Mogę wyjaśnić główne koncepty
- [ ] Porównuję z Java (jeśli wiesz)

---

## 🚀 Quick Start

Chcesz szybko zacząć? Oto najkrótszy możliwy początek:

```bash
# Temat 1: Inheritance basics
cd _01_inheritance_basics/code
dotnet run
dotnet test

# Temat 2: Access modifiers
cd ../../_02_access_modifiers/code
dotnet run
dotnet test

# ... i tak dalej dla każdego tematu
```

---

## 💡 Porównanie Z Poprzednimi Modułami

| Moduł | Tematy | Kod | Testy | Zadania | Status |
|-------|--------|-----|-------|---------|--------|
| 02-Konstruktory | 10 | 3000+ | 50+ | 30+ | ✅ 100% |
| 03-Właściwości | 7 | 2500+ | 45+ | 20+ | ✅ 100% |
| 04-Statyczne | 7 | 1500+ | 35+ | 15+ | ✅ 100% |
| 05-Dziedziczenie | 7 | 1500+ | 40+ | 20+ | ✅ 100% |

---

## 📝 Historia Zmian

- **2024-08-30**: v1.0 - Moduł kompletny, 7 tematów, wszystkie budują ✅

---

## 🏆 Podsumowanie

**Moduł 05-Dziedziczenie to kompleksowy zestaw materiałów edukacyjnych** do nauczania dziedziczenia, polimorfizmu i wzorców OOP w C#.

- ✅ **7 tematów** od podstaw do zaawansowanych koncepcji
- ✅ **1500+ linii kodu** działającego i przetestowanego
- ✅ **20+ ćwiczeń** dla studentów
- ✅ **40+ testów** xUnit
- ✅ **7 diagramów** UML/Mermaid
- ✅ **Porównanie C# vs Java** dla tych co znają Javę
- ✅ **Real-world examples** (Payment systems, Databases, itp.)
- ✅ **Gotowy do użycia** w nauczaniu

**Status**: 🟢 **GOTOWY DO NAUCZANIA**

---

*Stworzone dla uniwersytetu, nauczycieli i studentów C#*

*Ostatnia aktualizacja: 2024-08-30*
