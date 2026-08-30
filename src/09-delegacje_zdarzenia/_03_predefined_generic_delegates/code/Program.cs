using System;
using System.Collections.Generic;
using System.Linq;

namespace PredefinedGenericDelegates;

// ==================== PRZYKŁAD 1: ACTION ====================

public class Example1_Action
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Action<T> ===");

        // Action<string> - wypisuj wiadomość
        Action<string> greet = name => Console.WriteLine($"  Hello, {name}!");
        greet("Alice");
        greet("Bob");

        // Action<int, int> - operacja na dwóch liczbach
        Action<int, int> multiply = (a, b) => 
            Console.WriteLine($"  {a} × {b} = {a * b}");
        multiply(5, 3);

        // Action bez parametrów
        Action log = () => Console.WriteLine($"  Logged at {DateTime.Now:HH:mm:ss}");
        log();

        // ForEach z Action
        Console.WriteLine("\nProcessing list with ForEach:");
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        numbers.ForEach(n => Console.WriteLine($"  Value: {n}"));
    }
}

// ==================== PRZYKŁAD 2: FUNC ====================

public class Example2_Func
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Func<T, TResult> ===");

        // Func<int, int> - kwadrat
        Func<int, int> square = x => x * x;
        Console.WriteLine($"Square of 5: {square(5)}");

        // Func<int, int, int> - dodawanie
        Func<int, int, int> add = (a, b) => a + b;
        Console.WriteLine($"Add 10 + 5: {add(10, 5)}");

        // Func<string, int> - liczba słów
        Func<string, int> wordCount = text => text.Split(' ').Length;
        Console.WriteLine($"Word count in 'Hello C# World': {wordCount("Hello C# World")}");

        // Func w LINQ
        Console.WriteLine("\nUsing Func with LINQ:");
        var numbers = new[] { 1, 2, 3, 4, 5 };
        Func<int, string> format = n => $"[{n}]";
        var formatted = numbers.Select(format).ToList();
        Console.WriteLine($"Formatted: {string.Join(", ", formatted)}");

        // Func dla transformacji
        Console.WriteLine("\nChaining transformations:");
        Func<int, int> double_val = x => x * 2;
        Func<int, int> addTen = x => x + 10;
        Func<int, int> square_again = x => x * x;

        int value = 5;
        value = double_val(value);      // 10
        value = addTen(value);           // 20
        value = square_again(value);     // 400
        Console.WriteLine($"Final result: {value}");
    }
}

// ==================== PRZYKŁAD 3: PREDICATE ====================

public class Example3_Predicate
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Predicate<T> ===");

        // Predicate<int> - czy dodatnie?
        Predicate<int> isPositive = n => n > 0;
        Console.WriteLine($"Is 5 positive? {isPositive(5)}");
        Console.WriteLine($"Is -5 positive? {isPositive(-5)}");

        // Predicate<string> - pusta?
        Predicate<string> isEmpty = s => string.IsNullOrWhiteSpace(s);
        Console.WriteLine($"Is '' empty? {isEmpty("")}");
        Console.WriteLine($"Is 'hello' empty? {isEmpty("hello")}");

        // Predicate z List.FindAll
        Console.WriteLine("\nFinding numbers with predicate:");
        var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        Predicate<int> isEven = n => n % 2 == 0;
        var evens = numbers.FindAll(isEven);
        Console.WriteLine($"Even numbers: {string.Join(", ", evens)}");

        Predicate<int> greaterThanFive = n => n > 5;
        var greaters = numbers.FindAll(greaterThanFive);
        Console.WriteLine($"Greater than 5: {string.Join(", ", greaters)}");

        // Predicate w FindIndex
        Console.WriteLine("\nFinding index:");
        int indexOfEight = numbers.FindIndex(n => n == 8);
        Console.WriteLine($"Index of 8: {indexOfEight}");
    }
}

// ==================== PRZYKŁAD 4: PORÓWNANIE ====================

public record Product(string Name, int Price);

public class Example4_Comparison
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Action vs Func vs Predicate ===");

        var products = new List<Product>
        {
            new("Laptop", 1000),
            new("Mouse", 20),
            new("Monitor", 300),
            new("Keyboard", 80),
            new("USB Cable", 5)
        };

        Console.WriteLine("All products:");
        // Action - efekt uboczny (wypisanie)
        products.ForEach(p => Console.WriteLine($"  - {p.Name}: ${p.Price}"));

        Console.WriteLine("\nTransformed (Func):");
        // Func - transformacja (zwrócenie wartości)
        Func<Product, string> toDisplayString = p => $"{p.Name} (${p.Price})";
        var displayed = products.Select(toDisplayString).ToList();
        displayed.ForEach(s => Console.WriteLine($"  {s}"));

        Console.WriteLine("\nFiltered (Predicate):");
        // Predicate - test warunku
        Predicate<Product> isExpensive = p => p.Price > 100;
        var expensive = products.FindAll(isExpensive);
        expensive.ForEach(p => Console.WriteLine($"  - {p.Name}: ${p.Price}"));
    }
}

