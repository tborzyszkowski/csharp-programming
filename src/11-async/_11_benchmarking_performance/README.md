# Temat 11: Benchmarking Performance (Async vs Sync Metrics)

## 📚 Koncepcja

Benchmarking to systematyczne mierzenie i porównanie wydajności kodu. W kontekście programowania asynchronicznego, benchmarking pozwala nam:

- **Porównać rzeczywisty zysk wydajności** async vs sync
- **Zmierzyć thread pool utilization** i resource usage
- **Identyfikować bottlenecks** w aplikacji
- **Optymalizować kod** w oparciu o dane
- **Walidować założenia** o wydajności

---

## 🎯 Kluczowe Koncepty

### 1. **BenchmarkDotNet**
```csharp
[MemoryDiagnoser]
public class AsyncVsSyncBenchmark
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
```

### 2. **Pomiary Czasowe**
- **Mean (średnia)** - średni czas wykonania
- **Median (mediana)** - środkowa wartość
- **Min/Max** - najszybsze i najwolniejsze wykonania
- **StdDev (odchylenie standardowe)** - konsystencja wyników

### 3. **Metryki Pamięci**
- **Allocated** - ile bajty pamięci zostało przydzielone
- **Gen0/Gen1/Gen2 collections** - ile razy odśmiecacz pracował

### 4. **Thread Pool Metrics**
- **Thread count** - ile threadów było aktywnych
- **Context switches** - ile razy system przełączał kontekst
- **I/O Completion Ports** - w Windows dla async I/O

---

## 💡 Praktyczne Zastosowanie

### Problem: Czy Naprawdę Async Jest Szybsze?

```csharp
// ❌ Fałszywa teza
"Async zawsze szybsze niż sync"

// ✅ Prawda
"Async efektywniej wykorzystuje zasoby w scenariuszach I/O-bound"
```

### Scenariusze Benchmarku

1. **I/O-Bound:** File I/O, Network requests
2. **CPU-Bound:** Calculations, Data processing
3. **Mixed:** Combination of I/O and CPU work

---

## 📊 Wyniki Benchmarkowania

### Przykład 1: File I/O
```
Benchmark           Mean        StdDev      Ratio
Sync 10 files       1500 ms     50 ms       1.00
Async 10 files      150 ms      10 ms       0.10  ✅ 10x faster!
```

### Przykład 2: Network Requests
```
Benchmark           Mean        StdDev      Ratio
Sync 5 URLs         5000 ms     200 ms      1.00
Async 5 URLs        1000 ms     100 ms      0.20  ✅ 5x faster!
```

### Przykład 3: CPU-Bound Work
```
Benchmark           Mean        StdDev      Ratio
Sync Calculate      500 ms      10 ms       1.00
Async Calculate     500 ms      10 ms       1.00  ⚠️ Same speed
```

> **Wniosek:** Async nie przyspieszył CPU-bound operacji, ale pozwolił wykonywać je bez blokowania!

---

## 🔍 Metryki Do Mierzenia

### 1. **Throughput (Przepustowość)**
- Ile operacji na sekundę
- Liczba żądań obsługiwanych równocześnie

### 2. **Latency (Opóźnienie)**
- Czas od wysłania żądania do otrzymania odpowiedzi
- Percentyle: P50, P95, P99

### 3. **Resource Utilization**
- CPU usage %
- Memory usage MB
- Thread count

### 4. **Allocation Pressure**
- Bytes allocated per operation
- Garbage collection pauses

---

## 🛠️ Narzędzia Benchmarkowania

### BenchmarkDotNet
```csharp
dotnet add package BenchmarkDotNet
```

### Stopwatch (Manual)
```csharp
var sw = Stopwatch.StartNew();
// kod
sw.Stop();
Console.WriteLine($"Time: {sw.ElapsedMilliseconds}ms");
```

### dotTrace (JetBrains)
```
Profiling tool dla .NET
```

### PerfView (Microsoft)
```
ETW tracing tool
```

---

## 📈 Interpretacja Wyników

### Good Results
- ✅ Consistent results (niskie StdDev)
- ✅ Significant differences (>10% to consider valid)
- ✅ Memory allocations minimized
- ✅ No GC pressure

### Watch Out
- ⚠️ High variance (niestabilne wyniki)
- ⚠️ JIT compilation not warmed up
- ⚠️ Interference from background tasks
- ⚠️ Too short benchmark duration

---

## 🎓 Workflow Benchmarkowania

```
1. Zidentyfikuj obszar do optymalizacji
   ↓
2. Napisz baseline benchmark
   ↓
3. Uruchom i zapisz wyniki
   ↓
4. Implementuj optymalizację
   ↓
5. Uruchom benchmark ponownie
   ↓
6. Porównaj wyniki
   ↓
7. Jeśli dobrze → commit, Jeśli źle → iterate
```

---

## 📚 Referencje

- [BenchmarkDotNet](https://benchmarkdotnet.org/)
- [System.Diagnostics.Stopwatch](https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.stopwatch)
- [Performance Best Practices](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/performance-best-practices)
- [Async Performance](https://learn.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming)

---

## ✅ Summary

**Benchmarking performance** to kluczowa praktyka w optymalizacji:
- Mierz rzeczywiste liczby, nie domysły
- Porównuj async vs sync w kontekście twojego problemu
- Pamiętaj: async nie zawsze szybsze, ale zawsze lepiej wykorzystuje zasoby
- Unikaj przedwczesnej optymalizacji - najpierw benchmark, potem optimize

**Następny temat:** Reactive Extensions (IObservable patterns)
