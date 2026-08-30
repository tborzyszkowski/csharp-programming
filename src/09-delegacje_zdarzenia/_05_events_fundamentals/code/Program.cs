using System;
using System.Collections.Generic;

namespace EventsFundamentals;

// ==================== PRZYKŁAD 1: BASIC EVENT ====================

public class Example1_BasicEvent
{
    public class Button
    {
        public event EventHandler? OnClick;

        public void Click()
        {
            Console.WriteLine("  Button clicked!");
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 1: Basic Event ===");

        var button = new Button();

        // Subscribe
        button.OnClick += (s, e) => Console.WriteLine("    Handler 1 executed");
        button.OnClick += (s, e) => Console.WriteLine("    Handler 2 executed");

        button.Click();
    }
}

// ==================== PRZYKŁAD 2: CUSTOM EVENTARGS ====================

public class Example2_CustomEventArgs
{
    public class DataReceivedEventArgs : EventArgs
    {
        public string Data { get; set; } = "";
        public DateTime ReceivedAt { get; set; }
    }

    public class Network
    {
        public event EventHandler<DataReceivedEventArgs>? OnDataReceived;

        public void ReceiveData(string data)
        {
            Console.WriteLine($"  Receiving: {data}");
            
            var args = new DataReceivedEventArgs
            {
                Data = data,
                ReceivedAt = DateTime.Now
            };

            OnDataReceived?.Invoke(this, args);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 2: Custom EventArgs ===");

        var network = new Network();

        network.OnDataReceived += (s, e) =>
            Console.WriteLine($"    Received: {e.Data} at {e.ReceivedAt:HH:mm:ss}");

        network.ReceiveData("Hello");
        network.ReceiveData("World");
    }
}

// ==================== PRZYKŁAD 3: PUBLISHER-SUBSCRIBER ====================

public class Example3_PublisherSubscriber
{
    public class Publisher
    {
        public event EventHandler? OnNotify;

        public void NotifyAll(string message)
        {
            Console.WriteLine($"  Publishing: {message}");
            OnNotify?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Subscriber
    {
        public string Name { get; }

        public Subscriber(string name)
        {
            Name = name;
        }

        public void HandleEvent(object? sender, EventArgs e)
        {
            Console.WriteLine($"    {Name} received notification");
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 3: Publisher-Subscriber ===");

        var publisher = new Publisher();

        var sub1 = new Subscriber("Alice");
        var sub2 = new Subscriber("Bob");
        var sub3 = new Subscriber("Charlie");

        // Subscribe
        publisher.OnNotify += sub1.HandleEvent;
        publisher.OnNotify += sub2.HandleEvent;
        publisher.OnNotify += sub3.HandleEvent;

        publisher.NotifyAll("Hello everyone!");

        Console.WriteLine("\n  Removing Bob...");
        publisher.OnNotify -= sub2.HandleEvent;

        publisher.NotifyAll("Only Alice and Charlie receive this");
    }
}

// ==================== PRZYKŁAD 4: SUBSCRIBE/UNSUBSCRIBE ====================

public class Example4_SubscribeUnsubscribe
{
    public class EventSource
    {
        public event EventHandler? OnEvent;

        public void RaiseEvent()
        {
            OnEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 4: Subscribe/Unsubscribe ===");

        var source = new EventSource();

        void Handler1(object? s, EventArgs e) => Console.WriteLine("    Handler 1");
        void Handler2(object? s, EventArgs e) => Console.WriteLine("    Handler 2");
        void Handler3(object? s, EventArgs e) => Console.WriteLine("    Handler 3");

        // Subscribe
        source.OnEvent += Handler1;
        source.OnEvent += Handler2;
        source.OnEvent += Handler3;

        Console.WriteLine("All handlers registered:");
        source.RaiseEvent();

        // Unsubscribe Handler2
        Console.WriteLine("\nRemoving Handler2:");
        source.OnEvent -= Handler2;
        source.RaiseEvent();

        // Unsubscribe all
        Console.WriteLine("\nRemoving all:");
        source.OnEvent -= Handler1;
        source.OnEvent -= Handler3;
        source.RaiseEvent();
    }
}

// ==================== PRZYKŁAD 5: EVENT ARGS PARAMETERS ====================

public class Example5_EventArgsParameters
{
    public class MouseClickEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public DateTime ClickTime { get; set; }
    }

    public class Window
    {
        public event EventHandler<MouseClickEventArgs>? OnMouseClick;

        public void SimulateClick(int x, int y)
        {
            var args = new MouseClickEventArgs
            {
                X = x,
                Y = y,
                ClickTime = DateTime.Now
            };

            OnMouseClick?.Invoke(this, args);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 5: EventArgs Parameters ===");

        var window = new Window();

        window.OnMouseClick += (s, e) =>
            Console.WriteLine($"    Clicked at ({e.X}, {e.Y}) at {e.ClickTime:HH:mm:ss}");

        window.SimulateClick(100, 200);
        window.SimulateClick(150, 250);
    }
}

// ==================== PRZYKŁAD 6: EVENT CHAINING ====================

public class Example6_EventChaining
{
    public class EventRelay
    {
        public event EventHandler? OnForward;

        private int eventCount = 0;

        public void RelayEvent(object? sender, EventArgs e)
        {
            eventCount++;
            Console.WriteLine($"    Relay #{eventCount} forwarding...");
            OnForward?.Invoke(this, e);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 6: Event Chaining ===");

        var source = new EventRelay();
        var relay = new EventRelay();

        // source -> relay -> final handler
        source.OnForward += relay.RelayEvent;
        relay.OnForward += (s, e) => Console.WriteLine("      Final handler");

        source.RelayEvent(null, EventArgs.Empty);
        source.RelayEvent(null, EventArgs.Empty);
    }
}

// ==================== PRZYKŁAD 7: MULTIPLE PUBLISHERS ====================

public class Example7_MultiplePublishers
{
    public class EventAggregator
    {
        private List<EventHandler>? handlers = null;

        public void Subscribe(EventHandler handler)
        {
            handlers += handler;
        }

        public void NotifyAll()
        {
            handlers?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void Run()
    {
        Console.WriteLine("\n=== EXAMPLE 7: Event Aggregator ===");

        var aggregator = new EventAggregator();

        aggregator.Subscribe((s, e) => Console.WriteLine("    Subscriber 1"));
        aggregator.Subscribe((s, e) => Console.WriteLine("    Subscriber 2"));

        aggregator.NotifyAll();
    }
}

// ==================== MAIN ====================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   EVENTS: FUNDAMENTALS                                    ║");
        Console.WriteLine("║   Publisher-Subscriber Pattern in .NET                    ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");

        Example1_BasicEvent.Run();
        Example2_CustomEventArgs.Run();
        Example3_PublisherSubscriber.Run();
        Example4_SubscribeUnsubscribe.Run();
        Example5_EventArgsParameters.Run();
        Example6_EventChaining.Run();
        Example7_MultiplePublishers.Run();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ All Examples Completed                                 ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
    }
}
