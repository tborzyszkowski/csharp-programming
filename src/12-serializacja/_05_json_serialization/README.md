# Temat 5: JSON Serialization - System.Text.Json, Modern Approach

## 📋 Czym jest JSON Serialization?

**JSON Serialization** = konwersja obiektu do formatu JSON (JavaScript Object Notation).

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person = new Person { Name = "Alice", Age = 30 };

// Serializacja do JSON
var json = JsonSerializer.Serialize(person);
// Output: {"Name":"Alice","Age":30}

// Deserializacja z JSON
var restored = JsonSerializer.Deserialize<Person>(json);
```

---

## ✅ Dlaczego JSON?

1. **Universal** - Każdy język obsługuje JSON
2. **Human Readable** - Można czytać i edytować
3. **Web Standard** - Default dla REST API
4. **Fast** - Nowsze implementacje szybkie
5. **Safe** - No auto-invocation (unlike BinaryFormatter)
6. **Compact** - Mniej verbose niż XML
7. **Versioning** - Łatwo dodać nowe pola

---

## 🎯 System.Text.Json (Official Microsoft)

### Podstawowy Przykład

```csharp
using System.Text.Json;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person = new Person { Name = "Alice", Age = 30 };

// Serialize
var json = JsonSerializer.Serialize(person);
// {"Name":"Alice","Age":30}

// Deserialize
var restored = JsonSerializer.Deserialize<Person>(json);
```

### Konfiguracja

```csharp
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // name, age
    WriteIndented = true,                               // Pretty-print
    PropertyNameCaseInsensitive = true,                 // "NAME" == "name"
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    Converters = { new JsonStringEnumConverter() }
};

var json = JsonSerializer.Serialize(person, options);
```

### Atrybuty

```csharp
public class Person
{
    [JsonPropertyName("firstName")]  // Custom JSON name
    public string Name { get; set; }
    
    [JsonIgnore]  // Don't serialize
    public string Password { get; set; }
    
    [JsonInclude]  // Include private field
    private string internalId = "";
}

// Output:
// {"firstName":"Alice"}
```

---

## 🔧 Opcje Zaawansowane

### Null Handling

```csharp
var options = new JsonSerializerOptions
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};

var person = new Person { Name = "Alice", Age = null };
var json = JsonSerializer.Serialize(person, options);
// {"Name":"Alice"} - Age omitted because null
```

### Date/Time Formatting

```csharp
var options = new JsonSerializerOptions
{
    Converters =
    {
        new JsonStringEnumConverter(),
        new DateTimeConverter()  // Custom converter
    }
};

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}
```

### Enum Support

```csharp
public enum Status { Active, Inactive }

public class User
{
    public string Name { get; set; }
    public Status Status { get; set; }  // Enum!
}

var options = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }
};

var json = JsonSerializer.Serialize(new User { Name = "Alice", Status = Status.Active }, options);
// {"Name":"Alice","Status":"Active"}
```

---

## 🚀 Source Generators (Zero Reflection)

### .NET 5+ Feature

```csharp
// Compile-time code generation
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true)]
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(Team))]
internal partial class AppJsonSerializerContext 
    : JsonSerializerContext { }

// Usage (NO REFLECTION!)
var person = new Person { Name = "Alice", Age = 30 };
var json = JsonSerializer.Serialize(person, AppJsonSerializerContext.Default.Person);
```

**Zaletę Source Generators:**
- ✅ Zero reflection at runtime
- ✅ AOT (Ahead-of-Time) compatible
- ✅ Trimming safe (for smaller apps)
- ✅ Type-safe code generation
- ✅ Fastest JSON serialization

---

## 📊 Porównanie Formatów

```
Object: Person { Name: "Alice", Age: 30 }

Format          Output                              Size
────────────────────────────────────────────────────
Binary          [Binary blob]                       14 bytes
JSON (compact)  {"Name":"Alice","Age":30}          26 bytes
JSON (pretty)   {\n  "Name": "Alice"\n}            40 bytes
XML             <Person><Name>Alice</Name>...</>   96 bytes
```

---

## 🔄 Collections & Nested Objects

### Simple Collections

```csharp
public class Team
{
    public string Name { get; set; }
    public List<string> Members { get; set; }
}

var team = new Team 
{ 
    Name = "Engineers",
    Members = new() { "Alice", "Bob", "Charlie" }
};

