# Ćwiczenia: Advanced Attributes

## 🟢 Basic Level

### Zadanie 1: Attribute Inheritance
Utwórz bazowy atrybut i pochodny:

```csharp
// TODO: Create BaseDoc and AdvDoc inheriting from Attribute
```

**Rozwiązanie:**
```csharp
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class BaseDocAttribute : Attribute
{
    public BaseDocAttribute(string desc) => Description = desc;
    public string Description { get; }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class AdvDocAttribute : BaseDocAttribute
{
    public AdvDocAttribute(string desc) : base(desc) { }
    public string? Author { get; set; }
}

[AdvDoc("User", Author = "Alice")]
public class User { }
```

---

### Zadanie 2: Attribute Stacking
Stack wiele atrybutów na klasie:

```csharp
[Author("Bob")]
[Version("1.0")]
[License("MIT")]
public class Library { }

// TODO: Read all 3 attributes
```

**Rozwiązanie:**
```csharp
var author = typeof(Library).GetCustomAttribute<AuthorAttribute>();
var version = typeof(Library).GetCustomAttribute<VersionAttribute>();
var license = typeof(Library).GetCustomAttribute<LicenseAttribute>();

Console.WriteLine($"Author: {author?.Name}");
Console.WriteLine($"Version: {version?.Version}");
Console.WriteLine($"License: {license?.License}");
```

---

### Zadanie 3: AllowMultiple
Użyj tego samego atrybutu wielokrotnie:

```csharp
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequirementAttribute : Attribute
{
    public RequirementAttribute(string req) => Requirement = req;
    public string Requirement { get; }
}

// TODO: Apply 3x [Requirement]
```

**Rozwiązanie:**
```csharp
[Requirement("Immutable")]
[Requirement("Serializable")]
[Requirement("Thread-safe")]
public class Config { }
```

---

### Zadanie 4: Property Validation
Dodaj [Range] i [Pattern] atrybuty:

```csharp
public class Product
{
    [Range(1, 10000)]
    public int Price { get; set; }
    
    [Pattern(@"^[A-Z0-9]+$")]
    public string Sku { get; set; }
}
```

---

### Zadanie 5: GetCustomAttributes
Czytaj wielokrotne atrybuty:

```csharp
[Tag("important")]
[Tag("production")]
[Tag("urgent")]
public class CriticalTask { }

// TODO: Read all tags
```

**Rozwiązanie:**
```csharp
var tags = typeof(CriticalTask).GetCustomAttributes<TagAttribute>();

foreach (var tag in tags)
    Console.WriteLine($"Tag: {tag.Name}");
```

---

## 🟡 Intermediate Level

### Zadanie 6: Advanced Validator
Zbuduj validator dla [Range] i [Pattern]:

```csharp
public class Product
{
    [Range(1, 5000)]
    public int Price { get; set; }
    
    [Pattern(@"^[A-Z0-9\-]+$")]
    public string Sku { get; set; }
}

// TODO: Create validator that checks both constraints
```

**Rozwiązanie:**
```csharp
public class ConstraintValidator
{
    public bool Validate(object obj, out List<string> errors)
    {
        errors = new();
        var type = obj.GetType();
        
        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(obj);
            
            var rangeAttr = prop.GetCustomAttribute<RangeAttribute>();
            if (rangeAttr != null && value is int intVal)
            {
                if (intVal < rangeAttr.Min || intVal > rangeAttr.Max)
                    errors.Add($"{prop.Name}: {intVal} out of range");
            }
            
            var patternAttr = prop.GetCustomAttribute<PatternAttribute>();
            if (patternAttr != null && value is string strVal)
            {
                if (!Regex.IsMatch(strVal, patternAttr.Pattern))
                    errors.Add($"{prop.Name}: Invalid format");
            }
        }
        
        return errors.Count == 0;
    }
}
```

---

### Zadanie 7: ORM Metadata
Ekstrakcja metadanych dla [Table] i [Column]:

```csharp
[Table("users")]
public class User
{
    [Column("id", IsPrimaryKey = true)]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}

// TODO: Extract table name and column info
```

**Rozwiązanie:**
```csharp
var type = typeof(User);
var tableAttr = type.GetCustomAttribute<TableAttribute>();
Console.WriteLine($"Table: {tableAttr?.TableName}");

foreach (var prop in type.GetProperties())
{
    var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
    if (colAttr != null)
    {
        Console.WriteLine($"  Column: {colAttr.ColumnName}");
        Console.WriteLine($"    PrimaryKey: {colAttr.IsPrimaryKey}");
    }
}
```

---

### Zadanie 8: Composition over Inheritance
Zamiast inherit, stackuj atrybuty:

