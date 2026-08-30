# Diagramy: Advanced Features

## Diagram 1: Polymorphism Challenge

```mermaid
graph TD
    Base["Animal (base class)"]
    D["Dog : Animal"]
    C["Cat : Animal"]
    
    Base --> D
    Base --> C
    
    JSON["JSON: {Name:Buddy}"]
    
    JSON -.->|Unknown type!| Base
    
    style JSON fill:#ffccbc
```

## Diagram 2: Type Discriminator Solution

```mermaid
graph LR
    A["Dog object"]
    B["Serialize with<br/>type discriminator"]
    C["JSON:<br/>{$type:dog,Name:Buddy}"]
    D["Deserialize:<br/>Recognize type"]
    E["Correct Dog object"]
    
    A --> B --> C --> D --> E
    
    style E fill:#c8e6c9
```

## Diagram 3: Versioning Timeline

```mermaid
graph LR
    V1["V1<br/>Name"]
    V2["V2<br/>Name + Email"]
    V3["V3<br/>FirstName + Email"]
    
    V1 -->|Add Email| V2
    V2 -->|Rename Name| V3
    
    style V1 fill:#fff9c4
    style V2 fill:#ffe0b2
    style V3 fill:#c8e6c9
```

## Diagram 4: Backward Compatibility Matrix

```mermaid
graph TD
    A["Compatibility Check"]
    
    A --> B["V1 App"]
    A --> C["V2 App"]
    A --> D["V3 App"]
    
    B --> B1["Reads V1: ✓"]
    B --> B2["Reads V2: ✗"]
    B --> B3["Reads V3: ✗"]
    
    C --> C1["Reads V1: ✓"]
    C --> C2["Reads V2: ✓"]
    C --> C3["Reads V3: ✗"]
    
    D --> D1["Reads V1: ✓"]
    D --> D2["Reads V2: ✓"]
    D --> D3["Reads V3: ✓"]
    
    style B1 fill:#c8e6c9
    style B2 fill:#ffccbc
    style C1 fill:#c8e6c9
    style C2 fill:#c8e6c9
    style D1 fill:#c8e6c9
    style D2 fill:#c8e6c9
    style D3 fill:#c8e6c9
```

## Diagram 5: [JsonExtensionData] for Extra Fields

```mermaid
graph LR
    A["V2 JSON<br/>Name, Email, Phone"]
    B["Deserialize into<br/>V1 class"]
    C["V1 Object<br/>+ ExtraData:<br/>{Phone: ...}"]
    
    A --> B --> C
    
    style C fill:#c8e6c9
```

## Diagram 6: Version Detection

```mermaid
graph LR
    JSON["JSON Input"]
    
    JSON -->|Check fields| A{Which version?}
    
    A -->|Has FirstName| B["V3: FirstName"]
    A -->|No FirstName<br/>Has Email| C["V2: Name + Email"]
    A -->|Only Name| D["V1: Name only"]
    
    style B fill:#fff9c4
    style C fill:#ffe0b2
    style D fill:#ffccbc
```

## Diagram 7: Type Discriminator Pattern

```mermaid
graph TD
    JSON["JSON with $type"]
    
    JSON --> Switch{$type value?}
    
    Switch -->|dog| Dog["Deserialize as Dog"]
    Switch -->|cat| Cat["Deserialize as Cat"]
    Switch -->|unknown| Error["❌ Error"]
    
    style Dog fill:#c8e6c9
    style Cat fill:#c8e6c9
    style Error fill:#ffccbc
```

## Diagram 8: Gradual Migration Strategy

```mermaid
graph LR
    V1["✅ V1 Support<br/>(Legacy)"]
    V2["✅ V2 Support<br/>(Current)"]
    V3["✅ V3 Support<br/>(New)"]
    Deprecate["⏱️ Deprecate V1"]
    Remove["❌ Remove V1"]
    
    V1 -->|Introduce| V2
    V2 -->|Introduce| V3
    V3 -->|Timeline| Deprecate
    Deprecate -->|Deadline| Remove
    
    style V1 fill:#fff9c4
    style V2 fill:#c8e6c9
    style V3 fill:#a5d6a7
    style Deprecate fill:#ffe0b2
    style Remove fill:#ffccbc
```
