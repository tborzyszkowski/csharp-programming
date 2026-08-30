# Temat 4: Binary Serialization - Efektywność i Bezpieczeństwo

## 🔢 Czym jest Binary Serialization?

**Binary Serialization** = konwersja obiektu na sekwencję bajtów (kompaktowa, szybka).

```csharp
// Obiekt
var person = new Person { Name = "Alice", Age = 30 };

// Binary representation (example)
byte[] data = {
    0x41,           // 'A' (ASCII)
    0x6C, 0x69, ... // "lice"
    0x1E            // 30 (Age)
};

// File: person.bin (nie human-readable)
File.WriteAllBytes("person.bin", data);
```

---

## ⚠️ BinaryFormatter - DEPRECATED!

### Historia

```csharp
// .NET 2002-2020 (18 lat)
[Serializable]
public class Person
{
    public string Name;
    public int Age;
}

var bf = new BinaryFormatter();
bf.Serialize(stream, person);        // ❌ DEPRECATED since .NET 5
bf.Deserialize(stream);              // ❌ Security risk!
```

### Czemu Deprecated?

**Remote Code Execution (RCE) Vulnerability:**

```csharp
// Attacker crafts malicious binary stream
// Zawiera: type info + method calls
// Result: Unauthorized code execution during Deserialize()!

// Example:
// 1. Stream says: "Create ObjectA and call Delete()"
// 2. Deserializer obeys (RCE!)
// 3. Your data is compromised

// Timeline:
// 2017: CVE-2017-9822 (first major RCE)
// 2019: More vulnerabilities discovered
// 2020: Deprecated in .NET 5
// 2021: Disabled by default
// 2023: Removed completely in .NET 8+
```

### Security Risk Example

```csharp
// Safe code:
var person = new Person { Name = "Alice" };

// UNSAFE: Deserializing untrusted binary
byte[] maliciousData = GetFromNetwork();  // From attacker!
var restored = bf.Deserialize(maliciousData);
// ❌ Attacker can execute arbitrary code!

// Why? BinaryFormatter auto-invokes methods during deserialization
// Example payload: "Create WindowsIdentity(token) → Impersonate"
```

---

## ✅ Modern Binary Alternatives

### Opcja 1: System.Text.Json (Recommended)

```csharp
// Modern, safe, built-in
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var json = JsonSerializer.Serialize(person);
var restored = JsonSerializer.Deserialize<Person>(json);
```

**Zalety:**
- ✅ No RCE risks
- ✅ Fast
- ✅ Built-in
- ✅ Trim-safe (AOT friendly)

---

### Opcja 2: Protocol Buffers

```csharp
// Compact binary format
[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string Name { get; set; }
    
    [ProtoMember(2)]
    public int Age { get; set; }
}

var model = RuntimeTypeModel.Default;
using var ms = new MemoryStream();
model.Serialize(ms, person);
byte[] data = ms.ToArray();  // Compact!
```

**Zalety:**
- ✅ Very small size
- ✅ Very fast
- ✅ Safe (no auto-invocation)
- ✅ Industry standard (gRPC)

---

### Opcja 3: Custom Binary (Manual)

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Custom serialization (full control)
    public byte[] Serialize()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        
        writer.Write(Name);  // String (length-prefixed)
        writer.Write(Age);   // Int32 (4 bytes)
        
        return ms.ToArray();
    }
    
    public static Person Deserialize(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        
        var name = reader.ReadString();
        var age = reader.ReadInt32();
        
        return new Person { Name = name, Age = age };
    }
}

// Usage (fully controlled, secure)
var person = new Person { Name = "Alice", Age = 30 };
byte[] data = person.Serialize();
var restored = Person.Deserialize(data);
```

---

## 📊 Size Comparison: Binary Formats

```
Person { Name = "Alice", Age = 30 }

Format              Size        Ratio
─────────────────────────────────────
Binary (Custom)     16 bytes    1.0x
Protocol Buffers    12 bytes    0.75x
JSON (compact)      26 bytes    1.6x
XML                 96 bytes    6.0x
```

---

## ⚡ Performance Comparison

```
Serializing 10,000 objects:

Format              Time        Speed
─────────────────────────────────────
BinaryFormatter     150ms       1.0x (DEPRECATED)
Custom Binary       20ms        7.5x
Protocol Buffers    15ms        10x
System.Text.Json    20ms        7.5x
Source-gen JSON     5ms         30x
```

---

## 🔐 Security Best Practices

### ❌ NEVER do:

```csharp
var bf = new BinaryFormatter();
bf.Deserialize(untrustedData);  // ❌ RCE RISK!
```

### ✅ DO instead:

```csharp
// Option 1: JSON
var person = JsonSerializer.Deserialize<Person>(json);

// Option 2: Protocol Buffers
var model = RuntimeTypeModel.Default;
var person = (Person)model.Deserialize(stream, null, typeof(Person));

// Option 3: Custom Binary (full control)
var person = Person.Deserialize(data);
```

---

## 🔄 Custom Binary Serialization Pattern

```csharp
public interface ISerializable
{
    byte[] Serialize();
    void Deserialize(byte[] data);
}

public class Person : ISerializable
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public byte[] Serialize()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        
        // Version (for versioning support)
        writer.Write((byte)1);
        
        // Data
        writer.Write(Name ?? "");
        writer.Write(Age);
        
        return ms.ToArray();
    }
    
    public static Person Deserialize(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        
        byte version = reader.ReadByte();
        
        if (version != 1)
            throw new InvalidOperationException("Unsupported version");
        
        var name = reader.ReadString();
        var age = reader.ReadInt32();
        
        return new Person { Name = name, Age = age };
    }
}
```

---

## 📚 Versioning Support

```csharp
public class PersonV2 : ISerializable
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }  // NEW in v2
    
    public byte[] Serialize()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        
        writer.Write((byte)2);  // Version 2
        writer.Write(Name ?? "");
        writer.Write(Age);
        writer.Write(Email ?? "");  // New field
        
        return ms.ToArray();
    }
    
    public static PersonV2 Deserialize(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        
        byte version = reader.ReadByte();
        var name = reader.ReadString();
        var age = reader.ReadInt32();
        
        string email = "";
        if (version >= 2)
            email = reader.ReadString();  // Read new field
        
        return new PersonV2 { Name = name, Age = age, Email = email };
    }
}
```

---

## 🎯 Kiedy Użyć Binary?

### ✅ Dobrze dla:
- **High-performance systems** - szybko, mało overhead
- **Network protocols** - kompaktowe dane
- **File formats** - efektywne przechowywanie
- **Custom needs** - pełna kontrola

### ❌ Nie używaj:
- **BinaryFormatter** (deprecated, unsafe)
- **Untrusted sources** (RCE risk)

### ✅ Preferuj:
- **System.Text.Json** - modern default
- **Protocol Buffers** - industry standard
- **Custom Binary** - full control

---

## 📈 Modern Landscape (2024+)

```
BinaryFormatter (2002-2023)
├─ Era: DEPRECATED
├─ Status: Removed in .NET 8+
└─ Migration: Use alternatives

System.Text.Json (2019+)
├─ Era: MODERN
├─ Status: Recommended
└─ Speed: Very fast, AOT-ready

Protocol Buffers (2008+)
├─ Era: INDUSTRY STANDARD
├─ Status: Popular for gRPC
└─ Speed: Fastest, smallest

Custom Binary (Always)
├─ Era: FULL CONTROL
├─ Status: Use when needed
└─ Speed: Controllable
```

---

## 📚 Referencje

- [Microsoft: BinaryFormatter is dangerous](https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide)
- [Protocol Buffers Tutorial](https://learn.microsoft.com/en-us/shows/protobuf)
- [gRPC Documentation](https://grpc.io/)

---

## ✅ Summary

**Binary Serialization** = Kompaktowa reprezentacja obiektu.

**BinaryFormatter:** ❌ DEPRECATED (RCE vulnerabilities)

**Alternatywy:**
- ✅ System.Text.Json (default)
- ✅ Protocol Buffers (performance)
- ✅ Custom Binary (control)

**Kluczowy lesson:** Nigdy nie deserializuj untrusted binary data z BinaryFormatter!

---

## 🎯 Następny Temat

Temat 5: JSON Serialization - System.Text.Json, Modern Approach, Source Generators