```csharp
// BAD: Attribute inheritance
public class AdvancedAttr : BaseAttr { }

// GOOD: Composition
[Base("text")]
[Extended("more")]
public class MyClass { }

// TODO: Explain why composition better
```

**Rozwiązanie:**
```
Composition korzyści:
1. Mniej coupling
2. Większa elastyczność
3. Łatwiej mockować
4. Mniej złożoności
5. Łatwiej testować
```

---

### Zadanie 9: Inherited = true/false
Testuj dziedziczenie atrybutów:

```csharp
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class InheritedAttr : Attribute { }

[Inherited]
public class Base { }

public class Derived : Base { }

// TODO: Check if Derived has [Inherited]
```

**Rozwiązanie:**
```csharp
var attr = typeof(Derived).GetCustomAttribute<InheritedAttr>();
Console.WriteLine(attr != null);  // true (dziedziczy!)
```

---

### Zadanie 10: Attribute Scope Validator
Sprawdzaj, że atrybuty są użyte na właściwych targets:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class CacheableAttribute : Attribute { }

// TODO: Validate that [Cacheable] not used on fields
```

**Rozwiązanie:**
```csharp
public bool ValidateUsage(Type type)
{
    var fields = type.GetFields();
    foreach (var field in fields)
    {
        if (field.GetCustomAttribute<CacheableAttribute>() != null)
        {
            Console.WriteLine($"ERROR: {field.Name} cannot be [Cacheable]");
            return false;
        }
    }
    return true;
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Security Attributes Framework
Utwórz [RequiresPermission] i [RateLimit]:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class RequiresPermissionAttribute : Attribute
{
    public RequiresPermissionAttribute(string perm) => Permission = perm;
    public string Permission { get; }
}

[AttributeUsage(AttributeTargets.Method)]
public class RateLimitAttribute : Attribute
{
    public RateLimitAttribute(int rpm) => RequestsPerMin = rpm;
    public int RequestsPerMin { get; }
}

public class AdminPanel
{
    [RequiresPermission("admin.delete")]
    [RateLimit(10)]
    public void DeleteUser(int id) { }
}

// TODO: Create security handler
```

**Rozwiązanie:**
```csharp
public class SecurityHandler
{
    public bool CanAccess(Type type, string method, string userPerm, int rateLimit)
    {
        var methodInfo = type.GetMethod(method);
        var permAttr = methodInfo?.GetCustomAttribute<RequiresPermissionAttribute>();
        var rateLimitAttr = methodInfo?.GetCustomAttribute<RateLimitAttribute>();
        
        if (permAttr != null && userPerm != permAttr.Permission)
            return false;
        
        if (rateLimitAttr != null && rateLimit > rateLimitAttr.RequestsPerMin)
            return false;
        
        return true;
    }
}
```

---

### Zadanie 12: Dynamic Attribute-Driven Configuration
Stwórz system konfiguracji oparty na atrybutach:

```csharp
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ConfigurableAttribute : Attribute
{
    public ConfigurableAttribute(string key, object? defaultValue = null)
    {
        Key = key;
        DefaultValue = defaultValue;
    }
    
    public string Key { get; }
    public object? DefaultValue { get; }
}

public class AppConfig
{
    [Configurable("app.name", "MyApp")]
    public string AppName { get; set; } = "";
    
    [Configurable("app.version", "1.0")]
    public string Version { get; set; } = "";
}

// TODO: Create config builder from attributes
```

**Rozwiązanie:**
```csharp
public class ConfigBuilder
{
    public Dictionary<string, object?> BuildConfig(Type type)
    {
        var config = new Dictionary<string, object?>();
        
        foreach (var prop in type.GetProperties())
        {
            var attrs = prop.GetCustomAttributes<ConfigurableAttribute>();
            
            foreach (var attr in attrs)
            {
                config[attr.Key] = attr.DefaultValue;
            }
        }
        
        return config;
    }
}
```

---

## 📊 Wskazówki

- ✅ Composition > Inheritance dla atrybutów
- ✅ AllowMultiple dla logicznych grup
- ✅ Waliduj attribute usage runtime
- ✅ Inherited = true dla dziedziczenia
- ✅ Cache metadata results
- ✅ Używaj regex dla [Pattern]
- ❌ Nie zapomnij error handling
- ❌ Nie ignoruj performance impact
- ❌ Nie ufaj user-defined attribute names

---

## 🎯 Key Takeaways

Advanced Attributes:

```
Inheritance: Hierarchie atrybutów
Composition: Stackowanie atrybutów
AllowMultiple: Wielokrotne użycie
Validation: Sprawdzenie constraints
Metadata: Ekstrakcja informacji
Security: Kontrola dostępu
ORM: Mapowanie bazy danych
```

Pamiętaj: **Atrybuty to potęga — używaj mądrze!**
