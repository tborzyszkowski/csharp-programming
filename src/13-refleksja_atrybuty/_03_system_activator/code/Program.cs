using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Basic CreateInstance
RunExample1();

// Example 2: CreateInstance with Parameters
RunExample2();

// Example 3: Generic CreateInstance<T>
RunExample3();

// Example 4: Plugin System Pattern
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Basic CreateInstance ===");
    
    var type = typeof(Book);
    
    Console.WriteLine("  Creating instance without parameters:");
    var book1 = Activator.CreateInstance(type);
    
    Console.WriteLine($"    ✓ Created: {book1!.GetType().Name}");
    Console.WriteLine($"    ✓ Is Book: {book1 is Book}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: CreateInstance with Parameters ===");
    
    var type = typeof(Book);
    
    Console.WriteLine("  Creating instance WITH constructor parameters:");
    var book2 = (Book)Activator.CreateInstance(type, "Clean Code", "Robert C. Martin", 464)!;
    
    Console.WriteLine($"    ✓ Title: {book2.Title}");
    Console.WriteLine($"    ✓ Author: {book2.Author}");
    Console.WriteLine($"    ✓ Pages: {book2.Pages}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: Generic CreateInstance<T> ===");
    
    Console.WriteLine("  Type-safe generic variant:");
    var book3 = Activator.CreateInstance<Book>();
    
    Console.WriteLine($"    ✓ Type: {book3.GetType().Name}");
    Console.WriteLine($"    ✓ Return Type: {nameof(Book)}");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Plugin System Pattern ===");
    
    Console.WriteLine("  Simulating plugin loading:");
    
    var plugins = new List<Type>
    {
        typeof(MathPlugin),
        typeof(TextPlugin)
    };
    
    foreach (var pluginType in plugins)
    {
        var plugin = (IPlugin)Activator.CreateInstance(pluginType)!;
        
        Console.WriteLine($"\n    Plugin: {plugin.Name}");
        Console.WriteLine($"    Version: {plugin.Version}");
        plugin.Execute();
    }
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   SYSTEM.ACTIVATOR                                                 ║");
    Console.WriteLine("║   Dynamic Instance Creation, CreateInstance, Factory Patterns      ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ System.Activator Examples Completed                            ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// Domain classes

public class Book
{
    public Book() { }
    public Book(string title, string author, int pages)
    {
        Title = title;
        Author = author;
        Pages = pages;
    }
    
    public string Title { get; set; } = "Unknown";
    public string Author { get; set; } = "Unknown";
    public int Pages { get; set; } = 0;
}

// Plugin interface
public interface IPlugin
{
    string Name { get; }
    string Version { get; }
    void Execute();
}

public class MathPlugin : IPlugin
{
    public string Name => "Math Calculator";
    public string Version => "1.0";
    
    public void Execute()
    {
        Console.WriteLine("    → Executing math calculations...");
    }
}

public class TextPlugin : IPlugin
{
    public string Name => "Text Processor";
    public string Version => "2.1";
    
    public void Execute()
    {
        Console.WriteLine("    → Processing text...");
    }
}
