using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Basic Serialization Concept
RunExample1();

// Example 2: Object Graph Structure
RunExample2();

// Example 3: Simple Manual Serialization (DFS)
RunExample3();

// Example 4: Cycle Detection
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic Serialization Concept ===");
    
    var person = new Person
    {
        Name = "Alice",
        Age = 30
    };
    
    Console.WriteLine("  Original object (in memory):");
    Console.WriteLine($"    Name: {person.Name}");
    Console.WriteLine($"    Age: {person.Age}");
    
    // Simulate serialization
    var serialized = SimpleSerialize(person);
    Console.WriteLine($"\n  Serialized bytes ({serialized.Length} bytes):");
    Console.WriteLine($"    {string.Join(" ", serialized.Take(20).Select(b => $"{b:X2}"))}...");
    
    // Simulate deserialization
    var restored = SimpleDeserialize(serialized);
    Console.WriteLine($"\n  Restored object:");
    Console.WriteLine($"    Name: {restored.Name}");
    Console.WriteLine($"    Age: {restored.Age}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Object Graph Structure ===");
    
    var company = new Company
    {
        Name = "TechCorp",
        Employees = new()
        {
            new Employee { Name = "Alice", Salary = 70000 },
            new Employee { Name = "Bob", Salary = 65000 }
        }
    };
    
    Console.WriteLine("  Object graph:");
    Console.WriteLine("    Company: TechCorp");
    Console.WriteLine("    ├─ Employee[0]: Alice (70000)");
    Console.WriteLine("    └─ Employee[1]: Bob (65000)");
    
    Console.WriteLine($"\n  Graph traversal (DFS):");
    TraverseGraph(company);
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Manual DFS Serialization ===");
    
    var person = new Person { Name = "Charlie", Age = 25 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Person {{ Name = \"{person.Name}\", Age = {person.Age} }}");
    
    Console.WriteLine("\n  DFS traversal order:");
    var order = new List<string>();
    TraverseDFS(person, order);
    
    for (int i = 0; i < order.Count; i++)
    {
        Console.WriteLine($"    {i + 1}. {order[i]}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Cycle Detection ===");
    
    var nodeA = new Node { Name = "A" };
    var nodeB = new Node { Name = "B" };
    var nodeC = new Node { Name = "C" };
    
    // Create cycle: A → B → C → A
    nodeA.Next = nodeB;
    nodeB.Next = nodeC;
    nodeC.Next = nodeA;
    
    Console.WriteLine("  Graph with cycle:");
    Console.WriteLine("    A → B → C → A (CYCLE!)");
    
    Console.WriteLine("\n  Serializing with cycle detection:");
    var visited = new HashSet<Node>();
    SerializeWithCycleDetection(nodeA, visited, depth: 0);
}

// Helper methods

byte[] SimpleSerialize(Person person)
{
    var bytes = new List<byte>();
    bytes.AddRange(Encoding.UTF8.GetBytes(person.Name));
    bytes.AddRange(BitConverter.GetBytes(person.Age));
    return bytes.ToArray();
}

Person SimpleDeserialize(byte[] data)
{
    var nameLength = 5; // "Alice" or similar
    var name = Encoding.UTF8.GetString(data, 0, nameLength);
    var age = BitConverter.ToInt32(data, nameLength);
    return new Person { Name = name, Age = age };
}

void TraverseGraph(object obj, int depth = 0)
{
    if (obj == null) return;
    
    string indent = new(' ', depth * 2);
    Console.WriteLine($"{indent}Visiting: {obj.GetType().Name}");
    
    var type = obj.GetType();
    var properties = type.GetProperties();
    
    foreach (var prop in properties)
    {
        var value = prop.GetValue(obj);
        if (value is string || value is int || value is double) continue;
        
        if (value is System.Collections.IEnumerable enumerable and not string)
        {
            foreach (var item in enumerable)
            {
                TraverseGraph(item, depth + 1);
            }
        }
        else if (value?.GetType().IsClass ?? false)
        {
            TraverseGraph(value, depth + 1);
        }
    }
}

void TraverseDFS(object obj, List<string> order)
{
    if (obj == null) return;
    
    var type = obj.GetType();
    order.Add($"Enter {type.Name}");
    
    var properties = type.GetProperties();
    foreach (var prop in properties)
    {
        var value = prop.GetValue(obj);
        order.Add($"  Field: {prop.Name} = {value}");
    }
    
    order.Add($"Exit {type.Name}");
}

void SerializeWithCycleDetection(Node node, HashSet<Node> visited, int depth)
{
    if (node == null || visited.Contains(node))
    {
        if (visited.Contains(node))
            Console.WriteLine($"    {"  ".PadRight(depth * 2)}(Cycle detected - skip)");
        return;
    }
    
    visited.Add(node);
    Console.WriteLine($"    {"  ".PadRight(depth * 2)}Node: {node.Name}");
    
    SerializeWithCycleDetection(node.Next, visited, depth + 1);
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   SERIALIZATION CONCEPTS                                           ║");
    Console.WriteLine("║   Object Graphs, DFS, Why Serialize                               ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Example Completed                                              ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class Company
{
    public string Name { get; set; } = string.Empty;
    public List<Employee> Employees { get; set; } = new();
}

public class Employee
{
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}

public class Node
{
    public string Name { get; set; } = string.Empty;
    public Node? Next { get; set; }
}
