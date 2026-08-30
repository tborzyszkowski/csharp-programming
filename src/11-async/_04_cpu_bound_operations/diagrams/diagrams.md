# Tematy 5-10: SZYBKA REALIZACJA

## Temat 5: Advanced Patterns & Deadlocks

**Zawartość:**
- CancellationToken
- ConfigureAwait(false)
- Deadlock patterns i jak ich uniknąć
- Timeout handling

```csharp
// Deadlock example
var result = GetDataAsync().Result;  // ❌ DEADLOCK

// Proper way
var result = await GetDataAsync();   // ✅ OK
```

---

## Temat 6: Testing Async Code

**Zawartość:**
- xUnit async tests
- Task-returning test methods
- Mocking async methods
- Testing timeouts

```csharp
[Fact]
public async Task GetData_ReturnsData()
{
    var result = await _service.GetDataAsync();
    Assert.NotNull(result);
}
```

---

## Temat 7: Async LINQ & Channels

**Zawartość:**
- IAsyncEnumerable<T> (C# 8)
- Async streams
- Channels for producer/consumer
- System.Threading.Channels

```csharp
public async IAsyncEnumerable<int> GetDataAsync()
{
    for (int i = 0; i < 10; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}
```

---

## Temat 8: Dependency Injection

**Zawartość:**
- Async service initialization
- Lazy<Task<T>>
- IAsyncInitialize pattern
- ASP.NET Core DI

```csharp
services.AddScoped<IDataService>(sp => 
    new LazyDataService(() => _initAsync()));
```

---

## Temat 9: Real-World Integration

**Zawartość:**
- HTTP + Database
- Error handling
- Logging
- Comprehensive example

---

## Temat 10: Modern C# Features

**Zawartość:**
- ValueTask (C# 7)
- Async iterators (C# 8)
- Top-level async Main (C# 11)
- Latest patterns

```csharp
// C# 11: Top-level async Main
await RunAsync();

async Task RunAsync()
{
    var data = await GetDataAsync();
}
```
