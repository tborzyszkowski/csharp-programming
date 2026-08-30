using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Performance Benchmarks
RunExample1();

// Example 2: BinaryFormatter Vulnerability
RunExample2();

// Example 3: XXE Security
RunExample3();

// Example 4: Best Practices
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Performance Benchmarks ===");
    
    Console.WriteLine("  Serialization speed comparison (10K objects):");
    Console.WriteLine("\n  Format          Size (KB)   Time (ms)   Speed");
    Console.WriteLine("  ───────────────────────────────────────────────");
    Console.WriteLine("  JSON            350         35         1.0x");
    Console.WriteLine("  Binary (custom) 100         12         2.9x");
    Console.WriteLine("  Protobuf        80          8          4.4x");
    Console.WriteLine("  Protobuf (AOT)  80          5          7.0x");
    
    Console.WriteLine("\n  Key insight:");
    Console.WriteLine("    Source Generators = 7x faster than reflection!");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: BinaryFormatter Vulnerability ===");
    
    Console.WriteLine("  BinaryFormatter = CRITICAL SECURITY RISK");
    Console.WriteLine("\n  Timeline:");
    Console.WriteLine("    2020: Deprecated in .NET 5");
    Console.WriteLine("    2021: Disabled by default");
    Console.WriteLine("    2023: Removed in .NET 8+");
    
    Console.WriteLine("\n  Why so dangerous?");
    Console.WriteLine("    ✗ Deserializes arbitrary object graphs");
    Console.WriteLine("    ✗ Invokes constructors during deserialization");
    Console.WriteLine("    ✗ Gadget chain attacks possible");
    Console.WriteLine("    ✗ Remote Code Execution (RCE)");
    
    Console.WriteLine("\n  Attack scenario:");
    Console.WriteLine("    1. Attacker sends malicious binary data");
    Console.WriteLine("    2. BinaryFormatter.Deserialize() called");
    Console.WriteLine("    3. Gadget chain triggered");
    Console.WriteLine("    4. Attacker code executes ← RCE!");
    
    Console.WriteLine("\n  Migration:");
    Console.WriteLine("    ✓ Replace with System.Text.Json");
    Console.WriteLine("    ✓ Type-safe deserialization");
    Console.WriteLine("    ✓ No gadget chain attacks");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: XXE (XML External Entity) Injection ===");
    
    Console.WriteLine("  Vulnerable pattern:");
    Console.WriteLine("    var doc = new XmlDocument();");
    Console.WriteLine("    doc.LoadXml(userInput);  // ← User-controlled!");
    
    Console.WriteLine("\n  Attack payload:");
    Console.WriteLine("    <!DOCTYPE foo [");
    Console.WriteLine("      <!ENTITY xxe SYSTEM \"file:///etc/passwd\">");
    Console.WriteLine("    ]>");
    Console.WriteLine("    <root>&xxe;</root>");
    
    Console.WriteLine("\n  Impact:");
    Console.WriteLine("    ✗ File system access");
    Console.WriteLine("    ✗ SSRF attacks");
    Console.WriteLine("    ✗ DoS (billion laughs attack)");
    
    Console.WriteLine("\n  Safe approach:");
    Console.WriteLine("    var settings = new XmlReaderSettings");
    Console.WriteLine("    {");
    Console.WriteLine("        DtdProcessing = DtdProcessing.Prohibit,");
    Console.WriteLine("        XmlResolver = null");
    Console.WriteLine("    };");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Best Practices ===");
    
    Console.WriteLine("  Security checklist:");
    Console.WriteLine("    ✓ Never use BinaryFormatter");
    Console.WriteLine("    ✓ Validate deserialized data");
    Console.WriteLine("    ✓ Use [JsonIgnore] for sensitive fields");
    Console.WriteLine("    ✓ Type-safe deserialization only");
    Console.WriteLine("    ✓ Prevent XXE attacks");
    
    Console.WriteLine("\n  Performance checklist:");
    Console.WriteLine("    ✓ Use Source Generators (7x faster)");
    Console.WriteLine("    ✓ Stream large files");
    Console.WriteLine("    ✓ Enable Native AOT");
    Console.WriteLine("    ✓ Profile with real data");
    Console.WriteLine("    ✓ Monitor in production");
    
    Console.WriteLine("\n  Format selection:");
    
    var person = new Person { Name = "Alice", Age = 30 };
    var json = JsonSerializer.Serialize(person);
    
    Console.WriteLine("    Use JSON for: REST APIs, config files");
    Console.WriteLine("    Use Protobuf for: gRPC, microservices");
    Console.WriteLine("    Use XML for: Legacy systems");
    Console.WriteLine("    Avoid BinaryFormatter always!");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   PERFORMANCE & SECURITY                                           ║");
    Console.WriteLine("║   Benchmarks, Vulnerabilities, Best Practices                      ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Serialization Module Complete (10/10 Topics)                  ║");
    Console.WriteLine("║   ✓ All Performance & Security Examples Done                     ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}
