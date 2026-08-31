# Temat 7: Dobre Praktyki i Sugestywne Przykłady

## 🎯 Cel Tematu

Podsumujesz cały moduł, poznając zasady **F.I.R.S.T.**, najczęstsze **anti-patterns** w testowaniu, temat pokrycia kodu (code coverage) oraz zestaw **sugestywnych przykładów** – dobrych i złych – które pomogą Ci rozpoznawać jakość testów w praktyce.

### Słowa Kluczowe
- Zasady F.I.R.S.T.
- Anti-patterns: Fragile Test, Test spaghetti, Mystery Guest
- Pokrycie kodu (code coverage) – korzyści i pułapki

---

## ✅ Zasady F.I.R.S.T. Dobrego Testu Jednostkowego

| Litera | Zasada | Znaczenie |
|---|---|---|
| **F** | **Fast** (szybki) | Test wykonuje się w milisekundach – setki testów w kilka sekund |
| **I** | **Independent** (niezależny) | Testy nie wpływają na siebie nawzajem; kolejność uruchomienia nie ma znaczenia |
| **R** | **Repeatable** (powtarzalny) | Ten sam wynik za każdym razem, niezależnie od środowiska (twój komputer, CI, kolega z zespołu) |
| **S** | **Self-validating** (samowalidujący) | Test sam mówi Pass/Fail – bez ręcznego sprawdzania logów |
| **T** | **Timely** (na czas) | Test pisany blisko momentu tworzenia kodu (idealnie: TDD, [Temat 2](../_02_tdd_red_green_refactor/README.md)) |

---

## ❌ Anti-Pattern #1: Fragile Test (Kruchy Test)

**Problem:** Test psuje się przy każdej drobnej, niezwiązanej zmianie w kodzie.

```csharp
// ❌ ŹLE - test zna szczegóły implementacji (kolejność wywołań wewnętrznych)
[Fact]
public void ProcessOrder_WywolujeMetodyWOkreslonejKolejnosci()
{
    mockLogger.Verify(l => l.Log("Start"), Times.Once);
    mockRepository.Verify(r => r.Save(It.IsAny<Order>()), Times.Once);
    mockLogger.Verify(l => l.Log("End"), Times.Once);
    // Jeśli ktoś zmieni kolejność logowania (bez zmiany zachowania biznesowego) - test padnie!
}
```

```csharp
// ✅ DOBRZE - test weryfikuje ZACHOWANIE (obserwowalny efekt), nie szczegóły implementacji
[Fact]
public void ProcessOrder_ZapisujeZamowienie()
{
    orderProcessor.Process(order);

    mockRepository.Verify(r => r.Save(order), Times.Once);
}
```

**Zasada:** Testuj *co* system robi (publiczne zachowanie), nie *jak* to robi wewnętrznie.

---

## ❌ Anti-Pattern #2: Mystery Guest

**Problem:** Test zależy od danych spoza samego testu (plik konfiguracyjny, stan globalny, kolejność uruchomienia innych testów), przez co czytający test nie widzi skąd biorą się dane.

```csharp
// ❌ ŹLE - skąd biorą się te dane? Trzeba szukać w zewnętrznym pliku!
[Fact]
public void CalculateDiscount_DlaKlienta_ZwracaPoprawnyRabat()
{
    var customer = TestDataLoader.LoadFromFile("customer_42.json"); // "Mystery Guest"
    // ...
}
```

```csharp
// ✅ DOBRZE - wszystkie dane widoczne wprost w teście (Arrange)
[Fact]
public void CalculateDiscount_DlaKlientaVip_ZwracaRabat20Procent()
{
    var customer = new Customer { Id = 42, IsVip = true };
    // ...
}
```

**Zasada:** Dobry test da się zrozumieć **bez opuszczania metody testowej**.

---

## ❌ Anti-Pattern #3: Test Spaghetti (Test Sprawdzający Zbyt Wiele)

```csharp
// ❌ ŹLE - jeden test sprawdza 5 różnych zachowań na raz
[Fact]
public void OrderService_DzialaPoprawnie()
{
    var service = new OrderService(/* ... */);

    Assert.Equal(100m, service.CalculateTotal(1));
    Assert.True(service.IsValid(1));
    service.Cancel(1);
    Assert.True(service.IsCancelled(1));
    Assert.Throws<InvalidOperationException>(() => service.CalculateTotal(1));
    // Gdy padnie - który dokładnie fragment zawiódł? Nazwa testu nic nie mówi.
}
```

