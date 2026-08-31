using System;
using Xunit;

namespace TestingFundamentals;

// ==================== KOD PRODUKCYJNY ====================

/// <summary>
/// Prosty kalkulator - System Under Test (SUT) dla tego tematu.
/// </summary>
public class Calculator
{
    public int Add(int a, int b) => a + b;

    public int Subtract(int a, int b) => a - b;

    public int Divide(int a, int b)
    {
        if (b == 0)
            throw new DivideByZeroException("Nie można dzielić przez zero.");

        return a / b;
    }
}

// ==================== TESTY XUNIT ====================

public class CalculatorTests
{
    [Fact]
    public void Add_DwieLiczbyDodatnie_ZwracaSume()
    {
        // Arrange
        var calculator = new Calculator();

        // Act
        int result = calculator.Add(2, 3);

        // Assert
        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_LiczbyUjemne_ZwracaPoprawnaSume()
    {
        var calculator = new Calculator();

        int result = calculator.Add(-2, -3);

        Assert.Equal(-5, result);
    }

    [Fact]
    public void Subtract_MniejszaOdWiekszej_ZwracaWartoscUjemna()
    {
        var calculator = new Calculator();

        int result = calculator.Subtract(2, 5);

        Assert.Equal(-3, result);
    }

    [Fact]
    public void Divide_DwieLiczby_ZwracaIloraz()
    {
        var calculator = new Calculator();

        int result = calculator.Divide(10, 2);

        Assert.Equal(5, result);
    }

    [Fact]
    public void Divide_DzielenieprzezZero_RzucaWyjatek()
    {
        var calculator = new Calculator();

        // Assert.Throws sprawdza, czy dany kod rzuca oczekiwany wyjątek
        Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
    }
}

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Temat 1: Testy Jednostkowe - Wprowadzenie ===\n");

        var calculator = new Calculator();

        Console.WriteLine($"Add(2, 3) = {calculator.Add(2, 3)}");
        Console.WriteLine($"Subtract(5, 2) = {calculator.Subtract(5, 2)}");
        Console.WriteLine($"Divide(10, 2) = {calculator.Divide(10, 2)}");

        try
        {
            calculator.Divide(10, 0);
        }
        catch (DivideByZeroException ex)
        {
            Console.WriteLine($"Złapano oczekiwany wyjątek: {ex.Message}");
        }

        Console.WriteLine("\nUruchom 'dotnet test', aby zobaczyć testy xUnit dla tej klasy.");
    }
}
