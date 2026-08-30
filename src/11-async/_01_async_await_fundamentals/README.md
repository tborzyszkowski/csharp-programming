# Temat 1: Async/Await Fundamentals 🚀

## Wprowadzenie

Programowanie asynchroniczne w C# pozwala na wykonywanie operacji bez blokowania głównego threada. Zamiast czekać, główny thread może pracować nad innymi zadaniami.

---

## 📖 Kluczowe Koncepty

### 1. TAP (Task-based Asynchronous Pattern)

**TAP** to wzorzec asynchroniczny oparty na klasach `Task` i `Task<T>`:

```csharp
// Zwraca Task (bez wyniku)
public async Task DoWorkAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Praca zakończona!");
}

// Zwraca Task<T> (z wynikiem typu T)
public async Task<int> GetNumberAsync()
{
    await Task.Delay(1000);
    return 42;
}
```

**Zasady TAP:**
- Metody asynchroniczne zwracają `Task` lub `Task<T>`
- Konwencja: suffix `-Async` (`GetDataAsync`, `DoWorkAsync`)
- Muszą zawierać co najmniej jedno `await`

---

### 2. Task i Task<T>

**Task** - reprezentuje asynchroniczną operację:

```csharp
// Task - operacja bez wyniku
Task task = DoWorkAsync();
await task;

// Task<T> - operacja z wynikiem
Task<int> taskWithResult = GetNumberAsync();
int result = await taskWithResult;

// Alternatywnie (krócej)
int result = await GetNumberAsync();
```

**Stany Task'a:**
- `Created` - nowy, nie uruchomiony
- `Running` - aktualnie wykonywany
- `Completed` - zakończony sukcesem
- `Faulted` - rzucił exception
- `Canceled` - został anulowany

---

### 3. Async i Await Keywords

**`async`** - Oznacza, że funkcja może zawierać `await`:

```csharp
// Bez async - zwykła synchroniczna funkcja
public void DoWork() { }

// Z async - może zawierać await
public async Task DoWorkAsync()
{
    // await jest dozwolony tutaj
    await Task.Delay(1000);
}
```

**`await`** - Czeka na Task bez blokowania threada:

```csharp
// ❌ Blokuje thread (synchronicznie czeka)
int result = GetNumberAsync().Result;
Thread.Sleep(1000);

// ✅ Nie blokuje thread (asynchronicznie czeka)
int result = await GetNumberAsync();
```

---

### 4. Async/Await Flow

```csharp
async Task Example()
{
    Console.WriteLine("1. Start"); // Uruchamia się synchronicznie
    
    var data = await GetDataAsync(); // Thread zostaje zwolniony tutaj
    
    Console.WriteLine("2. Data: " + data); // Uruchamia się gdy Task się skończy
}
```

**Co się dzieje:**
1. Kod uruchamia się synchronicznie aż do `await`
2. Na `await`: thread jest zwolniony (może robić inne rzeczy)
3. Gdy Task się skończy: continuation (reszta kodu) wykonuje się

---

### 5. Composition: Task.WhenAll i Task.WhenAny

**Task.WhenAll** - czeka aż WSZYSTKIE taskami się skończą:

```csharp
var task1 = GetDataAsync();
var task2 = GetDataAsync();
var task3 = GetDataAsync();

// Czeka aż wszystkie się skończą
await Task.WhenAll(task1, task2, task3);
```

**Task.WhenAny** - czeka aż JEDEN task się skończy:

```csharp
var task1 = DownloadAsync("url1");
var task2 = DownloadAsync("url2");

// Czeka na pierwszy
var firstCompleted = await Task.WhenAny(task1, task2);
```

---

### 6. Exception Handling

```csharp
async Task SafeDoWorkAsync()
{
    try
    {
        await RiskyOperationAsync();
    }
    catch (IOException ex)
    {
        Console.WriteLine($"IO Error: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Cleanup");
    }
}
```

---

### 7. Fire-and-Forget (UNIKAJ!)

```csharp
// ❌ ZRÓB NIE: Fire-and-forget (gubimy exception!)
_ = DoWorkAsync();

// ✅ DOBRZE: Przynajmniej obsługuj exception
_ = DoWorkAsync().ContinueWith(t => 
{
    if (t.IsFaulted)
        Console.WriteLine($"Error: {t.Exception}");
});
```

---

## 💻 Praktyczne Przykłady

### Przykład 1: Synchronicznie vs Asynchronicznie

**Synchronicznie (blokuje thread):**
```csharp
Console.WriteLine("Start");
Thread.Sleep(2000);  // Blokuje na 2 sekundy!
Console.WriteLine("After 2s");
```

**Asynchronicznie (nie blokuje thread):**
```csharp
Console.WriteLine("Start");
await Task.Delay(2000);  // Thread jest wolny!
Console.WriteLine("After 2s");
```

### Przykład 2: Task.WhenAll

```csharp
async Task FetchMultipleDataAsync()
{
    var task1 = FetchAsync("data1");
    var task2 = FetchAsync("data2");
    var task3 = FetchAsync("data3");
    
    // Wszystkie 3 działają równocześnie!
    await Task.WhenAll(task1, task2, task3);
    
    Console.WriteLine("All done!");
}
```

### Przykład 3: ConfigureAwait

```csharp
// W bibliotekach (non-UI code)
await GetDataAsync().ConfigureAwait(false);

// Zmniejsza context switching overhead
// Bezpieczne w library code
```

---

## 🔗 Referencje

- [Async/Await - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [Task-based Asynchronous Pattern](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)
- [async/await keywords](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/async)
- [Task API](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task)

---

## 📚 Słownik Pojęć

| Termin | Wyjaśnienie |
|--------|-------------|
| **TAP** | Task-based Asynchronous Pattern |
| **Task** | Reprezentacja asynchronicznej operacji |
| **async** | Keyword: funkcja może zawierać await |
| **await** | Keyword: czeka na Task bez blokowania |
| **Continuation** | Kod po `await` (uruchamia się po skończeniu Task) |
| **Thread Pool** | Pula gotowych threadów dla operacji |
| **Context** | SynchronizationContext (UI, ASP.NET) |
| **ConfigureAwait** | Optymalizacja context switching |

---

**Następnie:** [Temat 2: Breakfast Example](../_02_breakfast_concurrent)
