# Diagramy: Built-in Attributes

## Diagram 1: [Obsolete] Deprecation

```mermaid
graph LR
    A["[Obsolete]"]
    B["Method"]
    C["Compiler"]
    D["Warning/Error"]
    
    A --> B --> C --> D
    
    style D fill:#ffccbc
```

## Diagram 2: [Flags] Enum Benefits

```mermaid
graph LR
    A["Enum"]
    
    A -->|Without [Flags]| Bad["Output: 3"]
    A -->|With [Flags]| Good["Output: Read, Write"]
    
    style Good fill:#c8e6c9
    style Bad fill:#ffccbc
```

## Diagram 3: [Conditional] Compilation

```mermaid
graph LR
    A["[Conditional('DEBUG')]"]
    B["Method"]
    C["Compile"]
    
    A --> B --> C
    
    C -->|DEBUG=true| Include["✓ Included"]
    C -->|DEBUG=false| Exclude["✗ Excluded"]
    
    style Include fill:#c8e6c9
    style Exclude fill:#ffccbc
```

## Diagram 4: DataAnnotations Validation

```mermaid
graph LR
    A["[Required]<br/>[EmailAddress]<br/>[Range]"]
    B["Property"]
    C["Validator"]
    D["Valid/Invalid"]
    
    A --> B --> C --> D
    
    style D fill:#c8e6c9
```

## Diagram 5: Serialization Attributes

```mermaid
graph LR
    A["[Serializable]"]
    B["Class"]
    C["BinaryFormatter"]
    D["Serialized"]
    
    A --> B --> C --> D
    
    B -->|[NonSerialized]| Skip["Skip field"]
    
    style D fill:#c8e6c9
```

## Diagram 6: Built-in Attributes Categories

```mermaid
graph TD
    BuiltIn["Built-in Attributes"]
    
    BuiltIn --> Deprec["Deprecation<br/>[Obsolete]"]
    BuiltIn --> Serial["Serialization<br/>[Serializable]<br/>[NonSerialized]"]
    BuiltIn --> Validation["Validation<br/>[Required]<br/>[Range]<br/>[EmailAddress]"]
    BuiltIn --> Compile["Compilation<br/>[Conditional]<br/>[Flags]"]
    BuiltIn --> Debug["Debug<br/>[DebuggerDisplay]"]
    
    style Deprec fill:#ffccbc
    style Serial fill:#fff9c4
    style Validation fill:#c8e6c9
    style Compile fill:#bbdefb
    style Debug fill:#e1bee7
```

## Diagram 7: [ThreadStatic] Storage

```mermaid
graph LR
    A["[ThreadStatic]<br/>Static Field"]
    
    A --> Thread1["Thread 1<br/>Value: 1"]
    A --> Thread2["Thread 2<br/>Value: 2"]
    A --> Thread3["Thread 3<br/>Value: 3"]
    
    style Thread1 fill:#c8e6c9
    style Thread2 fill:#c8e6c9
    style Thread3 fill:#c8e6c9
```

## Diagram 8: [StructLayout] Memory Control

```mermaid
graph LR
    A["[StructLayout]"]
    
    A -->|Sequential| SeqLayout["Fields in order<br/>Offset 0,4,8..."]
    A -->|Explicit| ExpLayout["Manual offsets<br/>For P/Invoke"]
    A -->|Auto| AutoLayout["Automatic layout"]
    
    style SeqLayout fill:#c8e6c9
    style ExpLayout fill:#c8e6c9
```
