# Temat 1: Refleksja - Wprowadzenie & Historia

## 🔍 Czym jest Refleksja?

**Refleksja** (Reflection) = zdolność programu do **badania i manipulacji** swoją własną strukturą w **runtime**

```csharp
// Refleksja w akcji: badanie klasy bez posiadania kodu źródłowego
var type = typeof(Person);
var properties = type.GetProperties();  // Odkryj properties!
var methods = type.GetMethods();        // Odkryj metody!

foreach (var prop in properties)
{
    Console.WriteLine($"Property: {prop.Name}, Type: {prop.PropertyType}");
}

// Output:
// Property: Name, Type: System.String
// Property: Age, Type: System.Int32
```

**Analogia**: Jak lustro pokazujące strukturę kodu w locie

---

## 📚 Historia Refleksji w .NET

### Timeline: 2002-2026

```
2002: .NET 1.0
  └─ System.Reflection namespace introduced
     ├─ Type class
     ├─ PropertyInfo, MethodInfo, FieldInfo
     └─ Basic runtime type inspection

2005: .NET 2.0
  └─ Generics support in reflection
     ├─ MakeGenericType()
     ├─ GetGenericArguments()
     └─ Generic type constraints inspection

2007: .NET 3.5
  └─ LINQ + Expression Trees
     ├─ Expression<T> (compilable reflection)
     ├─ Func<T> delegates
     └─ Compilation time safety

2010: .NET 4.0
  └─ Dynamic keyword
     ├─ DynamicObject
     ├─ ExpandoObject
     └─ Late binding support

2015: .NET Core 1.0
  └─ Cross-platform reflection
     ├─ Consistent API across platforms
     ├─ Performance improvements
     └─ Trimming considerations

2020: .NET 5.0
  └─ Performance optimizations
     ├─ Compiled delegates caching
     ├─ Native AOT preparation
     └─ Reflection emit improvements

2024: .NET 9.0
  └─ Source Generators maturity
     ├─ Compile-time code generation
     ├─ AOT (Ahead-of-Time) JIT
     ├─ Reduced runtime reflection need
     └─ Better performance & smaller binaries

2025: C# 13
  └─ Modern reflection patterns
     ├─ Global type inference
     ├─ Record improvements
     ├─ File-scoped types
     └─ Reusable attributes
```

---

## 🏗️ System.Reflection Namespace

### Core Types

```csharp
using System.Reflection;

// 1. Assembly - kolekcja typów
var assembly = Assembly.GetExecutingAssembly();
var types = assembly.GetTypes();  // Wszystkie typy!

// 2. Type - reprezentacja klasy/struktury
var type = typeof(Person);
var type2 = obj.GetType();

// 3. MemberInfo - bazowa klasa dla:
//   ├─ PropertyInfo
//   ├─ MethodInfo
//   ├─ FieldInfo
//   ├─ EventInfo
//   └─ ConstructorInfo

var properties = type.GetProperties();
var methods = type.GetMethods();
var fields = type.GetFields();
var constructors = type.GetConstructors();
var events = type.GetEvents();
```

---

## 🔑 System.Type - Fundament Refleksji

### Czym jest Type?

`System.Type` = reprezentacja typu w runtime (klasa, struktura, interfejs, itp.)

```csharp
// Sposób 1: typeof()
var typeA = typeof(string);
var typeB = typeof(List<int>);

// Sposób 2: GetType()
var obj = new Person { Name = "Alice" };
var typeC = obj.GetType();  // Person

// Sposób 3: Type.GetType()
var typeD = Type.GetType("System.String");

// Wszystkie to to samo!
Console.WriteLine(typeA == typeC);  // true
```

### Properties Type

```csharp
var type = typeof(Person);

// Basic info
Console.WriteLine(type.Name);           // "Person"
Console.WriteLine(type.FullName);       // "Demo.Person"
Console.WriteLine(type.BaseType);       // Object or inheritance

// Metadata
Console.WriteLine(type.IsClass);        // true
Console.WriteLine(type.IsInterface);    // false
Console.WriteLine(type.IsValueType);    // false
Console.WriteLine(type.IsAbstract);     // false
Console.WriteLine(type.IsSealed);       // false

// Generics
Console.WriteLine(type.IsGenericType);  // false
Console.WriteLine(type.GenericTypeArguments);  // []

// Visibility
Console.WriteLine(type.IsPublic);       // true
Console.WriteLine(type.IsNestedPublic); // false
```

---

## 🔄 Workflow Refleksji

### Od Ładowania Kodu do Uruchomienia

