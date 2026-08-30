# Temat 8: Advanced Serialization Features - Polymorphism, Versioning, Compatibility

## 📦 Polymorphism in Serialization

### Challenge: Base Class Serialization

```csharp
public class Animal
{
    public string Name { get; set; }
}

public class Dog : Animal
{
    public string Breed { get; set; }
}

public class Cat : Animal
{
    public string Color { get; set; }
}

// Problem: How to serialize List<Animal> with correct types?
public class Zoo
{
    public List<Animal> Animals { get; set; }
}
```

### Solution 1: [JsonDerivedType]

```csharp
[JsonDerivedType(typeof(Dog), "dog")]
[JsonDerivedType(typeof(Cat), "cat")]
public class Animal
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    public string Name { get; set; }
}

public class Dog : Animal { ... }
public class Cat : Animal { ... }

// JSON Output:
// [
//   {"type":"dog","name":"Buddy","breed":"Labrador"},
//   {"type":"cat","name":"Whiskers","color":"Orange"}
// ]
```

### Solution 2: Custom Converter

```csharp
public class AnimalConverter : JsonConverter<Animal>
{
    public override Animal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        var type = root.GetProperty("type").GetString();
        
        return type switch
        {
            "dog" => JsonSerializer.Deserialize<Dog>(root.GetRawText(), options),
            "cat" => JsonSerializer.Deserialize<Cat>(root.GetRawText(), options),
            _ => throw new JsonException($"Unknown type: {type}")
        };
    }
    
    public override void Write(Utf8JsonWriter writer, Animal value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        writer.WriteString("type", value.GetType().Name.ToLower());
        writer.WriteString("name", value.Name);
        
        if (value is Dog dog)
            writer.WriteString("breed", dog.Breed);
        else if (value is Cat cat)
            writer.WriteString("color", cat.Color);
        
        writer.WriteEndObject();
    }
}
```

---

## 📈 Versioning Strategy

### Problem: Format Changes

```csharp
// V1: Simple structure
public class PersonV1
{
    public string Name { get; set; }
}

// V2: Added email
public class PersonV2
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// V3: Renamed Name → FirstName
public class PersonV3
{
    public string FirstName { get; set; }
    public string Email { get; set; }
}

// Challenge: How to read old V1/V2 data into V3?
```

### Solution: Version-Aware Converter

```csharp
public class VersionAwareConverter : JsonConverter<PersonV3>
{
    public override PersonV3 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        // Detect version by checking fields
        string name;
        if (root.TryGetProperty("FirstName", out var firstName))
            name = firstName.GetString();  // V3 format
        else if (root.TryGetProperty("Name", out var nameField))
            name = nameField.GetString();  // V1/V2 format
        else
            throw new JsonException("Missing name");
        
        var email = root.TryGetProperty("Email", out var emailField)
            ? emailField.GetString()
            : "";
        
        return new PersonV3 { FirstName = name, Email = email };
    }
    
    public override void Write(Utf8JsonWriter writer, PersonV3 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("FirstName", value.FirstName);
        writer.WriteString("Email", value.Email);
        writer.WriteEndObject();
    }
}
```

---

## 🔄 Backward Compatibility

### Graceful Degradation

```csharp
public class Person
{
    public string Name { get; set; }
    public string Email { get; set; }
    
    // V2 fields - optional, default if missing
    [JsonPropertyName("phone")]
    public string? Phone { get; set; } = null;
    
    [JsonPropertyName("address")]
    public string? Address { get; set; } = null;
}

// V1 JSON: {"Name":"Alice","Email":"alice@example.com"}
// ↓ Deserialize into V2
// V2 Object: { Name, Email, Phone=null, Address=null }
```

### Handling Removed Fields

```csharp
public class PersonWithExtras
{
    public string Name { get; set; }
    
    // Ignore extra fields from newer versions
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtraData { get; set; }
}

// V2 JSON with extra fields: {"Name":"Alice","PhoneV3":"555-1234"}
// ↓ Deserialize into V1
// V1 Object: { Name="Alice", ExtraData={"PhoneV3": "555-1234"} }
```

---

## ✅ Migration Path

```csharp
// Strategy: Gradual Migration

// Step 1: Support both V1 and V2
public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    // V2 addition - optional
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}

// Step 2: Gradually phase out old format
// Clients update at their own pace

// Step 3: Set deadline, require V2

// Step 4: Add V3 support
// Repeat process
```

---

## 🔐 Type Discriminator Pattern

```csharp
[JsonDerivedType(typeof(AdminUser), "admin")]
[JsonDerivedType(typeof(RegularUser), "user")]
[JsonDerivedType(typeof(GuestUser), "guest")]
public abstract class User
{
    [JsonPropertyName("$type")]  // Discriminator
    public abstract string Type { get; }
    
    public string Username { get; set; }
}

public class AdminUser : User
{
    public override string Type => "admin";
    public bool CanDeleteUsers { get; set; }
}

// JSON:
// {"$type":"admin","username":"root","canDeleteUsers":true}
```

---

## 📊 Compatibility Matrix

| Version | Read V1 | Read V2 | Read V3 |
|---------|---------|---------|---------|
| V1 App  | ✅      | ❌      | ❌      |
| V2 App  | ✅      | ✅      | ❌      |
| V3 App  | ✅      | ✅      | ✅      |

**Best Practice:** Always support reading N-1 versions

---

## 📚 Best Practices

✅ Do:
- Versjonuj format z jasnym strategy
- Support backward compatibility
- Use custom converters dla transitions
- Document version changes
- Test migrations
- Provide deprecation notices

❌ Don't:
- Break backward compatibility abruptly
- Zapomnij about existing clients
- Mix multiple versions w jednej app
- Ignoruj migration planning
- Serialize internal state

---

## 🎯 Version Negotiation

```csharp
public class VersionedPerson
{
    [JsonPropertyName("version")]
    public int Version { get; set; } = 3;  // Current version
    
    public string FirstName { get; set; }
    public string? Email { get; set; }
}

// Client can check version:
var person = JsonSerializer.Deserialize<VersionedPerson>(json);
if (person.Version > 3)
    throw new InvalidOperationException("Unsupported version");
```

---

## 📚 Summary

**Advanced Features** = Polymorphism, Versioning, Compatibility

**Kluczowe elementy:**
- ✅ `[JsonDerivedType]` dla polymorphism
- ✅ Custom converters dla versioning
- ✅ `[JsonExtensionData]` dla graceful degradation
- ✅ Version discriminators
- ✅ Migration strategies

**Best Practice:** Plan for evolution from the start!

---

## 🎯 Następny Temat

Temat 9: Protocol Buffers - Industry Standard, gRPC, Performance
