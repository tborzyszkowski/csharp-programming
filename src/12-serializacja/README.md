# 12. Serializacja w C# 📦

**Wersja:** 1.0  
**.NET:** 9.0+  
**C#:** 13 (latest)

---

## 📌 Przegląd Modułu

Kompleksowy kurs serializacji danych w C# obejmujący:
- Fundamentalne pojęcia (object graphs, algorytmy drzew rozpinających)
- Historyczne i współczesne podejścia (.NET 1.0 do .NET 9.0)
- Praktyczne implementacje (XML, Binary, JSON, Protocol Buffers)
- Zaawansowane scenariusze (circular references, versioning, security)
- Performance i best practices

---

## 🎯 10 Tematów Nauki

| # | Temat | Focus | Czas |
|---|-------|-------|------|
| 1️⃣ | **Serialization Concepts** | Object graph, tree traversal, why serialize | 50 min |
| 2️⃣ | **History & Evolution** | .NET 1.0 → 9.0, changing approaches | 40 min |
| 3️⃣ | **XML Serialization** | XmlSerializer, attributes, options | 50 min |
| 4️⃣ | **Binary Serialization** | BinaryFormatter (legacy), events, attributes | 45 min |
| 5️⃣ | **JSON Serialization (Modern)** | System.Text.Json, JsonSerializer, options | 50 min |
| 6️⃣ | **Custom Serialization** | ISerializable, custom implementations | 45 min |
| 7️⃣ | **Circular References** | Deep/shallow copy, graph traversal, cycles | 40 min |
| 8️⃣ | **Advanced Patterns** | Events, versioning, compatibility, streaming | 50 min |
| 9️⃣ | **Protocol Buffers & gRPC** | Modern format, performance, schema | 50 min |
| 🔟 | **Performance & Security** | Benchmarking, security concerns, best practices | 50 min |

**Razem: ~470 minut (~8 godzin nauki)**

---

## 🗺️ Ścieżka Nauki

```
Temat 1: Core Concepts
    ↓
Temat 2: Historical Context
    ↓
    ┌───────────────────────────────────┐
    ↓                                   ↓
Temat 3: XML          Temat 4: Binary
    │                     │
    └────────┬────────────┘
             ↓
         Temat 5: JSON
             ↓
         Temat 6: Custom
             ↓
    ┌────────┴─────────┐
    ↓                  ↓
Temat 7: Graphs   Temat 8: Advanced
    │                 │
    └────────┬────────┘
             ↓
         Temat 9: Protocol Buffers
             ↓
         Temat 10: Performance & Security
             ↓
    ✓ MASTER Serialization
```

---

## 📚 Wymagania Wstępne

- ✅ [Klasy (01-klasy)](../01-klasy)
- ✅ [Właściwości (03-wlasciwosci)](../03-wlasciwosci)
- ✅ [Interfejsy (07-interfejsy_abstrakcje)](../07-interfejsy_abstrakcje)
- ✅ [Async/Await (11-async)](../11-async) - dla streaming
- ✅ LINQ, LINQ to XML
- ✅ Reflection basics

---

## 🚀 Szybki Start

### 1️⃣ Struktura

```bash
cd src/12-serializacja
```

### 2️⃣ Wybierz Temat

```bash
# Temat 1: Concepts
cd _01_serialization_concepts
cat README.md
dotnet run --project code/Program.csproj
```

### 3️⃣ Czytaj, Uruchamiaj, Praktykuj

- 📖 Przeczytaj README + diagramy
- ▶️ Uruchom `dotnet run`
- 📝 Analizuj kod i output
- 💪 Rozwiąż ćwiczenia

---

## 🔑 Kluczowe Koncepty

### Co To Jest Serializacja?

Proces konwersji **obiektu w pamięci** na **sekwencję bajtów** (stream) do:
- Przechowywania w pliku
- Przesłania przez sieć
- Współdzielenia między procesami

```csharp
// Obiekt w pamięci
var person = new Person { Name = "Alice", Age = 30 };

// Serializacja
byte[] data = Serialize(person);

// Przechowywanie/przesłanie/etc
File.WriteAllBytes("person.dat", data);

// Deserializacja
var restored = Deserialize<Person>(data);
```

