# Temat 5: Zdarzenia - Fundamenty

## 🎯 Cel Tematu

Nauczysz się standardowego wzorca zdarzeń w .NET i jak zdarzenia różnią się od delegacji.

---

## 📖 Co To Jest Zdarzenie?

**Zdarzenie** to wrapper wokół delegacji, który:
- Pozwala **innym** rejestrować handlery
- Zapobiega **zmianom** listy handlerow z zewnątrz
- Przestrzega .NET standardów EventHandler

### Podstawowa Składnia

```csharp
// Delegacja - tylko dla przykładu
public delegate void EventOccurred(string message);

// Zdarzenie - klucz "event"
public event EventOccurred? OnSomethingHappened;

// Wywoływanie
OnSomethingHappened?.Invoke("Something happened!");

// Rejestracja handlera (z zewnątrz klasy)
obj.OnSomethingHappened += handler;  // ✅ OK
obj.OnSomethingHappened -= handler;  // ✅ OK
obj.OnSomethingHappened = handler;   // ❌ BŁĄD - nie można przypisać!
```

---

## 🏛️ Standardowy .NET Wzorzec Zdarzeń

### EventArgs - Informacje o Zdarzeniu

```csharp
// Standardowy wzorzec
public delegate void EventHandler(object? sender, EventArgs e);

// EventArgs zawiera dane o zdarzeniu
public class ClickedEventArgs : EventArgs
{
    public DateTime Timestamp { get; set; }
    public int ClickCount { get; set; }
}

// Zdarzenie
public event EventHandler? OnClicked;

// Wywoływanie
void RaiseClickEvent(int clickCount)
{
    var args = new ClickedEventArgs
    {
        Timestamp = DateTime.Now,
        ClickCount = clickCount
    };
    
    OnClicked?.Invoke(this, args);
}

// Rejestracja handlera
button.OnClicked += (sender, e) =>
{
    if (e is ClickedEventArgs args)
        Console.WriteLine($"Clicked {args.ClickCount} times at {args.Timestamp}");
};
```

---

## 📝 Publisher-Subscriber Pattern

### Architektura

```
┌─────────────┐
│ Publisher   │  Publikuje zdarzenia
│ (emitent)   │
└─────────────┘
       ↓ event
┌─────────────┐
│   Event     │  Transport
│   Handler   │  informacji
└─────────────┘
       ↓ callback
┌─────────────┐
│ Subscriber  │  Reaguje
│ (obserwator)│
└─────────────┘
```

### Implementacja

```csharp
// Publisher
public class Button
{
    public event EventHandler? OnClick;
    
    public void Click()
    {
        OnClick?.Invoke(this, EventArgs.Empty);
    }
}

// Subscriber
public class Logger
{
    public void HandleButtonClick(object? sender, EventArgs e)
    {
        Console.WriteLine("Button was clicked at " + DateTime.Now);
    }
}

// Użycie
var button = new Button();
var logger = new Logger();

button.OnClick += logger.HandleButtonClick;  // Subscribe
button.Click();  // Publikuje zdarzenie

button.OnClick -= logger.HandleButtonClick;  // Unsubscribe
```

---

## ⚠️ Unsubscribe Trap - Memory Leaks

### Problem

```csharp
// ❌ Memory leak - nigdy się nie wypisuje!
Subscriber sub = new Subscriber();
publisher.OnEvent += sub.Handle;

// Subscriber został usunięty (garbage collected)
sub = null;

// ALE: publisher nadal ma referencję w liście handlerów!
// sub nigdy się nie wyczyści z pamięci
```

### Rozwiązanie

```csharp
// ✅ Prawidłowe - zapisz referencję
Subscriber sub = new Subscriber();
publisher.OnEvent += sub.Handle;

// Później - odpis
publisher.OnEvent -= sub.Handle;
sub = null;  // Teraz może być garbage collected
```

---

## 🎬 Praktyczne Przykłady

### Przykład 1: Form Events

```csharp
public class Form
{
    public event EventHandler? OnLoad;
    public event EventHandler? OnClose;
    
    public void Load()
    {
        Console.WriteLine("Form loading...");
        OnLoad?.Invoke(this, EventArgs.Empty);
    }
    
    public void Close()
    {
        Console.WriteLine("Form closing...");
        OnClose?.Invoke(this, EventArgs.Empty);
    }
}

// Użycie
var form = new Form();
form.OnLoad += (s, e) => Console.WriteLine("  ✓ Form loaded!");
form.OnClose += (s, e) => Console.WriteLine("  ✓ Form closed!");

form.Load();
form.Close();
```

### Przykład 2: Custom EventArgs

```csharp
public class DataReceivedEventArgs : EventArgs
{
    public string Data { get; set; }
    public DateTime ReceivedAt { get; set; }
}

public class NetworkClient
{
    public event EventHandler<DataReceivedEventArgs>? OnDataReceived;
    
    public void SimulateDataReceival(string data)
    {
        var args = new DataReceivedEventArgs
        {
            Data = data,
            ReceivedAt = DateTime.Now
        };
        
        OnDataReceived?.Invoke(this, args);
    }
}

// Użycie
var client = new NetworkClient();
client.OnDataReceived += (s, e) => 
    Console.WriteLine($"Received: {e.Data} at {e.ReceivedAt}");

client.SimulateDataReceival("Hello");
```

---

## 📊 Zdarzenia vs Delegacje

| Aspekt | Delegacja | Zdarzenie |
|--------|-----------|----------|
| **Przypisanie** | `delegate_var = ...` | `event_var += ...` |
| **Bezpieczeństwo** | Brak ochrony | Ochrona przed zmianami |
| **Użycie** | Callback, transformation | Pub-Sub, notifications |
| **Wielokrotne subscribe** | Multicast | Event += |
| **Reset z poza klasy** | Możliwe | Niemożliwe |

---

## 🔗 Referencje

- [Events - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/events/)
- [EventHandler Pattern](https://learn.microsoft.com/en-us/dotnet/fundamentals/events/how-to-subscribe-to-and-unsubscribe-from-events)

---

## ➡️ Następny Krok

Temat 6: **Wzorce Pracy ze Zdarzeniami** - praktyka zaawansowana
