# Temat 6: Custom Serialization - ISerializable, Converters, Advanced Patterns

## 🔧 Czym jest Custom Serialization?

**Custom Serialization** = definiowanie własnej logiki serializacji dla specificznych typów.

Kiedy użyć:
- Pola private/internal muszą być serializowane
- Specjalne transformacje danych
- Backwards compatibility maintenance
- Custom format requirements

---

## 📋 ISerializable Interface

### .NET Framework Approach (Legacy)

```csharp
[Serializable]
public class Person : ISerializable
{
    public string Name { get; set; }
    private string internalId;  // Private field
    
    // Special handling for deserialization
    protected Person(SerializationInfo info, StreamingContext context)
    {
        Name = info.GetString("Name");
        internalId = info.GetString("InternalId");
    }
    
    // Custom serialization logic
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("Name", Name);
        info.AddValue("InternalId", internalId);
    }
}
```

**⚠️ Note:** ISerializable mainly for BinaryFormatter (deprecated). Use alternatives instead!

---

## 🔄 IXmlSerializable Interface

### Custom XML Format Control

```csharp
public class Person : IXmlSerializable
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Define schema (can return null for auto)
    public XmlSchema GetSchema() => null;
    
    // Custom deserialization from XML
    public void ReadXml(XmlReader reader)
    {
        reader.MoveToAttribute("name");
        Name = reader.Value;
        
        reader.MoveToAttribute("age");
        Age = int.Parse(reader.Value);
    }
    
    // Custom serialization to XML
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("name", Name);
        writer.WriteAttributeString("age", Age.ToString());
    }
}

// Usage:
var person = new Person { Name = "Alice", Age = 30 };
var xs = new XmlSerializer(typeof(Person));
xs.Serialize(stream, person);  // Uses custom WriteXml()
```

**Output:**
```xml
<Person name="Alice" age="30" />
```

---

## 🎨 Custom JSON Converters

### System.Text.Json Converter

```csharp
public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";
    
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateStr = reader.GetString();
        return DateTime.ParseExact(dateStr, Format, CultureInfo.InvariantCulture);
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

// Usage:
public class Event
{
    public string Name { get; set; }
    
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}

var options = new JsonSerializerOptions
{
    Converters = { new DateTimeConverter() }
};

var json = JsonSerializer.Serialize(new Event 
{ 
    Name = "Birthday", 
    Date = new DateTime(1990, 5, 15) 
}, options);

// Output: {"Name":"Birthday","Date":"1990-05-15"}
```

---

## 🛡️ Validation During Serialization

```csharp
public class Person
{
    private int age;
    
    public string Name { get; set; }
    
    public int Age
    {
        get => age;
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Invalid age");
            age = value;
        }
    }
}

public class AgeConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt32();
        if (value < 0 || value > 150)
            throw new JsonException("Age must be 0-150");
        return value;
    }
    
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        if (value < 0 || value > 150)
            throw new ArgumentException("Invalid age");
        writer.WriteNumberValue(value);
    }
}
```

---

## 🔌 Serialization Surrogate Pattern

```csharp
// Original type (can't modify)
public class LegacyPerson
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Surrogate for serialization
public class LegacyPersonSurrogate
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Convert from original
    public static LegacyPersonSurrogate FromPerson(LegacyPerson person)
        => new() { Name = person.Name, Age = person.Age };
    
    // Convert back to original
    public LegacyPerson ToPerson()
        => new() { Name = Name, Age = Age };
}

// Usage with surrogates:
var original = new LegacyPerson { Name = "Alice", Age = 30 };
var surrogate = LegacyPersonSurrogate.FromPerson(original);
var json = JsonSerializer.Serialize(surrogate);
var restored = JsonSerializer.Deserialize<LegacyPersonSurrogate>(json).ToPerson();
```

---

## 🎯 Custom Serialization Pattern Example

