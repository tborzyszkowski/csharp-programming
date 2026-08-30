# Ćwiczenia: Custom Serialization

## 🟢 Basic Level

### Zadanie 1: [JsonIgnore]
Wyklucz poufne pola:

```csharp
public class User
{
    public string Username { get; set; }
    ???  // Jak exclude Password?
    public string Password { get; set; }
}
```

**Rozwiązanie:**
```csharp
[JsonIgnore]
public string Password { get; set; }
```

---

### Zadanie 2: [JsonConverter]
Zastosuj custom converter:

```csharp
public class Person
{
    public string Name { get; set; }
    ???  // Jak użyć custom date converter?
    public DateTime BirthDate { get; set; }
}
```

**Rozwiązanie:**
```csharp
[JsonConverter(typeof(DateTimeConverter))]
public DateTime BirthDate { get; set; }
```

---

### Zadanie 3: IXmlSerializable
Zaimplementuj interface:

```csharp
public class Person : IXmlSerializable
{
    ???  // Jakie metody wymagane?
}
```

**Rozwiązanie:**
```csharp
public XmlSchema GetSchema() => null;

public void ReadXml(XmlReader reader)
{
    // Read from XML
}

public void WriteXml(XmlWriter writer)
{
    // Write to XML
}
```

---

### Zadanie 4: Custom Date Format
Jak zmienić format daty w JSON?

**Rozwiązanie:**
```csharp
public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";
    
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.ParseExact(reader.GetString(), Format, ...);
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}
```

---

### Zadanie 5: Validation
Validuj dane podczas deserializacji:

```csharp
public class AgeValidator : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, ...)
    {
        var value = reader.GetInt32();
        if (value < 0 || value > 150)
            throw new JsonException("Invalid age");
        return value;
    }
}
```

---

## 🟡 Intermediate Level

### Zadanie 6: Surrogate Pattern
Otwórz wrappera dla legacy type:

```csharp
public class LegacyPerson { ... }  // Can't modify

// TODO: Create surrogate pattern
```

**Rozwiązanie:**
```csharp
public class LegacyPersonSurrogate
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    public static LegacyPersonSurrogate FromPerson(LegacyPerson person)
        => new() { Name = person.Name, Age = person.Age };
    
    public LegacyPerson ToPerson()
        => new() { Name = Name, Age = Age };
}
```

---

### Zadanie 7: Enum Custom Converter
Konwertuj enum na custom format:

```csharp
public enum Status { Active, Inactive }

// Want: "Active" in JSON, not 0, 1
// Want: Custom serialization logic
```

**Rozwiązanie:**
```csharp
public class CustomEnumConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        return (T)Enum.Parse(typeof(T), str, ignoreCase: true);
    }
    
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

var options = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }  // Built-in
};
```

---

### Zadanie 8: Conditional Serialization
Pomiń pola w określonych warunkach:

```csharp
var options = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

var person = new Person { Name = "Alice", Email = null };
var json = JsonSerializer.Serialize(person, options);
// {"Name":"Alice"} ← Email omitted
```

---

### Zadanie 9: Collection Converter
Custom converter dla List<T>:

```csharp
public class CommaSeparatedConverter : JsonConverter<List<string>>
{
    public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();
        return str.Split(',').Select(s => s.Trim()).ToList();
    }
    
    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(string.Join(", ", value));
    }
}

// Usage:
public class Tags
{
    [JsonConverter(typeof(CommaSeparatedConverter))]
    public List<string> Items { get; set; }
}
```

---

### Zadanie 10: Security Pattern
Exclude sensitive fields z custom converter:

```csharp
public class SecureUserConverter : JsonConverter<User>
{
    public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("username", value.Username);
        // Password NOT written!
        writer.WriteEndObject();
    }
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Complex Type Transformation
Konwertuj objekt na inny format:

```csharp
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
}

// Want: Serialize as "Street, City"
// Want: Deserialize from "Street, City"

public class CompactAddressConverter : JsonConverter<Address>
{
    public override Address Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var parts = reader.GetString().Split(", ");
        return new Address { Street = parts[0], City = parts[1] };
    }
    
    public override void Write(Utf8JsonWriter writer, Address value, JsonSerializerOptions options)
    {
        writer.WriteStringValue($"{value.Street}, {value.City}");
    }
}
```

---

### Zadanie 12: Versioning with Custom Converter
Obsłuż starą i nową strukturę:

```csharp
public class PersonConverter : JsonConverter<Person>
{
    public override Person Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        string name;
        if (root.TryGetProperty("FullName", out var fullName))
            name = fullName.GetString();  // V2 format
        else
            name = root.GetProperty("Name").GetString();  // V1 format
        
        return new Person { Name = name };
    }
}
```

---

## 📊 Wskazówki

- ✅ Zawsze exclude sensitive fields ([JsonIgnore])
- ✅ Implementuj JsonConverter<T> dla custom transformacji
- ✅ Validuj data w Read() method
- ✅ Test round-trip: serialize → deserialize
- ✅ Używaj Surrogate Pattern dla legacy types
- ✅ Obsługuj versioning dla backward compatibility
- ❌ Nigdy nie exponuj passwords/tokens
- ❌ Nie ignoruj validation errors
- ❌ Nie zapomnij null handling

---

## 🎯 Key Takeaways

Custom serialization = Kontrola nad format i transformacją

```
Techniki:
[JsonIgnore] - Simple exclusion
JsonConverter<T> - Type transformation
IXmlSerializable - XML control
Surrogate - Legacy wrapping
Custom methods - Full control
```

Pamiętaj: **Security first - exclude sensitive data!**
