using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Attribute Inheritance
RunExample1();

// Example 2: Attribute Stacking
RunExample2();

// Example 3: Advanced Validation
RunExample3();

// Example 4: Metadata Extraction
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Attribute Hierarchy ===");
    
    var type = typeof(AdvancedUserClass);
    
    Console.WriteLine($"  Type: {type.Name}\n");
    
    var baseAttr = type.GetCustomAttribute<BaseDocAttribute>();
    var advAttr = type.GetCustomAttribute<AdvDocAttribute>();
    
    if (baseAttr != null)
        Console.WriteLine($"    Base: {baseAttr.Description}");
    
    if (advAttr != null)
    {
        Console.WriteLine($"    Advanced: {advAttr.Description}");
        Console.WriteLine($"    Author: {advAttr.Author}");
        Console.WriteLine($"    Version: {advAttr.Version}");
    }
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Attribute Stacking ===");
    
    var type = typeof(ComposedClass);
    
    Console.WriteLine($"  Type: {type.Name}\n");
    
    var authors = type.GetCustomAttributes<AuthorAttr>().ToArray();
    var versions = type.GetCustomAttributes<VersionAttr>().ToArray();
    var licenses = type.GetCustomAttributes<LicenseAttr>().ToArray();
    
    Console.WriteLine($"    Authors: {authors.Length}");
    foreach (var auth in authors)
        Console.WriteLine($"      - {auth.Name}");
    
    Console.WriteLine($"    Versions: {versions.Length}");
    foreach (var ver in versions)
        Console.WriteLine($"      - {ver.Version}");
    
    Console.WriteLine($"    Licenses: {licenses.Length}");
    foreach (var lic in licenses)
        Console.WriteLine($"      - {lic.License}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Advanced Validation ===");
    
    var product = new ConstrainedProduct
    {
        Price = 5000,
        Quantity = 50,
        Sku = "PROD-2024-001"
    };
    
    Console.WriteLine($"  Validating product: {product.Sku}\n");
    
    var validator = new AdvValidator();
    bool isValid = validator.Validate(product, out var errors);
    
    if (isValid)
        Console.WriteLine("    ✓ All constraints satisfied!");
    else
    {
        Console.WriteLine("    ✗ Validation errors:\n");
        foreach (var error in errors)
            Console.WriteLine($"      ✗ {error}");
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Metadata Extraction ===");
    
    var type = typeof(ORMEntity);
    var builder = new MetadataBuilder();
    
    Console.WriteLine($"  Type: {type.Name}\n");
    
    try
    {
        var metadata = builder.GetTableMetadata(type);
        
        Console.WriteLine($"    Table: {metadata["TableName"]}");
        
        var props = (List<Dictionary<string, object>>)metadata["Properties"]!;
        Console.WriteLine($"    Properties: {props.Count}\n");
        
        foreach (var prop in props.Take(3))
        {
            Console.WriteLine($"      Property: {prop["PropertyName"]}");
            Console.WriteLine($"        Column: {prop["ColumnName"]}");
            Console.WriteLine($"        Type: {prop["Type"]}");
            Console.WriteLine($"        PrimaryKey: {prop["IsPrimaryKey"]}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"    Error: {ex.Message}");
    }
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ADVANCED ATTRIBUTES                                              ║");
    Console.WriteLine("║   Inheritance, Stacking, Validation, Metadata Extraction           ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Advanced Attributes Examples Completed                         ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// ATTRIBUTE DEFINITIONS
// ============================================================================

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class BaseDocAttribute : Attribute
{
    public BaseDocAttribute(string description) => Description = description;
    public string Description { get; }
}

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class AdvDocAttribute : BaseDocAttribute
{
    public AdvDocAttribute(string description) : base(description) { }
    public string? Author { get; set; }
    public string? Version { get; set; }
}

[AttributeUsage(AttributeTargets.Class)]
public class AuthorAttr : Attribute
{
    public AuthorAttr(string name) => Name = name;
    public string Name { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class VersionAttr : Attribute
{
    public VersionAttr(string version) => Version = version;
    public string Version { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class LicenseAttr : Attribute
{
    public LicenseAttr(string license) => License = license;
    public string License { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RangeAttr : Attribute
{
    public RangeAttr(int min, int max) { Min = min; Max = max; }
    public int Min { get; }
    public int Max { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class PatternAttr : Attribute
{
    public PatternAttr(string pattern) => Pattern = pattern;
    public string Pattern { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class TableAttr : Attribute
{
    public TableAttr(string tableName) => TableName = tableName;
    public string TableName { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttr : Attribute
{
    public ColumnAttr(string columnName) => ColumnName = columnName;
    public string ColumnName { get; }
    public bool IsPrimaryKey { get; set; }
    public bool IsNullable { get; set; } = true;
}

// ============================================================================
// DOMAIN CLASSES
// ============================================================================

[AdvDoc("Advanced user model", Author = "Alice", Version = "2.5")]
public class AdvancedUserClass { }

[AuthorAttr("Bob")]
[VersionAttr("1.0")]
[LicenseAttr("MIT")]
public class ComposedClass { }

public class ConstrainedProduct
{
    [RangeAttr(1, 10000)]
    public int Price { get; set; }
    
    [RangeAttr(1, 1000)]
    public int Quantity { get; set; }
    
    [PatternAttr(@"^[A-Z0-9\-]+$")]
    public string Sku { get; set; } = "";
}

[TableAttr("orm_entities")]
public class ORMEntity
{
    [ColumnAttr("id")]
    public int Id { get; set; }
    
    [ColumnAttr("name")]
    public string Name { get; set; } = "";
    
    [ColumnAttr("created_at")]
    public DateTime CreatedAt { get; set; }
}

// ============================================================================
// VALIDATORS & BUILDERS
// ============================================================================

public class AdvValidator
{
    public bool Validate(object obj, out List<string> errors)
    {
        errors = new();
        var type = obj.GetType();
        
        foreach (var property in type.GetProperties())
        {
            var value = property.GetValue(obj);
            
            var rangeAttr = property.GetCustomAttribute<RangeAttr>();
            if (rangeAttr != null && value is int intVal)
            {
                if (intVal < rangeAttr.Min || intVal > rangeAttr.Max)
                    errors.Add($"{property.Name}: {intVal} not in range {rangeAttr.Min}-{rangeAttr.Max}");
            }
            
            var patternAttr = property.GetCustomAttribute<PatternAttr>();
            if (patternAttr != null && value is string strVal)
            {
                if (!Regex.IsMatch(strVal, patternAttr.Pattern))
                    errors.Add($"{property.Name}: '{strVal}' doesn't match pattern {patternAttr.Pattern}");
            }
        }
        
        return errors.Count == 0;
    }
}

public class MetadataBuilder
{
    public Dictionary<string, object> GetTableMetadata(Type type)
    {
        var tableAttr = type.GetCustomAttribute<TableAttr>();
        if (tableAttr == null)
            throw new InvalidOperationException("Type must have [Table]");
        
        var metadata = new Dictionary<string, object>
        {
            { "TableName", tableAttr.TableName },
            { "Properties", new List<Dictionary<string, object>>() }
        };
        
        var properties = (List<Dictionary<string, object>>)metadata["Properties"]!;
        
        foreach (var prop in type.GetProperties())
        {
            var columnAttr = prop.GetCustomAttribute<ColumnAttr>();
            if (columnAttr == null)
                continue;
            
            properties.Add(new()
            {
                { "PropertyName", prop.Name },
                { "ColumnName", columnAttr.ColumnName },
                { "Type", prop.PropertyType.Name },
                { "IsPrimaryKey", columnAttr.IsPrimaryKey },
                { "IsNullable", columnAttr.IsNullable }
            });
        }
        
        return metadata;
    }
}
