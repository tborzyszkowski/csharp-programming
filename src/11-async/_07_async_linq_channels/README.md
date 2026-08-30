# Temat 7: Async LINQ & Channels 📡

IAsyncEnumerable<T>, async streams, Channels (producer/consumer).

```csharp
// Async stream (C# 8+)
public async IAsyncEnumerable<int> GetDataAsync()
{
    for (int i = 0; i < 10; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}

// Usage
await foreach (var item in GetDataAsync())
{
    Console.WriteLine(item);
}
```
