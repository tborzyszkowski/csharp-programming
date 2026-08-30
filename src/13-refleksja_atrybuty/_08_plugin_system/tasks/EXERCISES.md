# Ćwiczenia: Plugin System

## 🟢 Basic Level

### Zadanie 1: Assembly.LoadFrom
Zaladuj assembly z pliku:

```csharp
// TODO: Load assembly from path
```

**Rozwiązanie:**
```csharp
string path = "C:\\Plugins\\MyPlugin.dll";
Assembly asm = Assembly.LoadFrom(path);
```

---

### Zadanie 2: GetTypes
Odkryj wszystkie typy w assembly:

```csharp
// TODO: Get all types from loaded assembly
```

**Rozwiązanie:**
```csharp
var types = asm.GetTypes();
foreach (var type in types)
    Console.WriteLine(type.Name);
```

---

### Zadanie 3: Plugin Interface
Utwórz bazowy interfejs:

```csharp
// TODO: Define IPlugin interface
public interface IPlugin
{
    // Methods
}
```

**Rozwiązanie:**
```csharp
public interface IPlugin
{
    string Name { get; }
    void Execute();
}
```

---

### Zadanie 4: IsAssignableFrom
Sprawdzaj czy typ implementuje interfejs:

```csharp
// TODO: Check if type implements IPlugin
```

**Rozwiązanie:**
```csharp
var pluginInterface = typeof(IPlugin);
var implements = pluginInterface.IsAssignableFrom(type);
```

---

### Zadanie 5: Activator.CreateInstance
Utwórz plugin dynamicznie:

```csharp
// TODO: Create plugin instance
```

**Rozwiązanie:**
```csharp
var plugin = (IPlugin)Activator.CreateInstance(pluginType);
plugin.Execute();
```

---

## 🟡 Intermediate Level

### Zadanie 6: Plugin Discovery
Odkryj wszystkie IPlugin implementacje:

```csharp
var asm = Assembly.LoadFrom("Plugin.dll");

// TODO: Find all IPlugin implementations
```

**Rozwiązanie:**
```csharp
var pluginType = typeof(IPlugin);
var implementations = asm.GetTypes()
    .Where(t => pluginType.IsAssignableFrom(t) && 
               !t.IsInterface &&
               !t.IsAbstract)
    .ToList();
```

---

### Zadanie 7: Plugin Attributes
Dodaj metadane do pluginów:

```csharp
[AttributeUsage(AttributeTargets.Class)]
public class PluginAttribute : Attribute
{
    // TODO: Define constructor and properties
}
```

**Rozwiązanie:**
```csharp
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
}
```

---

### Zadanie 8: Read Plugin Metadata
Czytaj metadane z atrybutów:

```csharp
[Plugin("Calculator", "1.0")]
public class CalcPlugin : IPlugin { }

// TODO: Read Plugin attribute
```

**Rozwiązanie:**
```csharp
var attr = typeof(CalcPlugin).GetCustomAttribute<PluginAttribute>();
Console.WriteLine($"Name: {attr?.Name}, Version: {attr?.Version}");
```

---

### Zadanie 9: Error Handling
Obsługuj błędy przy ładowaniu:

```csharp
try
{
    var asm = Assembly.LoadFrom(dllPath);
    // ...
}
catch (Exception ex)
{
    // TODO: Log error
}
```

**Rozwiązanie:**
```csharp
catch (FileNotFoundException)
{
    Console.WriteLine($"Plugin file not found: {dllPath}");
}
catch (BadImageFormatException)
{
    Console.WriteLine($"Invalid DLL: {dllPath}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error loading plugin: {ex.Message}");
}
```

---

### Zadanie 10: Plugin Manager
Zarządzaj cyklem życia pluginów:

```csharp
public class PluginManager
{
    // TODO: Implement LoadPlugin, ExecutePlugin, UnloadPlugin
}
```

**Rozwiązanie:**
```csharp
public class PluginManager
{
    private Dictionary<string, IPlugin> _plugins = new();
    
    public void LoadPlugin(string name, IPlugin plugin)
        => _plugins[name] = plugin;
    
    public void ExecutePlugin(string name)
        => _plugins[name]?.Execute();
    
    public void UnloadPlugin(string name)
        => _plugins.Remove(name);
}
```

---

## 🔴 Advanced Level

### Zadanie 11: Security Whitelisting
Implementuj whitelist dla pluginów:

```csharp
private static readonly HashSet<string> AllowedPlugins = new()
{
    "Calculator.dll",
    "Reporter.dll"
};

// TODO: Create secure loader
```

**Rozwiązanie:**
```csharp
public IPlugin LoadPlugin(string dllPath)
{
    var fileName = Path.GetFileName(dllPath);
    
    if (!AllowedPlugins.Contains(fileName))
        throw new SecurityException($"Plugin {fileName} not allowed");
    
    var asm = Assembly.LoadFrom(dllPath);
    var pluginType = asm.GetTypes()
        .FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));
    
    return (IPlugin)Activator.CreateInstance(pluginType);
}
```

---

### Zadanie 12: Plugin Discovery with Metadata
Zbuduj kompletny system z metadanymi i filtrowaniem:

```csharp
// TODO: Load plugins from directory with metadata filtering
```

**Rozwiązanie:**
```csharp
public List<(string Name, string Version, IPlugin Instance)> 
    LoadPluginsFromDirectory(string dir)
{
    var result = new List<(string, string, IPlugin)>();
    
    foreach (var dllPath in Directory.GetFiles(dir, "*.dll"))
    {
        try
        {
            var asm = Assembly.LoadFrom(dllPath);
            
            var pluginTypes = asm.GetTypes()
                .Where(t => typeof(IPlugin).IsAssignableFrom(t) && 
                           !t.IsAbstract)
                .ToList();
            
            foreach (var type in pluginTypes)
            {
                var plugin = (IPlugin)Activator.CreateInstance(type);
                var attr = type.GetCustomAttribute<PluginAttribute>();
                
                result.Add((
                    attr?.Name ?? "Unknown",
                    attr?.Version ?? "1.0",
                    plugin
                ));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading {dllPath}: {ex.Message}");
        }
    }
    
    return result;
}
```

---

## 📊 Wskazówki

- ✅ Assembly.LoadFrom zamiast LoadFile
- ✅ Zawsze sprawdzaj !IsAbstract przy discovery
- ✅ Użyj try-catch dla każdego LoadFrom
- ✅ Whitelist dla bezpieczeństwa
- ✅ Implementuj Initialize/Cleanup lifecycle
- ✅ Przechowuj metadane w atrybutach
- ❌ Nie załaduj z untrusted directories
- ❌ Nie ignoruj LoadingException
- ❌ Nie zapomnij Cleanup dla pluginów

---

## 🎯 Key Takeaways

Plugin System:

```
Assembly.LoadFrom() = zaladuj DLL
GetTypes() = odkryj typy
IsAssignableFrom() = sprawdź interfejs
Activator.CreateInstance() = utwórz instancję
Attributes = metadane pluginu
Whitelist = bezpieczeństwo
Lifecycle = Initialize → Execute → Cleanup
Error handling = obsługuj LoadingException
```

Pamiętaj: **Bezpieczeństwo jest krytyczne dla systemów plugin!**
