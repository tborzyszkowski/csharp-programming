# Konstruktory, Destruktory i Inicjalizacja Obiektów w C#

## 🎯 Cel modułu

Głębokie zrozumienie mechanizmów tworzenia i inicjalizacji obiektów w C#, od podstawowych konstruktorów, przez zaawansowane wzorce projektowe, aż po nowoczesne cechy C# (records, init properties, primary constructors).

## 📚 Spis tematów

1. **[Konstruktory: o co chodzi?](_01_constructors_basics/README.md)** - Podstawy, syntaktyka, parametry
2. **[Łańcuchowe wywołanie konstruktorów](_02_constructor_chaining/README.md)** - `this()` i konstruktory zależne
3. **[Inicjalizatory obiektów](_03_object_initializers/README.md)** - Object initializers, collection initializers, target-typed expressions
4. **[Kolejność inicjalizacji](_04_initialization_order/README.md)** - Pola, konstruktory, właściwości - co wykonuje się kiedy?
5. **[Struktury: inicjalizacja i konstruktory](_05_struct_initialization/README.md)** - Value types, default constructor, stackalloc
6. **[Czym jest Destruktor?](_06_destructors/README.md)** - Finalizers, `~Destruktor()`, garbage collection
7. **[Wzorce projektowe: co to jest?](_07_design_patterns_intro/README.md)** - Wprowadzenie do Design Patterns, SOLID principles
8. **[Tworzenie obiektu na podstawie wzorca Prototyp](_08_prototype_pattern/README.md)** - Prototype Pattern, shallow/deep copy, ICloneable
9. **[Destruktory a kod zarządzany i niezarządzany](_09_managed_vs_unmanaged/README.md)** - GC, IDisposable, using statement, C# 8+ using declaration
10. **[Nowoczesne C#: Records, Init Properties, Primary Constructors](_10_modern_csharp/README.md)** - C# 9+, 11+, 12+ features

---

## 🚀 Jak pracować z tym modułem

### Uruchomienie dowolnego tematu

```bash
# Wejdź do folderu tematu (np. temat 1)
cd _01_constructors_basics/code/

# Uruchom demonstrację
rtk dotnet run

# Uruchom testy
rtk dotnet test

# Zbuduj projekt
rtk dotnet build
```

### Struktura każdego tematu

```
_NN_temat_name/
├── README.md          (2000-3000 słów, pełne wyjaśnienia + kody)
├── code/
│   ├── TopicName.csproj    (projekt .NET 9.0)
│   └── Program.cs          (demonstracja + xUnit testy)
├── diagrams/
│   └── NN-name.mermaid     (UML, sekwencyjne, flow)
└── tasks/
    └── README.md           (3-5 zadań dla studentów + rozwiązania)
```

### Wymogi techniczne

- ✅ **.NET 9.0+** SDK
- ✅ **C# 13** (LangVersion: latest)
- ✅ **xUnit 2.6.6** dla testów
- ✅ **Nullable: enable** dla strict null checking

---

## 📊 Charakterystyka tematu

| Aspekt | Szczegóły |
|--------|-----------|
| Poziom | Średniozaawansowany → Zaawansowany |
| Czas | 15-20 godzin (wszystkie tematy) |
| Przedmioty | OOP, Design Patterns, Memory Management, Modern C# |
| Ćwiczenia | 30+ zadań z pełnymi rozwiązaniami |
| Diagramy | 20+ diagramów UML/Mermaid |
| Testy | 50+ testów xUnit |

---

## 🎓 Topologia Nauki

### Ścieżka Rekomendowana

```
Temat 1: Konstruktory (podstawy)
    ↓
Temat 4: Kolejność inicjalizacji (zrozumienie głębokie)
    ↓
Temat 2: Łańcuchowe konstruktory (efektywność)
    ↓
Temat 3: Inicjalizatory (elegancja kodu)
    ↓
Temat 5: Struktury (value types)
    ↓
Temat 6: Destruktory (cleanup)
    ↓
Temat 7: Wzorce projektowe (teoretyczne podłoże)
    ↓
Temat 8: Prototype Pattern (praktyczne)
    ↓
Temat 9: Managed vs Unmanaged (zaawansowane)
    ↓
Temat 10: Nowoczesny C# (best practices 2024+)
```

---

## 💡 Kluczowe Koncepty do Opanowania

- ✅ Konstruktor domyślny vs. parametrowy
- ✅ Konstruktor prywatny i vzorzec Singleton
- ✅ Łańcuch konstruktorów (constructor chaining)
- ✅ Inicjalizatory obiektów i kolekcji
- ✅ Pola readonly i inicjalizacja
- ✅ Konstruktory statyczne
- ✅ Destruktor i finalizacja
- ✅ Garbage Collection (GC) i generacje obiektów
- ✅ IDisposable pattern
- ✅ `using` statement (C# 8+)
- ✅ Records (C# 9+)
- ✅ Init-only properties (C# 9+)
- ✅ Primary constructors (C# 12+)
- ✅ Wzorzec Prototyp (Prototype Pattern)
- ✅ Shallow copy vs. deep copy

---

## 📖 Referencje i Zasoby

### Dokumentacja Oficjalna

- [Microsoft Learn: Constructors](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/constructors)
- [Microsoft Learn: Destructors](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/destructors)
- [Microsoft Learn: IDisposable](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose)
- [C# 9 Records Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records)
- [C# Language Features Reference](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new)

### Książki

- "C# Player's Guide" (RB Whitaker)
- "CLR via C#" (Jeffrey Richter) - zaawansowany
- "Design Patterns: Elements of Reusable Object-Oriented Software" (Gang of Four)
- "Pro C#" (Andrew Troelsen)

### YouTube & Tutoriale

- [Konstruktory w C# - Microsoft](https://www.youtube.com/results?search_query=C%23+constructors+tutorial)
- [Design Patterns in C#](https://www.youtube.com/results?search_query=design+patterns+C%23)
- [Garbage Collection .NET](https://www.youtube.com/results?search_query=.NET+garbage+collection+explained)
- [IDisposable Pattern](https://www.youtube.com/results?search_query=C%23+IDisposable+pattern+tutorial)

---

## ✅ Checklist Wiedzy

Po ukończeniu tego modułu powinieneś wiedzieć:

- [ ] Kiedy i dlaczego konstruktor się uruchamia
- [ ] Jak łączyć konstruktory za pomocą `this()`
- [ ] Różnica między inicjalizatorem obiektu a konstruktorem
- [ ] Dokładna kolejność inicjalizacji pól, konstruktora i właściwości
- [ ] Jak struktury (value types) różnią się w inicjalizacji od klas
- [ ] Rola destruktora i finalizers w garbage collection
- [ ] Czym są wzorce projektowe i do czego służą
- [ ] Jak implementować Prototype Pattern
- [ ] Zarządzanie zasobami niezarządzanymi (GC, finalizers, IDisposable)
- [ ] Nowe cechy C# 9+ (records, init properties, primary constructors)

---

## 🎬 Jak Uczyć (dla wykładowców)

1. **Wstęp** (Temat 1) - Pokażiż kod w Visual Studio Code, `dotnet run`
2. **Interaktywnie** - Modyfikujcie kod na żywo, obserwujcie konsolę
3. **Unikajcie Teorii** - Najpierw kod, potem wyjaśnienia
4. **Diagramy** - Prezentujcie diagramy Mermaid do wizualizacji
5. **Zadania** - Studenci robią zadania, dyskutujcie rozwiązania
6. **Testy** - Pokażcie jak testy weryfikują kod

---

## 📝 Najnowsze Aktualizacje

- ✅ C# 12+ Primary Constructors
- ✅ C# 11 Required Keyword
- ✅ .NET 9.0 features
- ✅ Modern IDisposable patterns
- ✅ Nullable reference types

---

**Status: ✅ Moduł pełny i gotowy do nauczania**

*Ostatnia aktualizacja: 2024-08-30*
