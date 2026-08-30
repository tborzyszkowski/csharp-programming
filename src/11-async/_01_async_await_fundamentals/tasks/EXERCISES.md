# Ćwiczenia: Async/Await Fundamentals

## 🟢 Basic Level

### 1. Convert Sync to Async
Skonwertuj poniższą synchroniczną funkcję na asynchroniczną:

```csharp
public static int FetchData(int delayMs)
{
    System.Threading.Thread.Sleep(delayMs);
    return 42;
}
```

**Wymagania:**
- Funkcja powinna zwracać `Task<int>`
- Użyj `async` i `await`
- Zamień `Thread.Sleep()` na `Task.Delay()`

**Rozwiązanie:**
```csharp
public static async Task<int> FetchDataAsync(int delayMs)
{
    await Task.Delay(delayMs);
    return 42;
}
```

---

### 2. Call Async Method
Napisz funkcję która:
1. Wołuje `FetchDataAsync(1000)` bez blokowania
2. Wypisuje wynik

**Rozwiązanie:**
```csharp
public static async Task MainAsync()
{
    var data = await FetchDataAsync(1000);
    Console.WriteLine($"Data: {data}");
}
```

---

### 3. Task.WhenAll with 2 Tasks
Napisz kod który równocześnie:
1. Czeka 1000ms i zwraca "A"
2. Czeka 1500ms i zwraca "B"

Zmierz łączny czas (powinno być ~1500ms, nie 2500ms)

**Rozwiązanie:**
```csharp
public static async Task Main()
{
    var start = DateTime.Now;
    
    var task1 = Task.Delay(1000).ContinueWith(_ => "A");
    var task2 = Task.Delay(1500).ContinueWith(_ => "B");
    
    var results = await Task.WhenAll(task1, task2);
    
    var duration = (DateTime.Now - start).TotalSeconds;
    Console.WriteLine($"Results: {string.Join(", ", results)}");
    Console.WriteLine($"Duration: {duration:F1}s");  // Should be ~1.5s
}
```

---

## 🟡 Intermediate Level

### 4. Multiple Async Operations
Napisz metodę:
- `GetUserAsync(id)` - czeka 1000ms, zwraca username
- `GetPostsAsync(userId)` - czeka 800ms, zwraca liczba postów

Wywołaj obie operacje równocześnie i wyświetl wyniki.

**Rozwiązanie:**
```csharp
public static async Task<string> GetUserAsync(int id)
{
    await Task.Delay(1000);
    return $"User{id}";
}

public static async Task<int> GetPostsAsync(string userId)
{
    await Task.Delay(800);
    return 5;
}

public static async Task Main()
{
    var userTask = GetUserAsync(1);
    var postsTask = GetPostsAsync("User1");
    
    await Task.WhenAll(userTask, postsTask);
    
    Console.WriteLine($"User: {userTask.Result}");
    Console.WriteLine($"Posts: {postsTask.Result}");
}
```

---

### 5. Exception Handling
Napisz metodę `DivideAsync(a, b)` która:
- Czeka 500ms
- Rzuca `ArgumentException` jeśli b == 0
- Zwraca wynik dzielenia

Obsługaj exception w try-catch

**Rozwiązanie:**
```csharp
public static async Task<int> DivideAsync(int a, int b)
{
    await Task.Delay(500);
    if (b == 0)
        throw new ArgumentException("Cannot divide by zero");
    return a / b;
}

public static async Task Main()
{
    try
    {
        var result = await DivideAsync(10, 0);
        Console.WriteLine($"Result: {result}");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

---

### 6. Task.WhenAny Race
Napisz kod który symuluje pobieranie danych z 3 źródeł:
- Server1: 3000ms
- Server2: 1000ms
- Server3: 2000ms

Wyświetl który server odpowiedział pierwszy (powinien być Server2)

**Rozwiązanie:**
```csharp
public static async Task<string> FetchAsync(string server, int delayMs)
{
    await Task.Delay(delayMs);
    return $"Data from {server}";
}

