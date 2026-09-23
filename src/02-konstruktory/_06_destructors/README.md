# Czym jest Destruktor?

## 🎯 Cel rozdziału

Zrozumienie destruktorów (finalizers), garbage collection i czyszczenia zasobów w C#.

## 📚 Spis treści

1. [Destruktor - co to?](#destruktor)
2. [Finalizers w C#](#finalizers)
3. [Garbage Collection](#garbage-collection)
4. [IDisposable pattern](#idisposable)
5. [Using statement](#using-statement)

---

## Destruktor

**Destruktor** (zwany też finalizer) to metoda wywoływana przy usuwaniu obiektu z pamięci.

```csharp
public class Resource
{
    private string name;
    
    public Resource(string name)
    {
        this.name = name;
        Console.WriteLine($"[Constructor] {name} created");
    }
    
    // Destruktor - uruchamia się gdy GC usuwa obiekt
    ~Resource()
    {
        Console.WriteLine($"[Destructor] {name} destroyed");
    }
}

var res = new Resource("MyResource");
// ... kod ...
// Gdzieś w przyszłości: GC.Collect() -> Destruktor się uruchomia
```

---

## Finalizers w C#

C# destruct ory to **finalizers** - nie są deterministyczne:

```csharp
~MyClass() { }  // Finalizer - wykonuje się ostatecznie
```

**Problem**: nie wiadomo KIEDY się uruchomi!

```csharp
public class DatabaseConnection
{
    ~DatabaseConnection()
    {
        Console.WriteLine("Closing connection");  // Może być zbyt późno!
    }
}

var db = new DatabaseConnection();
// Obiekt wychodzi ze scope, ale destruktor może się uruchomić DUŻO PÓŹNIEJ
```

---

## Garbage Collection

Garbage Collector w .NET automatycznie usuwa obiekty, które nie mają referencji.

**Generacje GC:**
- Generacja 0 - młode obiekty
- Generacja 1 - średnie obiekty
- Generacja 2 - stare obiekty

```csharp
public class GCDemo
{
    ~GCDemo()
    {
        Console.WriteLine($"Finalized in gen {GC.GetGeneration(this)}");
    }
}

for (int i = 0; i < 100; i++)
{
    var obj = new GCDemo();  // Obiekty wychodzą ze scope
}

GC.Collect();  // Wymusz Garbage Collection
```

---

## IDisposable pattern

Lepsze niż destruktor - **deterministyczne czyszczenie zasobów**:

```csharp
public class Resource : IDisposable
{
    private bool disposed = false;
    
    public void DoWork()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(Resource));
        
        Console.WriteLine("Working...");
    }
    
    // Czyszczenie zasobów - DETERMINISTYCZE
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);  // Powiedz GC by nie uruchamiał finalizer
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // Czyszcz zasoby zarządzane
                Console.WriteLine("Disposing managed resources");
            }
            
            // Czyszcz zasoby niezarządzane
            Console.WriteLine("Disposing unmanaged resources");
            disposed = true;
        }
    }
    
    // Finalizer - backup jeśli ktoś zapomni Dispose
    ~Resource()
    {
        Dispose(false);
    }
}

// Użycie
using (var res = new Resource())
{
    res.DoWork();
}  // Automatycznie Dispose() się uruchomi
```

---

## Using statement (C# 8+)

```csharp
// C# 8+ - najprostszy sposób
using var res = new Resource();
res.DoWork();
// Tutaj automatycznie Dispose()

// lub
using (var res = new Resource())
{
    res.DoWork();
}
```

---

## Best Practices

✅ **Używaj `IDisposable` dla zasobów niezarządzanych**

✅ **Zawsze używaj `using` statement**

✅ **Finalizers jako backup**

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
