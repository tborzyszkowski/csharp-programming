using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Assembly Loading
RunExample1();

// Example 2: Plugin Discovery
RunExample2();

// Example 3: Plugin Manager Lifecycle
RunExample3();

// Example 4: Security - Whitelisting
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Assembly Loading ===");
    
    // Simulate loading current assembly
    var currentAsm = typeof(Program).Assembly;
    
    Console.WriteLine($"  Assembly: {currentAsm.GetName().Name}");
    Console.WriteLine($"  Version: {currentAsm.GetName().Version}");
    
    var types = currentAsm.GetTypes().Where(t => t.IsClass).ToArray();
    Console.WriteLine($"  Types: {types.Length}\n");
    
    foreach (var type in types.Take(3))
        Console.WriteLine($"    - {type.Name}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Plugin Discovery ===");
    
    var asm = typeof(Program).Assembly;
    
    // Discover IPlugin implementations
    var pluginType = typeof(IPlugin);
    var implementations = asm.GetTypes()
        .Where(t => pluginType.IsAssignableFrom(t) && 
                   !t.IsInterface &&
                   !t.IsAbstract)
        .ToArray();
    
    Console.WriteLine($"  Found {implementations.Length} plugin(s):\n");
    
    foreach (var impl in implementations)
    {
        Console.WriteLine($"    Plugin: {impl.Name}");
        
        // Check for Plugin attribute
        var attr = impl.GetCustomAttribute<PluginAttr>();
        if (attr != null)
            Console.WriteLine($"      Metadata: {attr.Description}");
    }
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Plugin Manager Lifecycle ===");
    
    var manager = new SimplePluginManager();
    
    // Load plugins
    var plugin1 = new TestPlugin1();
    var plugin2 = new TestPlugin2();
    
    manager.RegisterPlugin("test1", plugin1);
    manager.RegisterPlugin("test2", plugin2);
    
    Console.WriteLine($"  Loaded plugins: {manager.GetCount()}\n");
    
    // Execute
    Console.WriteLine("  Executing plugins:");
    manager.ExecuteAll();
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Security - Whitelisting ===");
    
    var allowed = new[] { "Calculator.dll", "Reporter.dll" };
    var toLoad = new[] { "Calculator.dll", "Malware.dll", "Reporter.dll" };
    
    Console.WriteLine("  Whitelist check:\n");
    
    foreach (var dll in toLoad)
    {
        bool isAllowed = allowed.Contains(dll);
        var status = isAllowed ? "✓ ALLOWED" : "✗ BLOCKED";
        Console.WriteLine($"    {dll}: {status}");
    }
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   PLUGIN SYSTEM                                                    ║");
    Console.WriteLine("║   Assembly Loading, Discovery, Lifecycle Management                ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Plugin System Examples Completed                               ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// PLUGIN INFRASTRUCTURE
// ============================================================================

public interface IPlugin
{
    string Name { get; }
    string Version { get; }
    string Description { get; }
    void Execute();
}

[AttributeUsage(AttributeTargets.Class)]
public class PluginAttr : Attribute
{
    public PluginAttr(string name, string version, string desc)
    {
        Name = name;
        Version = version;
        Description = desc;
    }
    
    public string Name { get; }
    public string Version { get; }
    public string Description { get; }
}

// ============================================================================
// SAMPLE PLUGINS
// ============================================================================

[PluginAttr("Calculator", "1.0", "Math operations plugin")]
public class TestPlugin1 : IPlugin
{
    public string Name => "Calculator";
    public string Version => "1.0";
    public string Description => "Performs calculations";
    
    public void Execute() => Console.WriteLine("      → Calculator executing");
}

[PluginAttr("Reporter", "2.0", "Report generation plugin")]
public class TestPlugin2 : IPlugin
{
    public string Name => "Reporter";
    public string Version => "2.0";
    public string Description => "Generates reports";
    
    public void Execute() => Console.WriteLine("      → Reporter executing");
}

// ============================================================================
// PLUGIN MANAGER
// ============================================================================

public class SimplePluginManager
{
    private Dictionary<string, IPlugin> _plugins = new();
    
    public void RegisterPlugin(string key, IPlugin plugin)
    {
        _plugins[key] = plugin;
    }
    
    public int GetCount() => _plugins.Count;
    
    public void ExecuteAll()
    {
        foreach (var kvp in _plugins)
            kvp.Value.Execute();
    }
}
