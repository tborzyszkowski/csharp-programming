# Temat 10: Performance & Security - Benchmarks, Vulnerabilities, Best Practices

## 📊 Performance Benchmarks

### Format Comparison (10,000 objects, ~50 bytes each)

```
Format          Serialized Size   Time (ms)   Speed vs JSON   Memory
─────────────────────────────────────────────────────────────────────
JSON            350 KB            35 ms       1.0x            450 MB
System.Xml      420 KB            55 ms       0.6x            520 MB
Binary (custom) 100 KB            12 ms       2.9x            180 MB
Protobuf        80 KB             8 ms        4.4x            150 MB
Protobuf (AOT)  80 KB             5 ms        7.0x            140 MB
```

### System.Text.Json Performance

```csharp
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = false  // Much faster without formatting
};

// Source Generators (compile-time codegen)
var context = new MySerializerContext();
var json = JsonSerializer.Serialize(person, context.Person);

// Benchmarks:
// - Regular: ~35 ms for 10K objects
// - Source Generators: ~5 ms (7x faster!)
// - Reason: No reflection at runtime
```

---

## 🔐 Security Vulnerabilities

### 1. BinaryFormatter RCE (CRITICAL)

**Status:** Deprecated in .NET 5, Removed in .NET 8+

```csharp
// ❌ NEVER USE!
[Serializable]
public class BadExample
{
    private void OnDeserialized(StreamingContext context)
    {
        // Malicious code could execute here
    }
}

// Attack flow:
// 1. Attacker crafts malicious binary data
// 2. BinaryFormatter.Deserialize() called
// 3. ObjectStateFormatter invokes dangerous methods
// 4. Remote Code Execution!

// Famous example: Windows Identity Impersonation
// var malicious = <special binary data>;
// var obj = BinaryFormatter.Deserialize(malicious);
// // Now running as attacker's identity! 🔴
```

**Migration:**
```csharp
// ✅ Use System.Text.Json instead
var json = JsonSerializer.Serialize(obj);
var deserialized = JsonSerializer.Deserialize<MyClass>(json);
```

---

### 2. XXE (XML External Entity) Injection

**Vulnerable Code:**
```csharp
// ❌ DON'T DO THIS
var xmlDoc = new XmlDocument();
xmlDoc.LoadXml(userInput);  // User-controlled XML!

// Attack:
var malicious = """
<?xml version="1.0"?>
<!DOCTYPE foo [
  <!ENTITY xxe SYSTEM "file:///etc/passwd">
]>
<root>&xxe;</root>
""";

xmlDoc.LoadXml(malicious);
// Now has access to file system! 🔴
```

**Safe Code:**
```csharp
// ✅ Disable external entities
var settings = new XmlReaderSettings
{
    DtdProcessing = DtdProcessing.Prohibit,  // ← Key!
    XmlResolver = null  // ← Prevent external resolution
};

using (var reader = XmlReader.Create(xmlStream, settings))
{
    var doc = new XmlDocument();
    doc.Load(reader);
}
```

---

### 3. JSON Injection

**Vulnerable Code:**
```csharp
// ❌ String concatenation
var json = "{\"name\":\"" + userName + "\",\"age\":30}";
// If userName = "Alice\",\"isAdmin\":true,\"name\":\"Bob"
// Result: {"name":"Alice","isAdmin":true,"name":"Bob","age":30}
// isAdmin flag injected! 🔴

var obj = JsonSerializer.Deserialize<dynamic>(json);
```

**Safe Code:**
```csharp
// ✅ Proper serialization
var obj = new User { Name = userName, Age = 30 };
var json = JsonSerializer.Serialize(obj);
// Safe even with special characters
```

---

### 4. Deserialization of Untrusted Data

**Vulnerable Pattern:**
```csharp
// ❌ Don't deserialize without validation
public object DeserializeUser(byte[] data)
{
    return BinaryFormatter.Deserialize(data);  // What type is this?
}

// Could be:
// - Correct User object
// - Malicious payload
// - Wrong type entirely
```

**Safe Pattern:**
```csharp
// ✅ Type-safe deserialization
public User DeserializeUser(string json)
{
    return JsonSerializer.Deserialize<User>(json) 
        ?? throw new JsonException("Invalid format");
}

// Benefits:
// - Only deserializes to User
// - No gadget chain attacks
// - Type-safe
```

---

## 🔒 Best Practices

### Defensive Serialization

