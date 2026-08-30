# Diagramy: JSON Serialization

## Diagram 1: JSON Serialization Flow

```mermaid
graph LR
    A["Object in Memory<br/>Person {<br/>Name: Alice<br/>Age: 30<br/>}"]
    
    A -->|JsonSerializer<br/>.Serialize| B["JSON String<br/>{<br/>  'Name': 'Alice'<br/>  'Age': 30<br/>}"]
    
    B -->|Transmit/Store| C["Network / File<br/>person.json"]
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#ffccbc
```

## Diagram 2: JsonSerializerOptions

```mermaid
graph TD
    A["JsonSerializerOptions"]
    
    A --> B["PropertyNamingPolicy"]
    A --> C["WriteIndented"]
    A --> D["PropertyNameCaseInsensitive"]
    A --> E["DefaultIgnoreCondition"]
    A --> F["Converters"]
    
    B --> B1["CamelCase"]
    B --> B2["KebabCase"]
    B --> B3["SnakeCase"]
    
    style B1 fill:#fff9c4
    style B2 fill:#fff9c4
    style B3 fill:#fff9c4
```

## Diagram 3: Attributes Control

```mermaid
graph LR
    A["Attributes"]
    
    A --> B["[JsonPropertyName]<br/>Custom JSON name"]
    A --> C["[JsonIgnore]<br/>Exclude from JSON"]
    A --> D["[JsonRequired]<br/>Must be present"]
    A --> E["[JsonInclude]<br/>Include private field"]
    
    style B fill:#fff9c4
    style C fill:#ffccbc
    style D fill:#ffe0b2
    style E fill:#c8e6c9
```

## Diagram 4: Nested Object Serialization

```mermaid
graph TD
    A["Team<br/>Name: Engineers<br/>Members: [...]"]
    
    B["Person<br/>Alice, 30"]
    C["Person<br/>Bob, 35"]
    
    A --> B
    A --> C
    
    D["JSON Output<br/>{<br/>  'Name': 'Engineers'<br/>  'Members': [<br/>    {'Name': 'Alice', 'Age': 30},<br/>    {'Name': 'Bob', 'Age': 35}<br/>  ]<br/>}"]
    
    A -.->|Serialize| D
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#c8e6c9
    style D fill:#ffe0b2
```

## Diagram 5: Size Comparison

```mermaid
pie title JSON Size vs Other Formats
    "Binary" : 14
    "JSON" : 26
    "JSON Pretty" : 40
    "XML" : 96
```

## Diagram 6: Source Generators (Zero Reflection)

```mermaid
graph LR
    A["Source Generator<br/>Context"]
    B["Compile-time<br/>Code Generation"]
    C["Zero Reflection<br/>at Runtime"]
    D["AOT Compatible<br/>Small Binary"]
    
    A --> B
    B --> C
    C --> D
    
    style A fill:#fff9c4
    style B fill:#ffe0b2
    style C fill:#c8e6c9
    style D fill:#a5d6a7
```

## Diagram 7: Performance: JSON Serialization (10,000 objects)

```mermaid
graph LR
    A["10,000 objects"]
    
    A -->|System.Text.Json| B1["35ms"]
    A -->|Source Generators| B2["7ms"]
    A -->|Newtonsoft.Json| B3["65ms"]
    A -->|Custom Binary| B4["45ms"]
    
    style B2 fill:#a5d6a7
    style B1 fill:#c8e6c9
    style B4 fill:#fff9c4
    style B3 fill:#ffccbc
```

## Diagram 8: REST API JSON Flow

```mermaid
sequenceDiagram
    participant Client as Client App
    participant API as REST API
    participant Serializer as JsonSerializer
    
    Client->>Serializer: Serialize(person)
    Serializer->>API: POST JSON
    API->>Serializer: Deserialize(json)
    Serializer->>API: person object
    API-->>Serializer: Response JSON
    Serializer-->>Client: person object
```
