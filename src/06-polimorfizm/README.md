# MODUŁ 06-POLIMORFIZM: Polimorfizm i Funkcje Wirtualne w C#

## 📚 Wprowadzenie

Moduł kompleksowo omawia **polimorfizm (polymorphism)** - fundamentalny koncept programowania obiektowego pozwalający na dynamiczne wiązanie metod i elastyczną architekturę.

Od podstaw wirtualnych metod, przez praktyczne e-commerce example'i, aż po nowoczesne C# 8+ features.

---

## 📋 Tematy (6 wykładów)

| # | Temat | Opis | Status | Code | Testy | Zadania |
|---|-------|------|--------|------|-------|---------|
| 1 | [Funkcje Wirtualne: Wstęp](#temat-1-funkcje-wirtualne-wstęp) | Virtual methods, late binding, polimorfizm | ✅ | ✅ | ✅ | ✅ |
| 2 | [Przykład: E-Commerce](#temat-2-przykład-funkcje-wirtualne-w-praktyce) | Payment gateway system, real-world | ✅ | ✅ | ✅ | - |
| 3 | [Virtual vs New](#temat-3-virtual-vs-new) | Override vs new, LSP, anti-patterns | ✅ | ✅ | ✅ | ✅ |
| 4 | [Metody Klasy Object](#temat-4-metody-klasy-object) | ToString, Equals, GetHashCode | ✅ | ✅ | ✅ | ✅ |
| 5 | [Adapter + Factory](#temat-5-adapter--factory-pattern) | Design patterns, payment integration | ✅ | ✅ | ✅ | - |
| 6 | [Default Interface Members](#temat-6-default-interface-members) | C# 8.0+, nowoczesne interfejsy | ✅ | ✅ | ✅ | ✅ |

---

## 🎯 Topologia Nauki

```
Temat 1: Wstęp do Polimorfizmu (fundament)
    ↓
Temat 2: Praktyczny Przykład (e-commerce)
    ↓
Temat 3: Override vs New (uwaga na pułapki!)
    ↓
Temat 4: Metody Object (equality, hashing)
    ↓
Temat 5: Adapter + Factory (design patterns)
    ↓
Temat 6: Nowoczesne C# (evolucja interfejsów)
```

---

## 📖 Szczegóły Tematów

### Temat 1: Funkcje Wirtualne - Wstęp

**Kategoria**: Fundamenty

**Kluczowe Koncepty**:
- Słowo kluczowe `virtual` i `override`
- Late binding vs early binding
- Virtual Method Table (VMT)
- Polimorfizm - wiele form tego samego interfejsu

**Gdzie**: [`_01_virtual_methods_intro/`](_01_virtual_methods_intro/)

**Kod**:
```csharp
public class Animal
{
    public virtual void Speak() { }  // ✅ Może być przesłonięta
}

public class Dog : Animal
{
    public override void Speak()  // ✅ Przesłaniam
    {
        Console.WriteLine("Woof!");
    }
}

Animal animal = new Dog();
animal.Speak();  // Output: "Woof!" - Runtime wola Dog version!
```

---

### Temat 2: Przykład - Funkcje Wirtualne w Praktyce

**Kategoria**: Real-World E-Commerce

**Kluczowe Koncepty**:
- Payment gateway system
- Różne metody płatności z jednym interfejsem
- Polimorfizm w akcji
- Strategia rabatów

**Gdzie**: [`_02_virtual_methods_example/`](_02_virtual_methods_example/)

**Kod**:
```csharp
List<PaymentMethod> payments = new()
{
    new CreditCardPayment(),
    new PayPalPayment(),
    new BitcoinPayment()
};

foreach (var payment in payments)
{
    payment.ProcessPayment();  // Każdy robi coś innego!
}
```

---

### Temat 3: Virtual vs New - Kluczowe Różnice

**Kategoria**: Unikanie Pułapek

**Kluczowe Koncepty**:
- `override` = polimorfizm (virtual dispatch)
- `new` = ukrycie (early binding) - ANTI-PATTERN!
- Liskov Substitution Principle
- Kiedy co używać

**Gdzie**: [`_03_virtual_vs_new/`](_03_virtual_vs_new/)

**Kod**:
```csharp
// ✅ DOBRY - Override (polimorfizm)
public class Dog : Animal
{
    public override void Speak() { }  // Wołuje Dog.Speak
}

// ❌ ZŁY - New (nie polimorfizm!)
public class Cat : Animal
{
    public new void Speak() { }  // Wołuje Animal.Speak!
}

Animal animal = new Cat();
animal.Speak();  // Animal version, nie Cat!
```

---

### Temat 4: Metody Klasy Object

**Kategoria**: Equlity i Hashing

**Kluczowe Koncepty**:
- `ToString()` - reprezentacja tekstowa
- `Equals()` - porównanie równości (wartości vs referencje)
- `GetHashCode()` - hash dla kolekcji
- IEquatable<T>

**Gdzie**: [`_04_object_methods/`](_04_object_methods/)

**Kod**:
```csharp
public class User : IEquatable<User>
{
    public override bool Equals(object? obj)
    {
        return obj is User u && u.Id == Id;  // Wartości, nie referencje
    }
    
    public override int GetHashCode()  // ✅ ZAWSZE razem z Equals!
    {
        return Id.GetHashCode();
    }
}

var users = new HashSet<User> { user1, user1 };  // Duplikat usunięty!
```

---

### Temat 5: Adapter + Factory Pattern

**Kategoria**: Design Patterns

**Kluczowe Koncepty**:
- Adapter - konwersja interfejsów
- Factory - centralne tworzenie
- Unified interface dla różnych providery
- Dependency Injection

**Gdzie**: [`_05_adapter_factory/`](_05_adapter_factory/)

**Kod**:
```csharp
// Factory tworzy właściwy adapter
var gateway = PaymentGatewayFactory.Create("Stripe");

// Unified interface - działa dla wszystkich!
gateway.Authorize(100m);
gateway.Capture();
```

---

### Temat 6: Default Interface Members - C# 8.0+

**Kategoria**: Nowoczesne C#

**Kluczowe Koncepty**:
- Implementacja w interface'ach (C# 8.0+)
- Backward compatibility
- Static members (C# 11+)
- Access modifiers (C# 11+)

**Gdzie**: [`_06_default_interface_members/`](_06_default_interface_members/)

**Kod**:
```csharp
public interface ILogger
{
    void Log(string message);
    
    // ✅ C# 8.0+: Default implementation
    public void LogInfo(string info)
    {
        Console.WriteLine($"[INFO] {info}");
    }
}

// Klasy nie muszą implementować LogInfo!
public class ConsoleLogger : ILogger
{
    public void Log(string message) { }  // Tylko to musimy implementować
}
```

---

## 💻 Jak Pracować

### Dla każdego tematu:

```bash
# 1. Wejdź do katalogu kodu
cd _01_virtual_methods_intro/code/

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
06-polimorfizm/
├── README.md                    # Ten plik
├── _01_virtual_methods_intro/
│   ├── README.md               # Wyjaśnienia
│   ├── code/
│   │   ├── VirtualMethodsIntro.csproj
│   │   └── Program.cs          # Demo + xUnit testy
│   ├── diagrams/
│   │   └── 01-virtual-methods-intro.mermaid
│   └── tasks/
│       └── README.md           # Zadania dla studentów
├── _02_virtual_methods_example/
│   └── [identyczna struktura]
├── ... (tematy 3-6)
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
| Tematy | 6 |
| README.md | 7 (6 + 1 główny) |
| Program.cs | 6 (każdy z testami xUnit) |
| .csproj | 6 (wszystkie .NET 9.0) |
| Diagramy Mermaid | 6+ |
| Zadania | 15+ ćwiczeń |
| Testy xUnit | 50+ testów |
| Linii kodu C# | 2000+ |
| Dokumentacja | 12000+ słów (PL) |

---

## 🎓 Oczekiwane Efekty Uczenia

Po ukończeniu tego modułu student powinien:

- [ ] Rozumieć `virtual` methods i `override`
- [ ] Znać różnicę między `override` i `new`
- [ ] Stosować polimorfizm w praktyce
- [ ] Implementować `ToString()`, `Equals()`, `GetHashCode()`
- [ ] Używać `HashSet` i `Dictionary` z custom objects
- [ ] Znać pattern Adapter
- [ ] Znać pattern Factory
- [ ] Pracować z default interface members (C# 8.0+)
- [ ] Projektować systemy z polimorfizmem
- [ ] Zrozumieć Liskov Substitution Principle

---

## 📚 Referencje i Zasoby

### Microsoft Learn
- [Virtual and Override Keywords](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [Object Members](https://learn.microsoft.com/en-us/dotnet/api/system.object)
- [Default Interface Members](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/default-interface-members-versions)

### Design Patterns
- [Adapter Pattern](https://refactoring.guru/design-patterns/adapter)
- [Factory Pattern](https://refactoring.guru/design-patterns/factory-method)

### SOLID Principles
- [Liskov Substitution Principle](https://en.wikipedia.org/wiki/Liskov_substitution_principle)

### Książki
- **C# Player's Guide** - RB Whitaker (Chapt. 16-18: Polymorphism)
- **CLR via C#** - Jeffrey Richter (Chapt. 12: Interfaces)
- **Design Patterns** - Gang of Four (Adapter, Factory)

---

## ✅ Checklist dla Nauczycieli

Przed pierwszym wykładem:

- [ ] Przeczytaj README każdego tematu
- [ ] Uruchom `dotnet run` w każdym temacie
- [ ] Przejrzyj diagramy (Mermaid)
- [ ] Przygotuj pytania do studentów
- [ ] Test projektor/HDMI połączenie
- [ ] Przygotuj live-coding examples
- [ ] Skopiuj zadania do LMS (jeśli używasz)

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
- [ ] Widzę praktyczne zastosowania

---

## 🚀 Quick Start

Chcesz szybko zacząć? Oto najkrótszy możliwy początek:

```bash
# Temat 1: Virtual Methods Intro
cd _01_virtual_methods_intro/code
dotnet run
dotnet test

# Temat 2: E-Commerce Payment System
cd ../../_02_virtual_methods_example/code
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
| 05-Dziedziczenie | 7 | 1500+ | 35+ | 20+ | ✅ 100% |
| 06-Polimorfizm | 6 | 2000+ | 50+ | 15+ | ✅ 100% |

---

## 📝 Historia Zmian

- **2024-08-30**: v1.0 - Moduł kompletny, 6 tematów, wszystkie budują ✅

---

## 🏆 Podsumowanie

**Moduł 06-Polimorfizm to kompleksowy zestaw materiałów edukacyjnych** do nauczania polimorfizmu, wirtualnych metod i nowoczesnych C# features.

- ✅ **6 tematów** od podstaw do zaawansowanych koncepcji
- ✅ **2000+ linii kodu** działającego i przetestowanego
- ✅ **15+ ćwiczeń** dla studentów
- ✅ **50+ testów** xUnit
- ✅ **6+ diagramów** UML/Mermaid
- ✅ **Real-world example**: E-commerce payment system
- ✅ **Nowoczesne C# 8.0+** features
- ✅ **Design patterns**: Adapter, Factory
- ✅ **Gotowy do użycia** w nauczaniu

**Status**: 🟢 **GOTOWY DO NAUCZANIA**

---

*Stworzone dla uniwersytetu, nauczycieli i studentów C#*

*Ostatnia aktualizacja: 2024-08-30*
