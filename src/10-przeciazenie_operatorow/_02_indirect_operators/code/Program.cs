using System;

namespace IndirectOperators;

public class Example1_EqualityPairs
{
    public class Complex
    {
        public double Real { get; }
        public double Imaginary { get; }
        public Complex(double real, double imaginary) { Real = real; Imaginary = imaginary; }
        
        public static bool operator ==(Complex a, Complex b)
            => a.Real == b.Real && a.Imaginary == b.Imaginary;
        
        public static bool operator !=(Complex a, Complex b)
            => !(a == b);  // Pair with ==
        
        public override string ToString() => $"{Real}+{Imaginary}i";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Równość - Pary Operatorów ===");
        var z1 = new Complex(1, 2);
        var z2 = new Complex(1, 2);
        var z3 = new Complex(3, 4);
        
        Console.WriteLine($"  z1 == z2: {z1 == z2}");
        Console.WriteLine($"  z1 != z2: {z1 != z2}");
        Console.WriteLine($"  z1 == z3: {z1 == z3}");
        Console.WriteLine($"  z1 != z3: {z1 != z3}");
    }
}

public class Example2_ComparisonSets
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
        Console.WriteLine("\n=== EXAMPLE 2: Porównania Porządkowe - Kompletne Zestawy ===");
        var t1 = new Temperature(20);
        var t2 = new Temperature(25);
        
        Console.WriteLine($"  t1 < t2: {t1 < t2}");
        Console.WriteLine($"  t1 > t2: {t1 > t2}");
        Console.WriteLine($"  t1 <= t2: {t1 <= t2}");
        Console.WriteLine($"  t1 >= t2: {t1 >= t2}");
    }
}

public class Example3_TrueFalsePair
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = "";

        public static bool operator true(Result r)
        {
            Console.WriteLine($"    operator true: {r.IsSuccess}");
            return r.IsSuccess;
        }

        public static bool operator false(Result r)
        {
            Console.WriteLine($"    operator false: {!r.IsSuccess}");
            return !r.IsSuccess;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Operatory true/false - Pary ===");
        
        var success = new Result { IsSuccess = true, Message = "OK" };
        var failure = new Result { IsSuccess = false, Message = "Error" };

        Console.WriteLine("  if (success)");
        if (success) Console.WriteLine("    Success!");
        
        Console.WriteLine("  if (!failure)");
        if (!failure) Console.WriteLine("    Is failure!");
    }
}

public class Example4_ArithmeticRelated
{
    public record Vector(double X, double Y)
    {
        // Semantycznie powiązane
        public static Vector operator +(Vector a, Vector b)
            => new(a.X + b.X, a.Y + b.Y);
        
        public static Vector operator -(Vector a, Vector b)
            => new(a.X - b.X, a.Y - b.Y);
        
        public override string ToString() => $"[{X:F1}, {Y:F1}]";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Operatory Arytmetyczne - Semantycznie Powiązane ===");
        var v1 = new Vector(1, 2);
        var v2 = new Vector(3, 4);
        
        var sum = v1 + v2;
        var diff = v1 - v2;
        
        Console.WriteLine($"  v1: {v1}");
        Console.WriteLine($"  v2: {v2}");
        Console.WriteLine($"  v1 + v2: {sum}");
        Console.WriteLine($"  v1 - v2: {diff}");
    }
}

public class Example5_BitwiseWithLogical
{
    public class Permissions
    {
        public int Flags { get; set; }

        public static Permissions operator &(Permissions a, Permissions b)
            => new() { Flags = a.Flags & b.Flags };

        public static Permissions operator |(Permissions a, Permissions b)
            => new() { Flags = a.Flags | b.Flags };

        public static bool operator true(Permissions p)
            => p.Flags != 0;

        public static bool operator false(Permissions p)
            => p.Flags == 0;

        public override string ToString() => $"[{Flags:B8}]";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Operatory Bitowe + Logiczne ===");
        var read = new Permissions { Flags = 4 };    // 0100
        var write = new Permissions { Flags = 2 };   // 0010
        var admin = new Permissions { Flags = 1 };   // 0001
        
        var readWrite = read | write;  // 0110
        
        Console.WriteLine($"  Read: {read}");
        Console.WriteLine($"  Write: {write}");
        Console.WriteLine($"  Read | Write: {readWrite}");
        
        if (readWrite)
            Console.WriteLine("  Has permissions!");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   INDIRECT OPERATORS (IMPLIED OPERATORS)                  ║");
        Console.WriteLine("║   Relationships Between Overloaded Operators              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_EqualityPairs.Run();
        Example2_ComparisonSets.Run();
        Example3_TrueFalsePair.Run();
        Example4_ArithmeticRelated.Run();
        Example5_BitwiseWithLogical.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
