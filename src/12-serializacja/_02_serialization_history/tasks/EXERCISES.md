# Ćwiczenia: Serialization History

## 🟢 Basic Level

### Zadanie 1: Timeline Chronologiczny
Ustaw w porządku chronologicznym:

- [ ] System.Text.Json
- [ ] BinaryFormatter
- [ ] XmlSerializer
- [ ] WCF DataContract
- [ ] Source Generators

**Rozwiązanie:**
```
1. BinaryFormatter (2002)
2. XmlSerializer (2003)
3. WCF DataContract (2006)
4. System.Text.Json (2019)
5. Source Generators (2020)
```

---

### Zadanie 2: Dlaczego Deprecated?
BinaryFormatter jest marked as `[Obsolete]` i deprecated w .NET 5+.

Wymień 3 powody:

**Rozwiązanie:**
```
1. Security: Remote Code Execution (RCE) vulnerabilities
2. Interoperability: Only works with .NET ecosystem
3. Versioning: Difficult to maintain across versions
4. Performance: Slower than modern alternatives
5. Cloud: Not suitable for cloud-native apps (AOT)
```

---

### Zadanie 3: Format Improvement
Które stwierdzenie jest PRAWDĄ?

A. BinaryFormatter jest szybszy niż System.Text.Json  
B. XmlSerializer jest bardziej bezpieczny niż BinaryFormatter  
C. System.Text.Json wymaga atrybutów do serializacji  
D. Source Generators używają reflection  

**Rozwiązanie:** B (XmlSerializer nie ma RCE vulnerabilities jak BinaryFormatter)

---

### Zadanie 4: Wybór Technologii
Dopasuj technologię do scenariusza:

```
1. Legacy .NET Framework app        → A. System.Text.Json
2. New Web API (REST)               → B. Protocol Buffers
3. High-performance microservice    → C. XmlSerializer
4. Interoperability between systems → D. Keep BinaryFormatter
```

**Rozwiązanie:**
```
1 → C (lub keep as-is dla legacy)
2 → A (JSON standard dla REST)
3 → B (Protocol Buffers najszybsze)
4 → A (JSON universal)
```

---

### Zadanie 5: Atrybuty vs Conventions
Co to jest [JsonPropertyName]?

**Rozwiązanie:**
```
Atrybut System.Text.Json:
- Określa JSON property name
- Allowuje custom naming (np. camelCase)
- Przykład: [JsonPropertyName("firstName")]
```

---

## 🟡 Intermediate Level

### Zadanie 6: Migration Checklist
Jeśli migrowałeś aplikację z BinaryFormatter do System.Text.Json, co sprawdzić?

**Rozwiązanie:**
```
[ ] 1. Zamień [Serializable] → [JsonSerializable] (na tipos)
[ ] 2. Zamiń BinaryFormatter → JsonSerializer
[ ] 3. Test round-trip serialization
[ ] 4. Verify field names match JSON properties
[ ] 5. Handle circular references
[ ] 6. Test performance
[ ] 7. Update exception handling
```

---

### Zadanie 7: Performance Estimation
Porównaj czasy serializacji dla 1000 dużych obiektów:

```csharp
var people = Enumerable.Range(0, 1000)
    .Select(i => new Person { Name = $"Person{i}", Age = 20 + i })
    .ToList();

// Estimate time for each:
// 1. BinaryFormatter.Serialize() → ?
// 2. XmlSerializer.Serialize() → ?
// 3. System.Text.Json.Serialize() → ?
// 4. Source Generated JSON → ?
```

**Rozwiązanie (rough estimates):**
```
1. BinaryFormatter: ~150ms (deprecated)
2. XmlSerializer: ~500ms (slowest - reflection + XML)
3. System.Text.Json: ~50ms (modern)
4. Source Gen: ~10ms (compile-time codegen)
```

---

### Zadanie 8: Security Risk Analysis
Jaki jest RCE risk w BinaryFormatter?

