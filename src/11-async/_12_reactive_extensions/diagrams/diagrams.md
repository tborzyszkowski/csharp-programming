# Diagramy: Reactive Extensions (Rx)

## Diagram 1: Observable vs Enumerable

```mermaid
graph TD
    A["Data Sequence"]
    
    A --> B["Enumerable (Pull)"]
    A --> C["Observable (Push)"]
    
    B --> D["Code asks<br/>for data"]
    B --> E["Lazy evaluation<br/>on iteration"]
    
    C --> F["Data pushes<br/>to code"]
    C --> G["Eager subscription<br/>OnNext callbacks"]
    
    style B fill:#66bb6a
    style C fill:#ab47bc
    style D fill:#c8e6c9
    style E fill:#c8e6c9
    style F fill:#e1bee7
    style G fill:#e1bee7
```

## Diagram 2: Observable Lifecycle

```mermaid
sequenceDiagram
    participant Sub as Subscriber
    participant Obs as Observable
    participant Comp as Component
    
    Sub->>Obs: Subscribe(observer)
    Obs->>Comp: Start
    
    loop Multiple Values
        Comp->>Obs: OnNext(value)
        Obs->>Sub: Emit value
    end
    
    alt Success
        Comp->>Obs: OnCompleted()
        Obs->>Sub: Completion signal
    else Error
        Comp->>Obs: OnError(exception)
        Obs->>Sub: Error signal
    end
    
    Sub->>Obs: Dispose
```

## Diagram 3: Cold vs Hot Observable

```mermaid
graph LR
    A["Observable"]
    
    A --> B["Cold<br/>Unicast"]
    A --> C["Hot<br/>Multicast"]
    
    B --> D["Each subscriber<br/>gets own execution"]
    B --> E["Like Range, Create"]
    
    C --> F["All subscribers<br/>share execution"]
    C --> G["Like Subject, Events"]
    
    style B fill:#64b5f6
    style C fill:#ff8a65
    style D fill:#bbdefb
    style E fill:#bbdefb
    style F fill:#ffe0b2
    style G fill:#ffe0b2
```

## Diagram 4: LINQ Operators Pipeline

```mermaid
graph LR
    A["Observable<br/>Range 1-10"]
    A --> B["Where<br/>x > 5"]
    B --> C["Select<br/>x * 2"]
    C --> D["Take 3"]
    D --> E["Subscribe<br/>Print"]
    
    E --> F["Output:<br/>12, 14, 16"]
    
    style A fill:#4db6ac
    style B fill:#4db6ac
    style C fill:#4db6ac
    style D fill:#4db6ac
    style E fill:#81c784
    style F fill:#c8e6c9
```

## Diagram 5: Subject Pattern

```mermaid
graph TD
    A["Subject"]
    
    A --> B["Both Observable<br/>and Observer"]
    
    B --> C["Act as Consumer<br/>Subscribe to other observables"]
    B --> D["Act as Producer<br/>Emit values to subscribers"]
    
    C --> E["OnNext, OnError,<br/>OnCompleted"]
    D --> F["Multiple subscribers<br/>receive same values"]
    
    style A fill:#ff9800
    style B fill:#ffe0b2
    style C fill:#ffe0b2
    style D fill:#ffe0b2
    style E fill:#ffcc80
    style F fill:#ffcc80
```

## Diagram 6: Error Handling Flow

```mermaid
graph TD
    A["Observable<br/>produces value"]
    
    A --> B{Exception?}
    
    B -->|No| C["OnNext<br/>to observers"]
    B -->|Yes| D["OnError<br/>to observers"]
    
    C --> E["Continue or<br/>OnCompleted"]
    D --> F["Stream terminates"]
    
    E --> F
    
    style A fill:#e3f2fd
    style B fill:#ffcdd2
    style C fill:#c8e6c9
    style D fill:#ffcdd2
    style F fill:#bcaaa4
```

## Diagram 7: Marble Diagram Example

```mermaid
graph LR
    A["--1--2--3--|<br/>Source Observable"]
    B["--2--4--|<br/>After Select x*2"]
    C["--2--|<br/>After Take 1"]
    
    A --> B
    B --> C
    
    style A fill:#b3e5fc
    style B fill:#b2dfdb
    style C fill:#c8e6c9
```

## Diagram 8: Time-based Operators

```mermaid
graph LR
    A["Source"]
    
    A --> B["Delay<br/>Shift all values"]
    A --> C["Throttle<br/>Max frequency"]
    A --> D["Debounce<br/>Wait for pause"]
    A --> E["Timeout<br/>Error if no value"]
    
    style A fill:#e8f5e9
    style B fill:#a5d6a7
    style C fill:#a5d6a7
    style D fill:#a5d6a7
    style E fill:#a5d6a7
```

## Diagram 9: Combination Operators

```mermaid
graph TD
    A["Observable 1: a-b-c"]
    B["Observable 2: x-y-z"]
    
    A --> C["Merge"]
    B --> C
    C --> D["Output:<br/>a-x-b-y-c-z"]
    
    A --> E["Zip"]
    B --> E
    E --> F["Output:<br/>ax-by-cz"]
    
    A --> G["CombineLatest"]
    B --> G
    G --> H["Output:<br/>ax-bx-by-cy-cz"]
    
    style A fill:#fff9c4
    style B fill:#fff9c4
    style D fill:#fff59d
    style F fill:#fff59d
    style H fill:#fff59d
```
