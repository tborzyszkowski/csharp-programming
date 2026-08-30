# Diagramy: Type Inspection

## Diagram 1: GetProperties() Workflow

```mermaid
graph LR
    T["typeof(Person)"]
    GP["GetProperties()"]
    Props["PropertyInfo[]"]
    Iterate["Iterate properties"]
    
    T --> GP
    GP --> Props
    Props --> Iterate
    
    Iterate --> Name["Name (string)"]
    Iterate --> Age["Age (int)"]
    Iterate --> Email["Email (string)"]
    
    style Props fill:#c8e6c9
```

## Diagram 2: GetMethods() with BindingFlags

```mermaid
graph TD
    GetMethods["GetMethods()"]
    
    GetMethods --> BF["BindingFlags"]
    
    BF --> Public["Public"]
    BF --> Private["Private/NonPublic"]
    BF --> Static["Static"]
    BF --> Instance["Instance"]
    
    Public --> Add["Add()"]
    Public --> Subtract["Subtract()"]
    Private --> Internal["_Helper()"]
    Static --> Factory["Create()"]
    
    style BF fill:#fff9c4
```

## Diagram 3: Generic Type Inspection

```mermaid
graph LR
    A["typeof(List<int>)"]
    B["IsGenericType: true"]
    C["GetGenericArguments()"]
    D["int"]
    
    A --> B
    B --> C
    C --> D
    
    style A fill:#c8e6c9
```

## Diagram 4: Inheritance Hierarchy

```mermaid
graph TD
    T["Poodle"]
    P1["Dog"]
    P2["Animal"]
    P3["Object"]
    
    T --> P1
    P1 --> P2
    P2 --> P3
    
    style T fill:#c8e6c9
    style P1 fill:#fff9c4
    style P2 fill:#ffe0b2
```

## Diagram 5: MemberInfo Hierarchy

```mermaid
graph TD
    MI["MemberInfo"]
    
    MI --> PI["PropertyInfo"]
    MI --> MeI["MethodInfo"]
    MI --> FI["FieldInfo"]
    MI --> EI["EventInfo"]
    MI --> CI["ConstructorInfo"]
    
    style MI fill:#bbdefb
    style PI fill:#c8e6c9
    style MeI fill:#c8e6c9
    style FI fill:#c8e6c9
```

## Diagram 6: MethodInfo Details

```mermaid
graph LR
    M["MethodInfo"]
    
    M --> Name["Name"]
    M --> RetType["ReturnType"]
    M --> Params["GetParameters()"]
    M --> IsStatic["IsStatic"]
    M --> IsVirtual["IsVirtual"]
    
    Params --> P1["param1: int"]
    Params --> P2["param2: string"]
    
    style M fill:#bbdefb
```

## Diagram 7: Performance - Caching

```mermaid
graph LR
    A["Non-cached<br/>GetProperties() × N times<br/>SLOW"]
    B["Cached<br/>Cache[Type] = props<br/>FAST"]
    
    style A fill:#ffccbc
    style B fill:#c8e6c9
```

## Diagram 8: Type Inspection Flow

```mermaid
graph LR
    A["1. Get Type"]
    B["2. Get Members"]
    C["3. Get Metadata"]
    D["4. Cache Results"]
    E["5. Use Cached"]
    
    A --> B --> C --> D
    D --> E
    E --> D
    
    style D fill:#c8e6c9
```
