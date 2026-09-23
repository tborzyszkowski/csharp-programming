using System;
using System.Collections.Generic;
using Xunit;

namespace AssertionsAndOrganization;

// ==================== KOD PRODUKCYJNY ====================

public class AgeValidator
{
    public bool IsAdult(int age) => age >= 18;
}

public class DiscountCalculator
{
    public decimal ApplyDiscount(decimal price, int discountPercent)
    {
        if (discountPercent is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(discountPercent));

        return price - price * discountPercent / 100m;
    }
}

// ==================== TESTY XUNIT ====================

public class AgeValidatorTests
{
    [Theory]
    [InlineData(17, false)]
    [InlineData(18, true)]
    [InlineData(65, true)]
    [InlineData(0, false)]
    public void IsAdult_RoznyWiek_ZwracaPoprawnyWynik(int age, bool expected)
    {
        var validator = new AgeValidator();

        bool result = validator.IsAdult(age);

        Assert.Equal(expected, result);
    }
}

public class DiscountCalculatorTests
{
    public static IEnumerable<object[]> DiscountScenarios =>
        new List<object[]>
        {
            new object[] { 100m, 0, 100m },
            new object[] { 100m, 10, 90m },
            new object[] { 200m, 50, 100m },
        };

    [Theory]
    [MemberData(nameof(DiscountScenarios))]
    public void ApplyDiscount_RoznePrzypadki_ZwracaPoprawnaCene(
        decimal price, int discountPercent, decimal expected)
    {
        var calculator = new DiscountCalculator();

        decimal result = calculator.ApplyDiscount(price, discountPercent);

        Assert.Equal(expected, result);
    }

    [Fact]
    public void ApplyDiscount_UjemnyProcent_RzucaWyjatek()
    {
        var calculator = new DiscountCalculator();

        Assert.Throws<ArgumentOutOfRangeException>(() => calculator.ApplyDiscount(100m, -5));
    }

    [Fact]
    public void ApplyDiscount_ListaCen_NieJestPusta()
    {
        var prices = new List<decimal> { 10m, 20m };

        Assert.NotEmpty(prices);
        Assert.Contains(10m, prices);
    }
}

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Temat 3: Asercje, Organizacja i Parametryzacja ===\n");

        var validator = new AgeValidator();
        Console.WriteLine($"IsAdult(17) = {validator.IsAdult(17)}");
        Console.WriteLine($"IsAdult(18) = {validator.IsAdult(18)}");

        var calculator = new DiscountCalculator();
        Console.WriteLine($"ApplyDiscount(100, 10%) = {calculator.ApplyDiscount(100m, 10)}");

        Console.WriteLine("\nUruchom 'dotnet test' - zobaczysz osobny wynik dla każdego [InlineData]/[MemberData].");
    }
}
