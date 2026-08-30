# Ćwiczenia: Advanced Features

## 🟢 Basic Level

### Zadanie 1: [JsonDerivedType]
Zadeklaruj typy pochodne:

```csharp
[JsonDerivedType(???)]
[JsonDerivedType(???)]
public abstract class Animal { }
```

**Rozwiązanie:**
```csharp
[JsonDerivedType(typeof(Dog), "dog")]
[JsonDerivedType(typeof(Cat), "cat")]
public abstract class Animal { }
```

---

### Zadanie 2: Type Discriminator
Jak Identificować typ w JSON?

**Rozwiązanie:**
```csharp
// JSON:
{
  "$type": "dog",
  "Name": "Buddy",
  "Breed": "Labrador"
}
```

---

### Zadanie 3: Optional Fields
Dodaj nowe pole bez breaking change:

```csharp
public class Person
{
    public string Name { get; set; }
    ???  // Add Phone (optional)
}
```

**Rozwiązanie:**
```csharp
public string? Phone { get; set; }  // Nullable = optional
```

---

### Zadanie 4: Version Detect
Która wersja to JSON?

```
A. {"Name":"Alice"}
B. {"Name":"Alice","Email":"alice@example.com"}
C. {"FirstName":"Alice","Email":"alice@example.com"}
```

**Rozwiązanie:**
```
A = V1 (Name only)
B = V2 (Name + Email)
C = V3 (FirstName + Email)
```

---

### Zadanie 5: Extension Data
Przechowaj dodatkowe pola:

```csharp
[JsonExtensionData]
public ??? ExtraData { get; set; }
```

**Rozwiązanie:**
```csharp
[JsonExtensionData]
public Dictionary<string, JsonElement>? ExtraData { get; set; }
```

---

## 🟡 Intermediate Level

### Zadanie 6: Polymorphic Deserialization
Deserializuj mixed types:

```csharp
var json = """
[
  {"$type":"dog","Name":"Buddy"},
  {"$type":"cat","Name":"Whiskers"}
]
""";

var animals = JsonSerializer.Deserialize<Animal[]>(json);
```

**Rozwiązanie:** Compiler obsługuje `[JsonDerivedType]` automatycznie

---

### Zadanie 7: Custom Type Converter
Zaimplementuj polymorphic converter:

```csharp
public class AnimalConverter : JsonConverter<Animal>
{
    public override Animal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // TODO: Detect type and deserialize
    }
}
```

**Rozwiązanie:**
```csharp
using var doc = JsonDocument.ParseValue(ref reader);
var type = doc.RootElement.GetProperty("$type").GetString();

return type switch
{
    "dog" => JsonSerializer.Deserialize<Dog>(...),
    "cat" => JsonSerializer.Deserialize<Cat>(...),
    _ => throw new JsonException()
};
```

---

### Zadanie 8: Version-Aware Converter
Obsługuj wiele versji:

```csharp
public class PersonConverter : JsonConverter<PersonV3>
{
    public override PersonV3 Read(ref Utf8JsonReader reader, ...)
    {
        // TODO: Handle V1, V2, V3 formats
    }
}
```

**Rozwiązanie:**
```csharp
using var doc = JsonDocument.ParseValue(ref reader);
var root = doc.RootElement;

// Try V3 format first
if (root.TryGetProperty("FirstName", out var firstName))
    return new PersonV3 { FirstName = firstName.GetString() };

// Fall back to V1/V2 format
if (root.TryGetProperty("Name", out var name))
    return new PersonV3 { FirstName = name.GetString() };

throw new JsonException("Invalid format");
```

---

### Zadanie 9: Graceful Degradation
Obsługuj brakujące pola:

```csharp
var v1Json = "{\"Name\":\"Alice\"}";
var person = JsonSerializer.Deserialize<PersonV2>(v1Json);

// What should happen?
// person.Name = "Alice"
// person.Email = ??? 
// person.Phone = ???
```

**Rozwiązanie:**
```
person.Name = "Alice"
person.Email = null (default)
person.Phone = null (default)
```

---

### Zadanie 10: Migration Planning
Zaplanuj migrację V1 → V2 → V3:

```
Phase 1: Support both V1 and V2
Phase 2: ???
Phase 3: ???
Phase 4: ???
```

**Rozwiązanie:**
```
Phase 1: Support both V1 and V2
Phase 2: Introduce V3 support (backward compat)
Phase 3: Notify clients - set deprecation date
Phase 4: Deadline passes, remove V1 support
Phase 5: Optional - remove V2 support later
```

---

## 🔴 Advanced Level

### Zadanie 11: Complex Polymorphism
Zagnieżdżone typy pochodne:

```csharp
Vehicle (base)
├─ Car
│  ├─ SportsCar
│  └─ Sedan
├─ Truck
└─ Motorcycle

// Serialize: List<Vehicle> with all subtypes
```

---

### Zadanie 12: Zero-Downtime Migration
Jak zmienić format bez outage?

```
Strategy:
1. Support V1 AND V2 simultaneously
2. New clients use V2
3. Old clients still use V1
4. Gradually migrate
5. After deadline, remove V1
```

---

## 📊 Wskazówki

- ✅ Plan versioning strategy from start
- ✅ Always support N-1 versions
- ✅ Use type discriminators
- ✅ Make fields nullable for new versions
- ✅ Test migrations carefully
- ✅ Document version timeline
- ❌ Nie rób breaking changes
- ❌ Nie serialize internal implementation
- ❌ Nie forget about old clients

---

## 🎯 Key Takeaways

Advanced Features:

```
Polymorphism: [JsonDerivedType]
Versioning: Custom converters
Compatibility: Nullable fields + [JsonExtensionData]
Migration: Gradual, documented strategy
```

Pamiętaj: **Plan for evolution!**
