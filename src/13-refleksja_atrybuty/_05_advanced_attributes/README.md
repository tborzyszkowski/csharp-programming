# Temat 5: Zaawansowane Atrybuty - Custom Attributes Part 2

## 🎯 Zaawansowane Wzorce Atrybutów

**Part 1** poznałeś: tworzenie, AttributeUsage, AttributeTargets, walidacja.

**Part 2** = zaawansowane scenariusze: hierarchie, kompozycja, warunkowość, złożone pattern.

---

## 🔗 Hierarchia Atrybutów - Inheritance

```csharp
// Bazowy atrybut
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class BaseDocumentationAttribute : Attribute
{
    public BaseDocumentationAttribute(string description) => Description = description;
    public string Description { get; }
}

// Pochodny atrybut
[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class AdvancedDocumentationAttribute : BaseDocumentationAttribute
{
    public AdvancedDocumentationAttribute(string description) : base(description) { }
    
    public string? Author { get; set; }
    public string? Version { get; set; }
}

// Użycie
[AdvancedDocumentation("User class",
    Author = "Alice",
    Version = "2.0")]
public class User { }

// Czytaj bazowy atrybut
var baseAttr = typeof(User).GetCustomAttribute<BaseDocumentationAttribute>();
Console.WriteLine(baseAttr?.Description);  // "User class"
```

---

## 🏛️ Composition Over Inheritance

```csharp
// Nie dziedzicz - komponuj!

[AttributeUsage(AttributeTargets.Class)]
public class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name) => Name = name;
    public string Name { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttribute : Attribute
{
    public VersionAttribute(string version) => Version = version;
    public string Version { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class LicenseAttribute : Attribute
{
    public LicenseAttribute(string license) => License = license;
    public string License { get; }
}

// Użycie - stack atrybutów
[Author("Bob")]
[Version("3.5")]
[License("MIT")]
public class Library { }

// Czytaj wszystkie
var author = typeof(Library).GetCustomAttribute<AuthorAttribute>();
var version = typeof(Library).GetCustomAttribute<VersionAttribute>();
var license = typeof(Library).GetCustomAttribute<LicenseAttribute>();

Console.WriteLine($"Author: {author?.Name}");
Console.WriteLine($"Version: {version?.Version}");
Console.WriteLine($"License: {license?.License}");
```

---

## 🔀 Conditional Attributes - Warunkowość

```csharp
// Atrybut, który stosuje się tylko pod warunkami

[AttributeUsage(AttributeTargets.Method)]
public class PerformanceRequirementAttribute : Attribute
{
    public PerformanceRequirementAttribute(int maxMilliseconds) => MaxMs = maxMilliseconds;
    public int MaxMs { get; }
}

[AttributeUsage(AttributeTargets.Method)]
public class DeprecatedIfAttribute : Attribute
{
    public DeprecatedIfAttribute(string condition) => Condition = condition;
    public string Condition { get; }  // "NET6_OR_LESS", "DEBUG_ONLY", etc.
}

// Użycie
public class DataService
{
    [PerformanceRequirement(maxMilliseconds: 100)]
    public void FetchData() { }
    
    [DeprecatedIf(condition: "NET6_OR_LESS")]
    public void LegacyMethod() { }
}
```

---

## 🎨 Attribute Stacking & Composition

```csharp
// Wielokrotne użycie tego samego atrybutu

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class RequirementAttribute : Attribute
{
    public RequirementAttribute(string requirement) => Requirement = requirement;
    public string Requirement { get; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ValidatorAttribute : Attribute
{
    public ValidatorAttribute(Type validatorType) => ValidatorType = validatorType;
    public Type ValidatorType { get; }
}

// Użycie - stack atrybutów tego samego typu
[Requirement("Must be immutable")]
[Requirement("Must be serializable")]
[Requirement("Must have validation")]
[Validator(typeof(EmailValidator))]
[Validator(typeof(PhoneValidator))]
public class ContactInfo { }

// Czytaj wszystkie
var requirements = typeof(ContactInfo).GetCustomAttributes<RequirementAttribute>();
var validators = typeof(ContactInfo).GetCustomAttributes<ValidatorAttribute>();

Console.WriteLine("Requirements:");
foreach (var req in requirements)
    Console.WriteLine($"  - {req.Requirement}");

Console.WriteLine("Validators:");
foreach (var val in validators)
    Console.WriteLine($"  - {val.ValidatorType.Name}");
```

---

## 🛡️ Attribute Filters & Validators

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public RangeAttribute(int min, int max) 
    { 
        Min = min;
        Max = max;
    }
    public int Min { get; }
    public int Max { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class PatternAttribute : Attribute
{
    public PatternAttribute(string pattern) => Pattern = pattern;
    public string Pattern { get; }  // Regex
}

// Model
public class Product
{
    [Range(1, 10000)]
    public int Price { get; set; }
    
    [Range(1, 1000)]
    public int Quantity { get; set; }
    
    [Pattern(@"^[A-Z0-9\-]+$")]
    public string Sku { get; set; } = "";
}

// Advanced Validator
public class ConstraintValidator
{
    public bool Validate(object obj, out List<string> errors)
    {
        errors = new();
        var type = obj.GetType();
        
        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(obj);
            
            // Check [Range]
            var rangeAttr = property.GetCustomAttribute<RangeAttribute>();
            if (rangeAttr != null && value is int intVal)
            {
                if (intVal < rangeAttr.Min || intVal > rangeAttr.Max)
                    errors.Add($"{property.Name}: Must be between {rangeAttr.Min}-{rangeAttr.Max}");
            }
            
            // Check [Pattern]
            var patternAttr = property.GetCustomAttribute<PatternAttribute>();
            if (patternAttr != null && value is string strVal)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(strVal, patternAttr.Pattern))
                    errors.Add($"{property.Name}: Must match pattern {patternAttr.Pattern}");
            }
        }
        
        return errors.Count == 0;
    }
}
```

---

## 🔍 Metadata Extraction - Kompleksne Scenariusze

```csharp
// Buildowanie bazy danych z atrybutów

