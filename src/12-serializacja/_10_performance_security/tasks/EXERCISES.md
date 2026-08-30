# Ćwiczenia: Performance & Security

## 🟢 Basic Level

### Zadanie 1: BinaryFormatter Status
Czy BinaryFormatter jest bezpieczny?

**Rozwiązanie:**
```
❌ Nie! (RCE vulnerability)
- Deprecated: .NET 5 (2020)
- Removed: .NET 8+ (2023)
- Migrate to: System.Text.Json
```

---

### Zadanie 2: Format Speed
Który format jest najszybszy?

A. JSON  
B. Protobuf  
C. XML  
D. Binary  

**Rozwiązanie:** B (Protobuf, ~5ms vs 35ms JSON)

---

### Zadanie 3: Source Generators
Ile razy szybciej?

**Rozwiązanie:**
```
Regular JSON: 35ms
Source Generators: 5ms
Speed: 7x faster!

Why: No reflection at runtime
```

---

### Zadanie 4: XXE Protection
Jak zabezpieczyć XmlReader?

```csharp
var settings = new XmlReaderSettings
{
    ???  // Prevent XXE
    ???  // Prevent external resolution
};
```

**Rozwiązanie:**
```csharp
var settings = new XmlReaderSettings
{
    DtdProcessing = DtdProcessing.Prohibit,
    XmlResolver = null
};
```

---

### Zadanie 5: [JsonIgnore]
Na którym polu powinna być?

```csharp
public class User
{
    public string Username { get; set; }
    ???
    public string ApiKey { get; set; }  // Sensitive!
    ???
    public string Email { get; set; }
}
```

**Rozwiązanie:**
```csharp
public class User
{
    public string Username { get; set; }
    
    [JsonIgnore]  // ← Don't serialize!
    public string ApiKey { get; set; }
    
    public string Email { get; set; }
}
```

---

## 🟡 Intermediate Level

### Zadanie 6: Performance Optimization
Która technika daje 7x speedup?

**Rozwiązanie:**
```csharp
[JsonSerializable(typeof(Person))]
internal partial class MyContext : JsonSerializerContext { }

// Use Source Generators
var context = new MyContext();
var json = JsonSerializer.Serialize(person, context.Person);
```

---

### Zadanie 7: Streaming Large Files
Jak deserializować 1GB JSON?

```csharp
using (var reader = File.OpenText("huge.jsonl"))
{
    string? line;
    while ((line = reader.ReadLine()) != null)
    {
        var item = ???
        ProcessItem(item);
    }
}
```

**Rozwiązanie:**
```csharp
var item = JsonSerializer.Deserialize<Item>(line);
// Deserialize line-by-line, not entire file
```

---

### Zadanie 8: XXE Attack Prevention
Napisz bezpieczny kod:

```csharp
var doc = new XmlDocument();
var reader = XmlReader.Create(stream, ???);
doc.Load(reader);
```

**Rozwiązanie:**
```csharp
var settings = new XmlReaderSettings
{
    DtdProcessing = DtdProcessing.Prohibit,
    XmlResolver = null
};
var reader = XmlReader.Create(stream, settings);
var doc = new XmlDocument();
doc.Load(reader);
```

---

### Zadanie 9: Type-Safe Deserialization
Jaka jest różnica?

```csharp
// A
object obj = JsonSerializer.Deserialize<dynamic>(json);

// B
User obj = JsonSerializer.Deserialize<User>(json) 
    ?? throw new JsonException();
```

**Rozwiązanie:**
```
A = Unsafe (unknown type, gadget chain risk)
B = Safe (constrained to User, validation possible)
```

---

### Zadanie 10: Format Selection
JSON czy Protobuf?

```
A. REST API with browser
B. gRPC microservices
C. Configuration files
D. High-frequency trading
```

**Rozwiązanie:**
```
A = JSON (human-readable)
B = Protobuf (fast, gRPC-native)
C = JSON/YAML (human-readable)
D = Protobuf (performance critical)
```

---

## 🔴 Advanced Level

### Zadanie 11: Security Hardening
Zaimplementuj production serializer:

```csharp
public class SecureSerializer<T>
{
    public string Serialize(T obj) { /* TODO */ }
    public T Deserialize(string json) { /* TODO */ }
}

// Requirements:
// 1. Use Source Generators (fast)
// 2. Type-safe deserialization
// 3. Validate on deserialize
// 4. Log errors
```

---

### Zadanie 12: BinaryFormatter Migration
Plan migracji z BinaryFormatter:

```
Current: Using BinaryFormatter for persistence
Goal: Migrate to System.Text.Json

Steps:
1. ???
2. ???
3. ???
4. ???
5. ???
```

**Rozwiązanie:**
```
1. Identify all BinaryFormatter usage
2. Implement JSON serialization alongside
3. Test deserialization with old data
4. Create version migration path
5. Test thoroughly
6. Update all code
7. Set migration deadline
8. Remove old code
```

---

## 📊 Wskazówki

- ✅ Always validate deserialized data
- ✅ Use Source Generators for performance
- ✅ Stream large files
- ✅ Prevent XXE attacks
- ✅ Use type-safe deserialization
- ✅ Mark sensitive fields [JsonIgnore]
- ✅ Use Native AOT for deployment
- ✅ Monitor performance in production
- ❌ Nie use BinaryFormatter ever!
- ❌ Nie trust user input
- ❌ Nie concatenate JSON strings
- ❌ Nie enable DTD processing

---

## 🎯 Key Takeaways

Module 12 (Serialization) Complete!

```
10 Topics Covered:
1. Concepts (object graphs, DFS)
2. History (evolution from BinaryFormatter)
3. XML (XmlSerializer, attributes)
4. Binary (custom serialization)
5. JSON (System.Text.Json, modern)
6. Custom (converters, validation)
7. Circular Refs (cycle detection)
8. Advanced (polymorphism, versioning)
9. Protobuf (gRPC, performance)
10. Perf & Security (benchmarks, vulnerabilities)

Best Practices Summary:
- JSON for REST APIs
- Protobuf for microservices
- Source Generators for speed
- Type-safe deserialization always
- Never BinaryFormatter
```

🎓 **Pamiętaj: Choose the right format, validate data, secure by default!**
