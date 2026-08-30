# Diagramy: Binary Serialization

## Diagram 1: BinaryFormatter RCE Vulnerability

```mermaid
graph LR
    A["Malicious Binary<br/>Stream"]
    B["Deserialize()<br/>Auto-invokes"]
    C["Type Embedding<br/>WindowsIdentity"]
    D["Code Execution<br/>Impersonate()"]
    E["System Compromised<br/>❌"]
    
    A --> B
    B --> C
    C --> D
    D --> E
    
    style A fill:#ffccbc
    style B fill:#ffccbc
    style C fill:#ffccbc
    style D fill:#ffccbc
    style E fill:#ffccbc
```

## Diagram 2: Custom Binary Serialization Process

```mermaid
graph LR
    A["Object<br/>Person {<br/>Name: Alice<br/>Age: 30<br/>}"]
    
    A -->|Serialize()| B["Version Header<br/>01"]
    B -->|Write string| C["Name<br/>41 6C 69 63 65"]
    C -->|Write int32| D["Age<br/>1E 00 00 00"]
    D -->|Combine| E["Binary Stream<br/>01 41 6C... 1E..."]
    
    style A fill:#bbdefb
    style E fill:#c8e6c9
```

## Diagram 3: Binary Format Comparison

```mermaid
pie title Binary Format Sizes (1000 objects)
    "Custom Binary" : 16
    "Protocol Buffers" : 12
    "System.Text.Json" : 26
    "XML" : 96
```

## Diagram 4: Security: BinaryFormatter vs Alternatives

```mermaid
graph TD
    A["Deserialization<br/>Methods"]
    
    A --> B["BinaryFormatter<br/>❌ RCE Risk<br/>Auto-invokes methods"]
    A --> C["Custom Binary<br/>✅ Safe<br/>Explicit decode"]
    A --> D["Protocol Buffers<br/>✅ Safe<br/>No auto-invocation"]
    A --> E["System.Text.Json<br/>✅ Safe<br/>No reflection auto-invoke"]
    
    style B fill:#ffccbc
    style C fill:#c8e6c9
    style D fill:#c8e6c9
    style E fill:#c8e6c9
```

## Diagram 5: BinaryFormatter Timeline

```mermaid
timeline
    title BinaryFormatter Lifecycle
    
    2002 : Introduced
    2017 : First RCE vulnerabilities
    2020 : Deprecated (.NET 5)
    2021 : Disabled by default
    2023 : Removed (.NET 8+)
```

## Diagram 6: Versioning in Custom Binary

```mermaid
graph LR
    A["Version 1<br/>Name<br/>Age"]
    
    A -->|Add field| B["Version 2<br/>Name<br/>Age<br/>Email"]
    
    B -->|Backward compat| C["V1 reader<br/>Reads Name, Age<br/>Ignores Email"]
    B -->|Forward compat| D["V2 reader<br/>Reads all fields<br/>Handles missing Email"]
    
    style A fill:#fff9c4
    style B fill:#c8e6c9
```

## Diagram 7: Performance: BinaryFormatter vs Alternatives

```mermaid
graph LR
    A["10,000 objects"]
    
    A -->|BinaryFormatter| B1["150ms<br/>❌ DEPRECATED"]
    A -->|Custom Binary| B2["20ms<br/>✅ 7.5x faster"]
    A -->|Protocol Buffers| B3["15ms<br/>✅ 10x faster"]
    A -->|System.Text.Json| B4["20ms<br/>✅ 7.5x faster"]
    
    style B1 fill:#ffccbc
    style B2 fill:#c8e6c9
    style B3 fill:#a5d6a7
    style B4 fill:#c8e6c9
```

## Diagram 8: Migration Path from BinaryFormatter

```mermaid
graph TD
    A["❌ BinaryFormatter<br/>(Unsafe RCE)"]
    
    A -->|migrate| B["✅ System.Text.Json<br/>(Default choice)"]
    A -->|migrate| C["✅ Protocol Buffers<br/>(High performance)"]
    A -->|migrate| D["✅ Custom Binary<br/>(Full control)"]
    
    style A fill:#ffccbc
    style B fill:#c8e6c9
    style C fill:#a5d6a7
    style D fill:#c8e6c9
```
