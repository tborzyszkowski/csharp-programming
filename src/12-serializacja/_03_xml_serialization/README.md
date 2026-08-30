# Temat 3: XML Serialization - XmlSerializer

## 📝 Czym jest XML Serialization?

**XML Serialization** = konwersja obiektu na dokument XML (humanReadable, structured).

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person = new Person { Name = "Alice", Age = 30 };

// Serializacja do XML
var serializer = new XmlSerializer(typeof(Person));
var writer = new StreamWriter("person.xml");
serializer.Serialize(writer, person);
writer.Close();

// Wynik:
// <?xml version="1.0"?>
// <Person>
//   <Name>Alice</Name>
//   <Age>30</Age>
// </Person>
```

---

## ✅ Zalety XmlSerializer

1. **Human Readable** - Można edytować notepadem
2. **Standard** - XML universal format
3. **No [Serializable]** - Bardziej elastyczne niż BinaryFormatter
4. **Attributes Control** - [XmlElement], [XmlAttribute], [XmlIgnore]
5. **Versioning** - Łatwo dodać nowe pola

---

## ❌ Wady XmlSerializer

1. **Duży rozmiar** - Redundantne tagi
2. **Powolny** - Reflection overhead
3. **Performance** - ~10x wolniej niż JSON
4. **XML overhead** - `<Name>` + `</Name>` = 17 bajtów vs "name": 6 bajtów

---

## 🎯 Praktyczne Przykłady

### Przykład 1: Atrybuty vs Elementy

```csharp
public class Person
{
    [XmlAttribute("id")]  // Atrybut XML
    public int Id { get; set; }
    
    [XmlElement]          // Element (domyślny)
    public string Name { get; set; }
    
    [XmlIgnore]           // Nie serializuj
    public string Password { get; set; }
}

// Wynik:
// <Person id="1">
//   <Name>Alice</Name>
//   <!-- Password nie zawiera się -->
// </Person>
```

### Przykład 2: Arraye i Kolekcje

```csharp
public class Team
{
    [XmlArray("Members")]
    [XmlArrayItem("Member")]
    public List<Person> Employees { get; set; }
}

// Wynik:
// <Team>
//   <Members>
//     <Member>
//       <Name>Alice</Name>
//     </Member>
//     <Member>
//       <Name>Bob</Name>
//     </Member>
//   </Members>
// </Team>
```

### Przykład 3: Custom Type Names

```csharp
[XmlRoot("Employee")]   // Zmień root name
[XmlType("Worker")]     // Zmień type name
public class Person
{
    [XmlElement("FirstName")]  // Custom property name
    public string Name { get; set; }
}

// Wynik:
// <Employee>
//   <FirstName>Alice</FirstName>
// </Employee>
```

---

## 🔧 XmlSerializerOptions

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

var person = new Person { Name = "Alice", Age = 30 };
var settings = new XmlWriterSettings
{
    Indent = true,                  // Pretty print
    IndentChars = "  ",             // 2 spaces
    OmitXmlDeclaration = false,     // <?xml...?> header
    Encoding = Encoding.UTF8
};

using var writer = XmlWriter.Create("person.xml", settings);
new XmlSerializer(typeof(Person)).Serialize(writer, person);
```

---

## 🔐 Ograniczenia

### Circular References
```csharp
public class Node
{
    public string Name { get; set; }
    public Node? Parent { get; set; }  // ❌ XmlSerializer nie obsługuje
}

// Error: Type node cannot be serialized
```

**Rozwiązanie:** Użyć `[XmlIgnore]` na backref lub użyć ID zamiast bezpośredniej referencji.

### Polymorphism

```csharp
public class Animal { }
public class Dog : Animal { }

public class Zoo
{
    [XmlElement(typeof(Dog))]  // Jawnie zadeklaruj typy pochodne
    public List<Animal> Animals { get; set; }
}
```

---

## 📊 Porównanie Format XML

### Malutki obiekt
```csharp
var person = new Person { Name = "Alice", Age = 30 };
```

**XML:**
```xml
<?xml version="1.0"?>
<Person>
  <Name>Alice</Name>
  <Age>30</Age>
</Person>
```
**Rozmiar:** 96 bajtów

**JSON:**
```json
{"Name":"Alice","Age":30}
```
**Rozmiar:** 26 bajtów

**Różnica:** XML = **3.7x większy** dla małych obiektów!

---

## 📱 Kiedy Użyć XML?

### ✅ Dobrze dla:
- Konfiguracyjne pliki (app.config)
- Dokumenty biznesowe (invoices, reports)
- SOAP Web Services
- Starsze systemy
- Pliki wymagające edycji ręcznej

### ❌ Słabo dla:
- REST API (użyj JSON)
- Mobile apps (za dużo danych)
- Real-time communication (za powolny)
- High-performance systems (zbyt dużo overhead)

---

## 🔄 Round-trip Serialization

```csharp
// Serialize
var person = new Person { Name = "Alice", Age = 30 };
var serializer = new XmlSerializer(typeof(Person));
var stream = new MemoryStream();
serializer.Serialize(stream, person);

// Deserialize
stream.Seek(0, SeekOrigin.Begin);
var restored = (Person)serializer.Deserialize(stream);

// Verify
Assert.AreEqual(person.Name, restored.Name);
Assert.AreEqual(person.Age, restored.Age);
```

---

## ⚠️ Performance Impact

```
Serialization time for 1000 objects:

BinaryFormatter: 50ms
XmlSerializer:   500ms (10x slower!)
System.Text.Json: 50ms
Source Generated: 10ms
```

**Podsumowanie:** XML jest ~10x wolniej niż binarne lub JSON!

---

## 📚 Best Practices

✅ Do:
- Używaj `[XmlIgnore]` na sensitive fields
- Dodaj `[XmlRoot]` dla jasności
- Pretty-print dla debugowania
- Test round-trip serialization

❌ Nie rób:
- Nie używaj XML dla API (JSON jest standard)
- Nie ignoruj performance (XML duży)
- Nie serializuj circular refs bez obsługi
- Nie serializuj bez atrybutów kontrolnych

---

## 📚 Referencje

- [Microsoft: XmlSerializer Guide](https://learn.microsoft.com/en-us/dotnet/api/system.xml.serialization.xmlserializer)
- [XmlElement vs XmlAttribute](https://learn.microsoft.com/en-us/dotnet/standard/serialization/controlling-xml-serialization-using-attributes)

---

## ✅ Summary

**XML Serialization** to metoda serializacji do formatu XML za pomocą `XmlSerializer`.

**Kluczowe cechy:**
- ✅ Human readable
- ✅ Standardowy format
- ✅ Atrybuty `[XmlElement]`, `[XmlAttribute]`
- ❌ Duży rozmiar
- ❌ Powolny (~10x vs JSON)
- ⚠️ Brak wsparcia circular refs

**Czasem używany:** Konfiguracyjne pliki, starsze systemy, SOAP, dokumenty biznesowe

**Nowoczesny standard:** System.Text.Json (szybszy, mniejszy)

---

## 🎯 Następny Temat

Temat 4: Binary Serialization - BinaryFormatter (legacy), Custom Binary, Efficiency
