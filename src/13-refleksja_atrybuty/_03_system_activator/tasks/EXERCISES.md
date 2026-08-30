# Ćwiczenia: System.Activator

## 🟢 Basic Level

### Zadanie 1: CreateInstance without Parameters
Utwórz instancję Person bez parametrów:

```csharp
var type = typeof(Person);
// TODO: Create instance
```

**Rozwiązanie:**
```csharp
var instance = Activator.CreateInstance(type);
Console.WriteLine(instance is Person);  // true
```

---

### Zadanie 2: CreateInstance<T> Generic
Użyj generic wariantu:

```csharp
// TODO: Create Person using generic CreateInstance<T>
```

**Rozwiązanie:**
```csharp
var person = Activator.CreateInstance<Person>();
Console.WriteLine(person.GetType().Name);  // "Person"
```

---

### Zadanie 3: Cast Result
Jak castować wynik CreateInstance?

```csharp
var type = typeof(Book);
var instance = Activator.CreateInstance(type);
// TODO: Cast to Book
```

**Rozwiązanie:**
```csharp
var book = (Book)instance!;
// lub
var book = instance as Book;
```

---

### Zadanie 4: Constructor Parameters
Utwórz Book z 3 parametrami:

```csharp
public class Book
{
    public Book(string title, string author, int pages) { }
}

// TODO: Create with parameters
```

**Rozwiązanie:**
```csharp
var type = typeof(Book);
var book = (Book)Activator.CreateInstance(
    type, 
    "Clean Code",    // title
    "Robert Martin", // author
    464              // pages
)!;
```

---

### Zadanie 5: Type.GetType()
Ładuj typ z nazwy stringa:

```csharp
// TODO: Load Person type from string "Person"
```

**Rozwiązanie:**
```csharp
var type = Type.GetType("Person");
if (type != null)
{
    var instance = Activator.CreateInstance(type);
}
```

---

## 🟡 Intermediate Level

### Zadanie 6: Plugin System
Utwórz prosty plugin system:

```csharp
public interface IPlugin
{
    void Execute();
}

public class Plugin1 : IPlugin
{
    public void Execute() => Console.WriteLine("Plugin 1");
}

// TODO: Load and execute plugin dynamically
```

**Rozwiązanie:**
```csharp
Type pluginType = typeof(Plugin1);
var plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
plugin.Execute();
```

---

### Zadanie 7: Array of Types
Utwórz instancje wielu typów:

```csharp
Type[] types = { typeof(Person), typeof(Book), typeof(Author) };

// TODO: Create instances for all types
```

**Rozwiązanie:**
```csharp
var instances = types
    .Select(t => Activator.CreateInstance(t))
    .ToList();
```

---

### Zadanie 8: Generic Type Creation
Utwórz List<int> dynamicznie:

```csharp
var openGeneric = typeof(List<>);

// TODO: Create concrete List<int> and instantiate
```

**Rozwiązanie:**
```csharp
var listOfInt = openGeneric.MakeGenericType(typeof(int));
var instance = Activator.CreateInstance(listOfInt);
// Now instance is List<int>
```

---

### Zadanie 9: DI Container Simulation
Prosty DI container:

```csharp
public interface IRepository { }
public class DatabaseRepository : IRepository { }

// TODO: Register and resolve
```

**Rozwiązanie:**
```csharp
var registrations = new Dictionary<Type, Type>
{
    { typeof(IRepository), typeof(DatabaseRepository) }
};

var repo = (IRepository)Activator.CreateInstance(
    registrations[typeof(IRepository)]
)!;
```

---

### Zadanie 10: Error Handling
Obsługuj błędy przy CreateInstance:

```csharp
var type = typeof(SomeClass);

// TODO: Handle MissingMethodException
```

**Rozwiązanie:**
```csharp
try
{
    var instance = Activator.CreateInstance(type);
}
catch (MissingMethodException)
{
    Console.WriteLine("No parameterless constructor found");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Performance Comparison
Porównaj wydajność new vs Activator:

```csharp
var iterations = 100000;

// Metoda 1: new
var sw1 = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    var _ = new Person();
}
sw1.Stop();

// Metoda 2: Activator
var sw2 = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    var _ = Activator.CreateInstance<Person>();
}
sw2.Stop();

Console.WriteLine($"new: {sw1.ElapsedMilliseconds}ms");
Console.WriteLine($"Activator: {sw2.ElapsedMilliseconds}ms");
Console.WriteLine($"Ratio: {(double)sw2.ElapsedMilliseconds / sw1.ElapsedMilliseconds:F1}x");
```

---

### Zadanie 12: Secure Plugin Loader
Zaladuj plugin, ale waliduj typ:

```csharp
public interface IPlugin { }

private static readonly HashSet<Type> AllowedPlugins = new()
{
    typeof(MathPlugin),
    typeof(TextPlugin)
};

public IPlugin LoadPlugin(Type pluginType)
{
    // TODO: Validate and load only whitelisted plugins
}
```

**Rozwiązanie:**
```csharp
public IPlugin LoadPlugin(Type pluginType)
{
    if (!AllowedPlugins.Contains(pluginType))
        throw new SecurityException("Plugin not allowed");
    
    if (!typeof(IPlugin).IsAssignableFrom(pluginType))
        throw new InvalidOperationException("Not a valid plugin");
    
    return (IPlugin)Activator.CreateInstance(pluginType)!;
}
```

---

## 📊 Wskazówki

- ✅ Zawsze sprawdzaj czy typ implementuje interfejs
- ✅ Handle MissingMethodException (no constructor)
- ✅ Cache instances jeśli możliwe
- ✅ Waliduj user-provided type names
- ✅ Używaj try-catch dla robustności
- ✅ Rozważ source generators dla performance
- ❌ Nie trusted user input dla type names
- ❌ Nie używaj Activator w tight loops
- ❌ Nie zapomnij null checks na wyniku

---

## 🎯 Key Takeaways

System.Activator:

```
CreateInstance() = dynamiczne tworzenie instancji
Bez parametrów = new Person()
Z parametrami = new Person(name, age)
Generic<T> = type-safe variant
BindingFlags = zaawansowana kontrola

Use Cases: Plugins, DI, ORM, Factory patterns
Performance: ~5-20x slower than new
Security: Waliduj typy!
```

Pamiętaj: **Activator to moc, ale z wydajnościową ceną!**
