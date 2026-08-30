using System;
using Xunit;

namespace StructInitialization;

public struct Point
{
    public double X { get; set; }
    public double Y { get; set; }
    
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
    
    public double Distance() => Math.Sqrt(X * X + Y * Y);
    
    public override string ToString() => $"({X}, {Y})";
}

public struct Color
{
    public byte Red { get; init; }
    public byte Green { get; init; }
    public byte Blue { get; init; }
    
    public override string ToString() => $"RGB({Red}, {Green}, {Blue})";
}

public struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    
    public override string ToString() => $"{Amount} {Currency}";
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 5: STRUKTURY - INICJALIZACJA ===\n");
        
        Console.WriteLine("1. DOMYŚLNY KONSTRUKTOR:");
        var p1 = new Point();
        Console.WriteLine($"Default Point: {p1}");
        Console.WriteLine();
        
        Console.WriteLine("2. PARAMETROWY KONSTRUKTOR:");
        var p2 = new Point(3, 4);
        Console.WriteLine($"Point(3,4): {p2}, Distance: {p2.Distance()}");
        Console.WriteLine();
        
        Console.WriteLine("3. INIT-ONLY PROPERTIES:");
        var red = new Color { Red = 255, Green = 0, Blue = 0 };
        var green = new Color { Red = 0, Green = 255, Blue = 0 };
        Console.WriteLine($"Red: {red}");
        Console.WriteLine($"Green: {green}");
        Console.WriteLine();
        
        Console.WriteLine("4. MONEY STRUCT:");
        var price = new Money(99.99m, "PLN");
        Console.WriteLine(price);
    }
}

public class StructTests
{
    [Fact]
    public void Struct_DefaultConstructor_ZeroInitializesFields()
    {
        var point = new Point();
        
        Assert.Equal(0, point.X);
        Assert.Equal(0, point.Y);
    }
    
    [Fact]
    public void Struct_WithConstructor_InitializesFields()
    {
        var point = new Point(5, 12);
        
        Assert.Equal(5, point.X);
        Assert.Equal(12, point.Y);
        Assert.Equal(13, point.Distance());
    }
    
    [Fact]
    public void Struct_InitProperties_CanInitialize()
    {
        var color = new Color { Red = 255, Green = 128, Blue = 0 };
        
        Assert.Equal(255, color.Red);
        Assert.Equal(128, color.Green);
    }
}
