# Temat 2: Serialization History - Ewolucja w .NET

## 📜 Historia Serializacji w .NET

Serializacja w .NET ewoluowała przez 25 lat, od prostych podejść aż do nowoczesnych wydajnych rozwiązań.

---

## 🕐 Faza 1: .NET Framework 1.0-2.0 (2002-2005)

### BinaryFormatter - Pionier

```csharp
// .NET 1.0 - Rok 2002
[Serializable]
public class Person
{
    public string Name;
    public int Age;
}

// Serializacja
var bf = new BinaryFormatter();
var stream = File.Create("person.bin");
bf.Serialize(stream, new Person { Name = "Alice", Age = 30 });
stream.Close();

// Deserializacja
var restored = (Person)bf.Deserialize(File.OpenRead("person.bin"));
```

**Charakterystyka:**
- ✅ Proste - `[Serializable]` + BinaryFormatter
- ✅ Kompaktowe - mały rozmiar danych
- ✅ Obsługa wszystkich typów
- ❌ **DEPRECATED w .NET 5+** - security risks!
- ❌ Tylko .NET ecosystem
- ❌ Brak wersjonowania

### XmlSerializer - Alternatywa

```csharp
// .NET 1.1 (2003)
public class Person  // Bez [Serializable]!
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var xs = new XmlSerializer(typeof(Person));
xs.Serialize(File.Create("person.xml"), 
    new Person { Name = "Alice", Age = 30 });

// Wynik:
// <?xml version="1.0"?>
// <Person>
//   <Name>Alice</Name>
//   <Age>30</Age>
// </Person>
```

**Charakterystyka:**
- ✅ Human readable
- ✅ Standard XML format
- ✅ Interoperability
- ❌ Duży rozmiar
- ❌ Powolny
- ⚠️ Reflection overhead

---

## 🌐 Faza 2: .NET 3.0-3.5 (2006-2007)

### Windows Communication Foundation (WCF)

Ścieżka serializacji sformalizowana:

```csharp
// Kontrakty serializacji
[DataContract]
public class Person
{
    [DataMember]
    public string Name { get; set; }
    
    [DataMember]
    public int Age { get; set; }
    
    [IgnoreDataMember]  // To nie będzie serializowane
    public string Password { get; set; }
}

// WCF auto-serializuje do XML/JSON
```

**Innowacje:**
- ✅ Atrybuty - `[DataContract]`, `[DataMember]`
- ✅ Kontrol nad polami
- ✅ WCF automatycznie wybiera format (XML/JSON)
- ❌ Konfiguracja złożona

---

## 📱 Faza 3: .NET 4.0-4.5 (2010-2012)

### JSON jako Standard Web

```csharp
// .NET 4.0 - Rok 2010
// System.Web.Script.Serialization
public class JavaScriptSerializer
{
    public string Serialize(object obj) { }
    public T Deserialize<T>(string json) { }
}

var jss = new JavaScriptSerializer();
string json = jss.Serialize(new Person { Name = "Alice", Age = 30 });
// {"Name":"Alice","Age":30}
```

**Kontext:**
- REST API staje się standardem
- JSON zastępuje XML na web
- AJAX dominuje w frontendu

**Problem:** Każda biblioteka własną implementację:
- NewtonSoft.Json (Linq2Json)
- ServiceStack.Text
- Jil
- Protobuf-net

---

## 🎯 Faza 4: .NET Core 1.0+ (2016+)

### Unifikacja: System.Text.Json

```csharp
// .NET Core 3.0 (2019) - Microsoft oficjalny JSON
using System.Text.Json;

public class Person
{
    [JsonPropertyName("name")]  // Custom name
    public string Name { get; set; }
    
    [JsonIgnore]  // Nie serializuj to
    public string Password { get; set; }
}

// Serializacja
var json = JsonSerializer.Serialize(person);

// Deserializacja
var restored = JsonSerializer.Deserialize<Person>(json);
```

**Rewolucja:**
- ✅ Built-in, nie potrzeba NuGet
- ✅ Wydajny (source generators)
- ✅ Modern C# patterns
- ✅ Async support
- ✅ Streaming support

---

## 🚀 Faza 5: .NET 5+ (2020+)

### Source Generators & Trimming

```csharp
// .NET 5 (2020) - Compile-time serialization
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase)]
[JsonSerializable(typeof(Person))]
internal partial class PersonJsonContext 
    : JsonSerializerContext { }

// Zero reflection, pure codegen
var json = JsonSerializer.Serialize(person, 
    PersonJsonContext.Default.Person);
```

**Game Changer:**
- ✅ No reflection at runtime
- ✅ AOT (Ahead-of-Time) friendly
- ✅ Source trimming possible
- ✅ Native compilation ready

### Protokoł Buffers Integration

