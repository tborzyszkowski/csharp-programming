#!/usr/bin/env dotnet-script
// Demonstracja LINQ - Wprowadzenie

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqIntroDemo
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
        Console.WriteLine("║           LINQ Introdukcja - Demonstracja                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        var people = new List<Person>
        {
            new("Alice", 28, "IT"),
            new("Bob", 35, "HR"),
            new("Charlie", 22, "IT"),
            new("Diana", 32, "Finance"),
            new("Eve", 26, "IT")
        };
        
        DemonstrateWhere(people);
        DemonstrateSelect(people);
        DemonstrateOrderBy(people);
        DemonstrateGroupBy(people);
        DemonstrateFirstLast(people);
        DemonstrateLazyEvaluation();
    }
    
    private static void DemonstrateWhere(List<Person> people)
    {
        Console.WriteLine("\n📌 WHERE - Filtrowanie\n");
        
        var adults = people.Where(p => p.Age >= 30);
        
        Console.WriteLine("Osoby w wieku 30+:");
        foreach (var p in adults)
            Console.WriteLine($"  {p}");
    }
    
    private static void DemonstrateSelect(List<Person> people)
    {
        Console.WriteLine("\n📌 SELECT - Transformacja\n");
        
        var names = people.Select(p => p.Name);
        
        Console.WriteLine("Imiona:");
        foreach (var name in names)
            Console.WriteLine($"  {name}");
        
        var anonymous = people.Select(p => new { p.Name, p.Age });
        Console.WriteLine("\nAnonimowe obiekty (Name, Age):");
        foreach (var item in anonymous)
            Console.WriteLine($"  {item}");
    }
    
    private static void DemonstrateOrderBy(List<Person> people)
    {
        Console.WriteLine("\n📌 ORDERBY - Sortowanie\n");
        
        var byAge = people.OrderBy(p => p.Age);
        Console.WriteLine("Sortowanie po wieku (rosnąco):");
        foreach (var p in byAge)
            Console.WriteLine($"  {p}");
        
        var byAgeDesc = people.OrderByDescending(p => p.Age);
        Console.WriteLine("\nSortowanie po wieku (malejąco):");
        foreach (var p in byAgeDesc.Take(3))
            Console.WriteLine($"  {p}");
    }
    
    private static void DemonstrateGroupBy(List<Person> people)
    {
        Console.WriteLine("\n📌 GROUPBY - Grupowanie\n");
        
        var byDept = people.GroupBy(p => p.Department);
        
        foreach (var group in byDept)
        {
            Console.WriteLine($"Department: {group.Key} ({group.Count()} osób)");
            foreach (var person in group)
                Console.WriteLine($"  {person}");
        }
    }
    
    private static void DemonstrateFirstLast(List<Person> people)
    {
        Console.WriteLine("\n📌 FIRST/LAST OPERATORY\n");
        
        var youngest = people.OrderBy(p => p.Age).First();
        Console.WriteLine($"Najmłodszy: {youngest}");
        
        var anyAdult = people.FirstOrDefault(p => p.Age >= 35);
        Console.WriteLine($"Dorosły (35+): {anyAdult}");
        
        bool hasITDept = people.Any(p => p.Department == "IT");
        Console.WriteLine($"Czy jest IT department? {hasITDept}");
    }
    
    private static void DemonstrateLazyEvaluation()
    {
        Console.WriteLine("\n📌 LAZY EVALUATION\n");
        
        var numbers = Enumerable.Range(1, 10);
        
        var query = numbers
            .Where(n => { Console.Write($"[Filter {n}] "); return n % 2 == 0; })
            .Select(n => { Console.Write($"[Select {n}] "); return n * 2; });
        
        Console.WriteLine("Query created - nothing executed yet!");
        
        Console.WriteLine("\nEnumerating query:");
        var result = query.ToList();  // Tutaj się wykonuje
        
        Console.WriteLine($"\nWynik: [{string.Join(", ", result)}]");
    }
}