```csharp
// ✅ DOBRZE - jeden test = jedno zachowanie, czytelna nazwa
[Fact]
public void CalculateTotal_AktywneZamowienie_ZwracaPoprawnaCene() { /* ... */ }

[Fact]
public void Cancel_AktywneZamowienie_UstawiaStatusAnulowany() { /* ... */ }

[Fact]
public void CalculateTotal_AnulowaneZamowienie_RzucaWyjatek() { /* ... */ }
```

**Zasada:** Gdy test pada, jego **nazwa** powinna od razu mówić, co jest nie tak.

---

## 📊 Pokrycie Kodu (Code Coverage) – Korzyści i Pułapki

**Code coverage** mierzy, jaki procent linii/gałęzi kodu wykonał się podczas testów:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### ✅ Korzyści
- Pokazuje kod, który **nigdy nie był testowany** – dobry punkt startowy do analizy ryzyka
- Łatwy do zautomatyzowania w CI/CD jako "bramka jakości"

### ⚠️ Pułapka: 100% Coverage ≠ Dobre Testy

```csharp
// Ten test daje 100% pokrycia linii kodu Divide(), ale...
[Fact]
public void Divide_Test()
{
    var calculator = new Calculator();
    calculator.Divide(10, 2); // Wywołanie bez ŻADNEJ asercji!
}
```

Linia kodu się wykonała (100% coverage!), ale **nic nie zostało zweryfikowane**. To fałszywe poczucie bezpieczeństwa.

**Zasada:** Coverage to **wskaźnik pomocniczy**, nie cel sam w sobie. Zawsze pytaj: *"Czy ten test faktycznie by wykrył błąd, gdyby ktoś zepsuł kod?"*

---

## 💡 Sugestywne Przykłady: Przed i Po

### Przykład 1: Nazewnictwo

| ❌ Słabe | ✅ Dobre |
|---|---|
| `Test1()` | `Withdraw_KwotaWiekszaNizSaldo_RzucaWyjatek()` |
| `TestCalculator()` | `Add_DwieLiczbyDodatnie_ZwracaSume()` |
| `SprawdzWalidacje()` | `IsValid_HasloBezWielkiejLitery_ZwracaFalse()` |

### Przykład 2: Jedna Asercja Logiczna na Test

```csharp
// ✅ Kilka Assert.Equal na RÓŻNE właściwości TEGO SAMEGO obiektu wyniku - to OK
[Fact]
public void CreateOrder_PoprawneDane_TworzyZamowienieZDomyslnymStatusem()
{
    var order = OrderFactory.Create(customerId: 1, amount: 100m);

    Assert.Equal(1, order.CustomerId);
    Assert.Equal(100m, order.Amount);
    Assert.Equal(OrderStatus.Pending, order.Status);
}
```

To wciąż jest **jedna** logiczna weryfikacja: "czy zamówienie zostało poprawnie utworzone" – różni się to od Test Spaghetti, gdzie sprawdzamy **niepowiązane** zachowania.

---

## 🗺️ Checklist Przed Code Review

- [ ] Nazwa testu jasno opisuje scenariusz i oczekiwany wynik
- [ ] Test stosuje strukturę Arrange-Act-Assert
- [ ] Test nie zależy od kolejności wykonania innych testów
- [ ] Zależności zewnętrzne (baza, czas, sieć) są mokowane lub świadomie testowane integracyjnie
- [ ] Test weryfikuje **zachowanie**, nie szczegóły implementacji
- [ ] Test ma dokładnie jedną logiczną odpowiedzialność
- [ ] Wysokie pokrycie kodu nie jest jedynym kryterium jakości

---

## 📝 Podsumowanie Całego Modułu

W tym module poznałeś:
1. **Filozofię testowania** i piramidę testów
2. **TDD** – pisanie testów przed kodem (Red-Green-Refactor)
3. **Asercje i parametryzację** – `Assert.*`, `[Theory]`, `[InlineData]`, `[MemberData]`
4. **Mokowanie zależności** za pomocą Moq – `Setup`, `Verify`, `It.Is`, `Callback`
5. **Nietypowe testy** – wyjątki, async, abstrakcje czasu/losowości, `IClassFixture`
6. **Zarys testów integracyjnych** – kiedy testować z prawdziwym I/O, `WebApplicationFactory`, EF Core InMemory, Testcontainers
7. **Dobre praktyki** – F.I.R.S.T., anti-patterns, pułapki code coverage

Umiejętności z tego modułu możesz od razu zastosować do kodu z **dowolnego** wcześniejszego modułu tego kursu (1-13) oraz do projektu [Moduł A01: ASP.NET Core](../../A01-aspnet_core/README.md).

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md) | [Wróć do głównego README](../../../README.md)