public static async Task Main()
{
    var s1 = FetchAsync("Server1", 3000);
    var s2 = FetchAsync("Server2", 1000);
    var s3 = FetchAsync("Server3", 2000);
    
    var winner = await Task.WhenAny(s1, s2, s3);
    Console.WriteLine($"Fastest: {winner.Result}");
}
```

---

## 🔴 Advanced Level

### 7. Timeout with Task.WhenAny
Implementuj timeout dla async operacji używając `Task.WhenAny`:

```csharp
public static async Task<string> FetchDataWithTimeoutAsync(
    Func<Task<string>> operation, 
    int timeoutMs)
{
    var timeoutTask = Task.Delay(timeoutMs)
        .ContinueWith<string>(_ => throw new TimeoutException());
    
    var resultTask = operation();
    
    var completed = await Task.WhenAny(resultTask, timeoutTask);
    
    if (completed == timeoutTask)
        throw new TimeoutException($"Operation timed out after {timeoutMs}ms");
    
    return resultTask.Result;
}
```

**Użycie:**
```csharp
try
{
    var data = await FetchDataWithTimeoutAsync(
        () => SlowOperationAsync(), 
        2000
    );
    Console.WriteLine($"Data: {data}");
}
catch (TimeoutException)
{
    Console.WriteLine("Operation timed out!");
}
```

---

### 8. Sequential vs Concurrent
Porównaj czas:

```csharp
// Sequential (nie efektywnie)
public static async Task SequentialAsync()
{
    var r1 = await GetDataAsync(1000);
    var r2 = await GetDataAsync(1000);
    var r3 = await GetDataAsync(1000);
    // Total: ~3000ms
}

// Concurrent (efektywnie)
public static async Task ConcurrentAsync()
{
    var t1 = GetDataAsync(1000);
    var t2 = GetDataAsync(1000);
    var t3 = GetDataAsync(1000);
    await Task.WhenAll(t1, t2, t3);
    // Total: ~1000ms
}
```

---

### 9. Chain Multiple Async Operations
Napisz kod który:
1. Pobiera ID użytkownika (500ms)
2. Następnie pobiera dane użytkownika (500ms)
3. Następnie pobiera postów użytkownika (500ms)

**Rozwiązanie:**
```csharp
public static async Task<int> GetUserIdAsync()
{
    await Task.Delay(500);
    return 1;
}

public static async Task<string> GetUserDataAsync(int id)
{
    await Task.Delay(500);
    return $"User{id}Name";
}

public static async Task<int> GetPostCountAsync(int userId)
{
    await Task.Delay(500);
    return 5;
}

public static async Task Main()
{
    var userId = await GetUserIdAsync();
    var userData = await GetUserDataAsync(userId);
    var postCount = await GetPostCountAsync(userId);
    
    Console.WriteLine($"{userData}: {postCount} posts");
    // Total: ~1500ms (sequential)
}
```

---

### 10. Challenge: Complex Async Workflow
Stwórz system "Restauracja":

```csharp
public async Task<string> OrderMealAsync(string meal)
{
    // 1. Sprawdź czy jest dostępne (500ms)
    var available = await CheckAvailabilityAsync(meal);
    if (!available)
        throw new InvalidOperationException("Not available");
    
    // 2. Przygotuj jedzenie (2000ms)
    var prepared = await PrepareAsync(meal);
    
    // 3. Zapakuj (500ms)
    var packaged = await PackAsync(prepared);
    
    return $"Ready: {packaged}";
}
```

**Wymagania:**
- 3 asynchroniczne operacje
- Error handling dla niedostępnych potraw
- Zmierz całkowity czas

---

## ✅ Rozwiązania i Wyjaśnienia

### Temat 1: Konwersja do Async

**Kluczowe zmiany:**
- `public static` → `public static async Task<>`
- `return value` → `return value;` (TAP zwraca Task)
- `Thread.Sleep()` → `Task.Delay()`
- Caller musi używać `await`

### Temat 3: Task.WhenAll Timing

**Ważne:** `Task.WhenAll` uruchamia taskami **równocześnie**, nie sekwencyjnie!
- Task1: 1000ms
- Task2: 1500ms ← najdłuższy
- Task3: 800ms

**Razem:** ~1500ms (nie 3300ms)

### Temat 7: Timeout Trick

Używamy `Task.WhenAny` z `Task.Delay` aby zmierzyć timeout:
```csharp
var winner = await Task.WhenAny(operation, timeoutTask);
if (winner == timeoutTask) // Timeout wygrał!
    throw new TimeoutException();
```

---

## 📚 Dodatkowe Zasoby

- [Microsoft: Async/Await Patterns](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [Task-based Async Pattern](https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap)
- [Async Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
