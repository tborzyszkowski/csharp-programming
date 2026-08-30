# Diagramy: Custom Attributes

## Diagram 1: Attribute Structure

```mermaid
graph LR
    A["Custom Attribute"]
    B["Inherit from<br/>System.Attribute"]
    C["Add Constructor"]
    D["Add Properties"]
    E["Add AttributeUsage"]
    
    A --> B --> C --> D --> E
    
    style B fill:#c8e6c9
```

## Diagram 2: AttributeUsage & AttributeTargets

```mermaid
graph TD
    AU["AttributeUsage"]
    
    AU --> AT["AttributeTargets"]
    
    AT --> Class["Class"]
    AT --> Method["Method"]
    AT --> Property["Property"]
    AT --> Field["Field"]
    AT --> Parameter["Parameter"]
    AT --> Constructor["Constructor"]
    
    style AT fill:#fff9c4
```

## Diagram 3: Attribute with Constructor & Named Params

```mermaid
graph LR
    A["[Documentation"]
    B["(summary)"]
    C["Author = ..."]
    D["Version = ...]"]
    E["]"]
    
    A --> B
    B --> C
    C --> D
    D --> E
    
    style B fill:#c8e6c9
    style C fill:#fff9c4
    style D fill:#fff9c4
```

## Diagram 4: Validation Attribute Workflow

```mermaid
graph LR
    A["Class with<br/>Validation Attrs"]
    B["Validator.Validate()"]
    C["GetCustomAttributes"]
    D["Check Constraints"]
    E["Return Errors"]
    
    A --> B --> C --> D --> E
    
    style E fill:#c8e6c9
```

## Diagram 5: AllowMultiple = true

```mermaid
graph LR
    A["[Tag('a')]<br/>[Tag('b')]<br/>[Tag('c')]"]
    B["Class"]
    C["GetCustomAttributes<br/>TagAttribute"]
    D["Array with 3 items"]
    
    A --> B
    B --> C --> D
    
    style D fill:#c8e6c9
```

## Diagram 6: Inherited = true/false

```mermaid
graph TD
    A["[Marked('base')]"]
    B["Base Class"]
    C["Derived : Base"]
    
    A --> B
    B --> C
    
    C -->|Inherited=true| D["✓ Has [Marked]"]
    C -->|Inherited=false| E["✗ No [Marked]"]
    
    style D fill:#c8e6c9
    style E fill:#ffccbc
```

## Diagram 7: Attribute Reading Flow

```mermaid
graph LR
    Type["Type"]
    GetAttr["GetCustomAttribute<T>"]
    Single["Single Attribute"]
    
    Type --> GetAttr --> Single
    
    Type -->|GetCustomAttributes<T>| Multi["Multiple Attributes"]
    
    style Single fill:#c8e6c9
    style Multi fill:#c8e6c9
```

## Diagram 8: Security Attributes

```mermaid
graph LR
    A["[RequiresRole]"]
    B["Method/Class"]
    C["Runtime Check"]
    D["CanAccess()?"]
    
    A --> B --> C --> D
    
    D -->|true| Allow["✓ Execute"]
    D -->|false| Deny["✗ Reject"]
    
    style Allow fill:#c8e6c9
    style Deny fill:#ffccbc
```
