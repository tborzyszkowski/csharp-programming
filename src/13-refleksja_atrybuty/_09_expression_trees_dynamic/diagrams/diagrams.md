# Diagramy: Expression Trees & Dynamic

## Diagram 1: Expression Tree Structure

```mermaid
graph TD
    A["Lambda"]
    B["Body: Add"]
    C["Left: Parameter x"]
    D["Right: Constant 5"]
    
    A --> B
    B --> C
    B --> D
    
    style A fill:#c8e6c9
    style B fill:#fff9c4
```

## Diagram 2: Expression Compilation

```mermaid
graph LR
    A["Expression Tree"]
    B["Compile()"]
    C["Delegate"]
    D["Execute"]
    
    A --> B --> C --> D
    
    style C fill:#c8e6c9
```

## Diagram 3: Dynamic Type Resolution

```mermaid
graph LR
    A["dynamic obj"]
    B["Compile-time"]
    C["Runtime"]
    D["Method Resolution"]
    
    A -->|No check| B --> C --> D
    
    style C fill:#fff9c4
    style D fill:#c8e6c9
```

## Diagram 4: Performance Hierarchy

```mermaid
graph TD
    A["Fastest"]
    B["Direct Call<br/>~10ns"]
    C["Compiled Expr<br/>~20ns"]
    D["Dynamic<br/>~100ns"]
    E["Reflection<br/>~200ns"]
    F["Slowest"]
    
    A --> B --> C --> D --> E --> F
    
    style B fill:#c8e6c9
    style C fill:#fff9c4
    style D fill:#ffe0b2
    style E fill:#ffccbc
```

## Diagram 5: DynamicObject Override

```mermaid
graph LR
    A["DynamicObject"]
    B["TryGetMember"]
    C["TrySetMember"]
    D["Dictionary"]
    
    A --> B
    A --> C
    B --> D
    C --> D
    
    style D fill:#c8e6c9
```

## Diagram 6: Reflection vs Expression vs Dynamic

```mermaid
graph LR
    Call["Method Call"]
    
    Call -->|Compile-time<br/>Type Safe| Direct["Direct<br/>FASTEST"]
    Call -->|Compile-time<br/>Dynamic Compile| Expr["Expression<br/>FAST"]
    Call -->|Runtime<br/>No Type Check| Dyn["Dynamic<br/>MEDIUM"]
    Call -->|Runtime<br/>Reflection| Refl["Reflection<br/>SLOW"]
    
    style Direct fill:#c8e6c9
    style Expr fill:#fff9c4
    style Dyn fill:#ffe0b2
    style Refl fill:#ffccbc
```

## Diagram 7: Expression Visitor

```mermaid
graph LR
    A["Expression"]
    B["ExpressionVisitor"]
    C["Visit BinaryExpr"]
    D["Visit Constant"]
    E["Analyze"]
    
    A --> B --> C
    B --> D
    C --> E
    D --> E
    
    style E fill:#c8e6c9
```

## Diagram 8: COM Interop with Dynamic

```mermaid
graph LR
    A["dynamic excelApp"]
    B["Runtime Resolution"]
    C["COM Interface"]
    D["Office Method"]
    
    A --> B --> C --> D
    
    style A fill:#c8e6c9
```
