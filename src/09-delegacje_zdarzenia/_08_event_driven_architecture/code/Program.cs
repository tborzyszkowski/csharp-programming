using System;
using System.Collections.Generic;
using System.Linq;

namespace EventDrivenArchitecture;

// ==================== EVENT DEFINITIONS ====================

public record OrderCreatedEvent(int OrderId, string Customer, decimal Amount, DateTime CreatedAt);
public record OrderConfirmedEvent(int OrderId, DateTime ConfirmedAt);
public record PaymentProcessedEvent(int OrderId, decimal Amount, bool Success);
public record OrderShippedEvent(int OrderId, string TrackingNumber);
public record OrderCancelledEvent(int OrderId, string Reason);

// ==================== EVENT BUS ====================

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

    public void Publish<T>(T @event)
    {
        if (handlers.TryGetValue(typeof(T), out var list))
        {
            foreach (var handler in list.Cast<Action<T>>())
            {
                handler(@event);
            }
        }
    }
}

// ==================== ORDER SERVICE (Publisher) ====================

public class OrderService
{
    private EventBus eventBus;

    public OrderService(EventBus eventBus)
    {
        this.eventBus = eventBus;
    }

    public void CreateOrder(int orderId, string customer, decimal amount)
    {
        Console.WriteLine($"\n📋 Creating order #{orderId}...");
        var @event = new OrderCreatedEvent(orderId, customer, amount, DateTime.Now);
        eventBus.Publish(@event);
    }

    public void ConfirmOrder(int orderId)
    {
        Console.WriteLine($"\n✓ Confirming order #{orderId}...");
        var @event = new OrderConfirmedEvent(orderId, DateTime.Now);
        eventBus.Publish(@event);
    }

    public void ProcessPayment(int orderId, decimal amount, bool success)
    {
        Console.WriteLine($"\n💳 Processing payment for order #{orderId}...");
        var @event = new PaymentProcessedEvent(orderId, amount, success);
        eventBus.Publish(@event);
    }

    public void ShipOrder(int orderId, string tracking)
    {
        Console.WriteLine($"\n📦 Shipping order #{orderId}...");
        var @event = new OrderShippedEvent(orderId, tracking);
        eventBus.Publish(@event);
    }

    public void CancelOrder(int orderId, string reason)
    {
        Console.WriteLine($"\n❌ Cancelling order #{orderId}...");
        var @event = new OrderCancelledEvent(orderId, reason);
        eventBus.Publish(@event);
    }
}

// ==================== SUBSCRIBERS ====================

public class EmailNotificationService
{
    public void OnOrderCreated(OrderCreatedEvent e)
    {
        Console.WriteLine($"  📧 Email: Welcome {e.Customer}! Your order #{e.OrderId} created.");
    }

    public void OnOrderConfirmed(OrderConfirmedEvent e)
    {
        Console.WriteLine($"  📧 Email: Order #{e.OrderId} confirmed! Thank you.");
    }

    public void OnOrderShipped(OrderShippedEvent e)
    {
        Console.WriteLine($"  📧 Email: Order #{e.OrderId} shipped! Tracking: {e.TrackingNumber}");
    }

    public void OnOrderCancelled(OrderCancelledEvent e)
    {
        Console.WriteLine($"  📧 Email: Order #{e.OrderId} cancelled. Reason: {e.Reason}");
    }
}

public class AnalyticsService
{
    private int orderCount = 0;
    private decimal totalRevenue = 0;

    public void OnOrderCreated(OrderCreatedEvent e)
    {
        orderCount++;
        totalRevenue += e.Amount;
        Console.WriteLine($"  📊 Analytics: Total orders: {orderCount}, Revenue: ${totalRevenue}");
    }

    public void OnPaymentProcessed(PaymentProcessedEvent e)
    {
        if (e.Success)
            Console.WriteLine($"  📊 Analytics: Payment successful for order #{e.OrderId}");
        else
            Console.WriteLine($"  📊 Analytics: Payment failed for order #{e.OrderId}");
    }
}

