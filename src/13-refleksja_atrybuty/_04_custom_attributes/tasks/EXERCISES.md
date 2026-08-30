# Ćwiczenia: Custom Attributes

## 🟢 Basic Level

### Zadanie 1: Create Simple Attribute
Utwórz najprostszy możliwy atrybut:

```csharp
// TODO: Create attribute inheriting from Attribute
```

**Rozwiązanie:**
```csharp
public class SimpleAttribute : Attribute
{
}

[Simple]
public class MyClass { }
```

---

### Zadanie 2: Add Constructor
Dodaj konstruktor do atrybutu:

```csharp
// TODO: Create Author attribute with name parameter
```

**Rozwiązanie:**
```csharp
public class AuthorAttribute : Attribute
{
    public AuthorAttribute(string name) => Name = name;
    public string Name { get; }
}

[Author("Alice")]
public class MyClass { }
```

---

### Zadanie 3: AttributeUsage - Class Only
Ogranicz użycie do klas:

```csharp
// TODO: Add AttributeUsage restricting to Class only
```

**Rozwiązanie:**
```csharp
[AttributeUsage(AttributeTargets.Class)]
public class ClassOnlyAttribute : Attribute { }

[ClassOnly]           // ✓ OK
public class MyClass { }

// [ClassOnly]       // ✗ ERROR
// public void MyMethod() { }
```

---

### Zadanie 4: Multiple Targets
Zezwól na klasy i metody:

```csharp
// TODO: Create attribute for Class and Method
```

**Rozwiązanie:**
```csharp
[AttributeUsage(
    AttributeTargets.Class | 
    AttributeTargets.Method)]
public class DocumentedAttribute : Attribute { }
```

---

### Zadanie 5: Get Attribute
Czytaj atrybut refleksją:

```csharp
[Author("Alice")]
public class MyClass { }

// TODO: Get Author attribute and print name
```

**Rozwiązanie:**
```csharp
var attr = typeof(MyClass).GetCustomAttribute<AuthorAttribute>();
Console.WriteLine(attr?.Name);  // "Alice"
```

---

## 🟡 Intermediate Level

### Zadanie 6: Named Parameters
Dodaj named parameters (opcjonalne właściwości):

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class DocAttribute : Attribute
{
    public DocAttribute(string summary) => Summary = summary;
    public string Summary { get; }
    
    // TODO: Add Version and Author as properties
}

// Usage
[Doc("My class", Version = "1.0", Author = "Bob")]
public class MyClass { }
```

**Rozwiązanie:**
```csharp
public string? Version { get; set; }
public string? Author { get; set; }
```

---

### Zadanie 7: AllowMultiple
Pozwól na wielokrotne użycie tego samego atrybutu:

```csharp
// TODO: Create Tag attribute with AllowMultiple = true
```

**Rozwiązanie:**
```csharp
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TagAttribute : Attribute
{
    public TagAttribute(string tag) => Tag = tag;
    public string Tag { get; }
}

[Tag("important")]
[Tag("production")]
public class MyClass { }
```

---

### Zadanie 8: Get All Attributes
Czytaj wiele atrybutów tego samego typu:

```csharp
[Tag("a")]
[Tag("b")]
[Tag("c")]
public class TaggedClass { }

// TODO: Get all Tag attributes
```

**Rozwiązanie:**
```csharp
var tags = typeof(TaggedClass).GetCustomAttributes<TagAttribute>();

foreach (var tag in tags)
{
    Console.WriteLine(tag.Tag);  // "a", "b", "c"
}
```

---

### Zadanie 9: Validation Attribute
Utwórz [Required] i [MaxLength] atrybuty:

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public MaxLengthAttribute(int length) => Length = length;
    public int Length { get; }
}

public class User
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}
```

---

### Zadanie 10: Inherited = false
Sprawdź czy atrybuty dziedziczy się lub nie:

```csharp
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NonInheritedAttribute : Attribute { }

[NonInherited]
public class Base { }

public class Derived : Base { }

// TODO: Check if Derived has [NonInherited]
```

**Rozwiązanie:**
```csharp
var attr = typeof(Derived).GetCustomAttribute<NonInheritedAttribute>();
Console.WriteLine(attr == null);  // true (nie dziedziczy!)
```

---

## 🔴 Advanced Level

### Zadanie 11: Validator Framework
Stwórz validator, który czyta atrybuty walidacji:

```csharp
public class Person
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [Email]
    public string Email { get; set; }
}

// TODO: Create validator that checks all validation attributes
```

**Rozwiązanie:**
```csharp
public class Validator
{
    public bool Validate(object obj, out List<string> errors)
    {
        errors = new();
        var type = obj.GetType();
        
        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(obj);
            
            // Check [Required]
            if (prop.GetCustomAttribute<RequiredAttribute>() != null)
            {
                if (string.IsNullOrEmpty(value?.ToString()))
                    errors.Add($"{prop.Name} is required");
            }
            
            // Check [MaxLength]
            var maxLen = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLen != null && value is string str)
            {
                if (str.Length > maxLen.Length)
                    errors.Add($"{prop.Name} exceeds {maxLen.Length} chars");
            }
            
            // Check [Email]
            if (prop.GetCustomAttribute<EmailAttribute>() != null && value is string email)
            {
                if (!email.Contains("@"))
                    errors.Add($"{prop.Name} invalid email");
            }
        }
        
        return errors.Count == 0;
    }
}
```

---

### Zadanie 12: Security Attribute
Utwórz [RequiresRole] do sprawdzania autoryzacji:

```csharp
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequiresRoleAttribute : Attribute
{
    public RequiresRoleAttribute(string role) => Role = role;
    public string Role { get; }
}

[RequiresRole("Admin")]
public class AdminPanel { }

// TODO: Implement CanAccess() that checks role
```

**Rozwiązanie:**
```csharp
public bool CanAccess(Type type, string userRole)
{
    var attr = type.GetCustomAttribute<RequiresRoleAttribute>();
    
    if (attr == null)
        return true;  // No role required
    
    return userRole == attr.Role;
}

// Usage
bool admin = CanAccess(typeof(AdminPanel), "Admin");      // true
bool user = CanAccess(typeof(AdminPanel), "User");        // false
```

---

## 📊 Wskazówki

- ✅ Zawsze dziedzicz z System.Attribute
- ✅ Używaj [AttributeUsage] do kontroli targets
- ✅ Constructor = obowiązkowe parametry
- ✅ Properties = opcjonalne (named parameters)
- ✅ AllowMultiple = true dla wielu instancji
- ✅ Inherited = true dla dziedziczenia
- ✅ Cachuj GetCustomAttribute() rezultaty
- ❌ Nie zapomnij GetCustomAttributes() dla wielu
- ❌ Nie ignoruj AttributeTargets validation
- ❌ Nie używaj Reflection na validated types bez error handling

---

## 🎯 Key Takeaways

Custom Attributes:

```
Inherit from Attribute
Add [AttributeUsage] for targets
Constructor = required params
Properties = optional (named)
GetCustomAttribute<T>() = get single
GetCustomAttributes<T>() = get multiple
AllowMultiple = true for repeats
Inherited = true for derived classes
```

Pamiętaj: **Atrybuty to metadane — perfect dla validation, documentation, security!**