var json = JsonSerializer.Serialize(team);
// {"Name":"Engineers","Members":["Alice","Bob","Charlie"]}
```

### Nested Objects

```csharp
public class Person
{
    public string Name { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string City { get; set; }
    public string Country { get; set; }
}

var person = new Person
{
    Name = "Alice",
    Address = new Address { City = "NYC", Country = "USA" }
};

var json = JsonSerializer.Serialize(person);
// {"Name":"Alice","Address":{"City":"NYC","Country":"USA"}}
```

---

## ⚡ Performance

### Round-Trip Benchmark (10,000 objects)

```
Format              Serialize   Deserialize   Total
─────────────────────────────────────────────────
System.Text.Json    15ms        20ms          35ms
Source Generated    3ms         4ms           7ms
NewtonSoft.Json     30ms        35ms          65ms
Custom Binary       20ms        25ms          45ms
Protocol Buffers    12ms        15ms          27ms
```

**System.Text.Json** jest wystarczająco szybki i wbudowany!

---

## 🌐 REST API Example

### Sending Data

```csharp
using var httpClient = new HttpClient();
var person = new Person { Name = "Alice", Age = 30 };

var json = JsonSerializer.Serialize(person);
var content = new StringContent(json, Encoding.UTF8, "application/json");
var response = await httpClient.PostAsync("https://api.example.com/people", content);
```

### Receiving Data

```csharp
var response = await httpClient.GetAsync("https://api.example.com/people/1");
var json = await response.Content.ReadAsStringAsync();

var person = JsonSerializer.Deserialize<Person>(json);
```

---

## 🔐 Security Considerations

### ✅ Safe by Default

```csharp
// System.Text.Json is safe
var json = "{}";  // Untrusted
var obj = JsonSerializer.Deserialize<Person>(json);  // Safe!
```

**Why safe?**
- No auto-invocation of methods
- Requires explicit type `<T>`
- No reflection-based attacks

### ⚠️ Nullable Reference Types

```csharp
public class Person
{
    [JsonRequired]  // Must be present in JSON
    public string Name { get; set; } = "";
    
    public string? Email { get; set; }  // Optional
}

// Deserialize
var json = "{\"Name\":\"Alice\"}";  // Email optional
var person = JsonSerializer.Deserialize<Person>(json);
```

---

## 🔄 Custom Converters

```csharp
public class Person
{
    public string Name { get; set; }
    
    [JsonConverter(typeof(AgeConverter))]
    public int Age { get; set; }
}

public class AgeConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt32();
        if (value < 0 || value > 150)
            throw new JsonException("Invalid age");
        return value;
    }
    
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        => writer.WriteNumberValue(value);
}
```

---

## 📈 Modern Landscape (2024+)

```
2010: JavaScriptSerializer (simple)
      ↓
2012: Newtonsoft JSON.NET (industry standard)
      ↓
2019: System.Text.Json (official, .NET Core 3.0)
      ↓
2020: Source Generators (.NET 5+, zero reflection)
      ↓
2024: System.Text.Json + Source Generators (recommended)
      ↓
2025+: Native AOT, full trimming support
```

---

## ✅ Best Practices

✅ Do:
- Używaj `System.Text.Json` dla nowych projektów
- Używaj Source Generators dla high-performance apps
- Zaaplikuj `[JsonPropertyName]` dla custom names
- Zaaplikuj `[JsonIgnore]` na sensitive fields
- Test round-trip serialization
- Używaj `JsonSerializerOptions` dla consistency

❌ Nie rób:
- Nie używaj `BinaryFormatter`
- Nie ignoruj Null handling
- Nie deserializuj untrusted JSON bez validation
- Nie zapomnij versioning strategy

---

## 📚 Referencje

- [Microsoft: System.Text.Json Guide](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/)
- [Source Generators Tutorial](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)
- [JSON Naming Policies](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonnamingpolicy)

---

## ✅ Summary

**JSON Serialization** to modern standard format dla serializacji w .NET.

**System.Text.Json:**
- ✅ Built-in (no NuGet needed)
- ✅ Fast (optimized for .NET)
- ✅ Safe (no RCE risk)
- ✅ AOT-ready (Source Generators)
- ✅ Web standard (REST API)

**Charakterystyka:**
- Human readable
- Universal (każdy język)
- Kompaktowy
- Szybki
- Bezpieczny

**Kiedy używać:**
- REST API (default)
- Web services
- Config files
- Data interchange

**Rekomendacja 2024:** Zawsze `System.Text.Json` + opcjonalnie Source Generators dla performance.

---

## 🎯 Następny Temat

Temat 6: Custom Serialization - ISerializable, Custom Types, Advanced Patterns