public class InventoryService
{
    private Dictionary<int, int> inventory = new();

    public void OnOrderCreated(OrderCreatedEvent e)
    {
        // Symulacja rezerwacji
        Console.WriteLine($"  🏭 Inventory: Reserved items for order #{e.OrderId}");
    }

    public void OnOrderCancelled(OrderCancelledEvent e)
    {
        Console.WriteLine($"  🏭 Inventory: Released items from order #{e.OrderId}");
    }
}

public class LoggerService
{
    public void OnOrderCreated(OrderCreatedEvent e)
        => Console.WriteLine($"  📝 LOG: OrderCreated - Order#{e.OrderId}, Customer:{e.Customer}, Amount:${e.Amount}");

    public void OnOrderConfirmed(OrderConfirmedEvent e)
        => Console.WriteLine($"  📝 LOG: OrderConfirmed - Order#{e.OrderId}");

    public void OnPaymentProcessed(PaymentProcessedEvent e)
        => Console.WriteLine($"  📝 LOG: PaymentProcessed - Order#{e.OrderId}, Success:{e.Success}");

    public void OnOrderShipped(OrderShippedEvent e)
        => Console.WriteLine($"  📝 LOG: OrderShipped - Order#{e.OrderId}, Tracking:{e.TrackingNumber}");

    public void OnOrderCancelled(OrderCancelledEvent e)
        => Console.WriteLine($"  📝 LOG: OrderCancelled - Order#{e.OrderId}, Reason:{e.Reason}");
}

// ==================== MAIN ====================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   EVENT-DRIVEN ARCHITECTURE                               ║");
        Console.WriteLine("║   Order Management System Example                         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        // Setup
        var eventBus = new EventBus();
        var orderService = new OrderService(eventBus);

        // Services
        var emailService = new EmailNotificationService();
        var analytics = new AnalyticsService();
        var inventory = new InventoryService();
        var logger = new LoggerService();

        // Subscribe
        eventBus.Subscribe<OrderCreatedEvent>(emailService.OnOrderCreated);
        eventBus.Subscribe<OrderCreatedEvent>(analytics.OnOrderCreated);
        eventBus.Subscribe<OrderCreatedEvent>(inventory.OnOrderCreated);
        eventBus.Subscribe<OrderCreatedEvent>(logger.OnOrderCreated);

        eventBus.Subscribe<OrderConfirmedEvent>(emailService.OnOrderConfirmed);
        eventBus.Subscribe<OrderConfirmedEvent>(logger.OnOrderConfirmed);

        eventBus.Subscribe<PaymentProcessedEvent>(analytics.OnPaymentProcessed);
        eventBus.Subscribe<PaymentProcessedEvent>(logger.OnPaymentProcessed);

        eventBus.Subscribe<OrderShippedEvent>(emailService.OnOrderShipped);
        eventBus.Subscribe<OrderShippedEvent>(logger.OnOrderShipped);

        eventBus.Subscribe<OrderCancelledEvent>(emailService.OnOrderCancelled);
        eventBus.Subscribe<OrderCancelledEvent>(inventory.OnOrderCancelled);
        eventBus.Subscribe<OrderCancelledEvent>(logger.OnOrderCancelled);

        // Execute Order Flow 1
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("SCENARIO 1: Complete Order Flow");
        Console.WriteLine("=" + new string('=', 60));

        orderService.CreateOrder(1001, "Alice Johnson", 199.99m);
        orderService.ConfirmOrder(1001);
        orderService.ProcessPayment(1001, 199.99m, true);
        orderService.ShipOrder(1001, "TRACK-2024-001");

        // Execute Order Flow 2 - With Cancellation
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("SCENARIO 2: Order Cancelled");
        Console.WriteLine("=" + new string('=', 60));

        orderService.CreateOrder(1002, "Bob Smith", 299.99m);
        orderService.ProcessPayment(1002, 299.99m, false);
        orderService.CancelOrder(1002, "Payment declined");

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("✓ Event-Driven System Complete!");
        Console.WriteLine("=" + new string('=', 60));
    }
}
