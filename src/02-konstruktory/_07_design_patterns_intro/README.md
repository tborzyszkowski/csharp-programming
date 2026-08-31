# Wzorce Projektowe: Co to jest?

## 🎯 Cel rozdziału

Wprowadzenie do wzorców projektowych (Design Patterns) - powtarzalnych rozwiązań dla typowych problemów programistycznych.

## 📚 Spis treści

1. [Co to jest Design Pattern?](#co-to-jest-design-pattern)
2. [Kategorie wzorców](#kategorie-wzorców)
3. [Gang of Four (GoF)](#gang-of-four)
4. [SOLID Principles](#solid-principles)

---

## Co to jest Design Pattern?

**Design Pattern** to ogólne, powtarzalne rozwiązanie dla typowych problemów w projektowaniu oprogramowania.

Wzorce zawierają:
- **Problem** - co chcemy rozwiązać?
- **Rozwiązanie** - jak to robimy?
- **Konsekwencje** - jakie są skutki?

---

## Kategorie wzorców

### 1. Creational Patterns (Tworzenie obiektów)

- **Singleton** - jedna instancja
- **Factory** - tworzenie bez wiedzy o klasie
- **Builder** - konstruowanie złożonych obiektów
- **Prototype** - klonowanie obiektów
- **Abstract Factory** - rodziny obiektów

### 2. Structural Patterns (Struktura)

- **Adapter** - kompatybilność interfejsów
- **Decorator** - dodawanie funkcjonalności
- **Facade** - uproszczony interfejs
- **Proxy** - kontrola dostępu

### 3. Behavioral Patterns (Zachowanie)

- **Observer** - powiadomienia zmian
- **Strategy** - zamienialne algorytmy
- **Command** - enkapsulacja żądań
- **State** - zmiana zachowania ze stanem

---

## Gang of Four

"Design Patterns: Elements of Reusable Object-Oriented Software" (1994) opisuje 23 klasyczne wzorce.

---

## SOLID Principles

**SOLID** - 5 zasad dobrych praktyk OOP:

| Skrót | Nazwa | Znaczenie |
|-------|-------|-----------|
| **S** | Single Responsibility | Każda klasa ma jedną odpowiedzialność |
| **O** | Open/Closed | Otwarta na rozszerzenie, zamknięta na modyfikację |
| **L** | Liskov Substitution | Podklasy mogą zastępować klasy bazowe |
| **I** | Interface Segregation | Małe, specyficzne interfejsy |
| **D** | Dependency Inversion | Zależy od abstrakcji, nie od konkretów |

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
