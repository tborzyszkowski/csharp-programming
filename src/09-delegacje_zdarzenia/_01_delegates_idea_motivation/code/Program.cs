using System;
using System.Collections.Generic;
using System.Linq;

namespace DelegatesIdeaMotivation;

// ==================== PRZYKŁAD 1: PODSTAWOWY CALLBACK ====================

/// <summary>
/// Delegacja reprezentująca operację na liczbie
/// </summary>
public delegate void NumberAction(int number);

public class Example1_BasicCallback
{
    public static void ProcessNumbers(int[] numbers, NumberAction action)
    {
        Console.WriteLine("\n--- Processing Numbers ---");
        foreach (var n in numbers)
        {
            action(n);  // Wykonanie przekazanej logiki
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Basic Callback ===");
        int[] numbers = { 1, 2, 3, 4, 5 };

        // Drukuj wszystkie
        ProcessNumbers(numbers, n => Console.WriteLine($"Value: {n}"));

        // Drukuj tylko parzyste
        ProcessNumbers(numbers, n =>
        {
            if (n % 2 == 0)
                Console.WriteLine($"Even: {n}");
        });
    }
}

// ==================== PRZYKŁAD 2: LATE BINDING ====================

public delegate int BinaryOperation(int a, int b);

public class Example2_LateBinding
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Late Binding ===");

        BinaryOperation operation;
        int a = 10, b = 5;

        // Runtime decyzja - co będziemy robić?
        Console.Write("Wybierz operację (1=Add, 2=Multiply, 3=Subtract): ");
        string choice = Console.ReadLine() ?? "1";

        operation = choice switch
        {
            "1" => (x, y) =>
            {
                Console.WriteLine($"Adding {x} + {y}");
                return x + y;
            },
            "2" => (x, y) =>
            {
                Console.WriteLine($"Multiplying {x} * {y}");
                return x * y;
            },
            "3" => (x, y) =>
            {
                Console.WriteLine($"Subtracting {x} - {y}");
                return x - y;
            },
            _ => (x, y) => x + y
        };

        int result = operation(a, b);
        Console.WriteLine($"Result: {result}");
    }
}

// ==================== PRZYKŁAD 3: STRATEGY PATTERN ====================

/// <summary>
/// Reprezentuje strategię sortowania
/// </summary>
public delegate int ComparisonStrategy(int a, int b);

public class Example3_StrategyPattern
{
    public static void SortAndPrint(int[] numbers, ComparisonStrategy strategy, string strategyName)
    {
        var sorted = numbers.OrderBy(n => n, new DelegateComparer(strategy)).ToList();
        Console.WriteLine($"\nSorted ({strategyName}): {string.Join(", ", sorted)}");
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Strategy Pattern ===");
        int[] numbers = { 5, 2, 8, 1, 9, 3 };

        // Strategia 1: Rosnąco
        SortAndPrint(numbers, (a, b) => a.CompareTo(b), "Ascending");

        // Strategia 2: Malejąco
        SortAndPrint(numbers, (a, b) => b.CompareTo(a), "Descending");

        // Strategia 3: Po wartości bezwzględnej
        SortAndPrint(numbers, (a, b) => Math.Abs(a).CompareTo(Math.Abs(b)), "By Absolute Value");
    }

    private class DelegateComparer : IComparer<int>
    {
        private readonly ComparisonStrategy _strategy;

        public DelegateComparer(ComparisonStrategy strategy)
        {
            _strategy = strategy;
        }

        public int Compare(int x, int y) => _strategy(x, y);
    }
}

// ==================== PRZYKŁAD 4: ASYNCHRONICZNY CALLBACK ====================

public delegate void OperationCompleted(string result);

public class Example4_AsyncCallback
{
    public static void SimulateAsyncOperation(string operationName, OperationCompleted onComplete)
    {
        Console.WriteLine($"\nStarting: {operationName}...");
        
        // Symulacja pracy
        Thread.Sleep(500);
        
        string result = $"Completed: {operationName} - {DateTime.Now:HH:mm:ss}";
        
        // Wywoła callback
        onComplete(result);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Async Callback ===");

        // Callback 1: Drukuj wynik
        SimulateAsyncOperation("Download File", result =>
        {
            Console.WriteLine($"✓ {result}");
        });

        // Callback 2: Loguj do listy
        var log = new List<string>();
        SimulateAsyncOperation("Process Data", result =>
        {
            log.Add(result);
            Console.WriteLine($"✓ {result}");
        });

        Console.WriteLine($"\nLog entries: {log.Count}");
    }
}

