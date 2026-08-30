# Ćwiczenia: XML Serialization

## 🟢 Basic Level

### Zadanie 1: Kiedy użyć [XmlAttribute]?
Które pola powinny być atrybutami, które elementami?

```csharp
public class Product
{
    public int Id { get; set; }        // Attribute?
    public string Name { get; set; }   // Element?
    public decimal Price { get; set; } // Element?
}
```

**Rozwiązanie:**
```
[XmlAttribute] - Id (identyfikatory, meta data)
[XmlElement]   - Name, Price (dane)

Reguła: Id, status, type → Attribute
        Data → Element
```

---

### Zadanie 2: Co to [XmlIgnore]?
Kiedy zaaplikować `[XmlIgnore]`?

**Rozwiązanie:**
```csharp
[XmlIgnore]
public string Password { get; set; }  // Sensitive data

[XmlIgnore]
public DateTime LastModified { get; set; }  // Computed

Cel: Nie wrzucaj do XML:
- Sensitive data (passwords, tokens)
- Computed fields (z cached)
- Temporary state
```

---

### Zadanie 3: XmlSerializer Setup
Napisz kod do serializacji:

```csharp
public class Person { ... }

var person = new Person { Name = "Alice", Age = 30 };

// TODO: Serialize to person.xml

var xs = new XmlSerializer(typeof(Person));
using var sw = new StreamWriter("person.xml");
xs.Serialize(sw, person);
sw.Close();
```

**Rozwiązanie:** (kod powyżej jest poprawny)

---

### Zadanie 4: Custom Element Names
Jak zmienić nazwę elementu XML?

```csharp
public class Employee
{
    public string Name { get; set; }  // XML: <Name> or <FullName>?
}
```

**Rozwiązanie:**
```csharp
[XmlElement("FullName")]
public string Name { get; set; }

// XML wynik:
// <Employee>
//   <FullName>Alice</FullName>
// </Employee>
```

---

### Zadanie 5: Deserializacja
Odczytaj XML i odtwórz obiekt:

```csharp
var xs = new XmlSerializer(typeof(Person));
using var sr = new StreamReader("person.xml");
var restored = (Person)xs.Deserialize(sr);
```

**Rozwiązanie:** Kod powyżej jest poprawny. Brak [Serializable], bo XmlSerializer nie wymaga.

---

## 🟡 Intermediate Level

### Zadanie 6: Kolekcje z XmlArray
Narysuj strukturę XML dla:

```csharp
public class Team
{
    public string Name { get; set; }
    
    [XmlArray("Members")]
    [XmlArrayItem("Person")]
    public List<Person> Employees { get; set; }
}
```

**Rozwiązanie:**
```xml
<Team>
  <Name>Engineers</Name>
  <Members>
    <Person>
      <Name>Alice</Name>
      <Age>30</Age>
    </Person>
    <Person>
      <Name>Bob</Name>
      <Age>35</Age>
    </Person>
  </Members>
</Team>
```

---

### Zadanie 7: Round-trip Validation
Weryfikuj, że serialization jest reversible:

```csharp
var original = new Person { Name = "Alice", Age = 30 };

// Serialize → Deserialize
var xs = new XmlSerializer(typeof(Person));

// TODO: Full round-trip with validation
```

**Rozwiązanie:**
```csharp
using var stream = new MemoryStream();
xs.Serialize(stream, original);

stream.Seek(0, SeekOrigin.Begin);
var restored = (Person)xs.Deserialize(stream);

Assert.AreEqual(original.Name, restored.Name);
Assert.AreEqual(original.Age, restored.Age);
```

---

### Zadanie 8: Polymorphism
Jak serializować klasy pochodne?

```csharp
public class Animal { }
public class Dog : Animal { }
public class Cat : Animal { }

public class Zoo
{
    public List<Animal> Animals { get; set; }  // ❌ Jak zadeklarować?
}
```

**Rozwiązanie:**
```csharp
public class Zoo
{
    [XmlElement(typeof(Dog))]
    [XmlElement(typeof(Cat))]
    public List<Animal> Animals { get; set; }
}

// Serialize'r będzie wiedział, jak obsługiwać Dog/Cat
```

---

### Zadanie 9: Performance Estimate
Porównaj wielkości dla 1000 obiektów Person:

```
Binary:  ? bytes
XML:     ? bytes
JSON:    ? bytes
```

**Rozwiązanie:**
```
Prosty Person { Name: 15 chars, Age: 30 }

Binary:  ~30 bytes × 1000 = ~30 KB
XML:     ~100 bytes × 1000 = ~100 KB
JSON:    ~30 bytes × 1000 = ~30 KB

XML: 3-4x większy!
```

---

### Zadanie 10: Pretty-printing
Zserializuj z formatowaniem:

```csharp
var settings = new XmlWriterSettings
{
    Indent = true,
    IndentChars = "  ",
    OmitXmlDeclaration = false,
    Encoding = Encoding.UTF8
};

// TODO: Use with XmlSerializer
```

**Rozwiązanie:**
```csharp
using var writer = XmlWriter.Create("person.xml", settings);
xs.Serialize(writer, person);
writer.Close();
```

---

## 🔴 Advanced Level

### Zadanie 11: Custom XmlSerializer
Zaimplementuj własny serializator dla specjalnych typów:

```csharp
public class CustomDateTimeSerializer : XmlSerializer
{
    // Override dla specjalnego formatowania DateTime
    // Format: yyyy-MM-dd (zamiast długiego format)
}
```

**Rozwiązanie:**
```csharp
public class PersonWithDate
{
    public string Name { get; set; }
    
    [XmlElement]
    public string BirthDate { get; set; }  // Store as string "1990-05-15"
}

// lub użyj IXmlSerializable
public class PersonCustom : IXmlSerializable
{
    public XmlSchema GetSchema() => null;
    
    public void ReadXml(XmlReader reader) { /* custom logic */ }
    
    public void WriteXml(XmlWriter writer) { /* custom logic */ }
}
```

---

### Zadanie 12: Circular Reference Handling
Jak obsługiwać grafy cykliczne w XML?

```csharp
public class Node
{
    public string Name { get; set; }
    public Node Parent { get; set; }  // ❌ Cykl!
}
```

**Rozwiązanie:**
```csharp
public class Node
{
    public string Name { get; set; }
    
    [XmlIgnore]  // ← Nie serializuj backref
    public Node Parent { get; set; }
    
    [XmlElement("ParentId")]  // ← Zamiast tego, użyj ID
    public int ParentId { get; set; }
}

// Reconstructor musi zrobić lookup by ID
```

---

## 📊 Wskazówki

- ✅ Używaj [XmlElement] dla standardowych danych
- ✅ Używaj [XmlAttribute] dla ID, meta
- ✅ Używaj [XmlIgnore] na sensitive fields
- ✅ Zawsze test round-trip
- ✅ Pretty-print dla debugowania
- ❌ Nie ignoruj performance (XML duży)
- ❌ Nie serializuj circular refs
- ❌ Nie zapomnij [XmlArray]/[XmlArrayItem] dla collections

---

## 🎯 Key Takeaways

**XmlSerializer** = System.Xml.Serialization.XmlSerializer

```
Nie wymaga [Serializable] atrybutu!
Używa reflection do odczytania properties.
Podpiera atrybuty: [XmlElement], [XmlAttribute], [XmlIgnore]
```

**Słaba strona:** ~10x większy rozmiar niż JSON/Binary

**Kiedy użyć:** Legacy systems, config files, dokumenty biznesowe

**Nowoczesna alternatywa:** System.Text.Json (lepszy performance)
