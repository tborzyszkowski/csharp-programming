# Init-only Properties i Required Keyword

## 🎯 Cel rozdziału

Zrozumienie nowoczesnych C# 9+ cech: init-only properties dla immutability i required keyword dla enforcing initialization.

## 📚 Spis treści

1. [Init-only Properties (C# 9+)](#init-only-properties-c-9)
2. [Required Keyword (C# 11+)](#required-keyword-c-11)
3. [Records vs Init Properties](#records-vs-init-properties)
4. [Praktyczne Użycie](#praktyczne-użycie)

---

## Init-only Properties (C# 9+)

**Init** pozwala na zmianę właściwości **tylko podczas inicjalizacji**, nie potem:

```csharp
public class Person
{
    public string Name { get; init; }
    public int Age { get; init; }
}

var person = new Person { Name = "John", Age = 30 };
Console.WriteLine(person.Name);  // John
// person.Name = "Jane";  // BŁĄD - init-only!
```

**Zalety:**
- ✅ Immutability - niemożliwość zmiany po created
- ✅ Thread-safe - brak lock'ów
- ✅ Data integrity - kontrola co się zmienia
- ✅ Krótszy kod niż backing fields

---

## Required Keyword (C# 11+)

**Required** wymusza że właściwość **musi być ustawiona przy inicjalizacji**:

```csharp
public class User
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public string? Phone { get; init; }  // Opcjonalne
}

// BŁĄD - Email i Username wymagane
// var user = new User { };

// OK
var user = new User 
{ 
    Email = "john@example.com", 
    Username = "john_doe" 
};
```

**Zalety:**
- ✅ Compile-time validation
- ✅ Nie potrzebujesz konstruktora
- ✅ Clear API - wiadomo co wymagane

---

## Records vs Init Properties

### Classes z Init

```csharp
public class PersonClass
{
    public required string Name { get; init; }
    public int Age { get; init; }
}
```

### Records

```csharp
public record PersonRecord(string Name, int Age);
```

**Kiedy użyć?**
- **Class + init**: Gdy chcesz kontrolę nad equals/toString
- **Record**: Gdy chcesz szybko immutable data object

---

## Praktyczne Użycie

### Configuration

```csharp
public class ApiConfig
{
    public required string BaseUrl { get; init; }
    public required string ApiKey { get; init; }
    public int Timeout { get; init; } = 30;
}

var config = new ApiConfig
{
    BaseUrl = "https://api.example.com",
    ApiKey = "secret123"
};
```

### DTOs (Data Transfer Objects)

```csharp
public class UserDto
{
    public required int Id { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; init; }
}

var dto = new UserDto
{
    Id = 1,
    Email = "user@example.com"
};
```

---

## Best Practices

✅ **Init properties** dla immutable data

✅ **Required** dla mandatory fields

✅ **Records** dla simple data objects

✅ **Kombinuj init + required** dla safety

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
