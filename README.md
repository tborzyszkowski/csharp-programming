<p align="left">
  <a href="#wykorzystanie-ai-w-materiałach">
    <kbd style="background-color: #0056b3; color: white; padding: 5px 10px; border-radius: 4px; font-weight: bold; border: none; font-family: sans-serif; font-size: 13px;">🤖 AI-Assisted</kbd>
    <kbd style="background-color: #6c757d; color: white; padding: 5px 10px; border-radius: 4px; font-weight: bold; border: none; font-family: sans-serif; font-size: 13px;">Edukacja</kbd>
  </a>
</p>

# C# Programowanie Obiektowe – Materiały Dydaktyczne

## 🎓 O tym repozytorium

Kompletne materiały dydaktyczne do nauki **Programowania Obiektowego w C#** na poziomie uniwersytetu/szkoły.

Projekt zawiera teorię, praktyczne przykłady kodu, diagramy UML, testy jednostkowe i zadania dla studentów.

---

## 📂 Struktura projektu

```
csharp-programming/
├── README.md (ten plik)
├── LICENSE.md
└── src/
    └── 01-klasy/               ← Moduł 1: Klasy i Obiekty ✅ KOMPLETNY
        ├── README.md
        ├── _01_oop_fundamentals/
        ├── _02_class_definition/
        ├── _03_object_usage/
        ├── _04_this_keyword/
        ├── _05_access_modifiers/
        ├── _06_partial_classes/
        ├── _07_partial_methods/
        ├── _08_structs/
        └── _09_uml/
```

Każdy temat zawiera:

