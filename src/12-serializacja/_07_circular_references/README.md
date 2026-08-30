# Temat 7: Circular References & Advanced Patterns

## 🔄 Problem: Circular References

### Czym jest Cykl?

```csharp
public class Node
{
    public string Name { get; set; }
    public Node? Next { get; set; }
}

// Cycle!
var a = new Node { Name = "A" };
var b = new Node { Name = "B" };
a.Next = b;
b.Next = a;  // ← Cycle: A → B → A
```

### Serialization Challenge

```
Naiwna serializacja:
A → Write A
  → Go to A.Next (B)
    → Write B
      → Go to B.Next (A)
        → Write A (again!) ← Infinite loop!
```

---

## ✅ Rozwiązanie 1: Cycle Detection

### HashSet Tracking

```csharp
public class GraphSerializer
{
    public string Serialize(Node root)
    {
        var visited = new HashSet<Node>();
        var sb = new StringBuilder();
        SerializeNode(root, visited, sb);
        return sb.ToString();
    }
    
    private void SerializeNode(Node? node, HashSet<Node> visited, StringBuilder sb)
    {
        if (node == null || visited.Contains(node))
        {
            sb.Append("[Cycle]");
            return;  // Stop recursion
        }
        
        visited.Add(node);
        sb.Append(node.Name);
        
        if (node.Next != null)
        {
            sb.Append(" → ");
            SerializeNode(node.Next, visited, sb);
        }
    }
}

// Usage:
var serializer = new GraphSerializer();
string result = serializer.Serialize(a);  // "A → B → [Cycle]"
```

---

## ✅ Rozwiązanie 2: Reference IDs

### Identity-Based Encoding

```csharp
public class Node
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? NextId { get; set; }  // Use ID, not object ref
}

// Graph serialization with IDs:
var nodeA = new Node { Id = 1, Name = "A", NextId = 2 };
var nodeB = new Node { Id = 2, Name = "B", NextId = 1 };

// Serialize:
var json = JsonSerializer.Serialize(new[] { nodeA, nodeB });
// [{"Id":1,"Name":"A","NextId":2},{"Id":2,"Name":"B","NextId":1}]

// Deserialize and reconstruct:
var nodes = JsonSerializer.Deserialize<Node[]>(json);
var nodeMap = nodes.ToDictionary(n => n.Id);
foreach (var node in nodes)
{
    if (node.NextId.HasValue)
        node.Next = nodeMap[node.NextId.Value];
}
```

---

## ✅ Rozwiązanie 3: JSON Converter with Reference Tracking

```csharp
public class GraphNode
{
    public string Name { get; set; }
    public GraphNode? Next { get; set; }
}

public class GraphNodeConverter : JsonConverter<GraphNode>
{
    private Dictionary<GraphNode, int> nodeIds = new();
    private int nextId = 0;
    
    public override GraphNode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Deserialization logic
        return new GraphNode();
    }
    
    public override void Write(Utf8JsonWriter writer, GraphNode value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        if (!nodeIds.TryGetValue(value, out var id))
        {
            id = nextId++;
            nodeIds[value] = id;
            
            writer.WriteNumber("id", id);
            writer.WriteString("name", value.Name);
            
            if (value.Next != null)
                Write(writer, value.Next, options);  // Recursive
        }
        else
        {
            writer.WriteNumber("ref", id);  // Reference only
        }
        
        writer.WriteEndObject();
    }
}
```

---

## 📊 Graph Traversal Patterns

### DFS (Depth-First Search)

```csharp
public void TraverseDFS(Node? node, HashSet<Node> visited, List<string> result)
{
    if (node == null || visited.Contains(node))
        return;
    
    visited.Add(node);
    result.Add(node.Name);
    
    TraverseDFS(node.Next, visited, result);
}

// Usage:
var visited = new HashSet<Node>();
var order = new List<string>();
TraverseDFS(a, visited, order);
// order = ["A", "B"] (stops at cycle)
```

### BFS (Breadth-First Search)

```csharp
public List<string> TraverseBFS(Node? root)
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

## 🔐 Parent Reference Issue

### Problem

```csharp
public class Employee
{
    public string Name { get; set; }
    public Department Department { get; set; }
}

public class Department
{
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }  // Cycle!
}

// Serialization would include:
// Department → Employees → Dept → Employees → ... (infinite!)
```

### Solution: Ignore Back Reference

```csharp
public class Employee
{
    public string Name { get; set; }
    public int DepartmentId { get; set; }
    
    [JsonIgnore]  // ← Don't serialize back reference
    public Department? Department { get; set; }
}

public class Department
{
    public string Name { get; set; }
    public List<Employee> Employees { get; set; }
}
```

---

## 🎯 Pattern Selection

| Pattern | Use Case | Complexity |
|---------|----------|-----------|
| **Cycle Detection** | Small graphs | Low |
| **Reference IDs** | Known graph structure | Medium |
| **JSON Converter** | Complex serialization | High |
| **Ignore Backref** | Parent-child relations | Low |

---

## 💾 Practical Example: File System

```csharp
public class Directory
{
    public string Name { get; set; }
    public List<File> Files { get; set; } = new();
    
    [JsonIgnore]  // ← Ignore parent reference
    public Directory? Parent { get; set; }
    
    public List<Directory> Subdirectories { get; set; } = new();
}

public class File
{
    public string Name { get; set; }
    public long Size { get; set; }
}

// Serialization:
var root = new Directory { Name = "C:\\" };
var docs = new Directory { Name = "Documents", Parent = root };
root.Subdirectories.Add(docs);

var json = JsonSerializer.Serialize(root);
// No infinite loops because Parent is [JsonIgnore]
```

---

## 📈 Memory Optimization

### Lazy Initialization

```csharp
public class LazyNode
{
    public string Name { get; set; }
    
    [JsonIgnore]
    private Lazy<Node>? nextLazy;
    
    public Node? Next
    {
        get => nextLazy?.Value;
        set => nextLazy = new Lazy<Node>(() => value);
    }
}
```

---

## ✅ Best Practices

✅ Do:
- Zawsze track visited nodes (HashSet)
- Ignore back references với `[JsonIgnore]`
- Use reference IDs dla known graphs
- Test round-trip with cycles
- Document cycle handling strategy

❌ Don't:
- Ignoruj cycle detection (infinite loops!)
- Serializuj parent pointers
- Zapomnij circular reference handling
- Use unbounded recursion

---

## 📚 Summary

**Circular References** = poważny problem serializacji.

**Rozwiązania:**
- ✅ Cycle Detection (HashSet, DFS)
- ✅ Reference IDs (structural approach)
- ✅ Ignore Back References ([JsonIgnore])
- ✅ Custom JSON Converter (full control)

**Best Practice:** Always use `[JsonIgnore]` na back references!

---

## 🎯 Następny Temat

Temat 8: Advanced Features - Polymorphism, Versioning, Backward Compatibility
