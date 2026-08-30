# Ćwiczenia: Circular References

## 🟢 Basic Level

### Zadanie 1: Identify Cycles
Które mają cykl?

A. A → B → C  
B. A ↔ B  
C. A → B → A  
D. A → B → C → D  

**Rozwiązanie:** B, C (mają zwrotne referencje)

---

### Zadanie 2: HashSet Detection
Zaznacz co się stanie:

```csharp
var visited = new HashSet<Node>();
visited.Add(nodeA);
visited.Add(nodeB);

if (visited.Contains(nodeA))  // ?
    return;
```

**Rozwiązanie:** Returns (nodeA already visited)

---

### Zadanie 3: [JsonIgnore]
Jak ignore back reference?

```csharp
public class Employee
{
    public string Name { get; set; }
    ???  // How to ignore Department?
    public Department Department { get; set; }
}
```

**Rozwiązanie:**
```csharp
[JsonIgnore]
public Department Department { get; set; }
```

---

### Zadanie 4: Reference ID Pattern
Użyj ID zamiast obiektu:

```csharp
public class Node
{
    public int Id { get; set; }
    ???  // NextId instead of Next
    public Node Next { get; set; }
}
```

**Rozwiązanie:**
```csharp
public int? NextId { get; set; }  // ← Use ID
```

---

### Zadanie 5: Cycle Example
Narysuj cykl dla:

```
A → B → C → A
```

**Rozwiązanie:**
```
Node A (visited=false)
  → Node B (visited=false)
    → Node C (visited=false)
      → Node A (visited=true) ← STOP
```

---

## 🟡 Intermediate Level

### Zadanie 6: HashSet Implementation
Zaimplementuj cycle detection:

```csharp
void TraverseDFS(Node node, HashSet<Node> visited, List<string> result)
{
    // TODO: Implement
}
```

**Rozwiązanie:**
```csharp
void TraverseDFS(Node node, HashSet<Node> visited, List<string> result)
{
    if (node == null || visited.Contains(node))
        return;
    
    visited.Add(node);
    result.Add(node.Name);
    
    TraverseDFS(node.Next, visited, result);
}
```

---

### Zadanie 7: BFS Traversal
Implementuj breadth-first search:

```csharp
List<string> TraverseBFS(Node root)
{
    // TODO: Use Queue instead of recursion
}
```

**Rozwiązanie:**
```csharp
List<string> TraverseBFS(Node root)
{
    if (root == null) return new();
    
    var visited = new HashSet<Node> { root };
    var queue = new Queue<Node> { root };
    var result = new List<string>();
    
    while (queue.Count > 0)
    {
        var node = queue.Dequeue();
        result.Add(node.Name);
        
        if (node.Next != null && !visited.Contains(node.Next))
        {
            visited.Add(node.Next);
            queue.Enqueue(node.Next);
        }
    }
    
    return result;
}
```

---

### Zadanie 8: Reference ID Reconstruction
Odtwórz obiekt z IDs:

```csharp
var nodes = JsonSerializer.Deserialize<NodeWithId[]>(json);
// TODO: Reconstruct references using IDs
```

**Rozwiązanie:**
```csharp
var nodeMap = nodes.ToDictionary(n => n.Id);
foreach (var node in nodes)
{
    if (node.NextId.HasValue && nodeMap.TryGetValue(node.NextId.Value, out var next))
        node.Next = next;
}
```

---

### Zadanie 9: Parent-Child Handling
Obsłuż parent pointer bez cyklu:

```csharp
public class TreeNode
{
    public string Name { get; set; }
    
    ???  // Ignore parent to prevent cycle
    public TreeNode Parent { get; set; }
    
    public List<TreeNode> Children { get; set; }
}
```

**Rozwiązanie:**
```csharp
[JsonIgnore]  // ← Ignore parent
public TreeNode Parent { get; set; }
```

---

### Zadanie 10: Graph Serialization
Serializuj graf z cyklami:

```csharp
public class GraphSerializer
{
    public string Serialize(Node root)
    {
        // TODO: Serialize with cycle handling
    }
}
```

**Rozwiązanie:**
```csharp
public string Serialize(Node root)
{
    var visited = new HashSet<Node>();
    var sb = new StringBuilder();
    SerializeNode(root, visited, sb);
    return sb.ToString();
}

private void SerializeNode(Node node, HashSet<Node> visited, StringBuilder sb)
{
    if (node == null || visited.Contains(node))
    {
        sb.Append("[Ref]");
        return;
    }
    
    visited.Add(node);
    sb.Append($"[{node.Name}]");
    if (node.Next != null)
        SerializeNode(node.Next, visited, sb);
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Cycle Detection with Path
Zwróć ścieżkę cyklu:

```csharp
List<string> FindCyclePath(Node root)
{
    // TODO: Return path that creates cycle
}
```

**Rozwiązanie:**
```csharp
List<string> FindCyclePath(Node root)
{
    var path = new List<string>();
    var visiting = new HashSet<Node>();
    
    if (FindCycleDFS(root, path, visiting))
        return path;
    
    return new();
}

bool FindCycleDFS(Node node, List<string> path, HashSet<Node> visiting)
{
    if (node == null) return false;
    
    if (visiting.Contains(node))
    {
        path.Add(node.Name);
        return true;
    }
    
    visiting.Add(node);
    path.Add(node.Name);
    
    if (FindCycleDFS(node.Next, path, visiting))
        return true;
    
    visiting.Remove(node);
    path.RemoveAt(path.Count - 1);
    return false;
}
```

---

### Zadanie 12: Advanced: Cycle Count
Policz liczbę cykli:

```csharp
int CountCycles(Node root)
{
    // TODO: Count how many cycles exist
}
```

---

## 📊 Wskazówki

- ✅ Zawsze track visited nodes
- ✅ Używaj [JsonIgnore] na back references
- ✅ Testuj z rzeczywistymi cyklami
- ✅ Rozważ Reference ID pattern
- ✅ Document cycle handling strategy
- ❌ Nie ignoruj cycle detection
- ❌ Nie zapomnij o back refs
- ❌ Nie używaj unbounded recursion

---

## 🎯 Key Takeaways

Circular References = Powszechny problem w grafach

```
Rozwiązania:
✅ HashSet detection (DFS)
✅ Reference IDs (structural)
✅ [JsonIgnore] (simple)
✅ Custom converter (advanced)
```

Pamiętaj: **Always detect cycles!**
