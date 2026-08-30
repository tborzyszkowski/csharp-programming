using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Polymorphism Problem
RunExample1();

// Example 2: Type Discriminator Solution
RunExample2();

// Example 3: Versioning Challenge
RunExample3();

// Example 4: Backward Compatibility
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Polymorphism Challenge ===");
    
    Console.WriteLine("  Problem: Serialize List<Animal>");
    Console.WriteLine("    Animal (base)");
    Console.WriteLine("    └─ Dog : Animal");
    Console.WriteLine("    └─ Cat : Animal");
    
    Console.WriteLine("\n  Issue: How to know which type when deserializing?");
    Console.WriteLine("    JSON: {\"Name\":\"Buddy\",\"Breed\":\"Labrador\"}");
    Console.WriteLine("    Is this Dog or something else?");
    
    Console.WriteLine("\n  Solution: Add type discriminator");
    Console.WriteLine("    {\"$type\":\"dog\",\"Name\":\"Buddy\",\"Breed\":\"Labrador\"}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Type Discriminator ===");
    
    var animals = new Animal[]
    {
        new Dog { Name = "Buddy", Breed = "Labrador" },
        new Cat { Name = "Whiskers", Color = "Orange" }
    };
    
    Console.WriteLine("  Objects to serialize:");
    foreach (var animal in animals)
    {
        Console.WriteLine($"    {animal.GetType().Name}: {animal.Name}");
    }
    
    var options = new JsonSerializerOptions { WriteIndented = true };
    var json = JsonSerializer.Serialize(animals, options);
    
    Console.WriteLine($"\n  Serialized with type info:");
    foreach (var line in json.Split('\n').Take(15))
    {
        Console.WriteLine($"    {line}");
    }
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Versioning ===");
    
    Console.WriteLine("  Evolution timeline:");
    Console.WriteLine("    V1: {\"Name\":\"Alice\"}");
    Console.WriteLine("    V2: {\"Name\":\"Alice\",\"Email\":\"alice@example.com\"}");
    Console.WriteLine("    V3: {\"FirstName\":\"Alice\",\"Email\":\"alice@example.com\"}");
    
    Console.WriteLine("\n  Challenge: Read V1/V2 data into V3");
    Console.WriteLine("    V1 → Rename Name to FirstName ✓");
    Console.WriteLine("    V2 → Keep Email, rename Name ✓");
    Console.WriteLine("    V3 → Already correct format ✓");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Backward Compatibility ===");
    
    // V1 JSON (old client)
    var v1Json = "{\"Name\":\"Alice\",\"Email\":\"alice@example.com\"}";
    
    // V2 class (new server)
    var person = JsonSerializer.Deserialize<PersonV2>(v1Json);
    
    Console.WriteLine("  V1 JSON (old format):");
    Console.WriteLine($"    {v1Json}");
    
    Console.WriteLine($"\n  Deserialized into V2 class:");
    Console.WriteLine($"    Name: {person!.Name}");
    Console.WriteLine($"    Email: {person.Email}");
    Console.WriteLine($"    Phone: {person.Phone ?? "(not provided)"}");
    
    Console.WriteLine("\n  Graceful degradation: Missing Phone field defaults to null ✓");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ADVANCED FEATURES                                                ║");
    Console.WriteLine("║   Polymorphism, Versioning, Backward Compatibility                ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Advanced Features Examples Completed                           ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

[JsonDerivedType(typeof(Dog), "dog")]
[JsonDerivedType(typeof(Cat), "cat")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
public abstract class Animal
{
    public string Name { get; set; } = string.Empty;
}

public class Dog : Animal
{
    public string Breed { get; set; } = string.Empty;
}

public class Cat : Animal
{
    public string Color { get; set; } = string.Empty;
}

public class PersonV2
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }  // V2 addition - optional
}
