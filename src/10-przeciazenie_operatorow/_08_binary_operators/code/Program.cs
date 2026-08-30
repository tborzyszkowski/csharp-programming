using System;

namespace BinaryOperators;

public class Example1_Arithmetic
{
    public record Complex(double Real, double Imaginary)
    {
        public static Complex operator +(Complex a, Complex b)
            => new(a.Real + b.Real, a.Imaginary + b.Imaginary);
        
        public static Complex operator -(Complex a, Complex b)
            => new(a.Real - b.Real, a.Imaginary - b.Imaginary);
        
        public static Complex operator *(Complex a, Complex b)
        {
            double r = a.Real * b.Real - a.Imaginary * b.Imaginary;
            double i = a.Real * b.Imaginary + a.Imaginary * b.Real;
            return new(r, i);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Arithmetic Binary Operators ===");
        var z1 = new Complex(1, 2);
        var z2 = new Complex(3, 4);
        
        var sum = z1 + z2;
        var diff = z1 - z2;
        var prod = z1 * z2;
        
        Console.WriteLine($"  z1 + z2 = {sum}");
        Console.WriteLine($"  z1 - z2 = {diff}");
        Console.WriteLine($"  z1 * z2 = {prod}");
    }
}

public class Example2_Bitwise
{
    public class Flags
    {
        public int Value { get; set; }

        public static Flags operator &(Flags a, Flags b)
            => new() { Value = a.Value & b.Value };
        
        public static Flags operator |(Flags a, Flags b)
            => new() { Value = a.Value | b.Value };
        
        public static Flags operator ^(Flags a, Flags b)
            => new() { Value = a.Value ^ b.Value };
        
        public override string ToString() => $"[{Value:B8}]";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Bitwise Operators ===");
        var f1 = new Flags { Value = 0b1100 };
        var f2 = new Flags { Value = 0b1010 };
        
        var and = f1 & f2;   // 1000
        var or = f1 | f2;    // 1110
        var xor = f1 ^ f2;   // 0110
        
        Console.WriteLine($"  {f1} & {f2} = {and}");
        Console.WriteLine($"  {f1} | {f2} = {or}");
        Console.WriteLine($"  {f1} ^ {f2} = {xor}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   BINARY OPERATORS                                        ║");
        Console.WriteLine("║   Arithmetic and Bitwise                                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_Arithmetic.Run();
        Example2_Bitwise.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
