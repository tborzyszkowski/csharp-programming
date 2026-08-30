using System;

namespace ConversionOperators;

public class Example1_ExplicitConversion
{
    public record Percent(double Value)
    {
        public static explicit operator Percent(double value)
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Percent must be 0-100");
            return new Percent(value);
        }

        public override string ToString() => $"{Value:F1}%";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Explicit Conversion ===");
        
        double d = 75.5;
        Percent p = (Percent)d;  // Explicit
        Console.WriteLine($"  {d} -> {p}");

        try
        {
            Percent invalid = (Percent)150.0;  // Throws
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"  Error: {ex.Message}");
        }
    }
}

public class Example2_ImplicitConversion
{
    public record Money(decimal Amount)
    {
        public static implicit operator Money(int amount)
            => new Money((decimal)amount);

        public static explicit operator Money(double amount)
            => new Money((decimal)amount);

        public override string ToString() => $"${Amount}";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Implicit Conversion ===");
        
        Money m1 = 100;              // Implicit from int
        Money m2 = (Money)99.99;     // Explicit from double (may lose precision)
        
        Console.WriteLine($"  100 (int) -> {m1}");
        Console.WriteLine($"  99.99 (double) -> {m2}");
    }
}

public class Example3_TemperatureConversion
{
    public record Temperature(double Celsius)
    {
        // Niejawna z double
        public static implicit operator Temperature(double celsius)
            => new(celsius);

        // Jawna na int (zaokrąglenie)
        public static explicit operator int(Temperature t)
            => (int)Math.Round(t.Celsius);

        // Jawna na double (Fahrenheit)
        public static explicit operator double(Temperature t)
            => (t.Celsius * 9 / 5) + 32;

        public override string ToString() => $"{Celsius}°C";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Temperature Conversion ===");
        
        Temperature t1 = 25.0;  // Implicit
        Console.WriteLine($"  25.0 -> {t1}");

        int celsius = (int)t1;  // Explicit to int
        double fahrenheit = (double)t1;  // Explicit to Fahrenheit
        
        Console.WriteLine($"  {t1} -> {celsius}°C (int)");
        Console.WriteLine($"  {t1} -> {fahrenheit}°F");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   CONVERSION OPERATORS                                    ║");
        Console.WriteLine("║   Explicit and Implicit Type Conversions                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_ExplicitConversion.Run();
        Example2_ImplicitConversion.Run();
        Example3_TemperatureConversion.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
