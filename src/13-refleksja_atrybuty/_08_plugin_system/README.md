# Temat 8: Plugin System - Dynamic Assembly Loading

## 🔌 Czym jest Plugin System?

**Plugin System** = dynamiczna załadowanie DLL w runtime, odkrycie klas, instancjacja.

```csharp
// Compile-time: znasz wszystkie kody
typeof(MyClass);

// Runtime: zaladuj nieznane DLL i użyj klas
Assembly asm = Assembly.LoadFrom("MyPlugin.dll");
Type pluginType = asm.GetType("MyPlugin.CalculatorPlugin");
IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);
```

---

## 📚 Assembly.LoadFrom vs LoadFile

### LoadFrom - Preferred

```csharp
// Załaduj DLL z pliku
string path = "C:\\Plugins\\MyPlugin.dll";
Assembly asm = Assembly.LoadFrom(path);

// Properties
Console.WriteLine($"Name: {asm.GetName().Name}");
Console.WriteLine($"Version: {asm.GetName().Version}");

// Get all types
var types = asm.GetTypes();
foreach (var type in types)
    Console.WriteLine($"  - {type.Name}");
```

### LoadFile - Raw Loading

```csharp
// LoadFile - ładuj bez Context
Assembly asm = Assembly.LoadFile("C:\\path\\to\\plugin.dll");

// Różnice:
// LoadFrom: caches, shadow copies
// LoadFile: raw load, no dependencies
```

---

## 🔍 Plugin Discovery

```csharp
// Interfejs pluginu
public interface IPlugin
{
    string Name { get; }
    string Version { get; }
    void Execute();
}

// Załaduj i odkryj pluginy
public class PluginLoader
{
    public List<IPlugin> LoadPlugins(string pluginDirectory)
    {
        var plugins = new List<IPlugin>();
        
        // Znajdź wszystkie DLL
        var dllFiles = Directory.GetFiles(pluginDirectory, "*.dll");
        
        foreach (var dllPath in dllFiles)
        {
            try
            {
                // Załaduj assembly
                var asm = Assembly.LoadFrom(dllPath);
                
                // Znajdź klasy implementujące IPlugin
                var pluginTypes = asm.GetTypes()
                    .Where(t => typeof(IPlugin).IsAssignableFrom(t) && 
                               !t.IsInterface && 
                               !t.IsAbstract)
                    .ToList();
                
                // Instancjonuj każdy plugin
                foreach (var pluginType in pluginTypes)
                {
                    try
                    {
                        var plugin = (IPlugin)Activator.CreateInstance(pluginType);
                        plugins.Add(plugin);
                        
                        Console.WriteLine($"✓ Loaded: {plugin.Name} v{plugin.Version}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"✗ Failed to create {pluginType.Name}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Failed to load {Path.GetFileName(dllPath)}: {ex.Message}");
            }
        }
        
        return plugins;
    }
}

// Użycie
var loader = new PluginLoader();
var plugins = loader.LoadPlugins("C:\\Plugins");

foreach (var plugin in plugins)
    plugin.Execute();
```

---

## 🏷️ Plugin Attributes - Metadata

```csharp
// Atrybut dla metadanych pluginu
[AttributeUsage(AttributeTargets.Class)]
public class PluginAttribute : Attribute
{
    public PluginAttribute(string name, string version)
    {
        Name = name;
        Version = version;
    }
    
    public string Name { get; }
    public string Version { get; }
    public string? Description { get; set; }
    public string? Author { get; set; }
}

// Plugin z atrybutem
[Plugin("Calculator", "1.0", 
    Description = "Math operations",
    Author = "Alice")]
public class CalculatorPlugin : IPlugin
{
    public string Name => "Calculator";
    public string Version => "1.0";
    
    public void Execute()
    {
        Console.WriteLine("Running calculator...");
    }
}

// Odkryj metadata z atrybutów
var type = typeof(CalculatorPlugin);
var attr = type.GetCustomAttribute<PluginAttribute>();
Console.WriteLine($"Plugin: {attr?.Name} v{attr?.Version}");
Console.WriteLine($"Description: {attr?.Description}");
```

---

## 🔒 Security - Plugin Whitelisting

