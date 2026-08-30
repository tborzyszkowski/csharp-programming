# Ćwiczenia: Serialization Concepts

## 🟢 Basic Level

### Zadanie 1: Definiuj Object Graph
Dla poniższej klasy, narysuj object graph (graf obiektu):

```csharp
public class Team
{
    public string Name { get; set; }
    public List<Member> Members { get; set; }
}

public class Member
{
    public string Name { get; set; }
    public int Score { get; set; }
}

var team = new Team 
{ 
    Name = "Wizards",
    Members = new() 
    { 
        new Member { Name = "Alice", Score = 100 },
        new Member { Name = "Bob", Score = 85 }
    }
};
```

**Rozwiązanie:**
```
Team (Wizards)
├─ Members [List]
│  ├─ Member[0] (Alice, 100)
│  └─ Member[1] (Bob, 85)
```

---

### Zadanie 2: Identyfikuj Cykl
Czy poniższy kod ma cykl? Jeśli tak, opisz go:

```csharp
public class Department
{
    public string Name { get; set; }
    public Manager Manager { get; set; }
}

public class Manager
{
    public string Name { get; set; }
    public Department Department { get; set; }
}

var dept = new Department { Name = "IT" };
var mgr = new Manager { Name = "Alice" };
dept.Manager = mgr;
mgr.Department = dept;
```

**Rozwiązanie:**
```
Tak, cykl: Department → Manager → Department
Przyczyna: mgr.Department wskazuje z powrotem na dept
```

---

### Zadanie 3: Formatowanie a Rozmiar
Porównaj rozmiary dla 3 formatów:

```csharp
var person = new Person { Name = "Alice", Age = 30 };
```

| Format | Przykład | Rozmiar |
|--------|----------|---------|
| Binary | `41 6C 69 63 65 1E` | ~10 bytes |
| XML | `<Person><Name>Alice</Name>...` | ~80 bytes |
| JSON | `{"name":"Alice","age":30}` | ~30 bytes |

**Zadanie:** Zakoduj tę osobę w JSON.

**Rozwiązanie:**
```json
{"name":"Alice","age":30}
```

---

### Zadanie 4: Dlaczego Serializacja?
Który scenariusz wymaga serializacji?

A. Zapis gry do pliku  
B. Wysłanie danych do API  
C. Cache'owanie skomplikowanego obiektu  
D. Wszystkie powyższe  

**Rozwiązanie:** D (wszystkie)

---

### Zadanie 5: Cycle Detection Logic
Uzupełnij funkcję:

```csharp
void SerializeWithCycleDetection(object obj, HashSet<object> visited)
{
    if (obj == null || ___________) 
        return;
    
    visited.Add(obj);
    // Serialize obj...
}
```

**Rozwiązanie:**
```csharp
if (obj == null || visited.Contains(obj)) 
    return;
```

---

## 🟡 Intermediate Level

### Zadanie 6: Implementacja Prostego Serializatora
Napisz funkcję `SimpleSerialize`, która zwraca długość grafu (liczbę odwiedzonych obiektów):

```csharp
int CountGraphSize(object root)
{
    var visited = new HashSet<object>();
    CountDFS(root, visited);
    return visited.Count;
}

void CountDFS(object obj, HashSet<object> visited)
{
    if (obj == null || visited.Contains(obj)) return;
    visited.Add(obj);
    
    // TODO: Traverse properties recursively
}

var team = new Team { /*...*/ };
int size = CountGraphSize(team);
```

**Rozwiązanie:**
```csharp
void CountDFS(object obj, HashSet<object> visited)
{
    if (obj == null || visited.Contains(obj)) return;
    visited.Add(obj);
    
    var type = obj.GetType();
    var properties = type.GetProperties();
    
    foreach (var prop in properties)
    {
        var value = prop.GetValue(obj);
        
        if (value is System.Collections.IEnumerable enumerable and not string)
        {
            foreach (var item in enumerable)
                CountDFS(item, visited);
        }
        else if (value?.GetType().IsClass ?? false)
        {
            CountDFS(value, visited);
        }
    }
}
```

---

### Zadanie 7: Wykryj Typ Cyklu
Dla grafu poniżej, określ typ cyklu:

```csharp
var a = new Node { Name = "A" };
var b = new Node { Name = "B" };
var c = new Node { Name = "C" };

a.Next = b;
b.Next = c;
c.Next = a;  // Cykl: A → B → C → A

// vs

a.Next = b;
b.Next = a;  // Cykl: A ↔ B
```

**Rozwiązanie:**
- Pierwszy: Długi cykl (3 węzły)
- Drugi: Krótki cykl (2 węzły, wzajemne)
- Teraz detekcja oba!

