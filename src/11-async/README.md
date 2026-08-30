# 11. Programowanie Asynchroniczne w C# 🚀

**Wersja:** 1.0  
**.NET:** 9.0+  
**C#:** 13 (latest)

---

## 📌 Przegląd Modułu

Kompleksowy kurs programowania asynchronicznego w C# obejmujący TAP (Task-based Asynchronous Pattern), async/await, obsługę I/O i CPU-bound operacji, oraz nowoczesne wzorce z C# 8+ (async streams, channels, ValueTask).

## 🎯 12 Tematów Nauki

| # | Temat | Focus | Czas |
|---|-------|-------|------|
| 1️⃣ | **Async/Await Fundamentals** | TAP, Task, Task<T>, async/await | 60 min |
| 2️⃣ | **Breakfast: Sequential vs Concurrent** | Współbieżność, Task.WhenAll | 45 min |
| 3️⃣ | **I/O Operations** | HttpClient, File I/O, dlaczego async | 50 min |
| 4️⃣ | **CPU-Bound Operations** | Task.Run, Thread Pool, deadlocks | 40 min |
| 5️⃣ | **Advanced Patterns & Deadlocks** | CancellationToken, ConfigureAwait | 55 min |
| 6️⃣ | **Testing Async Code** | xUnit, async tests, mocking | 45 min |
| 7️⃣ | **Async LINQ & Channels** | IAsyncEnumerable, Channels | 50 min |
| 8️⃣ | **Dependency Injection** | Async init, Lazy<Task<T>> | 40 min |
| 9️⃣ | **Real-World Integration** | Kompleksowy system | 60 min |
| 🔟 | **Modern C# Features** | ValueTask, async iterators | 40 min |
| 1️⃣1️⃣ | **Benchmarking Performance** | Async vs Sync metrics, Stopwatch | 45 min |
| 1️⃣2️⃣ | **Reactive Extensions (Rx)** | IObservable, Subjects, Event streams | 50 min |

**Razem: ~520 minut (~8.5 godzin nauki)**

---

## 🗺️ Ścieżka Nauki

```
Temat 1: Fundamentals
    ↓
Temat 2: Breakfast Example
    ├──────────────────────┐
    ↓                      ↓
Temat 3: I/O        Temat 4: CPU-Bound
    ├──────────────────────┤
    ↓
Temat 5: Advanced Patterns
    ├──────────────────────┐
    ↓                      ↓
Temat 6: Testing     Temat 7: Async LINQ
    └──────────────────────┤
                           ↓
                    Temat 8: DI + Async
                           ↓
                    Temat 9: Real-World
                           ↓
                    Temat 10: Modern Features
                           ↓
        ┌──────────────────────────────┐
        ↓                              ↓
    Temat 11:              Temat 12:
    Benchmarking           Reactive Extensions
        ↓                      ↓
        └──────────────────────┘
                    ↓
        ✓ MASTER Async Programming + Performance
```

---

## 📚 Wymagania Wstępne

- ✅ [Klasy (01-klasy)](../01-klasy)
- ✅ [Dziedziczenie (05-dziedziczenie)](../05-dziedziczenie)
- ✅ [Interfejsy (07-interfejsy_abstrakcje)](../07-interfejsy_abstrakcje)
- ✅ [Delegaty & Zdarzenia](../09-delegacje_zdarzenia) (przydatne)
- ✅ Podstawy: loops, exception handling, LINQ

---

## 🚀 Szybki Start

### 1️⃣ Klon i Setup

```bash
cd src/11-async
```

### 2️⃣ Wybierz Temat

```bash
# Temat 1: Fundamentals
cd _01_async_await_fundamentals
cat README.md
dotnet run --project code/Program.csproj
```

### 3️⃣ Czytaj, Uruchamiaj, Praktykuj

- 📖 Przeczytaj README każdego tematu
- ▶️ Uruchom `dotnet run`
- 📝 Analizuj kod
- 💪 Rozwiąż ćwiczenia (🟢 Basic / 🟡 Intermediate / 🔴 Advanced)

