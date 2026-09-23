# Temat 3: System.Activator - Dynamiczne Tworzenie Instancji

## 🏗️ Czym jest Activator?

**System.Activator** = fabryka do dynamicznego tworzenia instancji bez znania typu na compile-time

```csharp
// Znasz typ na compile-time → zwykły new
var person1 = new Person { Name = "Alice" };

// Nie znasz typu na compile-time → Activator
Type type = GetTypeAtRuntime();
var person2 = Activator.CreateInstance(type);  // Czarna magia!

// Oba dają ten sam rezultat, ale drugi jest dynamiczny
```

---

## 🔑 Activator.CreateInstance() - Wszystkie Warianty

### Variant 1: Brak Parametrów (Bezparametrowy Konstruktor)

```csharp
var type = typeof(Person);

// Tworzy instancję używając bezparametrowego konstruktora
var instance = Activator.CreateInstance(type);

Console.WriteLine(instance.GetType().Name);  // "Person"
Console.WriteLine(instance is Person);       // true
```

### Variant 2: Z Parametrami Konstruktora

```csharp
public class Person
{
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    public string Name { get; set; }
    public int Age { get; set; }
}

var type = typeof(Person);

// Tworzy instancję z parametrami
var instance = (Person)Activator.CreateInstance(type, "Alice", 30)!;

Console.WriteLine(instance.Name);  // "Alice"
Console.WriteLine(instance.Age);   // 30
```

### Variant 3: Named Parameters

```csharp
var type = typeof(Person);

// Przekaż parametry z wartościami
object?[] args = { "Bob", 25 };
var instance = (Person)Activator.CreateInstance(type, args)!;

Console.WriteLine(instance.Name);  // "Bob"
```

### Variant 4: Generic CreateInstance<T>

```csharp
// Type-safe variant
var instance = Activator.CreateInstance<Person>();

Console.WriteLine(instance.GetType().Name);  // "Person"
// Zwraca Type<Person> zamiast object
```

### Variant 5: Z BindingFlags (Advanced)

```csharp
var type = typeof(Person);

// Kontroluj jaki konstruktor użyć
var instance = Activator.CreateInstance(
    type,
    bindingAttr: System.Reflection.BindingFlags.Public,
    binder: null,
    args: new object[] { "Charlie", 35 },
    culture: null
);

Console.WriteLine(((Person)instance!).Name);  // "Charlie"
```

---

## 💡 Praktyczne Zastosowania

### 1. Plugin System

```csharp
// Interface dla pluginów
public interface IPlugin
{
    string Name { get; }
    void Execute();
}

// Implementacje w osobnych DLL
public class CalculatorPlugin : IPlugin
{
    public string Name => "Calculator";
    public void Execute() => Console.WriteLine("Calculating...");
}

// Loader (nie znasz typu na compile-time!)
public class PluginLoader
{
    public IPlugin LoadPlugin(Type pluginType)
    {
        // Dynamicznie utwórz instancję
        var instance = Activator.CreateInstance(pluginType);
        
        if (instance is IPlugin plugin)
            return plugin;
        
        throw new InvalidOperationException("Not a valid plugin");
    }
}

// Użycie
var loader = new PluginLoader();
var plugin = loader.LoadPlugin(typeof(CalculatorPlugin));
plugin.Execute();  // "Calculating..."
```

### 2. Dependency Injection Container (Simplified)

```csharp
public class Container
{
    private Dictionary<Type, Type> registrations = new();
    
    public void Register<TInterface, TImplementation>()
        where TImplementation : TInterface
    {
        registrations[typeof(TInterface)] = typeof(TImplementation);
    }
    
    public T Resolve<T>() where T : class
    {
        var interfaceType = typeof(T);
        
        if (!registrations.TryGetValue(interfaceType, out var implementationType))
            throw new InvalidOperationException($"No registration for {interfaceType.Name}");
        
        // Dynamically create implementation
        var instance = Activator.CreateInstance(implementationType);
        return (T)instance!;
    }
}

// Usage
public interface IRepository { }
public class DatabaseRepository : IRepository { }

var container = new Container();
container.Register<IRepository, DatabaseRepository>();

var repo = container.Resolve<IRepository>();  // Magicznie utworzono!
Console.WriteLine(repo is DatabaseRepository);  // true
```

### 3. ORM: Mapowanie Wyników Bazy Danych

```csharp
public class DataMapper<T> where T : class, new()
{
    public T MapFromDatabase(Dictionary<string, object> data)
    {
        // Utwórz instancję (wymaga bezparametrowego konstruktora)
        var instance = Activator.CreateInstance<T>();
        
        // Ustaw properties dynamicznie
        var type = typeof(T);
        foreach (var kvp in data)
        {
            var property = type.GetProperty(kvp.Key);
            if (property != null && property.CanWrite)
            {
                property.SetValue(instance, kvp.Value);
            }
        }
        
        return instance;
    }
}

// Użycie
public class User
{
    public string Name { get; set; } = "";
    public int Id { get; set; }
}

var data = new Dictionary<string, object>
{
    { "Name", "Alice" },
    { "Id", 1 }
};

var mapper = new DataMapper<User>();
var user = mapper.MapFromDatabase(data);

Console.WriteLine(user.Name);  // "Alice"
Console.WriteLine(user.Id);    // 1
```

