# Diagramy: Custom Serialization

## Diagram 1: JsonConverter Workflow

```mermaid
graph LR
    A["Person<br/>BirthDate: DateTime"]
    
    A -->|Write()| B["JSON String<br/>date: '1990-05-15'"]
    
    B -->|Read()| C["Parsed DateTime<br/>1990, 5, 15"]
    
    C -->|Validate| D["Valid/Invalid<br/>Exception if invalid"]
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#fff9c4
    style D fill:#ffccbc
```

## Diagram 2: IXmlSerializable Interface

```mermaid
graph TD
    A["Person Object"]
    
    A -->|WriteXml()| B["XML Output<br/>&lt;Person name='Alice' age='30' /&gt;"]
    
    B -->|ReadXml()| C["Restored Person<br/>Name: Alice, Age: 30"]
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#bbdefb
```

## Diagram 3: Surrogate Pattern

```mermaid
graph LR
    A["LegacyPerson<br/>(can't modify)"]
    
    A -->|FromPerson()| B["LegacyPersonSurrogate<br/>(wrapper)"]
    
    B -->|Serialize| C["JSON"]
    
    C -->|Deserialize| D["LegacyPersonSurrogate"]
    
    D -->|ToPerson()| E["LegacyPerson<br/>restored"]
    
    style A fill:#ffccbc
    style B fill:#fff9c4
    style C fill:#c8e6c9
    style D fill:#fff9c4
    style E fill:#a5d6a7
```

## Diagram 4: Custom Validation

```mermaid
graph LR
    A["Incoming JSON<br/>age: 200"]
    
    A -->|Deserialize| B["AgeValidator<br/>Read()"]
    
    B -->|Validate| C{Is Age 0-150?}
    
    C -->|No| D["❌ JsonException<br/>Invalid age"]
    C -->|Yes| E["✅ Person<br/>Age: 25"]
    
    style D fill:#ffccbc
    style E fill:#c8e6c9
```

## Diagram 5: Conditional Serialization

```mermaid
graph LR
    A["Person<br/>Name: Alice<br/>Email: null<br/>Phone: empty"]
    
    A -->|DefaultIgnoreCondition<br/>WhenWritingNull| B["JSON<br/>{<br/>  'Name': 'Alice'<br/>}"]
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
```

## Diagram 6: Security: Sensitive Data Handling

```mermaid
graph LR
    A["❌ UNSAFE<br/>Serialize User<br/>Password: exposed"]
    
    A -->|Convert to| B["✅ SAFE<br/>User with<br/>[JsonIgnore]<br/>on Password"]
    
    B -->|Serialize| C["JSON<br/>no Password"]
    
    style A fill:#ffccbc
    style B fill:#fff9c4
    style C fill:#c8e6c9
```

## Diagram 7: Converter Type Support

```mermaid
graph TD
    A["JsonConverter<br/>Custom Type Transform"]
    
    A --> B["DateTime"]
    A --> C["Decimal"]
    A --> D["Enum"]
    A --> E["Custom Objects"]
    
    B --> B1["Parse string<br/>to DateTime"]
    C --> C1["String amount<br/>to Decimal"]
    D --> D1["String to Enum"]
    E --> E1["Transform structure"]
    
    style B1 fill:#fff9c4
    style C1 fill:#fff9c4
    style D1 fill:#fff9c4
    style E1 fill:#fff9c4
```

## Diagram 8: Custom Serialization Approaches

```mermaid
graph TD
    Custom["Custom Serialization<br/>Approaches"]
    
    Custom --> A["[JsonIgnore]<br/>Field exclusion<br/>Simple, fast"]
    Custom --> B["JsonConverter<br/>Type transform<br/>Flexible"]
    Custom --> C["IXmlSerializable<br/>Full XML control<br/>Medium complexity"]
    Custom --> D["Surrogate Pattern<br/>Legacy wrapping<br/>Type isolation"]
    Custom --> E["ISerializable<br/>Full control<br/>Legacy"]
    
    style A fill:#c8e6c9
    style B fill:#fff9c4
    style C fill:#ffe0b2
    style D fill:#ffccbc
    style E fill:#ffccbc
```
