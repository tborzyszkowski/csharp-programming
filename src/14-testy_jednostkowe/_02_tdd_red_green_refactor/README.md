# Temat 2: Test-Driven Development – Red-Green-Refactor

## 🎯 Cel Tematu

Nauczysz się pisać **testy przed kodem produkcyjnym** i stosować cykl **Red-Green-Refactor** – rdzeń podejścia TDD.

### Słowa Kluczowe
- Test-Driven Development (TDD)
- Red-Green-Refactor
- Testowalny design (testable design)
- Baby steps (małe kroki)

---

## 📖 Czym Jest TDD?

**Test-Driven Development** to technika programowania, w której **najpierw piszesz test** (który na starcie zawodzi), a **dopiero potem** implementujesz kod, który sprawia, że test przechodzi.

To odwrócenie tradycyjnego podejścia "najpierw kod, potem (może) testy".

### Cykl Red-Green-Refactor

```
🔴 RED         Napisz test, który NIE przechodzi (kod jeszcze nie istnieje)
   ↓
🟢 GREEN       Napisz NAJPROSTSZY możliwy kod, żeby test przeszedł
   ↓
🔵 REFACTOR    Popraw jakość kodu, testy chronią Cię przed regresją
   ↓
   (powtórz dla kolejnego wymagania)
```

---

## 🧩 Przykład Krok Po Kroku: `StringCalculator`

Zaimplementujemy klasę, która sumuje liczby rozdzielone przecinkami w stringu: `"1,2,3"` → `6`.

### Krok 1: 🔴 RED – Pusty String Zwraca Zero

```csharp
[Fact]
public void Add_PustyString_ZwracaZero()
{
    var calculator = new StringCalculator();

    int result = calculator.Add("");

    Assert.Equal(0, result);
}
```

Uruchamiamy `dotnet test` – **kompilacja się nie powiedzie**, bo `StringCalculator` jeszcze nie istnieje. To wciąż "RED" – najprostsza możliwa forma czerwonego testu.

### Krok 2: 🟢 GREEN – Minimalna Implementacja

```csharp
public class StringCalculator
{
    public int Add(string numbers) => 0;
}
```

Test przechodzi! Tak, implementacja jest "oszukana" – ale to celowe. TDD nakazuje pisać **najmniej kodu, jaki jest potrzebny**, żeby test przeszedł.

### Krok 3: 🔴 RED – Jedna Liczba

```csharp
[Fact]
public void Add_JednaLiczba_ZwracaTaLiczbe()
{
    var calculator = new StringCalculator();

    int result = calculator.Add("5");

    Assert.Equal(5, result);
}
```

Ten test **nie przejdzie** przy obecnej implementacji (zawsze zwraca 0).

### Krok 4: 🟢 GREEN – Obsługa Jednej Liczby

```csharp
public class StringCalculator
{
    public int Add(string numbers)
    {
        if (string.IsNullOrEmpty(numbers))
            return 0;

        return int.Parse(numbers);
    }
}
```

Oba testy przechodzą.

### Krok 5: 🔴 RED – Wiele Liczb

```csharp
[Fact]
public void Add_WieleLiczb_ZwracaSume()
{
    var calculator = new StringCalculator();

    int result = calculator.Add("1,2,3");

    Assert.Equal(6, result);
}
```

### Krok 6: 🟢 GREEN – Obsługa Wielu Liczb

```csharp
public class StringCalculator
{
    public int Add(string numbers)
    {
        if (string.IsNullOrEmpty(numbers))
            return 0;

        return numbers.Split(',').Select(int.Parse).Sum();
    }
}
```

### Krok 7: 🔵 REFACTOR

Kod działa i wszystkie 3 testy przechodzą. Teraz możemy bezpiecznie go poprawić – np. wydzielić stałą dla separatora – **testy natychmiast wykryją**, jeśli refaktoryzacja coś zepsuje:

```csharp
public class StringCalculator
{
    private const char Separator = ',';

    public int Add(string numbers)
    {
        if (string.IsNullOrEmpty(numbers))
            return 0;

        return numbers.Split(Separator).Select(int.Parse).Sum();
    }
}
```

`dotnet test` → wszystkie testy nadal zielone. Refaktoryzacja bezpieczna. ✅

---

## 🤔 Dlaczego TDD Działa?

1. **Wymusza projektowanie pod testowalność** – kod pisany "z myślą o teście" jest naturalnie mniej sprzężony
2. **Małe kroki (baby steps)** – łatwiej zdiagnozować błąd, gdy zmiana jest mała
3. **Testy jako specyfikacja** – test opisuje *oczekiwane zachowanie* zanim jeszcze istnieje kod
4. **Pewność siebie przy refaktoryzacji** – zielony pasek testów = możesz bezpiecznie zmieniać kod

---

## ⚠️ Częste Błędy Początkujących w TDD

| Błąd | Konsekwencja |
|---|---|
| Pisanie zbyt dużego testu na raz | Trudno o "najprostszą implementację" |
| Pomijanie kroku REFACTOR | Kod działa, ale jest brzydki i trudny w utrzymaniu |
| Testowanie szczegółów implementacji zamiast zachowania | Testy się psują przy każdej drobnej zmianie kodu |
| Pisanie testu po napisaniu kodu "dla świętego spokoju" | To już nie jest TDD, to zwykłe testowanie post-factum |

---

## 📝 Podsumowanie

- TDD = pisz test **przed** kodem, w cyklu Red-Green-Refactor
- 🔴 RED: napisz test, który nie przechodzi
- 🟢 GREEN: napisz minimalny kod, aby test przeszedł
- 🔵 REFACTOR: popraw kod, testy chronią przed regresją
- Małe kroki i częste uruchamianie `dotnet test` to podstawa tej techniki

**Następny temat:** [Asercje, Organizacja i Parametryzacja Testów](../_03_assertions_test_organization/README.md)

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md)
