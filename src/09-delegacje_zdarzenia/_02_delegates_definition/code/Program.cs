using System;
using System.Collections.Generic;

namespace DelegatesDefinition;

// ==================== PRZYKŁAD 1: DEKLARACJA ====================

public delegate void SimpleDelegate();
public delegate int MathDelegate(int a, int b);
public delegate string TextDelegate(string input);
public delegate bool PredicateDelegate<T>(T value);

public class Example1_Declaration
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Delegate Declaration ===");
        Console.WriteLine("Delegacje zadeklarowane:");
        Console.WriteLine("  - SimpleDelegate: void()");
        Console.WriteLine("  - MathDelegate: int(int, int)");
        Console.WriteLine("  - TextDelegate: string(string)");
        Console.WriteLine("  - PredicateDelegate<T>: bool(T)");
    }
}

// ==================== PRZYKŁAD 2: INSTANCJOWANIE ====================

public class Example2_Instantiation
{
    public delegate int BinaryOp(int a, int b);

    // Nazwanoszcze metody
    public static int Add(int x, int y) => x + y;
    public static int Multiply(int x, int y) => x * y;

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Delegate Instantiation ===");

        // Metoda 1: Przypisanie metody nazwane
        Console.WriteLine("\n[Method 1] Named Method:");
        BinaryOp op1 = Add;
        Console.WriteLine($"op1(10, 5) = {op1(10, 5)}");

        // Metoda 2: Anonimowa metoda
        Console.WriteLine("\n[Method 2] Anonymous Method:");
        BinaryOp op2 = delegate(int a, int b)
        {
            Console.WriteLine($"  Anonymous: computing {a} - {b}");
            return a - b;
        };
        Console.WriteLine($"op2(10, 5) = {op2(10, 5)}");

        // Metoda 3: Lambda (expression body)
        Console.WriteLine("\n[Method 3] Lambda (single line):");
        BinaryOp op3 = (a, b) => a * b;
        Console.WriteLine($"op3(10, 5) = {op3(10, 5)}");

        // Metoda 4: Lambda (statement body)
        Console.WriteLine("\n[Method 4] Lambda (multi-line):");
        BinaryOp op4 = (a, b) =>
        {
            Console.WriteLine($"  Lambda: computing {a} / {b}");
            return b != 0 ? a / b : 0;
        };
        Console.WriteLine($"op4(10, 5) = {op4(10, 5)}");
    }
}

// ==================== PRZYKŁAD 3: WYWOŁYWANIE ====================

public class Example3_Invocation
{
    public delegate void Notify(string message);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Delegate Invocation ===");

        Notify notifier = msg => Console.WriteLine($"  ✓ {msg}");

        // Metoda 1: Bezpośrednie wywołanie
        Console.WriteLine("\n[Method 1] Direct Invocation:");
        notifier("Notification 1");
        notifier("Notification 2");

        // Metoda 2: Invoke
        Console.WriteLine("\n[Method 2] Using Invoke():");
        notifier.Invoke("Notification 3");

        // Metoda 3: Sprawdzenie null
        Console.WriteLine("\n[Method 3] Safe Invocation with Null Check:");
        Notify? nullNotifier = null;
        nullNotifier?.Invoke("This won't print");  // Nic się nie stanie
        Console.WriteLine("  ✓ No error occurred");

        // Metoda 4: Conditional invocation
        Console.WriteLine("\n[Method 4] Conditional Invocation:");
        if (notifier != null)
        {
            notifier("Notification 4");
        }
    }
}

// ==================== PRZYKŁAD 4: MULTICAST DELEGATES ====================

public class Example4_Multicast
{
    public delegate void Logger(string message);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Multicast Delegates ===");

        // Tworzymy delegaty dla różnych loggerów
        void ConsoleLogger(string msg) => Console.WriteLine($"  [CONSOLE] {msg}");
        void FileLogger(string msg) => Console.WriteLine($"  [FILE] {msg}");
        void AlertLogger(string msg) => Console.WriteLine($"  [ALERT!!!] {msg}");

        // Łączymy delegaty
        Logger allLoggers = null!;
        allLoggers += ConsoleLogger;
        allLoggers += FileLogger;

        Console.WriteLine("\n[Step 1] Two loggers (Console + File):");
        allLoggers("System started");

        // Dodaj trzeciego
        allLoggers += AlertLogger;
        Console.WriteLine("\n[Step 2] Three loggers (Console + File + Alert):");
        allLoggers("Important event!");

        // Usuń File Logger
        allLoggers -= FileLogger;
        Console.WriteLine("\n[Step 3] Removed File Logger (Console + Alert):");
        allLoggers("Another event");

        // Wyczyść wszystko
        allLoggers = null!;
        Console.WriteLine("\n[Step 4] Cleared all loggers:");
        allLoggers?.Invoke("This won't print");
        Console.WriteLine("  ✓ Safe null check prevented error");
    }
}

