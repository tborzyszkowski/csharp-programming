# Nowoczesne C#: Records, Init Properties, Primary Constructors

## 🎯 Cel rozdziału

Zrozumienie nowych cech C# (9+, 11+, 12+) dla bardziej zwięzłej i bezpiecznej inicjalizacji obiektów.

## 📚 Spis treści

1. [Records (C# 9+)](#records)
2. [Init-only Properties (C# 9+)](#init-only-properties)
3. [Primary Constructors (C# 12+)](#primary-constructors)
4. [Porównanie z tradycyjnymi klasami](#porównanie)

---

## Records (C# 9+)

**Record** to typ referencyjny zoptymalizowany dla **immutable data**.

### Automatyczne cechy:
- Implementacja `Equals()` i `GetHashCode()` 
- `ToString()` z wszyst kich właściwości
- Deconstruction

```csharp
// Tradycyjna klasa
public class PersonClass
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Record (C# 9+)
public record Person(string Name, int Age);

// Użycie
var p1 = new Person("John", 30);
var p2 = new Person("John", 30);

Console.WriteLine(p1 == p2);  // true (porównanie wartości!)
Console.WriteLine(p1.ToString());  // Person { Name = John, Age = 30 }
```

### Positional Records

```csharp
public record Point(double X, double Y);

var p = new Point(3, 4);
Console.WriteLine(p.X);  // 3
Console.WriteLine(p.Y);  // 4
```

### With-expression (immutability)

```csharp
public record Person(string Name, int Age);

var person1 = new Person("John", 30);
var person2 = person1 with { Age = 31 };  // Klonuj z zmianą

Console.WriteLine(person1);  // Person { Name = John, Age = 30 }
Console.WriteLine(person2);  // Person { Name = John, Age = 31 }
```

---

## Init-only Properties (C# 9+)

Właściwości które mogą być ustawiane tylko w inicjalizacji:

```csharp
public class Person
{
    public string Name { get; init; }  // Można ustawić tylko raz
    public int Age { get; init; }
}

var person = new Person { Name = "John", Age = 30 };
// person.Name = "Jane";  // BŁĄD! Init-only
```

---

## Primary Constructors (C# 12+)

Konstruktory zdefiniowane w deklaracji klasy:

```csharp
// Tradycyjnie
public class Person
{
    public string Name { get; }
    public int Age { get; }
    
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
}

// C# 12+ - Primary Constructor
public class Person(string name, int age)
{
    public string Name => name;
    public int Age => age;
}

// Użycie - to samo
var person = new Person("John", 30);
```

---

## Porównanie

| Cecha | Klasa | Record | Primary |
|-------|-------|--------|---------|
| Reference/Value | Ref | Ref | Ref |
| Equals() | Ręcznie | Auto | Ręcznie |
| ToString() | Ręcznie | Auto | Ręcznie |
| Immutable | Opcjonalne | Domyślnie | Opcjonalne |
| Init-only | Ręcznie | Auto | Wymaga init |

---

## Best Practices

✅ Używaj **records** dla immutable data (DTOs, value objects)

✅ Używaj **init** properties dla bezpieczeństwa

✅ Używaj **primary constructors** dla skrócenia kodu

✅ Kombinuj z **with-expression** dla klonowania

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
