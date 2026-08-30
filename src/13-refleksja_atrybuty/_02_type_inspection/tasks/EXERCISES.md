# Ćwiczenia: Type Inspection

## 🟢 Basic Level

### Zadanie 1: Count Properties
Ile properties ma klasa Person?

```csharp
var type = typeof(Person);
var props = type.GetProperties();
```

**Rozwiązanie:**
```
3: Name, Age, Email
```

---

### Zadanie 2: PropertyInfo - CanRead/CanWrite
Która property nie ma settera?

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; private set; }
}
```

**Rozwiązanie:**
```
Email (private setter → CanWrite = false)
```

---

### Zadanie 3: GetMethods()
Jakie flagi użyć, aby pobrać TYLKO instance metody?

**Rozwiązanie:**
```csharp
BindingFlags.Public | BindingFlags.Instance
```

---

### Zadanie 4: BaseType
Jakie jest BaseType dla custom klasy?

```csharp
public class MyClass { }
var type = typeof(MyClass);
var baseType = type.BaseType;
```

**Rozwiązanie:**
```
System.Object (jeśli nie ma explicit parent)
```

---

### Zadanie 5: IsGenericType
Czy List<int> jest generic?

```csharp
var type = typeof(List<int>);
Console.WriteLine(type.IsGenericType);
```

**Rozwiązanie:**
```
true
```

---

## 🟡 Intermediate Level

### Zadanie 6: Generic Arguments
Co zwraca GetGenericArguments() dla Dictionary<string, int>?

**Rozwiązanie:**
```csharp
var type = typeof(Dictionary<string, int>);
var args = type.GetGenericArguments();
// args[0] = typeof(string)
// args[1] = typeof(int)
```

---

### Zadanie 7: Print Property Types
Wypisz typ każdej property:

```csharp
public class Book
{
    public string Title { get; set; }
    public int Pages { get; set; }
    public decimal Price { get; set; }
}

var type = typeof(Book);
// TODO: Print property types
```

**Rozwiązanie:**
```csharp
var props = type.GetProperties();
foreach (var prop in props)
{
    Console.WriteLine($"{prop.Name}: {prop.PropertyType.Name}");
}
```

---

### Zadanie 8: Find Method by Name
Znajdź metodę o nazwie "Greet":

```csharp
var type = typeof(Person);
// TODO: Get Greet method
```

**Rozwiązanie:**
```csharp
var method = type.GetMethod("Greet");
if (method != null)
    Console.WriteLine($"Found: {method.Name}");
```

---

### Zadanie 9: Get Constructor Parameters
Odkryj parametry konstruktora:

```csharp
public class Book
{
    public Book(string title, string author) { }
}

var type = typeof(Book);
// TODO: Get constructor and its parameters
```

**Rozwiązanie:**
```csharp
var ctor = type.GetConstructors()[0];
var parameters = ctor.GetParameters();
foreach (var param in parameters)
{
    Console.WriteLine($"{param.Name}: {param.ParameterType.Name}");
}
```

---

### Zadanie 10: Inheritance Chain
Wypisz całą hierarchię (od Poodle do Object):

```csharp
public abstract class Animal { }
public class Dog : Animal { }
public class Poodle : Dog { }

// TODO: Print full hierarchy
```

**Rozwiązanie:**
```csharp
var type = typeof(Poodle);
var current = type;
while (current != null && current != typeof(object))
{
    Console.WriteLine(current.Name);
    current = current.BaseType;
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Cache Property Metadata
Stwórz cache dla PropertyInfo:

```csharp
private static Dictionary<Type, PropertyInfo[]> cache = new();

PropertyInfo[] GetPropertiesCached(Type type)
{
    // TODO: Implement caching
}
```

**Rozwiązanie:**
```csharp
PropertyInfo[] GetPropertiesCached(Type type)
{
    if (!cache.TryGetValue(type, out var props))
    {
        props = type.GetProperties();
        cache[type] = props;
    }
    return props;
}
```

---

### Zadanie 12: Discover Unknown Type Structure
Masz obiekt, ale nie znasz jego typu. Odkryj strukturę:

```csharp
object mystery = GetUnknownObject();

// TODO: Print:
// - Type name
// - All properties with types
// - All public methods
```

**Rozwiązanie:**
```csharp
var type = mystery.GetType();
Console.WriteLine($"Type: {type.Name}");

Console.WriteLine("Properties:");
foreach (var prop in type.GetProperties())
    Console.WriteLine($"  {prop.Name}: {prop.PropertyType.Name}");

Console.WriteLine("Methods:");
foreach (var method in type.GetMethods(
    BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
    Console.WriteLine($"  {method.Name}()");
```

---

## 📊 Wskazówki

- ✅ Zawsze cache PropertyInfo/MethodInfo
- ✅ Używaj BindingFlags do kontroli co się odkrywa
- ✅ Check CanRead/CanWrite dla properties
- ✅ Handle null results (metoda nie istnieje)
- ✅ Iteruj BaseType.BaseType dla hierarchii
- ✅ Check IsGenericType przed GetGenericArguments()
- ❌ Nie zapomnij exception handling
- ❌ Nie ignoruj performance impact

---

## 🎯 Key Takeaways

Type Inspection:

```
GetProperties() = odkryj properties
GetMethods() = odkryj metody
GetFields() = odkryj pola
GetConstructors() = odkryj konstruktory
GetInterfaces() = odkryj interfejsy
BaseType = parent klasa
IsGenericType = czy generic?
GetGenericArguments() = type arguments
```

Pamiętaj: **Zawsze cache metadata results!**
