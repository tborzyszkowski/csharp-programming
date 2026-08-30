# Diagramy - Ograniczenia Typów Generycznych

## 1. Typy Ograniczeń

```mermaid
graph TD
    A["Ograniczenia where T :"]
    
    A --> B1["Interface<br/>IComparable<T>"]
    A --> B2["Base Class<br/>Entity"]
    A --> B3["Constructor<br/>new()"]
    A --> B4["Reference Type<br/>class"]
    A --> B5["Value Type<br/>struct"]
    A --> B6["Not Null<br/>notnull"]
    
    B1 --> C1["T implementuje interfejs"]
    B2 --> C2["T dziedziczy z klasy"]
    B3 --> C3["T() - konstruktor bez parametrów"]
    B4 --> C4["T jest referencyjny"]
    B5 --> C5["T jest wartościowy"]
    B6 --> C6["T nigdy nie jest null"]
    
    style B1 fill:#ffebee
    style B2 fill:#f3e5f5
    style B3 fill:#e3f2fd
    style B4 fill:#e8f5e9
    style B5 fill:#fff3e0
    style B6 fill:#fce4ec
```

## 2. Hierarchia Klas z Ograniczeniami

```mermaid
graph TB
    A["EntityRepository<T><br/>where T : Entity"]
    
    B["Entity"]
    B1["User : Entity"]
    B2["Product : Entity"]
    
    A --> B
    B --> B1
    B --> B2
    
    C["EntityRepository<User>"]
    D["EntityRepository<Product>"]
    
    A -.->|"pracuje z"| C
    A -.->|"pracuje z"| D
    
    style A fill:#e3f2fd
    style B fill:#fff9c4
    style B1 fill:#c8e6c9
    style B2 fill:#c8e6c9
```

## 3. Kombinacja Ograniczeń

```mermaid
graph LR
    A["T : class, Entity,<br/>ITimestamped, new()"]
    
    A --> A1["class<br/>Reference Type"]
    A --> A2["Entity<br/>Base Class"]
    A --> A3["ITimestamped<br/>Interface"]
    A --> A4["new()<br/>Constructor"]
    
    B["✅ Spełnia:<br/>AuditedEntity"]
    
    A -.-> B
    
    style A fill:#bbdefb
    style A1 fill:#c8e6c9
    style A2 fill:#ffe0b2
    style A3 fill:#f8bbd0
    style A4 fill:#d1c4e9
```

## 4. Default Value dla Typów

```mermaid
graph TD
    A["default(T)"]
    
    A --> B["Value Types<br/>(struct)"]
    A --> C["Reference Types<br/>(class)"]
    
    B --> B1["int: 0"]
    B --> B2["bool: false"]
    B --> B3["DateTime: 0001-01-01"]
    B --> B4["T?: null"]
    
    C --> C1["object: null"]
    C --> C2["string: null"]
    C --> C3["User: null"]
    
    style A fill:#fff9c4
    style B fill:#c8e6c9
    style C fill:#ffccbc
```

## 5. Factory Pattern z new() Constraint

```mermaid
graph LR
    A["Factory<T><br/>where T : new()"]
    
    A --> B["Create()"]
    B --> B1["new T()"]
    B1 --> C["Wymaga bezparametrowego<br/>konstruktora"]
    
    A --> D["CreateAndConfigure()"]
    D --> D1["T instance = new T()"]
    D1 --> D2["configure(instance)"]
    D2 --> E["Zwraca skonfigurowany T"]
    
    style A fill:#e1f5fe
    style C fill:#c8e6c9
    style E fill:#c8e6c9
```

## 6. Specjalizacja Typów Generycznych

```mermaid
graph TB
    A["EntityRepository<T><br/>where T : Entity"]
    
    B["EntityRepository<User>"]
    C["EntityRepository<Product>"]
    D["EntityRepository<Order>"]
    
    A -.-> B
    A -.-> C
    A -.-> D
    
    B --> B1["Add(User)"]
    B --> B2["GetById(int) → User"]
    
    C --> C1["Add(Product)"]
    C --> C2["GetById(int) → Product"]
    
    style A fill:#e0e0e0
    style B fill:#bbdefb
    style C fill:#c8e6c9
    style D fill:#ffe0b2
```

## 7. Reference Type vs Value Type

```mermaid
graph LR
    A["Ograniczenie"]
    
    A --> B["class<br/>Reference Types"]
    A --> C["struct<br/>Value Types"]
    
    B --> B1["Nullable: T: object, string, User"]
    B1 --> B1A["T = null ✅"]
    
    C --> C1["Non-nullable: T: int, bool, DateTime"]
    C1 --> C1A["T = default (wartość)"]
    C1A --> C1B["int: 0, bool: false"]
    
    style B fill:#ffccbc
    style C fill:#c8e6c9
```

## 8. Constraint Flow - Walidacja Kompilacji

```mermaid
stateDiagram-v2
    [*] --> Code: Code with Generic<T>
    
    Code --> Check: Kompilator sprawdza<br/>constraints
    
    Check --> Valid: Constraints OK?
    
    Valid -->|YES| Generate: Generuj kod<br/>dla T
    Valid -->|NO| Error: ❌ Błąd kompilacji
    
    Generate --> Runtime: Runtime: Kod dla<br/>konkretnego T
    
    Error --> [*]
    Runtime --> [*]
    
    style Check fill:#fff9c4
    style Valid fill:#fff9c4
    style Generate fill:#c8e6c9
    style Error fill:#ffccbc
```
