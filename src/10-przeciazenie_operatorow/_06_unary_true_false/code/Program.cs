using System;

namespace UnaryTrueFalse;

public class Example1_BasicTrueFalse
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public static bool operator true(Result r) => r.IsSuccess;
        public static bool operator false(Result r) => !r.IsSuccess;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Basic true/false ===");
        
        var success = new Result { IsSuccess = true };
        var failure = new Result { IsSuccess = false };

        if (success)
            Console.WriteLine("  Success!");
        
        if (!failure)
            Console.WriteLine("  Not a failure!");
    }
}

public class Example2_OptionalPattern
{
    public class OptionalInt
    {
        public int? Value { get; set; }

        public static bool operator true(OptionalInt oi)
            => oi.Value.HasValue;

        public static bool operator false(OptionalInt oi)
            => !oi.Value.HasValue;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Optional Pattern ===");
        
        var has = new OptionalInt { Value = 42 };
        var empty = new OptionalInt { Value = null };

        if (has)
            Console.WriteLine("  Has value!");

        if (!empty)
            Console.WriteLine("  Is empty!");
    }
}

public class Example3_TernaryOperator
{
    public class Permission
    {
        public bool CanRead { get; set; }

        public static bool operator true(Permission p) => p.CanRead;
        public static bool operator false(Permission p) => !p.CanRead;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Ternary with true/false ===");
        
        var perm = new Permission { CanRead = true };
        string message = perm ? "Can read" : "Cannot read";
        Console.WriteLine($"  {message}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   UNARY OPERATORS: TRUE AND FALSE                         ║");
        Console.WriteLine("║   Boolean Conversion Operators                            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_BasicTrueFalse.Run();
        Example2_OptionalPattern.Run();
        Example3_TernaryOperator.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
