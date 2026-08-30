# Diagramy: Performance & Security

## Diagram 1: Performance Comparison

```mermaid
graph LR
    JSON["JSON<br/>35 ms<br/>350 KB"]
    Binary["Binary<br/>12 ms<br/>100 KB"]
    Protobuf["Protobuf<br/>8 ms<br/>80 KB"]
    PBAOT["Protobuf AOT<br/>5 ms<br/>80 KB"]
    
    style JSON fill:#ffccbc
    style Binary fill:#fff9c4
    style Protobuf fill:#ffe0b2
    style PBAOT fill:#c8e6c9
```

## Diagram 2: BinaryFormatter RCE Attack

```mermaid
graph LR
    A["Attacker<br/>creates malicious<br/>binary data"]
    B["Send to<br/>vulnerable server"]
    C["BinaryFormatter<br/>.Deserialize()"]
    D["Gadget chain<br/>triggered"]
    E["Remote Code<br/>Execution!"]
    
    A --> B --> C --> D --> E
    
    style A fill:#ffccbc
    style E fill:#ffccbc
```

## Diagram 3: XXE Attack Flow

```mermaid
graph LR
    A["Attacker<br/>sends XXE XML"]
    B["Server loads<br/>with LTD enabled"]
    C["XML parser<br/>resolves entities"]
    D["File system<br/>accessed"]
    E["Data leak"]
    
    A --> B --> C --> D --> E
    
    style E fill:#ffccbc
```

## Diagram 4: Security Defense Layers

```mermaid
graph TD
    Input["Untrusted Input"]
    L1["Layer 1: Type Safety<br/>Deserialize<T>"]
    L2["Layer 2: Validation<br/>Check values"]
    L3["Layer 3: Sanitization<br/>[JsonIgnore]"]
    L4["Layer 4: Audit Log<br/>Track access"]
    Safe["Safe Data"]
    
    Input --> L1
    L1 --> L2
    L2 --> L3
    L3 --> L4
    L4 --> Safe
    
    style Safe fill:#c8e6c9
```

## Diagram 5: Source Generators vs Reflection

```mermaid
graph TD
    A["System.Text.Json"]
    
    A --> B["Reflection<br/>Runtime type discovery<br/>35ms for 10K"]
    A --> C["Source Generators<br/>Compile-time codegen<br/>5ms for 10K"]
    
    style B fill:#ffccbc
    style C fill:#c8e6c9
```

## Diagram 6: BinaryFormatter Timeline

```mermaid
graph LR
    A["2020<br/>Deprecated<br/>.NET 5"]
    B["2021<br/>Disabled<br/>by default"]
    C["2023<br/>Removed<br/>.NET 8+"]
    D["2024<br/>Complete<br/>removal"]
    
    A -->|1 year| B
    B -->|2 years| C
    C -->|1 year| D
    
    style C fill:#ffccbc
    style D fill:#ffccbc
```

## Diagram 7: Format Decision Tree

```mermaid
graph TD
    Q["Choose Format"]
    
    Q -->|REST API| JSON["✓ JSON<br/>Human readable<br/>Standard"]
    Q -->|Microservices| PB["✓ Protobuf<br/>Fast, small<br/>gRPC compatible"]
    Q -->|Config files| YAML["✓ YAML/JSON<br/>Human readable"]
    Q -->|Legacy| XML["✓ XML<br/>Only if needed"]
    Q -->|BinaryFormatter?| NO["✗ NO!<br/>Security risk<br/>Use JSON"]
    
    style JSON fill:#c8e6c9
    style PB fill:#c8e6c9
    style YAML fill:#a5d6a7
    style XML fill:#fff9c4
    style NO fill:#ffccbc
```

## Diagram 8: Production Serialization Stack

```mermaid
graph TD
    Input["Application Data"]
    
    A["Serialize"]
    B["Source Generators<br/>(compile-time)"]
    C["Fast & Type-safe"]
    
    D["Deserialize"]
    E["Type-safe<br/>Validation"]
    F["Safe & Validated"]
    
    Input --> A --> B --> C
    C --> Output["Binary/JSON"]
    
    Network["Network/Disk"]
    
    Network --> D --> E --> F
    Output --> Network
    
    style C fill:#c8e6c9
    style F fill:#c8e6c9
```
