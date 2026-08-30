# Temat 8: Event-Driven Architecture - Większy Przykład

## 🎯 Cel Tematu

Praktyczny przykład - system zarządzania zamówieniami (e-commerce) używający delegacji i zdarzeń.

---

## 📖 Architektura Projektu

### Komponenty Systemu

```
┌──────────────────────────────────────────────────────────┐
│                    ORDER MANAGEMENT SYSTEM               │
├──────────────────────────────────────────────────────────┤
│                                                          │
│  ┌─────────────┐  ┌──────────┐  ┌───────────────────┐  │
│  │   OrderService  →  Events →  │  Subscribers    │  │
│  │   (Publisher)│  │(EventBus)│  │  (Analytics,   │  │
│  └─────────────┘  └──────────┘  │   Email, etc)  │  │
│                                  └───────────────────┘  │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

### Event Types

1. **OrderCreated** - Zamówienie utworzone
2. **OrderConfirmed** - Zamówienie potwierdzone
3. **PaymentProcessed** - Płatność przetworzzona
4. **OrderShipped** - Zamówienie wysłane
5. **OrderCancelled** - Zamówienie anulowane

### Subscribers

- **EmailNotifier** - wysyła emaile
- **AnalyticsService** - zbiera statystyki
- **InventoryService** - aktualizuje magazyn
- **Logger** - loguje wszystko

---

## 🏗️ Implementacja

### Event Definitions

```csharp
public record OrderCreatedEvent(int OrderId, string CustomerName, decimal Amount);
public record OrderConfirmedEvent(int OrderId, DateTime ConfirmedAt);
public record PaymentProcessedEvent(int OrderId, decimal Amount, bool Success);
public record OrderShippedEvent(int OrderId, string TrackingNumber);
public record OrderCancelledEvent(int OrderId, string Reason);
```

### Event Bus (Centralized)

```csharp
public class EventBus
{
    private Dictionary<Type, List<Delegate>> handlers = new();

    public void Subscribe<T>(Action<T> handler)
    {
        // Zarejestruj handler
    }

    public void Publish<T>(T @event)
    {
        // Opublikuj event
    }
}
```

### Services (Publishers)

```csharp
public class OrderService
{
    private EventBus eventBus;

    public void CreateOrder(int orderId, string customer, decimal amount)
    {
        eventBus.Publish(new OrderCreatedEvent(orderId, customer, amount));
    }
}
```

### Subscribers

```csharp
public class EmailNotifier
{
    public void OnOrderCreated(OrderCreatedEvent e)
    {
        Console.WriteLine($"Sending email to {e.CustomerName}...");
    }
}

public class AnalyticsService
{
    public void OnOrderCreated(OrderCreatedEvent e)
    {
        Console.WriteLine($"Recording order: {e.OrderId}");
    }
}
```

---

## 💡 Advantages

1. **Loose Coupling** - komponenty niezależne
2. **Scalability** - łatwo dodać nowych subscriberów
3. **Testability** - łatwo mockować eventy
4. **Flexibility** - łatwo zmienić logikę biznesową

---

## 🔗 Referencje

- [Event-Driven Architecture](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/event-driven)
- [Pub-Sub Pattern](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-queues-topics-subscriptions)
- [Async/Await with Events](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)

---

## ✨ Nowoczesne Rozszerzenia

### Reactive Extensions (RxJS)
```csharp
// IObservable<T> - deklaracyjny event stream
IObservable<OrderEvent> orderStream = ...
orderStream.Where(e => e.Amount > 1000)
           .Select(e => new HighValueOrderEvent(e))
           .Subscribe(Console.WriteLine);
```

### Async/Await
```csharp
public event Func<OrderEvent, Task>? OnOrderAsync;

await OnOrderAsync?.Invoke(orderEvent)!;
```

---

## ➡️ Koniec Modułu!

Ukończyłeś kompleksowy kurs na temat delegacji i zdarzeń w C#!
