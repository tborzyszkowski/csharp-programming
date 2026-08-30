# MODUŁ 07-INTERFEJSY_ABSTRAKCJE: Abstrakcja i Interfejsy w C#

## 📚 Wprowadzenie

Moduł kompleksowo omawia **abstrakcję i interfejsy** - fundamentalne koncepty OOP pozwalające na projektowanie elastycznych, testowalnych systemów.

Od klas abstrakcyjnych, przez interfejsy, aż po nowoczesne C# 8.0+ features.

---

## 📋 Tematy (7 wykładów)

| # | Temat | Opis | Topics |
|---|-------|------|--------|
| 1 | [Klasy Abstrakcyjne: Wstęp](#temat-1-klasy-abstrakcyjne-wstęp) | abstract, konwencja Base, Shape library | 7 |
| 2 | [Metody Abstrakcyjne](#temat-2-metody-abstrakcyjne) | abstract vs virtual, ograniczenia | 7 |
| 3 | [Sealed Keyword](#temat-3-sealed-keyword) | sealed na klasach, metodach, właściwościach | 7 |
| 4 | [Interfejsy: Wstęp](#temat-4-interfejsy-wstęp) | budowa, DI, lose coupling, vs Java | 8 |
| 5 | [Jawna Implementacja](#temat-5-jawna-implementacja-interfejsu) | implicit/explicit, gdy używać, real-world | 7 |
| 6 | [Konwersje Operatory](#temat-6-konwersje-i-operatory-castingu) | (), is, as, pattern matching | 8 |
| 7 | [Zaawansowane Interfejsy](#temat-7-zaawansowane-interfejsy) | C# 8.0+ default, C# 11+ static abstract | 9 |

---

## 🎯 Topologia Nauki

```
Temat 1: Klasy Abstrakcyjne (fundament)
    ↓
Temat 2: Metody Abstrakcyjne (vs virtual)
    ↓
Temat 3: Sealed Keyword (kontrola dziedziczenia)
    ↓
Temat 4: Interfejsy (kontrakt, DI)
    ↓
Temat 5: Jawna Implementacja (zaawansowane)
    ↓
Temat 6: Konwersje (is, as, pattern matching)
    ↓
Temat 7: Zaawansowane Interfejsy (C# 8.0+, 11+)
```

---

## 📖 Szczegóły Tematów

### Temat 1: Klasy Abstrakcyjne - Wstęp

**Kategoria**: Fundamenty

**Kluczowe Koncepty**:
- Słowo kluczowe `abstract`
- Konwencja nazewnictwa: `Base`
- Metody konkretne i abstrakcyjne w klasie abstrakcyjnej
- Polimorfizm z klasami abstrakcyjnymi

**Gdzie**: [`_01_abstract_classes_intro/`](_01_abstract_classes_intro/)

**Przykłady**:
- Animal hierarchy (Dog, Cat)
- Shape library (Circle, Rectangle, Triangle - geometria!)
- Payment processing (CreditCard, PayPal)

**Testy**: 7 xUnit facts

---

### Temat 2: Metody Abstrakcyjne

**Kategoria**: Ograniczenia i kontrakty

**Kluczowe Koncepty**:
- Abstrakcyjna metoda = tylko sygnatura, brak implementacji
- MUSZĄ być implementowane w podklasach
- Abstract vs Virtual - różnice
- Łańcuch abstrakcji (chain of abstractions)

**Gdzie**: [`_02_abstract_methods/`](_02_abstract_methods/)

**Przykłady**:
- Payment processing (Process())
- Document operations (Open, Save, Close)
- Repository pattern (Add, Remove, GetById)

**Testy**: 7 xUnit facts

---

### Temat 3: Sealed Keyword

**Kategoria**: Kontrola dziedziczenia

**Kluczowe Koncepty**:
- `sealed class` - niemożna dziedziczyć
- `sealed override` - niemożna override'ować
- Security-critical operations
- Performance considerations

**Gdzie**: [`_03_sealed_keyword/`](_03_sealed_keyword/)

**Przykłady**:
- Sealed classes (SpecialOffer)
- Sealed methods (Validate() w PdfDocument)
- Encryption classes (AES, security)

**Testy**: 7 xUnit facts

---

### Temat 4: Interfejsy - Wstęp

**Kategoria**: Dependency Injection, Loose Coupling

**Kluczowe Koncepty**:
- Interfejs = kontrakt, WSZYSTKIE elementy public
- C# interfejsy vs Java interfejsy
- Dependency Injection (DI)
- Loose coupling vs tight coupling
- Polimorfizm z interfejsami

**Gdzie**: [`_04_interfaces_intro/`](_04_interfaces_intro/)

**Kod**:
```csharp
// ✅ DI - loose coupling
public class App
{
    private ILogger _logger;
    
    public App(ILogger logger)  // Constructor injection
    {
        _logger = logger;
    }
}

// ✅ Can swap implementations easily
App app1 = new App(new ConsoleLogger());
App app2 = new App(new FileLogger());
```

**Testy**: 8 xUnit facts

---

### Temat 5: Jawna Implementacja Interfejsu

**Kategoria**: Zaawansowane techniki

**Kluczowe Koncepty**:
- Implicit implementation (public)
- Explicit implementation (IInterface.Method)
- Kiedy używać explicit
- Różne implementacje dla tej samej metody

**Gdzie**: [`_05_explicit_implementation/`](_05_explicit_implementation/)

**Kod**:
```csharp
public class AnimatedShape : IShapable, IAnimatable
{
    // ✅ Implicit - dla IShapable
    public void Draw() { }
    
    // ✅ Explicit - dla IAnimatable
    void IAnimatable.Draw() { }
    
    // Dostęp:
    shape.Draw();              // IShapable
    ((IAnimatable)shape).Draw(); // IAnimatable
}
```

**Testy**: 7 xUnit facts

---

### Temat 6: Konwersje i Operatory Castingu

**Kategoria**: Type Safety, Pattern Matching

**Kluczowe Koncepty**:
- Cast operator `()`  - throws if wrong
- `is` operator - safe type check
- `as` operator - safe cast, returns null
- Pattern matching (C# 7.0+)
- Not pattern (C# 9.0+)

**Gdzie**: [`_06_casting_operators/`](_06_casting_operators/)

**Kod**:
```csharp
// ✅ Pattern matching with cast (modern!)
if (animal is Dog dog)
{
    dog.Fetch();
}

// ✅ Not pattern
if (animal is not Cat) { }
```

**Testy**: 8 xUnit facts

---

### Temat 7: Zaawansowane Interfejsy (C# 8.0+)

**Kategoria**: Nowoczesne C#

**Kluczowe Koncepty**:
- Default interface members (C# 8.0)
- Static abstract members (C# 11)
- Access modifiers (C# 11)
- Interface versioning (backward compatibility)

**Gdzie**: [`_07_advanced_interfaces/`](_07_advanced_interfaces/)

**Kod**:
```csharp
// ✅ C# 8.0+: Default implementation
public interface ILogger
{
    void Log(string msg);
    
    public void LogInfo(string info)
    {
        Console.WriteLine($"[INFO] {info}");
    }
}

// ✅ C# 11+: Static abstract
public interface IConverter
{
    static abstract double Convert(double value);
}

// ✅ C# 11+: Private methods
public interface IService
{
    private void LogInternal(string msg) { }
}
```

**Testy**: 9 xUnit facts

---

## 🛠️ Jak Pracować

### Dla każdego tematu:

```bash
# 1. Wejdź do katalogu kodu
cd _01_abstract_classes_intro/code/

# 2. Uruchom demonstrację
dotnet run

# 3. Uruchom testy
dotnet test

# 4. Otwórz kod w VS Code
code Program.cs
```

### Struktura projektu:

```
07-interfejsy_abstrakcje/
├── README.md                        # Ten plik
├── _01_abstract_classes_intro/
│   ├── README.md
│   ├── code/
│   │   ├── AbstractClassesIntro.csproj
│   │   └── Program.cs              # Code + 7 tests
│   ├── diagrams/
│   │   └── *.mermaid
│   └── tasks/
│       └── README.md               # Student exercises
├── _02_abstract_methods/
├── _03_sealed_keyword/
├── _04_interfaces_intro/
├── _05_explicit_implementation/
├── _06_casting_operators/
├── _07_advanced_interfaces/
└── [all with identical structure]
```

---

## 🔧 Technologia

| Komponent | Wersja | Dlaczego |
|-----------|--------|---------|
| .NET SDK | 9.0+ | Nowsze API, performance |
| C# | 13 (latest) | Pełne wsparcie C# 8.0+, 11+ features |
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
| Testy xUnit | 52 testów |
| Linii kodu C# | 2500+ |
| Dokumentacja | 6000+ słów (PL) |

---

## 🎓 Oczekiwane Efekty Uczenia

Po ukończeniu tego modułu student powinien:

- [ ] Projektować klasy abstrakcyjne (`abstract`)
- [ ] Rozumieć konwencję `Base` w nazewnictwie
- [ ] Implementować metody abstrakcyjne
- [ ] Znać różnicę między `abstract` a `virtual`
- [ ] Używać `sealed` do kontroli dziedziczenia
- [ ] Projektować interfejsy (kontrakt)
- [ ] Wdrażać DI (Dependency Injection)
- [ ] Rozumieć loose coupling
- [ ] Implementować jawnie interfejsy (`IInterface.Method`)
- [ ] Używać operatorów castingu (`()`, `is`, `as`)
- [ ] Pracować z pattern matching (C# 7.0+)
- [ ] Wdrażać C# 8.0+ default interface members
- [ ] Wdrażać C# 11+ static abstract members
- [ ] Projektować wersjonowane interfejsy (backward compatibility)

---

## 📚 Referencje i Zasoby

### Microsoft Learn
- [Abstract Classes and Sealed Classes](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/abstract)
- [Interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces)
- [Default Interface Members](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/default-interface-members-versions)
- [Pattern Matching](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/pattern-matching)

### Wideo/Kursy
- [Microsoft Learn: OOP Concepts](https://learn.microsoft.com/en-us/training/modules/object-oriented-programming-csharp/)

### Książki
- **C# Player's Guide** - RB Whitaker (Chapt. 19-21)
- **CLR via C#** - Jeffrey Richter (Chapt. 12: Interfaces)
- **Dependency Injection Principles, Practices, and Patterns** - Steven van Deursen, Mark Seemann

---

## ✅ Checklist dla Nauczycieli

Przed pierwszym wykładem:

- [ ] Przeczytaj README każdego tematu
- [ ] Uruchom `dotnet run` w każdym temacie
- [ ] Przejrzyj diagramy (Mermaid)
- [ ] Przygotuj pytania do studentów
- [ ] Test projektor/HDMI
- [ ] Przygotuj live-coding examples
- [ ] Skopiuj zadania do LMS (jeśli używasz)

---

## ✅ Checklist dla Studentów

Po każdym temacie:

- [ ] Przeczytałem README.md
- [ ] Rozumiem koncepty i diagramy
- [ ] Zagrałem demo (`dotnet run`)
- [ ] Analizowałem kod w detail
- [ ] Zrobiłem przynajmniej 2 zadania
- [ ] Uruchomiłem testy (`dotnet test`)
- [ ] Mogę wyjaśnić główne koncepty
- [ ] Widzę praktyczne zastosowania

---

## 🚀 Quick Start

```bash
# Temat 1: Abstract Classes
cd _01_abstract_classes_intro/code
dotnet run
dotnet test

# Temat 4: Interfaces
cd ../../_04_interfaces_intro/code
dotnet run
dotnet test

# ...i tak dalej dla każdego tematu
```

---

## 💡 Porównanie Z Poprzednimi Modułami

| Moduł | Tematy | Kod | Testy | Status |
|-------|--------|-----|-------|--------|
| 05-Dziedziczenie | 7 | 1500+ | 35+ | ✅ 100% |
| 06-Polimorfizm | 6 | 2000+ | 50+ | ✅ 100% |
| **07-Interfejsy** | **7** | **2500+** | **52** | **✅ 100%** |

---

## 🏆 Podsumowanie

**Moduł 07-Interfejsy_Abstrakcje to kompleksowy zestaw materiałów edukacyjnych** do nauczania abstrakcji, interfejsów i nowoczesnych C# features.

- ✅ **7 tematów** od podstaw do zaawansowanych koncepcji
- ✅ **2500+ linii kodu** działającego i przetestowanego
- ✅ **52 testów** xUnit
- ✅ **6000+ słów** dokumentacji (PL)
- ✅ **Real-world example**: Shape library, Payment system, DI
- ✅ **Nowoczesne C# 8.0+** features (default interface members)
- ✅ **C# 11+ features** (static abstract members, private methods)
- ✅ **Design patterns**: DI, Loose coupling, Repository
- ✅ **Gotowy do użycia** w nauczaniu

**Status**: 🟢 **GOTOWY DO NAUCZANIA**

---

## 📝 Historia Zmian

- **2024-08-30**: v1.0 - Moduł kompletny, 7 tematów, wszystkie budują ✅

---

*Stworzone dla uniwersytetu, nauczycieli i studentów C#*

*Ostatnia aktualizacja: 2024-08-30*
