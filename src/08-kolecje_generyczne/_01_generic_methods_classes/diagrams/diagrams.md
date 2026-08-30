# Diagramy - Metody i Klasy Generyczne

## 1. Hierarchia Generyk w C#

```mermaid
graph TD
    A["🎯 Generyki w C#"]
    
    A --> B["Metody Generyczne"]
    A --> C["Klasy Generyczne"]
    A --> D["Interfejsy Generyczne"]
    A --> E["Struktury Generyczne"]
    
    B --> B1["public T Method<T>"]
    B --> B2["public T1 Convert<T1, T2>"]
    
    C --> C1["public class Stack<T>"]
    C --> C2["public class Repository<T>"]
    C --> C3["public class Container<T>"]
    
    D --> D1["IEnumerable<T>"]
    D --> D2["IComparer<T>"]
    
    E --> E1["public struct Pair<T,U>"]
```

## 2. Architektura Stack<T>

```mermaid
graph LR
    A["Stack<T>"]
    
    A --> A1["items: T[]"]
    A --> A2["count: int"]
    
    A2 --> A3["Capacity Management"]
    A3 --> A31["Resize() - 2x growth"]
    
    A --> B["Methods"]
    B --> B1["Push(T item)"]
    B --> B2["Pop() → T"]
    B --> B3["Peek() → T"]
    B --> B4["IsEmpty → bool"]
    B --> B5["Count → int"]
    
    B1 --> B1A["Add to items[count++]"]
    B1A --> B1B["Resize if needed"]
    
    B2 --> B2A["Check if empty"]
    B2A --> B2B["Return items[--count]"]
    
    B3 --> B3A["Check if empty"]
    B3A --> B3B["Return items[count-1]"]
```

## 3. Wariancja Typów

```mermaid
graph TB
    A["Wariancja Typów<br/>(Variance)"]
    
    A --> B["Invariant (zwykły)<br/>public class List<T>"]
    A --> C["Kowariantny (out)<br/>IEnumerable<out T>"]
    A --> D["Kontrawariantny (in)<br/>IComparer<in T>"]
    
    B --> B1["❌ List<Dog> ≠ List<Animal><br/>Nie można przypisać"]
    
    C --> C1["✅ IEnumerable<Dog><br/>⊆ IEnumerable<Animal><br/>Można przypisać do bardziej ogólnego"]
    
    D --> D1["✅ IComparer<Animal><br/>⊆ IComparer<Dog><br/>Bardziej ogólny do bardziej specjalistycznego"]
```

## 4. Relacja Klas w Hierarchii Dziedziczenia

```mermaid
graph TD
    A["Animal"]
    B["Dog"]
    C["Cat"]
    
    A --> B
    A --> C
    
    D["IProducer<out T>"]
    E["DogProducer<br/>: IProducer<Dog>"]
    
    D --> D1["Kowariantny<br/>IProducer<Dog> ⊆ IProducer<Animal>"]
    E --> E1["Zwraca Dog"]
    
    F["IConsumer<in T>"]
    G["AnimalConsumer<br/>: IConsumer<Animal>"]
    
    F --> F1["Kontrawariantny<br/>IConsumer<Animal> ⊆ IConsumer<Dog>"]
    G --> G1["Akceptuje Animal"]
```

## 5. Transformacja Container<T> - Map Pattern

```mermaid
graph LR
    A["Container<int><br/>(42)"]
    
    A -->|"Map(x → x*2)"| B["Container<int><br/>(84)"]
    B -->|"Map(x → x.ToString())"| C["Container<string><br/>'84'"]
    C -->|"Map(x → x.Length)"| D["Container<int><br/>(2)"]
    
    style A fill:#e1f5ff
    style B fill:#b3e5fc
    style C fill:#81d4fa
    style D fill:#4fc3f7
```

## 6. Przepływ Danych w Repository<T>

```mermaid
graph TB
    A["Repository<T>"]
    
    A --> B["private List<T> items"]
    
    A --> C["Add(T item)"]
    C --> C1["Walidacja: item != null"]
    C1 --> C2["items.Add(item)"]
    
    A --> D["Remove(T item)"]
    D --> D1["items.Remove(item)"]
    
    A --> E["Get(predicate)"]
    E --> E1["items.FirstOrDefault(predicate)"]
    
    A --> F["GetAll()"]
    F --> F1["return items.AsReadOnly()"]
```

## 7. Pair<TFirst, TSecond> - Struktura Danych

```mermaid
graph LR
    A["Pair<TFirst, TSecond>"]
    
    A --> B["TFirst First"]
    A --> C["TSecond Second"]
    
    D["Pair<string, int>"]
    E["Pair<string, Dog>"]
    F["Pair<int, List<string>>"]
    
    D --> D1["'Name', 25"]
    E --> E1["'Buddy', Dog object"]
    F --> F1["42, [list of strings]"]
```

## 8. Ogólny Schemat Metody Generycznej

```mermaid
graph TD
    A["Metoda Generyczna"]
    
    A --> B["Sygnatura z parametrem typowym"]
    B --> B1["public T Method<T>(T param)"]
    
    A --> C["Użycie"]
    C --> C1["Kompilatora dedukuje typ T"]
    C2["Lub jawnie: Method<int>(42)"]
    C --> C2
    
    A --> D["Kompilacja"]
    D --> D1["Dla każdego T tworzy specjalizację"]
    
    A --> E["Runtime"]
    E --> E1["Kod maszynowy dla konkretnych T"]
```

## 9. Wariancja - Porównanie

```mermaid
stateDiagram-v2
    [*] --> Invariant: List<T>
    
    Invariant: List<Dog> ≠ List<Animal>
    Invariant: Nie można przypisać ani na którą stronę
    
    [*] --> Covariant: IEnumerable<out T>
    
    Covariant: Dog ⊆ Animal
    Covariant: IEnumerable<Dog> ⊆ IEnumerable<Animal>
    Covariant: Możesz przypisać bardziej specjalistyczne do ogólnego
    
    [*] --> Contravariant: IComparer<in T>
    
    Contravariant: Dog ⊆ Animal
    Contravariant: IComparer<Animal> ⊆ IComparer<Dog>
    Contravariant: Możesz przypisać bardziej ogólny do specjalistycznego
```
