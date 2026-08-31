using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace UnusualTests;

// ==================== KOD PRODUKCYJNY ====================

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

public class OrderProcessor
{
    public async Task<decimal> GetTotalAsync(int orderId)
    {
        await Task.Delay(10);
        return orderId * 10m;
    }

    public async Task ValidateAsync(decimal amount)
    {
        await Task.Delay(5);
        if (amount < 0)
            throw new ArgumentException("Kwota nie może być ujemna.");
    }
}

public interface IClock
{
    DateTime Now { get; }
}

public class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}

public class DiscountCampaign
{
    private readonly IClock _clock;

    public DiscountCampaign(IClock clock) => _clock = clock;

    public bool IsActive() => _clock.Now.Month == 12;
}

// ==================== FIXTURE ====================

public class DatabaseFixture : IDisposable
{
    public List<string> SeedData { get; } = new() { "Alice", "Bob", "Carol" };

    public DatabaseFixture()
    {
        Console.WriteLine("[Fixture] Inicjalizacja danych testowych...");
    }

    public void Dispose()
    {
        Console.WriteLine("[Fixture] Sprzątanie po testach...");
    }
}

// ==================== TESTY XUNIT ====================

public class BankAccountTests
{
    [Fact]
    public void Withdraw_KwotaWiekszaNizSaldo_RzucaWyjatek()
    {
        var account = new BankAccount();

        var exception = Assert.Throws<InvalidOperationException>(() => account.Withdraw(100));

        Assert.Equal("Niewystarczające środki na koncie.", exception.Message);
    }
}

public class OrderProcessorTests
{
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

        await Assert.ThrowsAsync<ArgumentException>(() => processor.ValidateAsync(-10));
    }
}

public class DiscountCampaignTests
{
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
}

public class UserRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

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

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("=== Temat 5: Nietypowe Testy ===\n");

        var account = new BankAccount();
        try
        {
            account.Withdraw(100);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Złapano wyjątek: {ex.Message}");
        }

        var processor = new OrderProcessor();
        decimal total = await processor.GetTotalAsync(5);
        Console.WriteLine($"GetTotalAsync(5) = {total}");

        var mockClock = new Mock<IClock>();
        mockClock.Setup(c => c.Now).Returns(new DateTime(2026, 12, 15));
        var campaign = new DiscountCampaign(mockClock.Object);
        Console.WriteLine($"IsActive() w grudniu (mock) = {campaign.IsActive()}");

        Console.WriteLine("\nUruchom 'dotnet test', aby zobaczyć testy wyjątków, async i IClassFixture.");
    }
}
