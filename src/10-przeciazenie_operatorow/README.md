# Przeciążanie Operatorów w C# - Kompletny Kurs

## 📚 Spis Treści

1. **[Operatory które można przeciążyć](#temat-1)** - Jakie operatory są dostępne do przeciążenia?
2. **[Operatory przeciążane pośrednio](#temat-2)** - Relacje między operatorami
3. **[Metoda definiująca operator](#temat-3)** - Zasady ogólne i najlepsze praktyki
4. **[Operatory jednoargumentowe: +, -, !, ~](#temat-4)** - Operatory arytmetyczne i logiczne
5. **[Operatory ++, --](#temat-5)** - Pre/post increment i decrement
6. **[Operatory true, false](#temat-6)** - Konwersja do wartości logicznych
7. **[Operatory relacyjne](#temat-7)** - Porównania: ==, !=, <, >, <=, >=
8. **[Operatory binarne](#temat-8)** - Arytmetyka: +, -, *, /, %, &, |, ^, <<, >>
9. **[Operatory konwersji](#temat-9)** - Jawne i niejawne konwersje typu
10. **[Większy przykład](#temat-10)** - Kompleksowy system z Vector3D

---

## 🎯 Cel Kursu

Nauczysz się:
- ✅ Jakie operatory można przeciążać w C#
- ✅ Zasady i ograniczenia przeciążania
- ✅ Praktyczne implementacje dla każdego rodzaju operatora
- ✅ Best practices i anti-patterns
- ✅ Real-world przykłady z nowoczesnym C#

---

## 🔧 Wymagania

- **.NET:** 9.0 lub nowsza
- **C#:** 13 (latest features)
- **IDE:** Visual Studio Code, Visual Studio, czy Rider

---

## 📖 Jak Używać Tego Kursu

### Dla Studentów

```bash
# Przejdź do tematu
cd src/10-przeciazenie_operatorow/_01_overloadable_operators

# Przeczytaj temat
cat README.md

# Uruchom przykłady
dotnet run --project code/Program.csproj

# Rozwiąż ćwiczenia
# (zadania w tasks/EXERCISES.md)
```

### Dla Wykładowców

1. Każdy temat ma pełne wyjaśnienia koncepcji
2. Kod może być bezpośrednio pokazywany na wykładzie
3. Diagramy ilustrują kluczowe pojęcia
4. Ćwiczenia mają 3 poziomy trudności
5. Pełny example w Temacie 10

---

## 📊 Ścieżka Nauki

```
Temat 1 (Przegląd)
    ↓
Temat 2 (Relacje między operatorami)
    ↓
Temat 3 (Zasady ogólne)
    ├─────────────────────┐
    ↓                     ↓
Temat 4-6 (Jednoargumentowe)    Temat 7-8 (Binarne)
    ├─────────────────────┤
    ↓
Temat 9 (Konwersje)
    ↓
Temat 10 (Pełny przykład)
    ↓
✓ MASTER Operator Overloading
```

---

## 🌟 Nowoczesne Cechy C#

Kurs wykorzystuje:
- **C# 9:** Records, init-only properties
- **C# 8:** Nullable reference types
- **C# 11:** Required members, raw string literals
- **C# 12:** Collection expressions
- **Latest:** Best practices i patterns

---

## 🏗️ Struktura Modułu

```
src/10-przeciazenie_operatorow/
├── README.md                                    (ten plik)
├── _01_overloadable_operators/
│   ├── README.md
│   ├── code/Program.cs + .csproj
│   ├── diagrams/diagrams.md
│   └── tasks/EXERCISES.md
├── _02_indirect_operators/
├── _03_operator_definition_rules/
├── _04_unary_arithmetic/
├── _05_unary_increment_decrement/
├── _06_unary_true_false/
├── _07_relational_operators/
├── _08_binary_operators/
├── _09_conversion_operators/
└── _10_real_world_example/
```

---

## 📚 Referencje

### Microsoft Documentation
- [Operator Overloading in C#](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading)
- [Operator Precedence](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/)
- [User-defined conversion operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators)

### Najlepsze Praktyki
- [C# Operator Overloading Guidelines](https://learn.microsoft.com/en-us/dotnet/fundamentals/coding-style/coding-conventions)
- [API Design Guidelines](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/operator-overloads)

---

## 🚀 Szybki Start

Przejdź do Tematu 1:
```bash
cd _01_overloadable_operators
dotnet run --project code/Program.csproj
```

---

## 💡 Kluczowe Koncepty (Preview)

### Przeciążanie Operatorów

```csharp
public record Complex(double Real, double Imaginary)
{
    // Przeciążenie operatora +
    public static Complex operator +(Complex a, Complex b)
        => new(a.Real + b.Real, a.Imaginary + b.Imaginary);
    
    // Przeciążenie operatora !
    public static bool operator !(Complex c)
        => c.Real == 0 && c.Imaginary == 0;
}

// Użycie
var z1 = new Complex(1, 2);
var z2 = new Complex(3, 4);
var sum = z1 + z2;  // [4, 6]
if (!z1) Console.WriteLine("Zero!");
```

---

## 📝 Notatki Implementacyjne

### Ograniczenia

- ❌ Nie można przeciążać: `=`, `+=`, `-=`, itp (ale `-=` itd. są przeciążane pośrednio)
- ❌ Nie można zmieniać precedencji operatorów
- ✅ Można przeciążać: operatory arytmetyczne, relacyjne, logiczne, konwersji
- ✅ Operatory muszą być `public static`

### Best Practices

1. Zawsze przeciążaj parami (==, !=), (<, >), etc.
2. Implementuj `IEquatable<T>` razem z `==`
3. Zwracaj nowy obiekt (immutable pattern)
4. Respektuj matematyczne/semantyczne znaczenie operatorów
5. Dokumentuj zachowanie operatorów

---

## ✅ Kontrola Jakości

- [x] Wszystkie 10 tematów zawiera kompletne materiały
- [x] Kod kompiluje się z .NET 9.0+
- [x] Diagramy są w formacie Mermaid
- [x] Ćwiczenia na 3 poziomach (🟢 Basic, 🟡 Intermediate, 🔴 Advanced)
- [x] Referencje do Microsoft Docs
- [x] Nowoczesne C# best practices

---

## 🎓 Licencja i Autorstwo

Materiały edukacyjne  
Wersja: 1.0  
.NET 9.0  
C# 13

---

## 🔗 Przewodnik do Kolejnych Tematów

Po ukończeniu tego kursu, możesz przejść do:
- Interfejsy (src/07-interfejsy_abstrakcje)
- Generyki (future - src/11-generyki)
- LINQ (future - src/12-linq)
- Async/Await (future - src/13-asynchronous)

---

**Zaloguj się w Github Copilot i przejdź do Tematu 1! 🚀**
