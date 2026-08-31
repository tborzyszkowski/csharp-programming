using System;
using System.Linq;
using Xunit;

namespace TddRedGreenRefactor;

// ==================== KOD PRODUKCYJNY (WYNIK CYKLU TDD) ====================

/// <summary>
/// Zaimplementowane w cyklu Red-Green-Refactor - zobacz README.md tego tematu
/// dla pełnej historii kolejnych kroków.
/// </summary>
public class StringCalculator
{
    private const char Separator = ',';

    public int Add(string numbers)
    {
        if (string.IsNullOrEmpty(numbers))
            return 0;

        return numbers.Split(Separator).Select(int.Parse).Sum();
    }
}

// ==================== TESTY XUNIT (NAPISANE PRZED KODEM) ====================

public class StringCalculatorTests
{
    [Fact]
    public void Add_PustyString_ZwracaZero()
    {
        var calculator = new StringCalculator();

        int result = calculator.Add("");

        Assert.Equal(0, result);
    }

    [Fact]
    public void Add_JednaLiczba_ZwracaTaLiczbe()
    {
        var calculator = new StringCalculator();

        int result = calculator.Add("5");

        Assert.Equal(5, result);
    }

    [Fact]
    public void Add_WieleLiczb_ZwracaSume()
    {
        var calculator = new StringCalculator();

        int result = calculator.Add("1,2,3");

        Assert.Equal(6, result);
    }
}

// ==================== DEMONSTRACJA W MAIN ====================

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== Temat 2: TDD - Red-Green-Refactor ===\n");

        var calculator = new StringCalculator();

        Console.WriteLine($"Add(\"\") = {calculator.Add("")}");
        Console.WriteLine($"Add(\"5\") = {calculator.Add("5")}");
        Console.WriteLine($"Add(\"1,2,3\") = {calculator.Add("1,2,3")}");

        Console.WriteLine("\nTa implementacja powstała w 3 rundach Red-Green-Refactor.");
        Console.WriteLine("Uruchom 'dotnet test', aby zobaczyć wszystkie kroki jako testy.");
    }
}