```
┌─────────────────────────────────────────────────────────┐
│ 1. COMPILE TIME                                         │
│    └─ C# compiler generates IL + metadata               │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ 2. ASSEMBLY LOAD                                        │
│    └─ System.Reflection reads assembly metadata         │
│       ├─ Types
│       ├─ Members (properties, methods, fields)
│       ├─ Attributes
│       └─ Custom metadata
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ 3. TYPE INSPECTION                                      │
│    └─ Query Type object for information                 │
│       ├─ type.GetProperties()
│       ├─ type.GetMethods()
│       ├─ type.GetCustomAttributes()
│       └─ type.GetConstructors()
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ 4. DYNAMIC EXECUTION                                    │
│    └─ Execute methods/create instances without          │
│       knowing type at compile time                      │
│       ├─ Activator.CreateInstance()
│       ├─ MethodInfo.Invoke()
│       ├─ PropertyInfo.GetValue()/SetValue()
│       └─ ConstructorInfo.Invoke()
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ 5. RUNTIME ADAPTATION                                   │
│    └─ Program behavior changes based on discovered      │
│       metadata/attributes                               │
│       ├─ Different property values
│       ├─ Validation results
│       ├─ Serialization behavior
│       └─ Plugin loading
└─────────────────────────────────────────────────────────┘
```

---

## 💡 Praktyczne Zastosowania

### 1. Serialization (JSON, XML)
```csharp
// System.Text.Json używa refleksji do odkrycia properties
var json = JsonSerializer.Serialize(person);
// Wewnętrznie: typeof(Person).GetProperties()
```

### 2. Dependency Injection
```csharp
// DI containers (Autofac, Ninject) używają refleksji
// do automatycznego wstrzykiwania zależności
services.AddScoped<IRepository, DatabaseRepository>();
// Wewnętrznie: badanie konstruktorów i ich parametrów
```

### 3. ORM (Entity Framework)
```csharp
// EF Core używa refleksji do mapowania klas na tabele
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}
// EF: typeof(User).GetProperties() → SQL CREATE TABLE
```

### 4. Validation (Data Annotations)
```csharp
public class Person
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
}
// Validator: typeof(Person).GetProperties()
//           → checks for [Required], [StringLength] attributes
```

### 5. Testing Frameworks
```csharp
// XUnit, NUnit: odkrywają testy poprzez refleksję
[Fact]
public void TestMethod() { /* ... */ }
// Framework: typeof(TestClass).GetMethods()
//           → finds methods with [Fact] attribute
```

---

## ⚡ Performance: Refleksja vs Compiled Code

```csharp
// SCENARIO: Wywoływanie metody 1 milion razy

// Metoda 1: Direct call (FAST)
for (int i = 0; i < 1_000_000; i++)
    person.GetAge();  // ~1 ms

// Metoda 2: Reflection (SLOW)
var method = typeof(Person).GetMethod("GetAge");
for (int i = 0; i < 1_000_000; i++)
    method.Invoke(person, null);  // ~100 ms (100x slower!)

// Metoda 3: Cached compiled delegate (FAST)
var compiled = method.CreateDelegate(typeof(Func<int>), person);
for (int i = 0; i < 1_000_000; i++)
    ((Func<int>)compiled)();  // ~2 ms (almost as fast!)
```

**Wnioski:**
- ❌ Refleksja w tight loops = złe
- ✅ Refleksja + caching = akceptowalne
- ✅ Source Generators = Best (compile-time)

---

## 📖 Kluczowe Koncepty

### 1. Type Safety
Refleksja traci type safety (compile-time checks)
```csharp
// Compile-time: nieznane!
object obj = GetSomething();
var prop = obj.GetType().GetProperty("Name");
// Runtime error jeśli property nie istnieje
```

### 2. Performance Implications
Refleksja = powolna, zawsze cache!
```csharp
// ❌ BAD: Reflection każdą iterację
foreach (var item in items)
{
    var type = item.GetType();  // SLOW!
    var method = type.GetMethod("Process");
    method.Invoke(item, null);
}

// ✅ GOOD: Cache metadata
var type = items[0].GetType();
var method = type.GetMethod("Process");
foreach (var item in items)
{
    method.Invoke(item, null);  // Fast!
}
```

### 3. Metadata Everywhere
Każdy .NET assembly zawiera pełne metadata
```csharp
// Możesz badać ANY assembly
var assembly = Assembly.LoadFrom("SomeLibrary.dll");
var types = assembly.GetTypes();
foreach (var type in types)
{
    Console.WriteLine($"Type: {type.FullName}");
    foreach (var method in type.GetMethods())
    {
        Console.WriteLine($"  Method: {method.Name}");
    }
}
```

---

## 🎯 Best Practices

✅ Do:
- Cache Type objects i MemberInfo
- Używaj compiled delegates zamiast Invoke()
- Validate attributes na compile-time (C# analyzers)
- Document expected types/structure
- Handle reflection errors gracefully

❌ Don't:
- Nie używaj reflection w tight loops
- Nie zapomnij exception handling
- Nie traverse wszystkie types bez potrzeby
- Nie zapomnij null checks
- Nie ignoruj performance implications

---

## 📚 Summary

**Refleksja** = moc do badania i manipulacji kodem w runtime

**Historia**: Od .NET 1.0 (2002) do nowoczesnego AOT w .NET 9.0

**System.Reflection**: Namespace zawierający Assembly, Type, MemberInfo, itp.

**System.Type**: Fundament - reprezentacja klasy/typu w runtime

**Workflow**: Load → Inspect → Execute → Adapt

**Performance**: Zawsze cache! Rozważ Source Generators dla performance-critical code

---

## 🎯 Następny Temat

Temat 2: Badanie Typów - GetProperties(), GetMethods(), GenericTypes
