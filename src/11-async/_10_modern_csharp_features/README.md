# Temat 10: Modern C# Features 🚀

ValueTask, async iterators, top-level async Main (C# 11).

```csharp
// C# 7: ValueTask (zero-alloc if synchronous)
public ValueTask<int> GetNumberAsync()
{
    if (_cache.TryGetValue("key", out var value))
        return new ValueTask<int>(value);  // No allocation
    
    return new ValueTask<int>(FetchAsync());
}

// C# 11: Top-level async Main
await RunAsync();
async Task RunAsync() { }
```
