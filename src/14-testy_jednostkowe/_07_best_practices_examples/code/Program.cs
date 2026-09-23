using System;
using Moq;
using Xunit;

namespace BestPracticesExamples;

// ==================== KOD PRODUKCYJNY ====================

public enum OrderStatus { Pending, Cancelled }

public class Order
{
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
}

public static class OrderFactory
{
    public static Order Create(int customerId, decimal amount) =>
        new() { CustomerId = customerId, Amount = amount, Status = OrderStatus.Pending };
}

public interface IOrderRepository
{
    void Save(Order order);
}

public interface IAppLogger
{
    void Log(string message);
}

public class OrderProcessor
{
    private readonly IOrderRepository _repository;
    private readonly IAppLogger _logger;

    public OrderProcessor(IOrderRepository repository, IAppLogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public void Process(Order order)
    {
        _logger.Log("Start");
        _repository.Save(order);
        _logger.Log("End");
    }
}

// ==================== ✅ DOBRE TESTY (wzorcowe dla tego tematu) ====================

public class OrderProcessorTests
{
    [Fact]
    public void Process_PoprawneZamowienie_ZapisujeZamowienie()
    {
        // Arrange
        var mockRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<IAppLogger>();
        var processor = new OrderProcessor(mockRepository.Object, mockLogger.Object);
        var order = OrderFactory.Create(customerId: 1, amount: 100m);

        // Act
        processor.Process(order);

        // Assert - weryfikujemy ZACHOWANIE (że zamówienie zostało zapisane),
        // a NIE dokładną kolejność wywołań logera (to byłby Fragile Test)
        mockRepository.Verify(r => r.Save(order), Times.Once);
    }
}

public class OrderFactoryTests
{
    [Fact]
    public void Create_PoprawneDane_TworzyZamowienieZDomyslnymStatusem()
    {
        var order = OrderFactory.Create(customerId: 1, amount: 100m);

        // Kilka asercji na TEN SAM logiczny fakt "zamówienie utworzone poprawnie" - to OK,
        // to nie jest Test Spaghetti (patrz README tego tematu)
        Assert.Equal(1, order.CustomerId);
        Assert.Equal(100m, order.Amount);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }
}

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Temat 7: Dobre Praktyki i Sugestywne Przykłady ===\n");

        var mockRepository = new Mock<IOrderRepository>();
        var mockLogger = new Mock<IAppLogger>();
        var processor = new OrderProcessor(mockRepository.Object, mockLogger.Object);

        var order = OrderFactory.Create(customerId: 1, amount: 100m);
        processor.Process(order);

        Console.WriteLine($"Zamówienie utworzone: CustomerId={order.CustomerId}, Amount={order.Amount}, Status={order.Status}");
        Console.WriteLine("\nUruchom 'dotnet test' - zobaczysz testy zgodne z zasadami F.I.R.S.T.");
        Console.WriteLine("Porównaj je z przykładami anti-patterns opisanymi w README.md tego tematu.");
    }
}