```csharp
// Niebezpieczne: zaladuj KAŻDĄ DLL
var loader = new PluginLoader();
var plugins = loader.LoadPlugins("C:\\untrusted");  // ❌

// Bezpieczne: whitelist znanych pluginów
public class SecurePluginLoader
{
    private static readonly HashSet<string> AllowedPlugins = new()
    {
        "Calculator.dll",
        "Reporter.dll",
        "Exporter.dll"
    };
    
    public IPlugin LoadPlugin(string dllPath)
    {
        var fileName = Path.GetFileName(dllPath);
        
        if (!AllowedPlugins.Contains(fileName))
            throw new SecurityException($"Plugin {fileName} not in whitelist");
        
        // Verify signature (optional but recommended)
        if (!VerifyPluginSignature(dllPath))
            throw new SecurityException($"Plugin {fileName} signature invalid");
        
        var asm = Assembly.LoadFrom(dllPath);
        // ... rest of loading
    }
    
    private bool VerifyPluginSignature(string dllPath)
    {
        // Check digital signature
        // (simplified - in production use proper verification)
        return File.Exists(dllPath);
    }
}
```

---

## 🧩 Plugin Lifecycle Management

```csharp
public interface IPlugin
{
    string Name { get; }
    void Initialize();
    void Execute();
    void Cleanup();
}

public class PluginManager
{
    private Dictionary<string, IPlugin> _loadedPlugins = new();
    
    public void LoadPlugin(string dllPath)
    {
        try
        {
            var asm = Assembly.LoadFrom(dllPath);
            var pluginType = asm.GetTypes()
                .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));
            
            if (pluginType == null)
                throw new InvalidOperationException("No IPlugin implementation found");
            
            var plugin = (IPlugin)Activator.CreateInstance(pluginType);
            plugin.Initialize();
            
            _loadedPlugins[plugin.Name] = plugin;
            Console.WriteLine($"✓ Plugin loaded: {plugin.Name}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}");
        }
    }
    
    public void ExecutePlugin(string pluginName)
    {
        if (_loadedPlugins.TryGetValue(pluginName, out var plugin))
        {
            plugin.Execute();
        }
        else
        {
            Console.WriteLine($"Plugin not found: {pluginName}");
        }
    }
    
    public void UnloadPlugin(string pluginName)
    {
        if (_loadedPlugins.TryGetValue(pluginName, out var plugin))
        {
            plugin.Cleanup();
            _loadedPlugins.Remove(pluginName);
            Console.WriteLine($"✓ Plugin unloaded: {pluginName}");
        }
    }
    
    public void UnloadAll()
    {
        foreach (var plugin in _loadedPlugins.Values)
            plugin.Cleanup();
        
        _loadedPlugins.Clear();
    }
}
```

---

## 🔗 Dependency Injection in Plugins

```csharp
// Wstrzykuj zależności do pluginów

public interface ILogger { void Log(string msg); }
public interface IDatabase { void Query(string sql); }

public class PluginContext
{
    public ILogger Logger { get; }
    public IDatabase Database { get; }
    
    public PluginContext(ILogger logger, IDatabase db)
    {
        Logger = logger;
        Database = db;
    }
}

public interface IPlugin
{
    void Initialize(PluginContext context);
    void Execute();
}

public class SmartPlugin : IPlugin
{
    private ILogger? _logger;
    private IDatabase? _database;
    
    public void Initialize(PluginContext context)
    {
        _logger = context.Logger;
        _database = context.Database;
    }
    
    public void Execute()
    {
        _logger?.Log("Plugin executing...");
        _database?.Query("SELECT * FROM users");
    }
}

// Użycie
var context = new PluginContext(new ConsoleLogger(), new SqlDatabase());
var plugin = new SmartPlugin();
plugin.Initialize(context);
plugin.Execute();
```

---

## 📚 Summary

**Plugin System:**

- **LoadFrom** - zaladuj DLL z ścieżki
- **GetTypes()** - odkryj dostępne typy
- **IsAssignableFrom** - sprawdź czy typ implementuje interfejs
- **Activator** - utwórz instancję dynamicznie
- **Attributes** - przechowuj metadane
- **Whitelist** - bezpieczeństwo
- **Lifecycle** - Initialize, Execute, Cleanup

---

## 🎯 Następny Temat

Temat 9: Expression Trees & Dynamic - Building expressions, dynamic keyword, performance
