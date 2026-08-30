using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: [Obsolete]
RunExample1();

// Example 2: [Flags] Enum
RunExample2();

// Example 3: DataAnnotations Validation
RunExample3();

// Example 4: Built-in Attributes Overview
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: [Obsolete] ===");
    
    var type = typeof(LegacyClass);
    var method = type.GetMethod("OldMethod");
    
    if (method != null)
    {
        var obsolete = method.GetCustomAttribute<ObsoleteAttribute>();
        
        if (obsolete != null)
        {
            Console.WriteLine($"  Method: {method.Name}");
            Console.WriteLine($"    Status: DEPRECATED");
            Console.WriteLine($"    Message: {obsolete.Message}");
        }
    }
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: [Flags] Enum ===");
    
    var permsWithFlags = Permissions.Read | Permissions.Write;
    var permsWithoutFlags = RegularPerms.Read | RegularPerms.Write;
    
    Console.WriteLine("  With [Flags]:");
    Console.WriteLine($"    Value: {permsWithFlags}");
    
    Console.WriteLine("\n  Without [Flags]:");
    Console.WriteLine($"    Value: {permsWithoutFlags}");
    
    // Check attribute
    var hasFlags = typeof(Permissions).GetCustomAttribute<FlagsAttribute>() != null;
    Console.WriteLine($"\n  Has [Flags]: {hasFlags}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: DataAnnotations ===");
    
    var user = new ValidatedUser
    {
        Name = "Alice",
        Email = "alice@example.com",
        Age = 30
    };
    
    Console.WriteLine($"  User: {user.Name}\n");
    
    // Check attributes
    var emailProp = typeof(ValidatedUser).GetProperty("Email");
    var emailAttr = emailProp?.GetCustomAttribute<EmailAddressAttribute>();
    
    var ageProp = typeof(ValidatedUser).GetProperty("Age");
    var rangeAttr = ageProp?.GetCustomAttribute<RangeAttribute>();
    
    Console.WriteLine($"    Email has [EmailAddress]: {emailAttr != null}");
    Console.WriteLine($"    Age has [Range]: {rangeAttr != null}");
    
    if (rangeAttr != null)
        Console.WriteLine($"      Range: {rangeAttr.Minimum} - {rangeAttr.Maximum}");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Built-in Attributes Overview ===");
    
    var attrs = new[]
    {
        typeof(ObsoleteAttribute),
        typeof(SerializableAttribute),
        typeof(FlagsAttribute),
        typeof(RequiredAttribute),
        typeof(RangeAttribute)
    };
    
    Console.WriteLine("  Common Built-in Attributes:\n");
    
    foreach (var attr in attrs)
    {
        Console.WriteLine($"    - [{attr.Name.Replace("Attribute", "")}]");
    }
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   BUILT-IN .NET ATTRIBUTES                                         ║");
    Console.WriteLine("║   [Obsolete], [Flags], [Required], [Range], [EmailAddress]         ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Built-in Attributes Examples Completed                        ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// DOMAIN CLASSES
// ============================================================================

public class LegacyClass
{
    [Obsolete("Use NewMethod instead")]
    public void OldMethod() { }
}

[Flags]
public enum Permissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
    All = Read | Write | Delete
}

public enum RegularPerms
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4
}

public class ValidatedUser
{
    [Required]
    public string Name { get; set; } = "";
    
    [EmailAddress]
    public string Email { get; set; } = "";
    
    [Range(18, 120)]
    public int Age { get; set; }
}