- 📄 **README.md** – Szczegółowa dokumentacja (1500-3000 słów)
- 💻 **code/** – Projekt .NET 9.0 z kodem + testami xUnit
- 📊 **diagrams/** – Diagramy Mermaid UML
- 📝 **tasks/** – Zadania z rozwiązaniami

---

## 🚀 Szybki Start

### 1. Klonowanie repozytorium

```bash
git clone https://github.com/tborzyszkowski/csharp-programming.git
cd csharp-programming
```

### 2. Otwórz w VS Code

```bash
code .
```

### 3. Przejdź do tematu 1

```bash
cd src/01-klasy/_01_oop_fundamentals/code/
dotnet run
```

---

## 📚 Moduły dostępne

### **Moduł 1: Klasy i Obiekty** ✅ KOMPLETNY

9 tematów + 50+ diagramów + 40+ zadań

| # | Temat | Status |
| --- | ------- | -------- |
| 1 | Programowanie Obiektowe – Podstawowe Pojęcia | ✅ |
| 2 | Definicja Klasy w Języku C# | ✅ |
| 3 | Tworzenie i Korzystanie z Obiektów | ✅ |
| 4 | Słowo Kluczowe `this` | ✅ |
| 5 | Ukrywanie Informacji – Modyfikatory Dostępu | ✅ |
| 6 | Klasy Częściowe | ✅ |
| 7 | Metody Częściowe | ✅ |
| 8 | Struktury – Słowo Kluczowe `struct` | ✅ |
| 9 | Język UML – Diagramy | ✅ |

**Przejdź do**: [src/01-klasy/README.md](src/01-klasy/README.md)

### **Planowane moduły**

- Moduł 2: Dziedziczenie i Polimorfizm (coming soon)
- Moduł 3: Interfejsy i Abstrakcje (coming soon)
- Moduł 4: Generyki (coming soon)
- Moduł 5: Wyjątki i obsługa błędów (coming soon)
- Moduł 6: LINQ i kolekcje (coming soon)
- Moduł 7: Design Patterns (coming soon)

---

## 🎯 Cele Nauczania

Po ukończeniu tego kursu, nauczysz się:

### Wiedzy (Knowledge)

- ✅ Czterech filarów OOP (abstrakcja, enkapsulacja, dziedziczenie, polimorfizm)
- ✅ Definiowania klas i tworzenia obiektów
- ✅ Pracy z polami, metodami i właściwościami
- ✅ Modyfikatorów dostępu i enkapsulacji
- ✅ Czytania i tworzenia diagramów UML

### Umiejętności (Skills)

- ✅ Projektowania klas z respektowaniem OOP
- ✅ Refaktoryzacji kodu do formy obiektowej
- ✅ Testowania kodu za pomocą xUnit
- ✅ Dokumentowania kodu za pomocą XML comments
- ✅ Czytania i pisania diagramów UML

---

## 💻 Technologia

| Technologia | Wersja | Cel |
| --- | --- | --- |
| **C#** | 12+ | Język programowania |
| **.NET** | 9.0 | Framework |
| **xUnit** | 2.6.6 | Testy jednostkowe |
| **Mermaid** | Latest | Diagramy |
| **VS Code** | Latest | Editor |

---

## 🔧 Uruchamianie testów

```bash
# Wejdź do katalogu tematu
cd src/01-klasy/_01_oop_fundamentals/code/

# Uruchom wszystkie testy
dotnet test

# Uruchom program
dotnet run

# Oczyszczaj i buduj
dotnet clean
dotnet build
```

---

## 📊 Statystyki projektu

| Metrika | Liczba |
| --------- | -------- |
| **Tematy** | 9 |
| **Stron dokumentacji** | ~22 500 słów |
| **Linii kodu** | 1500+ |
| **Diagramów UML** | 30+ |
| **Zadań** | 25+ |
| **Testów jednostkowych** | 60+ |
| **Plików** | 100+ |

---

## 🌐 Referencje i linki

### Oficjalna dokumentacja

- [Microsoft C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [.NET Official Site](https://dotnet.microsoft.com/)
- [xUnit.net Testing Framework](https://xunit.net/)

### Powiązane projekty

- [CS_OOP Repository](https://github.com/tborzyszkowski/CS_OOP) – Stare materiały
- [OOP Concepts Java](https://github.com/tborzyszkowski/oop-concepts-java) – Wersja Java

### Książki polecane

- "C# Player's Guide" – RB Whitaker
- "Clean Code" – Robert C. Martin
- "Head First Design Patterns" – Freeman & Robson

---

## 🤝 Wkład w projekt

### Jak wspierać?

1. **Zgłosić błędy** – Otwórz GitHub Issue
2. **Sugestie** – Dyskusje i pull requests
3. **Tłumaczenia** – Pomóż w tłumaczeniu
4. **Udział** – Dodaj własne przykłady

### Wytyczne

- Kod zgodny z [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Dokumentacja w formacie Markdown
- Diagramy w Mermaid
- Testy dla każdego kodu

---

## 📝 Licencja

**CC BY-NC-SA 4.0** – Wolno dzielić i modyfikować na potrzeby edukacyjne  
Wymagane przypisanie autorstwa  
Nie do celów komercyjnych  
Licencja musi być zachowana w pochodnych pracach

---

## 💬 FAQ

### P: Ile czasu zajmie ten kurs?

**O:** Około 40-50 godzin (5-10 tygodni, 4-5 godzin tygodniowo)

### P: Czy potrzebuję wcześniejszej wiedzy?

**O:** TAK. Powinieneś znać podstawy C# (zmienne, pętle, instrukcje warunkowe)

### P: Czy kod jest testowany?

**O:** TAK. Każdy temat ma testy xUnit. Możesz uruchomić `dotnet test`

### P: Czy mogę używać tych materiałów komercyjnie?

**O:** NIE. Licencja CC BY-NC-SA zabrania użytku komercyjnego

### P: Jak zgłosić błędy?

**O:** Otwórz GitHub Issue z opisem problemu

---

## 👨‍💼 O autorze

**Tomasz Borzyszkowski**  
Nauczyciel OOP w UG, Gdańsk  
GitHub: [@tborzyszkowski](https://github.com/tborzyszkowski)

---

## 📞 Kontakt i wsparcie

- **GitHub Issues**: [Otwórz issue](https://github.com/tborzyszkowski/csharp-programming/issues)
- **Dyskusje**: [GitHub Discussions](https://github.com/tborzyszkowski/csharp-programming/discussions)
- **Email**: Dostępny w moim profilu GitHub

---

## 🙏 Podziękowania

Dziękuję inspiracji od:

- Microsoft Learn
- Refactoring.Guru
- C# Player's Guide – RB Whitaker
- GitHub Education Community

---

**Powodzenia w nauce! 🚀**

Jeśli ten projekt Ci się podoba, daj ⭐ Star!

## Wykorzystanie AI w materiałach

Materiały dydaktyczne zawarte w tym repozytorium są przygotowywane przy wsparciu narzędzi sztucznej inteligencji (Generative AI), które pełnią rolę asystenta twórcy.

Sztuczna inteligencja jest wykorzystywana w celach pomocniczych, w szczególności do:

- Współtworzenia i optymalizacji bazowych przykładów kodu oraz konfiguracji.
- Formatowania, strukturyzacji oraz automatyzacji generowania dokumentacji.
- Wsparcia procesu redakcyjnego, korekty językowej oraz generowania alternatywnych wyjaśnień pojęć technicznych.

Wszystkie materiały, schematy oraz kody źródłowe podlegają **weryfikacji merytorycznej i edycji przez człowieka**.
Ostateczna treść oraz układ dydaktyczny są wynikiem autorskiego nadzoru, co zapewnia ich poprawność oraz zgodność ze standardami akademickimi.

---

_Last updated: 2024-09-24_  
_Version: 1.0 – Pierwsza wersja modułów ukończona_
