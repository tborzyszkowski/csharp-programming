# Ćwiczenia: Benchmarking Performance

## 🟢 Basic Level

### Zadanie 1: Stopwatch Benchmark
Napisz program, który mierzy czas wykonania metody asynchronicznej:

```csharp
async Task<string> FetchDataAsync()
{
    await Task.Delay(500);
    return "Data fetched";
}

// Mierz czas wykonania
```

**Rozwiązanie:**
```csharp
var sw = Stopwatch.StartNew();
var result = await FetchDataAsync();
sw.Stop();
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
```

---

### Zadanie 2: Porównanie Dwóch Metod
Porównaj czas wykonania dwóch prostych metod i wyświetl ile razy jedna jest szybsza od drugiej:

```csharp
void Method1() { Thread.Sleep(100); }
async Task Method2Async() { await Task.Delay(100); }
```

**Rozwiązanie:**
```csharp
var sw1 = Stopwatch.StartNew();
Method1();
sw1.Stop();

var sw2 = Stopwatch.StartNew();
await Method2Async();
sw2.Stop();

double ratio = (double)sw1.ElapsedMilliseconds / sw2.ElapsedMilliseconds;
Console.WriteLine($"Ratio: {ratio:F2}x");
```

---

### Zadanie 3: Benchmark Przemnażania
Zmierz czas potrzebny do wykonania 1000 operacji mnożenia:

```csharp
for (int i = 0; i < 1000; i++)
{
    var result = i * i;
}
```

**Rozwiązanie:**
```csharp
var sw = Stopwatch.StartNew();
for (int i = 0; i < 1000; i++)
{
    var result = i * i;
}
sw.Stop();
Console.WriteLine($"Time: {sw.ElapsedMicroseconds}µs");
```

---

## 🟡 Intermediate Level

### Zadanie 4: File I/O Sync vs Async Benchmark
Porównaj wydajność synchronicznego i asynchronicznego zapisu do 50 plików:

```csharp
// Synchronicznie
for (int i = 0; i < 50; i++)
{
    File.WriteAllText($"file_{i}.txt", $"Content {i}");
}

// Asynchronicznie
var tasks = new Task[50];
for (int i = 0; i < 50; i++)
{
    int index = i;
    tasks[i] = File.WriteAllTextAsync($"file_{index}.txt", $"Content {index}");
}
await Task.WhenAll(tasks);
```

**Wynik:** Async powinno być ~5-10x szybsze!

---

### Zadanie 5: Memory Allocation Tracking
Zmierz ile pamięci zostało przydzielone przed i po wykonaniu operacji:

```csharp
var before = GC.GetTotalMemory(true);

// Twój kod do mierzenia

var after = GC.GetTotalMemory(false);
Console.WriteLine($"Allocated: {after - before} bytes");
```

**Rozwiązanie:**
```csharp
var before = GC.GetTotalMemory(true);

for (int i = 0; i < 1000; i++)
{
    var task = ProcessAsync(i);
}

var after = GC.GetTotalMemory(false);
Console.WriteLine($"Allocated: {(after - before) / 1024}KB");
```

---

### Zadanie 6: Throughput Measurement
Zmierz ile operacji na sekundę możesz wykonać:

```csharp
var sw = Stopwatch.StartNew();
int count = 0;
while (sw.ElapsedMilliseconds < 1000)
{
    await SimulateWorkAsync();
    count++;
}
Console.WriteLine($"Throughput: {count} operations/second");
```

---

## 🔴 Advanced Level

### Zadanie 7: Comprehensive Benchmark Suite
Stwórz kompletny benchmark porównujący:
- File I/O (sync vs async)
- Network calls (sequential vs concurrent)
- CPU work (single vs parallel)

```csharp
class ComprehensiveBenchmark
{
    async Task RunAll()
    {
        var results = new Dictionary<string, long>();
        
        // File I/O benchmark
        results["FileSync"] = await BenchmarkFileSync();
        results["FileAsync"] = await BenchmarkFileAsync();
        
        // Network benchmark
        results["NetworkSeq"] = await BenchmarkNetworkSequential();
        results["NetworkConc"] = await BenchmarkNetworkConcurrent();
        
        // Display results
        foreach (var kvp in results)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}ms");
        }
    }
}
```

---

### Zadanie 8: Statistical Analysis
Wykonaj benchmark 10 razy i oblicz średnią, medianę i odchylenie standardowe:

```csharp
var results = new long[10];
for (int run = 0; run < 10; run++)
{
    var sw = Stopwatch.StartNew();
    await YourMethodAsync();
    sw.Stop();
    results[run] = sw.ElapsedMilliseconds;
}

// Oblicz statystyki
long mean = results.Sum() / results.Length;
long median = results.OrderBy(x => x).ElementAt(5);
double variance = results.Average(x => Math.Pow(x - mean, 2));
double stdDev = Math.Sqrt(variance);

Console.WriteLine($"Mean: {mean}ms");
Console.WriteLine($"Median: {median}ms");
Console.WriteLine($"StdDev: {stdDev:F2}ms");
```

---

### Zadanie 9: GC Pressure Analysis
Porównaj ilość garbage collection między podejściem sync a async:

```csharp
int gcCountBefore = GC.CollectionCount(0);

// Twoja operacja

int gcCountAfter = GC.CollectionCount(0);
Console.WriteLine($"GC runs: {gcCountAfter - gcCountBefore}");
```

**Wskazówka:** Async operacje mogą wyzwalać więcej GC ze względu na alokacje Task obiektów.

---

### Zadanie 10: BenchmarkDotNet Integration (Bonus)
Zainstaluj BenchmarkDotNet i zrefaktoryzuj jeden z benchmarków:

```bash
dotnet add package BenchmarkDotNet
```

```csharp
[MemoryDiagnoser]
public class MyBenchmark
{
    [Benchmark]
    public async Task AsyncMethod()
    {
        await Task.Delay(100);
    }

    [Benchmark]
    public void SyncMethod()
    {
        Thread.Sleep(100);
    }
}

// Uruchom: dotnet run -c Release
```

---

## 📊 Wskazówki

- ✅ Zawsze uruchamiaj benchmarki w **Release mode** (`-c Release`)
- ✅ **Wyłącz background processes** przed pomiarem
- ✅ Uruchom benchmark **wielokrotnie** (10+ razy) dla dokładności
- ✅ Patrz na **średnią i odchylenie standardowe**, nie na jedno wykonanie
- ✅ Różnice poniżej **10%** mogą być w szumie pomiarowym
- ❌ Nie porównuj wyników z **różnych maszyn**
- ❌ Nie mierz za **krótkich operacji** (<100μs)

---

## 🎯 Klucz do Sukcesu

**Benchmarking to nauka, nie sztuka:**
1. Postaw hipotezę (async powinno być 5x szybsze)
2. Napisz benchmark
3. Zbierz dane
4. Analizuj wyniki
5. Wyciągnij wnioski
6. Iterate jeśli potrzeba

**Pamiętaj:** Liczą się rzeczywiste pomiary, nie domysły!
