# Ćwiczenia: Reading Attributes

## 🟢 Basic Level

### Zadanie 1: GetCustomAttribute
Czytaj pojedynczy atrybut:

```csharp
[Author("Alice")]
public class User { }

// TODO: Read Author attribute
```

**Rozwiązanie:**
```csharp
var attr = typeof(User).GetCustomAttribute<AuthorAttribute>();
Console.WriteLine($"Author: {attr?.Name}");
```

---

### Zadanie 2: GetCustomAttributes
Czytaj wielokrotne atrybuty:

```csharp
[Tag("important")]
[Tag("urgent")]
public class Task { }

// TODO: Read all tags
```

**Rozwiązanie:**
```csharp
var tags = typeof(Task).GetCustomAttributes<TagAttribute>();
foreach (var tag in tags)
    Console.WriteLine($"Tag: {tag.Name}");
```

---

### Zadanie 3: IsDefined
Sprawdzaj czy atrybut istnieje:

```csharp
// TODO: Check if has [Obsolete]
```

**Rozwiązanie:**
```csharp
bool hasObsolete = typeof(MyClass).IsDefined(typeof(ObsoleteAttribute));
```

---

### Zadanie 4: Property Attributes
Czytaj atrybuty z property:

```csharp
public class User
{
    [Required]
    public string Name { get; set; }
}

// TODO: Read [Required] from Name property
```

**Rozwiązanie:**
```csharp
var prop = typeof(User).GetProperty("Name");
var required = prop?.GetCustomAttribute<RequiredAttribute>();
if (required != null)
    Console.WriteLine("Name is required");
```

---

### Zadanie 5: Method Attributes
Czytaj atrybuty z metody:

```csharp
public class Service
{
    [Obsolete]
    public void OldMethod() { }
}

// TODO: Check if method is obsolete
```

**Rozwiązanie:**
```csharp
var method = typeof(Service).GetMethod("OldMethod");
var obsolete = method?.GetCustomAttribute<ObsoleteAttribute>();
if (obsolete != null)
    Console.WriteLine($"Deprecated: {obsolete.Message}");
```

---

## 🟡 Intermediate Level

### Zadanie 6: null Check Safety
Bezpieczne czytanie atrybutów:

```csharp
var type = typeof(SomeClass);
var attr = type.GetCustomAttribute<DocAttribute>();

// TODO: Safe handling
```

**Rozwiązanie:**
```csharp
if (attr != null)
    Console.WriteLine(attr.Description);
else
    Console.WriteLine("No documentation");
```

---

### Zadanie 7: All Attributes Untyped
Czytaj WSZYSTKIE atrybuty:

```csharp
[Obsolete]
[Serializable]
[DebuggerDisplay("Test")]
public class MyClass { }

// TODO: Read all attributes (any type)
```

**Rozwiązanie:**
```csharp
var allAttrs = typeof(MyClass).GetCustomAttributes();
foreach (var attr in allAttrs)
    Console.WriteLine(attr.GetType().Name);
```

---

### Zadanie 8: Inherit Parameter
Testuj dziedziczenie atrybutów:

```csharp
[Doc("Base")]
public class Base { }

public class Derived : Base { }

// TODO: Check with inherit=true and inherit=false
```

**Rozwiązanie:**
```csharp
var with = typeof(Derived).GetCustomAttributes<DocAttribute>(inherit: true);
var without = typeof(Derived).GetCustomAttributes<DocAttribute>(inherit: false);

Console.WriteLine($"With inherit: {with.Length}");     // 1
Console.WriteLine($"Without inherit: {without.Length}"); // 0
```

---

### Zadanie 9: Attribute Reader Pattern
Stwórz helper do czytania atrybutów:

```csharp
public class AttributeReader
{
    // TODO: Create GetAllAttributes() method
}
```

**Rozwiązanie:**
```csharp
public Dictionary<string, List<object>> GetAllAttributes(Type type)
{
    var result = new Dictionary<string, List<object>>();
    
    result["Type"] = new(type.GetCustomAttributes());
    
    foreach (var prop in type.GetProperties())
        result[$"Property.{prop.Name}"] = new(prop.GetCustomAttributes());
    
    foreach (var method in type.GetMethods())
        result[$"Method.{method.Name}"] = new(method.GetCustomAttributes());
    
    return result;
}
```

---

### Zadanie 10: LINQ Filtering
Filtruj typy po atrybutach:

```csharp
var types = new[] { Type1, Type2, Type3 };

// TODO: Find types with [Author]
```

**Rozwiązanie:**
```csharp
var withAuthor = types
    .Where(t => t.GetCustomAttribute<AuthorAttribute>() != null)
    .ToList();
```

---

## 🔴 Advanced Level

### Zadanie 11: Attribute Caching
Implementuj cache dla atrybutów:

```csharp
private static Dictionary<Type, DocAttribute[]> Cache = new();

// TODO: Create cached reader
```

**Rozwiązanie:**
```csharp
public DocAttribute[] GetCached(Type type)
{
    if (!Cache.TryGetValue(type, out var attrs))
    {
        attrs = type.GetCustomAttributes<DocAttribute>().ToArray();
        Cache[type] = attrs;
    }
    return attrs;
}
```

---

### Zadanie 12: Dynamic Attribute Collection
Zbierz atrybuty z całej hierarchii klas:

```csharp
[Doc("Base")]
public class Base { }

public class Derived : Base { }

public class MoreDerived : Derived { }

// TODO: Collect all attributes from entire hierarchy
```

**Rozwiązanie:**
```csharp
public List<object> GetHierarchyAttributes(Type type)
{
    var result = new List<object>();
    
    var current = type;
    while (current != null && current != typeof(object))
    {
        result.AddRange(current.GetCustomAttributes());
        current = current.BaseType;
    }
    
    return result;
}
```

---

## 📊 Wskazówki

- ✅ Zawsze cache GetCustomAttributes
- ✅ Użyj IsDefined() dla szybkiego check
- ✅ Check != null na wyniku GetCustomAttribute
- ✅ Kombinuj z LINQ do filtrowania
- ✅ Iteruj properties/methods dla głębokich reads
- ✅ Waliduj inherit parameter
- ❌ Nie ignoruj null results
- ❌ Nie zapomnij .ToArray() dla Length
- ❌ Nie czytaj atrybutów w tight loops

---

## 🎯 Key Takeaways

Reading Attributes:

```
GetCustomAttribute<T>() = jeden
GetCustomAttributes<T>() = wszystkie (typed)
GetCustomAttributes() = wszystkie (untyped)
IsDefined() = szybki check
inherit = czy parent classes

Czytaj z: Type, Property, Method, Parameter
Cache results dla performance!
Kombinuj z LINQ do filtrowania
```

Pamiętaj: **Caching to klucz do performance!**
