# Ćwiczenia: JSON Serialization

## 🟢 Basic Level

### Zadanie 1: JsonSerializer Basics
Napisz kod do serializacji:

```csharp
public class Person { public string Name { get; set; } public int Age { get; set; } }
```

**Rozwiązanie:**
```csharp
var person = new Person { Name = "Alice", Age = 30 };
var json = JsonSerializer.Serialize(person);
// Output: {"Name":"Alice","Age":30}

var restored = JsonSerializer.Deserialize<Person>(json);
```

---

### Zadanie 2: Pretty-Print JSON
Formatuj JSON ze wcięciami:

**Rozwiązanie:**
```csharp
var options = new JsonSerializerOptions { WriteIndented = true };
var json = JsonSerializer.Serialize(person, options);

// Output:
// {
//   "Name": "Alice",
//   "Age": 30
// }
```

---

### Zadanie 3: [JsonPropertyName]
Zmień JSON property names:

```csharp
public class Person
{
    ???  // Jak zmienić "Name" na "firstName"?
    public string Name { get; set; }
}
```

**Rozwiązanie:**
```csharp
[JsonPropertyName("firstName")]
public string Name { get; set; }

// JSON output: {"firstName":"Alice"}
```

---

### Zadanie 4: [JsonIgnore]
Pomiń field w serializacji:

```csharp
public class Employee
{
    public string Name { get; set; }
    ???  // Jak exclude Password?
    public string Password { get; set; }
}
```

**Rozwiązanie:**
```csharp
[JsonIgnore]
public string Password { get; set; }

// JSON output nie zawiera Password
```

---

### Zadanie 5: CamelCase Naming
Konwertuj property names na camelCase:

**Rozwiązanie:**
```csharp
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

var json = JsonSerializer.Serialize(person, options);
// {"name":"Alice","age":30}  ← lowercase!
```

---

## 🟡 Intermediate Level

### Zadanie 6: Nested Objects
Serializuj skomplikowaną strukturę:

```csharp
public class Team
{
    public string Name { get; set; }
    public List<Person> Members { get; set; }
}
```

**Rozwiązanie:**
```csharp
var team = new Team
{
    Name = "Engineers",
    Members = new() 
    { 
        new Person { Name = "Alice", Age = 30 },
        new Person { Name = "Bob", Age = 35 }
    }
};

var json = JsonSerializer.Serialize(team, new JsonSerializerOptions { WriteIndented = true });

// Output:
// {
//   "Name": "Engineers",
//   "Members": [
//     {"Name":"Alice","Age":30},
//     {"Name":"Bob","Age":35}
//   ]
// }
```

---

### Zadanie 7: Enum Serialization
Serializuj enum values:

```csharp
public enum Status { Active, Inactive }

public class User
{
    public string Name { get; set; }
    public Status Status { get; set; }
}
```

**Rozwiązanie:**
```csharp
var options = new JsonSerializerOptions
{
    Converters = { new JsonStringEnumConverter() }
};

var user = new User { Name = "Alice", Status = Status.Active };
var json = JsonSerializer.Serialize(user, options);
// {"Name":"Alice","Status":"Active"}
```

---

### Zadanie 8: Null Handling
Pomiń pola które są null:

**Rozwiązanie:**
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

### Zadanie 9: Performance Test
Porównaj czasy:

```
System.Text.Json: ?
Source Generators: ?
Newtonsoft.Json: ?
```

**Rozwiązanie (10,000 round-trips):**
```
System.Text.Json: 35ms
Source Generators: 7ms (5x faster!)
Newtonsoft.Json: 65ms
```

---

### Zadanie 10: REST API Example
Wysłanie danych do API:

```csharp
using var client = new HttpClient();
var person = new Person { Name = "Alice", Age = 30 };

// TODO: Serialize and POST
```

**Rozwiązanie:**
```csharp
var json = JsonSerializer.Serialize(person);
var content = new StringContent(json, Encoding.UTF8, "application/json");
var response = await client.PostAsync("https://api.example.com/people", content);
```

---

## 🔴 Advanced Level

### Zadanie 11: Custom JSON Converter
Zaimplementuj custom date format:

```csharp
public class Person
{
    public string Name { get; set; }
    
    [JsonConverter(typeof(CustomDateConverter))]
    public DateTime BirthDate { get; set; }
}
```

**Rozwiązanie:**
```csharp
public class CustomDateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateStr = reader.GetString();
        return DateTime.ParseExact(dateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}

// Usage:
var json = JsonSerializer.Serialize(person);
// {"Name":"Alice","BirthDate":"1990-05-15"}
```

---

### Zadanie 12: Source Generators
Zadeklaruj context do serializacji bez reflection:

```csharp
// .NET 5+
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true)]
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(Team))]
internal partial class AppJsonSerializerContext 
    : JsonSerializerContext { }

// Usage:
var json = JsonSerializer.Serialize(person, 
    AppJsonSerializerContext.Default.Person);
```

**Zalety:**
```
✅ Zero reflection at runtime
✅ AOT compatible
✅ Type-safe code generation
✅ Fastest JSON serialization
✅ Trimming-friendly for small binaries
```

---

## 📊 Wskazówki

- ✅ Zawsze używaj System.Text.Json (built-in)
- ✅ Używaj Source Generators dla high-performance apps
- ✅ Zaaplikuj [JsonPropertyName] dla custom naming
- ✅ Zaaplikuj [JsonIgnore] na sensitive fields
- ✅ Test round-trip serialization
- ✅ Używaj JsonSerializerOptions dla consistency
- ✅ Obsługuj nulls za pomocą DefaultIgnoreCondition
- ❌ Nie ignoruj Enum handling
- ❌ Nie deserializuj bez validation
- ❌ Nie zapomnij about performance (Source Generators!)

---

## 🎯 Key Takeaways

```
JSON Serialization: System.Text.Json

2019: First release in .NET Core 3.0
      Fast, built-in, safe

2020: Source Generators (.NET 5+)
      Zero reflection, AOT-ready

2024: Mature ecosystem
      Standard for REST API
      Recommended default
```

Pamiętaj: **System.Text.Json = Modern Standard**

```
DO NOT use:
❌ BinaryFormatter (RCE risk)
❌ Newtonsoft.Json (use STJ instead)
❌ XmlSerializer (for APIs)

DO use:
✅ System.Text.Json (default)
✅ Source Generators (performance)
✅ Protocol Buffers (gRPC)
```
