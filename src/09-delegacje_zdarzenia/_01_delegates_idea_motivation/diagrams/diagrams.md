# Diagramy - Delegacje: Idea i Motywacja

## 1. Delegacje vs Tradycyjne Podejście

```mermaid
graph TB
    subgraph Traditional["❌ Tradycyjne (bez delegacji)"]
        T1["Print Method"]
        T2["Sum Method"]
        T3["Filter Method"]
        T4["...więcej metod"]
    end
    
    subgraph Delegates["✅ Z Delegacjami"]
        D1["ProcessNumbers Method"]
        D2["Akcept: NumberAction delegate"]
        D3["Print Logic - Inline"]
        D4["Sum Logic - Inline"]
        D5["Filter Logic - Inline"]
    end
    
    T1 --> |"Duża powtarzalność"| CODE["❌ Code Smell"]
    Delegates --> |"DRY Principle"| CLEAN["✅ Clean Code"]
```

## 2. Late Binding Flow

```mermaid
graph LR
    USER["Użytkownik"]
    RUNTIME["Runtime"]
    DELEGATE["Delegacja"]
    METHOD1["Add Method"]
    METHOD2["Multiply Method"]
    METHOD3["Subtract Method"]
    
    USER -->|"Wybór w Runtime"| RUNTIME
    RUNTIME -->|"Przydziela metodę"| DELEGATE
    DELEGATE --> METHOD1
    DELEGATE --> METHOD2
    DELEGATE --> METHOD3
    
    DELEGATE -->|"Wykonuje wybraną"| RESULT["Wynik"]
    
    style RUNTIME fill:#ffeb3b
    style DELEGATE fill:#2196f3
```

## 3. Type Safety - Kompilator Sprawdza

```mermaid
graph TB
    A["delegate int Operation int, int"]
    
    A --> B["✅ (x,y) => x + y"]
    A --> C["✅ (x,y) => x * y"]
    A --> D["❌ n => Console.WriteLine n"]
    A --> E["❌ (x,y) => x > y"]
    A --> F["❌ (x,y,z) => x + y + z"]
    
    B --> OK["OK - Typy się zgadzają"]
    C --> OK
    D --> ERROR["ERROR - Zwraca void, oczekujesz int"]
    E --> ERROR2["ERROR - Zwraca bool, oczekujesz int"]
    F --> ERROR3["ERROR - 3 parametry, oczekujesz 2"]
    
    style A fill:#e3f2fd
    style OK fill:#c8e6c9
    style ERROR fill:#ffcdd2
    style ERROR2 fill:#ffcdd2
    style ERROR3 fill:#ffcdd2
```

## 4. Strategy Pattern - Architektura

```mermaid
graph TB
    CLIENT["Client Code"]
    PROCESSOR["DataProcessor"]
    STRATEGY["Strategy Delegate"]
    
    IMPL1["Strategy: Filter Odd"]
    IMPL2["Strategy: Filter Even"]
    IMPL3["Strategy: Sum All"]
    
    CLIENT -->|"new DataProcessor()"| PROCESSOR
    CLIENT -->|"process(data, strategy)"| PROCESSOR
    PROCESSOR -->|"Execute strategy"| STRATEGY
    
    STRATEGY --> IMPL1
    STRATEGY --> IMPL2
    STRATEGY --> IMPL3
    
    style CLIENT fill:#fff9c4
    style PROCESSOR fill:#b3e5fc
    style STRATEGY fill:#b2dfdb
```

## 5. Callback Pattern - Timeline

```mermaid
sequenceDiagram
    participant Client
    participant Downloader
    participant NetworkIO as Network I/O
    participant Callback
    
    Client->>Downloader: Download(url, callback)
    Downloader->>NetworkIO: Fetch Data
    Note over NetworkIO: Takes time...
    NetworkIO-->>Downloader: Data Ready
    Downloader->>Callback: callback(data)
    Callback->>Client: Display Result
```

## 6. Multicast Delegates

```mermaid
graph TB
    NOTIFY["NotificationAction Delegate"]
    
    NOTIFY --> CB1["Callback 1: Console Logger"]
    NOTIFY --> CB2["Callback 2: File Logger"]
    NOTIFY --> CB3["Callback 3: Email Sender"]
    NOTIFY --> CB4["Callback 4: Database Logger"]
    
    CB1 --> EXEC["Execute ALL sequentially"]
    CB2 --> EXEC
    CB3 --> EXEC
    CB4 --> EXEC
    
    EXEC --> RESULT["All callbacks executed"]
    
    style NOTIFY fill:#9c27b0,color:#fff
    style EXEC fill:#4caf50,color:#fff
    style RESULT fill:#2196f3,color:#fff
```

## 7. Delegacja jako Typ

```mermaid
graph LR
    subgraph TypeSystem["C# Type System"]
        INT["int type"]
        STRING["string type"]
        DELEGATE["delegate type"]
    end
    
    INT --> VAR1["int x = 5"]
    STRING --> VAR2["string s = 'hello'"]
    DELEGATE --> VAR3["NumberAction = lambda"]
    
    VAR3 --> COMPILE["Compiler checks"]
    COMPILE --> SAFE["Type safe!"]
    
    style TypeSystem fill:#f5f5f5
    style DELEGATE fill:#ff9800,color:#fff
    style SAFE fill:#4caf50,color:#fff
```

## 8. Real-World: Event Handling Pipeline

```mermaid
graph LR
    USER["User Action<br/>Click Button"]
    EVENT["Event Fired"]
    HANDLER1["Handler 1<br/>Update UI"]
    HANDLER2["Handler 2<br/>Log Action"]
    HANDLER3["Handler 3<br/>Send Telemetry"]
    
    USER -->|"Event raised"| EVENT
    EVENT --> HANDLER1
    EVENT --> HANDLER2
    EVENT --> HANDLER3
    
    HANDLER1 -->|"Cascade"| RESULT["System Updated"]
    HANDLER2 --> RESULT
    HANDLER3 --> RESULT
    
    style USER fill:#fff9c4
    style EVENT fill:#ff9800,color:#fff
    style RESULT fill:#4caf50,color:#fff
```

## 9. Porównanie: Function Pointer (C) vs Delegate (C#)

```mermaid
graph TB
    subgraph C["C - Function Pointer"]
        C1["int (*operation) = &add"]
        C2["No type checking"]
        C3["Memory unsafe"]
    end
    
    subgraph CSharp["C# - Delegate"]
        CS1["delegate int Operation(int,int)"]
        CS2["Full type checking"]
        CS3["Memory safe"]
    end
    
    C --> UNSAFE["⚠️ Unsafe"]
    CSharp --> SAFE["✓ Safe"]
```
