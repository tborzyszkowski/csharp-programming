# Temat 9: Protocol Buffers - Industry Standard Binary Format

## 🔧 What is Protocol Buffers?

### Definition

Protocol Buffers (protobuf) = Binary serialization format by Google

```
Created: 2008
Language: Language-agnostic
Use Cases: gRPC, microservices, high-performance APIs
Adoption: Google, Uber, Square, Dropbox
```

---

## 📊 vs JSON vs Binary

| Aspect | JSON | XML | Binary | Protobuf |
|--------|------|-----|--------|----------|
| **Size** | 100 | 200 | 50 | 30 |
| **Speed** | Medium | Slow | Fast | Very Fast |
| **Human Readable** | ✅ | ✅ | ❌ | ❌ |
| **Versioning** | Hard | Medium | Hard | ✅ |
| **Learning Curve** | Easy | Medium | Hard | Hard |
| **Ecosystem** | Large | Large | Small | Medium |

---

## 🎯 Protobuf Benefits

✅ **Compact Binary Format** (30% size of JSON)  
✅ **Forward/Backward Compatible** (version safe)  
✅ **Fast Serialization** (5-10x faster than JSON)  
✅ **Strongly Typed** (compile-time safety)  
✅ **Language Agnostic** (works with many languages)  
✅ **Zero-Copy Reads** (efficient parsing)  

---

## 📝 .proto File Format

### Basic Syntax

```protobuf
syntax = "proto3";

package example.v1;

message Person {
  string name = 1;
  int32 age = 2;
  string email = 3;
}

message Book {
  string title = 1;
  string author = 2;
  int32 pages = 3;
  Person publisher = 4;  // Nested message
}
```

### Field Numbers

```protobuf
message Person {
  string name = 1;      // Field number 1 (never change!)
  int32 age = 2;        // Field number 2 (never change!)
  string email = 3;     // Field number 3 (never change!)
}

// Why numbers?
// - Backward compatibility (if you rename fields, number stays same)
// - Compact binary encoding (numbers are smaller than names)
```

### Wire Types

```protobuf
// Protobuf uses wire types for encoding:
// 0 = Varint (int32, int64, enum)
// 1 = Fixed64 (double, fixed64)
// 2 = Length-delimited (string, bytes, nested messages)
// 5 = Fixed32 (float, fixed32)
```

---

## 🔧 protobuf-net Library

### Installation

```bash
dotnet add package protobuf-net
```

### Usage

```csharp
using ProtoBuf;
using System.IO;

[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string Name { get; set; }
    
    [ProtoMember(2)]
    public int Age { get; set; }
    
    [ProtoMember(3)]
    public string? Email { get; set; }
}

// Serialize
var person = new Person { Name = "Alice", Age = 30, Email = "alice@example.com" };
var ms = new MemoryStream();
Serializer.Serialize(ms, person);
byte[] data = ms.ToArray();
Console.WriteLine($"Serialized size: {data.Length} bytes");  // ~30 bytes vs 50+ for JSON

// Deserialize
ms.Position = 0;
var deserialized = Serializer.Deserialize<Person>(ms);
Console.WriteLine($"Name: {deserialized.Name}");
```

---

## 🔄 Nested Messages

```protobuf
message Address {
  string street = 1;
  string city = 2;
  string zip = 3;
}

message Person {
  string name = 1;
  Address address = 2;  // Nested message
  repeated string phone_numbers = 3;
}
```

### C# Implementation

```csharp
[ProtoContract]
public class Address
{
    [ProtoMember(1)]
    public string Street { get; set; }
    
    [ProtoMember(2)]
    public string City { get; set; }
}

[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string Name { get; set; }
    
    [ProtoMember(2)]
    public Address Address { get; set; }  // Nested!
    
    [ProtoMember(3)]
    public List<string> PhoneNumbers { get; set; }
}
```

---

## 📡 gRPC Integration

### What is gRPC?

gRPC = Google RPC Framework using Protocol Buffers

```protobuf
// .proto file
syntax = "proto3";

service GreeterService {
  rpc SayHello (HelloRequest) returns (HelloReply) {}
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

### C# gRPC Server

```csharp
public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name}"
        });
    }
}

// Usage
var builder = WebApplication.CreateBuilder();
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<GreeterService>();
app.Run();
```

---

## ⚡ Performance Characteristics

### Benchmark Results (serializing 10,000 objects)

```
Format          Size (KB)   Time (ms)   Speed vs JSON
─────────────────────────────────────────────────────
JSON            350         35         1.0x (baseline)
Binary (custom) 100         12         2.9x
Protobuf        80          8          4.4x
Protobuf (AOT)  80          5          7.0x
```

---

## 🔐 Version Safety

```protobuf
// V1
message Person {
  string name = 1;
  int32 age = 2;
}

// V2 - Adding new field is safe!
message Person {
  string name = 1;
  int32 age = 2;
  string email = 3;  // ← New field!
}

// V1 client sends: name="Alice", age=30
// V2 server receives: name="Alice", age=30, email="" (default)
// ✅ No breaking changes!

// V3 - Removing field (careful!)
message Person {
  string name = 1;
  // int32 age = 2;  // ← Removed, but number reserved!
  reserved 2;  // Prevent reuse
  string email = 3;
}
```

---

## 💡 Use Cases

**✅ Perfect for:**
- High-frequency trading systems
- IoT device communication
- Microservices APIs
- Real-time applications
- Mobile apps (bandwidth sensitive)

**❌ Not for:**
- Human-readable data exchange
- Simple REST APIs (use JSON)
- One-off scripts
- Browser-based APIs

---

## 📚 Best Practices

✅ Do:
- Use field numbers consistently
- Never reuse removed field numbers
- Plan schema evolution
- Version your .proto files
- Document message contracts
- Use reserved for removed fields

❌ Don't:
- Change field types (unsafe)
- Reuse field numbers
- Forget versioning
- Mix protobuf with JSON APIs
- Assume backward compat without testing

---

## 🎯 Example Workflow

```
1. Define .proto file (Person message)
   ↓
2. Generate C# classes
   ↓
3. Implement [ProtoContract] attributes
   ↓
4. Serialize/Deserialize with Serializer
   ↓
5. Ship in gRPC service
```

---

## 📚 Summary

**Protocol Buffers** = Industry standard for high-performance APIs

**Kluczowe cechy:**
- ✅ Compact binary format (30% JSON size)
- ✅ Fast serialization (5-10x faster)
- ✅ Version safe (forward/backward compat)
- ✅ Language agnostic
- ✅ gRPC ecosystem

**Best Practice:** Use for performance-critical systems!

---

## 🎯 Następny Temat

Temat 10: Performance & Security - Benchmarks, Vulnerabilities, Best Practices
