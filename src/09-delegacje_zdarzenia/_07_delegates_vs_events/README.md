# Temat 7: Delegacje czy Zdarzenia? Wybór i Best Practices

## 🎯 Cel Tematu

Nauczysz się kiedy używać delegacji a kiedy zdarzeń.

---

## 📊 Porównanie

| Aspekt | Delegacja | Zdarzenie |
|--------|-----------|----------|
| **Bezpieczeństwo** | Brak ochrony przed zmianami | ✅ Ochrona |
| **Reset z poza klasy** | Możliwy (`=`) | ❌ Niemożliwy |
| **Logika biznesowa** | ✅ Callback, Strategy | Nie |
| **Powiadomienia** | Nie | ✅ Observer, Pub-Sub |
| **Standardowy wzorzec** | - | ✅ .NET standard |

---

## 📖 Kiedy Używać Delegacji?

### Scenariusz 1: Callback w Asynchronicznych Operacjach

```csharp
// ✅ DELEGACJA
public class FileDownloader
{
    public delegate void DownloadCompleted(string data);
    
    public void Download(string url, DownloadCompleted onComplete)
    {
        // ... pobranie
        onComplete(data);
    }
}
```

### Scenariusz 2: Strategy Pattern

```csharp
// ✅ DELEGACJA
public void SortData(int[] data, Func<int, int, int> comparer)
{
    Array.Sort(data, (a, b) => comparer(a, b));
}
```

### Scenariusz 3: Transformacja Danych

```csharp
// ✅ DELEGACJA (LINQ)
var numbers = new[] { 1, 2, 3 };
var doubled = numbers.Select(n => n * 2);  // Func<T, TResult>
```

---

## 📖 Kiedy Używać Zdarzeń?

### Scenariusz 1: Powiadomienia

```csharp
// ✅ ZDARZENIE
public class Button
{
    public event EventHandler? OnClick;
    
    public void Click()
    {
        OnClick?.Invoke(this, EventArgs.Empty);
    }
}
```

### Scenariusz 2: Obserwowanie Zmian

```csharp
// ✅ ZDARZENIE
public class Model
{
    public event EventHandler? OnDataChanged;
    
    private string data;
    public string Data
    {
        get => data;
        set
        {
            data = value;
            OnDataChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
```

### Scenariusz 3: Decoupling Komponentów

```csharp
// ✅ ZDARZENIE - UI nie musi znać biznesu
public class Application
{
    public event EventHandler? OnError;
    
    public void ProcessData()
    {
        try { /* ... */ }
        catch
        {
            OnError?.Invoke(this, new ErrorEventArgs(...));
        }
    }
}
```

---

## 💡 Decyzja w Praktyce

**Użyj DELEGACJI gdy:**
- Potrzebujesz callback'a
- Implementujesz Strategy/Command pattern
- Pracujesz z LINQ
- Funkcja bierze funkcję jako parametr

**Użyj ZDARZENIA gdy:**
- Słabo sprzężone komponenty
- Multiple subscribers
- Powiadomienia o zmianach
- Publiczny interfejs klasy
- Standardowy .NET pattern

---

## 🔗 Referencje

- [Events vs Delegates](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/events/)

---

## ➡️ Następny Krok

Temat 8: **Event-Driven Architecture** - rzeczywisty projekt
