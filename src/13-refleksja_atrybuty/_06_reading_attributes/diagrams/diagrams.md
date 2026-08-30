# Diagramy: Reading Attributes

## Diagram 1: GetCustomAttribute vs GetCustomAttributes

```mermaid
graph LR
    A["Class with Attributes"]
    
    A --> GCA["GetCustomAttribute<T>"]
    A --> GCAS["GetCustomAttributes<T>"]
    
    GCA --> Single["Single: T or null"]
    GCAS --> Multiple["Array: T[]"]
    
    style Single fill:#c8e6c9
    style Multiple fill:#c8e6c9
```

## Diagram 2: Attribute Existence Check

```mermaid
graph LR
    Type["Type"]
    
    Type -->|Method 1| GCA["GetCustomAttribute != null"]
    Type -->|Method 2| IsDef["IsDefined()"]
    Type -->|Method 3| Count["GetCustomAttributes().Length"]
    
    GCA --> Slow["Slower"]
    IsDef --> Fast["Faster!"]
    Count --> Medium["Medium"]
    
    style Fast fill:#c8e6c9
```

## Diagram 3: Reading Attributes at Different Levels

```mermaid
graph LR
    Type["Type"]
    Prop["Property"]
    Method["Method"]
    
    Type --> TypeAttrs["[Class Attrs]"]
    Prop --> PropAttrs["[Prop Attrs]"]
    Method --> MethodAttrs["[Method Attrs]"]
    
    style TypeAttrs fill:#c8e6c9
    style PropAttrs fill:#fff9c4
    style MethodAttrs fill:#ffe0b2
```

## Diagram 4: Inherit Parameter Effect

```mermaid
graph LR
    Base["[Doc]<br/>Base Class"]
    Derived["Derived : Base"]
    
    Base --> Derived
    
    Derived -->|inherit=true| Has["Has [Doc]"]
    Derived -->|inherit=false| NoHas["No [Doc]"]
    
    style Has fill:#c8e6c9
    style NoHas fill:#ffccbc
```

## Diagram 5: GetCustomAttributes Untyped

```mermaid
graph LR
    A["Class"]
    B["GetCustomAttributes()"]
    C["Array<Attribute>"]
    D["OfType filter"]
    
    A --> B --> C --> D
    
    D --> Obs["OfType<Obsolete>"]
    D --> Ser["OfType<Serializable>"]
    
    style C fill:#c8e6c9
```

## Diagram 6: Attribute Caching

```mermaid
graph LR
    A["Non-cached<br/>GetCustomAttributes()×N<br/>SLOW"]
    B["Cached<br/>Dictionary<Type,attrs><br/>FAST"]
    
    A -->|First time| Cache["Cache[type] = attrs"]
    Cache --> B
    
    style B fill:#c8e6c9
```

## Diagram 7: LINQ Filtering Pattern

```mermaid
graph LR
    Types["Types[]"]
    Filter["Where(t => t.HasAttribute<T>)"]
    Result["Filtered List"]
    
    Types --> Filter --> Result
    
    style Result fill:#c8e6c9
```

## Diagram 8: AttributeReader Pattern

```mermaid
graph TD
    Reader["AttributeReader"]
    
    Reader --> GetAll["GetAllAttributes(type)"]
    
    GetAll --> TypeAttrs["Read Type attrs"]
    GetAll --> PropAttrs["Read Prop attrs"]
    GetAll --> MethodAttrs["Read Method attrs"]
    
    TypeAttrs --> Dict["Dictionary<string,<br/>List<Attribute>>"]
    PropAttrs --> Dict
    MethodAttrs --> Dict
    
    style Dict fill:#c8e6c9
```