**Rozwiązanie:**
```
BinaryFormatter.Deserialize() problem:
- Reads type info from stream
- Automatically instantiates objects
- Invokes constructors/methods
- Attacker can embed malicious type
- Result: Remote Code Execution

System.Text.Json fix:
- Requires explicit type specification
- No auto-invocation
- Safe by default
```

---

### Zadanie 9: Wersjonowanie
Jak System.Text.Json.JsonSerializerOptions obsługuje versionowanie?

**Rozwiązanie:**
```csharp
var options = new JsonSerializerOptions
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true
};

// Pozwala na:
// - Nowe pola (ignored by old clients)
// - Zmienę nazw (case insensitive)
// - Backward compatible (ignore nulls)
```

---

### Zadanie 10: Interoperability
Której technologii użyć do API między:
- C# (serwer)
- Python (klient)
- JavaScript (frontend)

**Rozwiązanie:**
```
JSON (System.Text.Json)

Powody:
✅ Uniwersalny format
✅ Obsługiwany wszędzie
✅ Human readable
✅ Standardowy dla REST API
```

---

## 🔴 Advanced Level

### Zadanie 11: Source Generators Implementation
Zaproponuj, jak działają Source Generators dla JSON:

```csharp
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Person))]
internal partial class PersonJsonContext 
    : JsonSerializerContext { }
```

Co kompilator generuje?

**Rozwiązanie:**
```csharp
// Kompilator auto-generuje:
public static class PersonJsonContextSerializer
{
    public static byte[] Serialize(Person person)
    {
        // Brak reflection! Czysty kod dla typów znanych w compile-time
        var buffer = new byte[1024];
        Write(buffer, person.Name);      // Direct field access
        Write(buffer, person.Age);       // No PropertyInfo.GetValue()
        return buffer;
    }
}

// Korzyści:
// - Zero reflection overhead
// - AOT compatible
// - Trimming safe
// - Small bundle size
```

---

### Zadanie 12: Format Evolution Analysis
Dla poniższej struktury, jak by wyglądała w każdej epoce:

```csharp
public class Order
{
    public int Id { get; set; }
    public List<Item> Items { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

**Rozwiązanie:**

**2002 (BinaryFormatter):**
```
[Serializable]
public class Order { ... }

BinaryFormatter.Serialize(stream, order);
// Wynik: Binary blob (kompaktowy, unsafe)
```

**2003 (XmlSerializer):**
```
public class Order { ... }  // No attr needed

xs.Serialize(stream, order);
// Wynik: <?xml><Order>...<Items>...</Items>...</Order>
// (Verbose, readable, slow)
```

**2019 (System.Text.Json):**
```
public class Order { ... }  // No attr needed

JsonSerializer.Serialize(order);
// Wynik: {"id":1,"items":[...],"createdAt":"2024-..."}
// (Fast, modern, compact)
```

**2024 (Source Generators):**
```
[JsonSerializable(typeof(Order))]
partial class OrderJsonContext : JsonSerializerContext { }

JsonSerializer.Serialize(order, OrderJsonContext.Default.Order);
// Wynik: Same JSON, but zero reflection overhead!
```

---

## 📊 Wskazówki

- ✅ Zapamiętaj timeline: BinaryFormatter → Xml → WCF → JSON → Generators
- ✅ Zawsze używaj System.Text.Json dla nowych projektów
- ✅ Migruj z BinaryFormatter natychmiast (security!)
- ✅ Zrozum RCE risk - ważne w bezpieczeństwie
- ❌ Nie używaj BinaryFormatter
- ❌ Nie ignoruj wersjonowania

---

## 🎯 Key Takeaways

```
2002-2022: Era BinaryFormatter (expired)
           ↓
2003-today: Era XmlSerializer (legacy)
           ↓
2015-2020: Era Newtonsoft JSON (industry standard)
           ↓
2019+:     Era System.Text.Json (official modern)
           ↓
2020+:     Era Source Generators (zero-cost)
```

Pamiętaj: **Zawsze System.Text.Json dla nowego kodu!**
