# Temat 4: Tworzenie Własnych Atrybutów - Custom Attributes Part 1

## 🏷️ Czym są Atrybuty?

**Atrybut** = metadane dołączone do kodu, które można czytać w runtime refleksją

```csharp
// Wbudowany atrybut (Obsolete)
[Obsolete("Use NewMethod instead")]
public void OldMethod() { }

// Twój własny atrybut
[CustomAuthor("Alice")]
public class MyClass { }

// Czytaj w runtime
var attr = typeof(MyClass).GetCustomAttribute<CustomAuthorAttribute>();
Console.WriteLine($"Author: {attr.Name}");  // "Alice"
```

---

## 📋 AttributeUsage - Kontrola Jak Używać Atrybutu

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name) => Name = name;
    public string Name { get; }
}

// ✅ OK: Class
[Author("Alice")]
public class MyClass { }

// ❌ ERROR: Method (AttributeTargets.Class tylko dla klas)
[Author("Bob")]
public void MyMethod() { }
```

---

## 🎯 AttributeTargets - Gdzie Użyć?

```csharp
// Tylko dla klas
[AttributeUsage(AttributeTargets.Class)]
public class OnlyClassAttribute : Attribute { }

// Tylko dla metod
[AttributeUsage(AttributeTargets.Method)]
public class OnlyMethodAttribute : Attribute { }

// Tylko dla properties
[AttributeUsage(AttributeTargets.Property)]
public class OnlyPropertyAttribute : Attribute { }

// Tylko dla pól
[AttributeUsage(AttributeTargets.Field)]
public class OnlyFieldAttribute : Attribute { }

// Tylko dla parametrów
[AttributeUsage(AttributeTargets.Parameter)]
public class OnlyParameterAttribute : Attribute { }

// Kombinuj! Dla klas i metod
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BothAttribute : Attribute { }

// Wszędzie (ALL)
[AttributeUsage(AttributeTargets.All)]
public class EverywhereAttribute : Attribute { }
```

### Pełna Lista AttributeTargets

```
All                  // Wszędzie
Assembly             // Na Assembly
Module               // Na Module
Class                // Na Class
Struct               // Na Struct
Enum                 // Na Enum
Constructor          // Na Constructor
Method               // Na Method
Property             // Na Property
Field                // Na Field
Event                // Na Event
Interface            // Na Interface
Parameter            // Na Parameter
Delegate             // Na Delegate
ReturnValue          // Na wartości zwracanej
GenericParameter     // Na generic parameter
```

---

## 🏗️ Tworzenie Custom Attributu - Podstawowe

### Krok 1: Dziedzicz z Attribute

```csharp
// Najprostszy możliwy atrybut
public class SimpleAttribute : Attribute
{
}

// Użycie
[Simple]
public class MyClass { }
```

### Krok 2: Dodaj Constructor

```csharp
public class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
}

// Użycie
[Author("Alice")]
public class MyClass { }
```

### Krok 3: Dodaj AttributeUsage

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name) => Name = name;
    public string Name { get; }
}

// OK: Class i Method
[Author("Alice")]
public class MyClass { }

[Author("Bob")]
public void MyMethod() { }

// ERROR: Property nie w AttributeTargets
[Author("Charlie")]  // ❌
public string MyProperty { get; set; }
```

---

## 🔧 Named Parameters (Właściwości Atrybutu)

### Constructora vs Named Parameters

```csharp
// Constructor parameters
[AttributeUsage(AttributeTargets.All)]
public class DescriptionAttribute : Attribute
{
    public DescriptionAttribute(string description)
    {
        Description = description;
    }
    
    public string Description { get; }
    
    // Named parameter
    public string Version { get; set; }
}

// Użycie z constructor i named parameter
[Description("My class", Version = "1.0")]
public class MyClass { }
```

---

## 📝 Przykład: Documentation Atrybut

```csharp
[AttributeUsage(
    AttributeTargets.Class | 
    AttributeTargets.Method | 
    AttributeTargets.Property)]
public class DocumentationAttribute : Attribute
{
    // Constructor parameter (obowiązkowy)
    public DocumentationAttribute(string summary)
    {
        Summary = summary;
    }
    
    public string Summary { get; }
    
    // Named parameters (opcjonalne)
    public string? Author { get; set; }
    public string? Version { get; set; }
    public string? Remarks { get; set; }
}

// Użycie
[Documentation("Main user class",
    Author = "Alice",
    Version = "2.0",
    Remarks = "Active in production")]
public class User
{
    [Documentation("User identifier")]
    public int Id { get; set; }
    
    [Documentation("Full name", Version = "1.5")]
    public string Name { get; set; } = "";
}
```

---

