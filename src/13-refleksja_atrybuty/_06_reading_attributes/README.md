# Temat 6: Czytanie Atrybutów - GetCustomAttribute(s)

## 📖 Czytanie Atrybutów z Refleksją

**Part 4** poznałeś: tworzenie atrybutów

**Part 5** poznałeś: zaawansowane wzorce

**Part 6** = czytanie atrybutów refleksją (runtime inspection)

---

## 🔍 GetCustomAttribute<T>() vs GetCustomAttributes<T>()

### Pojedynczy Atrybut

```csharp
// Zwraca JEDEN atrybut lub null

[Author("Alice")]
public class User { }

// Get single attribute
var attr = typeof(User).GetCustomAttribute<AuthorAttribute>();

if (attr != null)
    Console.WriteLine($"Author: {attr.Name}");  // "Alice"
else
    Console.WriteLine("No [Author] attribute");
```

### Wielokrotne Atrybuty Tego Samego Typu

```csharp
// Zwraca WSZYSTKIE atrybuty danego typu

[Tag("important")]
[Tag("urgent")]
[Tag("production")]
public class CriticalService { }

// Get ALL tags
var tags = typeof(CriticalService).GetCustomAttributes<TagAttribute>();

foreach (var tag in tags)
    Console.WriteLine($"Tag: {tag.Name}");
// Output:
// Tag: important
// Tag: urgent
// Tag: production
```

---

## ✅ Sprawdzanie Istnienia Atrybutu

### Has Attribute?

```csharp
var type = typeof(SomeClass);

// Metoda 1: Check if result != null
var attr = type.GetCustomAttribute<ObsoleteAttribute>();
if (attr != null)
    Console.WriteLine("✓ Has [Obsolete]");
else
    Console.WriteLine("✗ No [Obsolete]");

// Metoda 2: Get count
var attrs = type.GetCustomAttributes<ObsoleteAttribute>();
if (attrs.Length > 0)
    Console.WriteLine("✓ Has [Obsolete]");

// Metoda 3: IsDefined
bool hasObsolete = type.IsDefined(typeof(ObsoleteAttribute));
Console.WriteLine(hasObsolete ? "✓" : "✗");
```

---

## 🏷️ Atrybuty na Różnych Elementach

### Type vs Property vs Method

```csharp
[Documented("Class-level doc")]
public class DataService
{
    [Documented("Property-level doc")]
    public string Name { get; set; } = "";
    
    [Documented("Method-level doc")]
    public void Process() { }
}

// Get class attribute
var typeAttr = typeof(DataService).GetCustomAttribute<DocumentedAttribute>();

// Get property attribute
var propAttr = typeof(DataService).GetProperty("Name")?.GetCustomAttribute<DocumentedAttribute>();

// Get method attribute
var methodAttr = typeof(DataService).GetMethod("Process")?.GetCustomAttribute<DocumentedAttribute>();

Console.WriteLine($"Class: {typeAttr?.Description}");      // "Class-level doc"
Console.WriteLine($"Property: {propAttr?.Description}");   // "Property-level doc"
Console.WriteLine($"Method: {methodAttr?.Description}");   // "Method-level doc"
```

---

## 🔎 GetCustomAttributes() - All Attributes

```csharp
// Zwraca WSZYSTKIE atrybuty (ANY typ)

[Obsolete]
[Serializable]
[DebuggerDisplay("Test")]
public class MyClass { }

// Get all (untyped)
var allAttrs = typeof(MyClass).GetCustomAttributes();

Console.WriteLine($"Total attributes: {allAttrs.Length}");

foreach (var attr in allAttrs)
{
    Console.WriteLine($"  - {attr.GetType().Name}");
}
// Output:
//   - ObsoleteAttribute
//   - SerializableAttribute
//   - DebuggerDisplayAttribute
```

---

## 🎯 Filtrowanie Atrybutów

### Inherit Parameter

```csharp
// inherit = true → czytaj atrybuty z klas parent
// inherit = false → TYLKO bezpośrednio

[Documented("Base doc")]
public class Base { }

public class Derived : Base { }

// inherit = true
var attrs1 = typeof(Derived).GetCustomAttributes<DocumentedAttribute>(inherit: true);
Console.WriteLine($"With inherit: {attrs1.Length}");  // 1 (z Base)

// inherit = false
var attrs2 = typeof(Derived).GetCustomAttributes<DocumentedAttribute>(inherit: false);
Console.WriteLine($"Without inherit: {attrs2.Length}");  // 0 (brak bezpośrednio)
```

### Filter by Attribute Type

```csharp
var attrs = typeof(MyClass)
    .GetCustomAttributes()
    .OfType<ObsoleteAttribute>();

foreach (var attr in attrs)
{
    Console.WriteLine($"Obsolete: {attr.Message}");
}
```

---

## 📊 Ekstakcja Metadanych z Atrybutów

