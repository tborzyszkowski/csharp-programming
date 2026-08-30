# Diagramy: Breakfast Sequential vs Concurrent

## 1. Sequential Timeline

```mermaid
gantt
    title Sequential Breakfast Preparation
    dateFormat HH:mm:ss
    axisFormat %H:%M:%S
    
    Eggs      :eggs, 00:00:00, 3s
    Bread     :bread, after eggs, 2s
    Coffee    :coffee, after bread, 1.5s
    Bacon     :bacon, after coffee, 1s
    
    Total: ~7.5s
```

## 2. Concurrent Timeline

```mermaid
gantt
    title Concurrent Breakfast Preparation
    dateFormat HH:mm:ss
    axisFormat %H:%M:%S
    
    Eggs      :eggs, 00:00:00, 3s
    Bread     :bread, 00:00:00, 2s
    Coffee    :coffee, 00:00:00, 1.5s
    Bacon     :bacon, 00:00:00, 1s
    
    Total: ~3s (longest task)
```

## 3. Task Flow Diagram

```mermaid
graph LR
    A["Start"] --> B["Create Tasks"]
    B --> C["Task 1"]
    B --> D["Task 2"]
    B --> E["Task 3"]
    C --> F["Task.WhenAll"]
    D --> F
    E --> F
    F --> G["All Complete"]
    
    style C fill:#ffcccc
    style D fill:#ffcccc
    style E fill:#ffcccc
    style F fill:#fff9c4
    style G fill:#c8e6c9
```

## 4. Sequential vs Concurrent Comparison

```mermaid
graph TD
    SEQ["Sequential Approach<br/>7500ms"]
    CONC["Concurrent Approach<br/>3000ms"]
    
    SEQ --> |2.5x slower| X["❌ Inefficient"]
    CONC --> |Optimal| Y["✅ Efficient"]
    
    style SEQ fill:#ffcdd2
    style CONC fill:#c8e6c9
```

## 5. When to Use Each

```mermaid
graph TD
    A["Independent Tasks?"] --> |YES| B["Use Concurrent<br/>Task.WhenAll"]
    A --> |NO| C["Sequential<br/>Await each"]
    
    B --> D["✅ Parallel execution<br/>better performance"]
    C --> E["Sequential flow<br/>predictable order"]
    
    style B fill:#c8e6c9
    style D fill:#c8e6c9
    style C fill:#fff9c4
```