// ==================== PRZYKŁAD 5: CHAIN OF COMMAND ====================

public class Example5_ChainOfCommand
{
    public delegate string DataProcessor(string data);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Chain of Command ===");

        // Pojedyncze procesory
        string ToUpperCase(string s) => s.ToUpper();
        string AddPrefix(string s) => $">>> {s}";
        string Wrap(string s) => $"[{s}]";

        DataProcessor processor = ToUpperCase;
        processor += AddPrefix;
        processor += Wrap;

        string input = "hello world";
        Console.WriteLine($"Input: {input}");

        // Każdy procesor modyfikuje wynik
        string result = input;
        result = ToUpperCase(result);
        Console.WriteLine($"After ToUpperCase: {result}");

        result = AddPrefix(result);
        Console.WriteLine($"After AddPrefix: {result}");

        result = Wrap(result);
        Console.WriteLine($"After Wrap: {result}");
    }
}

// ==================== PRZYKŁAD 6: ZWRACANE WARTOŚCI ====================

public class Example6_ReturnValues
{
    public delegate int Calculator(int a, int b);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 6: Delegate Return Values ===");

        Calculator calc = null!;

        // Dodaj operacje
        calc += (a, b) =>
        {
            Console.WriteLine($"  Operation 1: {a} + {b} = {a + b}");
            return a + b;
        };

        calc += (a, b) =>
        {
            Console.WriteLine($"  Operation 2: {a} * {b} = {a * b}");
            return a * b;
        };

        calc += (a, b) =>
        {
            Console.WriteLine($"  Operation 3: {a} - {b} = {a - b}");
            return a - b;
        };

        Console.WriteLine("\nExecuting multicast delegate:");
        int finalResult = calc(10, 3);

        Console.WriteLine($"\nNote: Final result is from LAST delegate: {finalResult}");
        Console.WriteLine("(Previous results are computed but overwritten)");
    }
}

// ==================== PRZYKŁAD 7: CLOSURE ====================

public class Example7_Closure
{
    public delegate void Counter();

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 7: Closure in Delegates ===");

        // Każda delegacja "pamięta" zmienną lokalną
        var delegates = new List<Counter>();

        for (int i = 0; i < 5; i++)
        {
            int captured = i;  // Przechwycona zmienna
            delegates.Add(() => Console.WriteLine($"  Captured value: {captured}"));
        }

        Console.WriteLine("\nWithout closure (would all print 5):");
        Console.WriteLine("  (C# automatically captures by value in simple loops)");

        Console.WriteLine("\nActual output with proper closure:");
        foreach (var del in delegates)
        {
            del();
        }

        // Inny przykład
        Console.WriteLine("\nClosing over mutable variable:");
        var multipliers = new List<Func<int, int>>();

        for (int factor = 1; factor <= 3; factor++)
        {
            multipliers.Add(x => x * factor);
        }

        Console.WriteLine("Multiplying 5 with different factors:");
        for (int i = 0; i < multipliers.Count; i++)
        {
            Console.WriteLine($"  Factor {i + 1}: 5 * {i + 1} = {multipliers[i](5)}");
        }
    }
}

// ==================== PRZYKŁAD 8: GENERIC DELEGATES ====================

public class Example8_GenericDelegates
{
    public delegate T Transform<T>(T input);
    public delegate bool Condition<T>(T value);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 8: Generic Delegates ===");

        // Generic delegate dla Int
        Transform<int> doubleInt = x => x * 2;
        Console.WriteLine($"Transform<int>: 5 * 2 = {doubleInt(5)}");

        // Generic delegate dla String
        Transform<string> toUpper = s => s.ToUpper();
        Console.WriteLine($"Transform<string>: 'hello' -> '{toUpper("hello")}'");

        // Predicate
        Condition<int> isEven = n => n % 2 == 0;
        Console.WriteLine($"\nCondition<int>: Is 4 even? {isEven(4)}");
        Console.WriteLine($"Condition<int>: Is 5 even? {isEven(5)}");

        Condition<string> isLongString = s => s.Length > 5;
        Console.WriteLine($"\nCondition<string>: Is 'hello' long? {isLongString("hello")}");
        Console.WriteLine($"Condition<string>: Is 'hello world' long? {isLongString("hello world")}");
    }
}

// ==================== MAIN ====================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   DELEGATES: DEFINITION AND SYNTAX                        ║");
        Console.WriteLine("║   How to Declare, Instantiate, and Invoke Delegates       ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_Declaration.Run();
        Example2_Instantiation.Run();
        Example3_Invocation.Run();
        Example4_Multicast.Run();
        Example5_ChainOfCommand.Run();
        Example6_ReturnValues.Run();
        Example7_Closure.Run();
        Example8_GenericDelegates.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
