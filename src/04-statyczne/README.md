# MODUŁ 04-STATYCZNE: Składowe Statyczne w C#

## 📚 Wprowadzenie

Moduł kompleksowo omawia **składowe statyczne (static members)** w C# - pola, metody, konstruktory, klasy statyczne, metody rozszerzające i wzorce projektowe.

Od podstawowych pól statycznych, przez utility funkcje, aż po nowoczesne metody rozszerzające i lazy initialization.

---

## 📋 Tematy (7 wykładów)

| # | Temat | Opis | Status | Code | Testy | Zadania |
|---|-------|------|--------|------|-------|---------|
| 1 | [Pole statyczne](#temat-1-pole-statyczne) | Zmienne dzielane między instancje | ✅ | ✅ | ✅ | ✅ |
| 2 | [Metody statyczne](#temat-2-metody-statyczne) | Funkcje utility, factory methods | ✅ | ✅ | ✅ | ✅ |
| 3 | [Konstruktor statyczny](#temat-3-konstruktor-statyczny) | Inicjalizacja statycznych pól | ✅ | ✅ | ✅ | ✅ |
| 4 | [Klasy statyczne](#temat-4-klasy-statyczne) | Tylko statyczne członkowie | ✅ | ✅ | ✅ | ✅ |
| 5 | [Metody rozszerzające](#temat-5-metody-rozszerzające) | Extension Methods - dodawanie metod do typów | ✅ | ✅ | ✅ | ✅ |
| 6 | [Wzorzec Singleton](#temat-6-wzorzec-singleton) | Gwarantowanie jednej instancji | ✅ | ✅ | ✅ | ✅ |
| 7 | [Static Properties & Lazy<T>](#temat-7-static-properties--lazyt) | Nowoczesna inicjalizacja (C# 9+) | ✅ | ✅ | ✅ | ✅ |

---

## 🎯 Topologia Nauki

```
Temat 1: Pole statyczne (foundation)
    ↓
Temat 2: Metody statyczne (utility)
    ↓
Temat 3: Konstruktor statyczny (initialization)
    ↓
Temat 4: Klasy statyczne (organization)
    ↓
Temat 5: Metody rozszerzające (advanced)
    ↓
Temat 6: Singleton (pattern)
    ↓
Temat 7: Static Properties & Lazy<T> (modern)
```

---

## 📖 Szczegóły Tematów

### Temat 1: Pole statyczne

**Kategoria**: Fundamenty

**Kluczowe Koncepty**:
- Pola należące do klasy, nie do instancji
- Dzielone między wszystkie instancje
- Liczniki, konfiguracja, cache
- Thread safety considerations

**Gdzie**: [`_01_static_fields/`](_01_static_fields/)

**Kod**:
```csharp
public class Counter
{
    public static int TotalCount = 0;  // Dzielone!
    
    public Counter()
    {
        TotalCount++;  // Każda instancja inkrementuje
    }
}
```

---

### Temat 2: Metody statyczne

**Kategoria**: Utility i Helper

**Kluczowe Koncepty**:
- Metody bez potrzeby instancji
- Utility functions (Math, String, Conversion)
- Factory methods
- Brak dostępu do pól instancji

**Gdzie**: [`_02_static_methods/`](_02_static_methods/)

**Kod**:
```csharp
public static class StringHelper
{
    public static string Reverse(string str)
    {
        char[] chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

// Użycie
StringHelper.Reverse("hello");  // "olleh"
```

---

### Temat 3: Konstruktor statyczny

**Kategoria**: Inicjalizacja

**Kluczowe Koncepty**:
- Wywoływany raz przed pierwszym użyciem
- Inicjalizacja statycznych pól
- Ładowanie konfiguracji
- Singleton pattern foundation

**Gdzie**: [`_03_static_constructor/`](_03_static_constructor/)

**Kod**:
```csharp
public class Database
{
    public static string ConnectionString;
    
    static Database()  // Konstruktor statyczny
    {
        ConnectionString = Environment.GetEnvironmentVariable("DB") ?? "default";
    }
}
```

---

### Temat 4: Klasy statyczne

**Kategoria**: Organizacja kodu

**Kluczowe Koncepty**:
- Zawierają tylko statyczne członkowie
- Nie można instancjonować
- Utility classes (Math, File, Path)
- Organizacja helper funkcji

**Gdzie**: [`_04_static_classes/`](_04_static_classes/)

**Kod**:
```csharp
public static class MathHelper
{
    public const double PI = 3.14159;
    
    public static double CircleArea(double radius)
    {
        return PI * radius * radius;
    }
}

// Nie można: new MathHelper()
```

---

### Temat 5: Metody rozszerzające

**Kategoria**: Zaawansowany C#

**Kluczowe Koncepty**:
- Extension Methods - rozszerzanie istniejących typów
- `this` jako pierwszy parametr
- Fluent interfaces
- LINQ foundations

**Gdzie**: [`_05_extension_methods/`](_05_extension_methods/)

**Kod**:
```csharp
public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        return char.ToUpper(str[0]) + str.Substring(1);
    }
}

// Użycie jak metoda instancji!
"hello".Capitalize();  // "Hello"
```

---

### Temat 6: Wzorzec Singleton

**Kategoria**: Design Pattern

**Kluczowe Koncepty**:
- Gwarantowanie jednej instancji klasy
- Prywatny konstruktor
- Double-checked locking
- Thread-safe patterns

**Gdzie**: [`_06_singleton_pattern/`](_06_singleton_pattern/)

**Kod**:
```csharp
public sealed class Logger
{
    private static readonly Lazy<Logger> _instance = 
        new(() => new Logger());
    
    private Logger() { }
    
    public static Logger Instance => _instance.Value;
}
```

---

### Temat 7: Static Properties & Lazy<T>

**Kategoria**: Nowoczesny C# (C# 9+)

**Kluczowe Koncepty**:
- Static properties - kontrola dostępu
- Lazy<T> - inicjalizacja leniwa
- Thread-safe initialization
- Performance optimization

**Gdzie**: [`_07_static_properties_lazy/`](_07_static_properties_lazy/)

**Kod**:
```csharp
public class AppConfig
{
    public static string ApiUrl { get; set; } = "http://localhost";
}

public class ExpensiveResource
{
    private static readonly Lazy<ExpensiveResource> _instance =
        new(() => new ExpensiveResource());
    
    public static ExpensiveResource Instance => _instance.Value;
}
```

---

## 💻 Jak Pracować

### Dla każdego tematu:

```bash
# 1. Wejdź do katalogu kodu
cd _01_static_fields/code/

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
04-statyczne/
├── README.md                    # Ten plik
├── _01_static_fields/
│   ├── README.md               # Wyjaśnienia
│   ├── code/
│   │   ├── StaticFields.csproj
│   │   └── Program.cs          # Demo + xUnit testy
│   ├── diagrams/
│   │   └── 01-static-fields.mermaid
│   └── tasks/
│       └── README.md           # Zadania dla studentów
├── _02_static_methods/
│   └── [identyczna struktura]
├── ... (tematy 3-7)
└── [status plik w /status/]
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
| Zadania | 15+ ćwiczeń |
| Testy xUnit | 35+ testów |
| Linii kodu C# | 1500+ |
| Dokumentacja | 8000+ słów (PL) |

---

## 🎓 Oczekiwane Efekty Uczenia

Po ukończeniu tego modułu student powinien:

- [ ] Rozumieć pola statyczne i ich zastosowania
- [ ] Pisać metody statyczne do utility funkcji
- [ ] Implementować konstruktory statyczne
- [ ] Organizować kod w klasy statyczne
- [ ] Tworzyć metody rozszerzające
- [ ] Implementować Singleton pattern
- [ ] Stosować Lazy<T> w nowoczesnym C#
- [ ] Bezpiecznie pracować ze statycznymi zasobami

---

## 📚 Referencje i Zasoby

### Microsoft Learn
- [Static Members](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-members)
- [Static Classes and Class Members](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members)
- [Extension Methods](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/extension-methods)
- [Lazy<T> Class](https://learn.microsoft.com/en-us/dotnet/api/system.lazy-1)

### Książki
- **C# Player's Guide** - RB Whitaker
- **CLR via C#** - Jeffrey Richter
- **C# 11 in a Nutshell** - Joseph Albahari

### Online
- Stack Overflow: `[c#] static` tags
- YouTube: "C# Static Members Tutorial"
- GitHub: Open source C# projects

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

---

## ✅ Checklist dla Studentów

Po każdym temacie:

- [ ] Przeczytałem README.md
- [ ] Rozumiem diagramy
- [ ] Zagrałem demo (`dotnet run`)
- [ ] Analizowałem kod
- [ ] Zrobiłem przynajmniej 2 zadania
- [ ] Uruchomiłem testy (`dotnet test`)
- [ ] Mogę wyjaśnić główne koncepty

---

## 🚀 Quick Start

Chcesz szybko zacząć? Oto najkrótszy możliwy początek:

```bash
# Temat 1: Pole statyczne
cd _01_static_fields/code
dotnet run
dotnet test

# Temat 2: Metody statyczne
cd ../../_02_static_methods/code
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

---

## 📝 Historia Zmian

- **2024-08-30**: v1.0 - Moduł kompletny, 7 tematów, wszystkie budują ✅

---

## 🏆 Podsumowanie

**Moduł 04-Statyczne to kompleksowy zestaw materiałów edukacyjnych** do nauczania składowych statycznych, metod rozszerzających i wzorców projektowych w C#.

- ✅ **7 tematów** od podstaw do nowoczesnego C#
- ✅ **1500+ linii kodu** działającego i przetestowanego
- ✅ **15+ ćwiczeń** dla studentów
- ✅ **35+ testów** xUnit
- ✅ **7 diagramów** UML
- ✅ **Gotowy do użycia** w nauczaniu

**Status**: 🟢 **GOTOWY DO NAUCZANIA**

---

*Stworzone dla uniwersytetu, nauczycieli i studentów C#*

*Ostatnia aktualizacja: 2024-08-30*
