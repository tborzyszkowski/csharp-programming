using System;

namespace OperatorDefinitionRules;

public class Example1_BasicStructure
{
    public record Money(decimal Amount)
    {
        // Poprawna struktura operatora
        public static Money operator +(Money a, Money b)
        {
            if (a == null || b == null)
                throw new ArgumentNullException("Parametr nie może być null");
            
            return new Money(a.Amount + b.Amount);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Struktura Operatora ===");
        var m1 = new Money(100m);
        var m2 = new Money(50m);
        var sum = m1 + m2;
        Console.WriteLine($"  {m1} + {m2} = {sum}");
    }
}

public class Example2_ImmutabilityPattern
{
    public record Point(double X, double Y)
    {
        // ✅ Immutable - zwraca nowy obiekt
        public static Point operator +(Point a, Point b)
            => new(a.X + b.X, a.Y + b.Y);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Wzorzec Immutability ===");
        var p1 = new Point(1, 2);
        var p2 = new Point(3, 4);
        var p3 = p1 + p2;
        
        Console.WriteLine($"  p1: ({p1.X}, {p1.Y}) - nie zmieniony");
        Console.WriteLine($"  p2: ({p2.X}, {p2.Y}) - nie zmieniony");
        Console.WriteLine($"  p3 = p1 + p2: ({p3.X}, {p3.Y})");
    }
}

public class Example3_ParameterValidation
{
    public record Division
    {
        public static decimal operator /(decimal a, decimal b)
        {
            if (b == 0)
                throw new ArgumentException("Dzielenie przez zero!");
            return a / b;
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Walidacja Parametrów ===");
        try
        {
            decimal result = 10m / 2m;
            Console.WriteLine($"  10 / 2 = {result}");

            result = 10m / 0m;  // Wysypie się
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  Error: {ex.Message}");
        }
    }
}

public class Example4_UnaryOperators
{
    public record Complex(double Real, double Imaginary)
    {
        // Unary +
        public static Complex operator +(Complex c) => c;
        
        // Unary -
        public static Complex operator -(Complex c)
            => new(-c.Real, -c.Imaginary);
        
        // Logical NOT
        public static bool operator !(Complex c)
            => c.Real == 0 && c.Imaginary == 0;
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Operatory Jednoargumentowe ===");
        var z = new Complex(3, 4);
        
        Console.WriteLine($"  z = {z.Real} + {z.Imaginary}i");
        Console.WriteLine($"  +z = {(+z).Real} + {(+z).Imaginary}i");
        Console.WriteLine($"  -z = {(-z).Real} + {(-z).Imaginary}i");
        Console.WriteLine($"  !z (czy zero?): {!z}");
    }
}

public class Example5_ReturnType
{
    public record Ratio(int Numerator, int Denominator)
    {
        // Zwraca inny typ niż parametry
        public static bool operator ==(Ratio a, Ratio b)
            => a.Numerator * b.Denominator == a.Denominator * b.Numerator;
        
        public static bool operator !=(Ratio a, Ratio b)
            => !(a == b);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Zwracany Typ Może Być Inny ===");
        var r1 = new Ratio(1, 2);
        var r2 = new Ratio(2, 4);
        
        Console.WriteLine($"  1/2 == 2/4: {r1 == r2}");  // bool, nie Ratio!
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   OPERATOR DEFINITION RULES                               ║");
        Console.WriteLine("║   General Principles and Best Practices                   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_BasicStructure.Run();
        Example2_ImmutabilityPattern.Run();
        Example3_ParameterValidation.Run();
        Example4_UnaryOperators.Run();
        Example5_ReturnType.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
