# Ćwiczenia: Performance & Best Practices

## 🟢 Basic Level

### Zadanie 1: Dictionary Cache
Implementuj prosty cache:

```csharp
private static Dictionary<Type, Attribute[]> Cache = new();

// TODO: Implement GetCached method
```

**Rozwiązanie:**
```csharp
public Attribute[] GetCached(Type type)
{
    if (!Cache.TryGetValue(type, out var attrs))
    {
        attrs = type.GetCustomAttributes();
        Cache[type] = attrs;
    }
    return attrs;
}
```

---

### Zadanie 2: IsDefined vs GetCustomAttribute
Porównaj performance:

```csharp
// TODO: Use IsDefined() for fast check
```

**Rozwiązanie:**
```csharp
// Fast check
bool has = type.IsDefined(typeof(DocAttr));

// Slower
bool hasAttr = type.GetCustomAttribute<DocAttr>() != null;
```

---

### Zadanie 3: Avoid Reflection in Loops
Refactor: wciągnij reflection poza pętlę:

```csharp
// ❌ BAD
for (int i = 0; i < 1000; i++)
{
    var attrs = type.GetCustomAttributes();
}

// TODO: Move reflection outside
```

**Rozwiązanie:**
```csharp
// ✅ GOOD
var attrs = type.GetCustomAttributes();

for (int i = 0; i < 1000; i++)
{
    // Use cached attrs
}
```

---

### Zadanie 4: Compile Expressions
Kompiluj zamiast interpretować:

```csharp
// TODO: Compile expression for reuse
```

**Rozwiązanie:**
```csharp
var param = Expression.Parameter(typeof(int));
var body = Expression.Add(param, Expression.Constant(1));
var lambda = Expression.Lambda<Func<int, int>>(body, param);

var compiled = lambda.Compile();  // Compile once
var result1 = compiled(10);        // Reuse
var result2 = compiled(20);
```

---

### Zadanie 5: Table Attribute for ORM
Utwórz [Table] atrybut:

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    // TODO: Define constructor and property
}
```

**Rozwiązanie:**
```csharp
[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public TableAttribute(string name) => Name = name;
    public string Name { get; }
}
```

---

## 🟡 Intermediate Level

### Zadanie 6: Column Attribute for ORM
Utwórz [Column] atrybut:

```csharp
[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    // TODO: Define constructor and property
}
```

**Rozwiązanie:**
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute : Attribute
{
    public ColumnAttribute(string name) => Name = name;
    public string Name { get; }
}
```

---

### Zadanie 7: Simple ORM Implementation
Zbuduj klasy z atrybutami:

```csharp
[Table("users")]
public class User
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
}
```

**Rozwiązanie:**
```csharp
// Already shown above
```

---

### Zadanie 8: ORM Metadata Extraction
Czytaj metadata z atrybutów:

```csharp
public class ORMMapper
{
    public string GetTableName<T>()
    {
        // TODO: Extract table name from [Table]
    }
}
```

**Rozwiązanie:**
```csharp
public string GetTableName<T>()
{
    var type = typeof(T);
    var attr = type.GetCustomAttribute<TableAttribute>();
    return attr?.Name ?? type.Name;
}
```

---

### Zadanie 9: Generate SELECT SQL
Zbuduj SQL query:

```csharp
public string GenerateSelectSQL<T>()
{
    // TODO: Build: SELECT col1, col2 FROM table
}
```

**Rozwiązanie:**
```csharp
public string GenerateSelectSQL<T>()
{
    var type = typeof(T);
    var tableAttr = type.GetCustomAttribute<TableAttribute>();
    var tableName = tableAttr?.Name ?? type.Name;
    
    var columns = new List<string>();
    foreach (var prop in type.GetProperties())
    {
        var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
        columns.Add(colAttr?.Name ?? prop.Name);
    }
    
    var cols = string.Join(", ", columns);
    return $"SELECT {cols} FROM {tableName}";
}
```

---

### Zadanie 10: Weak Reference Cache
Implementuj GC-friendly cache:

```csharp
private Dictionary<Type, WeakReference<Attribute[]>> Cache = new();

// TODO: GetCached with weak references
```

**Rozwiązanie:**
```csharp
public Attribute[] GetCached(Type type)
{
    if (Cache.TryGetValue(type, out var weakRef) && 
        weakRef.TryGetTarget(out var attrs))
    {
        return attrs;  // Still in memory
    }
    
    attrs = type.GetCustomAttributes();
    Cache[type] = new WeakReference<Attribute[]>(attrs);
    return attrs;
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Full ORM with Caching
Zbuduj kompletny ORM z cache:

```csharp
public class CachedORM<T> where T : class, new()
{
    private static readonly Dictionary<Type, ORMMetadata> MetadataCache = new();
    
