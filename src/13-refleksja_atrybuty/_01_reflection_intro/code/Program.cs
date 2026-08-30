using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Basic Type Inspection
RunExample1();

// Example 2: Assembly Loading
RunExample2();

// Example 3: Workflow Demo
RunExample3();

// Example 4: Performance Comparison
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic Type Inspection ===");
    
    var type = typeof(Person);
    
    Console.WriteLine($"  Type Name: {type.Name}");
    Console.WriteLine($"  Full Name: {type.FullName}");
    Console.WriteLine($"  Base Type: {type.BaseType}");
    Console.WriteLine($"  Is Class: {type.IsClass}");
    Console.WriteLine($"  Is Public: {type.IsPublic}");
    
    Console.WriteLine($"\n  Properties:");
    var props = type.GetProperties();
    foreach (var prop in props)
    {
        Console.WriteLine($"    - {prop.Name} ({prop.PropertyType.Name})");
    }
    
    Console.WriteLine($"\n  Methods:");
    var methods = type.GetMethods(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public);
    foreach (var method in methods.Take(3))
    {
        Console.WriteLine($"    - {method.Name}()");
    }
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Assembly Loading ===");
    
    var assembly = Assembly.GetExecutingAssembly();
    Console.WriteLine($"  Assembly: {assembly.GetName().Name}");
    Console.WriteLine($"  Version: {assembly.GetName().Version}");
    
    var types = assembly.GetTypes();
    Console.WriteLine($"  Total Types: {types.Length}");
    
    var publicTypes = types.Where(t => t.IsPublic).Take(5);
    Console.WriteLine($"\n  First 5 Public Types:");
    foreach (var type in publicTypes)
    {
        Console.WriteLine($"    - {type.Name}");
    }
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Reflection Workflow ===");
    
    Console.WriteLine("  Step 1: Get Type");
    var type = typeof(Person);
    Console.WriteLine($"    ✓ Got Type: {type.Name}");
    
    Console.WriteLine("\n  Step 2: Get Property Info");
    var prop = type.GetProperty("Name");
    Console.WriteLine($"    ✓ Got Property: {prop!.Name}");
    
    Console.WriteLine("\n  Step 3: Create Instance");
    var instance = Activator.CreateInstance(type);
    Console.WriteLine($"    ✓ Created: {instance!.GetType().Name}");
    
    Console.WriteLine("\n  Step 4: Set Property via Reflection");
    prop.SetValue(instance, "Alice");
    var value = prop.GetValue(instance);
    Console.WriteLine($"    ✓ Set Name to: {value}");
    
    Console.WriteLine("\n  Step 5: Invoke Method via Reflection");
    var method = type.GetMethod("Greet");
    var result = method!.Invoke(instance, null);
    Console.WriteLine($"    ✓ Method Result: {result}");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Performance Comparison ===");
    
    var iterations = 10000;
    var person = new Person { Name = "Bob", Age = 30 };
    var type = typeof(Person);
    var prop = type.GetProperty("Name");
    
    // Method 1: Direct access
    var sw1 = System.Diagnostics.Stopwatch.StartNew();
    for (int i = 0; i < iterations; i++)
    {
        var _ = person.Name;
    }
    sw1.Stop();
    
    // Method 2: Reflection (not cached)
    var sw2 = System.Diagnostics.Stopwatch.StartNew();
    for (int i = 0; i < iterations; i++)
    {
        var _ = typeof(Person).GetProperty("Name")!.GetValue(person);
    }
    sw2.Stop();
    
    // Method 3: Reflection (cached)
    var sw3 = System.Diagnostics.Stopwatch.StartNew();
    for (int i = 0; i < iterations; i++)
    {
        var _ = prop!.GetValue(person);
    }
    sw3.Stop();
    
    Console.WriteLine($"  Direct Access:        {sw1.ElapsedMilliseconds} ms");
    Console.WriteLine($"  Reflection (uncached): {sw2.ElapsedMilliseconds} ms ({(double)sw2.ElapsedMilliseconds / sw1.ElapsedMilliseconds:F1}x slower)");
    Console.WriteLine($"  Reflection (cached):  {sw3.ElapsedMilliseconds} ms ({(double)sw3.ElapsedMilliseconds / sw1.ElapsedMilliseconds:F1}x slower)");
    
    Console.WriteLine($"\n  Lesson: Cache reflection metadata!");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   REFLECTION INTRODUCTION                                          ║");
    Console.WriteLine("║   System.Reflection, System.Type, Runtime Inspection               ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Reflection Examples Completed                                  ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain class for examples
public class Person
{
    public string Name { get; set; } = "Unknown";
    public int Age { get; set; } = 0;
    
    public string Greet()
    {
        return $"Hello, I'm {Name} and I'm {Age} years old";
    }
    
    public void SetAge(int age)
    {
        Age = age;
    }
}
