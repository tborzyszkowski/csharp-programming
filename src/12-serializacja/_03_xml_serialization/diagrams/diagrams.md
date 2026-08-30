# Diagramy: XML Serialization

## Diagram 1: XmlSerializer Workflow

```mermaid
graph LR
    A["Object in Memory<br/>Person {<br/>  Name: Alice<br/>  Age: 30<br/>}"]
    
    A -->|XmlSerializer<br/>.Serialize| B["XML Document<br/>&lt;Person&gt;<br/>  &lt;Name&gt;...&lt;/Name&gt;<br/>  &lt;Age&gt;...&lt;/Age&gt;<br/>&lt;/Person&gt;"]
    
    B -->|Write to File| C["XML File<br/>person.xml"]
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#ffccbc
```

## Diagram 2: XmlAttribute vs XmlElement

```mermaid
graph LR
    A["Attribute Style<br/>&lt;Person id='1'&gt;"]
    B["Element Style<br/>&lt;Person&gt;<br/>  &lt;Id&gt;1&lt;/Id&gt;"]
    
    style A fill:#fff9c4
    style B fill:#c8e6c9
```

## Diagram 3: XmlArray for Collections

```mermaid
graph TD
    A["Team"]
    B["Members (XmlArray)"]
    C["Member (XmlArrayItem)"]
    D["Alice"]
    E["Bob"]
    
    A --> B
    B --> C
    C --> D
    C --> E
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#fff9c4
    style D fill:#ffe0b2
    style E fill:#ffe0b2
```

## Diagram 4: XmlIgnore for Security

```mermaid
graph LR
    A["Object<br/>Name: Alice<br/>Password: secret"]
    
    A -->|XmlIgnore| B["XML Output<br/>&lt;Person&gt;<br/>  &lt;Name&gt;Alice&lt;/Name&gt;<br/>&lt;/Person&gt;"]
    
    style A fill:#ffccbc
    style B fill:#c8e6c9
```

## Diagram 5: Size Comparison (Round-trip)

```mermaid
graph LR
    A["Object<br/>Person {<br/>Name: Alice<br/>Age: 30<br/>}"]
    
    A -->|Binary| B1["26 bytes"]
    A -->|XML| B2["96 bytes<br/>(3.7x larger)"]
    A -->|JSON| B3["26 bytes"]
    
    style A fill:#bbdefb
    style B1 fill:#c8e6c9
    style B2 fill:#ffccbc
    style B3 fill:#c8e6c9
```

## Diagram 6: Round-trip Serialization

```mermaid
sequenceDiagram
    participant App as Application
    participant XS as XmlSerializer
    participant File as person.xml
    
    App->>XS: Serialize(person)
    XS->>File: Write XML
    
    File->>XS: Read XML
    XS->>App: Deserialize()
    
    Note over App: Restored object == Original
```

## Diagram 7: Attribute Types

```mermaid
graph TD
    A["XmlSerializer Attributes"]
    
    A --> B["[XmlAttribute]<br/>Becomes XML attribute"]
    A --> C["[XmlElement]<br/>Becomes XML element"]
    A --> D["[XmlArray]<br/>Wrapper for collection"]
    A --> E["[XmlArrayItem]<br/>Items in collection"]
    A --> F["[XmlIgnore]<br/>Not serialized"]
    
    style B fill:#fff9c4
    style C fill:#c8e6c9
    style D fill:#ffe0b2
    style E fill:#ffe0b2
    style F fill:#ffccbc
```

## Diagram 8: Performance - XML vs JSON vs Binary

```mermaid
graph LR
    A["1000 Large Objects"]
    
    A -->|Serialize| B["BinaryFormatter: 50ms"]
    A -->|Serialize| C["XmlSerializer: 500ms"]
    A -->|Serialize| D["System.Text.Json: 50ms"]
    
    style B fill:#c8e6c9
    style C fill:#ffccbc
    style D fill:#c8e6c9
```
