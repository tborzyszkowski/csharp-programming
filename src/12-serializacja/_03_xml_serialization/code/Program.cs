using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Basic XmlSerializer
RunExample1();

// Example 2: Attributes Control
RunExample2();

// Example 3: Collections with XmlArray
RunExample3();

// Example 4: Round-trip Serialization
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic XmlSerializer ===");
    
    var person = new Person { Name = "Alice", Age = 30 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Person {{ Name = \"{person.Name}\", Age = {person.Age} }}");
    
    var serializer = new XmlSerializer(typeof(Person));
    using var stream = new MemoryStream();
    serializer.Serialize(stream, person);
    
    stream.Seek(0, SeekOrigin.Begin);
    var xml = new StreamReader(stream).ReadToEnd();
    
    Console.WriteLine("\n  Resulting XML:");
    var lines = xml.Split('\n');
    foreach (var line in lines)
    {
        if (!string.IsNullOrWhiteSpace(line))
            Console.WriteLine($"    {line}");
    }
    
    Console.WriteLine($"\n  Size: {Encoding.UTF8.GetByteCount(xml)} bytes");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Attributes Control ===");
    
    var employee = new Employee
    {
        Id = 1,
        Name = "Bob",
        Salary = 75000,
        Password = "secret123"  // Should be ignored
    };
    
    Console.WriteLine("  Object with control attributes:");
    Console.WriteLine("    [XmlAttribute] Id = 1");
    Console.WriteLine("    [XmlElement] Name = \"Bob\"");
    Console.WriteLine("    [XmlIgnore] Password = \"secret123\"");
    
    var serializer = new XmlSerializer(typeof(Employee));
    using var stream = new MemoryStream();
    serializer.Serialize(stream, employee);
    
    stream.Seek(0, SeekOrigin.Begin);
    var xml = new StreamReader(stream).ReadToEnd();
    
    Console.WriteLine("\n  Resulting XML:");
    var lines = xml.Split('\n');
    foreach (var line in lines)
    {
        if (!string.IsNullOrWhiteSpace(line))
            Console.WriteLine($"    {line}");
    }
    
    Console.WriteLine("\n  Note: Password field is NOT in XML ([XmlIgnore])");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Collections (XmlArray) ===");
    
    var team = new Team
    {
        Name = "Engineers",
        Members = new()
        {
            new Person { Name = "Alice", Age = 30 },
            new Person { Name = "Bob", Age = 35 }
        }
    };
    
    Console.WriteLine("  Team with members:");
    Console.WriteLine($"    Name: {team.Name}");
    Console.WriteLine($"    Members: {string.Join(", ", team.Members.Select(m => m.Name))}");
    
    var serializer = new XmlSerializer(typeof(Team));
    using var stream = new MemoryStream();
    serializer.Serialize(stream, team);
    
    stream.Seek(0, SeekOrigin.Begin);
    var xml = new StreamReader(stream).ReadToEnd();
    
    Console.WriteLine("\n  Resulting XML:");
    var lines = xml.Split('\n');
    foreach (var line in lines.Take(20))
    {
        if (!string.IsNullOrWhiteSpace(line))
            Console.WriteLine($"    {line}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Round-Trip Serialization ===");
    
    var original = new Person { Name = "Charlie", Age = 25 };
    
    Console.WriteLine("  Original object:");
    Console.WriteLine($"    Name: {original.Name}, Age: {original.Age}");
    
    // Serialize
    var serializer = new XmlSerializer(typeof(Person));
    using var stream = new MemoryStream();
    serializer.Serialize(stream, original);
    
    // Deserialize
    stream.Seek(0, SeekOrigin.Begin);
    var restored = (Person)serializer.Deserialize(stream)!;
    
    Console.WriteLine("\n  After round-trip:");
    Console.WriteLine($"    Name: {restored.Name}, Age: {restored.Age}");
    
    bool matches = original.Name == restored.Name && original.Age == restored.Age;
    Console.WriteLine($"\n  Round-trip successful: {matches}");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   XML SERIALIZATION                                                ║");
    Console.WriteLine("║   XmlSerializer, Attributes, Collections                          ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ XML Serialization Examples Completed                           ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

[XmlRoot("Worker")]
public class Employee
{
    [XmlAttribute("id")]
    public int Id { get; set; }
    
    [XmlElement("FullName")]
    public string Name { get; set; } = string.Empty;
    
    public decimal Salary { get; set; }
    
    [XmlIgnore]
    public string Password { get; set; } = string.Empty;
}

public class Team
{
    public string Name { get; set; } = string.Empty;
    
    [XmlArray("Members")]
    [XmlArrayItem("Member")]
    public List<Person> Members { get; set; } = new();
}
