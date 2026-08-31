# Temat 4: Mokowanie Zależności z Moq

## 🎯 Cel Tematu

Nauczysz się **izolować** testowaną klasę od jej zależności (baza danych, API, czas systemowy) za pomocą **mocków** tworzonych biblioteką **Moq** – najpopularniejszą biblioteką mockującą w .NET.

### Słowa Kluczowe
- Test double (dubler testowy): mock, stub, fake, spy
- `Mock<T>`, `Setup`, `Returns`, `Verify`, `Callback`, `It.Is`
- Dependency Injection jako fundament testowalności

---

## 📖 Problem: Zależności Utrudniają Testowanie

```csharp
public class OrderService
{
    public decimal CalculateTotal(int orderId)
    {
        // Zależność od prawdziwej bazy danych!
        var order = new SqlOrderRepository().GetById(orderId);
        var discount = new ExternalDiscountApi().GetDiscount(order.CustomerId);

        return order.Amount - order.Amount * discount / 100m;
    }
}
```

Aby przetestować `CalculateTotal`, musielibyśmy mieć **prawdziwą bazę danych** i **działające zewnętrzne API**. To sprawia, że test jest:
- Wolny (sieć, dysk)
- Niestabilny (API może nie działać, dane w bazie mogą się zmienić)
- Trudny do przygotowania (trzeba zaseedować konkretne dane)

**To już nie jest test jednostkowy – to test integracyjny w przebraniu.**

---

## 🔌 Rozwiązanie: Zależności Przez Interfejsy

Warunkiem mokowania jest programowanie **względem interfejsów**, a nie konkretnych implementacji (patrz [Moduł 7](../../07-interfejsy_abstrakcje/README.md)):

```csharp
public interface IOrderRepository
{
    Order GetById(int orderId);
}

public interface IDiscountService
{
    int GetDiscountPercent(int customerId);
}

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IDiscountService _discountService;

    // Dependency Injection przez konstruktor
    public OrderService(IOrderRepository repository, IDiscountService discountService)
    {
        _repository = repository;
        _discountService = discountService;
    }

    public decimal CalculateTotal(int orderId)
    {
        var order = _repository.GetById(orderId);
        int discount = _discountService.GetDiscountPercent(order.CustomerId);

        return order.Amount - order.Amount * discount / 100m;
    }
}
```

Teraz `OrderService` **nie wie**, czy `IOrderRepository` to prawdziwa baza SQL, czy atrapa w pamięci. To jest właśnie **Dependency Inversion** – jeden z filarów SOLID.

---

## 🧪 Moq: Tworzenie Mocków

Dodaj pakiet:

```bash
dotnet add package Moq
```

### Podstawowy Mock

```csharp
[Fact]
public void CalculateTotal_ZamowienieZeRabatem_ZwracaPoprawnaCene()
{
    // Arrange
    var mockRepository = new Mock<IOrderRepository>();
    var mockDiscountService = new Mock<IDiscountService>();

    // Setup - definiujemy zachowanie mocka
    mockRepository
        .Setup(r => r.GetById(1))
        .Returns(new Order { CustomerId = 42, Amount = 200m });

    mockDiscountService
        .Setup(d => d.GetDiscountPercent(42))
        .Returns(10);

    var service = new OrderService(mockRepository.Object, mockDiscountService.Object);

    // Act
    decimal result = service.CalculateTotal(1);

    // Assert
    Assert.Equal(180m, result);
}
```

**Kluczowe elementy:**
- `new Mock<IOrderRepository>()` – tworzy atrapę implementującą interfejs
- `.Setup(...)` – definiuje, co ma się stać po wywołaniu danej metody
- `.Returns(...)` – zwracana wartość
- `mockRepository.Object` – właściwa instancja `IOrderRepository` do wstrzyknięcia

---

## ✅ `Verify` – Sprawdzanie, Czy Metoda Została Wywołana

Czasem interesuje nas nie *co* metoda zwróciła, ale *czy w ogóle została wywołana* (np. logowanie, wysyłka e-maila):

