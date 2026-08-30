using System;
using System.Collections.Generic;

namespace OverloadableOperators;

// ==================== PRZYKŁAD 1: PRZEGLĄD KATEGORII ====================

public class Example1_Categories
{
    public class Money
    {
        public decimal Amount { get; }
        public Money(decimal amount) => Amount = amount;
        
        // Jednoargumentowe
        public static Money operator +(Money a) => a;
        public static Money operator -(Money a) => new(-a.Amount);
        
        // Binarne
        public static Money operator +(Money a, Money b) 
            => new Money(a.Amount + b.Amount);
        public static Money operator -(Money a, Money b) 
            => new Money(a.Amount - b.Amount);
        public static Money operator *(Money a, int factor) 
            => new Money(a.Amount * factor);
        
        // Relacyjne
        public static bool operator ==(Money a, Money b) 
            => a.Amount == b.Amount;
        public static bool operator !=(Money a, Money b) 
            => a.Amount != b.Amount;
        public static bool operator <(Money a, Money b) 
            => a.Amount < b.Amount;
        public static bool operator >(Money a, Money b) 
            => a.Amount > b.Amount;
        public static bool operator <=(Money a, Money b) 
            => a.Amount <= b.Amount;
        public static bool operator >=(Money a, Money b) 
            => a.Amount >= b.Amount;
        
        public override string ToString() => $"${Amount}";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Kategorie Operatorów ===");

        var m1 = new Money(100m);
        var m2 = new Money(50m);

        // Jednoargumentowe
        var positive = +m1;  // Stays same
        var negative = -m1;  // [-100]
        Console.WriteLine($"  Negacja: {m1} -> {negative}");

        // Binarne
        var sum = m1 + m2;  // [150]
        var diff = m1 - m2; // [50]
        var product = m1 * 2; // [200]
        Console.WriteLine($"  Suma: {m1} + {m2} = {sum}");
        Console.WriteLine($"  Różnica: {m1} - {m2} = {diff}");
        Console.WriteLine($"  Iloczyn: {m1} * 2 = {product}");

        // Relacyjne
        Console.WriteLine($"  Porównania:");
        Console.WriteLine($"    {m1} == {m2}: {m1 == m2}");
        Console.WriteLine($"    {m1} > {m2}: {m1 > m2}");
        Console.WriteLine($"    {m1} <= 200: {m1 <= new Money(200m)}");
    }
}

// ==================== PRZYKŁAD 2: OPERATORY BITOWE ====================

public class Example2_BitwiseOperators
{
    public class Flags
    {
        public int Value { get; private set; }

        public Flags(int value) => Value = value;

        // Bitowe AND
        public static Flags operator &(Flags a, Flags b)
        {
            Console.WriteLine($"    {a.Value} & {b.Value} = {a.Value & b.Value}");
            return new Flags(a.Value & b.Value);
        }

        // Bitowe OR
        public static Flags operator |(Flags a, Flags b)
        {
            Console.WriteLine($"    {a.Value} | {b.Value} = {a.Value | b.Value}");
            return new Flags(a.Value | b.Value);
        }

        // Bitowe NOT
        public static Flags operator ~(Flags a)
        {
            Console.WriteLine($"    ~{a.Value} = {~a.Value}");
            return new Flags(~a.Value);
        }

        public override string ToString() => $"[{Value:B8}]";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Operatory Bitowe ===");

        var flags1 = new Flags(12);  // 1100
        var flags2 = new Flags(10);  // 1010

        Console.WriteLine($"  Operand 1: {flags1}");
        Console.WriteLine($"  Operand 2: {flags2}");

        var and = flags1 & flags2;   // 1000 = 8
        var or = flags1 | flags2;    // 1110 = 14
        var not = ~flags1;           // Negacja bitowa
    }
}

// ==================== PRZYKŁAD 3: KONWERSJE ====================

public class Example3_Conversions
{
    public record Temperature(double Celsius)
    {
        // Niejawna konwersja z double
        public static implicit operator Temperature(double celsius)
        {
            Console.WriteLine($"  Niejawna konwersja: {celsius}°C");
            return new Temperature(celsius);
        }

        // Jawna konwersja do Fahrenheit
        public static explicit operator double(Temperature t)
        {
            double fahrenheit = (t.Celsius * 9 / 5) + 32;
            Console.WriteLine($"  Jawna konwersja: {t.Celsius}°C -> {fahrenheit}°F");
            return fahrenheit;
        }

        public override string ToString() => $"{Celsius}°C";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Operatory Konwersji ===");

        // Niejawna konwersja
        Temperature t1 = 25.0;  // Implicit
        Console.WriteLine($"  Temperatura: {t1}");

        // Jawna konwersja
        double fahrenheit = (double)t1;  // Explicit
        Console.WriteLine($"  W Fahrenheitach: {fahrenheit}°F");
    }
}

// ==================== PRZYKŁAD 4: PORÓWNANIA ====================

public class Example4_Comparisons
{
    public class Point
    {
        public double X { get; }
        public double Y { get; }
        public Point(double x, double y) { X = x; Y = y; }
        
        public static bool operator ==(Point a, Point b)
            => a.X == b.X && a.Y == b.Y;

        public static bool operator !=(Point a, Point b)
            => !(a == b);

        public static bool operator <(Point a, Point b)
            => Math.Sqrt(a.X*a.X + a.Y*a.Y) < Math.Sqrt(b.X*b.X + b.Y*b.Y);

        public static bool operator >(Point a, Point b)
            => b < a;

        public static bool operator <=(Point a, Point b)
            => a < b || a == b;

        public static bool operator >=(Point a, Point b)
            => a > b || a == b;
        
        public override string ToString() => $"({X}, {Y})";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Operatory Porównania ===");

        var p1 = new Point(3, 4);   // Odległość od (0,0) = 5
        var p2 = new Point(6, 8);   // Odległość od (0,0) = 10
        var p3 = new Point(3, 4);   // Równy p1

        Console.WriteLine($"  Punkt 1: ({p1.X}, {p1.Y})");
        Console.WriteLine($"  Punkt 2: ({p2.X}, {p2.Y})");
        Console.WriteLine($"  Punkt 3: ({p3.X}, {p3.Y})");

        Console.WriteLine($"  p1 == p3: {p1 == p3}");
        Console.WriteLine($"  p1 != p2: {p1 != p2}");
        Console.WriteLine($"  p1 < p2 (po odległości): {p1 < p2}");
        Console.WriteLine($"  p2 > p1: {p2 > p1}");
    }
}

// ==================== PRZYKŁAD 5: INKREMENTACJA ====================

public class Example5_IncrementDecrement
{
    public class Counter
    {
        public int Value { get; set; }

        // Pre-increment (++x)
        public static Counter operator ++(Counter c)
        {
            c.Value++;
            return c;
        }

        // Pre-decrement (--x)
        public static Counter operator --(Counter c)
        {
            c.Value--;
            return c;
        }

        public override string ToString() => $"Counter({Value})";
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Inkrementacja i Dekrementacja ===");

        var counter = new Counter { Value = 5 };
        Console.WriteLine($"  Początkowa wartość: {counter}");

        ++counter;
        Console.WriteLine($"  Po ++: {counter}");

        --counter;
        Console.WriteLine($"  Po --: {counter}");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   OVERLOADABLE OPERATORS IN C#                            ║");
        Console.WriteLine("║   Complete Survey of Operator Categories                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        Example1_Categories.Run();
        Example2_BitwiseOperators.Run();
        Example3_Conversions.Run();
        Example4_Comparisons.Run();
        Example5_IncrementDecrement.Run();

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
