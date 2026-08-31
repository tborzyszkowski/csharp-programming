using System;
using System.IO;
using System.Text.Json;
using Moq;
using Xunit;

namespace IntegrationTestsOverview;

// ==================== MODELE I INTERFEJSY ====================

public class Order
{
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
}

public interface IOrderRepository
{
    Order? GetById(int orderId);
}

public interface IDiscountService
{
    int GetDiscountPercent(int customerId);
}

// ==================== KOD PRODUKCYJNY ====================

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IDiscountService _discountService;

    public OrderService(IOrderRepository repository, IDiscountService discountService)
    {
        _repository = repository;
        _discountService = discountService;
    }

    public decimal CalculateTotal(int orderId)
    {
        var order = _repository.GetById(orderId)
            ?? throw new InvalidOperationException("Zamówienie nie istnieje.");

        int discount = _discountService.GetDiscountPercent(order.CustomerId);

        return order.Amount - order.Amount * discount / 100m;
    }
}

/// <summary>
/// Prawdziwa implementacja repozytorium - zapisuje/odczytuje z dysku (System pod testem integracyjnym).
/// </summary>
public class FileOrderRepository : IOrderRepository
{
    private readonly string _filePath;

    public FileOrderRepository(string filePath) => _filePath = filePath;

    public void Save(Order order)
    {
        var json = JsonSerializer.Serialize(order);
        File.WriteAllText(_filePath, json);
    }

    public Order? GetById(int orderId)
    {
        if (!File.Exists(_filePath))
            return null;

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<Order>(json);
    }
}

// ==================== TESTY JEDNOSTKOWE (mockowane zależności) ====================

public class OrderServiceUnitTests
{
    [Fact]
    public void CalculateTotal_ZMokowanymRepozytorium_ZwracaPoprawnaCene()
    {
        var mockRepo = new Mock<IOrderRepository>();
        var mockDiscount = new Mock<IDiscountService>();

        mockRepo.Setup(r => r.GetById(1)).Returns(new Order { CustomerId = 1, Amount = 100m });
        mockDiscount.Setup(d => d.GetDiscountPercent(1)).Returns(0);

        var service = new OrderService(mockRepo.Object, mockDiscount.Object);

        decimal result = service.CalculateTotal(1);

        Assert.Equal(100m, result);
    }
}

// ==================== TESTY INTEGRACYJNE (prawdziwe I/O) ====================

public class FileOrderRepositoryIntegrationTests
{
    [Fact]
    public void SaveAndGetById_PrawdziwyPlik_OdczytujeZapisaneZamowienie()
    {
        // Arrange - unikalna, tymczasowa ścieżka na każde uruchomienie testu
        string tempFile = Path.Combine(Path.GetTempPath(), $"order_{Guid.NewGuid()}.json");
        var repository = new FileOrderRepository(tempFile);

        try
        {
            // Act - PRAWDZIWY zapis i odczyt z dysku
            repository.Save(new Order { CustomerId = 1, Amount = 250m });
            var loaded = repository.GetById(1);

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

    [Fact]
    public void GetById_PlikNieIstnieje_ZwracaNull()
    {
        string tempFile = Path.Combine(Path.GetTempPath(), $"order_{Guid.NewGuid()}.json");
        var repository = new FileOrderRepository(tempFile);

        var loaded = repository.GetById(1);

        Assert.Null(loaded);
    }
}

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Temat 6: Zarys Testów Integracyjnych ===\n");

        string tempFile = Path.Combine(Path.GetTempPath(), "demo_order.json");
        var repository = new FileOrderRepository(tempFile);

        repository.Save(new Order { CustomerId = 1, Amount = 250m });
        var loaded = repository.GetById(1);

        Console.WriteLine($"Zapisano i odczytano z prawdziwego pliku: {tempFile}");
        Console.WriteLine($"Odczytana kwota: {loaded?.Amount}");

        File.Delete(tempFile);

        Console.WriteLine("\nUruchom 'dotnet test' - zobaczysz zarówno testy jednostkowe (mockowane),");
        Console.WriteLine("jak i integracyjne (prawdziwy zapis/odczyt z dysku).");
    }
}
