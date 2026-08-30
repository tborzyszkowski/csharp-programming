using System;
using Xunit;

namespace Structs;

// STRUKTURA - Value type
public struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public double Distance() => Math.Sqrt(X * X + Y * Y);
    
    public override string ToString() => $"({X}, {Y})";
}

// KLASA - Reference type
public class Circle
{
    public Point Center { get; set; }
    public int Radius { get; set; }
    
    public Circle(Point center, int radius)
    {
        Center = center;
        Radius = radius;
    }
    
    public override string ToString() => $"Kołocentrum: {Center}, Promień: {Radius}";
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  STRUKTURY (VALUE TYPE)                               ║");
        Console.WriteLine("║  Klasy (REFERENCE TYPE)                               ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        DemonstrateValueType();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateReferenceType();
    }
    
    private static void DemonstrateValueType()
    {
        Console.WriteLine("📦 Struktura (Value Type)");
        Console.WriteLine("───────────────────────────\n");
        
        var point1 = new Point(10, 20);
        var point2 = point1;  // KOPIA wartości
        
        Console.WriteLine($"point1: {point1}");
        Console.WriteLine($"point2: {point2}");
        
        point2.X = 100;
        Console.WriteLine($"\nPo zmianie point2.X:");
        Console.WriteLine($"point1.X: {point1.X} (bez zmian!)");
        Console.WriteLine($"point2.X: {point2.X}");
    }
    
    private static void DemonstrateReferenceType()
    {
        Console.WriteLine("🔵 Klasa (Reference Type)");
        Console.WriteLine("──────────────────────────\n");
        
        var circle1 = new Circle(new Point(0, 0), 5);
        var circle2 = circle1;  // Referencja do tego samego obiektu
        
        Console.WriteLine($"circle1: {circle1}");
        Console.WriteLine($"circle2: {circle2}");
        
        circle2.Radius = 10;
        Console.WriteLine($"\nPo zmianie circle2.Radius:");
        Console.WriteLine($"circle1.Radius: {circle1.Radius} (zmienił się!)");
        Console.WriteLine($"circle2.Radius: {circle2.Radius}");
    }
}

/// ============================================
/// TESTY
/// ============================================

public class StructTests
{
    [Fact]
    public void Struct_IsValueType()
    {
        var point1 = new Point(10, 20);
        var point2 = point1;
        
        point2.X = 100;
        
        Assert.Equal(10, point1.X);
        Assert.Equal(100, point2.X);
    }
    
    [Fact]
    public void Struct_Distance_CalculatesCorrectly()
    {
        var point = new Point(3, 4);
        Assert.Equal(5, point.Distance());
    }
}

public class ClassTests
{
    [Fact]
    public void Class_IsReferenceType()
    {
        var circle1 = new Circle(new Point(0, 0), 5);
        var circle2 = circle1;
        
        circle2.Radius = 10;
        
        Assert.Equal(10, circle1.Radius);
        Assert.Equal(10, circle2.Radius);
    }
}