---

### Zadanie 8: Obiekt-Referencja vs Duplikacja
Czy te dwa serializowania są równoważne?

```csharp
// Scenario 1: Dzielona referencja
var alice = new Person { Name = "Alice" };
var company = new Company
{
    Employees = new() { alice, alice, alice }
};

// Scenario 2: Kopiowanie
var company = new Company
{
    Employees = new() 
    { 
        new Person { Name = "Alice" },
        new Person { Name = "Alice" },
        new Person { Name = "Alice" }
    }
};
```

**Rozwiązanie:**
Semantyka: TAK (na wyjściu identyczne)  
Rozmiar serializacji: NIE (Scenario 1 mniejszy)  
Deserializacja: Mogą różnić się referencje (Scenario 1 może zachować shared ref)

---

### Zadanie 9: Wybór Formatu
Dla każdego scenariusza, wybierz format:

1. **Config application** → XML / JSON?
2. **High-speed data transmission** → JSON / Protocol Buffers?
3. **Human-readable API** → Binary / JSON?
4. **Compact IoT data** → JSON / Protocol Buffers?

**Rozwiązanie:**
1. JSON (nowoczesny standard)
2. Protocol Buffers (szybki, kompaktowy)
3. JSON (czytelny)
4. Protocol Buffers (mały rozmiar)

---

## 🔴 Advanced Level

### Zadanie 10: Implementacja Detektora Cykli
Napisz funkcję, która zwraca **ścieżkę** cyklu (jeśli istnieje):

```csharp
List<string>? FindCyclePath(Node root)
{
    var path = new List<string>();
    var visiting = new HashSet<Node>();
    var visited = new HashSet<Node>();
    
    if (FindCycleDFS(root, path, visiting, visited))
        return path;
    return null;
}

bool FindCycleDFS(Node node, List<string> path, 
                  HashSet<Node> visiting, HashSet<Node> visited)
{
    if (node == null) return false;
    
    if (visited.Contains(node))
        return false;
    
    if (visiting.Contains(node))
    {
        path.Add(node.Name);
        return true;  // Znalazł cykl!
    }
    
    visiting.Add(node);
    path.Add(node.Name);
    
    if (FindCycleDFS(node.Next, path, visiting, visited))
        return true;
    
    visiting.Remove(node);
    visited.Add(node);
    path.RemoveAt(path.Count - 1);
    
    return false;
}
```

---

### Zadanie 11: Estymacja Rozmiaru Serializacji
Napisz funkcję, która estymuje rozmiar JSON dla obiektu:

```csharp
int EstimateJsonSize(object obj)
{
    var visited = new HashSet<object>();
    return EstimateSizeDFS(obj, visited);
}

int EstimateSizeDFS(object obj, HashSet<object> visited)
{
    if (obj == null) return 4; // "null"
    if (visited.Contains(obj)) return 10; // "$ref"
    
    int size = 0;
    visited.Add(obj);
    
    if (obj is string str)
        return str.Length + 10; // Quotes + escapes
    
    // TODO: Estimate for properties...
    
    return size;
}
```

---

### Zadanie 12: Multi-Format Comparison
Zaimplementuj funkcję porównującą rozmiary 3 formatów:

```csharp
void CompareFormats(object obj)
{
    var xmlSize = EstimateXmlSize(obj);
    var jsonSize = EstimateJsonSize(obj);
    var binarySize = EstimateBinarySize(obj);
    
    Console.WriteLine($"XML: {xmlSize} bytes");
    Console.WriteLine($"JSON: {jsonSize} bytes");
    Console.WriteLine($"Binary: {binarySize} bytes");
    Console.WriteLine($"Smallest: {Math.Min(Math.Min(xmlSize, jsonSize), binarySize)}");
}
```

**Test z real object:**
```csharp
var person = new Person { Name = "Alice", Age = 30 };
CompareFormats(person);
```

---

## 📊 Wskazówki

- ✅ Zawsze rysuj graph visualnie przed implementacją
- ✅ Cycle detection = HashSet<object> + DFS
- ✅ Test na real objects, nie na teoriach
- ✅ Measure actual sizes, nie estymuj
- ❌ Nie ignoruj null checks
- ❌ Nie zapomnij o IEnumerable (collections)

---

## 🎯 Klucz do Sukcesu

Serializacja = DFS + reference tracking + encoding

```
Object Graph → Visit all objects → Encode to bytes → Write
```

Pamiętaj o:
1. Cyklach
2. Referencjach dzielonych
3. Wyborze formatu
4. Performance trade-offs
