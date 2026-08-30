using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: GetCustomAttribute vs GetCustomAttributes
RunExample1();

// Example 2: Check Attribute Existence
RunExample2();

// Example 3: Read Properties & Methods Attributes
RunExample3();

// Example 4: Attribute Filtering with LINQ
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: GetCustomAttribute vs GetCustomAttributes ===");
    
    Console.WriteLine("  Single Attribute:");
    var author = typeof(SingleAttrClass).GetCustomAttribute<AuthorAttr>();
    Console.WriteLine($"    Author: {author?.Name}");
    
    Console.WriteLine("\n  Multiple Attributes:");
    var tags = typeof(MultiAttrClass).GetCustomAttributes<TagAttr>().ToArray();
    Console.WriteLine($"    Tag Count: {tags.Length}");
    foreach (var tag in tags.Take(2))
        Console.WriteLine($"      - {tag.Name}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Check Attribute Existence ===");
    
    var type = typeof(DocumentedClass);
    
    // Method 1: GetCustomAttribute != null
    var has1 = type.GetCustomAttribute<DocAttr>() != null;
    Console.WriteLine($"  Has [Doc] (method 1): {has1}");
    
    // Method 2: IsDefined
    var has2 = type.IsDefined(typeof(DocAttr));
    Console.WriteLine($"  Has [Doc] (method 2): {has2}");
    
    // Check property
    var prop = typeof(DocumentedClass).GetProperty("Value");
    if (prop != null)
    {
        var hasProp = prop.IsDefined(typeof(DocAttr));
        Console.WriteLine($"  Property 'Value' has [Doc]: {hasProp}");
    }
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Read Properties & Methods ===");
    
    var type = typeof(AnnotatedClass);
    
    Console.WriteLine("  Properties:");
    foreach (var prop in type.GetProperties())
    {
        var doc = prop.GetCustomAttribute<DocAttr>();
        if (doc != null)
            Console.WriteLine($"    - {prop.Name}: {doc.Description}");
    }
    
    Console.WriteLine("\n  Methods:");
    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    foreach (var method in methods)
    {
        var doc = method.GetCustomAttribute<DocAttr>();
        if (doc != null)
            Console.WriteLine($"    - {method.Name}(): {doc.Description}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: LINQ Filtering ===");
    
    var types = new[]
    {
        typeof(AuthorClass),
        typeof(ObsoleteClass),
        typeof(RegularClass)
    };
    
    Console.WriteLine("  Filtering results:\n");
    
    var withAuthor = types
        .Where(t => t.GetCustomAttribute<AuthorAttr>() != null)
        .ToList();
    
    var withObsolete = types
        .Where(t => t.GetCustomAttribute<ObsoleteAttribute>() != null)
        .ToList();
    
    Console.WriteLine($"    Types with [Author]: {withAuthor.Count}");
    foreach (var t in withAuthor)
        Console.WriteLine($"      - {t.Name}");
    
    Console.WriteLine($"\n    Types with [Obsolete]: {withObsolete.Count}");
    foreach (var t in withObsolete)
        Console.WriteLine($"      - {t.Name}");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   READING ATTRIBUTES                                               ║");
    Console.WriteLine("║   GetCustomAttribute(s), Existence Check, Filtering                ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Reading Attributes Examples Completed                          ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// ATTRIBUTE DEFINITIONS
// ============================================================================

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property)]
public class DocAttr : Attribute
{
    public DocAttr(string desc) => Description = desc;
    public string Description { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class AuthorAttr : Attribute
{
    public AuthorAttr(string name) => Name = name;
    public string Name { get; }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class TagAttr : Attribute
{
    public TagAttr(string name) => Name = name;
    public string Name { get; }
}

// ============================================================================
// DOMAIN CLASSES
// ============================================================================

[AuthorAttr("Alice")]
public class SingleAttrClass { }

[TagAttr("important")]
[TagAttr("urgent")]
[TagAttr("production")]
public class MultiAttrClass { }

[DocAttr("Main documentation class")]
public class DocumentedClass
{
    [DocAttr("Value property")]
    public string Value { get; set; } = "";
}

[DocAttr("Annotated class")]
public class AnnotatedClass
{
    [DocAttr("Identifier")]
    public int Id { get; set; }
    
    [DocAttr("Name property")]
    public string Name { get; set; } = "";
    
    [DocAttr("Get data")]
    public void GetData() { }
    
    [DocAttr("Save data")]
    public void SaveData() { }
}

[AuthorAttr("Bob")]
public class AuthorClass { }

[Obsolete("Use NewClass instead")]
public class ObsoleteClass { }

public class RegularClass { }