// ==================== PRZYKŁAD 5: PIPELINE ====================

public class DataPipeline
{
    private List<Func<string, string>> transformations = new();

    public void AddStep(Func<string, string> transformation)
    {
        transformations.Add(transformation);
    }

    public string Execute(string input)
    {
        Console.WriteLine($"Input: '{input}'");
        
        var result = input;
        foreach (var transform in transformations)
        {
            result = transform(result);
            Console.WriteLine($"After transform: '{result}'");
        }
        
        return result;
    }
}

public class Example5_Pipeline
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Processing Pipeline with Func ===");

        var pipeline = new DataPipeline();
        pipeline.AddStep(s => s.Trim());
        pipeline.AddStep(s => s.ToUpper());
        pipeline.AddStep(s => s.Replace(' ', '_'));
        pipeline.AddStep(s => $"[{s}]");

        string result = pipeline.Execute("  hello world  ");
        Console.WriteLine($"Final: '{result}'");
    }
}

// ==================== PRZYKŁAD 6: FILTERED COLLECTION ====================

public class FilteredCollection<T>
{
    private List<T> items = new();

    public void Add(T item) => items.Add(item);

    public List<T> GetFiltered(Predicate<T> filter) => items.FindAll(filter);

    public void ProcessAll(Action<T> action) => items.ForEach(action);

    public List<R> TransformAll<R>(Func<T, R> transformer) 
        => items.Select(transformer).ToList();

    public int Count => items.Count;
}

public class Example6_FilteredCollection
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 6: Filtered Collection with Action/Func/Predicate ===");

        var numbers = new FilteredCollection<int>();
        for (int i = 1; i <= 10; i++)
            numbers.Add(i);

        Console.WriteLine("All numbers (Action):");
        numbers.ProcessAll(n => Console.WriteLine($"  {n}"));

        Console.WriteLine("\nEven numbers (Predicate):");
        var evens = numbers.GetFiltered(n => n % 2 == 0);
        evens.ForEach(n => Console.WriteLine($"  {n}"));

        Console.WriteLine("\nSquared values (Func):");
        var squared = numbers.TransformAll(n => n * n);
        squared.ForEach(s => Console.WriteLine($"  {s}"));

        Console.WriteLine("\nFiltered & Transformed:");
        var filtered = numbers.GetFiltered(n => n > 5);
        var transformed = numbers.TransformAll(n => $"Number: {n}");
        transformed.ForEach(t => Console.WriteLine($"  {t}"));
    }
}

// ==================== PRZYKŁAD 7: LINQ WITH DELEGATES ====================

public class Example7_LINQWithDelegates
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 7: LINQ with Action/Func/Predicate ===");

        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Console.WriteLine("Where (Predicate-like):");
        var result1 = numbers.Where(n => n > 5).ToList();
        result1.ForEach(n => Console.WriteLine($"  {n}"));

        Console.WriteLine("\nSelect (Func-like):");
        var result2 = numbers.Select(n => n * n).ToList();
        result2.ForEach(n => Console.WriteLine($"  {n}"));

        Console.WriteLine("\nForEach (Action):");
        numbers.ForEach(n => Console.WriteLine($"  Value: {n}"));

        Console.WriteLine("\nComplex LINQ chain:");
        var result3 = numbers
            .Where(n => n % 2 == 0)           // Filtruj (Predicate)
            .Select(n => n * 2)               // Transformuj (Func)
            .OrderByDescending(n => n)
            .ToList();
        
        Console.WriteLine("Even numbers doubled, descending:");
        result3.ForEach(n => Console.WriteLine($"  {n}"));
    }
}

// ==================== MAIN ====================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   PREDEFINED GENERIC DELEGATES                            ║");
        Console.WriteLine("║   Action<T>, Func<T, TResult>, Predicate<T>              ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_Action.Run();
        Example2_Func.Run();
        Example3_Predicate.Run();
        Example4_Comparison.Run();
        Example5_Pipeline.Run();
        Example6_FilteredCollection.Run();
        Example7_LINQWithDelegates.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
