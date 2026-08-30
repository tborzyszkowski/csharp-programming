# Temat 2: Breakfast - Sequential vs Concurrent 🍳

## Wprowadzenie

Klasyczny przykład: przygotowanie śniadania sekwencyjnie vs asynchronicznie. Pokazuje różnicę wydajności między czekaniem na każdy krok a równoczesnym wykonywaniem niezależnych zadań.

---

## 📖 Scenariusz

**Przygotowanie Śniadania:**
1. Ugotuj jajka (3000ms)
2. Upiecz chleb (2000ms)
3. Zrób kawę (1500ms)
4. Pokrój bekon (1000ms)

### Sekwencyjnie
```
Jajka [====3000ms====]
  └─ Chleb [==2000ms==]
       └─ Kawa [=1500ms=]
            └─ Bekon [1000ms]
            
RAZEM: 7500ms ⏱️
```

### Asynchronicznie (Concurrent)
```
Jajka [====3000ms====]
Chleb [==2000ms==]
Kawa  [=1500ms=]
Bekon [1000ms]

RAZEM: 3000ms ⏱️ (najdłuższa operacja)
```

---

## 💻 Implementacja

### Sekwencyjnie (❌ Nieefektywnie)

```csharp
public async Task PrepareBreakfastSequentialAsync()
{
    var eggs = await CookEggsAsync(3000);      // czeka 3s
    var bread = await ToastBreadAsync(2000);   // czeka 2s
    var coffee = await BrewCoffeeAsync(1500);  // czeka 1.5s
    var bacon = await CookBaconAsync(1000);    // czeka 1s
    
    return $"Breakfast: {eggs}, {bread}, {coffee}, {bacon}";
    // RAZEM: 7500ms ❌
}
```

### Asynchronicznie (✅ Efektywnie)

```csharp
public async Task<string> PrepareBreakfastConcurrentAsync()
{
    // Uruchom wszystkie równocześnie!
    var eggsTask = CookEggsAsync(3000);
    var breadTask = ToastBreadAsync(2000);
    var coffeeTask = BrewCoffeeAsync(1500);
    var baconTask = CookBaconAsync(1000);
    
    // Czekaj na wszystkie
    await Task.WhenAll(eggsTask, breadTask, coffeeTask, baconTask);
    
    return $"Breakfast: {eggsTask.Result}, {breadTask.Result}, {coffeeTask.Result}, {baconTask.Result}";
    // RAZEM: 3000ms ✅ (tylko najdłuższa operacja)
}
```

---

## 🎯 Kluczowe Intuicje

**Sekwencyjnie (Sequential):**
- Każdy krok czeka na poprzedni
- Brakuje paralelizmu
- Całkowity czas = suma wszystkich czasów

**Asynchronicznie (Concurrent):**
- Wszystkie kroki uruchamiają się natychmiast
- Pracują równocześnie (ale na tym samym threada, IO bound)
- Całkowity czas = max(czasy operacji)

---

## 🔗 Referencje

- [Async/Await Best Practices](https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)
- [Task.WhenAll](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.whenall)
