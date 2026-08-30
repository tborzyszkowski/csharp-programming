# Ćwiczenia: Reactive Extensions (Rx)

## 🟢 Basic Level

### Zadanie 1: Simple Observable
Stwórz observable, który emituje 5 wartości i zasubskrybuj go:

```csharp
Observable.Range(1, 5)
    .Subscribe(x => Console.WriteLine(x));
```

**Rozwiązanie:**
```csharp
Observable.Range(1, 5)
    .Subscribe(
        onNext: x => Console.WriteLine($"Value: {x}"),
        onCompleted: () => Console.WriteLine("Done!")
    );
```

---

### Zadanie 2: Filter Values
Filtruj observable, aby wybrać tylko wartości większe niż 3:

```csharp
Observable.Range(1, 10)
    .Where(x => x > 3)
    .Subscribe(x => Console.WriteLine(x));
```

**Rozwiązanie:**
```csharp
Observable.Range(1, 10)
    .Where(x => x > 3)
    .Subscribe(x => Console.WriteLine($"Filtered: {x}"));
```

---

### Zadanie 3: Transform Values
Transformuj wartości przez mnożenie przez 2:

```csharp
Observable.Range(1, 5)
    .Select(x => x * 2)
    .Subscribe(x => Console.WriteLine(x));
```

**Rozwiązanie:**
```csharp
Observable.Range(1, 5)
    .Select(x => x * 2)
    .Subscribe(x => Console.WriteLine($"Transformed: {x}"));
```

---

### Zadanie 4: Subject Pattern
Stwórz subject i wyślij kilka wartości do wielu subscriberów:

```csharp
var subject = new Subject<int>();

subject.Subscribe(x => Console.WriteLine($"Sub1: {x}"));
subject.OnNext(1);
subject.Subscribe(x => Console.WriteLine($"Sub2: {x}"));
subject.OnNext(2);
```

---

### Zadanie 5: Error Handling
Obsłuż błąd w observable:

```csharp
Observable.Create<int>(observer =>
{
    observer.OnNext(1);
    observer.OnError(new Exception("Test error"));
    return () => { };
})
.Subscribe(
    onNext: x => Console.WriteLine(x),
    onError: e => Console.WriteLine($"Error: {e.Message}")
);
```

---

## 🟡 Intermediate Level

### Zadanie 6: LINQ Operators Chain
Połącz wiele operatorów: Range → Where → Select → Take:

```csharp
Observable.Range(1, 20)
    .Where(x => x % 2 == 0)      // Even numbers
    .Select(x => x * 3)           // Multiply by 3
    .Take(5)                      // First 5
    .Subscribe(x => Console.WriteLine(x));
```

**Wyjście:** 6, 12, 18, 24, 30

---

### Zadanie 7: Combining Observables
Połącz dwa observables za pomocą Merge:

```csharp
var obs1 = Observable.Range(1, 3);
var obs2 = Observable.Range(10, 3);

Observable.Merge(obs1, obs2)
    .Subscribe(x => Console.WriteLine(x));
```

**Wyjście:** 1, 10, 2, 11, 3, 12 (mogą być w różnej kolejności)

---

### Zadanie 8: Interval Timer
Stwórz observable, który emituje wartość co 500ms przez 3 sekundy:

```csharp
Observable.Interval(TimeSpan.FromMilliseconds(500))
    .Take(6)
    .Subscribe(
        x => Console.WriteLine($"Tick: {x}"),
        () => Console.WriteLine("Timer completed")
    );

// Czekaj na zakończenie
System.Threading.Thread.Sleep(3500);
```

---

### Zadanie 9: Distinct Values
Usuń duplikaty z observable:

```csharp
Observable.Create<int>(observer =>
{
    observer.OnNext(1);
    observer.OnNext(1);
    observer.OnNext(2);
    observer.OnNext(2);
    observer.OnNext(3);
    observer.OnCompleted();
    return () => { };
})
.DistinctUntilChanged()
.Subscribe(x => Console.WriteLine(x));
```

**Wyjście:** 1, 2, 3 (duplikaty usunięte)

---

### Zadanie 10: Multiple Subjects
Stwórz wiele subjects i zsynchronizuj je:

```csharp
var nameSubject = new Subject<string>();
var ageSubject = new Subject<int>();

Observable.CombineLatest(nameSubject, ageSubject)
    .Subscribe(pair => 
        Console.WriteLine($"Name: {pair.First}, Age: {pair.Second}")
    );

nameSubject.OnNext("Alice");
ageSubject.OnNext(25);
nameSubject.OnNext("Bob");
```

---

## 🔴 Advanced Level

### Zadanie 11: Reactive Auto-Complete Simulation
Symuluj auto-complete z debounce:

```csharp
var searchSubject = new Subject<string>();

searchSubject
    .Debounce(TimeSpan.FromMilliseconds(500))  // Wait for 500ms pause
    .DistinctUntilChanged()                     // Only unique searches
    .Select(query => SearchAsync(query))        // Async search
    .SelectMany(x => x)                         // Flatten
    .Subscribe(
        results => Console.WriteLine($"Results: {string.Join(", ", results)}"),
        error => Console.WriteLine($"Error: {error}")
    );

// Simulate typing
searchSubject.OnNext("a");
searchSubject.OnNext("ab");
searchSubject.OnNext("abc");

await Task.Delay(600);
```

**Wskazówka:** Debounce czeka na pauzę przed wysłaniem wartości.

---

### Zadanie 12: Retry Pattern
Implementuj retry logic dla observable:

```csharp
int attemptCount = 0;

Observable.Create<string>(observer =>
{
    attemptCount++;
    if (attemptCount < 3)
    {
        observer.OnError(new Exception("Network error"));
    }
    else
    {
        observer.OnNext("Success!");
        observer.OnCompleted();
    }
    return () => { };
})
.Retry(3)
.Subscribe(
    x => Console.WriteLine(x),
    e => Console.WriteLine($"Failed after retries: {e}")
);
```

---

### Zadanie 13: Buffer and Aggregate
Zbuforuj wartości i oblicz sumę każdej grupy:

```csharp
Observable.Range(1, 10)
    .Buffer(3)                              // Group 3 at a time
    .Select(group => group.Sum())           // Sum each group
    .Subscribe(sum => Console.WriteLine($"Group sum: {sum}"));
```

**Wyjście:**
```
Group sum: 6      (1+2+3)
Group sum: 15     (4+5+6)
Group sum: 24     (7+8+9)
Group sum: 10     (10)
```

---

### Zadanie 14: Hot Observable with Share
Konwertuj cold observable na hot z Share():

```csharp
var source = Observable.Range(1, 5)
    .Do(x => Console.WriteLine($"Computing: {x}"))
    .Share();  // Convert to hot

Console.WriteLine("Sub1:");
source.Subscribe(x => Console.WriteLine($"  Sub1: {x}"));

Console.WriteLine("Sub2 (late):");
source.Subscribe(x => Console.WriteLine($"  Sub2: {x}"));
```

**Bez Share():** Oba observerzy dostają własną sekwencję
**Z Share():** Wszyscy observerzy dzielą się wartościami

---

### Zadanie 15: Custom Observable Factory
Stwórz custom observable factory dla async operation:

```csharp
IObservable<string> FetchDataAsync(int delayMs)
{
    return Observable.Create<string>(async observer =>
    {
        try
        {
            await Task.Delay(delayMs);
            observer.OnNext($"Data from {delayMs}ms");
            observer.OnCompleted();
        }
        catch (Exception ex)
        {
            observer.OnError(ex);
        }
    });
}

FetchDataAsync(1000)
    .Retry(2)
    .Subscribe(
        x => Console.WriteLine(x),
        e => Console.WriteLine($"Failed: {e}")
    );
```

---

## 💡 Praktyczne Scenariusze

### Zadanie 16: Search with Cancellation
Zimplementuj searchowanie z możliwością anulacji:

```csharp
var searchSubject = new Subject<string>();
var cts = new System.Threading.CancellationTokenSource();

searchSubject
    .Debounce(TimeSpan.FromMilliseconds(300))
    .TakeUntil(Observable.Never<string>())  // Można anulować
    .Subscribe(
        x => Console.WriteLine($"Searching: {x}"),
        e => Console.WriteLine($"Cancelled or error: {e}")
    );

// Later: cts.Cancel();
```

---

### Zadanie 17: Event Stream Transformation
Konwertuj event (np. button clicks) na observable:

```csharp
// Assuming you have a Form or Button
// var clicks = Observable.FromEvent<EventHandler, EventArgs>(
//     h => button.Click += h,
//     h => button.Click -= h
// );

// clicks
//     .Buffer(TimeSpan.FromSeconds(1))
//     .Select(x => x.Count)
//     .Subscribe(count => Console.WriteLine($"Clicks in 1s: {count}"));
```

---

## 📊 Wskazówki

- ✅ Zawsze dispose subscriptions gdy nie potrzebujesz
- ✅ Używaj `using` lub `Subscribe(...).AddTo(...)` dla cleanup
- ✅ Pamiętaj o różnicy między cold a hot observables
- ✅ Debounce dla delayed user input
- ✅ Throttle dla wysokiej częstotliwości events
- ❌ Nie ignoruj errors - obsługuj OnError
- ❌ Nie tworz memory leaks - unsub/dispose

---

## 🎯 Klucz do Sukcesu

**Reactive Extensions to:**
1. LINQ dla event streams
2. Potężne narzędzie dla async composition
3. Alternatywa dla event handlers
4. Bridge między async/await a streams

**Używaj Rx gdy:**
- Pracujesz z event streamami
- Kombinujesz wielokrotne async operacje
- Musisz time-based logic (throttle, debounce)
- Implementujesz reactive UI

**Łącz z Async/Await:**
```csharp
async Task<T> MyAsyncOp()
async IAsyncEnumerable<T> MyAsyncStream()

// vs

IObservable<T> MyReactiveStream()
```
