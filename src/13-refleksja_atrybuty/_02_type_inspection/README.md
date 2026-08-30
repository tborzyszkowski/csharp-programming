# Temat 2: Badanie Typów - Type Inspection

## 🔎 Czym jest Type Inspection?

**Type Inspection** = odkrywanie struktury typu w runtime poprzez refleksję

```csharp
// Masz obiekt, ale nie znasz jego struktury
object mystery = GetData();

// Type Inspection pozwala dowiedzieć się:
var type = mystery.GetType();
var properties = type.GetProperties();   // Properties
var methods = type.GetMethods();         // Methods
var fields = type.GetFields();           // Fields
var constructors = type.GetConstructors(); // Constructors
```

---

## 🏗️ Klasa System.Type

### Podstawowe Informacje

```csharp
var type = typeof(Person);

// Name info
Console.WriteLine(type.Name);              // "Person"
Console.WriteLine(type.FullName);          // "Demo.Person"
Console.WriteLine(type.Namespace);         // "Demo"

// Type info
Console.WriteLine(type.BaseType);          // Type.Object or parent
Console.WriteLine(type.IsAbstract);        // true/false
Console.WriteLine(type.IsSealed);          // true/false
Console.WriteLine(type.IsInterface);       // true/false
Console.WriteLine(type.IsEnum);            // true/false
Console.WriteLine(type.IsGenericType);     // true/false
```

---

## 📋 GetProperties() - Odkrywanie Properties

### Podstawowe Użycie

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; private set; }
}

var type = typeof(Person);
var properties = type.GetProperties();  // Zwraca public properties

foreach (var prop in properties)
{
    Console.WriteLine($"Property: {prop.Name}");
    Console.WriteLine($"  Type: {prop.PropertyType.Name}");
    Console.WriteLine($"  Readable: {prop.CanRead}");
    Console.WriteLine($"  Writable: {prop.CanWrite}");
}

// Output:
// Property: Name
//   Type: String
//   Readable: true
//   Writable: true
// Property: Age
//   Type: Int32
//   Readable: true
//   Writable: true
// Property: Email
//   Type: String
//   Readable: true
//   Writable: false (private setter)
```

### BindingFlags - Filtrowanie

```csharp
// Bierz TYLKO publiczne properties
var props1 = type.GetProperties(BindingFlags.Public);

// Bierz TYLKO private properties
var props2 = type.GetProperties(BindingFlags.NonPublic);

// Bierz statyczne properties
var props3 = type.GetProperties(BindingFlags.Static);

// Bierz instance properties
var props4 = type.GetProperties(BindingFlags.Instance);

// Kombinuj flagi
var props5 = type.GetProperties(
    BindingFlags.Public | BindingFlags.Instance);

// Bierz ALL properties (publiczne + private)
var props6 = type.GetProperties(
    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
```

---

## 🔧 GetMethods() - Odkrywanie Metod

### Podstawowe Użycie

```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    private int Multiply(int a, int b) => a * b;
    
    public static int Power(int a, int b) => (int)Math.Pow(a, b);
}

var type = typeof(Calculator);
var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

foreach (var method in methods)
{
    var parameters = string.Join(", ", 
        method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
    
    Console.WriteLine($"Method: {method.Name}({parameters})");
    Console.WriteLine($"  Returns: {method.ReturnType.Name}");
}

// Output:
// Method: Add(int a, int b)
//   Returns: Int32
// Method: Subtract(int a, int b)
//   Returns: Int32
```

### MethodInfo - Szczegóły

```csharp
var type = typeof(Calculator);
var method = type.GetMethod("Add");

// Info o metodzie
Console.WriteLine($"Name: {method.Name}");
Console.WriteLine($"Return Type: {method.ReturnType}");
Console.WriteLine($"Is Static: {method.IsStatic}");
Console.WriteLine($"Is Abstract: {method.IsAbstract}");
Console.WriteLine($"Is Virtual: {method.IsVirtual}");

// Info o parametrach
var parameters = method.GetParameters();
foreach (var param in parameters)
{
    Console.WriteLine($"Parameter: {param.Name} ({param.ParameterType.Name})");
}
```

---

## 📦 GetFields() - Odkrywanie Pól

### Podstawowe Użycie

```csharp
public class Config
{
    public string ApiKey;
    private int _timeout;
    public readonly string Version = "1.0";
}

var type = typeof(Config);
var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic);

foreach (var field in fields)
{
    Console.WriteLine($"Field: {field.Name}");
    Console.WriteLine($"  Type: {field.FieldType.Name}");
    Console.WriteLine($"  IsReadOnly: {field.IsInitOnly || field.IsLiteral}");
}

// Output:
// Field: ApiKey
//   Type: String
//   IsReadOnly: false
// Field: _timeout
//   Type: Int32
//   IsReadOnly: false
// Field: Version
//   Type: String
//   IsReadOnly: true
```

---

## 🏗️ GetConstructors() - Odkrywanie Konstruktorów

### Podstawowe Użycie

```csharp
public class Book
{
    public Book() { }
    public Book(string title) { Title = title; }
    public Book(string title, string author, int year) { }
    
