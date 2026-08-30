using System;
using System.Collections.Generic;
using System.Linq;

namespace LambdaAndAnonymous;

public class Example1_AnonymousMethods
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Anonymous Methods ===");

        delegate int Calc(int a, int b);

        // Metoda anonimowa
        Calc add = delegate(int x, int y)
        {
            Console.WriteLine($"  Computing {x} + {y}");
            return x + y;
        };

        Console.WriteLine($"Result: {add(5, 3)}");
    }
}

public class Example2_LambdaBasics
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Lambda Basics ===");

        // Jedno-linijkowa
        Func<int, int> square = x => x * x;
        Console.WriteLine($"Square(5) = {square(5)}");

        // Wielolinijkowa
        Func<int, int> complex = x => {
            int temp = x * 2;
            return temp + 10;
        };
        Console.WriteLine($"Complex(5) = {complex(5)}");

        // Bez parametrów
        Action getCurrentTime = () => Console.WriteLine($"  {DateTime.Now:HH:mm:ss}");
        getCurrentTime();
    }
}

public class Example3_Closure
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Closure ===");

        int multiplier = 10;
        Func<int, int> multiply = x => x * multiplier;

        Console.WriteLine($"Multiply 5 by {multiplier}: {multiply(5)}");

        multiplier = 20;
        Console.WriteLine($"After changing multiplier to {multiplier}: {multiply(5)}");

        // Multiple closures
        var funcs = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
        {
            int captured = i;
            funcs.Add(() => captured);
        }

        Console.WriteLine("\nCaptured values:");
        for (int j = 0; j < funcs.Count; j++)
        {
            Console.WriteLine($"  Func[{j}] returns: {funcs[j]()}");
        }
    }
}

public class Example4_LINQWithLambda
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: LINQ with Lambda ===");

        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Console.WriteLine("Original: " + string.Join(", ", numbers));

        // Where (filter)
        var evens = numbers.Where(n => n % 2 == 0);
        Console.WriteLine("Evens: " + string.Join(", ", evens));

        // Select (transform)
        var squared = numbers.Select(n => n * n);
        Console.WriteLine("Squared: " + string.Join(", ", squared));

        // OrderBy
        var reversed = numbers.OrderByDescending(n => n);
        Console.WriteLine("Reversed: " + string.Join(", ", reversed));

        // Complex chain
        Console.WriteLine("\nComplex query:");
        var result = numbers
            .Where(n => n > 3)
            .Select(n => n * 2)
            .OrderByDescending(n => n)
            .Take(3);
        
        Console.WriteLine("(n > 3) -> (n*2) -> OrderDesc -> Take 3:");
        Console.WriteLine(string.Join(", ", result));
    }
}

public record Student(string Name, int Grade);

public class Example5_GroupBy
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: GroupBy with Lambda ===");

        var students = new[]
        {
            new Student("Alice", 90),
            new Student("Bob", 85),
            new Student("Charlie", 90),
            new Student("Diana", 95),
            new Student("Eve", 85)
        };

        var grouped = students.GroupBy(s => s.Grade);

        foreach (var group in grouped)
        {
            Console.WriteLine($"\nGrade {group.Key}:");
            foreach (var student in group)
            {
                Console.WriteLine($"  - {student.Name}");
            }
        }
    }
}

public class Example6_FilterMapReduce
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 6: Filter-Map-Reduce Pattern ===");

        var numbers = Enumerable.Range(1, 20);

        // Filter: > 5
        // Map: * 2
        // Reduce: sum
        int result = numbers
            .Where(n => n > 5)                          // Filter
            .Select(n => n * 2)                         // Map
            .Aggregate(0, (sum, n) => sum + n);         // Reduce

        Console.WriteLine($"Numbers > 5, doubled, then summed: {result}");

        // Step by step
        Console.WriteLine("\nStep by step:");
        Console.WriteLine("Original: 1-20");
        
        var step1 = numbers.Where(n => n > 5);
        Console.WriteLine($"After filter (> 5): {string.Join(",", step1)}");
        
        var step2 = step1.Select(n => n * 2);
        Console.WriteLine($"After map (*2): {string.Join(",", step2)}");
        
        var step3 = step2.Aggregate(0, (sum, n) => sum + n);
        Console.WriteLine($"After reduce (sum): {step3}");
    }
}

public class Example7_EventHandlers
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 7: Event Handlers with Lambda ===");

        var button = new SimpleButton();

        // Dodaj handler z lambdą
        button.OnClick += () => Console.WriteLine("  Handler 1 called!");
        button.OnClick += () => Console.WriteLine("  Handler 2 called!");
        button.OnClick += () => Console.WriteLine("  Handler 3 called!");

        button.Click();
    }

    public class SimpleButton
    {
        public event Action? OnClick;

        public void Click()
        {
            Console.WriteLine("Button clicked!");
            OnClick?.Invoke();
        }
    }
}

public class Example8_PredicateChain
{
    public record User(string Name, int Age, bool IsBlocked);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 8: Predicate Chain ===");

        var users = new[]
        {
            new User("Alice", 25, false),
            new User("Bob", 17, false),
            new User("Charlie", 30, true),
            new User("Diana", 22, false)
        };

        // Complex predicate
        Func<User, bool> isValidUser = u =>
            !string.IsNullOrEmpty(u.Name) &&
            u.Age >= 18 &&
            !u.IsBlocked;

        Console.WriteLine("Valid users:");
        foreach (var user in users.Where(isValidUser))
        {
            Console.WriteLine($"  - {user.Name} ({user.Age})");
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   LAMBDA EXPRESSIONS & ANONYMOUS METHODS                 ║");
        Console.WriteLine("║   Writing Inline Delegates with Modern C#                ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_AnonymousMethods.Run();
        Example2_LambdaBasics.Run();
        Example3_Closure.Run();
        Example4_LINQWithLambda.Run();
        Example5_GroupBy.Run();
        Example6_FilterMapReduce.Run();
        Example7_EventHandlers.Run();
        Example8_PredicateChain.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
