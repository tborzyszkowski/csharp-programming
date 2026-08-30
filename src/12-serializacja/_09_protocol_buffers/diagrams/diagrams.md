# Diagramy: Protocol Buffers

## Diagram 1: Protobuf vs JSON vs Binary

```mermaid
graph LR
    JSON["JSON<br/>Size: 100<br/>Speed: 1x"]
    XML["XML<br/>Size: 200<br/>Speed: 0.5x"]
    Binary["Binary<br/>Size: 50<br/>Speed: 2x"]
    Protobuf["Protobuf<br/>Size: 30<br/>Speed: 5x"]
    
    style JSON fill:#fff9c4
    style XML fill:#ffccbc
    style Binary fill:#ffe0b2
    style Protobuf fill:#c8e6c9
```

## Diagram 2: .proto Message Structure

```mermaid
graph TD
    Proto["message Person {"]
    F1["string name = 1"]
    F2["int32 age = 2"]
    F3["string email = 3"]
    Close["}"]
    
    Proto --> F1
    F1 --> F2
    F2 --> F3
    F3 --> Close
    
    Note1["Field numbers<br/>NEVER change!"]
    
    style F1 fill:#fff9c4
    style F2 fill:#fff9c4
    style F3 fill:#fff9c4
    style Note1 fill:#ffccbc
```

## Diagram 3: Field Number Immutability

```mermaid
graph LR
    V1["V1<br/>name=1<br/>age=2"]
    V2["V2<br/>name=1<br/>age=2<br/>email=3"]
    V3["V3<br/>name=1<br/>age=2<br/>email=3<br/>phone=4"]
    
    V1 -->|Add email<br/>new number| V2
    V2 -->|Add phone<br/>new number| V3
    
    style V1 fill:#fff9c4
    style V2 fill:#ffe0b2
    style V3 fill:#c8e6c9
```

## Diagram 4: Size Comparison

```mermaid
graph LR
    A["JSON<br/>350 KB"]
    B["Binary<br/>100 KB"]
    C["Protobuf<br/>80 KB"]
    
    style A fill:#ffccbc
    style B fill:#fff9c4
    style C fill:#c8e6c9
```

## Diagram 5: Serialization Performance

```mermaid
graph LR
    A["JSON<br/>35 ms"]
    B["Binary<br/>12 ms"]
    C["Protobuf<br/>8 ms"]
    D["Protobuf AOT<br/>5 ms"]
    
    style A fill:#ffccbc
    style B fill:#fff9c4
    style C fill:#ffe0b2
    style D fill:#c8e6c9
```

## Diagram 6: Wire Types

```mermaid
graph TD
    Types["Protobuf Wire Types"]
    
    Types --> T0["Type 0: Varint<br/>int32, int64, enum"]
    Types --> T1["Type 1: Fixed64<br/>double, fixed64"]
    Types --> T2["Type 2: Length-delimited<br/>string, bytes, messages"]
    Types --> T5["Type 5: Fixed32<br/>float, fixed32"]
    
    style T0 fill:#c8e6c9
    style T1 fill:#a5d6a7
    style T2 fill:#fff9c4
    style T5 fill:#ffe0b2
```

## Diagram 7: gRPC Architecture

```mermaid
graph LR
    Client["Client"]
    Network["HTTP/2<br/>+ Protobuf"]
    Server["Server"]
    
    Client -->|Proto message| Network
    Network -->|Proto message| Server
    Server -->|Proto response| Network
    Network -->|Proto response| Client
    
    style Network fill:#c8e6c9
```

## Diagram 8: Version-Safe Evolution

```mermaid
graph TD
    A["V1 Client<br/>name=1, age=2"]
    B["V2 Server<br/>name=1, age=2, email=3"]
    C["V2 Client<br/>name=1, age=2, email=3"]
    D["V1 Server<br/>name=1, age=2"]
    
    A -->|Send: name, age| B
    B -->|Receive: name, age<br/>email defaults| B
    
    C -->|Send: name, age, email| D
    D -->|Receive: name, age<br/>email ignored| D
    
    style B fill:#c8e6c9
    style D fill:#c8e6c9
```
