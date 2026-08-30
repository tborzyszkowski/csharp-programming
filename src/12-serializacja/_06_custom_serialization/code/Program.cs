using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Text;
using System.Globalization;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: JSON Custom Converter
RunExample1();

// Example 2: IXmlSerializable Interface
RunExample2();

// Example 3: Validation During Deserialization
RunExample3();

// Example 4: Surrogate Pattern
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Custom JSON Converter ===");
    
    var person = new PersonWithDate { Name = "Alice", BirthDate = new DateTime(1990, 5, 15) };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Name: {person.Name}");
    Console.WriteLine($"    BirthDate: {person.BirthDate:yyyy-MM-dd}");
    
    var options = new JsonSerializerOptions
    {
        Converters = { new DateTimeConverter() }
    };
    
    var json = JsonSerializer.Serialize(person, options);
    Console.WriteLine($"\n  Serialized JSON (custom date format):");
    Console.WriteLine($"    {json}");
    
    var restored = JsonSerializer.Deserialize<PersonWithDate>(json, options)!;
    Console.WriteLine($"\n  Restored object:");
    Console.WriteLine($"    Name: {restored.Name}");
    Console.WriteLine($"    BirthDate: {restored.BirthDate:yyyy-MM-dd}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: IXmlSerializable Interface ===");
    
    var person = new PersonXmlCustom { Name = "Bob", Age = 35 };
    
    Console.WriteLine("  Object to serialize:");
    Console.WriteLine($"    Name: {person.Name}, Age: {person.Age}");
    
    var xs = new XmlSerializer(typeof(PersonXmlCustom));
    using var stream = new MemoryStream();
    xs.Serialize(stream, person);
    
    stream.Seek(0, SeekOrigin.Begin);
    var xml = new StreamReader(stream).ReadToEnd();
    
    Console.WriteLine($"\n  Custom XML output (attributes only):");
    var lines = xml.Split('\n');
    foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
    {
        Console.WriteLine($"    {line}");
    }
    
    stream.Seek(0, SeekOrigin.Begin);
    var restored = (PersonXmlCustom)xs.Deserialize(stream)!;
    Console.WriteLine($"\n  Restored object:");
    Console.WriteLine($"    Name: {restored.Name}, Age: {restored.Age}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Validation During Deserialization ===");
    
    Console.WriteLine("  Test Case 1: Valid Age");
    try
    {
        var validJson = "{\"name\":\"Alice\",\"age\":30}";
        var person = JsonSerializer.Deserialize<PersonWithValidation>(validJson);
        Console.WriteLine($"    ✅ Deserialized: Name={person!.Name}, Age={person.Age}");
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"    ❌ Error: {ex.Message}");
    }
    
    Console.WriteLine("\n  Test Case 2: Invalid Age (too high)");
    try
    {
        var invalidJson = "{\"name\":\"Bob\",\"age\":200}";
        var options = new JsonSerializerOptions
        {
            Converters = { new AgeValidator() }
        };
        var person = JsonSerializer.Deserialize<PersonWithValidation>(invalidJson, options);
        Console.WriteLine($"    ✅ Deserialized: Name={person!.Name}, Age={person.Age}");
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"    ❌ Validation error: {ex.Message}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Surrogate Pattern ===");
    
    var original = new LegacyPerson { Name = "Charlie", Age = 25 };
    
    Console.WriteLine("  Original object (legacy):");
    Console.WriteLine($"    Name: {original.Name}, Age: {original.Age}");
    
    // Serialize using surrogate
    var surrogate = LegacyPersonSurrogate.FromPerson(original);
    var json = JsonSerializer.Serialize(surrogate);
    
    Console.WriteLine($"\n  Serialized via surrogate:");
    Console.WriteLine($"    {json}");
    
    // Deserialize back through surrogate
    var deserializedSurrogate = JsonSerializer.Deserialize<LegacyPersonSurrogate>(json)!;
    var restored = deserializedSurrogate.ToPerson();
    
    Console.WriteLine($"\n  Restored via surrogate:");
    Console.WriteLine($"    Name: {restored.Name}, Age: {restored.Age}");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   CUSTOM SERIALIZATION                                             ║");
    Console.WriteLine("║   ISerializable, Converters, Advanced Patterns                    ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Custom Serialization Examples Completed                        ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class PersonWithDate
{
    public string Name { get; set; } = string.Empty;
    
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime BirthDate { get; set; }
}

public class DateTimeConverter : JsonConverter<DateTime>
{
    private const string Format = "yyyy-MM-dd";
    
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dateStr = reader.GetString();
        return DateTime.ParseExact(dateStr, Format, CultureInfo.InvariantCulture);
    }
    
    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

public class PersonXmlCustom : IXmlSerializable
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    
    public XmlSchema? GetSchema() => null;
    
    public void ReadXml(XmlReader reader)
    {
        Name = reader.GetAttribute("name") ?? "";
        Age = int.Parse(reader.GetAttribute("age") ?? "0");
    }
    
    public void WriteXml(XmlWriter writer)
    {
        writer.WriteAttributeString("name", Name);
        writer.WriteAttributeString("age", Age.ToString());
    }
}

public class PersonWithValidation
{
    public string Name { get; set; } = string.Empty;
    
    [JsonConverter(typeof(AgeValidator))]
    public int Age { get; set; }
}

public class AgeValidator : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetInt32();
        if (value < 0 || value > 150)
            throw new JsonException($"Age must be 0-150, got {value}");
        return value;
    }
    
    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        if (value < 0 || value > 150)
            throw new ArgumentException($"Age must be 0-150");
        writer.WriteNumberValue(value);
    }
}

public class LegacyPerson
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class LegacyPersonSurrogate
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    
    public static LegacyPersonSurrogate FromPerson(LegacyPerson person)
        => new() { Name = person.Name, Age = person.Age };
    
    public LegacyPerson ToPerson()
        => new() { Name = Name, Age = Age };
}
