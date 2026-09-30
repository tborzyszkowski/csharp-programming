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
    
    public override string ToString() => $"Koło: środek {Center}, promień {Radius}";
}

// STRUKTURA NIEZMIENNA (zalecany styl): readonly gwarantuje brak mutacji, więc kopie są bezpieczne
public readonly struct ImmutablePoint
{
    public int X { get; }
    public int Y { get; }
    
    public ImmutablePoint(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    // Zamiast zmieniać obiekt, zwracamy nową wartość
    public ImmutablePoint WithX(int x) => new(x, Y);
    
    public override string ToString() => $"({X}, {Y})";
}

// record struct: kompilator generuje równość wartościową, GetHashCode, ToString i wyrażenie with
public readonly record struct Vector2D(double X, double Y);

public static class StructDemo
{
    // Do metody trafia KOPIA struktury - zmiana nie jest widoczna u wołującego
    public static void MoveByValue(Point p) => p.X += 100;
    
    // Z ref metoda pracuje na oryginale
    public static void MoveByRef(ref Point p) => p.X += 100;
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  STRUKTURY (VALUE TYPE)                               ║");
        Console.WriteLine("║  Klasy (REFERENCE TYPE)                               ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        DemonstrateValueType();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateReferenceType();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateCopySemanticsAndImmutability();
    }
    
    private static void DemonstrateCopySemanticsAndImmutability()
    {
        Console.WriteLine("📋 Kopiowanie struktur i niezmienność");
        Console.WriteLine("──────────────────────────────────────\n");
        
        var p = new Point(1, 1);
        StructDemo.MoveByValue(p);
        Console.WriteLine($"Po MoveByValue(p):     {p}  (bez zmian - metoda dostała kopię)");
        StructDemo.MoveByRef(ref p);
        Console.WriteLine($"Po MoveByRef(ref p):   {p}  (zmienione - ref to oryginalna zmienna)");
        
        var circle = new Circle(new Point(0, 0), 5);
        // circle.Center.X = 10;  // BŁĄD KOMPILACJI CS1612: Center zwraca KOPIĘ, zmiana byłaby zgubiona
        Console.WriteLine($"\nPunkt Center w klasie Circle jest właściwością - zwraca kopię: {circle.Center}");
        
        var a = new ImmutablePoint(1, 2);
        var b = a.WithX(10);
        Console.WriteLine($"\nImmutablePoint: a = {a}, b = a.WithX(10) = {b}");
        
        var v1 = new Vector2D(1, 2);
        var v2 = v1 with { X = 5 };
        Console.WriteLine($"record struct: v1 = {v1}, v2 = {v2}, v1 == new Vector2D(1, 2)? {v1 == new Vector2D(1, 2)}");
        
        Console.WriteLine($"\ndefault(Point) = {default(Point)}  (struktura zawsze ma wartość - nie może być null)");
        int? maybe = null;   // Nullable<int> - struktura też może "nie mieć wartości"
        Console.WriteLine($"int? maybe = null -> HasValue: {maybe.HasValue}");
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
        Assert.Equal(5.0, point.Distance(), precision: 10);
    }

    [Fact]
    public void Struct_PassedToMethod_IsCopied()
    {
        var p = new Point(1, 1);

        StructDemo.MoveByValue(p);

        Assert.Equal(1, p.X);
    }

    [Fact]
    public void Struct_PassedByRef_IsModifiedInPlace()
    {
        var p = new Point(1, 1);

        StructDemo.MoveByRef(ref p);

        Assert.Equal(101, p.X);
    }

    [Fact]
    public void Struct_DefaultEquals_ComparesFieldValues()
    {
        // Domyślne ValueType.Equals porównuje pola - w przeciwieństwie do klas (referencje)
        Assert.Equal(new Point(1, 2), new Point(1, 2));
    }

    [Fact]
    public void Struct_Default_HasZeroedFields()
    {
        var p = default(Point);

        Assert.Equal(0, p.X);
        Assert.Equal(0, p.Y);
    }

    [Fact]
    public void ImmutableStruct_WithX_ReturnsNewValueAndKeepsOriginal()
    {
        var a = new ImmutablePoint(1, 2);

        var b = a.WithX(10);

        Assert.Equal(1, a.X);
        Assert.Equal(10, b.X);
        Assert.Equal(2, b.Y);
    }

    [Fact]
    public void RecordStruct_HasValueEquality()
    {
        var v1 = new Vector2D(1, 2);
        var v2 = v1 with { X = 5 };

        Assert.Equal(new Vector2D(1, 2), v1);
        Assert.NotEqual(v1, v2);
        Assert.Equal(1, v1.X);
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
