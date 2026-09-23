# Temat 6: Zarys Testów Integracyjnych

## 🎯 Cel Tematu

Poznasz różnicę między testami jednostkowymi a **integracyjnymi**, zobaczysz jak wygląda prosty test integracyjny z realnym I/O oraz otrzymasz **zarys** narzędzi używanych do testowania integracyjnego w rzeczywistych aplikacjach ASP.NET Core (`WebApplicationFactory`, EF Core InMemory, Testcontainers).

> To temat **przeglądowy** – celem jest zrozumienie koncepcji i wiedza "co i kiedy użyć", a nie wyczerpujące pokrycie każdego narzędzia.

### Słowa Kluczowe
- Test integracyjny vs test jednostkowy
- Fake (dubler w pamięci) vs prawdziwa infrastruktura
- `WebApplicationFactory<T>`
- EF Core InMemory Provider, Testcontainers

---

## 🆚 Test Jednostkowy vs Test Integracyjny

| Cecha | Test Jednostkowy | Test Integracyjny |
|---|---|---|
| Zakres | Jedna klasa/metoda, w izolacji | Współpraca kilku komponentów |
| Zależności | Mokowane (Moq) | Prawdziwe lub zbliżone do prawdziwych (baza, plik, HTTP) |
| Szybkość | Milisekundy | Setki milisekund – sekundy |
| Liczba w projekcie | Bardzo dużo | Znacznie mniej (patrz piramida testów, [Temat 1](../_01_testing_fundamentals/README.md)) |
| Co wykrywa | Błędy logiki biznesowej | Błędy konfiguracji, mapowania, integracji między warstwami |

**Przykład różnicy na tym samym kodzie:**

```csharp
// Test JEDNOSTKOWY - IOrderRepository jest mokowany (Temat 4)
[Fact]
public void CalculateTotal_ZMokowanymRepozytorium_ZwracaPoprawnaCene()
{
    var mockRepo = new Mock<IOrderRepository>();
    mockRepo.Setup(r => r.GetById(1)).Returns(new Order { Amount = 100m });
    var service = new OrderService(mockRepo.Object, new Mock<IDiscountService>().Object);

    // ...
}

// Test INTEGRACYJNY - używa PRAWDZIWEJ implementacji z prawdziwym I/O
[Fact]
public void SaveAndLoad_PrawdziwyPlikowyRepozytorium_ZwracaZapisaneDane()
{
    var repository = new FileOrderRepository("test_orders.json"); // prawdziwy zapis na dysk!
    repository.Save(new Order { CustomerId = 1, Amount = 100m });

    var loaded = repository.GetById(1);

    Assert.Equal(100m, loaded.Amount);
}
```

---

## 💾 Przykład: Test Integracyjny z Prawdziwym Plikiem

Poniższy przykład (pełny kod w `code/Program.cs`) pokazuje test integracyjny bez potrzeby stawiania bazy danych – wystarczy prawdziwy system plików:

```csharp
public class FileOrderRepository
{
    private readonly string _filePath;

    public FileOrderRepository(string filePath) => _filePath = filePath;

    public void Save(Order order)
    {
        var json = JsonSerializer.Serialize(order);
        File.WriteAllText(_filePath, json);
    }

    public Order? Load()
    {
        if (!File.Exists(_filePath))
            return null;

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<Order>(json);
    }
}

[Fact]
public void SaveAndLoad_PrawdziwyPlik_OdczytujeZapisaneZamowienie()
{
    // Arrange - unikalna, tymczasowa ścieżka na każde uruchomienie testu
    string tempFile = Path.Combine(Path.GetTempPath(), $"order_{Guid.NewGuid()}.json");
    var repository = new FileOrderRepository(tempFile);

    try
    {
        // Act - PRAWDZIWY zapis i odczyt z dysku
        repository.Save(new Order { CustomerId = 1, Amount = 250m });
        var loaded = repository.Load();

        // Assert
        Assert.NotNull(loaded);
        Assert.Equal(250m, loaded!.Amount);
    }
    finally
    {
        // Sprzątanie - test integracyjny odpowiada za swoje efekty uboczne
        if (File.Exists(tempFile))
            File.Delete(tempFile);
    }
}
```

