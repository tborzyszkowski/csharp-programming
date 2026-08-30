# Temat 7: Wbudowane Atrybuty .NET

## 🔨 Atrybuty Framework'a

**.NET Framework** zawiera wiele wbudowanych atrybutów. Najczęstsze:

---

## 🚫 [Obsolete] - Oznacz Kod jako Przestarzały

```csharp
// Mark method as deprecated
[Obsolete("Use NewMethod instead")]
public void OldMethod() { }

// With error flag - compilation will fail
[Obsolete("Use NewMethod instead", error: true)]
public void CriticallyOldMethod() { }

// Reading
var method = typeof(MyClass).GetMethod("OldMethod");
var obsolete = method?.GetCustomAttribute<ObsoleteAttribute>();

if (obsolete != null)
    Console.WriteLine($"Deprecated: {obsolete.Message}");
```

---

## 💾 [Serializable] - Oznacz Typ jako Serializable

```csharp
// Mark class as serializable (dla BinaryFormatter - deprecated!)
[Serializable]
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

// Modern approach: System.Text.Json doesn't need this
// But legacy code still uses it

var type = typeof(Person);
bool isSerializable = type.GetCustomAttribute<SerializableAttribute>() != null;
Console.WriteLine($"Serializable: {isSerializable}");
```

---

## 🚩 [Flags] - Enum jako Bitwise Flags

```csharp
// Regular Enum
public enum Permissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4
}

// Enum with [Flags] - indicates bitwise operations
[Flags]
public enum AdvancedPermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
    All = Read | Write | Delete
}

// Usage with [Flags]
var perms = AdvancedPermissions.Read | AdvancedPermissions.Write;
Console.WriteLine(perms);  // "Read, Write" (nice output!)

// Without [Flags]
var regPerms = Permissions.Read | Permissions.Write;
Console.WriteLine(regPerms);  // "3" (not as readable)

// Check if has [Flags]
var type = typeof(AdvancedPermissions);
bool hasFlags = type.GetCustomAttribute<FlagsAttribute>() != null;
Console.WriteLine($"Is Flags enum: {hasFlags}");
```

---

## 🔀 [Conditional] - Compilation Conditional

```csharp
// Method only compiled in DEBUG
[Conditional("DEBUG")]
public void DebugLog(string message)
{
    Console.WriteLine($"DEBUG: {message}");
}

// Only compiled if RELEASE
[Conditional("RELEASE")]
public void ProductionLog(string message)
{
    Console.WriteLine($"PROD: {message}");
}

// Multiple conditions
[Conditional("DEBUG")]
[Conditional("VERBOSE")]
public void VerboseLog(string message)
{
    Console.WriteLine($"VERBOSE: {message}");
}

// Usage - calls removed at compile time if condition not met
DebugLog("Only in debug build");
ProductionLog("Only in release");
```

---

## ✅ DataAnnotations - Walidacja

```csharp
using System.ComponentModel.DataAnnotations;

public class User
{
    [Required]
    public string Name { get; set; } = "";
    
    [EmailAddress]
    public string Email { get; set; } = "";
    
    [Range(18, 120)]
    public int Age { get; set; }
    
    [StringLength(50)]
    public string City { get; set; } = "";
}

// Validate using System.ComponentModel.DataAnnotations
var validator = new DataAnnotationsValidator();
bool isValid = validator.Validate(new User { Name = "Alice", Email = "invalid" });

// Check attributes
var emailProp = typeof(User).GetProperty("Email");
var emailAttr = emailProp?.GetCustomAttribute<EmailAddressAttribute>();
Console.WriteLine($"Has [EmailAddress]: {emailAttr != null}");
```

---

## 🔍 [DebuggerDisplay] - Custom Display w Debuggerze

```csharp
[DebuggerDisplay("{Name} ({Age})")]
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

// W debuggerze pokaże: "Alice (30)" zamiast "MyApp.Person"

// Kompleks expression
[DebuggerDisplay("Id={Id}, Status={Status}")]
public class Order
{
    public int Id { get; set; }
    public string Status { get; set; } = "";
}
```

---

## 📜 [Description] - Opis dla UI

```csharp
using System.ComponentModel;

[Description("Main application class")]
public class App { }

public class Config
{
    [Description("Database connection string")]
    public string ConnectionString { get; set; } = "";
    
    [Description("Maximum retry attempts")]
    public int MaxRetries { get; set; }
}

// Czytaj dla UI/help
var prop = typeof(Config).GetProperty("ConnectionString");
var desc = prop?.GetCustomAttribute<DescriptionAttribute>();
Console.WriteLine($"Help: {desc?.Description}");
```

---

## 🧵 [ThreadStatic] & [ThreadLocal]

```csharp
public class ThreadStorage
{
    // Static field, każdy thread ma swoją kopię
    [ThreadStatic]
    public static int ThreadLocalValue;
    
    public static void SetValue(int value) => ThreadLocalValue = value;
    public static int GetValue() => ThreadLocalValue;
}

// Każdy thread widzi inną wartość
Task.Run(() => {
    ThreadStorage.SetValue(1);
    Console.WriteLine(ThreadStorage.GetValue());  // 1
});

Task.Run(() => {
    ThreadStorage.SetValue(2);
    Console.WriteLine(ThreadStorage.GetValue());  // 2
});
```

---

## 🔐 [Serializable] + [NonSerialized]

```csharp
[Serializable]
public class Account
{
    public string Username { get; set; } = "";
    
    // Nie serializuj password!
    [NonSerialized]
    private string _password = "";
    
    public void SetPassword(string pwd) => _password = pwd;
}

// BinaryFormatter respects [NonSerialized]
```

---

## 🏗️ [StructLayout] - Memory Layout Control

```csharp
// Default - automatic layout
public struct Auto { }

// Sequential - fields in order
[StructLayout(LayoutKind.Sequential)]
public struct Sequential
{
    public int X;      // Offset 0
    public int Y;      // Offset 4
}

// Explicit - manual offsets (interop!)
[StructLayout(LayoutKind.Explicit)]
public struct Explicit
{
    [FieldOffset(0)]
    public int X;
    
    [FieldOffset(4)]
    public int Y;
}

// For P/Invoke interop with C++
```

---

## 📊 Podsumowanie Wbudowanych Atrybutów

| Atrybut | Target | Zastosowanie |
|---------|--------|--------------|
| [Obsolete] | Type, Method, Property | Oznacz jako przestarzałe |
| [Serializable] | Class, Struct | Serialization (legacy) |
| [Flags] | Enum | Bitwise flags |
| [Conditional] | Method | Conditional compilation |
| [Required] | Property | Data validation |
| [Range] | Property | Value range |
| [StringLength] | Property | String length |
| [EmailAddress] | Property | Email validation |
| [DebuggerDisplay] | Class | Debug visualization |
| [Description] | Property | UI descriptions |
| [NonSerialized] | Field | Exclude from serialization |
| [ThreadStatic] | Field | Thread-local storage |
| [StructLayout] | Struct | Memory layout |

---

## 📚 Summary

**.NET zawiera wiele wbudowanych atrybutów:**

- **Obsolete** - deprecation
- **Serializable** - serialization control
- **Flags** - enum bitwise operations
- **Conditional** - compilation control
- **DataAnnotations** - validation framework
- **DebuggerDisplay** - debug visualization
- **Description** - UI descriptions
- **ThreadStatic** - thread-local storage
- **StructLayout** - interop memory layout

---

## 🎯 Następny Temat

Temat 8: Plugin System - Assembly.LoadFrom, DLL loading, discovery
