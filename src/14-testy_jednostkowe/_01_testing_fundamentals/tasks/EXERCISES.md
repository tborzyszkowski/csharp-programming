# Ćwiczenia - Testy Jednostkowe: Wprowadzenie i Filozofia

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 1.1: Testy dla `StringHelper`
**Cel:** Napisać pierwsze testy `[Fact]` od podstaw.

Zaimplementuj klasę:
```csharp
public class StringHelper
{
    public string Reverse(string input) => new string(input.Reverse().ToArray());
    public bool IsPalindrome(string input) => input == Reverse(input);
}
```

Napisz testy sprawdzające:
- `Reverse("abc")` zwraca `"cba"`
- `IsPalindrome("kajak")` zwraca `true`
- `IsPalindrome("hello")` zwraca `false`

**Oczekiwany wynik:** 3 przechodzące testy po `dotnet test`.

---

### Ćwiczenie 1.2: Test Wyjątku
**Cel:** Przetestować metodę, która rzuca wyjątek.

Dodaj do `StringHelper` metodę `GetFirstChar(string input)`, która rzuca `ArgumentException`, gdy `input` jest pusty. Napisz test weryfikujący, że wyjątek jest rzucany za pomocą `Assert.Throws<ArgumentException>`.

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 1.3: Piramida Testów – Analiza
**Cel:** Zrozumieć klasyfikację testów.

Dla poniższych scenariuszy określ, czy jest to test jednostkowy, integracyjny czy E2E i uzasadnij:
1. Test sprawdzający, że `OrderCalculator.CalculateTotal()` poprawnie sumuje ceny produktów (bez bazy danych)
2. Test sprawdzający, że aplikacja poprawnie zapisuje zamówienie do prawdziwej bazy SQL Server
3. Test klikający przez przeglądarkę cały proces zakupu produktu na stronie

---

### Ćwiczenie 1.4: Refaktoryzacja Nazw Testów
**Cel:** Zastosować konwencję `Metoda_Scenariusz_OczekiwanyWynik`.

Poniższe nazwy testów są nieczytelne. Zaproponuj lepsze nazwy zgodne z konwencją z tego tematu:
- `Test1()`
- `TestujDodawanie()`
- `SprawdzCzyDzialaDzielenie()`

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 1.5: Projekt Testowy od Zera
**Cel:** Skonfigurować kompletny projekt testowy.

Stwórz nowy projekt `.csproj` z referencjami do `xunit`, `xunit.runner.visualstudio` i `Microsoft.NET.Test.Sdk`. Napisz klasę `TemperatureConverter` z metodą `CelsiusToFahrenheit(double celsius)` oraz komplet testów pokrywających:
- Wartość dodatnią (np. 100°C → 212°F)
- Wartość ujemną (np. -40°C → -40°F)
- Zero (0°C → 32°F)

**Podpowiedź:** Wzór: `F = C * 9/5 + 32`

---

## 💡 Rozwiązania

Rozwiązania do ćwiczeń 1.1-1.3 znajdziesz przeanalizowane w `code/Program.cs` tego tematu – klasa `Calculator` demonstruje identyczne zasady na innym przykładzie. Spróbuj samodzielnie przed podglądem!
