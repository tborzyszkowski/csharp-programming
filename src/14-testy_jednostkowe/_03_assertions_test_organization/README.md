# Temat 3: Asercje, Organizacja i Parametryzacja Testów

## 🎯 Cel Tematu

Poznasz pełne bogactwo biblioteki `Assert` w xUnit, nauczysz się organizować testy w klasy oraz **parametryzować** je za pomocą `[Theory]`, `InlineData` i `MemberData`, aby uniknąć powielania kodu testowego.

### Słowa Kluczowe
- `Assert.Equal`, `Assert.True`, `Assert.Same`, `Assert.Contains`
- `[Theory]`, `[InlineData]`, `[MemberData]`, `[ClassData]`
- Arrange-Act-Assert (AAA)
- DRY w testach

---

## 📖 Najważniejsze Asercje w xUnit

```csharp
// Równość wartości
Assert.Equal(5, result);
Assert.NotEqual(0, result);

// Wartości logiczne
Assert.True(isValid);
Assert.False(isValid);

// Referencje (ta sama instancja w pamięci)
Assert.Same(expectedInstance, actualInstance);
Assert.NotSame(expectedInstance, actualInstance);

// Null
Assert.Null(value);
Assert.NotNull(value);

// Kolekcje
Assert.Contains(3, new[] { 1, 2, 3 });
Assert.DoesNotContain(4, new[] { 1, 2, 3 });
Assert.Empty(new List<int>());
Assert.Single(new[] { "jeden element" });

// Typy
Assert.IsType<ArgumentException>(exception);

// Precyzja liczb zmiennoprzecinkowych
Assert.Equal(3.14, result, precision: 2);

// Wyjątki
Assert.Throws<InvalidOperationException>(() => obj.DoSomething());
```

**Zasada:** Wybieraj asercję, która **najlepiej opisuje intencję**. `Assert.Empty(list)` czyta się lepiej niż `Assert.Equal(0, list.Count)`.

---

## 🔁 Problem: Powielanie Testów

Wyobraź sobie walidator wieku:

```csharp
public class AgeValidator
{
    public bool IsAdult(int age) => age >= 18;
}
```

Bez parametryzacji musielibyśmy pisać osobny test dla każdego przypadku:

```csharp
[Fact]
public void IsAdult_Wiek17_ZwracaFalse()
{
    var validator = new AgeValidator();
    Assert.False(validator.IsAdult(17));
}

[Fact]
public void IsAdult_Wiek18_ZwracaTrue()
{
    var validator = new AgeValidator();
    Assert.True(validator.IsAdult(18));
}

[Fact]
public void IsAdult_Wiek65_ZwracaTrue()
{
    var validator = new AgeValidator();
    Assert.True(validator.IsAdult(65));
}
```

**Problem:** Trzy prawie identyczne testy. To narusza zasadę DRY (*Don't Repeat Yourself*).

---

## 🎨 Rozwiązanie: `[Theory]` + `[InlineData]`

```csharp
public class AgeValidatorTests
{
    [Theory]
    [InlineData(17, false)]
    [InlineData(18, true)]
    [InlineData(65, true)]
    [InlineData(0, false)]
    public void IsAdult_RoznyWiek_ZwracaPoprawnyWynik(int age, bool expected)
    {
        // Arrange
        var validator = new AgeValidator();

        // Act
        bool result = validator.IsAdult(age);

        // Assert
        Assert.Equal(expected, result);
    }
}
```

`dotnet test` uruchomi **4 osobne testy** – po jednym na każdy `[InlineData]` – każdy z osobnym wynikiem Pass/Fail.

**Zaleta:** Jeden test, wiele przypadków, zero powielania kodu.

---

## 📊 `[MemberData]` – Dane ze Współdzielonej Metody/Właściwości

Gdy dane testowe są zbyt złożone dla `InlineData` (np. obiekty, listy), używamy `MemberData`:

```csharp
public class DiscountCalculatorTests
{
    public static IEnumerable<object[]> DiscountScenarios =>
        new List<object[]>
        {
            new object[] { 100m, 0, 100m },      // Bez rabatu
            new object[] { 100m, 10, 90m },       // 10% rabatu
            new object[] { 200m, 50, 100m },      // 50% rabatu
        };

    [Theory]
    [MemberData(nameof(DiscountScenarios))]
    public void ApplyDiscount_RoznePrzypadki_ZwracaPoprawnaCene(
        decimal price, int discountPercent, decimal expected)
    {
        var calculator = new DiscountCalculator();

        decimal result = calculator.ApplyDiscount(price, discountPercent);

        Assert.Equal(expected, result);
    }
}
```

---

## 🗂️ Organizacja Testów w Klasy

Konwencja: **jedna klasa testowa na jedną klasę produkcyjną**, nazwana `<Klasa>Tests`:

```csharp
public class DiscountCalculator { /* ... */ }       // Kod produkcyjny
public class DiscountCalculatorTests { /* ... */ }  // Testy
```

Wewnątrz klasy testowej grupujemy testy według metody, którą testują (widoczne w nazwie: `Metoda_Scenariusz_Wynik`).

---

## 📝 Podsumowanie

- xUnit oferuje bogaty zestaw asercji – wybieraj najbardziej opisową
- `[Theory]` + `[InlineData]` eliminuje powielanie testów dla prostych danych
- `[MemberData]` obsługuje bardziej złożone dane testowe (obiekty, kolekcje)
- Konwencja `<Klasa>Tests` i `Metoda_Scenariusz_Wynik` utrzymuje porządek w dużych projektach

**Następny temat:** [Mokowanie Zależności z Moq](../_04_mocking_dependencies/README.md) – nauczysz się izolować testowany kod od jego zależności.

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md)
