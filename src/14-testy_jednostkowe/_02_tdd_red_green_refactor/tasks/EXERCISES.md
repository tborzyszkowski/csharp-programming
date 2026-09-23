# Ćwiczenia - TDD: Red-Green-Refactor

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 2.1: FizzBuzz w TDD
**Cel:** Przećwiczyć pełny cykl Red-Green-Refactor na klasycznym zadaniu.

Zaimplementuj `FizzBuzzConverter.Convert(int number)` metodą TDD:
1. 🔴 Napisz test: `Convert(1)` zwraca `"1"`
2. 🟢 Zaimplementuj minimalnie
3. 🔴 Napisz test: `Convert(3)` zwraca `"Fizz"`
4. 🟢 Rozszerz implementację
5. 🔴 Napisz test: `Convert(5)` zwraca `"Buzz"`
6. 🟢 Rozszerz implementację
7. 🔴 Napisz test: `Convert(15)` zwraca `"FizzBuzz"`
8. 🟢 Rozszerz implementację
9. 🔵 Zrefaktoryzuj – usuń powtórzenia

Zapisz w komentarzach, jak wyglądała implementacja **po każdej rundzie**.

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 2.2: Walidator Hasła metodą TDD
**Cel:** Zastosować TDD do bardziej złożonej logiki biznesowej.

Zaimplementuj `PasswordValidator.IsValid(string password)` w TDD, obsługując kolejno (jeden test na raz!):
1. Hasło krótsze niż 8 znaków → `false`
2. Hasło bez wielkiej litery → `false`
3. Hasło bez cyfry → `false`
4. Hasło spełniające wszystkie warunki → `true`

**Ważne:** Za każdym razem pisz **tylko jeden nowy test**, uruchom go (RED), dopiero potem zmień kod (GREEN).

---

### Ćwiczenie 2.3: Refaktoryzacja Pod Ochroną Testów
**Cel:** Zobaczyć wartość testów podczas refaktoryzacji.

Mając działający `StringCalculator` z tego tematu, dodaj obsługę separatora `;` obok `,` (np. `"1;2,3"` → `6`). Napisz najpierw test (RED), potem zaimplementuj (GREEN), a na końcu zrefaktoryzuj metodę `Add`, żeby czytelnie obsługiwała wiele separatorów jednocześnie.

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 2.4: TDD z Wyjątkami
**Cel:** Zastosować TDD, gdy oczekiwanym zachowaniem jest wyjątek.

Rozszerz `StringCalculator.Add`, aby rzucał `ArgumentException` z komunikatem zawierającym ujemne liczby, gdy w wejściu znajdą się liczby ujemne (np. `"1,-2,3"` → wyjątek z komunikatem `"liczby ujemne niedozwolone: -2"`).

Zastosuj pełen cykl:
1. 🔴 Napisz test z `Assert.Throws<ArgumentException>`
2. 🟢 Zaimplementuj sprawdzanie liczb ujemnych
3. 🔵 Zrefaktoryzuj tak, aby komunikat zawierał **wszystkie** ujemne liczby, nie tylko pierwszą

**Podpowiedź:** Sprawdź właściwość `Exception.Message` w asercji.
