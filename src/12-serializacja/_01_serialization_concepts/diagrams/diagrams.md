# Diagramy: Serialization Concepts

## Diagram 1: Serializacja vs Deserializacja

```mermaid
graph LR
    A["Object in Memory<br/>Person {<br/>  Name: Alice<br/>  Age: 30<br/>}"]
    
    A -->|Serialize| B["Byte Stream<br/>41 6C 69 63 65 1E"]
    B -->|Store/Send| C["File / Network<br/>person.dat"]
    C -->|Deserialize| D["Object Restored<br/>Person {<br/>  Name: Alice<br/>  Age: 30<br/>}"]
    
    style A fill:#e8f5e9
    style B fill:#fff9c4
    style C fill:#ffccbc
    style D fill:#e8f5e9
```

## Diagram 2: Object Graph Structure

```mermaid
graph TD
    A["Person<br/>Alice, 30"]
    B["Address<br/>Main St, NYC"]
    C["Country<br/>US, USA"]
    
    A -->|references| B
    B -->|references| C
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#ffe0b2
```

## Diagram 3: Depth-First Search (DFS) Traversal

```mermaid
graph LR
    A["Start: Person"]
    B["Visit Name<br/>field"]
    C["Visit Age<br/>field"]
    D["Visit Address<br/>reference"]
    E["Visit Street<br/>field"]
    F["Visit City<br/>field"]
    G["Visit Country<br/>reference"]
    
    A --> B
    B --> C
    C --> D
    D --> E
    E --> F
    F --> G
    
    style A fill:#ffcdd2
    style G fill:#a5d6a7
```

## Diagram 4: Cycle Detection Problem

```mermaid
graph LR
    A["Node A"]
    B["Node B"]
    C["Node C"]
    
    A -->|Next| B
    B -->|Next| C
    C -->|Next| A
    
    A -.->|CYCLE| A
    
    style A fill:#ffcdd2
    style B fill:#fff9c4
    style C fill:#ffe0b2
```

## Diagram 5: Serialization Formats Comparison

```mermaid
graph TD
    Formats["Serialization Formats"]
    
    Formats --> XML["XML<br/>✅ Human Readable<br/>❌ Large Size<br/>⚠️ Slow"]
    Formats --> Binary["Binary<br/>❌ Not Readable<br/>✅ Small<br/>✅ Fast"]
    Formats --> JSON["JSON<br/>✅ Human Readable<br/>📊 Medium Size<br/>📊 Medium Speed"]
    Formats --> PB["Protocol Buffers<br/>❌ Not Readable<br/>✅ Smallest<br/>✅ Fastest"]
    
    style XML fill:#ffccbc
    style Binary fill:#c8e6c9
    style JSON fill:#b3e5fc
    style PB fill:#f0f4c3
```

## Diagram 6: Serialization Workflow

```mermaid
sequenceDiagram
    participant App as Application
    participant S as Serializer
    participant Storage as File/Network
    
    App->>S: Serialize(object)
    S->>S: Traverse object graph (DFS)
    S->>S: Encode fields to bytes
    S->>S: Handle references & cycles
    S->>Storage: Write bytes
    
    Storage->>S: Read bytes
    S->>S: Parse format
    S->>S: Decode fields
    S->>S: Reconstruct object graph
    S->>App: Deserialize(bytes)
```

## Diagram 7: Object Reference Handling

```mermaid
graph LR
    A["Company"]
    B["Employee Alice"]
    C["Employee Alice"]
    D["Employee Alice"]
    
    A -->|Smart Ref| B
    A -->|Ref to ^1| C
    A -->|Ref to ^1| D
    
    style A fill:#bbdefb
    style B fill:#c8e6c9
    style C fill:#fff9c4
    style D fill:#fff9c4
```

## Diagram 8: Serialization Challenges

```mermaid
graph TD
    Challenges["Serialization Challenges"]
    
    Challenges --> Cycles["Cyclic References<br/>Need visited set"]
    Challenges --> Poly["Polymorphism<br/>Store type info"]
    Challenges --> Private["Private Fields<br/>Use reflection/attrs"]
    Challenges --> Large["Large Graphs<br/>Memory/Performance"]
    
    style Cycles fill:#ffccbc
    style Poly fill:#ffe0b2
    style Private fill:#fff9c4
    style Large fill:#ffcdd2
```
