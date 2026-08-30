# Temat 5: Advanced Patterns & Deadlocks ⚠️

## Kluczowe Tematy

### CancellationToken
```csharp
public async Task<string> FetchDataAsync(CancellationToken ct)
{
    while (!ct.IsCancellationRequested)
    {
        try
        {
            var data = await Task.Delay(1000, ct);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Cancelled!");
        }
    }
}
```

### Deadlock: .Result na UI Thread
```csharp
// ❌ DEADLOCK! Blokuje UI thread
var result = GetDataAsync().Result;

// ✅ Dobrze
var result = await GetDataAsync();
```

### ConfigureAwait(false)
```csharp
// W library code (non-UI):
await GetDataAsync().ConfigureAwait(false);

// Unika context switching overhead
```

---

## Pełna Dokumentacja

[Więcej szczegółów w README.md](README.md)
