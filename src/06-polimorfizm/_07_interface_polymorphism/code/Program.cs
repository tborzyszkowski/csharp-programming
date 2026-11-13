using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InterfacePolymorphism;

// ============================================================================
// INTERFEJS I IMPLEMENTACJE
// ============================================================================

public interface IShape
{
    double GetArea();
    double GetPerimeter();
}

public class Circle : IShape
{
    public double Radius { get; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double GetArea() => Math.PI * Radius * Radius;

    public double GetPerimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : IShape
{
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double GetArea() => Width * Height;

    public double GetPerimeter() => 2 * (Width + Height);
}

// ============================================================================
// PROGRAM GŁÓWNY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  POLIMORFIZM OPARTY NA INTERFEJSACH                    ║");
        Console.WriteLine("║  Interface-based Polymorphism                          ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        // Ziarno ustalone, aby wynik był powtarzalny między uruchomieniami
        var random = new Random(2024);
        List<IShape> shapes = CreateRandomShapes(random, 5);

        Console.WriteLine("Losowe kształty (każdy obiekt przez typ IShape):");
        foreach (IShape shape in shapes)
        {
            // Ten sam kod, różne zachowanie: GetArea() wybiera implementację
            // w zależności od rzeczywistego typu obiektu (Circle lub Rectangle)
            Console.WriteLine($"  {shape.GetType().Name,-10} pole = {shape.GetArea(),8:F2}, obwód = {shape.GetPerimeter(),8:F2}");
        }

        double totalArea = shapes.Sum(s => s.GetArea());
        Console.WriteLine($"\nSuma pól wszystkich kształtów: {totalArea:F2}");
    }

    // Losowo wybiera Circle lub Rectangle i losuje ich wymiary
    public static List<IShape> CreateRandomShapes(Random random, int count)
    {
        var shapes = new List<IShape>();
        for (int i = 0; i < count; i++)
        {
            if (random.Next(2) == 0)
            {
                shapes.Add(new Circle(random.NextDouble() * 10));
            }
            else
            {
                shapes.Add(new Rectangle(random.NextDouble() * 10, random.NextDouble() * 10));
            }
        }
        return shapes;
    }
}

// ============================================================================
// TESTY
// ============================================================================

public class InterfacePolymorphismTests
{
    private const double Delta = 0.0001;

    [Fact]
    public void Circle_ComputesAreaAndPerimeter()
    {
        IShape circle = new Circle(2);

        Assert.Equal(Math.PI * 4, circle.GetArea(), Delta);
        Assert.Equal(4 * Math.PI, circle.GetPerimeter(), Delta);
    }

    [Fact]
    public void Rectangle_ComputesAreaAndPerimeter()
    {
        IShape rectangle = new Rectangle(4, 6);

        Assert.Equal(24, rectangle.GetArea(), Delta);
        Assert.Equal(20, rectangle.GetPerimeter(), Delta);
    }

    [Fact]
    public void Interface_DispatchesToRuntimeType()
    {
        var shapes = new List<IShape> { new Circle(1), new Rectangle(2, 3) };

        Assert.Equal(Math.PI, shapes[0].GetArea(), Delta);
        Assert.Equal(6, shapes[1].GetArea(), Delta);
    }

    [Fact]
    public void CreateRandomShapes_ReturnsRequestedCountOfShapes()
    {
        List<IShape> shapes = Program.CreateRandomShapes(new Random(1), 10);

        Assert.Equal(10, shapes.Count);
        Assert.All(shapes, s => Assert.True(s is Circle || s is Rectangle));
    }

    [Fact]
    public void CreateRandomShapes_IsRepeatableForSameSeed()
    {
        List<IShape> first = Program.CreateRandomShapes(new Random(7), 5);
        List<IShape> second = Program.CreateRandomShapes(new Random(7), 5);

        Assert.Equal(
            first.Select(s => s.GetArea()),
            second.Select(s => s.GetArea()));
    }
}