**Kluczowa różnica:** ten test **naprawdę** zapisuje i czyta plik z dysku – weryfikuje, że serializacja, ścieżki i uprawnienia do zapisu **rzeczywiście** działają razem. Test jednostkowy z mockiem `IOrderRepository` tego by nie wykrył.

---

## 🌐 Zarys: Testy Integracyjne w ASP.NET Core

W pełnej aplikacji webowej (patrz [Moduł A01](../../A01-aspnet_core/README.md)) testy integracyjne zwykle korzystają z:

### `WebApplicationFactory<TEntryPoint>`

Uruchamia całą aplikację ASP.NET Core "w pamięci" (bez prawdziwego serwera sieciowego) i pozwala wysyłać do niej prawdziwe żądania HTTP:

```csharp
public class ProjectsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProjectsControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProjects_ZwracaStatus200()
    {
        var response = await _client.GetAsync("/api/projects");

        response.EnsureSuccessStatusCode();
    }
}
```

### EF Core InMemory Provider

Zamiast prawdziwej bazy SQL Server, testy integracyjne mogą użyć bazy danych "w pamięci" – szybszej niż prawdziwa baza, ale wciąż testującej realną warstwę Entity Framework Core (zapytania LINQ, mapowanie):

```csharp
var options = new DbContextOptionsBuilder<ApplicationDbContext>()
    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
    .Options;

using var context = new ApplicationDbContext(options);
context.Projects.Add(new Project { Name = "Test Project" });
context.SaveChanges();

Assert.Single(context.Projects);
```

> **Uwaga:** InMemory Provider nie wychwytuje wszystkich problemów specyficznych dla prawdziwej bazy SQL (np. ograniczeń kluczy obcych). Do pełnej wierności środowiska produkcyjnego służy **Testcontainers**.

### Testcontainers – Prawdziwa Baza Danych w Kontenerze Docker

**Testcontainers** uruchamia prawdziwy silnik bazy danych (np. PostgreSQL, SQL Server) w tymczasowym kontenerze Docker na czas testów – najbliższe środowisku produkcyjnemu, kosztem wolniejszego wykonania:

```csharp
// Koncepcyjny przykład - wymaga pakietu Testcontainers.PostgreSql
await using var postgres = new PostgreSqlBuilder().Build();
await postgres.StartAsync();

var connectionString = postgres.GetConnectionString();
// ... użyj connectionString do skonfigurowania prawdziwego DbContext
```

---

## 🧭 Kiedy Wybrać Który Rodzaj Testu?

```
Testujesz czystą logikę biznesową (obliczenia, walidacje)?
    → Test jednostkowy + mocki (Tematy 1-5)

Testujesz zapytania LINQ / mapowanie EF Core?
    → Test integracyjny z InMemory Provider (szybki, przybliżony)

Testujesz zachowanie specyficzne dla silnika bazy danych
(ograniczenia, transakcje, konkurencja)?
    → Test integracyjny z Testcontainers (wolniejszy, wierny produkcji)

Testujesz cały endpoint HTTP (routing, middleware, autoryzacja)?
    → Test integracyjny z WebApplicationFactory
```

---

## 📝 Podsumowanie

- Testy integracyjne weryfikują **współpracę** komponentów z prawdziwą (lub zbliżoną) infrastrukturą
- Są wolniejsze i mniej liczne niż testy jednostkowe – zgodnie z piramidą testów
- `WebApplicationFactory` – testuje całą aplikację ASP.NET Core "w pamięci" przez HTTP
- EF Core InMemory Provider – szybkie przybliżenie prawdziwej bazy danych
- Testcontainers – najwyższa wierność środowisku produkcyjnemu, kosztem czasu wykonania

**Następny temat:** [Dobre Praktyki i Sugestywne Przykłady](../_07_best_practices_examples/README.md)

[Przejdź do zadań](tasks/EXERCISES.md) | [Wróć do modułu](../README.md)
