# Ćwiczenia: Built-in .NET Attributes

## 🟢 Basic Level

### Zadanie 1: [Obsolete]
Oznacz metodę jako przestarzałą:

```csharp
// TODO: Mark OldMethod with [Obsolete]
public void OldMethod() { }
```

**Rozwiązanie:**
```csharp
[Obsolete("Use NewMethod instead")]
public void OldMethod() { }
```

---

### Zadanie 2: [Flags] Enum
Utwórz enum z [Flags]:

```csharp
// TODO: Add [Flags] and create bitwise permissions
public enum Permissions
{
    Read = 1,
    Write = 2,
    Delete = 4
}
```

**Rozwiązanie:**
```csharp
[Flags]
public enum Permissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
    All = Read | Write | Delete
}
```

---

### Zadanie 3: Check [Obsolete]
Czytaj [Obsolete] atrybut:

```csharp
// TODO: Check if method is obsolete
```

**Rozwiązanie:**
```csharp
var method = typeof(MyClass).GetMethod("OldMethod");
var obsolete = method?.GetCustomAttribute<ObsoleteAttribute>();
if (obsolete != null)
    Console.WriteLine($"Deprecated: {obsolete.Message}");
```

---

### Zadanie 4: [Flags] Output
Porównaj output z/bez [Flags]:

```csharp
var perms = Permissions.Read | Permissions.Write;
```

**Rozwiązanie:**
```
With [Flags]: Read, Write
Without [Flags]: 3
```

---

### Zadanie 5: [Required]
Dodaj walidację:

```csharp
public class User
{
    // TODO: Add [Required]
    public string Name { get; set; }
}
```

**Rozwiązanie:**
```csharp
[Required]
public string Name { get; set; }
```

---

## 🟡 Intermediate Level

### Zadanie 6: [Range]
Ogranicz wartości numeryczne:

```csharp
public class Product
{
    // TODO: Add [Range(1, 10000)]
    public int Price { get; set; }
}
```

**Rozwiązanie:**
```csharp
[Range(1, 10000)]
public int Price { get; set; }
```

---

### Zadanie 7: [EmailAddress]
Waliduj email:

```csharp
public class Contact
{
    // TODO: Add [EmailAddress]
    public string Email { get; set; }
}
```

**Rozwiązanie:**
```csharp
[EmailAddress]
public string Email { get; set; }
```

---

### Zadanie 8: [DebuggerDisplay]
Customize debug view:

```csharp
// TODO: Add [DebuggerDisplay]
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

**Rozwiązanie:**
```csharp
[DebuggerDisplay("{Name} ({Age})")]
public class Person { }
```

---

### Zadanie 9: [Description]
Dodaj opisy dla UI:

```csharp
public class Config
{
    // TODO: Add [Description]
    public string ConnectionString { get; set; }
}
```

**Rozwiązanie:**
```csharp
[Description("Database connection string")]
public string ConnectionString { get; set; }
```

---

### Zadanie 10: DataAnnotations Validation
Zbierz atrybuty walidacyjne:

```csharp
public class User
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}

// TODO: Read all validation attributes
```

**Rozwiązanie:**
```csharp
var nameProps = typeof(User).GetProperty("Name");
var required = nameProps?.GetCustomAttribute<RequiredAttribute>();
var length = nameProps?.GetCustomAttribute<StringLengthAttribute>();

Console.WriteLine($"Has [Required]: {required != null}");
Console.WriteLine($"Max length: {length?.MaximumLength}");
```

---

## 🔴 Advanced Level

### Zadanie 11: [Conditional] Compilation
Utwórz metody warunkowe:

```csharp
public class Logger
{
    // TODO: Only in DEBUG
    public void DebugLog(string msg) { }
    
    // TODO: Only in RELEASE
    public void ProductionLog(string msg) { }
}
```

**Rozwiązanie:**
```csharp
[Conditional("DEBUG")]
public void DebugLog(string msg) { }

[Conditional("RELEASE")]
public void ProductionLog(string msg) { }
```

---

### Zadanie 12: Reflection Attribute Inspector
Stwórz tool do inspekcji wbudowanych atrybutów:

```csharp
public class BuiltInAttributeInspector
{
    // TODO: Find all built-in validation attributes on type
    public List<string> FindValidationAttributes(Type type)
    {
        // Find [Required], [Range], [EmailAddress], etc.
    }
}
```

**Rozwiązanie:**
```csharp
public List<string> FindValidationAttributes(Type type)
{
    var result = new List<string>();
    
    var validationTypes = new[]
    {
        typeof(RequiredAttribute),
        typeof(RangeAttribute),
        typeof(EmailAddressAttribute),
        typeof(StringLengthAttribute)
    };
    
    foreach (var prop in type.GetProperties())
    {
        foreach (var valType in validationTypes)
        {
            if (prop.GetCustomAttribute(valType) != null)
                result.Add($"{prop.Name}: {valType.Name.Replace("Attribute", "")}");
        }
    }
    
    return result;
}
```

---

## 📊 Wskazówki

- ✅ Używaj [Obsolete] zamiast usuwać kod
- ✅ [Flags] dla bitwise enums
- ✅ [Required], [Range], [EmailAddress] dla walidacji
- ✅ [Conditional] zamiast #if directives
- ✅ [DebuggerDisplay] dla lepszej debug experience
- ❌ Nie ignoruj compiler warnings z [Obsolete]
- ❌ Nie mieszaj [Flags] z regular enums
- ❌ Nie zapomnij import System.ComponentModel.DataAnnotations

---

## 🎯 Key Takeaways

Built-in Attributes:

```
[Obsolete] - Mark deprecated
[Flags] - Bitwise enum operations
[Required] - Validation
[Range(min,max)] - Numeric range
[EmailAddress] - Email validation
[StringLength(n)] - Length validation
[Conditional] - Compilation conditional
[DebuggerDisplay] - Debug visualization
[Description] - UI descriptions
[ThreadStatic] - Thread-local storage
[Serializable] - Serialization control
```

Pamiętaj: **.NET ma atrybuty na prawie wszystko!**
