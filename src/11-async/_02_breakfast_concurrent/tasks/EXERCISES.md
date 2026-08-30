# Ćwiczenia: Breakfast Sequential vs Concurrent

## 🟢 Basic Level

### 1. Sequential vs Concurrent Timing
Porównaj czasy wykonania:

```csharp
// Task: CookAsync() - czeka timeMs i zwraca "Done"
public async Task MeasureSequential()
{
    var start = DateTime.Now;
    await CookAsync(1000);
    await CookAsync(1000);
    await CookAsync(1000);
    Console.WriteLine($"Sequential: {(DateTime.Now - start).TotalSeconds}s");
}

public async Task MeasureConcurrent()
{
    var start = DateTime.Now;
    var t1 = CookAsync(1000);
    var t2 = CookAsync(1000);
    var t3 = CookAsync(1000);
    await Task.WhenAll(t1, t2, t3);
    Console.WriteLine($"Concurrent: {(DateTime.Now - start).TotalSeconds}s");
}
```

**Wynik:** Concurrent powinien być ~3x szybszy (1s vs 3s)

---

### 2. Simple Breakfast
Symuluj przygotowanie:
1. Jajka - 2s
2. Chleb - 1s

Uruchom równocześnie.

```csharp
public async Task SimpleConcurrentBreakfast()
{
    var eggs = PrepareAsync("Eggs", 2000);
    var bread = PrepareAsync("Bread", 1000);
    
    await Task.WhenAll(eggs, bread);
    
    Console.WriteLine($"Breakfast ready: {eggs.Result}, {bread.Result}");
}
```

---

## 🟡 Intermediate Level

### 3. Full Breakfast Service
Zaimplementuj klasę `BreakfastService`:

```csharp
public class BreakfastService
{
    public async Task<string> PrepareAsync()
    {
        // Równocześnie:
        // - Jajka (2500ms)
        // - Chleb (1500ms)
        // - Kawa (1000ms)
        // - Bekon (2000ms)
        
        // Zmierz czas i zwróć string z wynikami
    }
}

// Oczekiwany czas: ~2500ms (najdłuższa operacja - jajka)
```

**Wymagania:**
- Wszystkie operacje uruchomić równocześnie
- Zmierzyć całkowity czas
- Zwrócić string z wszystkimi itemami

---

### 4. Error Handling in Concurrent Tasks
Dodaj obsługę błędów:

```csharp
public async Task PrepareWithErrorsAsync()
{
    try
    {
        var eggs = CookEggsAsync(2000);      // OK
        var toast = ToastBreadAsync(-1);     // Błąd! ujemny czas
        var coffee = BrewCoffeeAsync(1000);  // OK
        
        await Task.WhenAll(eggs, toast, coffee);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}
```

**Wymagania:**
- Zbadaj co się dzieje gdy jeden task wyrzuci exception
- Jak obsługić?

---

### 5. Progressive Completion (WhenAny)
Wyświetlaj rezultaty w miarę jak się kończą:

```csharp
public async Task PrepareWithProgressAsync()
{
    var tasks = new[]
    {
        CookAsync("Eggs", 3000),
        CookAsync("Bread", 1500),
        CookAsync("Coffee", 1000)
    };
    
    var remaining = tasks.ToList();
    
    while (remaining.Count > 0)
    {
        var completed = await Task.WhenAny(remaining);
        Console.WriteLine($"Ready: {completed.Result}");
        remaining.Remove(completed);
    }
}
```

---

## 🔴 Advanced Level

### 6. Performance Comparison Tool
Stwórz tool który porównuje wydajność:

```csharp
public async Task ComparePerformanceAsync(int taskCount, int timePerTask)
{
    // 1. Zmierz Sequential
    var seqStart = DateTime.Now;
    for (int i = 0; i < taskCount; i++)
        await DelayAsync(timePerTask);
    var seqTime = (DateTime.Now - seqStart).TotalSeconds;
    
    // 2. Zmierz Concurrent
    var concStart = DateTime.Now;
    var tasks = Enumerable.Range(0, taskCount)
        .Select(_ => DelayAsync(timePerTask))
        .ToList();
    await Task.WhenAll(tasks);
    var concTime = (DateTime.Now - concStart).TotalSeconds;
    
    // 3. Pokaż rezultaty
    Console.WriteLine($"Sequential: {seqTime:F2}s");
    Console.WriteLine($"Concurrent: {concTime:F2}s");
    Console.WriteLine($"Speedup: {seqTime/concTime:F2}x");
}
```

---

### 7. Breakfast with Constraints
Dodaj ograniczenia:
- Piekarnik może robić jajka ALBO chleb (nie oba naraz)
- Kawa/bekon mogą być równocześnie

```csharp
public async Task BreakfastWithConstraintsAsync()
{
    // Jajka i chleb sekwencyjnie (jeden piekarnik)
    var eggs = CookAsync("Eggs", 2000);
    await eggs;
    var bread = await CookAsync("Bread", 1500);
    
    // Ale kawa i bekon równocześnie
    var coffee = CookAsync("Coffee", 1000);
    var bacon = CookAsync("Bacon", 2000);
    await Task.WhenAll(coffee, bacon);
}

// Oczekiwany czas: 2000 + 1500 + 2000 = 5500ms
```

---

### 8. Challenge: Meal Preparation Pipeline
Zaimplementuj pełny system:

```csharp
public class MealPrepService
{
    public async Task<Meal> PrepareMealAsync(string type)
    {
        // Dla "breakfast":
        //   1. Przygotuj jajka (2s) → concurrent: kawa (1s) + bekon (1.5s)
        //   2. Nag. talerz (0.5s)
        //   3. Ułóż na talerzu (0.5s)
        
        // Dla "lunch":
        //   1. Przygotuj mięso (3s) + warzywa (1s) concurrent
        //   2. Gotuj ryż (2s)
        //   3. Nag. talerz (0.5s)
    }
}
```

**Wymagania:**
- Różne sequencje dla różnych posiłków
- Krok 1 możliwe są operacje concurrent
- Kroki muszą być sekwencyjne (poczekaj na krok 1 zanim przejdziesz do 2)
- Zwróć `Meal` z czasem przygotowania

---

## ✅ Rozwiązania

### Temat 1: Sequential vs Concurrent

**Sequential:** ~3 sekundy
```csharp
await Task.Delay(1000);  // 1s
await Task.Delay(1000);  // 2s total
await Task.Delay(1000);  // 3s total
```

**Concurrent:** ~1 sekunda
```csharp
var t1 = Task.Delay(1000);
var t2 = Task.Delay(1000);
var t3 = Task.Delay(1000);
await Task.WhenAll(t1, t2, t3);  // 1s max
```

### Temat 4: Error Handling

**Important:** Jeśli jakikolwiek task w `Task.WhenAll` rzuci exception, cały `WhenAll` rzuci AggregateException:

```csharp
try
{
    await Task.WhenAll(task1, task2, task3);
}
catch (Exception ex)
{
    // ex.Message = ostatni exception lub AggregateException
}
```

### Temat 5: Progressive Completion

Używamy pętli z `Task.WhenAny` aby wyświetlić rezultaty w miarę jak się kończą, zamiast czekać na wszystkie naraz.

---

## 📚 Dodatkowe Zasoby

- [Async Best Practices](https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
- [Task Parallel Library](https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl)
