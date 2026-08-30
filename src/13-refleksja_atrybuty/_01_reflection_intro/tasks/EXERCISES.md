# Ćwiczenia: Refleksja - Wprowadzenie

## 🟢 Basic Level

### Zadanie 1: typeof() vs GetType()
Jaka jest różnica?

```csharp
var type1 = typeof(string);
var type2 = "hello".GetType();
```

**Rozwiązanie:**
```
typeof() - compile-time, bierze typ z kodu
GetType() - runtime, bierze typ z obiektu
```

---

### Zadanie 2: Type Properties
Co zwraca `typeof(Person).Name`?

**Rozwiązanie:**
```
"Person" (bez namespace)
typeof(Person).FullName = "Demo.Person" (z namespace)
```

---

### Zadanie 3: GetProperties()
Ile properties ma klasa string?

```csharp
var props = typeof(string).GetProperties();
```

**Rozwiązanie:**
```
1 property: Length (read-only)
```

---

### Zadanie 4: IsClass vs IsValueType
Które są klasy?

A. string  
B. int  
C. Person (custom class)  
D. decimal  

**Rozwiązanie:**
```
A = true (klasa)
B = false (value type - struct)
C = true (klasa)
D = false (value type - struct)
```

---

### Zadanie 5: BaseType
Jakie jest BaseType dla wszystkich klas?

**Rozwiązanie:**
```csharp
var type = typeof(Person);
var baseType = type.BaseType;  // System.Object (jeśli nie ma :Base)
```

---

## 🟡 Intermediate Level

### Zadanie 6: Assembly Loading
Załaduj Assembly i wylistuj 5 typów:

```csharp
var assembly = Assembly.GetExecutingAssembly();
// TODO: Get types and print names
```

**Rozwiązanie:**
```csharp
var types = assembly.GetTypes()
    .Where(t => t.IsPublic)
    .Take(5);

foreach (var type in types)
    Console.WriteLine(type.Name);
```

---

### Zadanie 7: Method Discovery
Odkryj wszystkie publiczne metody klasy:

```csharp
var type = typeof(string);
// TODO: Get all public methods
```

**Rozwiązanie:**
```csharp
var methods = type.GetMethods(
    BindingFlags.Public | BindingFlags.Instance);

foreach (var method in methods)
    Console.WriteLine($"{method.Name}");
```

---

### Zadanie 8: Property Inspection
Dla każdej property wypisz typ:

```csharp
public class Book
{
    public string Title { get; set; }
    public int Pages { get; set; }
    public Author Author { get; set; }
}

// TODO: Print property types
```

**Rozwiązanie:**
```csharp
var type = typeof(Book);
foreach (var prop in type.GetProperties())
{
    Console.WriteLine($"{prop.Name}: {prop.PropertyType.Name}");
}

// Output:
// Title: String
// Pages: Int32
// Author: Author
```

---

### Zadanie 9: Attribute Discovery
Sprawdź czy klasa ma custom atrybuty:

```csharp
[Serializable]
public class Data { }

var type = typeof(Data);
// TODO: Check for attributes
```

**Rozwiązanie:**
```csharp
var attrs = type.GetCustomAttributes();
if (attrs.Length > 0)
    Console.WriteLine($"Has {attrs.Length} attributes");
```

---

### Zadanie 10: Generic Type Inspection
Badaj generic List<T>:

```csharp
var type = typeof(List<int>);
// TODO: Check if generic, get type arguments
```

**Rozwiązanie:**
```csharp
if (type.IsGenericType)
{
    var args = type.GetGenericArguments();
    Console.WriteLine($"Generic args: {args[0].Name}");  // "Int32"
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Dynamic Type Creation Discovery
Badaj typ, który nie znasz:

```csharp
object mystery = GetMysteryObject();  // Unknown type!

// TODO: Discover structure and print info
```

**Rozwiązanie:**
```csharp
var type = mystery.GetType();
Console.WriteLine($"Type: {type.Name}");

foreach (var prop in type.GetProperties())
{
    var value = prop.GetValue(mystery);
    Console.WriteLine($"  {prop.Name} = {value}");
}
```

---

### Zadanie 12: Reflection Caching Comparison
Porównaj performance: non-cached vs cached reflection:

```csharp
var person = new Person { Name = "Bob" };
var iterations = 100000;

// Method 1: Non-cached (BAD)
var sw1 = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    typeof(Person).GetProperty("Name")!.GetValue(person);
}
sw1.Stop();

// Method 2: Cached (GOOD)
var sw2 = Stopwatch.StartNew();
var prop = typeof(Person).GetProperty("Name")!;
for (int i = 0; i < iterations; i++)
{
    prop.GetValue(person);
}
sw2.Stop();

Console.WriteLine($"Non-cached: {sw1.ElapsedMilliseconds}ms");
Console.WriteLine($"Cached: {sw2.ElapsedMilliseconds}ms");
// Cached powinno być ~100x szybsze!
```

---

## 📊 Wskazówki

- ✅ Zawsze cache Type i MemberInfo
- ✅ Używaj typeof() zamiast GetType() gdy znasz typ
- ✅ Specify BindingFlags do GetMethods()
- ✅ Check type properties (IsClass, IsGeneric)
- ✅ Handle null values z GetProperty()
- ❌ Nie używaj reflection w tight loops bez cachingu
- ❌ Nie zapomnij HandleException (type nie istnieje)
- ❌ Nie ignoruj performance impact

---

## 🎯 Key Takeaways

Refleksja:

```
System.Reflection = badanie typów w runtime
System.Type = reprezentacja klasy/typu
Workflow = Type → GetProperties() → GetValue()
Performance = Zawsze cache!
Use Cases = Serialization, DI, ORM, Validation
```

Pamiętaj: **Refleksja to moc, ale z wydajnościową ceną!**
