# Temat 12: Reactive Extensions (IObservable Patterns)

## 📚 Koncepcja

Reactive Extensions (Rx) to biblioteka .NET do komponowania asynchronicznych i event-driven programów przy użyciu observable sequences i LINQ operators.

**Motto:** "Rx to LINQ dla events" 

```csharp
// Zamiast:
var button = new Button();
button.Click += (sender, e) => { /* handle click */ };

// Możesz:
Observable.FromEvent<EventHandler, EventArgs>(
    h => button.Click += h,
    h => button.Click -= h
)
.Subscribe(e => { /* handle click */ });
```

---

## 🎯 Kluczowe Koncepty

### 1. **IObservable<T> i IObserver<T>**

```csharp
// Producer
public interface IObservable<out T>
{
    IDisposable Subscribe(IObserver<T> observer);
}

// Consumer
public interface IObserver<in T>
{
    void OnNext(T value);
    void OnError(Exception error);
    void OnCompleted();
}
```

### 2. **Push vs Pull**

```csharp
// Pull (Enumerable) - kod pyta o dane
foreach (var item in list)
{
    Console.WriteLine(item);
}

// Push (Observable) - dane przychoddzą do kodu
observable.Subscribe(
    onNext: item => Console.WriteLine(item),
    onError: error => Console.WriteLine($"Error: {error}"),
    onCompleted: () => Console.WriteLine("Done")
);
```

### 3. **Lazy vs Eager Evaluation**

```csharp
// Enumerable (Lazy) - dane generowane na żądanie
var query = numbers
    .Where(x => x > 5)      // Nie wykonane jeszcze!
    .Select(x => x * 2);    // Nie wykonane jeszcze!

foreach (var item in query) // Teraz wykonane
{
    // Use item
}

// Observable (Push) - dane przychodzą natychmiast
var observable = Observable.Range(1, 10)
    .Where(x => x > 5)      // Rejestruje operację
    .Select(x => x * 2)     // Rejestruje operację
    .Subscribe(              // Teraz executes
        onNext: Console.WriteLine);
```

---

## 🔄 Observable Lifecycle

```
Subscription
    ↓
OnNext (zero lub więcej razy)
    ↓
OnCompleted lub OnError (dokładnie raz)
    ↓
Unsubscribe
```

### Przykład:
```csharp
observable.Subscribe(
    onNext: x => Console.WriteLine($"Value: {x}"),
    onError: e => Console.WriteLine($"Error: {e}"),
    onCompleted: () => Console.WriteLine("Completed")
);
```

---

## 💡 Praktyczne Wzorce

### 1. **Converting Events to Observables**
```csharp
// Button clicks jako observable
var clicks = Observable.FromEvent<EventHandler, EventArgs>(
    h => button.Click += h,
    h => button.Click -= h
);

clicks.Subscribe(_ => Console.WriteLine("Clicked!"));
```

### 2. **Mouse Movements**
```csharp
var mouseMoves = Observable.FromEvent<MouseEventHandler, MouseEventArgs>(
    h => form.MouseMove += h,
    h => form.MouseMove -= h
);

mouseMoves
    .Throttle(TimeSpan.FromMilliseconds(100))  // Co 100ms maksymalnie
    .Subscribe(e => Console.WriteLine($"Mouse: {e.X}, {e.Y}"));
```

### 3. **Timer / Polling**
```csharp
Observable.Interval(TimeSpan.FromSeconds(1))
    .Subscribe(x => Console.WriteLine($"Tick {x}"));
```

### 4. **Subject - Both Producer and Consumer**
```csharp
var subject = new Subject<string>();

// Multiple subscribers
subject.Subscribe(x => Console.WriteLine($"Sub1: {x}"));
subject.Subscribe(x => Console.WriteLine($"Sub2: {x}"));

// Multiple values
subject.OnNext("First");   // Obie subskrypcje dostają
subject.OnNext("Second");  // Obie subskrypcje dostają
subject.OnCompleted();     // Stream zakończony
```

### 5. **ReplaySubject - Replay Past Values**
```csharp
var replay = new ReplaySubject<string>(2);  // Replay 2 ostatnie wartości

replay.OnNext("A");
replay.OnNext("B");
replay.OnNext("C");

replay.Subscribe(x => Console.WriteLine(x));  // Wypisze: B, C
```

---

## 🔀 Operatory LINQ

### Transformacja
```csharp
observable.Select(x => x * 2)               // Map
observable.SelectMany(x => GetMore(x))      // Flat map
observable.Cast<int>()                      // Type cast
```

### Filtrowanie
```csharp
observable.Where(x => x > 5)                // Filter
observable.DistinctUntilChanged()           // Remove duplicates
observable.SkipWhile(x => x < 3)            // Skip until condition
observable.Take(5)                          // Take first 5
```