    public string Title { get; set; }
    public string Author { get; set; }
    public int Year { get; set; }
}

var type = typeof(Book);
var constructors = type.GetConstructors();

foreach (var ctor in constructors)
{
    var parameters = string.Join(", ",
        ctor.GetParameters().Select(p => p.ParameterType.Name));
    
    Console.WriteLine($"Constructor: new Book({parameters})");
}

// Output:
// Constructor: new Book()
// Constructor: new Book(String)
// Constructor: new Book(String, String, Int32)
```

---

## 🎯 Generic Types Inspection

### IsGenericType & GetGenericArguments()

```csharp
// Badanie generic types
var type1 = typeof(List<int>);
var type2 = typeof(Dictionary<string, int>);
var type3 = typeof(Person);

Console.WriteLine($"List<int> is generic: {type1.IsGenericType}");
Console.WriteLine($"  Generic args: {string.Join(", ", 
    type1.GetGenericArguments().Select(t => t.Name))}");
// Output: Generic args: Int32

Console.WriteLine($"\nDictionary<string,int> is generic: {type2.IsGenericType}");
Console.WriteLine($"  Generic args: {string.Join(", ",
    type2.GetGenericArguments().Select(t => t.Name))}");
// Output: Generic args: String, Int32

Console.WriteLine($"\nPerson is generic: {type3.IsGenericType}");
// Output: false
```

### MakeGenericType() - Tworzenie Generic Types

```csharp
// Masz generic definition
var listDef = typeof(List<>);  // Note: <>

// Twórz konkretne typy dynamicznie
var listOfInt = listDef.MakeGenericType(typeof(int));
var listOfString = listDef.MakeGenericType(typeof(string));

Console.WriteLine(listOfInt.Name);       // "List`1"
Console.WriteLine(listOfString.Name);    // "List`1"

// Instancjonuj je
var intList = Activator.CreateInstance(listOfInt);
var stringList = Activator.CreateInstance(listOfString);

Console.WriteLine(intList.GetType().Name);      // "List`1"
Console.WriteLine(stringList.GetType().Name);   // "List`1"
```

---

## 🔗 Hierarchia Typów & Dziedziczenie

### Badanie Hierarchii

```csharp
public abstract class Animal { }
public class Dog : Animal { }
public class Poodle : Dog { }

var type = typeof(Poodle);

// Direct parent
Console.WriteLine($"Base Type: {type.BaseType.Name}");  // "Dog"

// Wszystkie parenty (hierarchia)
var current = type;
Console.WriteLine("Inheritance chain:");
while (current != null && current != typeof(object))
{
    Console.WriteLine($"  - {current.Name}");
    current = current.BaseType;
}

// Output:
//   - Poodle
//   - Dog
//   - Animal
```

### Interface Implementation

```csharp
public interface IMovable { }
public interface IFlyable { }
public class Bird : IMovable, IFlyable { }

var type = typeof(Bird);
var interfaces = type.GetInterfaces();

Console.WriteLine("Interfaces:");
foreach (var iface in interfaces)
{
    Console.WriteLine($"  - {iface.Name}");
}

// Output:
//   - IMovable
//   - IFlyable
```

---

## 🔍 MemberInfo - Bazowa Klasa

```csharp
// Wszystkie membres dziedziczą z MemberInfo
var type = typeof(Person);

// Combine all members
var members = type.GetMembers();

foreach (var member in members)
{
    Console.WriteLine($"{member.MemberType}: {member.Name}");
}

// MemberTypes:
// - PropertyInfo
// - MethodInfo
// - FieldInfo
// - EventInfo
// - ConstructorInfo
// - NestedType
```

---

## ⚡ Performance: Caching Best Practice

```csharp
// ❌ BAD: Reflection every time
void ProcessBad(object obj)
{
    var properties = obj.GetType().GetProperties();  // SLOW!
    foreach (var prop in properties) { /* ... */ }
}

// ✅ GOOD: Cache metadata
private static readonly Dictionary<Type, PropertyInfo[]> Cache = new();

PropertyInfo[] GetPropertiesFromCache(Type type)
{
    if (!Cache.TryGetValue(type, out var props))
    {
        props = type.GetProperties();
        Cache[type] = props;
    }
    return props;
}

void ProcessGood(object obj)
{
    var properties = GetPropertiesFromCache(obj.GetType());  // FAST!
    foreach (var prop in properties) { /* ... */ }
}
```

---

## 📚 Summary

**Type Inspection** = odkrywanie struktury typu dynamicznie

**Kluczowe metody:**
- `GetProperties()` - odkryj properties
- `GetMethods()` - odkryj metody
- `GetFields()` - odkryj pola
- `GetConstructors()` - odkryj konstruktory
- `GetInterfaces()` - odkryj interfejsy
- `BaseType` - odkryj parent klasę

**BindingFlags** = kontrola co się odkrywa (public/private/static)

**Generics** = `IsGenericType`, `GetGenericArguments()`, `MakeGenericType()`

**Performance** = ZAWSZE CACHE PropertyInfo/MethodInfo!

---

## 🎯 Następny Temat

Temat 3: System.Activator - Dynamiczne Tworzenie Instancji
