# Diagramy: Performance & Best Practices

## Diagram 1: Caching Impact

```mermaid
graph LR
    A["GetCustomAttributes()"]
    B["No Cache<br/>1µs each time"]
    C["With Cache<br/>0.001µs cached"]
    
    A --> B
    A --> C
    
    C -->|1000 calls| Fast["1ms total"]
    B -->|1000 calls| Slow["1000ms total"]
    
    style Fast fill:#c8e6c9
    style Slow fill:#ffccbc
```

## Diagram 2: Reflection Performance Hierarchy

```mermaid
graph TD
    A["Fastest"]
    B["Direct Call<br/>~10ns"]
    C["Compiled Expr<br/>~20ns"]
    D["Cached Reflection<br/>~0.1µs"]
    E["Fresh Reflection<br/>~1µs"]
    F["Slowest"]
    
    A --> B --> C --> D --> E --> F
    
    style B fill:#c8e6c9
    style C fill:#c8e6c9
    style D fill:#fff9c4
    style E fill:#ffccbc
```

## Diagram 3: Caching Strategy

```mermaid
graph LR
    A["Request attrs"]
    B["Cache hit?"]
    C["Return cached"]
    D["No cache"]
    E["Reflect"]
    F["Store in cache"]
    G["Return attrs"]
    
    A --> B
    B -->|Yes| C --> G
    B -->|No| D --> E --> F --> G
    
    style C fill:#c8e6c9
```

## Diagram 4: Source Generators Alternative

```mermaid
graph LR
    A["Runtime Reflection<br/>SLOW"]
    B["Cache required"]
    C["1000x slower<br/>than direct"]
    
    D["Source Generators<br/>COMPILE-TIME"]
    E["Zero runtime cost"]
    F["Same performance<br/>as direct"]
    
    A --> B --> C
    D --> E --> F
    
    style C fill:#ffccbc
    style F fill:#c8e6c9
```

## Diagram 5: ORM Pattern

```mermaid
graph LR
    A["Class with [Table]<br/>[Column] attrs"]
    B["Parse attributes"]
    C["Cache metadata"]
    D["Generate SQL"]
    E["Execute query"]
    
    A --> B --> C --> D --> E
    
    style D fill:#c8e6c9
```

## Diagram 6: DO's and DON'Ts

```mermaid
graph LR
    A["Cache everything"]
    B["Use IsDefined()"]
    C["Compile expressions"]
    
    D["Reflect in loops"]
    E["GetCustomAttributes<br/>+ OfType"]
    F["Ignore null results"]
    
    A --> Good["✓ GOOD"]
    B --> Good
    C --> Good
    
    D --> Bad["✗ BAD"]
    E --> Bad
    F --> Bad
    
    style Good fill:#c8e6c9
    style Bad fill:#ffccbc
```

## Diagram 7: Benchmark Results

```mermaid
graph LR
    A["No Cache<br/>1000ms"]
    B["Dictionary Cache<br/>2ms"]
    C["WeakRef Cache<br/>5ms"]
    D["Generated<br/>0.1ms"]
    
    A -->|500x faster| B
    B -->|2.5x faster| C
    C -->|50x faster| D
    
    style D fill:#c8e6c9
```

## Diagram 8: AOT Compatibility

```mermaid
graph LR
    A["Reflection Code"]
    
    A -->|Runtime<br/>GetMethods()| Bad["❌ AOT Problem"]
    A -->|Annotated<br/>[DynamicallyAccessedMembers]| Good["✓ AOT Safe"]
    
    style Good fill:#c8e6c9
    style Bad fill:#ffccbc
```
