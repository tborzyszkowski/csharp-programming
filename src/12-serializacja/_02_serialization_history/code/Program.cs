using System;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: BinaryFormatter (Legacy)
RunExample1();

// Example 2: XmlSerializer (2003)
RunExample2();

// Example 3: WCF DataContract style
RunExample3();

// Example 4: System.Text.Json (Modern)
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: BinaryFormatter (Legacy - .NET 2002) ===");
    Console.WriteLine("  [!] Note: BinaryFormatter is DEPRECATED in .NET 5+");
    Console.WriteLine("  [!] Reason: Security vulnerabilities (RCE risks)");
    
    Console.WriteLine("\n  How it worked (2002-2020):");
    Console.WriteLine("    var bf = new BinaryFormatter();");
    Console.WriteLine("    bf.Serialize(stream, person);");
    
    Console.WriteLine("\n  Migration: Use System.Text.Json instead");
    Console.WriteLine("    var json = JsonSerializer.Serialize(person);");
    
    Console.WriteLine("\n  Timeline:");
    Console.WriteLine("    2002: Introduced (BinaryFormatter pioneering)");
    Console.WriteLine("    2020: Deprecated in .NET 5");
    Console.WriteLine("    2021: Disabled by default");
    Console.WriteLine("    2024: Removed from .NET 9");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: XmlSerializer (2003) ===");
    
    var person = new Person { Name = "Alice", Age = 30 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Person {{ Name = \"{person.Name}\", Age = {person.Age} }}");
    
    Console.WriteLine("\n  XmlSerializer approach:");
    Console.WriteLine("    var xs = new XmlSerializer(typeof(Person));");
    Console.WriteLine("    xs.Serialize(stream, person);");
    
    Console.WriteLine("\n  Resulting XML:");
    var xs = new XmlSerializer(typeof(Person));
    using var ms = new MemoryStream();
    xs.Serialize(ms, person);
    ms.Seek(0, SeekOrigin.Begin);
    var xml = new StreamReader(ms).ReadToEnd();
    
    var lines = xml.Split('\n');
    foreach (var line in lines.Take(10))
    {
        if (!string.IsNullOrWhiteSpace(line))
            Console.WriteLine($"    {line}");
    }
    
    Console.WriteLine("\n  Characteristics:");
    Console.WriteLine("    ✅ Human readable");
    Console.WriteLine("    ✅ Standard XML format");
    Console.WriteLine("    ❌ Large size (verbose tags)");
    Console.WriteLine("    ❌ Slow serialization/deserialization");
    Console.WriteLine("    ⚠️ Reflection overhead");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: WCF DataContract Style (2006) ===");
    
    var employee = new Employee { Name = "Bob", Salary = 75000 };
    
    Console.WriteLine("  WCF Approach (with [DataContract]):");
    Console.WriteLine("    [DataContract]");
    Console.WriteLine("    public class Employee");
    Console.WriteLine("    {");
    Console.WriteLine("        [DataMember]");
    Console.WriteLine("        public string Name { get; set; }");
    Console.WriteLine("        ");
    Console.WriteLine("        [DataMember]");
    Console.WriteLine("        public decimal Salary { get; set; }");
    Console.WriteLine("        ");
    Console.WriteLine("        [IgnoreDataMember]");
    Console.WriteLine("        public string Password { get; set; }");
    Console.WriteLine("    }");
    
    Console.WriteLine("\n  Benefits of WCF approach:");
    Console.WriteLine("    ✅ Explicit serialization control");
    Console.WriteLine("    ✅ [IgnoreDataMember] for sensitive data");
    Console.WriteLine("    ✅ Version-aware (KnownType)");
    Console.WriteLine("    ⚠️ But: Configuration complex");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: System.Text.Json (Modern - 2019+) ===");
    
    var person = new PersonModern { Name = "Charlie", Age = 35 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    PersonModern {{ Name = \"{person.Name}\", Age = {person.Age} }}");
    
    Console.WriteLine("\n  System.Text.Json (2019+):");
    var json = JsonSerializer.Serialize(person);
    Console.WriteLine($"    {json}");
    
    Console.WriteLine("\n  Characteristics:");
    Console.WriteLine("    ✅ Built-in (no NuGet needed)");
    Console.WriteLine("    ✅ Fast (optimized)");
    Console.WriteLine("    ✅ Source Generators ready (.NET 5+)");
    Console.WriteLine("    ✅ Streaming support");
    Console.WriteLine("    ✅ Async support");
    
    Console.WriteLine("\n  Comparison with history:");
    Console.WriteLine("    2002 BinaryFormatter: Fast but unsafe");
    Console.WriteLine("    2003 XmlSerializer: Safe but slow & verbose");
    Console.WriteLine("    2006 WCF: Standardized but complex");
    Console.WriteLine("    2019 System.Text.Json: Best of both (fast + safe)");
    Console.WriteLine("    2020+ Source Generators: Zero reflection overhead");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   SERIALIZATION HISTORY                                            ║");
    Console.WriteLine("║   .NET Evolution from BinaryFormatter to System.Text.Json         ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ History Review Complete                                        ║");
    Console.WriteLine("║   Key Lesson: BinaryFormatter → XmlSerializer → Json → Generators ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class PersonModern
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("age")]
    public int Age { get; set; }
}

public class Employee
{
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Password { get; set; } = string.Empty; // Should be [IgnoreDataMember]
}
