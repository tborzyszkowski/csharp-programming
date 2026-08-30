# Diagramy: Plugin System

## Diagram 1: Assembly Loading Flow

```mermaid
graph LR
    A["Plugin.dll"]
    B["Assembly.LoadFrom()"]
    C["Assembly Object"]
    D["Explore Types"]
    
    A --> B --> C --> D
    
    style C fill:#c8e6c9
```

## Diagram 2: Plugin Discovery

```mermaid
graph LR
    A["Assembly"]
    B["GetTypes()"]
    C["Filter IPlugin"]
    D["Found Plugins"]
    
    A --> B --> C --> D
    
    style D fill:#c8e6c9
```

## Diagram 3: Plugin Manager Lifecycle

```mermaid
graph LR
    A["Load"]
    B["Initialize"]
    C["Execute"]
    D["Cleanup"]
    E["Unload"]
    
    A --> B --> C --> D --> E
    
    style C fill:#c8e6c9
```

## Diagram 4: Security Whitelisting

```mermaid
graph LR
    DLL["Plugin.dll"]
    Whitelist["Whitelist"]
    
    DLL --> Check{"In Whitelist?"}
    Whitelist --> Check
    
    Check -->|Yes| Allow["✓ Load"]
    Check -->|No| Block["✗ Reject"]
    
    style Allow fill:#c8e6c9
    style Block fill:#ffccbc
```

## Diagram 5: Plugin Architecture

```mermaid
graph TD
    App["Application"]
    Manager["PluginManager"]
    Interface["IPlugin"]
    Plugin1["Plugin1"]
    Plugin2["Plugin2"]
    
    App --> Manager
    Manager --> Interface
    Interface --> Plugin1
    Interface --> Plugin2
    
    style Manager fill:#c8e6c9
    style Plugin1 fill:#fff9c4
    style Plugin2 fill:#fff9c4
```

## Diagram 6: Dependency Injection

```mermaid
graph LR
    Context["PluginContext"]
    Logger["ILogger"]
    Database["IDatabase"]
    
    Context --> Logger
    Context --> Database
    
    Logger -.->|inject| Plugin["IPlugin"]
    Database -.->|inject| Plugin
    
    style Plugin fill:#c8e6c9
```

## Diagram 7: Plugin Attributes

```mermaid
graph LR
    A["[PluginAttribute]"]
    B["Class"]
    C["Name, Version,<br/>Description"]
    
    A --> B --> C
    
    style C fill:#c8e6c9
```

## Diagram 8: Error Handling

```mermaid
graph LR
    Load["LoadFrom()"]
    
    Load -->|Success| Success["✓ Loaded"]
    Load -->|Error| Error["✗ Caught<br/>Log error"]
    
    style Success fill:#c8e6c9
    style Error fill:#ffccbc
```
