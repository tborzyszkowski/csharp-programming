# Diagramy: System.Activator

## Diagram 1: new vs Activator.CreateInstance

```mermaid
graph LR
    A["Compile-Time<br/>new Person()"]
    B["Runtime Unknown<br/>Activator.CreateInstance(type)"]
    
    A -->|Type known| Result1["Instance"]
    B -->|Type unknown| Result2["Instance"]
    
    style A fill:#c8e6c9
    style B fill:#fff9c4
```

## Diagram 2: CreateInstance Variants

```mermaid
graph TD
    CI["Activator.CreateInstance()"]
    
    CI --> V1["No Parameters"]
    CI --> V2["With Parameters"]
    CI --> V3["Generic<T>"]
    CI --> V4["BindingFlags"]
    
    V1 --> Ex1["new Person()"]
    V2 --> Ex2["new Person(name, age)"]
    V3 --> Ex3["Activator.CreateInstance<Person>()"]
    V4 --> Ex4["Advanced control"]
    
    style CI fill:#bbdefb
    style V1 fill:#c8e6c9
    style V2 fill:#c8e6c9
    style V3 fill:#c8e6c9
```

## Diagram 3: Plugin System Architecture

```mermaid
graph LR
    App["Application"]
    Loader["PluginLoader"]
    Assembly["Loaded Assembly"]
    Plugin["IPlugin Instance"]
    
    App --> Loader
    Loader -->|LoadFrom| Assembly
    Assembly -->|GetType()| IPlugin["IPlugin Type"]
    IPlugin -->|Activator.CreateInstance| Plugin
    Plugin -->|Execute| Result["Result"]
    
    style Plugin fill:#c8e6c9
```

## Diagram 4: DI Container with Activator

```mermaid
graph LR
    Register["Register<TInterface,<br/>TImplementation>"]
    Resolve["Resolve<T>"]
    Lookup["registrations<br/>TInterface → TImplementation"]
    Create["Activator.CreateInstance<br/>(implementationType)"]
    Return["Return TImplementation"]
    
    Register --> Lookup
    Resolve --> Lookup
    Lookup --> Create
    Create --> Return
    
    style Create fill:#c8e6c9
    style Return fill:#a5d6a7
```

## Diagram 5: Performance - new vs Activator

```mermaid
graph LR
    Direct["Direct new<br/>100%<br/>(baseline)"]
    Act["Activator<br/>~500-1000%<br/>(5-10x slower)"]
    ActParam["Activator+Params<br/>~1000-2000%<br/>(10-20x slower)"]
    
    style Direct fill:#c8e6c9
    style Act fill:#fff9c4
    style ActParam fill:#ffccbc
```

## Diagram 6: ORM Mapping Flow

```mermaid
graph LR
    DB["Database<br/>Row"]
    Map["Dictionary<br/>Name,Value"]
    Create["Activator.CreateInstance<T>()"]
    Props["Set Properties<br/>Reflection"]
    Obj["T Instance"]
    
    DB --> Map
    Map --> Create
    Create --> Props
    Props --> Obj
    
    style Obj fill:#c8e6c9
```

## Diagram 7: Generic Type Creation

```mermaid
graph LR
    Open["typeof(List<>)<br/>Open Generic"]
    Make["MakeGenericType<br/>(typeof(int))"]
    Concrete["typeof(List<int>)<br/>Concrete"]
    Activate["Activator.CreateInstance"]
    Instance["List<int> Instance"]
    
    Open --> Make --> Concrete
    Concrete --> Activate --> Instance
    
    style Instance fill:#c8e6c9
```

## Diagram 8: Security - Type Validation

```mermaid
graph TD
    Input["User Input<br/>Type Name"]
    Validate["Whitelist<br/>Check"]
    
    Validate -->|Not Allowed| Block["❌ Reject"]
    Validate -->|Allowed| Create["✓ CreateInstance"]
    
    Create --> Instance["Safe Instance"]
    
    style Block fill:#ffccbc
    style Instance fill:#c8e6c9
```
