# Typy Generyczne, Kolekcje i LINQ w C#

## 📚 Przegląd Rozdziału

Ten rozdział obejmuje fundamentalne koncepcje programowania generycznego w C#, w tym tworzenie oraz używanie kolekcji generycznych i zapytań LINQ. Materiały przygotowane są na podstawie nowoczesnych standardów C# (9.0 i nowszych) z praktycznymi przykładami kod, które można uruchamiać bezpośrednio w Visual Studio Code.

### Dlaczego generyki i LINQ?

Generyki umożliwiają pisanie **bezpiecznego pod względem typów** kodu wielokrotnego użytku, a LINQ (Language-Integrated Query) pozwala na ekspresyjne, deklaratywne zapytania na danych. Kombinacja tych narzędzi stanowi serce nowoczesnego programowania w C#.

## 📖 Struktura Materiałów

### 1. [Metody i Klasy Generyczne](_01_generic_methods_classes/README.md)
- Wstęp do generyk – korzyści i motywacja
- Składnia metod i klas generycznych
- Wariancja typów (`in`, `out`)
- Praktyczne przykłady: Stack, Queue, Repository
- Nowości w C# 9+ (records z typami generycznymi)

### 2. [Ograniczenia Typów Generycznych](_02_generic_constraints/README.md)
- Klauzula `where` – wszystkie typy ograniczeń
- `new()` – ograniczenie konstruktora
- Ograniczenia bazowe (klasa bazowa, interfejs)
- Wartość domyślna dla typów generycznych (`default(T)`)
- Spłycanie typów generycznych

### 3. [IEnumerable i IEnumerator](_03_ienumerable_ienumerator/README.md)
- Interfejs `IEnumerable<T>` i `IEnumerator<T>`
- Metoda `GetEnumerator()`
- Pętla `foreach` pod maską
- Implementacja niestandardowych kolekcji iterowalnych
- Iterator `yield` – upraszczanie implementacji

### 4. [Interfejsy Porównania Obiektów](_04_comparison_interfaces/README.md)
- `IComparable<T>` – porównywanie dla sortowania
- `IComparer<T>` – elastyczne strategie porównywania
- `IEquatable<T>` – równość obiektów
- Sortowanie kolekcji i wyszukiwanie
- Case studies: sortowanie złożonych obiektów

### 5. [Przegląd Kolekcji](_05_collections_overview/README.md)
- Hierarchia kolekcji w .NET
- `List<T>`, `Dictionary<K,V>`, `Queue<T>`, `Stack<T>`
- `HashSet<T>`, `SortedSet<T>`, `LinkedList<T>`
- `Enumerable` vs `Collection` interfejsy
- Wydajność i wybór właściwej kolekcji
- Initializers i collection expressions (C# 12)

### 6. [LINQ – Wprowadzenie](_06_linq_introduction/README.md)
- Historia i motywacja dla LINQ
- Składnia zapytań (Query Syntax) vs Method Syntax
- Podstawowe operatory: `Where`, `Select`, `OrderBy`, `GroupBy`
- Zakresy zmiennych i domknięcia
- Przetwarzanie w pamięci (`LINQ to Objects`)
- Lazy evaluation i `IEnumerable<T>`

### 7. [LINQ – Zaawansowane Techniki](_07_linq_advanced/README.md)
- Operatory agregujące: `Aggregate`, `Sum`, `Average`, `Count`
- Spłaszczanie: `SelectMany`, `Flatten`
- Łączenie zbiorów: `Join`, `GroupJoin`
- `Take`, `Skip`, `TakeWhile`, `SkipWhile`
- `Distinct`, `Except`, `Intersect`, `Union`
- Custom LINQ operators
- Performance considerations i optimizacje
- Expression Trees (przegląd)
- Nowości w C# 9+: init-only properties w LINQ

## 🎯 Jak Korzystać z Materiałów

### Dla Wykładowcy
Każdy temat zawiera:
- **README.md** – szczegółowe wyjaśnienia koncepcji z diagramami
- **Kod demonstracyjny** – gotowe przykłady do pokazania na wykładzie
- **Diagramy Mermaid** – wizualizacje architektury i przepływu danych
- **Zadania dla studentów** – ćwiczenia z rozwiązaniami i wyjaśnieniami

### Dla Studenta
1. Przeczytaj **README.md** danego tematu
2. Przeanalizuj kod w folderze `code/`
3. Uruchom kod: `dotnet run` w odpowiednim katalogu
4. Przejrzyj diagramy w folderze `diagrams/`
5. Wykonaj zadania z folderu `tasks/`
6. Porównaj rozwiązania z podanymi odpowiedziami

## 🚀 Wymagania

- .NET SDK 9.0 lub nowszy
- Visual Studio Code (lub Visual Studio Community)
- C# rozszerzenie do VS Code
- Podstawowa wiedza o OOP w C#

## 📦 Uruchamianie Przykładów

Każdy temat ma strukturę gotową do uruchomienia z `dotnet`:

```bash
cd _01_generic_methods_classes/code
dotnet run
```

Aby uruchomić testy:

```bash
cd _01_generic_methods_classes/code
dotnet test
```

## 📚 Referencje i Źródła

### Oficjalna Dokumentacja
- [Generics in .NET](https://learn.microsoft.com/en-us/dotnet/standard/generics/)
- [LINQ Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)
- [C# Language Features](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new)

### Rekomendowane Artykuły
- [Understanding Generic Constraints](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters)
- [LINQ Performance Tips](https://devblogs.microsoft.com/dotnet/performance-improvements-in-net-5/)

### Książki
- "C# Player's Guide" – RB Whitaker (generyki i praktyka)
- "LINQ in Action" – Fabrice Marguerie

## 🎓 Ścieżka Nauki

```
START
  ↓
[1. Metody i klasy generyczne] – zrozumienie parametrów typów
  ↓
[2. Ograniczenia typów] – jak kontrolować typy generyczne
  ↓
[3. IEnumerable/IEnumerator] – iteracja i enumery
  ↓
[4. Interfejsy porównania] – sortowanie i równość
  ↓
[5. Przegląd kolekcji] – praktyczne struktury danych
  ↓
[6. LINQ Intro] – zapytania deklaratywne
  ↓
[7. LINQ Advanced] – zaawansowane operacje
  ↓
KONIEC – możesz pisać efektywny, elegancki kod C#!
```

## 📝 Historia Wersji

| Wersja | Data | Zmiany |
|--------|------|--------|
| 1.0 | 2026-08-30 | Inicjalna wersja materiałów dydaktycznych dla C# 9+ |

---

**Autor:** Materiały dydaktyczne dla kursu OOP w C#  
**Ostatnia aktualizacja:** 2026-08-30  
**Kompatybilność:** .NET 9.0 lub nowsze
