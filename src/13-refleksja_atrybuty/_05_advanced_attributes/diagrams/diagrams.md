# Diagramy: Advanced Attributes

## Diagram 1: Attribute Inheritance

```mermaid
graph TD
    A["BaseDocAttribute"]
    B["AdvDocAttribute"]
    
    A -->|inherits| B
    
    A --> Desc["Description"]
    B --> Desc
    B --> Author["Author"]
    B --> Version["Version"]
    
    style B fill:#c8e6c9
```

## Diagram 2: Attribute Stacking

```mermaid
graph LR
    A["[Author]"]
    B["[Version]"]
    C["[License]"]
    D["Class"]
    
    A --> D
    B --> D
    C --> D
    
    style D fill:#c8e6c9
```

## Diagram 3: Advanced Validation Flow

```mermaid
graph LR
    A["Object"]
    B["Get Properties"]
    C["Check [Range]"]
    D["Check [Pattern]"]
    E["Validate"]
    F["Return Errors"]
    
    A --> B --> C --> D --> E --> F
    
    style F fill:#c8e6c9
```

## Diagram 4: ORM Metadata Extraction

```mermaid
graph LR
    A["[Table]<br/>[Column]"]
    B["Type"]
    C["GetCustomAttribute"]
    D["Dictionary<br/>TableName,<br/>Properties"]
    
    A --> B --> C --> D
    
    style D fill:#c8e6c9
```

## Diagram 5: Composition vs Inheritance

```mermaid
graph LR
    A["Inheritance<br/>BaseAttribute → AdvAttribute"]
    B["Composition<br/>[Author]<br/>[Version]<br/>[License]"]
    
    A -->|tight coupling| Tight["❌ Less flexible"]
    B -->|loose coupling| Loose["✓ More flexible"]
    
    style Loose fill:#c8e6c9
```

## Diagram 6: Constraint Validation

```mermaid
graph LR
    A["Property"]
    B["[Range] or [Pattern]"]
    C["Validate"]
    D["In Range?"]
    D -->|yes| Pass["✓ Valid"]
    D -->|no| Fail["✗ Error"]
    
    A --> B --> C --> D
    
    style Pass fill:#c8e6c9
    style Fail fill:#ffccbc
```

## Diagram 7: Security Attributes

```mermaid
graph LR
    A["[RequiresPermission]<br/>[RateLimit]"]
    B["Method"]
    C["AuthorizationHandler"]
    D["CanAccess()?"]
    
    A --> B --> C --> D
    
    D -->|true| Allow["✓ Execute"]
    D -->|false| Deny["✗ Reject"]
    
    style Allow fill:#c8e6c9
    style Deny fill:#ffccbc
```

## Diagram 8: AllowMultiple Attributes

```mermaid
graph LR
    A["[Requirement]<br/>[Requirement]<br/>[Requirement]"]
    B["Class"]
    C["GetCustomAttributes<br/>Requirement"]
    D["Array of 3<br/>Requirement instances"]
    
    A --> B --> C --> D
    
    style D fill:#c8e6c9
```
