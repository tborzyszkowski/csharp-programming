using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace MockingDependencies;

// ==================== MODELE I INTERFEJSY ====================

public class Order
{
    public int CustomerId { get; set; }
    public decimal Amount { get; set; }
}

public interface IOrderRepository
{
    Order GetById(int orderId);
}

public interface IDiscountService
{
    int GetDiscountPercent(int customerId);
}

public interface IEmailSender
{
    void Send(string to, string subject);
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
        var order = _repository.GetById(orderId);
        int discount = _discountService.GetDiscountPercent(order.CustomerId);

        return order.Amount - order.Amount * discount / 100m;
    }
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

// ==================== TESTY XUNIT ====================

public class OrderServiceTests
{
    [Fact]
    public void CalculateTotal_ZamowienieZeRabatem_ZwracaPoprawnaCene()
    {
        // Arrange
        var mockRepository = new Mock<IOrderRepository>();
        var mockDiscountService = new Mock<IDiscountService>();

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

    [Fact]
    public void CalculateTotal_BrakRabatu_ZwracaPelnaCene()
    {
        var mockRepository = new Mock<IOrderRepository>();
        var mockDiscountService = new Mock<IDiscountService>();

        mockRepository
            .Setup(r => r.GetById(2))
            .Returns(new Order { CustomerId = 7, Amount = 50m });

        mockDiscountService
            .Setup(d => d.GetDiscountPercent(7))
            .Returns(0);

        var service = new OrderService(mockRepository.Object, mockDiscountService.Object);

        decimal result = service.CalculateTotal(2);

        Assert.Equal(50m, result);
    }
}

public class OrderNotifierTests
{
    [Fact]
    public void NotifyCustomer_DuzeZamowienie_WysylaEmail()
    {
        var mockEmailSender = new Mock<IEmailSender>();
        var notifier = new OrderNotifier(mockEmailSender.Object);

        notifier.NotifyCustomer("jan@example.com", 150m);

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

    [Fact]
    public void NotifyCustomer_ZapisujeArgumentyWCallbacku()
    {
        var callLog = new List<string>();
        var mockEmailSender = new Mock<IEmailSender>();

        mockEmailSender
            .Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>()))
            .Callback<string, string>((to, subject) => callLog.Add($"{to}: {subject}"));

        var notifier = new OrderNotifier(mockEmailSender.Object);
        notifier.NotifyCustomer("anna@example.com", 200m);

        Assert.Single(callLog);
        Assert.Contains("anna@example.com", callLog[0]);
    }
}

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Temat 4: Mokowanie Zależności z Moq ===\n");

        var mockRepository = new Mock<IOrderRepository>();
        var mockDiscountService = new Mock<IDiscountService>();

        mockRepository.Setup(r => r.GetById(1)).Returns(new Order { CustomerId = 42, Amount = 200m });
        mockDiscountService.Setup(d => d.GetDiscountPercent(42)).Returns(10);

        var service = new OrderService(mockRepository.Object, mockDiscountService.Object);
        Console.WriteLine($"CalculateTotal(1) z mockami = {service.CalculateTotal(1)} zł");

        Console.WriteLine("\nUruchom 'dotnet test', aby zobaczyć pełny zestaw testów z Moq.");
    }
}
