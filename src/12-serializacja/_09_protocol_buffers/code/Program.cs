using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Diagnostics;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Protobuf Concept
RunExample1();

// Example 2: Size Comparison
RunExample2();

// Example 3: Version Safety
RunExample3();

// Example 4: gRPC Concept
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Protocol Buffers Concept ===");
    
    Console.WriteLine("  Protocol Buffers = Binary serialization by Google");
    Console.WriteLine("\n  Key characteristics:");
    Console.WriteLine("    ✓ Compact (30% of JSON size)");
    Console.WriteLine("    ✓ Fast (5-10x faster than JSON)");
    Console.WriteLine("    ✓ Version-safe (forward/backward compatible)");
    Console.WriteLine("    ✓ Language-agnostic");
    Console.WriteLine("    ✓ Used in gRPC");
    
    Console.WriteLine("\n  .proto syntax:");
    Console.WriteLine("    message Person {");
    Console.WriteLine("      string name = 1;     // Field number (key to compatibility)");
    Console.WriteLine("      int32 age = 2;       // Never change numbers!");
    Console.WriteLine("      string email = 3;");
    Console.WriteLine("    }");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Size Comparison ===");
    
    var person = new { Name = "Alice", Age = 30, Email = "alice@example.com" };
    
    var json = JsonSerializer.Serialize(person);
    Console.WriteLine($"  JSON format:");
    Console.WriteLine($"    {json}");
    Console.WriteLine($"    Size: {json.Length} bytes");
    
    Console.WriteLine($"\n  Protobuf format:");
    Console.WriteLine($"    (binary data - not human readable)");
    Console.WriteLine($"    Estimated size: {json.Length / 3}-{json.Length / 2} bytes");
    Console.WriteLine($"    Savings: ~60-70% smaller than JSON");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Version Safety ===");
    
    Console.WriteLine("  Schema evolution with field numbers:");
    Console.WriteLine("\n  V1 schema:");
    Console.WriteLine("    message Person {");
    Console.WriteLine("      string name = 1;");
    Console.WriteLine("      int32 age = 2;");
    Console.WriteLine("    }");
    
    Console.WriteLine("\n  V2 schema (added field):");
    Console.WriteLine("    message Person {");
    Console.WriteLine("      string name = 1;     // ← Never changed");
    Console.WriteLine("      int32 age = 2;       // ← Never changed");
    Console.WriteLine("      string email = 3;    // ← New field");
    Console.WriteLine("    }");
    
    Console.WriteLine("\n  Compatibility:");
    Console.WriteLine("    ✓ V1 client → V2 server: Works! (email defaults)");
    Console.WriteLine("    ✓ V2 client → V1 server: Works! (email ignored)");
    Console.WriteLine("    ✓ Field numbers are IMMUTABLE");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: gRPC Overview ===");
    
    Console.WriteLine("  gRPC = Google RPC Framework");
    Console.WriteLine("    Uses Protocol Buffers for serialization");
    Console.WriteLine("    HTTP/2 for transport");
    Console.WriteLine("    Bidirectional streaming");
    Console.WriteLine("\n  Service definition:");
    Console.WriteLine("    service GreeterService {");
    Console.WriteLine("      rpc SayHello (HelloRequest) returns (HelloReply) {}");
    Console.WriteLine("    }");
    
    Console.WriteLine("\n  Benefits over REST:");
    Console.WriteLine("    ✓ ~10x faster");
    Console.WriteLine("    ✓ Smaller payloads");
    Console.WriteLine("    ✓ Streaming support");
    Console.WriteLine("    ✓ Strongly typed contracts");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   PROTOCOL BUFFERS                                                 ║");
    Console.WriteLine("║   Industry Standard Binary Format, gRPC, Performance               ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Protocol Buffers Examples Completed                            ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}