### Object Graph

**Graf obiektów** - struktura pamięci z wszystkimi referencjami:

```
Person
├── string Name
├── int Age
└── Address (reference)
    ├── string Street
    ├── string City
    └── Country (reference)
        ├── string Code
        └── string Name
```

**Algorytm:** Depth-First Search (DFS) lub Breadth-First Search (BFS)

---

## 📊 Podsumowanie Formatów

| Format | Use Case | Performance | Human Readable | .NET Support |
|--------|----------|-------------|-----------------|----------------|
| **XML** | Config, SOAP, legacy | Slow | ✅ Yes | ✅ Built-in |
| **Binary** | Speed, compact | ✅ Fast | ❌ No | ⚠️ Legacy |
| **JSON** | Web APIs, modern | Good | ✅ Yes | ✅ Modern |
| **Protocol Buffers** | gRPC, performance | ✅ Fastest | ❌ No | ✅ Plugin |
| **MessagePack** | Binary, compact | ✅ Fast | ❌ No | ✅ Package |
| **YAML** | Config, human | Average | ✅ Yes | ⚠️ Package |

---

## 💡 Dlaczego Serializacja Ważna?

✅ **Persistence** - Zapisanie stanu aplikacji  
✅ **Communication** - Przesyłanie danych  
✅ **Caching** - Szybki dostęp  
✅ **Integration** - Współpraca systemów  
✅ **Backup** - Odtworzenie stanu  

---

## ⚠️ Ważne Uwagi

> **Security:** BinaryFormatter jest **deprecated** ze względów bezpieczeństwa (RCE). Używaj JSON lub Protocol Buffers.

> **Versioning:** Zawsze myśl o kompatybilności gdy zmienisz strukturę klasy.

> **Performance:** Różne formaty mają różne charakterystyki. Benchmarkuj!

> **Circular References:** Mogą powodować infinite loops. Wymaga specjalnej obsługi.

---

## 📋 Zawartość Każdego Tematu

Każdy temat zawiera:

```
_XX_topic_name/
├── README.md              ← Teoria + diagramy
├── code/
│   ├── Program.cs         ← 3-5 praktycznych przykładów
│   └── Program.csproj     ← .NET 9.0 konfiguracja
├── diagrams/
│   └── diagrams.md        ← Mermaid flowcharts
└── tasks/
    └── EXERCISES.md       ← 3-level exercises
```

---

## 🎓 Dla Których To Jest

- 👨‍💻 **Dla Studentów:** Od konceptów do praktyki
- 🏫 **Dla Wykładowców:** Gotowe slajdy i demo
- 🔧 **Dla Praktyk:** Real-world patterns
- 📚 **Dla Wszystkich:** Best practices

---

## 🔍 Co Nauczysz Się

✅ Jak działają algorytmy serializacji  
✅ Kiedy użyć jaki format  
✅ Implementacja własnych serializerów  
✅ Obsługa złożonych scenariuszy  
✅ Security best practices  
✅ Performance optimization  

---

## 📚 Referencje

- [Microsoft: Serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)
- [XmlSerializer](https://learn.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer)
- [System.Text.Json](https://learn.microsoft.com/en-us/dotnet/api/system.text.json)
- [Protocol Buffers](https://developers.google.com/protocol-buffers)
- [MessagePack](https://msgpack.org/)
- [BinaryFormatter (Deprecated)](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.serialization.formatters.binary.binaryformatter)

---

## 🚀 Przyspieszony Kurs (30 min)

```bash
# Szybki przegląd wszystkich formatów
cd _05_json_serialization_modern
dotnet run

# Porównanie wydajności
cd _10_performance_security
dotnet run
```

---

**Autor:** Educational Materials Generator  
**Wersja:** 1.0  
**Ostatnia aktualizacja:** 2024  
**Licencja:** Educational Use

---

🎉 Gotów? Zacznij od [Tematu 1: Serialization Concepts](_01_serialization_concepts/README.md)
