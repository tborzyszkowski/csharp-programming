# Temat 5: Nietypowe Testy – Wyjątki, Czas, Async

## 🎯 Cel Tematu

Poznasz techniki testowania scenariuszy, które sprawiają początkującym najwięcej trudności: **wyjątki**, **kod asynchroniczny**, **czas systemowy** (`DateTime.Now`) oraz **losowość** (`Random`) – wszystkie są z natury "niedeterministyczne" lub trudne do bezpośredniego zweryfikowania.

### Słowa Kluczowe
- `Assert.Throws` / `Assert.ThrowsAsync`
- Testowanie metod `async Task`
- Wstrzykiwana abstrakcja czasu (`IClock`)
- `IClassFixture<T>` – współdzielony kontekst testów

---

## 💥 Testowanie Wyjątków

```csharp
public class BankAccount
{
    public decimal Balance { get; private set; }

    public void Withdraw(decimal amount)
    {
        if (amount > Balance)
            throw new InvalidOperationException("Niewystarczające środki na koncie.");

        Balance -= amount;
    }
}

[Fact]
public void Withdraw_KwotaWiekszaNizSaldo_RzucaWyjatek()
{
    var account = new BankAccount(); // Balance = 0

    var exception = Assert.Throws<InvalidOperationException>(() => account.Withdraw(100));

    Assert.Equal("Niewystarczające środki na koncie.", exception.Message);
}
```

`Assert.Throws<T>` **zwraca** złapany wyjątek, dzięki czemu można dodatkowo zweryfikować jego `Message` lub inne właściwości.

---

## ⏳ Testowanie Kodu Asynchronicznego

Metody `async Task` testujemy, oznaczając sam test jako `async Task` (nie `async void`!):

```csharp
public class OrderProcessor
{
    public async Task<decimal> GetTotalAsync(int orderId)
    {
        await Task.Delay(10); // symulacja wywołania sieciowego
        return orderId * 10m;
    }

    public async Task ValidateAsync(decimal amount)
    {
        await Task.Delay(5);
        if (amount < 0)
            throw new ArgumentException("Kwota nie może być ujemna.");
    }
}

[Fact]
public async Task GetTotalAsync_ZwracaPoprawnaKwote()
{
    var processor = new OrderProcessor();

    decimal result = await processor.GetTotalAsync(5);

    Assert.Equal(50m, result);
}

[Fact]
public async Task ValidateAsync_UjemnaKwota_RzucaWyjatek()
{
    var processor = new OrderProcessor();

    // Assert.ThrowsAsync dla metod async
    await Assert.ThrowsAsync<ArgumentException>(() => processor.ValidateAsync(-10));
}
```

**Częsty błąd:** oznaczanie testu jako `async void` – xUnit **nie wykryje** wyjątków rzuconych z takiej metody, a test może fałszywie przejść. Zawsze używaj `async Task`.

> Głębszy materiał o async/await znajdziesz w [Module 11](../../11-async/README.md).

---

## 🕐 Problem: Testowanie Kodu Zależnego od `DateTime.Now`

```csharp
public class DiscountCampaign
{
    public bool IsActive()
    {
        // ❌ Trudne do przetestowania - zależy od "teraz"
        return DateTime.Now.Month == 12;
    }
}
```

Nie da się kontrolować `DateTime.Now` w teście – wartość zawsze jest "aktualna". Rozwiązanie: **wstrzyknij abstrakcję czasu**.

### Rozwiązanie: Abstrakcja `IClock`

```csharp
public interface IClock
{
    DateTime Now { get; }
}

public class SystemClock : IClock
{
    public DateTime Now => DateTime.Now; // Prawdziwa implementacja produkcyjna
}

public class DiscountCampaign
{
    private readonly IClock _clock;

    public DiscountCampaign(IClock clock) => _clock = clock;

    public bool IsActive() => _clock.Now.Month == 12;
}
```

W testach wstrzykujemy mocka (patrz [Temat 4](../_04_mocking_dependencies/README.md)) zamiast prawdziwego zegara:

```csharp
[Fact]
public void IsActive_Grudzien_ZwracaTrue()
{
    var mockClock = new Mock<IClock>();
    mockClock.Setup(c => c.Now).Returns(new DateTime(2026, 12, 15));

    var campaign = new DiscountCampaign(mockClock.Object);

    Assert.True(campaign.IsActive());
}

[Fact]
public void IsActive_Czerwiec_ZwracaFalse()
{
    var mockClock = new Mock<IClock>();
    mockClock.Setup(c => c.Now).Returns(new DateTime(2026, 6, 1));

    var campaign = new DiscountCampaign(mockClock.Object);

    Assert.False(campaign.IsActive());
}
```

Teraz test jest **w pełni deterministyczny** – nie zależy od tego, kiedy faktycznie go uruchomimy.

Ta sama technika działa dla `Random` (abstrakcja `IRandomProvider`) i innych "globalnych", niedeterministycznych zależności.

---

## 🏗️ `IClassFixture<T>` – Współdzielony Kontekst Testów

Gdy przygotowanie danych testowych jest **kosztowne** (np. otwarcie połączenia), a wiele testów może z niego bezpiecznie korzystać wielokrotnie, używamy `IClassFixture<T>`:

```csharp
public class DatabaseFixture : IDisposable
{
    public List<string> SeedData { get; } = new() { "Alice", "Bob", "Carol" };

    public DatabaseFixture()
    {
        // Kosztowna inicjalizacja - wykonywana RAZ dla całej klasy testowej
        Console.WriteLine("Inicjalizacja danych testowych...");
    }

    public void Dispose()
    {
        Console.WriteLine("Sprzątanie po testach...");
    }
}

public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    // xUnit automatycznie wstrzykuje tę samą instancję fixture do konstruktora
    public UserRepositoryTests(DatabaseFixture fixture) => _fixture = fixture;

    [Fact]
    public void SeedData_ZawieraAlice()
    {
        Assert.Contains("Alice", _fixture.SeedData);
    }

    [Fact]
    public void SeedData_MaTrzyElementy()
    {
        Assert.Equal(3, _fixture.SeedData.Count);
    }
}
```

`DatabaseFixture` jest tworzony **raz** dla wszystkich testów w klasie `UserRepositoryTests`, a nie od nowa przed każdym testem (co jest domyślnym zachowaniem xUnit dla samej klasy testowej).

---

## 📝 Podsumowanie

- `Assert.Throws<T>` / `Assert.ThrowsAsync<T>` weryfikują wyjątki (synchroniczne i asynchroniczne)
- Testy metod `async` muszą same być `async Task` – nigdy `async void`
- Niedeterministyczne zależności (czas, losowość) izolujemy przez abstrakcje (`IClock`, `IRandomProvider`) i mokujemy je
- `IClassFixture<T>` współdzieli kosztowny kontekst pomiędzy testami tej samej klasy

**Następny temat:** [Zarys Testów Integracyjnych](../_06_integration_tests_overview/README.md)

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md)
