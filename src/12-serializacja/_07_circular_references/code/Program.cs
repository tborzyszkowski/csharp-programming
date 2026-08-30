using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Cycle Detection Problem
RunExample1();

// Example 2: HashSet Solution
RunExample2();

// Example 3: Reference ID Solution
RunExample3();

// Example 4: JsonIgnore Back Reference
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Cycle Detection Problem ===");
    
    var nodeA = new Node { Name = "A" };
    var nodeB = new Node { Name = "B" };
    nodeA.Next = nodeB;
    nodeB.Next = nodeA;  // Cycle!
    
    Console.WriteLine("  Graph structure:");
    Console.WriteLine("    A ↔ B (cycle)");
    
    Console.WriteLine("\n  Problem: Naive serialization would:");
    Console.WriteLine("    1. Try to serialize A");
    Console.WriteLine("    2. Go to A.Next (B)");
    Console.WriteLine("    3. Go to B.Next (A) again");
    Console.WriteLine("    4. Infinite loop! ❌");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: HashSet Cycle Detection ===");
    
    var nodeA = new Node { Name = "A" };
    var nodeB = new Node { Name = "B" };
    var nodeC = new Node { Name = "C" };
    nodeA.Next = nodeB;
    nodeB.Next = nodeC;
    nodeC.Next = nodeA;  // Cycle!
    
    Console.WriteLine("  Graph: A → B → C → A (cycle)");
    
    var visited = new HashSet<Node>();
    var order = new List<string>();
    TraverseDFS(nodeA, visited, order);
    
    Console.WriteLine("\n  DFS with cycle detection:");
    for (int i = 0; i < order.Count; i++)
    {
        Console.WriteLine($"    {i + 1}. {order[i]}");
    }
    Console.WriteLine($"    (Stopped after {visited.Count} nodes to prevent infinite loop)");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Reference ID Solution ===");
    
    var nodes = new NodeWithId[]
    {
        new NodeWithId { Id = 1, Name = "A", NextId = 2 },
        new NodeWithId { Id = 2, Name = "B", NextId = 3 },
        new NodeWithId { Id = 3, Name = "C", NextId = 1 }  // Back to A
    };
    
    Console.WriteLine("  Graph with IDs: 1→2→3→1");
    
    var json = JsonSerializer.Serialize(nodes);
    Console.WriteLine($"\n  Serialized JSON:");
    Console.WriteLine($"    {json}");
    
    Console.WriteLine($"\n  No cycles because we use IDs, not object refs!");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: [JsonIgnore] Back Reference ===");
    
    var dept = new Department { Name = "Engineering" };
    var emp1 = new Employee { Name = "Alice", Department = dept };
    var emp2 = new Employee { Name = "Bob", Department = dept };
    
    dept.Employees.Add(emp1);
    dept.Employees.Add(emp2);
    
    Console.WriteLine("  Object graph:");
    Console.WriteLine("    Department");
    Console.WriteLine("      ├─ Employees[0] (Alice)");
    Console.WriteLine("      │   └─ Department (back ref) [JsonIgnore]");
    Console.WriteLine("      └─ Employees[1] (Bob)");
    Console.WriteLine("          └─ Department (back ref) [JsonIgnore]");
    
    var json = JsonSerializer.Serialize(dept);
    Console.WriteLine($"\n  Serialized JSON:");
    Console.WriteLine($"    {json}");
    
    Console.WriteLine($"\n  No cycles because Department field on Employees is ignored!");
}

void TraverseDFS(Node? node, HashSet<Node> visited, List<string> result)
{
    if (node == null || visited.Contains(node))
        return;
    
    visited.Add(node);
    result.Add(node.Name);
    TraverseDFS(node.Next, visited, result);
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   CIRCULAR REFERENCES & ADVANCED PATTERNS                         ║");
    Console.WriteLine("║   Cycle Detection, Reference IDs, JsonIgnore                      ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Circular Reference Examples Completed                         ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Node
{
    public string Name { get; set; } = string.Empty;
    public Node? Next { get; set; }
}

public class NodeWithId
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? NextId { get; set; }
}

public class Department
{
    public string Name { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
}

public class Employee
{
    public string Name { get; set; } = string.Empty;
    
    [JsonIgnore]  // Don't serialize back reference
    public Department? Department { get; set; }
}
