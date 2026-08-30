# Temat 4: CPU-Bound Operations ⚙️

## Wprowadzenie

CPU-bound operacje (długo trwające obliczenia) wymagają innego podejścia niż I/O. Zamiast `await`, używamy `Task.Run()` aby przesunąć pracę na thread pool.

## Kiedy Użyć Task.Run?

```csharp
// ❌ Nie powinno być async (CPU-bound)
public async Task<int> CalculateAsync(int n)
{
    await Task.Delay(0);  // Nonsense!
    return Fibonacci(n);
}

// ✅ Powinno być async z Task.Run
public async Task<int> CalculateAsync(int n)
{
    return await Task.Run(() => Fibonacci(n));
}
```

## Problem: Deadlock

```csharp
// ❌ DEADLOCK! Nie rób tego!
var result = GetNumberAsync().Result;  // Blokuje thread!

// ✅ Dobrze
var result = await GetNumberAsync();
```
