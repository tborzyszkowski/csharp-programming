# Temat 8: Dependency Injection + Async

Async initialization w DI kontenerach, `Lazy<Task<T>>`, `IAsyncInitialize`.

```csharp
// Lazy<Task<T>> pattern
services.AddScoped<Lazy<Task<DataService>>>(sp =>
    new Lazy<Task<DataService>>(async () =>
    {
        var service = new DataService();
        await service.InitializeAsync();
        return service;
    }));
```
