# Diagramy: Circular References

## Diagram 1: Cycle Problem Visualization

```mermaid
graph LR
    A["Node A"] -->|Next| B["Node B"]
    B -->|Next| C["Node C"]
    C -->|Next| A
    
    A -.->|CYCLE| A
    
    style A fill:#ffccbc
    style B fill:#fff9c4
    style C fill:#fff9c4
```

## Diagram 2: Naive Serialization (Infinite Loop)

```mermaid
graph LR
    A["Start A"]
    B["Visit B"]
    C["Visit C"]
    D["Back to A"]
    E["Loop!"]
    
    A -->|Serialize| B
    B -->|Follow Next| C
    C -->|Follow Next| D
    D -->|Serialize A again| E
    
    style E fill:#ffccbc
```

## Diagram 3: Cycle Detection with HashSet

```mermaid
graph LR
    A["Node A<br/>visited = {A}"]
    B["Node B<br/>visited = {A,B}"]
    C["Node C<br/>visited = {A,B,C}"]
    D["Back to A<br/>Already in visited!<br/>STOP"]
    
    A -->|Add to set| B
    B -->|Add to set| C
    C -->|Check: A in set?<br/>YES → Stop| D
    
    style D fill:#c8e6c9
```

## Diagram 4: Reference ID Solution

```mermaid
graph LR
    A["Node 1<br/>Name: A<br/>NextId: 2"]
    B["Node 2<br/>Name: B<br/>NextId: 3"]
    C["Node 3<br/>Name: C<br/>NextId: 1"]
    
    A -->|ID ref| B
    B -->|ID ref| C
    C -->|ID ref| A
    
    D["JSON:<br/>[<br/>  {Id:1,NextId:2},<br/>  {Id:2,NextId:3},<br/>  {Id:3,NextId:1}<br/>]"]
    
    style D fill:#c8e6c9
```

## Diagram 5: Parent-Child [JsonIgnore]

```mermaid
graph TD
    A["Department<br/>Engineers"]
    B["Employee: Alice<br/>Department: ref"]
    C["Employee: Bob<br/>Department: ref"]
    
    A -->|Employees| B
    A -->|Employees| C
    B -.->|[JsonIgnore]| A
    C -.->|[JsonIgnore]| A
    
    style B fill:#c8e6c9
    style C fill:#c8e6c9
    style A fill:#fff9c4
```

## Diagram 6: DFS vs BFS Traversal

```mermaid
graph LR
    A["DFS<br/>Deep-first<br/>Stack-based"]
    B["BFS<br/>Broad-first<br/>Queue-based"]
    
    A -->|Order: A,B,C| C["A → B → C"]
    B -->|Order: A,(B,C)| D["A → B,C → ..."]
    
    style C fill:#fff9c4
    style D fill:#ffe0b2
```

## Diagram 7: Cycle Detection Strategies

```mermaid
graph TD
    Strategies["Cycle Detection<br/>Strategies"]
    
    Strategies --> A["HashSet<br/>Visited tracking<br/>Simple, effective"]
    Strategies --> B["Reference IDs<br/>Structural encoding<br/>Flexibility"]
    Strategies --> C["[JsonIgnore]<br/>Ignore back refs<br/>Simple"]
    Strategies --> D["Custom Converter<br/>Full control<br/>Complex"]
    
    style A fill:#c8e6c9
    style B fill:#fff9c4
    style C fill:#a5d6a7
    style D fill:#ffe0b2
```

## Diagram 8: Real-World Example: File System

```mermaid
graph TD
    Root["Directory: C:\\<br/>[JsonIgnore] Parent"]
    D1["Directory: Documents<br/>[JsonIgnore] Parent"]
    D2["Directory: Desktop<br/>[JsonIgnore] Parent"]
    F1["File: readme.txt"]
    F2["File: config.json"]
    
    Root -->|Subdirs| D1
    Root -->|Subdirs| D2
    D1 -->|Files| F1
    D2 -->|Files| F2
    
    style Root fill:#bbdefb
    style D1 fill:#c8e6c9
    style D2 fill:#c8e6c9
    style F1 fill:#fff9c4
    style F2 fill:#fff9c4
```
