# Ćwiczenia: Protocol Buffers

## 🟢 Basic Level

### Zadanie 1: Field Numbers
Dlaczego field numbers są ważne?

**Rozwiązanie:**
```
- Binary encoding uses numbers (smaller)
- Backward compatibility (rename fields safely)
- Forward compatibility (add new fields)
- NEVER change numbers!
```

---

### Zadanie 2: .proto Syntax
Napisz message dla Book:

```
title, author, pages, publisher
```

**Rozwiązanie:**
```protobuf
message Book {
  string title = 1;
  string author = 2;
  int32 pages = 3;
  string publisher = 4;
}
```

---

### Zadanie 3: Wire Types
Które fields używają Type 2 (Length-delimited)?

A. int32  
B. string  
C. double  
D. bytes  

**Rozwiązanie:** B, D (string i bytes = length-delimited)

---

### Zadanie 4: Size Comparison
JSON jest ~100 bytes, ile Protobuf?

**Rozwiązanie:** ~30 bytes (roughly 30% of JSON)

---

### Zadanie 5: gRPC
Czym jest gRPC?

**Rozwiązanie:**
```
Google RPC Framework
- Uses Protobuf for serialization
- HTTP/2 transport
- Supports streaming
- ~10x faster than REST
```

---

## 🟡 Intermediate Level

### Zadanie 6: Version Evolution
Dodaj email bez breaking change:

```protobuf
// V1
message Person {
  string name = 1;
  int32 age = 2;
}

// V2 - Add email
???
```

**Rozwiązanie:**
```protobuf
message Person {
  string name = 1;
  int32 age = 2;
  string email = 3;  // ← New field, different number
}
```

---

### Zadanie 7: Reserved Fields
Usuń age pole bezpiecznie:

```protobuf
message Person {
  string name = 1;
  // int32 age = 2;  // ← Removed
  ???  // Prevent reuse of number 2
}
```

**Rozwiązanie:**
```protobuf
message Person {
  string name = 1;
  reserved 2;  // ← Prevent reuse
}
```

---

### Zadanie 8: Nested Messages
Zagnieżdż Address:

```protobuf
message Address {
  string street = 1;
  string city = 2;
}

message Person {
  string name = 1;
  ???  // Embed Address
}
```

**Rozwiązanie:**
```protobuf
message Person {
  string name = 1;
  Address address = 2;  // ← Nested message
}
```

---

### Zadanie 9: Repeated Fields
Przechowuj listę telefono numerów:

```protobuf
message Person {
  string name = 1;
  ???  // List of phone numbers
}
```

**Rozwiązanie:**
```protobuf
message Person {
  string name = 1;
  repeated string phone_numbers = 2;
}
```

---

### Zadanie 10: Proto Attributes
C# atrybuty dla protobuf:

```csharp
???  // Class attribute
public class Person {
    ???  // Property attribute
    public string Name { get; set; }
}
```

**Rozwiązanie:**
```csharp
[ProtoContract]
public class Person {
    [ProtoMember(1)]
    public string Name { get; set; }
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Compatibility Matrix
Które kombinacje działają?

```
V1 Client ↔ V1 Server: ✓
V1 Client ↔ V2 Server: ?
V2 Client ↔ V1 Server: ?
V2 Client ↔ V2 Server: ✓
```

**Rozwiązanie:**
```
V1 Client ↔ V1 Server: ✓ (exact match)
V1 Client ↔ V2 Server: ✓ (new fields ignored)
V2 Client ↔ V1 Server: ✓ (new fields ignored)
V2 Client ↔ V2 Server: ✓ (exact match)
```

---

### Zadanie 12: gRPC Service
Zdefiniuj gRPC service:

```protobuf
service GreeterService {
  ???  // RPC method
}

message HelloRequest {
  string name = 1;
}

message HelloReply {
  string message = 1;
}
```

---

## 📊 Wskazówki

- ✅ Field numbers = immutable
- ✅ Add fields with new numbers
- ✅ Mark removed fields as reserved
- ✅ Use repeated for lists
- ✅ Plan versioning strategy
- ✅ Test version compatibility
- ❌ Nie zmieniaj field numbers
- ❌ Nie reuse removed numbers
- ❌ Nie change field types
- ❌ Nie use protobuf for human-readable data

---

## 🎯 Key Takeaways

Protocol Buffers:

```
Binary format: Compact (30% JSON size)
Performance: 5-10x faster than JSON
Versioning: Safe (forward/backward compatible)
Ecosystem: gRPC, microservices
Field Numbers: IMMUTABLE!
```

Użyj protobuf dla: High-performance APIs
Nie używaj dla: Human-readable data, simple REST
