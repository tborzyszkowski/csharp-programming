# Struktury: Inicjalizacja i Konstruktory

## 🎯 Cel rozdziału

Zrozumienie, jak struktury (value types) różnią się od klas w inicjalizacji i wymaganiach konstruktorów.

## 📚 Spis treści

1. [Value Types vs Reference Types](#value-types-vs-reference-types)
2. [Domyślny konstruktor struct](#domyślny-konstruktor-struct)
3. [Konstruktory w structach](#konstruktory-w-structach)
4. [Init-only properties](#init-only-properties)

---

## Value Types vs Reference Types

**Struktury (struct)** to value types - różnią się od klas (reference types):

| Aspekt | Struct | Class |
|--------|--------|-------|
| Typ | Value type | Reference type |
| Alokacja | Stack | Heap |
| Konstruktor domyślny | Automatycznie (all fields = default) | Nie (trzeba zdefiniować) |
| Inicjalizacja | Obowiązkowe wszystkie pola | Opcjonalne |

```csharp
public struct Point  // Value type
{
    public int X;
    public int Y;
}

public class Location  // Reference type
{
    public int X { get; set; }
    public int Y { get; set; }
}

// Point - automatycznie dostępny domyślny konstruktor
var p = new Point();  // OK - X=0, Y=0

// Location - wymaga konstruktora jeśli definiujesz parametrowy
var loc = new Location();  // OK tylko jeśli nie zdefiniujesz konstruktora
```

---

## Domyślny konstruktor struct

Każdy struct ma automatycznie dostępny domyślny konstruktor:

```csharp
public struct Color
{
    public byte Red { get; set; }
    public byte Green { get; set; }
    public byte Blue { get; set; }
}

// Domyślny konstruktor - wszystkie pola = 0
var black = new Color();  // R=0, G=0, B=0
Console.WriteLine($"RGB: {black.Red}, {black.Green}, {black.Blue}");  // 0,0,0
```

---

## Konstruktory w structach

Możesz definiować parametrowe konstruktory w structach, ale muszą inicjalizować WSZYSTKIE pola:

```csharp
public struct Rectangle
{
    public double Width { get; }
    public double Height { get; }
    
    // Konstruktor MUSI inicjalizować wszystkie pola
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    
    public double GetArea() => Width * Height;
}

var rect = new Rectangle(5, 3);
Console.WriteLine(rect.GetArea());  // 15
```

---

## Init-only properties (C# 9+)

Use `init` accessor for immutable structs:

```csharp
public struct Date
{
    public int Day { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
}

// Initialization
var date = new Date { Day = 25, Month = 12, Year = 2024 };

// Cannot modify after
// date.Day = 26;  // Error!
```

---

## Diagrama

```mermaid
graph LR
    A["Struct - Value Type"] --> B["Default Constructor"]
    A --> C["Stack Allocation"]
    
    D["Class - Reference Type"] --> E["No Default Constructor"]
    D --> F["Heap Allocation"]
    
    B --> G["Auto-generated"]
    E --> G
    
    G --> H["Zero-initialized fields"]
    
    style A fill:#e3f2fd
    style D fill:#f3e5f5
