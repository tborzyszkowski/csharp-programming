# Diagramy: Serialization History

## Diagram 1: .NET Serialization Timeline

```mermaid
timeline
    title .NET Serialization Evolution
    
    2002 : BinaryFormatter (Unsafe)
    2003 : XmlSerializer (Readable)
    2006 : WCF DataContract (Structured)
    2010 : JavaScriptSerializer (JSON)
    2015 : Newtonsoft JSON.NET (Industry standard)
    2019 : System.Text.Json (Official)
    2020 : Source Generators (.NET 5+)
    2024 : Native AOT ready
```

## Diagram 2: Technology Adoption Curve

```mermaid
graph LR
    A["2002<br/>BinaryFormatter<br/>100%"] 
    B["2008<br/>XmlSerializer<br/>80%"]
    C["2015<br/>JSON.NET<br/>70%"]
    D["2019<br/>System.Text.Json<br/>50%"]
    E["2024<br/>Mixed<br/>STJ+PB"]
    
    A -->|declining| B
    B -->|declining| C
    C -->|rising| D
    D -->|growing| E
    
    style A fill:#ffccbc
    style B fill:#ffe0b2
    style C fill:#fff9c4
    style D fill:#c8e6c9
    style E fill:#a5d6a7
```

## Diagram 3: Format Complexity vs Performance

```mermaid
graph TD
    A["Serialization Formats<br/>(2024 Landscape)"]
    
    A --> B["BinaryFormatter<br/>❌ DEPRECATED<br/>⚡ Very Fast<br/>❌ Unsafe"]
    A --> C["XmlSerializer<br/>⚠️ Legacy<br/>🐢 Slow<br/>✅ Readable"]
    A --> D["System.Text.Json<br/>✅ Recommended<br/>⚡ Fast<br/>📊 Balanced"]
    A --> E["Protocol Buffers<br/>⚡ Fastest<br/>🗜️ Smallest<br/>❌ Binary"]
    
    style B fill:#ffccbc
    style C fill:#ffe0b2
    style D fill:#c8e6c9
    style E fill:#a5d6a7
```

## Diagram 4: Security Evolution

```mermaid
graph TD
    BF["BinaryFormatter (2002)<br/>RCE Vulnerabilities<br/>❌ Unsafe deserialization"]
    XS["XmlSerializer (2003)<br/>Reflection attacks<br/>⚠️ Better but not perfect"]
    WCF["WCF DataContract (2006)<br/>Type control<br/>✅ Safer"]
    JSON["System.Text.Json (2019)<br/>No auto-invocation<br/>✅✅ Safe by default"]
    SG["Source Generators (2020)<br/>No reflection<br/>✅✅✅ Compile-time safe"]
    
    BF --> XS
    XS --> WCF
    WCF --> JSON
    JSON --> SG
    
    style BF fill:#ffccbc
    style XS fill:#ffe0b2
    style WCF fill:#fff9c4
    style JSON fill:#c8e6c9
    style SG fill:#a5d6a7
```

## Diagram 5: .NET Framework vs .NET Core Timeline

```mermaid
graph LR
    subgraph NET_Framework["".NET Framework (2002-2022)""]
        A["BinaryFormatter<br/>2002-2022"]
        B["XmlSerializer<br/>2003-present"]
        C["WCF<br/>2006-2022"]
        D["Json.NET<br/>2011-present"]
    end
    
    subgraph NET_Core["".NET Core / .NET 5+ (2016-present)""]
        E["System.Text.Json<br/>2019+"]
        F["Source Generators<br/>2020+"]
        G["Protocol Buffers<br/>2020+"]
    end
    
    style A fill:#ffccbc
    style B fill:#ffe0b2
    style C fill:#fff9c4
    style D fill:#c8e6c9
    style E fill:#a5d6a7
    style F fill:#80cbc4
    style G fill:#64b5f6
```

## Diagram 6: Industry Adoption (Web APIs)

```mermaid
pie title Web API Format Adoption 2024
    "JSON" : 92
    "Protocol Buffers (gRPC)" : 5
    "XML" : 2
    "Other" : 1
```

## Diagram 7: Performance Comparison

```mermaid
graph LR
    A["BinaryFormatter<br/>10ms"] --> B["XmlSerializer<br/>50ms"]
    B --> C["System.Text.Json<br/>2ms"]
    C --> D["Source Gen JSON<br/>0.5ms"]
    
    style A fill:#ffccbc
    style B fill:#ffe0b2
    style C fill:#c8e6c9
    style D fill:#80cbc4
```

## Diagram 8: Migration Path Strategy

```mermaid
graph TD
    Old["❌ Old Code<br/>(BinaryFormatter)<br/>Unsafe"]
    
    Old -->|migrate| Step1["⚠️ Step 1<br/>XmlSerializer<br/>Safer"]
    
    Step1 -->|upgrade| Step2["📊 Step 2<br/>System.Text.Json<br/>Modern"]
    
    Step2 -->|optimize| Step3["✅ Step 3<br/>Source Generators<br/>Zero-cost"]
    
    style Old fill:#ffccbc
    style Step1 fill:#ffe0b2
    style Step2 fill:#c8e6c9
    style Step3 fill:#a5d6a7
```
