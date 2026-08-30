# Ćwiczenia: Binary Serialization

## 🟢 Basic Level

### Zadanie 1: BinaryFormatter Status
Jaki jest status BinaryFormatter w .NET?

**Rozwiązanie:**
```
❌ DEPRECATED
- Removed in .NET 8+
- Security vulnerabilities (RCE)
- Not suitable for new projects
- Should migrate immediately
```

---

### Zadanie 2: RCE Risk
Co to jest RCE (Remote Code Execution)?

**Rozwiązanie:**
```
Remote Code Execution:

Attacker sends malicious binary:
├─ Contains embedded type: WindowsIdentity
├─ Contains method call: Impersonate()
│
Deserializer reads:
├─ Loads type from stream
├─ Auto-invokes Impersonate() ← ❌ AUTOMATIC!
│
Result: Attacker's code runs on your server!
```

---

### Zadanie 3: Custom Binary Size
Estymuj rozmiar serializacji dla:

```csharp
Person { Name = "Alice" (5 chars), Age = 30 }
```

Format: [Version:1][NameLength:4][Name:5][Age:4]

**Rozwiązanie:**
```
Total: 1 + 4 + 5 + 4 = 14 bytes

(BinaryFormatter: ~50+ bytes with overhead)
```

---

### Zadanie 4: Bezpieczeństwo
Która metoda jest SAFE?

A. `bf.Deserialize(untrustedData)`  
B. `JsonSerializer.Deserialize<T>(untrustedData)`  
C. `CustomBinary.Deserialize(untrustedData)`  

**Rozwiązanie:**
```
B, C (A is RCE risk)

System.Text.Json: Safe (no auto-invocation)
Custom Binary: Safe (explicit decode)
BinaryFormatter: Unsafe (RCE!)
```

---

### Zadanie 5: Migrations
Gdzie migrować z BinaryFormatter?

**Rozwiązanie:**
```
✅ System.Text.Json - Default choice
✅ Protocol Buffers - High performance
✅ Custom Binary - Full control
❌ BinaryFormatter - NEVER
```

---

## 🟡 Intermediate Level

### Zadanie 6: Custom Binary Implementation
Zaimplementuj Serialize/Deserialize dla klasy:

```csharp
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

**Rozwiązanie:**
```csharp
public byte[] Serialize()
{
    using var ms = new MemoryStream();
    using var writer = new BinaryWriter(ms);
    
    writer.Write((byte)1);  // Version
    writer.Write(Name ?? "");
    writer.Write(Price);
    
    return ms.ToArray();
}

public static Product Deserialize(byte[] data)
{
    using var ms = new MemoryStream(data);
    using var reader = new BinaryReader(ms);
    
    byte version = reader.ReadByte();
    if (version != 1) throw new InvalidOperationException();
    
    var name = reader.ReadString();
    var price = reader.ReadDecimal();
    
    return new Product { Name = name, Price = price };
}
```

---

### Zadanie 7: Versioning
Jak dodać nowe pole bez złamania starych danych?

```csharp
// V1
public class Person { public string Name; }

// V2 (add Email field)
public class Person { public string Name; public string Email; }
```

**Rozwiązanie:**
```csharp
public byte[] Serialize()
{
    using var ms = new MemoryStream();
    using var writer = new BinaryWriter(ms);
    
    writer.Write((byte)2);  // Version 2
    writer.Write(Name ?? "");
    writer.Write(Email ?? "");  // New field
    
    return ms.ToArray();
}

public static Person Deserialize(byte[] data)
{
    using var ms = new MemoryStream(data);
    using var reader = new BinaryReader(ms);
    
    byte version = reader.ReadByte();
    var name = reader.ReadString();
    
    string email = "";
    if (version >= 2)
        email = reader.ReadString();  // Handle v1 without email
    
    return new Person { Name = name, Email = email };
}
```

---

### Zadanie 8: Size Estimation
Porównaj rozmiary dla 100,000 obiektów:

```csharp
Person { Name: 10 chars, Age: 30 }
```

**Rozwiązanie:**
```
Custom Binary:      18 bytes × 100k = 1.8 MB
Protocol Buffers:   14 bytes × 100k = 1.4 MB
System.Text.Json:   30 bytes × 100k = 3.0 MB
XML:               100 bytes × 100k = 10.0 MB
BinaryFormatter:    60 bytes × 100k = 6.0 MB (+ UNSAFE!)