---

## ⚠️ Rzadkie Przypadki

### Generic Types

```csharp
// Problem: typeof(List<>) jest otwartym generycznym typem
var openGeneric = typeof(List<>);

// Rozwiązanie: Użyj MakeGenericType()
var listOfInt = openGeneric.MakeGenericType(typeof(int));
var instance = Activator.CreateInstance(listOfInt);

Console.WriteLine(instance.GetType().Name);  // "List`1"
```

### Konstruktor z Parametrami Domyślnymi

```csharp
public class Config
{
    public Config(string name = "default") => Name = name;
    public string Name { get; }
}

// Brak parametrów → używa default
var config1 = Activator.CreateInstance<Config>();
Console.WriteLine(config1.Name);  // "default"

// Z parametrem
var config2 = (Config)Activator.CreateInstance(typeof(Config), "custom")!;
Console.WriteLine(config2.Name);  // "custom"
```

### Private Constructors

```csharp
public class Singleton
{
    private Singleton() { }
    public static Singleton Instance { get; } = new();
}

// Normalnie: ERROR - nie możesz AccessViolation
// var instance = new Singleton();

// Z refleksją: Możesz, ale shouldn't!
try
{
    var instance = Activator.CreateInstance(
        typeof(Singleton),
        nonPublic: true
    );
    // Uda się! Ale łamie singleton pattern
}
catch (MissingMethodException)
{
    Console.WriteLine("Nie ma bezparametrowego konstruktora");
}
```

---

## 🏭 Factory Pattern z Activator

```csharp
// Marker interface
public interface IData { }

public class UserData : IData { }
public class ProductData : IData { }

// Generic factory
public class DataFactory<T> where T : class, IData, new()
{
    public T Create()
    {
        return Activator.CreateInstance<T>();
    }
    
    public T Create(object[] args)
    {
        return (T)Activator.CreateInstance(typeof(T), args)!;
    }
}

// Użycie
var factory = new DataFactory<UserData>();
var user = factory.Create();

var users = factory.Create(new object[] { "names", 100 });
```

---

## ⚡ Performance Considerations

```csharp
// Benchmark: new vs Activator.CreateInstance

var iterations = 100000;

// Metoda 1: Direct instantiation (FAST)
var sw1 = System.Diagnostics.Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    var _ = new Person { Name = "Alice" };
}
sw1.Stop();

// Metoda 2: Activator (SLOW)
var sw2 = System.Diagnostics.Stopwatch.StartNew();
var type = typeof(Person);
for (int i = 0; i < iterations; i++)
{
    var _ = Activator.CreateInstance(type);
}
sw2.Stop();

// Metoda 3: Activator + parameters (SLOWER)
var sw3 = System.Diagnostics.Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    var _ = Activator.CreateInstance(type, "Alice", 30);
}
sw3.Stop();

Console.WriteLine($"Direct: {sw1.ElapsedMilliseconds}ms");
Console.WriteLine($"Activator: {sw2.ElapsedMilliseconds}ms (~{(double)sw2.ElapsedMilliseconds/sw1.ElapsedMilliseconds:F0}x slower)");
Console.WriteLine($"Activator + params: {sw3.ElapsedMilliseconds}ms (~{(double)sw3.ElapsedMilliseconds/sw1.ElapsedMilliseconds:F0}x slower)");
```

**Lesson**: Activator powolny! Cache instances jeśli możliwe.

---

## 🔒 Bezpieczeństwo

```csharp
// ❌ NIEBEZPIECZNE: User-controlled type name
string userInput = "SomeEvilType";  // Może pochodzić z sieci!
Type type = Type.GetType(userInput);
var instance = Activator.CreateInstance(type);
// Mogę wczytać ANY typ i uruchomić jego konstruktor!

// ✅ BEZPIECZNE: Whitelist
private static readonly HashSet<Type> AllowedTypes = new()
{
    typeof(CalculatorPlugin),
    typeof(ReportPlugin)
};

public IPlugin LoadPlugin(Type type)
{
    if (!AllowedTypes.Contains(type))
        throw new SecurityException("Plugin not allowed");
    
    return (IPlugin)Activator.CreateInstance(type)!;
}
```

---

## 📚 Summary

**Activator.CreateInstance()** = dynamiczne tworzenie instancji

**Warianty:**
- Bez parametrów
- Z parametrami konstruktora
- Generic variant
- Z BindingFlags (advanced)

**Use Cases:**
- Plugin systems
- Dependency Injection
- ORM/Data Mapping
- Factory patterns

**Performance:** Powolna! Cache jeśli możliwe.

**Security:** Waliduj typy, nie wierzaj user input!

---

## 🎯 Następny Temat

Temat 4: Tworzenie Własnych Atrybutów - AttributeUsage, AttributeTargets
