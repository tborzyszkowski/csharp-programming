using System;

namespace UnaryArithmetic;

public class Example1_UnaryPlus
{
    public record Vector(double X, double Y)
    {
        public static Vector operator +(Vector v) => v;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Unary Plus ===");
        var v = new Vector(1, 2);
        var result = +v;
        Console.WriteLine($"  +{v} = {result}");
    }
}

public class Example2_UnaryMinus
{
    public record Complex(double Real, double Imaginary)
    {
        public static Complex operator -(Complex c)
            => new(-c.Real, -c.Imaginary);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Unary Minus ===");
        var z = new Complex(3, 4);
        var neg = -z;
        Console.WriteLine($"  -{z} = {neg}");
    }
}

public class Example3_LogicalNot
{
    public record Complex(double Real, double Imaginary)
    {
        public static bool operator !(Complex c)
            => c.Real == 0 && c.Imaginary == 0;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Logical NOT ===");
        var zero = new Complex(0, 0);
        var nonZero = new Complex(1, 1);
        
        Console.WriteLine($"  !{zero} = {!zero}");
        Console.WriteLine($"  !{nonZero} = {!nonZero}");
    }
}

public class Example4_BitwiseNot
{
    public class Flags
    {
        public int Value { get; set; }
        public static Flags operator ~(Flags f)
            => new() { Value = ~f.Value };
        
        public override string ToString() => $"[{Value:B8}]";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Bitwise NOT ===");
        var flags = new Flags { Value = 0b1100 };
        var inverted = ~flags;
        Console.WriteLine($"  ~{flags} = {inverted}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   UNARY OPERATORS: ARITHMETIC AND LOGIC                   ║");
        Console.WriteLine("║   +, -, !, ~                                              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_UnaryPlus.Run();
        Example2_UnaryMinus.Run();
        Example3_LogicalNot.Run();
        Example4_BitwiseNot.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