```csharp
public class SecureData
{
    // 1. Exclude sensitive fields
    [JsonIgnore]
    public string ApiKey { get; set; }
    
    // 2. Validate on deserialize
    [JsonPropertyName("age")]
    public int Age { get; set; }  // Validate in setter
    
    private int _age;
    public int AgeSafe
    {
        get => _age;
        set => _age = value > 0 && value < 150 ? value : throw new ArgumentException();
    }
    
    // 3. Use [JsonRequired] for mandatory fields
    [JsonRequired]
    public string Username { get; set; }
    
    // 4. Validate in custom converter
    public class AgeConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetInt32();
            if (value < 0 || value > 150)
                throw new JsonException("Invalid age");
            return value;
        }
    }
}
```

---

## 📈 Performance Optimization

### 1. Source Generators (Recommended)

```csharp
// Step 1: Create serializer context
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(Person))]
internal partial class MySerializerContext : JsonSerializerContext
{
}

// Step 2: Use generated code
var context = new MySerializerContext();
var json = JsonSerializer.Serialize(person, context.Person);

// Benefits:
// ✓ Zero reflection (~7x faster)
// ✓ Native AOT compatible
// ✓ Compile-time safety
```

### 2. Streaming Deserialization

```csharp
// For large files, deserialize line-by-line
using (var reader = File.OpenText("large.jsonl"))
{
    string? line;
    while ((line = reader.ReadLine()) != null)
    {
        var item = JsonSerializer.Deserialize<Item>(line);
        ProcessItem(item);  // Process as we go
    }
}

// Benefits:
// ✓ Low memory (doesn't load entire file)
// ✓ Progressive processing
```

### 3. Trimming & AOT

```csharp
// .csproj
<Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
        <PublishTrimmed>true</PublishTrimmed>
        <PublishAot>true</PublishAot>
    </PropertyGroup>
</Project>

// .NET Native compilation:
// ✓ No JIT compilation at runtime
// ✓ Smaller binary size
// ✓ Faster startup
// ✓ Predictable performance
```

---

## 🎯 Format Selection Decision Tree

```
Do you need:
├─ Human readability? → JSON (best) / XML (verbose)
├─ Performance critical? → Protobuf (best) / Binary (custom)
├─ Language interop? → JSON (best) / Protobuf (excellent)
├─ Simple REST API? → JSON (always)
├─ Microservices? → Protobuf (if gRPC)
├─ Configuration files? → JSON / YAML
├─ Backward compatibility critical? → Protobuf / JSON with versioning
└─ BinaryFormatter available? → MIGRATE NOW! Use JSON instead
```

---

## 🚀 Migration Checklist

**From BinaryFormatter:**
```
☐ Audit all BinaryFormatter usage
☐ Migrate to System.Text.Json
☐ Test deserialization with old data
☐ Implement version handling
☐ Test all code paths
☐ Deploy with monitoring
☐ Set deadline for old format
☐ Remove BinaryFormatter code
```

---

## 📚 Summary

**Performance & Security** = Critical for production systems

**Performance:**
- ✅ Source Generators (7x faster)
- ✅ Protobuf (4x faster than JSON)
- ✅ Streaming for large files
- ✅ Native AOT compatible

**Security:**
- ✅ Never use BinaryFormatter
- ✅ Prevent XXE with DtdProcessing
- ✅ Validate deserialized data
- ✅ Type-safe deserialization
- ✅ Use `[JsonIgnore]` for sensitive data
- ✅ Custom converters for validation

---

## 🎯 Production Best Practices

```csharp
// Template: Secure, Fast Serialization

public class ProductionSerializer<T>
{
    private readonly JsonSerializerContext context;
    private readonly JsonSerializerOptions options;
    
    public ProductionSerializer()
    {
        // Use Source Generators
        context = new MySerializerContext();
        
        // Safe defaults
        options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
    
    public string Serialize(T obj)
    {
        // Fast: Uses generated code
        return JsonSerializer.Serialize(obj, context.GetTypeInfo(typeof(T)));
    }
    
    public T? Deserialize(string json)
    {
        try
        {
            // Safe: Validates against schema
            return JsonSerializer.Deserialize<T>(json, options);
        }
        catch (JsonException ex)
        {
            // Log and throw
            throw new InvalidOperationException("Invalid data", ex);
        }
    }
}
```

---

## 🎓 Serialization Module Complete!

**Topics Covered:**
1. ✅ Concepts
2. ✅ History
3. ✅ XML
4. ✅ Binary
5. ✅ JSON
6. ✅ Custom
7. ✅ Circular References
8. ✅ Advanced Features
9. ✅ Protocol Buffers
10. ✅ Performance & Security

**Pamiętaj:**
- Choose right format for use case
- Performance matters at scale
- Security isn't optional
- Test thoroughly
- Monitor in production
