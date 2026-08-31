# Destruktory a Kod Zarządzany i Niezarządzany

## 🎯 Cel rozdziału

Zrozumienie różnicy między zarządzanymi i niezarządzanymi zasobami, garbage collection, i wzorca IDisposable dla czyszczenia.

## 📚 Spis treści

1. [Zarządzane vs Niezarządzane zasoby](#zarządzane-vs-niezarządzane)
2. [Garbage Collection](#garbage-collection)
3. [IDisposable Pattern](#idisposable-pattern)
4. [Using Statement](#using-statement)

---

## Zarządzane vs Niezarządzane

### Zarządzane (Managed)
- Obiekty .NET (strings, listy, custom klasy)
- Automatycznie czyszczone przez GC
- Brak konieczności ręcznego czyszczenia

```csharp
var list = new List<int>();  // Zarządzany
// GC automatycznie go czyści
```

### Niezarządzane (Unmanaged)
- Pliki, bazy danych, socket-y
- P/Invoke - native DLL-i
- Zasoby systemowe
- Wymaga ręcznego czyszczenia!

```csharp
var fileHandle = File.OpenRead("file.txt");  // Niezarządzany - MUSI być zamknięty!
// Bez using/try-finally -> memory leak!
```

---

## Garbage Collection

GC .NET automatycznie usuwa obiekty:

```csharp
public class Resource
{
    ~Resource()
    {
        Console.WriteLine("Finalized");
    }
}

var res = new Resource();
// Wychodzi ze scope
// Gdzieś w przyszłości: GC.Collect() -> finalizacja
```

**Problem**: Finalizery nie są deterministyczne!

---

## IDisposable Pattern

**Deterministyczne czyszczenie** zasobów:

```csharp
public class ManagedResource : IDisposable
{
    private bool disposed = false;
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Nie ma potrzeby finalizera
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Czyszczenie zasobów zarządzanych
                Console.WriteLine("Closing managed resources");
            }
            
            // Czyszczenie zasobów niezarządzanych
            Console.WriteLine("Closing unmanaged resources");
            disposed = true;
        }
    }
    
    ~ManagedResource()
    {
        Dispose(false);  // Backup
    }
}
```

---

## Using Statement

### C# 7.0 - 8.x

```csharp
using (var res = new ManagedResource())
{
    // Kod
}  // res.Dispose() tutaj
```

### C# 8.0+

```csharp
using var res = new ManagedResource();
// Kod
// res.Dispose() na końcu scope
```

---

## Best Practices

✅ Zawsze implementuj IDisposable dla zasobów niezarządzanych

✅ Używaj `using` statement

✅ Finalizer jako backup

✅ Przesłoń `Dispose(bool)` do rozróżnienia

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
