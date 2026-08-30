# Diagramy - Delegacje: Definicja i Składnia

## 1. Deklaracja Delegacji - Składnia

```mermaid
graph TD
    A["public delegate &lt;return-type&gt; &lt;name&gt; (&lt;params&gt;)"]
    
    A --> B["Return Type"]
    A --> C["Delegate Name"]
    A --> D["Parameters"]
    
    B --> B1["void - no return"]
    B --> B2["int, string, etc - with return"]
    
    D --> D1["No params"]
    D --> D2["Single param"]
    D --> D3["Multiple params"]
    
    style A fill:#e3f2fd
    style B fill:#bbdefb
    style C fill:#bbdefb
    style D fill:#bbdefb
```

## 2. Instancjowanie - Trzy Metody

```mermaid
graph TB
    INST["Instancjowanie Delegacji"]
    
    INST --> M1["1. Named Method"]
    INST --> M2["2. Anonymous Method"]
    INST --> M3["3. Lambda Expression"]
    
    M1 --> M1A["delegate_var = MethodName;"]
    M1 --> M1B["delegate_var = obj.Method;"]
    
    M2 --> M2A["delegate_var = delegate(params) { ... };"]
    
    M3 --> M3A["delegate_var = (params) => expression;"]
    M3 --> M3B["delegate_var = (params) => { ... };"]
    
    M1A --> EQUIV["All equivalent!"]
    M2A --> EQUIV
    M3A --> EQUIV
    M3B --> EQUIV
    
    style INST fill:#fff9c4
    style M1 fill:#c8e6c9
    style M2 fill:#c8e6c9
    style M3 fill:#c8e6c9
    style EQUIV fill:#4caf50,color:#fff
```

## 3. Odwoływanie - Metody

```mermaid
graph TB
    INVOKE["Wywoływanie Delegacji"]
    
    INVOKE --> M1["Direct Call"]
    INVOKE --> M2["Invoke Method"]
    INVOKE --> M3["Safe Null Check"]
    
    M1 --> M1A["delegate_var(args)"]
    M2 --> M2A["delegate_var.Invoke(args)"]
    M3 --> M3A["delegate_var?.Invoke(args)"]
    
    M1A --> EQUIV["Identyczne!"]
    M2A --> EQUIV
    M3A --> EQUIV
    
    style INVOKE fill:#f0f4ff
    style M1 fill:#90caf9
    style M2 fill:#90caf9
    style M3 fill:#90caf9,stroke:#ff9800,stroke-width:2px
```

## 4. Multicast Delegates - Łączenie

```mermaid
graph LR
    D1["Logger delegate"]
    
    D1 -->|"+= Handler1"| R1["Logger += Handler1"]
    R1 -->|"+= Handler2"| R2["Logger += Handler1 + Handler2"]
    R2 -->|"+= Handler3"| R3["Logger = Handler1 + Handler2 + Handler3"]
    
    R3 -->|"Invoke()"| EXEC["Execute ALL three"]
    
    EXEC --> H1["Handler1()"]
    EXEC --> H2["Handler2()"]
    EXEC --> H3["Handler3()"]
    
    style R3 fill:#9c27b0,color:#fff
    style EXEC fill:#4caf50,color:#fff
```

## 5. Multicast Delegates - Usuwanie

```mermaid
graph LR
    A["Logger = H1 + H2 + H3"]
    
    A -->|"-= H2"| B["Logger = H1 + H3"]
    
    B -->|"-= H1"| C["Logger = H3"]
    
    C -->|"-= H3"| D["Logger = empty/null"]
    
    D -->|"?.Invoke()"| E["Safe - no error!"]
    
    style A fill:#bbdefb
    style B fill:#81c784
    style C fill:#81c784
    style D fill:#4caf50,color:#fff
    style E fill:#4caf50,color:#fff
```

## 6. Closure - Zmienna Przechwycona

```mermaid
graph TB
    LOOP["for (int i = 0; i &lt; 3; i++) {"]
    
    LOOP --> CAP["Captured i"]
    
    CAP --> D1["Delegate 1 remembers i=0"]
    CAP --> D2["Delegate 2 remembers i=1"]
    CAP --> D3["Delegate 3 remembers i=2"]
    
    D1 --> CALL["Later when called..."]
    D2 --> CALL
    D3 --> CALL
    
    CALL --> R1["Execute with captured value"]
    
    style CAP fill:#ff9800,color:#fff
    style D1 fill:#e3f2fd
    style D2 fill:#e3f2fd
    style D3 fill:#e3f2fd
    style CALL fill:#4caf50,color:#fff
```

## 7. Generic Delegates

```mermaid
graph TB
    GENERIC["delegate T Transform&lt;T&gt;(T input)"]
    
    GENERIC --> INT["Transform&lt;int&gt;"]
    GENERIC --> STR["Transform&lt;string&gt;"]
    GENERIC --> DBL["Transform&lt;double&gt;"]
    
    INT --> INT_EX["d(5) where d = x => x * 2"]
    STR --> STR_EX["d('hello') where d = s => s.ToUpper()"]
    DBL --> DBL_EX["d(3.14) where d = x => x * 2"]
    
    INT_EX --> RESULT1["Result: 10"]
    STR_EX --> RESULT2["Result: 'HELLO'"]
    DBL_EX --> RESULT3["Result: 6.28"]
    
    style GENERIC fill:#e0b0ff,color:#000
    style INT fill:#b2dfdb
    style STR fill:#b2dfdb
    style DBL fill:#b2dfdb
```

## 8. Porównanie: Named vs Anonymous vs Lambda

```mermaid
graph TB
    D["delegate int Op(int a, int b)"]
    
    D --> N["Named Method"]
    D --> A["Anonymous Method"]
    D --> L["Lambda"]
    
    N --> NE["int Add(int x, int y) => x + y;<br/>Op op = Add;"]
    A --> AE["Op op = delegate(int x, int y) { return x + y; };"]
    L --> LE["Op op = (x, y) => x + y;"]
    
    NE --> EQUIV["All produce<br/>identical bytecode"]
    AE --> EQUIV
    LE --> EQUIV
    
    EQUIV --> MODERN["Lambda is most modern"]
    
    style L fill:#ff9800,color:#fff
    style LE fill:#ff9800,color:#fff
    style MODERN fill:#ff9800,color:#fff
```
