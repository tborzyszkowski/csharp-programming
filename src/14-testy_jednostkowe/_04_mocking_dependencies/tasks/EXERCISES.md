# Ćwiczenia - Mokowanie Zależności z Moq

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 4.1: Pierwszy Mock
**Cel:** Napisać test z pojedynczym mockiem.

Mając interfejs:
```csharp
public interface IWeatherApi
{
    int GetTemperature(string city);
}

public class WeatherAdvisor
{
    private readonly IWeatherApi _api;
    public WeatherAdvisor(IWeatherApi api) => _api = api;

    public string GetAdvice(string city)
    {
        int temp = _api.GetTemperature(city);
        return temp < 0 ? "Ubierz się ciepło!" : "Możesz wyjść w lekkiej kurtce.";
    }
}
```

Napisz dwa testy z `Mock<IWeatherApi>`: jeden dla temperatury ujemnej, drugi dla dodatniej.

---

### Ćwiczenie 4.2: `Verify` z `Times.Never`
**Cel:** Sprawdzić, że metoda NIE została wywołana.

Dla `OrderNotifier` z tego tematu napisz test sprawdzający, że dla zamówienia o wartości dokładnie `100m` (brzegowy przypadek) e-mail **nie** jest wysyłany (sprawdź warunek `> 100` w kodzie).

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 4.3: Mokowanie Wielu Zależności
**Cel:** Przetestować klasę z dwiema zależnościami, gdzie jedna zależy od wyniku drugiej.

Zaimplementuj `LoyaltyPointsService` zależny od `IOrderRepository` (pobiera kwotę zamówienia) i `ILoyaltyRulesEngine` (przelicza kwotę na punkty, np. `GetPoints(decimal amount)`). Napisz test, w którym oba mocki współpracują, by zwrócić oczekiwaną liczbę punktów.

---

### Ćwiczenie 4.4: `It.Is` z Predykatem
**Cel:** Zweryfikować wywołanie z konkretnym warunkiem na argumencie.

Dla `OrderNotifier.NotifyCustomer`, napisz test weryfikujący za pomocą `It.Is<string>(subject => subject.Contains("zamówienie"))`, że temat wiadomości e-mail zawiera słowo "zamówienie".

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 4.5: Refaktoryzacja z Wielu Mocków do Jednego Fasady
**Cel:** Rozpoznać nadmierne mokowanie jako code smell.

Zaimplementuj `ReportGenerator`, który zależy od **czterech** interfejsów: `IOrderRepository`, `ICustomerRepository`, `IDiscountService`, `IEmailSender`. Napisz test z czterema mockami dla jednej metody `GenerateAndSendReport()`.

Następnie zaproponuj (w komentarzu) refaktoryzację – np. wydzielenie klasy `ReportDataProvider` łączącej pierwsze trzy zależności – tak, aby docelowa klasa `ReportGenerator` potrzebowała mokować tylko **dwie** zależności zamiast czterech. Uzasadnij, dlaczego mniej mocków w jednym teście zwykle oznacza lepszy design (Single Responsibility Principle).

**Podpowiedź:** To ćwiczenie łączy wiedzę z tego tematu z zasadami SOLID z [Modułu 7](../../../07-interfejsy_abstrakcje/README.md).
