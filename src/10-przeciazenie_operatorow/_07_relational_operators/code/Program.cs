using System;

namespace RelationalOperators;

public class Example1_EqualityComparison
{
    public record Point(int X, int Y)
    {
        public static bool operator ==(Point a, Point b)
            => a.X == b.X && a.Y == b.Y;
        
        public static bool operator !=(Point a, Point b)
            => !(a == b);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Equality Comparison ===");
        var p1 = new Point(1, 2);
        var p2 = new Point(1, 2);
        var p3 = new Point(3, 4);

        Console.WriteLine($"  p1 == p2: {p1 == p2}");
        Console.WriteLine($"  p1 != p2: {p1 != p2}");
        Console.WriteLine($"  p1 == p3: {p1 == p3}");
    }
}

public class Example2_OrderComparison
{
    public record Temperature(double Celsius)
    {
        public static bool operator <(Temperature a, Temperature b)
            => a.Celsius < b.Celsius;
        
        public static bool operator >(Temperature a, Temperature b)
            => a.Celsius > b.Celsius;
        
        public static bool operator <=(Temperature a, Temperature b)
            => a.Celsius <= b.Celsius;
        
        public static bool operator >=(Temperature a, Temperature b)
            => a.Celsius >= b.Celsius;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Order Comparison ===");
        var t1 = new Temperature(20);
        var t2 = new Temperature(25);

        Console.WriteLine($"  20°C < 25°C: {t1 < t2}");
        Console.WriteLine($"  20°C > 25°C: {t1 > t2}");
        Console.WriteLine($"  20°C <= 20°C: {t1 <= new Temperature(20)}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   RELATIONAL OPERATORS                                    ║");
        Console.WriteLine("║   ==, !=, <, >, <=, >=                                    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_EqualityComparison.Run();
        Example2_OrderComparison.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
