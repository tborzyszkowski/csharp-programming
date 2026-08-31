# Ćwiczenia - Nietypowe Testy: Wyjątki, Czas, Async

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 5.1: Test Wyjątku z Weryfikacją Komunikatu
**Cel:** Sprawdzić nie tylko typ, ale i treść wyjątku.

Zaimplementuj `Rectangle` z konstruktorem rzucającym `ArgumentException`, gdy `width` lub `height` są ujemne. Napisz test weryfikujący zarówno typ wyjątku, jak i to, że `Message` zawiera słowo "ujemn".

---

### Ćwiczenie 5.2: Prosty Test Asynchroniczny
**Cel:** Napisać poprawny test dla metody `async Task`.

Zaimplementuj `async Task<int> CountVowelsAsync(string text)` (symuluj opóźnienie `await Task.Delay(1)`). Napisz test `async Task` weryfikujący poprawną liczbę samogłosek.

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 5.3: Abstrakcja Losowości
**Cel:** Uczynić kod zależny od `Random` testowalnym.

Zaprojektuj interfejs `IRandomProvider` z metodą `int Next(int min, int max)` oraz klasę `LotteryDrawer`, która losuje liczbę i zwraca `"Wygrana!"`, jeśli wylosowana liczba to dokładnie 7. Napisz dwa testy z mockiem `IRandomProvider`: jeden zwracający 7 (wygrana), drugi zwracający inną liczbę (przegrana).

---

### Ćwiczenie 5.4: `Assert.ThrowsAsync` z Walidacją
**Cel:** Przetestować asynchroniczną metodę walidującą.

Rozszerz `OrderProcessor.ValidateAsync`, aby przyjmowała też `maxAmount` i rzucała wyjątek, gdy kwota przekracza limit. Napisz komplet testów `async Task` pokrywających: kwotę ujemną, kwotę przekraczającą limit i kwotę poprawną (bez wyjątku – użyj `await processor.ValidateAsync(...)` bez `Assert.ThrowsAsync` i sprawdź, że test przechodzi bez błędu).

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 5.5: `IClassFixture` z Kosztownym Zasobem
**Cel:** Zasymulować współdzielenie kosztownego zasobu między testami.

Zaimplementuj `ExpensiveResourceFixture`, która w konstruktorze "symuluje" kosztowną operację (np. `Thread.Sleep(100)` i licznik statyczny `InitializationCount`). Napisz klasę testową z co najmniej 3 testami korzystającymi z tego fixture i zweryfikuj (np. przez asercję na statycznym liczniku odczytanym po wszystkich testach, albo komentarzem z obserwacją), że konstruktor fixture wykonał się tylko **raz**.

**Podpowiedź:** Porównaj to zachowanie z sytuacją, gdyby `ExpensiveResourceFixture` była tworzona bezpośrednio w konstruktorze klasy testowej (bez `IClassFixture`) – ile razy wykonałaby się wtedy kosztowna operacja?

### Ćwiczenie 5.6: Test dla `DateTime.Now` Bez Abstrakcji – Analiza Problemu
**Cel:** Zrozumieć, dlaczego pewnych rzeczy nie da się przetestować wprost.

Napisz krótkie uzasadnienie (komentarz w kodzie, 3-5 zdań): dlaczego test `Assert.Equal(DateTime.Now, someMethodResult)` jest z natury kruchy/niepoprawny, nawet jeśli w danym momencie "przechodzi". Zaproponuj alternatywne podejście bez wprowadzania abstrakcji `IClock` (np. sprawdzanie zakresu czasu z tolerancją) i wyjaśnij, dlaczego mimo wszystko `IClock` jest lepszym rozwiązaniem długoterminowo.
