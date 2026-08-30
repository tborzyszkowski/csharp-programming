using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: GetProperties
RunExample1();

// Example 2: GetMethods + MethodInfo
RunExample2();

// Example 3: Generic Types Inspection
RunExample3();

// Example 4: Inheritance Hierarchy
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: GetProperties() ===");
    
    var type = typeof(Person);
    var properties = type.GetProperties();
    
    Console.WriteLine($"  Type: {type.Name}");
    Console.WriteLine($"  Properties: {properties.Length}\n");
    
    foreach (var prop in properties)
    {
        var readable = prop.CanRead ? "✓" : "✗";
        var writable = prop.CanWrite ? "✓" : "✗";
        
        Console.WriteLine($"    {prop.Name} ({prop.PropertyType.Name})");
        Console.WriteLine($"      Readable: {readable}, Writable: {writable}");
    }
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: GetMethods() + MethodInfo ===");
    
    var type = typeof(Calculator);
    var methods = type.GetMethods(
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    
    Console.WriteLine($"  Type: {type.Name}");
    Console.WriteLine($"  Methods: {methods.Length}\n");
    
    foreach (var method in methods)
    {
        var parameters = string.Join(", ",
            method.GetParameters().Select(p => $"{p.ParameterType.Name} {p.Name}"));
        
        Console.WriteLine($"    {method.Name}({parameters})");
        Console.WriteLine($"      Returns: {method.ReturnType.Name}");
    }
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Generic Types ===");
    
    var types = new[]
    {
        typeof(List<int>),
        typeof(Dictionary<string, int>),
        typeof(Person)
    };
    
    foreach (var type in types)
    {
        Console.WriteLine($"  Type: {type.Name}");
        Console.WriteLine($"  Is Generic: {type.IsGenericType}");
        
        if (type.IsGenericType)
        {
            var args = type.GetGenericArguments();
            Console.WriteLine($"  Generic Args: {string.Join(", ", args.Select(t => t.Name))}");
        }
        Console.WriteLine();
    }
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Inheritance Hierarchy ===");
    
    var type = typeof(Poodle);
    
    Console.WriteLine($"  Type: {type.Name}");
    Console.WriteLine("  Inheritance Chain:");
    
    var current = type;
    while (current != null && current != typeof(object))
    {
        Console.WriteLine($"    ├─ {current.Name}");
        current = current.BaseType;
    }
    
    Console.WriteLine($"\n  Interfaces:");
    var interfaces = type.GetInterfaces();
    foreach (var iface in interfaces.Take(3))
    {
        Console.WriteLine($"    ├─ {iface.Name}");
    }
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   TYPE INSPECTION                                                  ║");
    Console.WriteLine("║   GetProperties, GetMethods, Generic Types, Inheritance            ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Type Inspection Examples Completed                             ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; private set; } = string.Empty;
}

public class Calculator
{
    public int Add(int a, int b) => a + b;
    public int Subtract(int a, int b) => a - b;
    public int Multiply(int a, int b) => a * b;
    public double Divide(double a, double b) => a / b;
}

public abstract class Animal
{
    public virtual string Speak() => "Some sound";
}

public class Dog : Animal
{
    public override string Speak() => "Woof!";
}

public class Poodle : Dog
{
    public void Groom() { }
}