```csharp
// .NET 5+ - Protocol Buffers standardowy
// protobuf-net package
[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string Name { get; set; }
    
    [ProtoMember(2)]
    public int Age { get; set; }
}

var model = RuntimeTypeModel.Default;
using var ms = new MemoryStream();
model.Serialize(ms, person);
```

---

## 📊 Timeline Porównania

```
2002: BinaryFormatter (Binary)
      └─→ Unsafe, deprecated

2003: XmlSerializer (XML)
      └─→ Human readable, slow

2006: WCF [DataContract] (Attributes)
      └─→ Standardized

2010: JavaScriptSerializer (JSON)
      └─→ Web ready

2015: NewtonSoft Json.NET dominates
      └─→ Industry standard

2019: System.Text.Json (.NET Core 3.0)
      └─→ Official, fast, trimming-ready

2020: Source Generators (.NET 5+)
      └─→ No reflection, AOT friendly

2024: Modern landscape
      └─→ System.Text.Json + Protocol Buffers
```

---

## 🔄 Migration Path

### Old Code (.NET Framework)

```csharp
[Serializable]
public class Person
{
    public string Name;
}

var bf = new BinaryFormatter();
bf.Serialize(stream, person);  // ❌ DEPRECATED
```

### Modern Code (.NET 6+)

```csharp
public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}

var json = JsonSerializer.Serialize(person);  // ✅ RECOMMENDED
```

---

## 🎓 Key Lessons

### Dlaczego Serializacja Ewoluowała?

1. **Security** - BinaryFormatter ma RCE vulnerabilities
2. **Performance** - Oczekiwania wzrosły (biliony requestów)
3. **Interoperability** - Różne języki/platformy
4. **Cloud** - Distributed systems, microservices
5. **AOT/Trimming** - Smaller apps, lower memory

### Krzywa Adopcji

```
Complexity
    │
    │    BinaryFormatter (Simple, Unsafe)
    │         │
    │         ├─→ XmlSerializer (Better, Verbose)
    │         │      │
    │         │      └─→ WCF [DataContract] (Structured)
    │         │             │
    │         │             └─→ Json.NET (Flexible)
    │         │                    │
    │         │                    └─→ System.Text.Json (Fast)
    │         │                           │
    │         │                           └─→ Source Generators (Zero-cost)
    │
    └────────────────────────────────────────────→ Time (2002-2024)

Adoption %
    │
  100%  ┌─ NewtonSoft JSON (Industry fav)
        │  ┌─ System.Text.Json (Official)
   80%  │  │
        │  │
   60%  │  ├─ Protocol Buffers (Specific use)
        │  │
   40%  │  ├─ XmlSerializer (Legacy)
        │  │
   20%  │  └─ BinaryFormatter (Deprecated)
        │
    0%  └────────────────────
        2020   2022   2024   2026
```

---

## 📈 Format Adoption (Current 2024-2026)

### Web APIs
- **92%** - JSON (REST, GraphQL)
- **5%** - Protocol Buffers (gRPC)
- **3%** - Other

### Microservices
- **65%** - JSON
- **30%** - Protocol Buffers
- **5%** - MessagePack

### Data Storage
- **70%** - JSON
- **20%** - Protocol Buffers
- **10%** - Binary formats

---

## 🛠️ Rekomendacje Dzisiaj (2024+)

### ✅ USE
- **System.Text.Json** - Default choice
- **Protocol Buffers** - High performance/space
- **NewtonSoft.Json** - Complex scenarios

### ⚠️ AVOID
- **BinaryFormatter** - Security risk!
- **XmlSerializer** - Performance concern
- **DataContractSerializer** - Legacy

### 🚀 FUTURE
- **Source Generators** - Next wave
- **Incremental Compilation** - Faster builds
- **Native AOT** - Cloud-native apps

---

## 📚 Referencje

- [Microsoft: BinaryFormatter is dangerous](https://learn.microsoft.com/en-us/dotnet/standard/serialization/binaryformatter-security-guide)
- [System.Text.Json vs Newtonsoft](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft)
- [Source Generators Tutorial](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation)
- [.NET History](https://en.wikipedia.org/wiki/.NET_(software_framework))

---

## ✅ Summary

Serializacja w .NET przeszła 5 faz:

1. **2002** - BinaryFormatter (simple, unsafe)
2. **2003** - XmlSerializer (readable, slow)
3. **2006** - WCF [DataContract] (structured)
4. **2019** - System.Text.Json (fast, modern)
5. **2020+** - Source Generators (zero-cost)

**Dzisiaj:** Przeważnie JSON dla API, Protocol Buffers dla performance.

**Przyszłość:** AOT, trimming, native compilation.

---

## 🎯 Następny Temat

Temat 3: XML Serialization - Implementacja, atrybuty, customization
