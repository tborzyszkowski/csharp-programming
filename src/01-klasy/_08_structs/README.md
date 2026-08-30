# Struktury – Słowo Kluczowe struct

## 🎯 Cel

Zrozumienie różnic między klasami a strukturami.

---

## 🚀 Uruchomienie

```bash
cd code/
dotnet run
dotnet test
```

## Klasa vs Struktura

| Aspekt | Klasa | Struktura |
|--------|-------|----------|
| Typ | Reference | Value |
| Przechowywanie | Heap | Stack |
| Domyślny constructor | Nie | Tak |
| Dziedziczenie | Tak | Nie (z object) |
| Nullable | Tak | Nie |
| Wydajność | Wolniejsza | Szybsza |

## Przykład

```csharp
// KLASA - Reference type
public class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}

// STRUKTURA - Value type
public struct Vector
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

## Kiedy używać

✅ **struct**: Małe obiekty, wartościowe (Point, Color, Date)  
✅ **class**: Większe obiekty, logika biznesowa  

---

## 📖 Referencje

[Microsoft Docs - Structs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/structs)

