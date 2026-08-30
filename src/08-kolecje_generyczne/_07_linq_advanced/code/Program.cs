#!/usr/bin/env dotnet-script
// Demonstracja LINQ - Zaawansowane Techniki

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqAdvancedDemo
{
    public class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string Department { get; set; } = "";
        
        public Person(string name, int age, string dept)
        {
            Name = name;
            Age = age;
            Department = dept;
        }
        
        public override string ToString() => $"{Name} ({Age}, {Department})";
    }
    
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║        LINQ Zaawansowane - Demonstracja                   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        var people = new List<Person>
        {
            new("Alice", 28, "IT"),
            new("Bob", 35, "HR"),
            new("Charlie", 22, "IT"),
            new("Diana", 32, "Finance"),
            new("Eve", 26, "IT")
        };
        
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        
        DemonstrateAggregate(people, numbers);
        DemonstrateSelectMany();
        DemonstrateSetOperations();
        DemonstrateTakeSkip(numbers);
        DemonstrateDistinct();
    }
    
    private static void DemonstrateAggregate(List<Person> people, int[] numbers)
    {
        Console.WriteLine("\n📌 AGREGUJĄCE OPERATORY\n");
        
        Console.WriteLine($"Count: {people.Count()}");
        Console.WriteLine($"Average Age: {people.Average(p => p.Age):F1}");
        Console.WriteLine($"Sum (numbers): {numbers.Sum()}");
        
        var product = numbers.Aggregate(1, (acc, x) => acc * x);
        Console.WriteLine($"Product (1*2*3*...*10): {product}");
    }
    
    private static void DemonstrateSelectMany()
    {
        Console.WriteLine("\n📌 SELECTMANY - Spłaszczanie\n");
        
        var departments = new[]
        {
            new { Name = "IT", Members = new[] { "Alice", "Bob" } },
            new { Name = "HR", Members = new[] { "Charlie" } }
        };
        
        var allMembers = departments.SelectMany(d => d.Members);
        Console.WriteLine("Wszyscy członkowie:");
        foreach (var member in allMembers)
            Console.WriteLine($"  {member}");
    }
    
    private static void DemonstrateSetOperations()
    {
        Console.WriteLine("\n📌 SET OPERATIONS\n");
        
        var set1 = new[] { 1, 2, 3, 4 };
        var set2 = new[] { 3, 4, 5, 6 };
        
        Console.WriteLine($"Set1: [{string.Join(", ", set1)}]");
        Console.WriteLine($"Set2: [{string.Join(", ", set2)}]");
        
        Console.WriteLine($"Union: [{string.Join(", ", set1.Union(set2))}]");
        Console.WriteLine($"Intersect: [{string.Join(", ", set1.Intersect(set2))}]");
        Console.WriteLine($"Except: [{string.Join(", ", set1.Except(set2))}]");
    }
    
    private static void DemonstrateTakeSkip(int[] numbers)
    {
        Console.WriteLine("\n📌 TAKE/SKIP\n");
        
        Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"Take(5): [{string.Join(", ", numbers.Take(5))}]");
        Console.WriteLine($"Skip(7): [{string.Join(", ", numbers.Skip(7))}]");
        
        var page = numbers.Skip(3).Take(3);
        Console.WriteLine($"Page (Skip 3, Take 3): [{string.Join(", ", page)}]");
    }
    
    private static void DemonstrateDistinct()
    {
        Console.WriteLine("\n📌 DISTINCT\n");
        
        var numbers = new[] { 1, 1, 2, 2, 3, 3, 3 };
        Console.WriteLine($"Numbers: [{string.Join(", ", numbers)}]");
        Console.WriteLine($"Distinct: [{string.Join(", ", numbers.Distinct())}]");
    }
}
