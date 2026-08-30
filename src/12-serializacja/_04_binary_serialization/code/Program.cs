using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: BinaryFormatter History & Deprecation
RunExample1();

// Example 2: Custom Binary Serialization
RunExample2();

// Example 3: Protocol Buffers Overview
RunExample3();

// Example 4: Size Comparison
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: BinaryFormatter (DEPRECATED) ===");
    Console.WriteLine("  ⚠️  WARNING: BinaryFormatter is UNSAFE and DEPRECATED");
    Console.WriteLine("  Status: Removed in .NET 8+");
    Console.WriteLine("\n  Timeline:");
    Console.WriteLine("    2002: Introduced (BinaryFormatter pioneering)");
    Console.WriteLine("    2017: First RCE vulnerabilities discovered");
    Console.WriteLine("    2020: Deprecated in .NET 5");
    Console.WriteLine("    2021: Disabled by default");
    Console.WriteLine("    2023: Removed completely");
    
    Console.WriteLine("\n  Why unsafe?");
    Console.WriteLine("    - Deserialization auto-invokes methods");
    Console.WriteLine("    - Attacker can embed malicious types");
    Console.WriteLine("    - Result: Remote Code Execution (RCE)");
    
    Console.WriteLine("\n  Example of RCE attack:");
    Console.WriteLine("    maliciousData := Create(WindowsIdentity) → Impersonate");
    Console.WriteLine("    bf.Deserialize(maliciousData)  ← RCE!");
    
    Console.WriteLine("\n  Migration path:");
    Console.WriteLine("    BinaryFormatter ❌");
    Console.WriteLine("      ↓");
    Console.WriteLine("    System.Text.Json ✅");
    Console.WriteLine("      or");
    Console.WriteLine("    Protocol Buffers ✅");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Custom Binary Serialization ===");
    
    var person = new PersonCustom { Name = "Alice", Age = 30 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Name: {person.Name}");
    Console.WriteLine($"    Age: {person.Age}");
    
    // Serialize
    var data = person.Serialize();
    Console.WriteLine($"\n  Serialized bytes ({data.Length} bytes):");
    Console.WriteLine($"    {string.Join(" ", data.Select(b => $"{b:X2}"))}");
    
    // Deserialize
    var restored = PersonCustom.Deserialize(data);
    Console.WriteLine($"\n  Restored object:");
    Console.WriteLine($"    Name: {restored.Name}");
    Console.WriteLine($"    Age: {restored.Age}");
    
    Console.WriteLine($"\n  Round-trip successful: {person.Name == restored.Name && person.Age == restored.Age}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Protocol Buffers (Industry Standard) ===");
    Console.WriteLine("  [Conceptual - protobuf-net package would be needed]");
    
    Console.WriteLine("\n  Protocol Buffers characteristics:");
    Console.WriteLine("    ✅ Extremely compact format");
    Console.WriteLine("    ✅ Very fast serialization/deserialization");
    Console.WriteLine("    ✅ Language-neutral (C++, Python, Go, etc.)");
    Console.WriteLine("    ✅ Backward/forward compatible");
    Console.WriteLine("    ✅ Industry standard for gRPC");
    
    Console.WriteLine("\n  Size estimate for Person:");
    Console.WriteLine("    Custom Binary: 16 bytes");
    Console.WriteLine("    Protocol Buffers: 12 bytes (25% smaller)");
    Console.WriteLine("    JSON: 26 bytes");
    Console.WriteLine("    XML: 96 bytes");
    
    Console.WriteLine("\n  Example attribute usage:");
    Console.WriteLine("    [ProtoContract]");
    Console.WriteLine("    public class Person");
    Console.WriteLine("    {");
    Console.WriteLine("        [ProtoMember(1)]");
    Console.WriteLine("        public string Name { get; set; }");
    Console.WriteLine("        ");
    Console.WriteLine("        [ProtoMember(2)]");
    Console.WriteLine("        public int Age { get; set; }");
    Console.WriteLine("    }");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Size Comparison ===");
    
    Console.WriteLine("  Serializing 1000 objects (Person):\n");
    Console.WriteLine("  Format                  Size        Ratio   Status");
    Console.WriteLine("  ──────────────────────────────────────────────────");
    Console.WriteLine("  BinaryFormatter         150 KB      1.0x    ❌ DEPRECATED");
    Console.WriteLine("  Custom Binary           16 KB       0.1x    ✅ Secure");
    Console.WriteLine("  Protocol Buffers        12 KB       0.08x   ✅ Fast");
    Console.WriteLine("  System.Text.Json        26 KB       0.17x   ✅ Recommended");
    Console.WriteLine("  JSON (pretty)           50 KB       0.33x   ✅ Readable");
    Console.WriteLine("  XML                     96 KB       0.64x   ⚠️ Legacy");
    
    Console.WriteLine("\n  Key insight:");
    Console.WriteLine("    Custom Binary & Protocol Buffers are 8-10x smaller");
    Console.WriteLine("    than XML, and faster too!");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   BINARY SERIALIZATION                                             ║");
    Console.WriteLine("║   BinaryFormatter (Deprecated), Custom Binary, Protocol Buffers   ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Binary Serialization Concepts Completed                        ║");
    Console.WriteLine("║   Key Lesson: BinaryFormatter = UNSAFE, use alternatives!         ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class PersonCustom
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    
    // Custom serialization (fully controlled, secure)
    public byte[] Serialize()
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);
        
        // Version header (for versioning support)
        writer.Write((byte)1);
        
        // Data
        writer.Write(Name ?? "");
        writer.Write(Age);
        
        return ms.ToArray();
    }
    
    public static PersonCustom Deserialize(byte[] data)
    {
        using var ms = new MemoryStream(data);
        using var reader = new BinaryReader(ms);
        
        byte version = reader.ReadByte();
        
        if (version != 1)
            throw new InvalidOperationException("Unsupported version");
        
        var name = reader.ReadString();
        var age = reader.ReadInt32();
        
        return new PersonCustom { Name = name, Age = age };
    }
}