### Kombinacja
```csharp
Observable.Concat(obs1, obs2)               // Sekwencyjnie
Observable.Merge(obs1, obs2)                // Równocześnie
Observable.CombineLatest(obs1, obs2)        // Najnowsze z każdego
Observable.Zip(obs1, obs2)                  // Pair elements
```

### Temporalne
```csharp
observable.Delay(TimeSpan.FromSeconds(1))   // Opóźnij
observable.Throttle(TimeSpan.FromMilliseconds(500))  // Ogranicz częstotliwość
observable.Debounce(TimeSpan.FromMilliseconds(300))  // Czekaj na pauzę
observable.Buffer(5)                        // Group 5 items
```

### Akumulacja
```csharp
observable.Scan((acc, x) => acc + x)        // Running accumulation
observable.Aggregate((acc, x) => acc + x)   // Final result
```

---

## 🛠️ System.Reactive Package

```bash
dotnet add package System.Reactive
```

### Główne Klasy

```csharp
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

// Basics
Observable.Create<T>(observer => { ... })
Observable.Return<T>(value)
Observable.Empty<T>()
Observable.Throw<T>(exception)

// Timers
Observable.Interval(timespan)
Observable.Timer(dueTime, period)

// Events
Observable.FromEvent(...)
Observable.FromEventPattern(...)

// Conversion
Observable.ToObservable(enumerable)
```

---

## 📊 Observable vs Enumerable

| Aspekt | Enumerable (LINQ) | Observable (Rx) |
|--------|---------|----------|
| **Pull/Push** | Pull | Push |
| **Lazy** | Lazy (when enumerated) | Eager (when subscribed) |
| **Usage** | Collections | Events, Streams |
| **Multiple iterations** | Re-evaluates | Push to all subscribers |
| **Error handling** | Try-catch | OnError callback |
| **Completion** | When enumeration ends | OnCompleted callback |
| **Time-based** | Not built-in | Built-in (Delay, Throttle) |

---

## 🔗 Rx Pattern - Hot vs Cold

### Cold Observable
```csharp
// Każdy subscriber dostaje swoje dane
var cold = Observable.Range(1, 3);

cold.Subscribe(x => Console.WriteLine($"Sub1: {x}"));
cold.Subscribe(x => Console.WriteLine($"Sub2: {x}"));

// Output:
// Sub1: 1
// Sub1: 2
// Sub1: 3
// Sub2: 1
// Sub2: 2
// Sub2: 3
```

### Hot Observable
```csharp
// Wszyscy subscriberzy dzielą się danymi
var hot = new Subject<int>();

hot.Subscribe(x => Console.WriteLine($"Sub1: {x}"));
hot.OnNext(1);
hot.Subscribe(x => Console.WriteLine($"Sub2: {x}"));
hot.OnNext(2);

// Output:
// Sub1: 1
// Sub1: 2
// Sub2: 2
```

---

## 💪 Praktyczne Aplikacje Rx

### 1. **Auto-complete Search**
```csharp
textBox.TextChanged
    .Throttle(TimeSpan.FromMilliseconds(300))
    .DistinctUntilChanged()
    .SelectMany(query => SearchAsync(query))
    .ObserveOn(mainThread)
    .Subscribe(results => UpdateUI(results));
```

### 2. **Gesture Recognition**
```csharp
touches
    .Buffer(TimeSpan.FromMilliseconds(100))
    .Where(x => x.Count > 3)
    .Select(x => RecognizeGesture(x))
    .Subscribe(gesture => HandleGesture(gesture));
```

### 3. **Reactive Forms**
```csharp
nameField.TextChanged.Combine(
    emailField.TextChanged,
    (name, email) => new { name, email }
)
.Subscribe(x => ValidateForm(x));
```

---

## 📚 Referencje

- [System.Reactive](https://github.com/dotnet/reactive)
- [MSDN: Reactive Extensions](https://learn.microsoft.com/en-us/previous-versions/dotnet/reactive-extensions/hh242985)
- [RxJS Marbles (Visual Guide)](https://rxmarbles.com/)
- [Rx.NET Documentation](https://reactivex.io/)

---

## ✅ Summary

**Reactive Extensions** to potężne narzędzie do:
- Pracy z event streams
- Komponowania asynchronicznych operacji
- Obsługi złożonych scenariuszy UI
- Pracy z real-time data

**Kluczowe różnice od Async/Await:**
- `async/await` → dla einzelnego async operation
- `Rx` → dla strumieni (0, 1 lub więcej wartości)

**Następny krok:** Łącz Async/Await z Reactive Extensions dla maksymalnej mocy!