---

## 🎓 Dla Których To Jest

- 👨‍💻 **Dla Studentów:** Pełna edukacja - od teorii do praktyki
- 🏫 **Dla Wykładowców:** Gotowe materiały do wykładów
- 🔧 **Dla Praktyk:** Real-world wzorce do adaptacji
- 📚 **Dla Wszystkich:** Best practices i design patterns

---

## 📋 Zawartość Każdego Tematu

Każdy temat zawiera:

```
_XX_topic_name/
├── README.md              ← Szczegółowa teoria + koncepty
├── code/
│   ├── Program.cs         ← 3-5 praktycznych przykładów
│   └── Program.csproj     ← .NET 9.0 konfiguracja
├── diagrams/
│   └── diagrams.md        ← Mermaid: flowcharts, sequences
└── tasks/
    └── EXERCISES.md       ← 3-level exercises
```

### README.md - Zawiera:
- 📖 **Koncepty**: Wyjaśnienia kluczowych idei
- 💻 **Kod**: Snippety z objaśnieniami
- 🔗 **Diagramy**: Mermaid flowcharts/sequences
- 🔍 **Referencje**: Linki do Microsoft Docs
- 📚 **Przykłady**: 2-5 praktycznych use cases

### Program.cs - Zawiera:
- ✅ 3-5 kompletnych, uruchomialnych przykładów
- ✅ Output w konsoli (formatowanie Unicode)
- ✅ Łatwe do debugowania
- ✅ Pokazuje best practices

### diagrams/diagrams.md - Zawiera:
- 🔀 **Flowcharts**: Przebieg logiki
- 📊 **Sequence Diagrams**: Kolejność operacji
- 🏗️ **Architecture**: Relacje między komponentami
- ⏱️ **Timing Diagrams**: Współbieżność vs sekwencja

### tasks/EXERCISES.md - Zawiera:
- 🟢 **Basic**: 2-3 łatwe zadania
- 🟡 **Intermediate**: 2-3 średnie zadania
- 🔴 **Advanced**: 1-2 trudne challenge'e
- ✅ Z rozwiązaniami i wyjaśnieniami

---

## 🔑 Kluczowe Koncepty

### TAP (Task-based Asynchronous Pattern)
```csharp
// Asynchroniczna funkcja zwracająca Task
async Task DoWorkAsync()
{
    await Task.Delay(1000);
    Console.WriteLine("Done!");
}

// Asynchroniczna funkcja zwracająca wartość
async Task<int> GetNumberAsync()
{
    await Task.Delay(1000);
    return 42;
}
```

### Async/Await Keywords
```csharp
// async = funkcja może zawierać await
// await = czekaj na Task bez blokowania threada
var result = await GetNumberAsync();
```

### Task Composition
```csharp
// Uruchom wiele zadań równocześnie
await Task.WhenAll(task1, task2, task3);

// Czekaj na pierwsze ukończone
var firstCompleted = await Task.WhenAny(task1, task2);
```

---

## 💡 Przykład: Hello Async

```csharp
// SEKWENCYJNIE (1 sekunda czekania)
Console.WriteLine("Start");
Thread.Sleep(1000);
Console.WriteLine("After 1s");

// ASYNCHRONICZNIE (0 sekund czekania na głównym threada!)
Console.WriteLine("Start");
await Task.Delay(1000);
Console.WriteLine("After 1s");
```

---

## 📚 Referencje

- **Microsoft Docs - TAP**
  - https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/
  - https://learn.microsoft.com/en-us/dotnet/standard/asynchronous-programming-patterns/task-based-asynchronous-pattern-tap

- **Task Parallel Library**
  - https://learn.microsoft.com/en-us/dotnet/standard/parallel-programming/task-parallel-library-tpl

- **Async/Await Best Practices**
  - https://docs.microsoft.com/en-us/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming

- **CancellationToken**
  - https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken

- **Async Streams (C# 8)**
  - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/async-streams

---

## ⚡ Performance Tips

| Pattern | Use | Avoid |
|---------|-----|-------|
| `async/await` | I/O operations | CPU-bound on UI thread |
| `Task.Run()` | CPU-bound (thread pool) | Light I/O |
| `Task.WhenAll()` | Parallel independent tasks | Blocking |
| `.Result` / `.Wait()` | Never on UI thread! | Deadlock risk |
| `ConfigureAwait(false)` | Library code (non-UI) | UI code (WinForms, WPF) |

---

## 🔧 Troubleshooting

### ❌ Deadlock: `task.Result` na UI Thread

```csharp
// ❌ ZRÓB: Blokuje UI
var result = GetDataAsync().Result;

// ✅ DOBRZE: Asynchronicznie
var result = await GetDataAsync();
```

### ❌ Forget-to-Await

```csharp
// ❌ ŹRÓDŁO ERRORA: Nie czekamy na Task
GetDataAsync();

// ✅ DOBRZE: Czekamy
await GetDataAsync();
```

### ❌ Async Void (poza Event Handlers)

```csharp
// ❌ ZŁAMANIA BŁĘDY: async void
async void ButtonClick()
{
    await DoWorkAsync();
}

// ✅ DOBRZE: async Task
async Task ButtonClickAsync()
{
    await DoWorkAsync();
}
```

---

## 🎯 Nauka Praktyczna

### Zadania do Samodzielnej Implementacji

1. **Temat 1:** Konwersja synchronicznego kodu na async
2. **Temat 2:** Porównanie czasu - sekwencja vs współbieżność
3. **Temat 3:** Download wielu plików równocześnie
4. **Temat 4:** Długo trwające obliczenia bez blokowania UI
5. **Temat 5:** Anulacja pracy w trakcie (CancellationToken)
6. **Temat 6:** Pisanie testów async methods
7. **Temat 7:** Przetwarzanie async streama danych
8. **Temat 8:** Setup DI z async inicjalizacją
9. **Temat 9:** Kompleksowy system API + Database
10. **Temat 10:** Optimizacja ValueTask

---

## 📊 Komponenty .NET 9.0

```xml
<!-- Program.csproj - Każdy temat -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="System.Threading.Channels" Version="9.0.0" />
    <PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="9.0.0" />
  </ItemGroup>
</Project>
```

---

## 🚀 Dalsze Tematy (Beyond 12)

Możliwości dalszego rozszerzenia:

- 🎯 **Custom Awaitables** - GetAwaiter() pattern
- ⚙️ **SynchronizationContext** - UI context switching
- 📡 **SignalR** - Real-time communication
- 🔄 **BackgroundService** - ASP.NET Core hosting
- 📈 **Advanced Benchmarking** - Profiling i optimization
- 🎬 **Blazor Async** - WebAssembly patterns
- 🌐 **gRPC** - High-performance RPC

---

## 📝 Uwagi dla Uczestników

> ⏰ **Bez pośpiechu:** Ta materia wymaga rozumienia, nie zapamiętywania. Czytaj, eksperymentuj, debuguj.

> 💻 **Hands-on:** Uruchamiaj kod, modyfikuj, obserwuj rezultaty w debuggerze.

> 🤔 **Stawiaj pytania:** Każdy przykład ma cel - zrozum WHY, nie tylko WHAT.

> 🎯 **Praktyka:** Rzeczywiste projekty zawsze mieszają async + I/O + error handling.

---

## 🎉 Sukces!

Po ukończeniu tego kursu będziesz umieć:

✅ Pisać asynchroniczny kod z async/await  
✅ Pracować z Task, Task<T>, ValueTask  
✅ Obsługiwać I/O i CPU-bound operacje  
✅ Unikać pułapek (deadlocks, forget-await, async void)  
✅ Testować async methods  
✅ Używać Channels i async streams  
✅ Integrować DI z async inicjalizacją  
✅ Pisać production-ready async code  

---

**Powodzenia w nauce! 🚀**

Autorzy: Educational Material Generator  
Wersja: 1.0  
Licencja: Educational Use