    // TODO: Implement with metadata caching
}
```

**Rozwiązanie:**
```csharp
public class ORMMetadata
{
    public string TableName { get; set; } = "";
    public Dictionary<string, string> Columns { get; set; } = new();
}

public class CachedORM<T> where T : class, new()
{
    private ORMMetadata _metadata;
    
    public CachedORM()
    {
        var type = typeof(T);
        
        if (!MetadataCache.TryGetValue(type, out var metadata))
        {
            metadata = ExtractMetadata(type);
            MetadataCache[type] = metadata;
        }
        
        _metadata = metadata;
    }
    
    private ORMMetadata ExtractMetadata(Type type)
    {
        var tableAttr = type.GetCustomAttribute<TableAttribute>();
        var metadata = new ORMMetadata
        {
            TableName = tableAttr?.Name ?? type.Name,
            Columns = new()
        };
        
        foreach (var prop in type.GetProperties())
        {
            var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
            metadata.Columns[prop.Name] = colAttr?.Name ?? prop.Name;
        }
        
        return metadata;
    }
    
    public string GenerateSQL()
    {
        var cols = string.Join(", ", _metadata.Columns.Values);
        return $"SELECT {cols} FROM {_metadata.TableName}";
    }
}
```

---

### Zadanie 12: Performance Benchmark
Porównaj reflection strategies:

```csharp
// TODO: Benchmark:
// - No cache
// - Dictionary cache
// - Weak reference cache
```

**Rozwiązanie:**
```csharp
public void BenchmarkCaching()
{
    var type = typeof(User);
    var sw = Stopwatch.StartNew();
    
    // Test 1: No caching (1000 calls)
    sw.Restart();
    for (int i = 0; i < 1000; i++)
        type.GetCustomAttributes();
    var noCacheTime = sw.ElapsedMilliseconds;
    
    // Test 2: Dictionary cache (1000 calls)
    sw.Restart();
    var cache = new SimpleAttributeCache();
    for (int i = 0; i < 1000; i++)
        cache.GetCached(type);
    var cacheTime = sw.ElapsedMilliseconds;
    
    Console.WriteLine($"No cache: {noCacheTime}ms");
    Console.WriteLine($"With cache: {cacheTime}ms");
    Console.WriteLine($"Improvement: {noCacheTime / (double)cacheTime:F1}x faster");
}
```

---

## 📊 Wskazówki

- ✅ ZAWSZE cache reflection results
- ✅ Use IsDefined() dla quick checks
- ✅ Compile expressions dla reuse
- ✅ Consider Source Generators
- ✅ Benchmark, nie zgadujesz
- ✅ Use WeakReference dla large caches
- ✅ Document AOT assumptions
- ❌ Nigdy reflection w tight loops
- ❌ Nie ignoruj null results
- ❌ Nie zapomnij TryGetValue null check

---

## 🎯 Key Takeaways

Performance:

```
Cache = 500x faster than uncached
Compiled expression = 50x faster than reflection
IsDefined = faster than GetCustomAttribute
WeakReference = balance memory + GC

Never premature optimize, but ALWAYS cache reflection!
```

ORM Pattern:

```
[Table] = class mapping
[Column] = property mapping
Extract metadata = once per type
Generate SQL = reuse metadata

Simple ORM solves 80% of use cases!
```

---

## 🏆 GRATULACJE!

**Module 13: Refleksja i Atrybuty - 100% COMPLETE**

```
✅ Topic 1: Reflection Introduction
✅ Topic 2: Type Inspection
✅ Topic 3: System.Activator
✅ Topic 4: Custom Attributes
✅ Topic 5: Advanced Attributes
✅ Topic 6: Reading Attributes
✅ Topic 7: Built-in .NET Attributes
✅ Topic 8: Plugin System
✅ Topic 9: Expression Trees & Dynamic
✅ Topic 10: Performance & Best Practices

Umiejętności opanowane:
✓ Runtime type manipulation
✓ Custom attribute creation
✓ Plugin systems
✓ Expression trees
✓ Performance optimization
✓ ORM patterns
✓ Real-world applications
```

**Jesteś gotów do zaawansowanych C#/.NET projektów! 🚀**
