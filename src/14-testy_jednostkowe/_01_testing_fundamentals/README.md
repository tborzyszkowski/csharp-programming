# Temat 1: Testy Jednostkowe – Wprowadzenie i Filozofia

## 🎯 Cel Tematu

Zrozumiesz **po co** piszemy testy jednostkowe, czym różnią się od innych rodzajów testów oraz jak napisać i uruchomić pierwszy test w **xUnit**.

### Słowa Kluczowe
- Unit test (test jednostkowy)
- Piramida testów (test pyramid)
- `[Fact]`, `Assert`
- Regresja (regression)
- `dotnet test`

---

## 📖 Po Co Testować?

### Problem: Kod Bez Testów

```csharp
public class Calculator
{
    public int Divide(int a, int b) => a / b;
}
```

Wygląda niewinnie, ale:
- Co się stanie przy `Divide(10, 0)`?
- Czy ktoś przypadkiem nie zmieni `/` na `+` podczas refaktoryzacji?
- Skąd wiesz, że kod nadal działa po zmianie w innym miejscu projektu?

**Bez testów** odpowiedzi na te pytania poznajesz dopiero na produkcji – najgorszym możliwym miejscu.

### Test Jednostkowy = Automatyczna, Powtarzalna Weryfikacja

**Test jednostkowy** sprawdza mały, izolowany fragment kodu (zwykle jedną metodę/klasę) w sposób:
- **Automatyczny** – nie wymaga ręcznego klikania w aplikacji
- **Powtarzalny** – ten sam wynik za każdym uruchomieniem
- **Szybki** – setki testów wykonują się w sekundy
- **Niezależny** – jeden test nie wpływa na wynik innego

```csharp
// Test dla Calculator.Divide
[Fact]
public void Divide_DwieLiczby_ZwracaIloraz()
{
    // Arrange
    var calculator = new Calculator();

    // Act
    int result = calculator.Divide(10, 2);

    // Assert
    Assert.Equal(5, result);
}
```

Jeśli ktoś kiedyś zmieni `Divide` tak, że zwróci błędny wynik – ten test **natychmiast** to wykryje. To jest **ochrona przed regresją**.

---

## 🏔️ Piramida Testów

```
        ▲
       ╱ ╲          E2E (End-to-End)
      ╱───╲         Wolne, kruche, ale testują całość
     ╱     ╲
    ╱───────╲       Testy Integracyjne
   ╱         ╲       Testują współpracę komponentów (baza, API)
  ╱───────────╲
 ╱             ╲     Testy Jednostkowe
╱───────────────╲    Szybkie, liczne, testują pojedyncze klasy/metody
```

- **Testy jednostkowe** – najliczniejsze, najszybsze, testują logikę w izolacji (ten moduł, tematy 1-5)
- **Testy integracyjne** – mniej liczne, testują współpracę z bazą danych, plikami, API (temat 6)
- **Testy E2E** – najmniej liczne, testują całą aplikację z perspektywy użytkownika (poza zakresem tego modułu)

**Zasada:** Im niżej w piramidzie, tym więcej testów – bo są tanie w utrzymaniu i szybkie w wykonaniu.

---

## 🧪 xUnit – Podstawowy Framework Testowy

**xUnit** to najpopularniejszy framework testowy w .NET (używany m.in. przez sam zespół ASP.NET Core).

### Anatomia Projektu Testowego

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="xunit" Version="2.6.6" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.2" />
  </ItemGroup>
</Project>
```

### Pierwszy Test: Atrybut `[Fact]`

`[Fact]` oznacza test, który **zawsze** wykonuje się tak samo (bez parametrów):

```csharp
public class CalculatorTests
{
    [Fact]
    public void Add_DwieLiczbyDodatnie_ZwracaSume()
    {
        // Arrange – przygotowanie danych
        var calculator = new Calculator();

        // Act – wykonanie testowanej operacji
        int result = calculator.Add(2, 3);

        // Assert – weryfikacja wyniku
        Assert.Equal(5, result);
    }
}
```

### Uruchamianie Testów

```bash
dotnet test
```

Przykładowy output:
```
Passed!  - Failed: 0, Passed: 1, Skipped: 0, Total: 1
```

Jeśli test padnie, `dotnet test` pokaże dokładnie która asercja i w której linii się nie zgodziła.

---

## ✅ Konwencja Nazewnictwa Testów

W tym module używamy konwencji: `MetodaTestowana_Scenariusz_OczekiwanyWynik`

```csharp
[Fact]
public void Divide_DzielenieprzezZero_RzucaWyjatek() { /* ... */ }

[Fact]
public void Add_LiczbyUjemne_ZwracaPoprawnaSume() { /* ... */ }
```

Dzięki temu nazwa testu **czyta się jak dokumentacja** – widać co jest testowane bez zaglądania w ciało metody.

---

## 🏛️ Diagram: Cykl Życia Testu

```
1. dotnet test uruchamia test runner
        ↓
2. Runner odnajduje wszystkie [Fact] i [Theory]
        ↓
3. Dla każdego testu: Arrange → Act → Assert
        ↓
4. Assert.Equal/True/Throws porównuje wynik
        ↓
5a. Zgodne → ✅ Passed       5b. Niezgodne → ❌ Failed + komunikat
```

---

## 📝 Podsumowanie

- Testy jednostkowe automatyzują weryfikację poprawności kodu i chronią przed regresją
- Piramida testów: dużo testów jednostkowych, mniej integracyjnych, jeszcze mniej E2E
- xUnit: `[Fact]` dla prostych testów, `Assert.*` dla weryfikacji, `dotnet test` do uruchamiania
- Wzorzec **Arrange-Act-Assert** porządkuje strukturę każdego testu
- Dobra nazwa testu = dokumentacja zachowania systemu

**Następny temat:** [Test-Driven Development: Red-Green-Refactor](../_02_tdd_red_green_refactor/README.md) – nauczysz się pisać testy **przed** kodem.

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md)