```csharp
public class SecureData
{
    public string Username { get; set; }
    private string password;
    
    // Custom serialization (exclude password, add timestamp)
    public SerializedData Serialize()
    {
        return new SerializedData
        {
            Username = Username,
            SerializedAt = DateTime.UtcNow,
            // Password NOT included!
            Hash = ComputeHash(Username)
        };
    }
    
    private string ComputeHash(string data)
        => Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data));
}

public class SerializedData
{
    public string Username { get; set; }
    public DateTime SerializedAt { get; set; }
    public string Hash { get; set; }
}

// Usage:
var data = new SecureData { Username = "alice", password = "secret" };
var serialized = data.Serialize();  // Password never exposed
var json = JsonSerializer.Serialize(serialized);
```

---

## 🔄 Handling Type Conversions

```csharp
public class FlexibleConverter<T> : JsonConverter<T>
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => (T)(object)reader.GetString(),
            JsonTokenType.Number => (T)(object)reader.GetInt32(),
            JsonTokenType.True or JsonTokenType.False => (T)(object)reader.GetBoolean(),
            _ => throw new JsonException($"Unexpected token: {reader.TokenType}")
        };
    }
    
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        if (value is string s)
            writer.WriteStringValue(s);
        else if (value is int i)
            writer.WriteNumberValue(i);
        else if (value is bool b)
            writer.WriteBooleanValue(b);
    }
}
```

---

## ⚙️ Conditional Serialization

```csharp
public class Person
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    
    // Only serialize public fields?
    public bool ShouldSerializeEmail()
        => !string.IsNullOrEmpty(Email);
    
    public bool ShouldSerializePhoneNumber()
        => !string.IsNullOrEmpty(PhoneNumber);
}

// With System.Text.Json:
var options = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

var person = new Person { Name = "Alice", Email = null };
var json = JsonSerializer.Serialize(person, options);
// Output: {"Name":"Alice"} - Email omitted!
```

---

## 📚 Best Practices for Custom Serialization

✅ Do:
- Keep serialization logic separate from domain logic
- Validate data during deserialization
- Handle null/missing fields gracefully
- Document format changes for versioning
- Test round-trip (serialize → deserialize)
- Use converters for type transformations

❌ Don't:
- Serialize sensitive data (passwords, tokens)
- Ignore validation errors
- Hard-code formats (use options)
- Forget backwards compatibility
- Mix serialization with business logic

---

## 🔐 Security Considerations

### ❌ UNSAFE
```csharp
// Direct serialization of sensitive data
var json = JsonSerializer.Serialize(new User 
{ 
    Name = "alice",
    Password = "secret123"  // ❌ EXPOSED!
});
```

### ✅ SAFE
```csharp
// Custom converter excludes sensitive fields
[JsonConverter(typeof(SafeUserConverter))]
public class User
{
    public string Name { get; set; }
    [JsonIgnore]
    public string Password { get; set; }
}

var json = JsonSerializer.Serialize(new User 
{ 
    Name = "alice",
    Password = "secret123"
});
// {"Name":"alice"} - Password excluded!
```

---

## 📊 Summary of Custom Serialization Approaches

| Approach | Use Case | Complexity | Performance |
|----------|----------|-----------|-------------|
| `[JsonIgnore]` | Simple field exclusion | Very Low | Excellent |
| `JsonConverter<T>` | Type transformation | Low | Good |
| `IXmlSerializable` | Custom XML format | Medium | Medium |
| `Surrogate Pattern` | Legacy type wrapping | Medium | Good |
| `Custom method` | Full control | High | Controllable |

---

## 📚 Referencje

- [Microsoft: JsonConverter Guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/custom-converters)
- [Custom Serialization](https://learn.microsoft.com/en-us/dotnet/standard/serialization/custom-serialization)

---

## ✅ Summary

**Custom Serialization** = kontrola nad sposobem serializacji obiektu.

**Podejścia:**
- ✅ `[JsonIgnore]` - Simple exclusion
- ✅ `JsonConverter<T>` - Type transformation
- ✅ `IXmlSerializable` - XML control
- ✅ Surrogate Pattern - Legacy wrapping
- ⚠️ `ISerializable` - Mainly BinaryFormatter (deprecated)

**Najlepiej dla:**
- Transformacji danych
- Bezpieczeństwa (exclude sensitive fields)
- Backwards compatibility
- Specjalnych formatów

---

## 🎯 Następny Temat

Temat 7: Circular References & Advanced Patterns
