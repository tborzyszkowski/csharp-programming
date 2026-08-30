# Temat 6: Wzorce Pracy ze Zdarzeniami

## 🎯 Cel Tematu

Praktyczne wzorce i best practices pracy z zdarzeniami.

---

## 📖 Zaawansowane Wzorce

### Pattern 1: Event Bus / Aggregator

```csharp
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
```

### Pattern 2: Weak Events (Zapobieganie Memory Leaks)

```csharp
public class WeakEventManager<TEventArgs> where TEventArgs : EventArgs
{
    private List<WeakReference> handlers = new();

    public void AddHandler(EventHandler handler)
    {
        handlers.Add(new WeakReference(handler));
    }

    public void RaiseEvent(object sender, TEventArgs e)
    {
        handlers.RemoveAll(wr => !wr.IsAlive);
        
        foreach (var wr in handlers)
        {
            if (wr.Target is EventHandler handler)
                handler(sender, e);
        }
    }
}
```

### Pattern 3: Async Events

```csharp
public delegate Task AsyncEventHandler(object? sender, EventArgs e);

public class AsyncEventPublisher
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
```

---

## 💡 Best Practices

1. **Zawsze sprawdzaj null**: `OnEvent?.Invoke(...)`
2. **Unsubscribe gdy nie potrzeba**: `event -= handler`
3. **Używaj EventArgs**: `new CustomEventArgs { ... }`
4. **Standardowy wzorzec**: `EventHandler<TEventArgs>`

---

## 🔗 Referencje

- [Event Patterns - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/fundamentals/events/how-to-raise-base-class-events-in-derived-classes)

---

## ➡️ Następny Krok

Temat 7: **Delegacje czy Zdarzenia?** - wybór w praktyce
