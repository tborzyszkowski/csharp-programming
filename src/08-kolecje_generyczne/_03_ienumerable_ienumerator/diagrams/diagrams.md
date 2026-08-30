# Diagramy - IEnumerable i IEnumerator

## 1. IEnumerable i IEnumerator Hierarchy

```mermaid
graph TD
    A["IEnumerable<T>"]
    B["GetEnumerator()"]
    C["IEnumerator<T>"]
    
    A --> B
    B --> C
    
    C --> D["Current"]
    C --> E["MoveNext()"]
    C --> F["Reset()"]
    
    D --> D1["Zwraca T"]
    E --> E1["Zwraca bool"]
    F --> F1["Resetuje pozycję"]
```

## 2. Pętla foreach pod maską

```mermaid
graph LR
    A["foreach (var item in collection)"]
    
    A --> B["collection.GetEnumerator()"]
    B --> C["IEnumerator<T>"]
    
    C --> D["while (enumerator.MoveNext())"]
    D --> E["var item = enumerator.Current"]
    E --> F["(treść pętli)"]
```

## 3. Yield Iterator - Jak Działa

```mermaid
graph TB
    A["public IEnumerator<T> GetEnumerator()"]
    
    A --> B["yield return item1"]
    B --> C["Pauzuj, zwróć kontrolę"]
    C --> D["caller iteruje"]
    D --> E["MoveNext() -> yield return item2"]
    E --> F["Pauzuj znowu"]
    F --> G["...aż yield break"]
```

## 4. Iterator State Machine (uproszczenie)

```mermaid
graph TD
    A["IEnumerator<T>"]
    
    A --> S1["State 0: Start"]
    S1 --> S2["State 1: First Yield"]
    S2 --> S3["State 2: Second Yield"]
    S3 --> S4["State N: End"]
    
    S1 -->|MoveNext()| S2
    S2 -->|MoveNext()| S3
    S3 -->|MoveNext()| S4
    S4 -->|MoveNext()| S5["Return false"]
```
