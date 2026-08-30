using System;

namespace DelegatesVsEvents;

// ==================== DELEGACJE - KIEDY? ====================

public class Example1_DelegatesUseCases
{
    // Przypadek 1: Callback
    public delegate void DownloadCallback(string result);
    
    public class FileDownloader
    {
        public void Download(string url, DownloadCallback onComplete)
        {
            Console.WriteLine($"  Downloading {url}...");
            string data = "File content";
            onComplete(data);
        }
    }

    // Przypadek 2: Strategy
    public static void ProcessArray(int[] data, Func<int, int, int> strategy)
    {
        Console.WriteLine($"  Using strategy");
        // process...
    }

    // Przypadek 3: Transformation (LINQ)
    public static void LINQExample()
    {
        var numbers = new[] { 1, 2, 3 };
        var doubled = Array.ConvertAll(numbers, n => n * 2);
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Delegacje - Kiedy? ===");

        var downloader = new FileDownloader();
        downloader.Download("file.txt", result =>
            Console.WriteLine($"    Downloaded: {result}"));
    }
}

// ==================== ZDARZENIA - KIEDY? ====================

public class Example2_EventsUseCases
{
    // Przypadek 1: Powiadomienia
    public class Button
    {
        public event EventHandler? OnClick;

        public void Click()
        {
            Console.WriteLine("  Button clicked!");
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }

    // Przypadek 2: Obserwowanie zmian
    public class Model
    {
        private string data = "";

        public string Data
        {
            get => data;
            set
            {
                data = value;
                OnDataChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler? OnDataChanged;
    }

    // Przypadek 3: Decoupling
    public class Application
    {
        public event EventHandler? OnError;

        public void ProcessData()
        {
            try
            {
                Console.WriteLine("  Processing...");
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Zdarzenia - Kiedy? ===");

        var button = new Button();
        button.OnClick += (s, e) => Console.WriteLine("    Handler 1");
        button.OnClick += (s, e) => Console.WriteLine("    Handler 2");
        button.Click();

        Console.WriteLine("\nModel change event:");
        var model = new Model();
        model.OnDataChanged += (s, e) => Console.WriteLine("    Data changed!");
        model.Data = "New value";
    }
}

// ==================== PORÓWNANIE ====================

public class Example3_Comparison
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Porównanie ===");

        Console.WriteLine("Delegacja - Callback (LINQ):");
        var nums = new[] { 1, 2, 3, 4, 5 };
        Func<int, bool> isEven = n => n % 2 == 0;
        var evens = Array.FindAll(nums, n => isEven(n));
        Console.WriteLine($"  Evens: {string.Join(", ", evens)}");

        Console.WriteLine("\nZdarzenie - Publisher-Subscriber:");
        var publisher = new SimplePublisher();
        publisher.OnNotify += () => Console.WriteLine("  Subscriber 1");
        publisher.OnNotify += () => Console.WriteLine("  Subscriber 2");
        publisher.Publish();
    }

    private class SimplePublisher
    {
        public event Action? OnNotify;

        public void Publish()
        {
            Console.WriteLine("  Publishing...");
            OnNotify?.Invoke();
        }
    }
}

// ==================== BEST PRACTICES ====================

public class Example4_BestPractices
{
    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Best Practices ===");

        Console.WriteLine("✅ Zdarzenie dla UI:");
        var ui = new UIComponent();
        ui.OnStateChanged += () => Console.WriteLine("  UI updated");
        ui.UpdateState("new state");

        Console.WriteLine("\n✅ Delegacja dla algoritmu:");
        var sorter = new DataSorter();
        sorter.Sort(new[] { 3, 1, 2 }, (a, b) => a.CompareTo(b));
        Console.WriteLine("  Sorted!");
    }

    private class UIComponent
    {
        public event Action? OnStateChanged;

        public void UpdateState(string newState)
        {
            Console.WriteLine($"  State: {newState}");
            OnStateChanged?.Invoke();
        }
    }

    private class DataSorter
    {
        public void Sort<T>(T[] data, Comparison<T> comparison)
        {
            Array.Sort(data, comparison);
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   DELEGATES VS EVENTS: CHOOSING THE RIGHT TOOL            ║");
        Console.WriteLine("║   When to Use Delegates and When to Use Events           ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_DelegatesUseCases.Run();
        Example2_EventsUseCases.Run();
        Example3_Comparison.Run();
        Example4_BestPractices.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