Custom Binary najlepszy dla kompaktowości
```

---

### Zadanie 9: Performance Test
Porównaj czasy dla 10,000 round-trips:

```
BinaryFormatter: ?
Custom Binary:   ?
Protocol Buffers: ?
JSON:            ?
```

**Rozwiązanie (estimated):**
```
BinaryFormatter: 1500ms (slow + unsafe)
Custom Binary:   200ms (7.5x faster)
Protocol Buffers: 150ms (10x faster)
System.Text.Json: 200ms (7.5x faster)
```

---

### Zadanie 10: Handle Collections
Jak serializować List<T>?

```csharp
public class Team
{
    public string Name { get; set; }
    public List<Person> Members { get; set; }
}
```

**Rozwiązanie:**
```csharp
public byte[] Serialize()
{
    using var ms = new MemoryStream();
    using var writer = new BinaryWriter(ms);
    
    writer.Write((byte)1);
    writer.Write(Name ?? "");
    
    // Serialize collection
    writer.Write(Members.Count);  // Count
    foreach (var member in Members)
    {
        writer.Write(member.Name ?? "");
        writer.Write(member.Age);
    }
    
    return ms.ToArray();
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Circular Reference Handling
Jak obsługiwać grafy cykliczne w Binary?

```csharp
var a = new Node { Name = "A" };
var b = new Node { Name = "B" };
a.Next = b;
b.Next = a;  // Cycle!
```

**Rozwiązanie:**
```csharp
public byte[] Serialize()
{
    var visited = new HashSet<Node>();
    using var ms = new MemoryStream();
    using var writer = new BinaryWriter(ms);
    
    SerializeNode(this, writer, visited);
    
    return ms.ToArray();
}

private void SerializeNode(Node node, BinaryWriter writer, HashSet<Node> visited)
{
    if (node == null || visited.Contains(node))
    {
        writer.Write((byte)0);  // Marker: null/visited
        return;
    }
    
    visited.Add(node);
    writer.Write((byte)1);  // Marker: valid node
    writer.Write(node.Name ?? "");
    
    SerializeNode(node.Next, writer, visited);
}
```

---

### Zadanie 12: RCE Attack Simulation
Wyjaśnij, jak by wyglądał RCE attack z BinaryFormatter:

```csharp
// Attacker craft:
var payload = CraftMaliciousPayload(
    type: "WindowsIdentity",
    method: "Impersonate",
    param: "SYSTEM"
);

// Your code:
var bf = new BinaryFormatter();
var obj = bf.Deserialize(payload);  // ❌ RCE!
```

**Rozwiązanie:**
```
Timeline:
1. Attacker sends binary stream
2. Stream contains type info: WindowsIdentity
3. Stream contains method: Impersonate(token)
4. Deserializer:
   ├─ Reads type → Loads WindowsIdentity class
   ├─ Reads constructor params → Creates instance
   ├─ Reads method calls → INVOKES Impersonate() ← ❌
   └─ Attacker now has SYSTEM privileges!

Fix: Never use BinaryFormatter on untrusted data!
Use: System.Text.Json, Protocol Buffers, Custom Binary
```

---

## 📊 Wskazówki

- ✅ Zapamiętaj: BinaryFormatter = UNSAFE
- ✅ Zawsze use: System.Text.Json, Protocol Buffers, lub Custom Binary
- ✅ Test round-trip: Serialize → Deserialize → Verify
- ✅ Obsługuj versioning (byte version header)
- ✅ Handle collections z Count prefix
- ❌ Nigdy nie ignoruj Security!
- ❌ Nie deserializuj untrusted binary

---

## 🎯 Key Takeaways

```
BinaryFormatter Timeline:
2002: Introduced
2017: First RCE vulnerabilities
2020: Deprecated
2023: Removed

Alternatives (2024):
✅ System.Text.Json - Default
✅ Protocol Buffers - Performance
✅ Custom Binary - Control
```

Pamiętaj: **Zawsze custom binary lub modern formats!**