// ==================== PRZYKŁAD 5: MULTICAST DELEGATES ====================

public delegate void NotificationAction(string message);

public class Example5_MulticastDelegates
{
    public static void NotifySubscribers(NotificationAction notifiers, string message)
    {
        Console.WriteLine($"\nSending notification: '{message}'");
        notifiers?.Invoke(message);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Multicast Delegates ===");

        // Łączenie delegacji - wielu obserwatorów
        NotificationAction notifiers = null!;

        // Dodaj logowanie do konsoli
        notifiers += msg => Console.WriteLine($"  [CONSOLE] {msg}");

        // Dodaj logowanie do pliku (symulacja)
        notifiers += msg => Console.WriteLine($"  [FILE] Logged: {msg}");

        // Dodaj wysyłanie email (symulacja)
        notifiers += msg => Console.WriteLine($"  [EMAIL] Sent: {msg}");

        // Wysłanie notyfikacji - wszystkie callbacks będą wykonane
        NotifySubscribers(notifiers, "Important Event!");

        Console.WriteLine("\n--- Removing file logger ---");
        // Usunięcie jednego callbacku
        notifiers -= msg => Console.WriteLine($"  [FILE] Logged: {msg}");

        NotifySubscribers(notifiers, "Another Event!");
    }
}

// ==================== PRZYKŁAD 6: TYPE SAFETY ====================

public delegate double MathOperation(double x, double y);
public delegate int IntOperation(int a, int b);

public class Example6_TypeSafety
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 6: Type Safety ===");

        // ✅ Poprawnie - typy się zgadzają
        MathOperation addDouble = (x, y) => x + y;
        Console.WriteLine($"5.5 + 3.2 = {addDouble(5.5, 3.2)}");

        IntOperation addInt = (a, b) => a + b;
        Console.WriteLine($"10 + 5 = {addInt(10, 5)}");

        // ❌ Compiler ERROR - odmienne typy parametrów
        // MathOperation wrong1 = (a, b) => a.ToString();  // ERROR: Expected double, got string

        // ❌ Compiler ERROR - odmienne typy zwracane
        // IntOperation wrong2 = (a, b) => a + b > 10;  // ERROR: Expected int, got bool

        // ❌ Compiler ERROR - liczba parametrów
        // MathOperation wrong3 = x => x + 1;  // ERROR: Expected 2 parameters, got 1

        Console.WriteLine("\n✓ All type checks passed!");
    }
}

// ==================== PRZYKŁAD 7: REAL-WORLD PATTERN ====================

public record DataRecord(string Name, int Value);

public delegate bool DataFilter(DataRecord record);
public delegate void DataProcessor(DataRecord record);

public class Example7_RealWorldPattern
{
    public static void ProcessData(
        IEnumerable<DataRecord> data,
        DataFilter filter,
        DataProcessor processor)
    {
        Console.WriteLine("\nProcessing data...");
        
        foreach (var record in data)
        {
            if (filter(record))
            {
                processor(record);
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 7: Real-World Pattern ===");

        var data = new[]
        {
            new DataRecord("Alice", 85),
            new DataRecord("Bob", 92),
            new DataRecord("Charlie", 78),
            new DataRecord("Diana", 95),
            new DataRecord("Eve", 88)
        };

        // Filtruj: wartość > 80
        DataFilter highScoreFilter = record => record.Value > 80;

        // Przetwarzaj: wydrukuj w specjalnym formacie
        DataProcessor printer = record =>
            Console.WriteLine($"  ✓ {record.Name}: {record.Value:D3}%");

        ProcessData(data, highScoreFilter, printer);

        Console.WriteLine("\n--- With Different Filter and Processor ---");

        // Inny filtr: nazwa zawiera 'a'
        DataFilter nameFilter = record => record.Name.ToLower().Contains('a');

        // Inny procesor: oblicz bonus
        DataProcessor bonusCalculator = record =>
        {
            int bonus = record.Value >= 90 ? 100 : 50;
            Console.WriteLine($"  💰 {record.Name} receives ${bonus} bonus");
        };

        ProcessData(data, nameFilter, bonusCalculator);
    }
}

// ==================== MAIN ====================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   DELEGATES: IDEA AND MOTIVATION                          ║");
        Console.WriteLine("║   Understanding WHY Delegates Exist                       ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_BasicCallback.Run();
        Example2_LateBinding.Run();
        Example3_StrategyPattern.Run();
        Example4_AsyncCallback.Run();
        Example5_MulticastDelegates.Run();
        Example6_TypeSafety.Run();
        Example7_RealWorldPattern.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
