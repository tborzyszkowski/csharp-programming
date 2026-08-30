# Diagramy: Refleksja - Wprowadzenie

## Diagram 1: Czym jest Refleksja

```mermaid
graph LR
    Code["Program C#<br/>(Compile Time)"]
    IL["IL + Metadata<br/>(Assembly)"]
    Runtime["Runtime<br/>System.Reflection"]
    
    Code -->|Compile| IL
    IL -->|Load| Runtime
    Runtime -->|Inspect| Mirror["🔍 Odkryj strukturę<br/>Properties, Methods,<br/>Attributes"]
    
    style Mirror fill:#c8e6c9
```

## Diagram 2: System.Reflection Namespace Hierarchy

```mermaid
graph TD
    SR["System.Reflection"]
    
    SR --> Assembly["Assembly<br/>DLL/EXE"]
    SR --> Type["Type<br/>Class/Struct"]
    SR --> Member["MemberInfo<br/>Base Class"]
    
    Member --> Prop["PropertyInfo"]
    Member --> Method["MethodInfo"]
    Member --> Field["FieldInfo"]
    Member --> Event["EventInfo"]
    Member --> Ctor["ConstructorInfo"]
    
    style SR fill:#bbdefb
    style Type fill:#c8e6c9
    style Member fill:#fff9c4
```

## Diagram 3: Refleksja Workflow

```mermaid
graph LR
    A["1. Compile Time<br/>C# → IL"]
    B["2. Assembly Load<br/>Read Metadata"]
    C["3. Type Inspection<br/>GetProperties()"]
    D["4. Dynamic Execution<br/>Invoke()"]
    E["5. Runtime Adaptation<br/>Behavior Changes"]
    
    A --> B --> C --> D --> E
    
    style A fill:#ffccbc
    style B fill:#fff9c4
    style C fill:#ffe0b2
    style D fill:#c8e6c9
    style E fill:#a5d6a7
```

## Diagram 4: System.Type - Example

```mermaid
graph TD
    T["typeof(Person)"]
    
    T --> Props["Properties"]
    T --> Methods["Methods"]
    T --> Fields["Fields"]
    T --> Attrs["Attributes"]
    
    Props --> P1["Name (string)"]
    Props --> P2["Age (int)"]
    
    Methods --> M1["Greet()"]
    Methods --> M2["SetAge(int)"]
    
    style T fill:#bbdefb
    style Props fill:#c8e6c9
    style Methods fill:#c8e6c9
```

## Diagram 5: Historia .NET Reflection

```mermaid
timeline
    title Refleksja .NET Timeline
    
    2002 : .NET 1.0 : System.Reflection introduced
    2005 : .NET 2.0 : Generics support
    2007 : .NET 3.5 : Expression Trees
    2010 : .NET 4.0 : Dynamic keyword
    2015 : .NET Core 1.0 : Cross-platform
    2020 : .NET 5.0 : Performance optimizations
    2024 : .NET 9.0 : Source Generators mature
```

## Diagram 6: Performance - Direct vs Reflection

```mermaid
graph LR
    A["Direct Access<br/>person.Name<br/>1x (baseline)"]
    B["Reflection<br/>Uncached<br/>100x slower"]
    C["Reflection<br/>Cached<br/>2x slower"]
    D["Compiled Delegate<br/>1.5x slower"]
    
    style A fill:#c8e6c9
    style B fill:#ffccbc
    style C fill:#fff9c4
    style D fill:#ffe0b2
```

## Diagram 7: Type Safety Loss

```mermaid
graph LR
    A["Compile-Time"]
    B["Direct Code<br/>person.Name<br/>✓ Type-safe"]
    C["Reflection<br/>obj.GetType().GetProperty('Name')<br/>✗ Runtime error if missing"]
    
    A --> B
    A --> C
    
    style B fill:#c8e6c9
    style C fill:#ffccbc
```

## Diagram 8: Assembly Structure

```mermaid
graph TD
    Assembly["Assembly<br/>MyApp.dll"]
    
    Assembly --> Types["Types<br/>(Classes, Structs)"]
    Assembly --> Metadata["Metadata<br/>(Properties, Methods)"]
    Assembly --> IL["IL Code<br/>(Intermediate Language)"]
    
    Types --> Person["Person"]
    Types --> Order["Order"]
    
    Metadata --> Attrs["Attributes"]
    Metadata --> Sigs["Method Signatures"]
    
    style Assembly fill:#bbdefb
```
