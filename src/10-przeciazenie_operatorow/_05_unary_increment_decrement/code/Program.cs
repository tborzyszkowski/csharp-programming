using System;

namespace UnaryIncrementDecrement;

public class Example1_Increment
{
    public class Counter
    {
        public int Value { get; set; }

        public static Counter operator ++(Counter c)
        {
            c.Value++;
            return c;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Increment ===");
        var c = new Counter { Value = 5 };
        Console.WriteLine($"  Before: {c.Value}");
        ++c;
        Console.WriteLine($"  After ++c: {c.Value}");
    }
}

public class Example2_Decrement
{
    public class Counter
    {
        public int Value { get; set; }

        public static Counter operator --(Counter c)
        {
            c.Value--;
            return c;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Decrement ===");
        var c = new Counter { Value = 5 };
        Console.WriteLine($"  Before: {c.Value}");
        --c;
        Console.WriteLine($"  After --c: {c.Value}");
    }
}

public class Example3_BothOperators
{
    public class Counter
    {
        public int Value { get; set; }

        public static Counter operator ++(Counter c)
        {
            c.Value++;
            return c;
        }

        public static Counter operator --(Counter c)
        {
            c.Value--;
            return c;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Increment and Decrement Together ===");
        var c = new Counter { Value = 0 };
        
        ++c; ++c; ++c;
        Console.WriteLine($"  After +++: {c.Value}");
        
        --c; --c;
        Console.WriteLine($"  After --: {c.Value}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   UNARY OPERATORS: INCREMENT/DECREMENT                    ║");
        Console.WriteLine("║   ++, --                                                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_Increment.Run();
        Example2_Decrement.Run();
        Example3_BothOperators.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
