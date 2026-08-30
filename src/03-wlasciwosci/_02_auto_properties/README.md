# Automatyczne Właściwości (Auto Properties)

## 🎯 Cel rozdziału

Zrozumienie automatycznych właściwości (C# 3+) i kiedy są efektywne jako alternatywa dla backing fields.

## 📚 Spis treści

1. [Co to Auto Property?](#co-to-auto-property)
2. [Składnia](#składnia)
3. [Backing Field vs Auto Property](#backing-field-vs-auto-property)
4. [Wady i Zalety](#wady-i-zalety)

---

## Co to Auto Property?

**Auto Property** kompilator automatycznie generuje prywatne pole (`backing field`):

```csharp
// Tradycyjnie - musisz pisać backing field
public class Person
{
    private string _name;
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
}

// Auto Property - kompilator generuje backing field
public class Person
{
    public string Name { get; set; }  // Gotowe!
}

// To samo, ale mniej kodu!
```

---

## Składnia

### Najprostszy case

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person = new Person { Name = "John", Age = 30 };
```

### Z inicjalizacją

```csharp
public class Config
{
    public string AppName { get; set; } = "MyApp";
    public int MaxConnections { get; set; } = 100;
}

var config = new Config();
Console.WriteLine(config.AppName);  // MyApp
```

### Asymetryczne accessory

```csharp
public class User
{
    public int Id { get; private set; }  // Publiczny get, prywatny set
    public string Email { get; set; }
    
    public User(int id)
    {
        Id = id;
    }
}

var user = new User(42);
var id = user.Id;  // OK
// user.Id = 100;  // BŁĄD - private setter
```

---

## Backing Field vs Auto Property

### Backing Field (Tradycyjnie)

```csharp
public string Name
{
    get { return _name; }
    set 
    { 
        if (!string.IsNullOrEmpty(value))
            _name = value;
    }
}
```

**Kiedy używać:**
- Potrzebujesz **walidacji** w setterze
- Potrzebujesz **side effects** (logowanie, event)
- Wiele operacji logicznych

### Auto Property

```csharp
public string Name { get; set; }
```

**Kiedy używać:**
- Proste mapowanie 1:1
- Nie ma logiki w getterze/setterze
- Kod musi być zwięzły

---

## Wady i Zalety

| Aspekt | Auto | Backing Field |
|--------|------|---------------|
| Kod | Krótki | Gadatliwy |
| Walidacja | Nie | Tak |
| Logowanie | Nie | Tak |
| Wydajność | Równa | Równa |
| Side effects | Nie | Tak |

---

## Best Practices

✅ Auto property dla **prostych właściwości**

✅ Backing field + property dla **walidacji/logiki**

✅ Asymetryczne accessory: `public get; private set;`

✅ Inicjalizuj domyślne wartości

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
