using System;
using System.Collections.Generic;
using Xunit;

namespace PrototypePattern;

// ============ SHALLOW COPY ============
public class Person : ICloneable
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    
    public object Clone()
    {
        return this.MemberwiseClone();  // Shallow copy
    }
    
    public override string ToString() => $"{Name} ({Age})";
}

// ============ DEEP COPY ============
public class Address
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}

public class Employee : ICloneable
{
    public string Name { get; set; } = string.Empty;
    public Address Address { get; set; } = new();
    
    public object Clone()
    {
        var cloned = (Employee)this.MemberwiseClone();
        // Deep copy Address
        cloned.Address = new Address 
        { 
            City = this.Address.City,
            Country = this.Address.Country
        };
        return cloned;
    }
    
    public override string ToString() => $"{Name} - {Address.City}";
}

// ============ PROTOTYPE FACTORY ============
public class DocumentTemplate : ICloneable
{
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;  // PDF, Word, HTML
    
    public object Clone() => this.MemberwiseClone();
    
    public override string ToString() => $"{Type}: {Title}";
}

public class PrototypeFactory
{
    private Dictionary<string, ICloneable> prototypes = new();
    
    public void RegisterPrototype(string key, ICloneable template)
    {
        prototypes[key] = template;
    }
    
    public ICloneable CreateFromPrototype(string key)
    {
        if (prototypes.TryGetValue(key, out var prototype))
        {
            return (ICloneable)prototype.Clone();
        }
        throw new KeyNotFoundException($"Prototype '{key}' not found");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 8: PROTOTYPE PATTERN ===\n");
        
        Console.WriteLine("1. SHALLOW COPY - Prymitywy:");
        var person1 = new Person { Name = "John", Age = 30 };
        var person2 = (Person)person1.Clone();
        person2.Name = "Jane";
        Console.WriteLine($"person1: {person1}");
        Console.WriteLine($"person2: {person2}");
        Console.WriteLine();
        
        Console.WriteLine("2. DEEP COPY - Obiekty zagnieżdżone:");
        var emp1 = new Employee
        {
            Name = "Alice",
            Address = new Address { City = "Warszawa", Country = "PL" }
        };
        var emp2 = (Employee)emp1.Clone();
        emp2.Name = "Bob";
        emp2.Address.City = "Kraków";
        Console.WriteLine($"emp1: {emp1}");
        Console.WriteLine($"emp2: {emp2}");
        Console.WriteLine();
        
        Console.WriteLine("3. PROTOTYPE FACTORY:");
        var factory = new PrototypeFactory();
        factory.RegisterPrototype("pdf-template", 
            new DocumentTemplate { Title = "Empty PDF", Type = "PDF" });
        factory.RegisterPrototype("word-template", 
            new DocumentTemplate { Title = "Empty Word", Type = "Word" });
        
        var doc1 = (DocumentTemplate)factory.CreateFromPrototype("pdf-template");
        var doc2 = (DocumentTemplate)factory.CreateFromPrototype("pdf-template");
        doc2.Title = "My PDF";
        
        Console.WriteLine($"doc1: {doc1}");
        Console.WriteLine($"doc2: {doc2}");
    }
}

public class PrototypePatternTests
{
    [Fact]
    public void ShallowCopy_CopiesValue()
    {
        var person1 = new Person { Name = "John", Age = 30 };
        var person2 = (Person)person1.Clone();
        
        Assert.NotSame(person1, person2);
        Assert.Equal("John", person2.Name);
    }
    
    [Fact]
    public void DeepCopy_CopiesNestedObjects()
    {
        var emp1 = new Employee
        {
            Name = "Alice",
            Address = new Address { City = "Warszawa" }
        };
        var emp2 = (Employee)emp1.Clone();
        emp2.Address.City = "Kraków";
        
        Assert.NotSame(emp1.Address, emp2.Address);
        Assert.Equal("Warszawa", emp1.Address.City);
        Assert.Equal("Kraków", emp2.Address.City);
    }
    
    [Fact]
    public void PrototypeFactory_CreatesClones()
    {
        var factory = new PrototypeFactory();
        var template = new DocumentTemplate { Title = "Template", Type = "PDF" };
        factory.RegisterPrototype("pdf", template);
        
        var doc1 = factory.CreateFromPrototype("pdf");
        var doc2 = factory.CreateFromPrototype("pdf");
        
        Assert.NotSame(doc1, doc2);
    }
}
