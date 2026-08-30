using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventsPatterns;

public class Example1_EventBus
{
    public class EventBus
    {
        private Dictionary<Type, List<Delegate>> handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var eventType = typeof(T);
            if (!handlers.ContainsKey(eventType))
                handlers[eventType] = new();
            handlers[eventType].Add(handler);
        }

        public void Publish<T>(T eventData)
        {
            if (handlers.TryGetValue(typeof(T), out var list))
            {
                foreach (var handler in list.Cast<Action<T>>())
                    handler(eventData);
            }
        }
    }

    public record UserSignedUp(string Name, DateTime SignedAt);
    public record UserSignedOut(string Name, DateTime SignedAt);

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Event Bus Pattern ===");

        var bus = new EventBus();

        // Subscribe
        bus.Subscribe<UserSignedUp>(e =>
            Console.WriteLine($"  [{e.SignedAt:HH:mm:ss}] Welcome {e.Name}!"));

        bus.Subscribe<UserSignedOut>(e =>
            Console.WriteLine($"  [{e.SignedAt:HH:mm:ss}] Goodbye {e.Name}!"));

        // Publish
        bus.Publish(new UserSignedUp("Alice", DateTime.Now));
        bus.Publish(new UserSignedOut("Alice", DateTime.Now));
    }
}

public class Example2_AsyncEvents
{
    public delegate Task AsyncEventHandler(object? sender, EventArgs e);

    public class AsyncPublisher
    {
        public event AsyncEventHandler? OnEvent;

        public async Task RaiseEventAsync()
        {
            if (OnEvent != null)
            {
                var tasks = OnEvent.GetInvocationList()
                    .Cast<AsyncEventHandler>()
                    .Select(h => h(this, EventArgs.Empty));
                
                await Task.WhenAll(tasks);
            }
        }
    }

    public static async Task Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Async Events ===");

        var publisher = new AsyncPublisher();

        publisher.OnEvent += async (s, e) =>
        {
            Console.WriteLine("  Handler 1 starting...");
            await Task.Delay(1000);
            Console.WriteLine("  Handler 1 done!");
        };

        publisher.OnEvent += async (s, e) =>
        {
            Console.WriteLine("  Handler 2 starting...");
            await Task.Delay(500);
            Console.WriteLine("  Handler 2 done!");
        };

        await publisher.RaiseEventAsync();
        Console.WriteLine("  All handlers completed!");
    }
}

public class Example3_MultipleSubscribers
{
    public class DataSource
    {
        public event EventHandler? OnDataChanged;

        public void ChangeData()
        {
            Console.WriteLine("  Data changed!");
            OnDataChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Multiple Subscribers ===");

        var source = new DataSource();

        // Multiple subscribers
        source.OnDataChanged += (s, e) => Console.WriteLine("    Logger 1: Data changed");
        source.OnDataChanged += (s, e) => Console.WriteLine("    Logger 2: Data changed");
        source.OnDataChanged += (s, e) => Console.WriteLine("    UI: Data changed");
        source.OnDataChanged += (s, e) => Console.WriteLine("    Analytics: Data changed");

        source.ChangeData();
    }
}

public class Example4_SafeUnsubscribe
{
    public class EventPublisher
    {
        public event EventHandler? OnEvent;

        public void RaiseEvent()
        {
            OnEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Safe Unsubscribe ===");

        var publisher = new EventPublisher();

        void Handler1(object? s, EventArgs e) => Console.WriteLine("  Handler 1");
        void Handler2(object? s, EventArgs e) => Console.WriteLine("  Handler 2");

        publisher.OnEvent += Handler1;
        publisher.OnEvent += Handler2;

        publisher.RaiseEvent();

        Console.WriteLine("\nAfter unsubscribe:");
        publisher.OnEvent -= Handler1;
        publisher.RaiseEvent();
    }
}

public class Example5_EventChaining
{
    public class Stage1
    {
        public event EventHandler? OnComplete;

        public void Execute()
        {
            Console.WriteLine("  Stage 1 executing...");
            OnComplete?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Stage2
    {
        public event EventHandler? OnComplete;

        public void Execute()
        {
            Console.WriteLine("  Stage 2 executing...");
            OnComplete?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: Event Chaining ===");

        var stage1 = new Stage1();
        var stage2 = new Stage2();

        stage1.OnComplete += (s, e) => stage2.Execute();
        stage2.OnComplete += (s, e) => Console.WriteLine("  All stages complete!");

        stage1.Execute();
    }
}

class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   EVENTS: PATTERNS AND BEST PRACTICES                     ║");
        Console.WriteLine("║   Advanced Patterns for Working with Events              ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_EventBus.Run();
        await Example2_AsyncEvents.Run();
        Example3_MultipleSubscribers.Run();
        Example4_SafeUnsubscribe.Run();
        Example5_EventChaining.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
