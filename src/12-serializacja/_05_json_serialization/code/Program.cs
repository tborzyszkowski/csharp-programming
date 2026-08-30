using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Basic JSON Serialization
RunExample1();

// Example 2: JsonSerializerOptions
RunExample2();

// Example 3: Nested Objects and Collections
RunExample3();

// Example 4: Attributes and Configuration
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic JSON Serialization ===");
    
    var person = new Person { Name = "Alice", Age = 30 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Person {{ Name = \"{person.Name}\", Age = {person.Age} }}");
    
    // Serialize
    var json = JsonSerializer.Serialize(person);
    Console.WriteLine($"\n  Serialized JSON:");
    Console.WriteLine($"    {json}");
    Console.WriteLine($"    (Size: {Encoding.UTF8.GetByteCount(json)} bytes)");
    
    // Deserialize
    var restored = JsonSerializer.Deserialize<Person>(json)!;
    Console.WriteLine($"\n  Restored object:");
    Console.WriteLine($"    Name: {restored.Name}");
    Console.WriteLine($"    Age: {restored.Age}");
    
    bool matches = person.Name == restored.Name && person.Age == restored.Age;
    Console.WriteLine($"\n  Round-trip successful: {matches}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: JsonSerializerOptions ===");
    
    var person = new Person { Name = "Alice", Age = 30 };
    
    Console.WriteLine("  Options: CamelCase, Pretty-print");
    
    var options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    var json = JsonSerializer.Serialize(person, options);
    Console.WriteLine($"\n  Formatted JSON:");
    foreach (var line in json.Split('\n'))
    {
        Console.WriteLine($"    {line}");
    }
    
    Console.WriteLine("\n  Properties:");
    Console.WriteLine("    ✅ CamelCase naming (Name → name)");
    Console.WriteLine("    ✅ Indented for readability");
    Console.WriteLine("    ✅ Customizable via options");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Nested Objects & Collections ===");
    
    var team = new Team
    {
        Name = "Engineers",
        Members = new()
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 35 }
        }
    };
    
    Console.WriteLine("  Object with nested collection:");
    Console.WriteLine($"    Team: {team.Name}");
    Console.WriteLine($"    Members: {string.Join(", ", team.Members.Select(m => m.Name))}");
    
    var options = new JsonSerializerOptions { WriteIndented = true };
    var json = JsonSerializer.Serialize(team, options);
    
    Console.WriteLine($"\n  Serialized JSON:");
    var lines = json.Split('\n').Take(15);
    foreach (var line in lines)
    {
        Console.WriteLine($"    {line}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Attributes & Configuration ===");
    
    var employee = new Employee
    {
        Id = 1,
        Name = "Charlie",
        Salary = 85000,
        Password = "secret123"  // Should be ignored
    };
    
    Console.WriteLine("  Object with attributes:");
    Console.WriteLine("    [JsonPropertyName(\"id\")] Id");
    Console.WriteLine("    [JsonIgnore] Password");
    
    var json = JsonSerializer.Serialize(employee);
    Console.WriteLine($"\n  Serialized JSON:");
    Console.WriteLine($"    {json}");
    
    Console.WriteLine($"\n  Note: Password field is NOT in JSON");
    Console.WriteLine("    [JsonIgnore] successfully excluded it");
    
    // Verify
    var parsed = JsonSerializer.Deserialize<Employee>(json)!;
    Console.WriteLine($"\n  Deserialized object:");
    Console.WriteLine($"    ID: {parsed.Id}");
    Console.WriteLine($"    Name: {parsed.Name}");
    Console.WriteLine($"    Password (should be empty): {(string.IsNullOrEmpty(parsed.Password) ? "(empty)" : parsed.Password)}");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   JSON SERIALIZATION                                               ║");
    Console.WriteLine("║   System.Text.Json, Modern Approach, Source Generators Ready      ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ JSON Serialization Examples Completed                          ║");
    Console.WriteLine("║   Recommendation: Always use System.Text.Json for new projects    ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Person
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("age")]
    public int Age { get; set; }
}

public class Team
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("members")]
    public List<Person> Members { get; set; } = new();
}

public class Employee
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("salary")]
    public decimal Salary { get; set; }
    
    [JsonIgnore]
    public string Password { get; set; } = string.Empty;
}