[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public TableAttribute(string tableName) => TableName = tableName;
    public string TableName { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public ColumnAttribute(string columnName) => ColumnName = columnName;
    public string ColumnName { get; }
    public bool IsPrimaryKey { get; set; }
    public bool IsNullable { get; set; } = true;
}

// Model
[Table("Users")]
public class User
{
    [Column("user_id", IsPrimaryKey = true)]
    public int Id { get; set; }
    
    [Column("full_name", IsNullable = false)]
    public string Name { get; set; } = "";
    
    [Column("email_address")]
    public string Email { get; set; } = "";
}

// ORM-like metadata builder
public class MetadataBuilder
{
    public Dictionary<string, object> GetTableMetadata(Type type)
    {
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        if (tableAttr == null)
            throw new InvalidOperationException("Type must have [Table]");
        
        var metadata = new Dictionary<string, object>
        {
            { "TableName", tableAttr.TableName },
            { "Properties", new List<Dictionary<string, object>>() }
        };
        
        var properties = (List<Dictionary<string, object>>)metadata["Properties"]!;
        
        foreach (var prop in type.GetProperties())
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr == null)
                continue;
            
            properties.Add(new()
            {
                { "PropertyName", prop.Name },
                { "ColumnName", columnAttr.ColumnName },
                { "Type", prop.PropertyType.Name },
                { "IsPrimaryKey", columnAttr.IsPrimaryKey },
                { "IsNullable", columnAttr.IsNullable }
            });
        }
        
        return metadata;
    }
}
```

---

## 🎪 Attribute Scope & Targets Validation

```csharp
// Waliduj, że atrybut jest użyty poprawnie

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
public class CacheableAttribute : Attribute
{
    public CacheableAttribute(int durationSeconds) => DurationSeconds = durationSeconds;
    public int DurationSeconds { get; }
}

[AttributeUsage(AttributeTargets.Parameter)]
public class ValidatedParameterAttribute : Attribute
{
    public ValidatedParameterAttribute(string rule) => Rule = rule;
    public string Rule { get; }
}

// Validator
public class AttributeUsageValidator
{
    public bool ValidateUsage(Type type)
    {
        // Sprawdzaj, czy [Cacheable] nie jest na fields
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<CacheableAttribute>() != null)
            {
                Console.WriteLine($"❌ {field.Name}: [Cacheable] nie może być na polu!");
                return false;
            }
        }
        
        // Sprawdzaj, czy [CacheableAttribute] ma sense
        var methods = type.GetMethods();
        foreach (var method in methods)
        {
            var cacheAttr = method.GetCustomAttribute<CacheableAttribute>();
            if (cacheAttr != null && method.ReturnType == typeof(void))
            {
                Console.WriteLine($"❌ {method.Name}: Nie można cache'ować void metody!");
                return false;
            }
        }
        
        return true;
    }
}
```

---

## 🔐 Security Attributes

```csharp
// Atrybuty do kontroli dostępu

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequiresPermissionAttribute : Attribute
{
    public RequiresPermissionAttribute(string permission) => Permission = permission;
    public string Permission { get; }
    public bool ThrowException { get; set; } = true;
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RateLimitAttribute : Attribute
{
    public RateLimitAttribute(int requestsPerSecond) => RequestsPerSecond = requestsPerSecond;
    public int RequestsPerSecond { get; }
}

// Użycie
public class AdminPanel
{
    [RequiresPermission("admin.write", ThrowException = true)]
    [RateLimit(requestsPerSecond: 10)]
    public void DeleteUser(int userId) { }
    
    [RequiresPermission("admin.read", ThrowException = false)]
    [RateLimit(requestsPerSecond: 100)]
    public void ListUsers() { }
}

// Authorization handler
public class AuthorizationHandler
{
    public bool CanAccess(Type type, string methodName, string userPermission)
    {
        var method = type.GetMethod(methodName);
        if (method == null)
            return false;
        
        var permAttr = method.GetCustomAttribute<RequiresPermissionAttribute>();
        if (permAttr == null)
            return true;  // Brak wymagań
        
        bool hasPermission = userPermission == permAttr.Permission;
        
        if (!hasPermission && permAttr.ThrowException)
            throw new UnauthorizedAccessException($"Missing permission: {permAttr.Permission}");
        
        return hasPermission;
    }
}
```

---

## 📚 Summary

**Zaawansowane Atrybuty:**

- **Hierarchia** = atrybuty dziedziczące od siebie
- **Composition** = stacking atrybutów zamiast inherit
- **Conditional** = atrybuty z warunkami
- **AllowMultiple** = wielokrotne użycie
- **Metadata** = ekstrakcja złożonych informacji
- **Validation** = sprawdzanie poprawności użycia
- **Security** = kontrola dostępu z atrybutami

**Best Practices:**
- Prefer composition over inheritance
- Użyj AllowMultiple dla logicznych grup
- Waliduj attribute usage runtime
- Cache metadata results
- Dokumentuj scope atrybutów

---

## 🎯 Następny Temat

Temat 6: Czytanie Atrybutów - GetCustomAttribute(s), filtrowanie, performance
