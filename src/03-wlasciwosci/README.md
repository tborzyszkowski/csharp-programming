# MODUŁ 03-WŁAŚCIWOŚCI: Właściwości i Indeksatory w C#

## 📚 Wprowadzenie

Moduł kompleksowo omawia **właściwości (properties)** i **indeksatory (indexers)** w C# - kluczowe koncepcje dla nowoczesnego programowania obiektowego.

Od podstawowych pól i właściwości, przez walidację i enkapsulację, aż po nowoczesne cechy C# 8+ i 11+ (nullable reference types, init properties, required keyword).

---

## 📋 Tematy (7 wykładów)

| # | Temat | Opis | Chwilowo | Code | Testy | Zadania |
|---|-------|------|----------|------|-------|---------|
| 1 | [Właściwości vs Pola](#temat-1-właściwości-vs-pola) | Encapsulation, gettery/settery | ✅ | ✅ | ✅ | ✅ |
| 2 | [Auto Properties](#temat-2-auto-properties) | `{ get; set; }` - składnia (C# 3+) | ✅ | ✅ | ✅ | ✅ |
| 3 | [Walidacja Właściwości](#temat-3-walidacja-właściwości) | Logika w setterze, exceptions, events | ✅ | ✅ | ✅ | ✅ |
| 4 | [Indeksatory](#temat-4-indeksatory) | `this[...]` - dostęp jak tablica/słownik | ✅ | ✅ | ✅ | ✅ |
| 5 | [Readonly vs Const](#temat-5-readonly-vs-const) | Stałe kompilacji vs runtime | ✅ | ✅ | ✅ | ✅ |
| 6 | [Init Properties (C# 9+)](#temat-6-init-properties-c-9) | `{ get; init; }`, `required` keyword (C# 11+) | ✅ | ✅ | ✅ | ✅ |
| 7 | [Nullable Reference Types (C# 8+)](#temat-7-nullable-reference-types-c-8) | `string?`, null-safety, null-checks | ✅ | ✅ | ✅ | ✅ |

---

## 🎯 Topologia Nauki

```
Temat 1: Właściwości vs Pola (foundation)
    ↓
Temat 2: Auto Properties (simplification)
    ↓
Temat 3: Walidacja (logic)
    ↓
Temat 4: Indeksatory (advanced access)
    ↓
Temat 5: Readonly vs Const (data protection)
    ↓
Temat 6: Init Properties (C# 9+ modern)
    ↓
Temat 7: Nullable Reference Types (C# 8+ safety)
```

---

## 📖 Szczegóły Tematów

### Temat 1: Właściwości vs Pola

**Kategoria**: Fundamenty OOP

**Kluczowe Koncepty**:
- Pola (fields) - zmienne publiczne (❌ UNIKAJ)
- Właściwości (properties) - kontrolowany dostęp
- Enkapsulacja - ukrywanie szczegółów implementacji
- Read-only, write-only, asymetryczne accessory

**Gdzie**: [`_01_properties_vs_fields/`](_01_properties_vs_fields/)

**Przykład Kodu**:
```csharp
public class Person
{
    private int _age;
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value >= 0 && value <= 150)
                _age = value;
        }
    }
}
```

---

### Temat 2: Auto Properties

**Kategoria**: Uproszczenie (C# 3+)

**Kluczowe Koncepty**:
- Auto properties - kompilator generuje backing field
- `{ get; set; }` - standardowa składnia
- Inicjalizacja wartości domyślnych
- Asymetryczne accessory: `public get; private set;`

**Gdzie**: [`_02_auto_properties/`](_02_auto_properties/)

**Kod**:
```csharp
public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
```

---

### Temat 3: Walidacja Właściwości

**Kategoria**: Logika i Ochrona Danych

**Kluczowe Koncepty**:
- Backing field pattern
- Walidacja w setterze
- Throw exceptions vs silent ignore
- PropertyChanged events
- Fail-fast principle

**Gdzie**: [`_03_property_validation/`](_03_property_validation/)

**Kod**:
```csharp
public int Age
{
    set
    {
        if (value < 0)
            throw new ArgumentException("Age cannot be negative");
        _age = value;
    }
}
```

---

### Temat 4: Indeksatory

**Kategoria**: Zaawansowany Dostęp

**Kluczowe Koncepty**:
- Indeksatory - `this[...]` indexer syntax
- Sekwencyjne indeksatory (int)
- Nienazwane indeksatory (string)
- Wielowymiarowe indeksatory `[row, col]`
- Tablice asocjacyjne

**Gdzie**: [`_04_indexers/`](_04_indexers/)

**Kod**:
```csharp
public string this[int index]
{
    get { return items[index]; }
    set { items[index] = value; }
}

public object? this[string propertyName]
{
    get { /* ... */ }
    set { /* ... */ }
}
```

---

### Temat 5: Readonly vs Const

**Kategoria**: Ochrona Danych

**Kluczowe Koncepty**:
- `const` - stałe czasu kompilacji
- `readonly` - stałe czasu runtime
- Statyczne readonly - singleton pattern
- Readonly collections

**Gdzie**: [`_05_readonly_const/`](_05_readonly_const/)

**Kod**:
```csharp
public const double PI = 3.14159;  // Compile-time
public readonly string ConfigPath;  // Runtime

static readonly Database Instance = new();  // Singleton
```

---

### Temat 6: Init Properties (C# 9+)

**Kategoria**: Nowoczesny C# (C# 9+, 11+)

**Kluczowe Koncepty**:
- Init-only properties - `{ get; init; }` (C# 9+)
- Immutability - niemożliwość zmiany po created
- `required` keyword (C# 11+)
- Kombinacja init + required dla bezpieczeństwa
- DTOs (Data Transfer Objects)

**Gdzie**: [`_06_init_properties/`](_06_init_properties/)

**Kod**:
```csharp
public class Person
{
    public required string Name { get; init; }
    public int Age { get; init; }
}

var person = new Person { Name = "John", Age = 30 };
// person.Name = "Jane";  // BŁĄD - init-only
```

---

### Temat 7: Nullable Reference Types (C# 8+)

**Kategoria**: Bezpieczeństwo Null (C# 8+)

**Kluczowe Koncepty**:
- Nullable reference types - `string?` (C# 8+)
- Non-nullable by default - `string` (nie null)
- Null-safety - compile-time checking
- Null-coalescing operator `??`
- Null-safe operator `?.`
- Nullable value types `int?`

**Gdzie**: [`_07_nullable_patterns/`](_07_nullable_patterns/)

**Kod**:
```csharp
#nullable enable

public class Person
{
    public string Name { get; set; }       // Non-nullable
    public string? Phone { get; set; }     // Nullable
}

var phone = person.Phone ?? "N/A";  // Null-coalescing
```

---

## 💻 Jak Pracować

### Dla każdego tematu:

```bash
# 1. Wejdź do katalogu kodu
cd _01_properties_vs_fields/code/

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
03-wlasciwosci/
├── README.md                    # Ten plik
├── _01_properties_vs_fields/
│   ├── README.md               # Wyjaśnienia
│   ├── code/
│   │   ├── PropertiesVsFields.csproj
│   │   └── Program.cs          # Demo + xUnit testy
│   ├── diagrams/
│   │   └── 01-properties-vs-fields.mermaid
│   └── tasks/
│       └── README.md           # Zadania dla studentów
├── _02_auto_properties/
│   └── [identyczna struktura]
├── ... (tematy 3-7)
└── COMPLETION_STATUS.md        # Finalne podsumowanie
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
| Program.cs | 7 (każdy z xUnit testami) |
| .csproj | 7 (wszystkie .NET 9.0) |
| Diagramy Mermaid | 7+ |
| Zadania | 20+ ćwiczeń |
| Testy xUnit | 45+ |
| Linii kodu | 2500+ |
| Słów dokumentacji | 12000+ |

---

## 🎓 Oczekiwane Efekty Uczenia

Po ukończeniu tego modułu student powinien:

- [ ] Rozumieć różnicę między polami a właściwościami
- [ ] Pisać auto properties z `{ get; set; }`
- [ ] Implementować walidację w setterach
- [ ] Tworzyć indeksatory dla custom collections
- [ ] Rozróżniać const (compile-time) od readonly (runtime)
- [ ] Używać init properties (C# 9+) i `required` keyword (C# 11+)
- [ ] Działać z nullable reference types (C# 8+)
- [ ] Pisać bezpieczny kod chroniący przed `NullReferenceException`

---

## 📚 Referencje i Zasoby

### Microsoft Learn
- [Properties (C#) - Dokumentacja](https://learn.microsoft.com/en-us/dotnet/csharp/properties)
- [Indexers - C# Guide](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/indexers/)
- [Nullable reference types - C# 8+](https://learn.microsoft.com/en-us/dotnet/csharp/nullable-references)
- [Init-only properties - C# 9+](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#init-only-properties)
- [Required keyword - C# 11+](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required)

### Książki
- **C# Player's Guide** - RB Whitaker (Properties & Encapsulation)
- **CLR via C#** - Jeffrey Richter (Type Safety & Properties)
- **C# 11 in a Nutshell** - Joseph Albahari (Modern Features)

### Online
- Stack Overflow: `[c#] properties` tags
- YouTube: "C# Properties and Indexers Tutorial"
- GitHub: Open source projects using properties

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
# Temat 1: Właściwości vs Pola
cd _01_properties_vs_fields/code
dotnet run
dotnet test

# Temat 2: Auto Properties
cd ../../_02_auto_properties/code
dotnet run
dotnet test

# ... i tak dalej dla każdego tematu
```

---

## 💡 Wskazówki dla Nauczycieli

### Prezentacja
- Zaczynaj od simplifications (auto properties) przed złożonością (validation)
- Pokaż real-world examples (DTOs, configuration classes)
- Demo live coding - zmień walidację i uruchom ponownie

### Interakcja
- Pytaj: "Co się stanie jeśli ustawimy age = -5?"
- Proś studentów o zmianę kodu i obserwowanie rezultatów
- Dyskutuj Trade-offs (safety vs performance)

### Łożyska
- Tematy 1-5 są fundamentalne
- Tematy 6-7 to nowoczesne best practices
- Połącz z poprzednim modułem (01-klasy) dla kontekstu

---

## 📞 Wsparcie

Masz pytania? Sprawdź:

1. README.md konkretnego tematu
2. Kod w `Program.cs` - zawiera examples
3. Diagramy `*.mermaid` - wizualne wyjaśnienia
4. Zadania w `tasks/` - practice exercises
5. Microsoft Learn documentation - official resource

---

## 📈 Przyszłe Rozszerzenia (Opcjonalne)

- [ ] Data Annotations validation
- [ ] Property-like backing fields (records)
- [ ] Reflection over properties
- [ ] LINQ with indexers
- [ ] Performance analysis (properties vs fields)
- [ ] Video tutorials
- [ ] Interactive quizzes

---

## 📝 Historia Zmian

- **2024-08-30**: v1.0 - Moduł kompletny, 7 tematów, wszystkie budują ✅

---

## 🏆 Podsumowanie

**Moduł 03-Właściwości to kompleksowy zestaw materiałów edukacyjnych** do nauczania właściwości, indeksatorów i nowoczesnych cech C# (8+, 9+, 11+).

- ✅ **7 tematów** od podstaw do nowoczesnych praktyk
- ✅ **2500+ linii kodu** działającego i przetestowanego
- ✅ **20+ ćwiczeń** dla studentów
- ✅ **45+ testów** xUnit
- ✅ **7 diagramów** Mermaid
- ✅ **Gotowy do użycia** w nauczaniu