## ✔️ Validation Atrybut - Praktyczne Zastosowanie

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    public string Message { get; set; } = "Field is required";
}

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public MaxLengthAttribute(int length) => Length = length;
    public int Length { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class EmailAttribute : Attribute
{
}

// Model z walidacją
[Documentation("User model", Author = "Alice")]
public class User
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";
    
    [Required]
    [Email]
    public string Email { get; set; } = "";
    
    public int Age { get; set; }
}

// Validator używający atrybutów
public class Validator
{
    public bool Validate(object obj, out List<string> errors)
    {
        errors = new();
        var type = obj.GetType();
        
        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(obj);
            
            // Check [Required]
            if (property.GetCustomAttribute<RequiredAttribute>() != null)
            {
                if (string.IsNullOrEmpty(value?.ToString()))
                {
                    errors.Add($"{property.Name} is required");
                }
            }
            
            // Check [MaxLength]
            var maxLenAttr = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLenAttr != null && value is string str)
            {
                if (str.Length > maxLenAttr.Length)
                {
                    errors.Add($"{property.Name} exceeds {maxLenAttr.Length} characters");
                }
            }
            
            // Check [Email]
            if (property.GetCustomAttribute<EmailAttribute>() != null && value is string email)
            {
                if (!email.Contains("@"))
                {
                    errors.Add($"{property.Name} is not a valid email");
                }
            }
        }
        
        return errors.Count == 0;
    }
}

// Użycie
var user = new User { Name = "Alice", Email = "alice" };
var validator = new Validator();

if (!validator.Validate(user, out var errors))
{
    foreach (var error in errors)
        Console.WriteLine($"✗ {error}");
}
```

---

## 🔄 AllowMultiple - Wielokrotne Użycie

```csharp
// Zezwól na wielokrotne użycie
[AttributeUsage(
    AttributeTargets.Class,
    AllowMultiple = true)]
public class TagAttribute : Attribute
{
    public TagAttribute(string tag) => Tag = tag;
    public string Tag { get; }
}

// Użycie - ten sam atrybut wielokrotnie
[Tag("important")]
[Tag("production")]
[Tag("optimized")]
public class CriticalClass { }

// Czytanie wszystkich
var tags = typeof(CriticalClass)
    .GetCustomAttributes<TagAttribute>();

foreach (var tag in tags)
    Console.WriteLine($"Tag: {tag.Tag}");
// Output:
// Tag: important
// Tag: production
// Tag: optimized
```

---

## 🎯 Inherited - Dziedziczenie Atrybutów

```csharp
// Pozwól na dziedziczenie w klasy pochodne
[AttributeUsage(
    AttributeTargets.Class,
    Inherited = true)]
public class MarkedAttribute : Attribute
{
    public MarkedAttribute(string mark) => Mark = mark;
    public string Mark { get; }
}

[Marked("base")]
public class Base { }

public class Derived : Base { }

// Derived dziedziczy [Marked] od Base
var attr = typeof(Derived).GetCustomAttribute<MarkedAttribute>();
Console.WriteLine(attr?.Mark);  // "base"

// Inherited = false (domyślnie)
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NonInheritedAttribute : Attribute { }

[NonInherited]
public class Base2 { }

public class Derived2 : Base2 { }

// Derived2 NIE dziedziczy [NonInherited]
var attr2 = typeof(Derived2).GetCustomAttribute<NonInheritedAttribute>();
Console.WriteLine(attr2 == null);  // true
```

---

## 🔒 Atrybut Bezpieczeństwa

```csharp
[AttributeUsage(
    AttributeTargets.Method |
    AttributeTargets.Class)]
public class RequiresRoleAttribute : Attribute
{
    public RequiresRoleAttribute(string role) => Role = role;
    public string Role { get; }
}

[RequiresRole("Admin")]
public class AdminPanel { }

[RequiresRole("User")]
public void UserMethod() { }

// Security check
public bool CanAccess(object obj, string userRole)
{
    var type = obj.GetType();
    var attr = type.GetCustomAttribute<RequiresRoleAttribute>();
    
    if (attr == null)
        return true;  // No security required
    
    return userRole == attr.Role;
}
```

---

## 📝 Summary

**Atrybut** = metadane w runtime

**Kroki tworzenia:**
1. Dziedzicz z Attribute
2. Dodaj AttributeUsage
3. Dodaj constructor
4. Dodaj properties (named parameters)

**AttributeTargets** = gdzie można użyć (Class, Method, Property, etc.)

**AllowMultiple** = czy można użyć wiele razy

**Inherited** = czy dziedziczą się w klasach pochodnych

**Use Cases:**
- Validation
- Documentation
- Security
- ORM mapping
- Serialization control

---

## 🎯 Następny Temat

Temat 5: Zaawansowane Atrybuty - Hierarchia, Pattern Matching, Attribute Walory
