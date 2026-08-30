using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Basic Custom Attribute
RunExample1();

// Example 2: AttributeUsage & AttributeTargets
RunExample2();

// Example 3: Named Parameters
RunExample3();

// Example 4: Validation Attribute
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic Custom Attribute ===");
    
    var type = typeof(LibraryClass);
    
    // Get attributes
    var attrs = type.GetCustomAttributes<LibraryAttribute>().ToArray();
    
    Console.WriteLine($"  Type: {type.Name}");
    Console.WriteLine($"  Attributes: {attrs.Length}\n");
    
    foreach (var attr in attrs)
    {
        Console.WriteLine($"    ✓ Author: {attr.Author}");
        Console.WriteLine($"    ✓ Version: {attr.Version}");
        Console.WriteLine($"    ✓ Date: {attr.CreatedDate}");
    }
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: AttributeUsage & AttributeTargets ===");
    
    var types = new[] 
    { 
        typeof(DocumentedClass),
        typeof(DocumentedProperty),
        typeof(DocumentedMethod)
    };
    
    Console.WriteLine("  Targets:\n");
    
    foreach (var type in types)
    {
        var attrs = type.GetCustomAttributes<DocumentedAttribute>();
        
        foreach (var attr in attrs)
        {
            Console.WriteLine($"    ✓ {type.Name}: {attr.Description}");
        }
    }
    
    // Check properties
    var prop = typeof(DocumentedProperty).GetProperty("Value");
    if (prop != null)
    {
        var propAttr = prop.GetCustomAttribute<DocumentedAttribute>();
        if (propAttr != null)
            Console.WriteLine($"    ✓ Property Value: {propAttr.Description}");
    }
    
    // Check methods
    var method = typeof(DocumentedMethod).GetMethod("DoWork");
    if (method != null)
    {
        var methodAttr = method.GetCustomAttribute<DocumentedAttribute>();
        if (methodAttr != null)
            Console.WriteLine($"    ✓ Method DoWork: {methodAttr.Description}");
    }
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Named Parameters ===");
    
    var type = typeof(AdvancedDocumentation);
    var attr = type.GetCustomAttribute<AdvancedDocumentationAttribute>();
    
    if (attr != null)
    {
        Console.WriteLine($"  Class: {type.Name}\n");
        Console.WriteLine($"    Summary: {attr.Summary}");
        Console.WriteLine($"    Author: {attr.Author}");
        Console.WriteLine($"    Version: {attr.Version}");
        Console.WriteLine($"    Status: {attr.Status}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Validation Attributes ===");
    
    var person = new Person 
    { 
        Name = "Alice",
        Email = "alice@invalid",
        Age = 25
    };
    
    Console.WriteLine($"  Validating: {person.Name}\n");
    
    var validator = new DataValidator();
    bool isValid = validator.Validate(person, out var errors);
    
    if (isValid)
    {
        Console.WriteLine("    ✓ All validations passed!");
    }
    else
    {
        Console.WriteLine("    ✗ Validation errors:\n");
        foreach (var error in errors)
        {
            Console.WriteLine($"      ✗ {error}");
        }
    }
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   CUSTOM ATTRIBUTES                                                ║");
    Console.WriteLine("║   AttributeUsage, AttributeTargets, Validation Patterns            ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Custom Attributes Examples Completed                           ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// ATTRIBUTE DEFINITIONS
// ============================================================================

[AttributeUsage(AttributeTargets.Class)]
public class LibraryAttribute : Attribute
{
    public LibraryAttribute(string author)
    {
        Author = author;
    }
    
    public string Author { get; }
    public string Version { get; set; } = "1.0";
    public string CreatedDate { get; set; } = "2024";
}

[AttributeUsage(
    AttributeTargets.Class | 
    AttributeTargets.Method | 
    AttributeTargets.Property)]
public class DocumentedAttribute : Attribute
{
    public DocumentedAttribute(string description)
    {
        Description = description;
    }
    
    public string Description { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class AdvancedDocumentationAttribute : Attribute
{
    public AdvancedDocumentationAttribute(string summary)
    {
        Summary = summary;
    }
    
    public string Summary { get; }
    public string? Author { get; set; }
    public string? Version { get; set; }
    public string? Status { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
    public string Message { get; set; } = "Field is required";
}

[AttributeUsage(AttributeTargets.Property)]
public class EmailValidationAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public MaxLengthAttribute(int length) => Length = length;
    public int Length { get; }
}

// ============================================================================
// DOMAIN CLASSES
// ============================================================================

[Library("System Library", Version = "2.0", CreatedDate = "2024-01-01")]
public class LibraryClass { }

[Documented("This is a documented class")]
public class DocumentedClass { }

public class DocumentedProperty
{
    [Documented("This is a documented property")]
    public string Value { get; set; } = "";
}

public class DocumentedMethod
{
    [Documented("This is a documented method")]
    public void DoWork() { }
}

[AdvancedDocumentation("Main application class",
    Author = "Alice",
    Version = "3.0",
    Status = "Production")]
public class AdvancedDocumentation { }

// ============================================================================
// VALIDATION EXAMPLE
// ============================================================================

public class Person
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = "";
    
    [Required]
    [EmailValidation]
    public string Email { get; set; } = "";
    
    public int Age { get; set; }
}

public class DataValidator
{
    public bool Validate(object obj, out List<string> errors)
    {
        errors = new();
        var type = obj.GetType();
        
        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(obj);
            
            // Check [Required]
            if (property.GetCustomAttribute<RequiredAttribute>() != null)
            {
                if (string.IsNullOrEmpty(value?.ToString()))
                {
                    var msg = property.GetCustomAttribute<RequiredAttribute>()!.Message;
                    errors.Add($"{property.Name}: {msg}");
                }
            }
            
            // Check [MaxLength]
            var maxLen = property.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLen != null && value is string str)
            {
                if (str.Length > maxLen.Length)
                {
                    errors.Add($"{property.Name}: Exceeds {maxLen.Length} characters");
                }
            }
            
            // Check [EmailValidation]
            if (property.GetCustomAttribute<EmailValidationAttribute>() != null && value is string email)
            {
                if (!email.Contains("@"))
                {
                    errors.Add($"{property.Name}: Invalid email format");
                }
            }
        }
        
        return errors.Count == 0;
    }
}
