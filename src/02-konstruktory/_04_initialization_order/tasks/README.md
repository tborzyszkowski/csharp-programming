# Zadania - Kolejność Inicjalizacji

## 📝 Zadanie 1: Obserwuj kolejność inicjalizacji

Stwórz klasę z logowaniem każdego kroku inicjalizacji, aby zobaczyć dokładną kolejność.

```csharp
public class Demo
{
    public string Field1 { get; set; } = Log("Field1") ?? "value1";
    public string Field2 { get; set; } = Log("Field2") ?? "value2";
    
    private static string? Log(string msg)
    {
        Console.WriteLine($"  {msg}");
        return null;
    }
    
    public Demo()
    {
        Console.WriteLine("  Constructor");
    }
}

// Output:
//   Field1
//   Field2
//   Constructor
```

## 📝 Zadanie 2: Pole readonly w konstruktorze

Zademonstruj, że readonly pole musi być ustawione w konstruktorze lub initializer.

```csharp
public readonly struct Person
{
    public string Name { get; init; }  // Init-only (C# 9+)
    public int Age { get; init; }
}

var p = new Person { Name = "John", Age = 30 };
```

---

## ✅ Rozwiązania w `code/Program.cs`
