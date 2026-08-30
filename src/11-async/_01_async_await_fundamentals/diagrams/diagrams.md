# Diagramy: Async/Await Fundamentals

## 1. Async/Await Flow

```mermaid
graph TD
    A["Start async Method"] --> B["Code runs synchronously<br/>until await"]
    B --> C{await Encountered}
    C -->|Task continues| D["Thread released<br/>available for other work"]
    D --> E["Task.Delay or I/O<br/>in progress"]
    E --> F["Task Completes"]
    F --> G["Continuation scheduled"]
    G --> H["Code after await<br/>resumes execution"]
    H --> I["End async Method"]
    
    style A fill:#e1f5ff
    style H fill:#c8e6c9
    style I fill:#ffebee
```

## 2. Synchronous vs Asynchronous Timeline

```mermaid
sequenceDiagram
    participant Main as Main Thread
    participant Sync as Sync Method
    participant Async as Async Method
    participant Task as Task/IO

    Note over Main: === SYNCHRONOUS ===
    Main->>Sync: DoWork()
    Sync->>Task: Sleep(2000)
    Task-->>Sync: (blocked 2s)
    Sync-->>Main: Done
    Note over Main: Total: 2.0s (thread blocked)

    Note over Main: === ASYNCHRONOUS ===
    Main->>Async: DoWorkAsync()
    Async->>Task: Task.Delay(2000)
    Task-->>Async: (await - thread released)
    Note over Main: Main thread free for<br/>other work
    Task->>Task: 2s passes...
    Task-->>Async: Completed
    Async-->>Main: Done
    Note over Main: Total: 2.0s (thread free!)
```

## 3. Task States

```mermaid
stateDiagram-v2
    [*] --> Created
    Created --> Running: Start
    Running --> Completed: Success
    Running --> Faulted: Exception
    Running --> Canceled: Cancel
    Completed --> [*]
    Faulted --> [*]
    Canceled --> [*]
```

## 4. Task.WhenAll Composition

```mermaid
graph LR
    A["Task 1<br/>1000ms"] --> D["Task.WhenAll"]
    B["Task 2<br/>1500ms"] --> D
    C["Task 3<br/>800ms"] --> D
    D --> E["All Completed<br/>~1500ms total"]
    
    style A fill:#ffcccc
    style B fill:#ffcccc
    style C fill:#ffcccc
    style D fill:#fff9c4
    style E fill:#c8e6c9
```

## 5. Task.WhenAny Composition

```mermaid
graph LR
    A["Task 1<br/>3000ms"] --> D["Task.WhenAny"]
    B["Task 2<br/>1000ms<br/>(fastest)"] --> D
    C["Task 3<br/>2000ms"] --> D
    D --> E["First Completed<br/>~1000ms"]
    
    style B fill:#a5d6a7
    style D fill:#fff9c4
    style E fill:#c8e6c9
```

## 6. Exception Handling in Async

```mermaid
graph TD
    A["async Task Method"] --> B["await DoWorkAsync"]
    B --> C{Exception?}
    C -->|No| D["Continue"]
    C -->|Yes| E["Exception aggregated<br/>in Task"]
    E --> F["catch block"]
    F --> G["Handle error"]
    G --> H["finally block"]
    H --> I["Cleanup"]
    I --> J["End"]
    D --> H
    
    style E fill:#ffcdd2
    style F fill:#fff9c4
    style I fill:#c8e6c9
```