```csharp
public interface IEmailSender
{
    void Send(string to, string subject);
}

public class OrderNotifier
{
    private readonly IEmailSender _emailSender;

    public OrderNotifier(IEmailSender emailSender) => _emailSender = emailSender;

    public void NotifyCustomer(string email, decimal total)
    {
        if (total > 100)
            _emailSender.Send(email, $"Twoje zamówienie: {total} zł");
    }
}

[Fact]
public void NotifyCustomer_DuzeZamowienie_WysylaEmail()
{
    var mockEmailSender = new Mock<IEmailSender>();
    var notifier = new OrderNotifier(mockEmailSender.Object);

    notifier.NotifyCustomer("jan@example.com", 150m);

    // Verify - sprawdzamy, czy Send zostało wywołane DOKŁADNIE raz z tymi argumentami
    mockEmailSender.Verify(
        e => e.Send("jan@example.com", It.IsAny<string>()),
        Times.Once);
}

[Fact]
public void NotifyCustomer_MaleZamowienie_NieWysylaEmaila()
{
    var mockEmailSender = new Mock<IEmailSender>();
    var notifier = new OrderNotifier(mockEmailSender.Object);

    notifier.NotifyCustomer("jan@example.com", 50m);

    mockEmailSender.Verify(
        e => e.Send(It.IsAny<string>(), It.IsAny<string>()),
        Times.Never);
}
```

`It.IsAny<string>()` oznacza "dowolna wartość typu string" – używamy go, gdy nie chcemy sprawdzać konkretnego argumentu.

---

## 🎛️ `It.Is` – Precyzyjne Dopasowanie Argumentów

```csharp
mockEmailSender.Verify(
    e => e.Send(
        It.Is<string>(to => to.EndsWith("@example.com")),
        It.IsAny<string>()),
    Times.Once);
```

---

## 🔄 `Callback` – Reakcja na Wywołanie

```csharp
var callLog = new List<string>();

mockEmailSender
    .Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>()))
    .Callback<string, string>((to, subject) => callLog.Add($"{to}: {subject}"));
```

Przydatne, gdy chcemy zarejestrować argumenty wywołania do dalszej analizy w teście.

---

## 🧭 Mock vs Stub vs Fake – Terminologia

| Rodzaj | Cel | Przykład |
|---|---|---|
| **Stub** | Zwraca z góry ustalone dane | `mockRepository.Setup(...).Returns(...)` |
| **Mock** | Weryfikuje interakcje (czy metoda została wywołana) | `mockEmailSender.Verify(...)` |
| **Fake** | Uproszczona, działająca implementacja (np. lista w pamięci zamiast bazy) | Zobacz [Temat 6](../_06_integration_tests_overview/README.md) |

Biblioteka Moq potrafi pełnić rolę zarówno stuba (przez `Setup...Returns`), jak i mocka (przez `Verify`) – stąd nazwa "Mock" w praktyce obejmuje oba znaczenia.

---

## ⚠️ Kiedy NIE Mokować?

- Nie mokuj prostych obiektów wartości (np. `DateTime`, `string`) – to nie są zależności, tylko dane
- Nie mokuj klasy, którą właśnie testujesz (System Under Test)
- Unikaj nadmiernego mokowania – jeśli test ma 10 mocków, prawdopodobnie klasa robi za dużo (narusza Single Responsibility Principle)

---

## 📝 Podsumowanie

- Mokowanie wymaga programowania względem **interfejsów** i wstrzykiwania zależności przez konstruktor
- `Mock<T>.Setup(...).Returns(...)` – definiuje zachowanie (rola stuba)
- `Mock<T>.Verify(...)` – sprawdza, czy metoda została wywołana (rola mocka)
- `It.IsAny<T>()` i `It.Is<T>(predicate)` kontrolują dopasowanie argumentów
- Zbyt wiele mocków w jednym teście to sygnał, że klasa może naruszać SRP

**Następny temat:** [Nietypowe Testy: Wyjątki, Czas, Async](../_05_unusual_tests/README.md)

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md)
