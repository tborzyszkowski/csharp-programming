using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel.DataAnnotations;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Caching Performance
RunExample1();

// Example 2: Simple ORM Pattern
RunExample2();

// Example 3: Best Practices Demo
RunExample3();

// Example 4: Performance Comparison
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Caching Performance ===");
    
    var cache = new SimpleAttributeCache();
    
    Console.WriteLine("  Without cache: SLOW (reflection every time)");
    Console.WriteLine("  With cache: FAST (memory lookup)\n");
    
    var attrs1 = cache.GetCached(typeof(User));
    Console.WriteLine($"    First call: {attrs1.Length} attributes");
    
    var attrs2 = cache.GetCached(typeof(User));
    Console.WriteLine($"    Second call (cached): {attrs2.Length} attributes");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Simple ORM Pattern ===");
    
    var orm = new SimpleORM<User>();
    var sql = orm.GenerateSelectSQL();
    
    Console.WriteLine($"  Generated SQL:");
    Console.WriteLine($"    {sql}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Best Practices ===");
    
    var type = typeof(User);
    
    Console.WriteLine("  ✓ GOOD practices:");
    Console.WriteLine("    - Cache reflection results");
    Console.WriteLine("    - Use GetCustomAttribute<T>");
    Console.WriteLine("    - Use IsDefined for checks");
    Console.WriteLine("    - Compile expressions");
    
    Console.WriteLine("\n  ✗ BAD practices:");
    Console.WriteLine("    - No caching");
    Console.WriteLine("    - GetCustomAttributes + OfType");
    Console.WriteLine("    - Reflection in loops");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Performance Hierarchy ===");
    
    Console.WriteLine("  Reflection performance (relative):\n");
    Console.WriteLine("    1. Direct call:      ⚡⚡⚡ ~10ns");
    Console.WriteLine("    2. Compiled expr:    ⚡⚡  ~20ns");
    Console.WriteLine("    3. Cached reflect:   ⚡   ~0.1µs");
    Console.WriteLine("    4. Fresh reflect:    🐢   ~1µs");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   PERFORMANCE & BEST PRACTICES                                     ║");
    Console.WriteLine("║   Caching, ORM Pattern, Optimization, AOT Compatibility            ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Performance & Best Practices Examples Completed                ║");
    Console.WriteLine("║   ✓ Module 13: Refleksja i Atrybuty COMPLETE (10/10)              ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// ATTRIBUTES
// ============================================================================

[AttributeUsage(AttributeTargets.Class)]
public class TableAttr : Attribute
{
    public TableAttr(string name) => Name = name;
    public string Name { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttr : Attribute
{
    public ColumnAttr(string name) => Name = name;
    public string Name { get; }
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttr : Attribute { }

// ============================================================================
// DOMAIN CLASSES
// ============================================================================

[TableAttr("users")]
public class User
{
    [ColumnAttr("id")]
    [RequiredAttr]
    public int Id { get; set; }
    
    [ColumnAttr("name")]
    [RequiredAttr]
    public string Name { get; set; } = "";
    
    [ColumnAttr("email")]
    [RequiredAttr]
    public string Email { get; set; } = "";
}

// ============================================================================
// CACHING INFRASTRUCTURE
// ============================================================================

public class SimpleAttributeCache
{
    private static readonly Dictionary<Type, Attribute[]> Cache = new();
    
    public Attribute[] GetCached(Type type)
    {
        if (!Cache.TryGetValue(type, out var attrs))
        {
            attrs = type.GetCustomAttributes().ToArray();
            Cache[type] = attrs;
        }
        return attrs;
    }
}

// ============================================================================
// SIMPLE ORM
// ============================================================================

public class SimpleORM<T> where T : class, new()
{
    private string _tableName;
    private Dictionary<string, string> _columns;
    
    public SimpleORM()
    {
        var type = typeof(T);
        
        var tableAttr = type.GetCustomAttribute<TableAttr>();
        _tableName = tableAttr?.Name ?? type.Name;
        
        _columns = new();
        foreach (var prop in type.GetProperties())
        {
            var colAttr = prop.GetCustomAttribute<ColumnAttr>();
            var colName = colAttr?.Name ?? prop.Name;
            _columns[prop.Name] = colName;
        }
    }
    
    public string GenerateSelectSQL()
    {
        var cols = string.Join(", ", _columns.Values);
        return $"SELECT {cols} FROM {_tableName}";
    }
}