```csharp
[Serializable]
[Obsolete("Use NewClass instead", false)]
[DebuggerDisplay("{Name}")]
public class LegacyClass
{
    [Obsolete]
    public void OldMethod() { }
}

// Build metadata
var metadata = new Dictionary<string, object>();

// Type attributes
var obsoleteAttr = typeof(LegacyClass).GetCustomAttribute<ObsoleteAttribute>();
if (obsoleteAttr != null)
{
    metadata["Obsolete"] = true;
    metadata["ObsoleteMessage"] = obsoleteAttr.Message;
}

if (typeof(LegacyClass).GetCustomAttribute<SerializableAttribute>() != null)
    metadata["Serializable"] = true;

// Method attributes
var methods = typeof(LegacyClass).GetMethods();
foreach (var method in methods)
{
    var methodObsolete = method.GetCustomAttribute<ObsoleteAttribute>();
    if (methodObsolete != null)
    {
        var key = $"Method.{method.Name}.Obsolete";
        metadata[key] = true;
    }
}

Console.WriteLine("Metadata:");
foreach (var kvp in metadata)
    Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
```

---

## 🎪 Attribute Reader Pattern

```csharp
public class AttributeReader
{
    public Dictionary<string, List<object>> GetAllAttributes(Type type)
    {
        var result = new Dictionary<string, List<object>>();
        
        // Read type attributes
        result["Type"] = new(type.GetCustomAttributes());
        
        // Read property attributes
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            var key = $"Property.{prop.Name}";
            result[key] = new(prop.GetCustomAttributes());
        }
        
        // Read method attributes
        var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
        foreach (var method in methods)
        {
            var key = $"Method.{method.Name}";
            result[key] = new(method.GetCustomAttributes());
        }
        
        return result;
    }
    
    public void PrintAttributes(Dictionary<string, List<object>> attrs)
    {
        foreach (var kvp in attrs)
        {
            if (kvp.Value.Count > 0)
            {
                Console.WriteLine($"\n{kvp.Key}:");
                foreach (var attr in kvp.Value)
                    Console.WriteLine($"  - {attr.GetType().Name}");
            }
        }
    }
}

// Usage
var type = typeof(MyClass);
var reader = new AttributeReader();
var attrs = reader.GetAllAttributes(type);
reader.PrintAttributes(attrs);
```

---

## ⚡ Performance - Caching Attributes

```csharp
// ❌ BAD: Read attributes every time
void ProcessBad(Type type)
{
    var attrs = type.GetCustomAttributes<DocAttribute>();  // SLOW every time!
}

// ✅ GOOD: Cache attributes
private static readonly Dictionary<Type, DocAttribute[]> AttributeCache = new();

DocAttribute[] GetAttributesCached(Type type)
{
    if (!AttributeCache.TryGetValue(type, out var attrs))
    {
        attrs = type.GetCustomAttributes<DocAttribute>().ToArray();
        AttributeCache[type] = attrs;
    }
    return attrs;
}

void ProcessGood(Type type)
{
    var attrs = GetAttributesCached(type);  // FAST! (cached)
}
```

---

## 🔍 Advanced: Reflection + LINQ

```csharp
[Author("Alice"), Author("Bob")]
public class SharedProject { }

[Obsolete]
public class LegacyService { }

public class ModernService { }

//Find all classes with [Author]
var types = new[] { typeof(SharedProject), typeof(LegacyService), typeof(ModernService) };

var authorsClasses = types
    .Where(t => t.GetCustomAttribute<AuthorAttribute>() != null)
    .ToList();

var obsoleteClasses = types
    .Where(t => t.GetCustomAttribute<ObsoleteAttribute>() != null)
    .ToList();

Console.WriteLine($"Classes with [Author]: {authorsClasses.Count}");
Console.WriteLine($"Classes with [Obsolete]: {obsoleteClasses.Count}");
```

---

## 📋 IsDefined() - Fast Existence Check

```csharp
// Szybsza alternatywa do GetCustomAttribute != null

var type = typeof(SomeClass);

// Wolne
var has1 = type.GetCustomAttribute<SerializableAttribute>() != null;

// Szybsze
var has2 = type.IsDefined(typeof(SerializableAttribute));

// IsDefined jest szybszy w tight loops!
```

---

## 📚 Summary

**Czytanie Atrybutów:**

- `GetCustomAttribute<T>()` = jeden atrybut
- `GetCustomAttributes<T>()` = wszystkie atrybuty
- `GetCustomAttributes()` = ALL atrybuty (any type)
- `IsDefined()` = szybka sprawdzenie istnienia
- `inherit` = czy czytać z parent klas

**Best Practices:**
- Cache attributes - GetCustomAttributes powolne!
- Use IsDefined() dla sprawdzenia istnienia
- Iterate properties/methods dla głębokich reads
- Kombinuj z LINQ dla filtrowania

---

## 🎯 Następny Temat

Temat 7: Wbudowane Atrybuty .NET - [Obsolete], [Serializable], [Flags]
