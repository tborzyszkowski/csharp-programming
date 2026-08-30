#!/usr/bin/env dotnet-script
// Demonstracja Interfejsów Porównania

using System;
using System.Collections.Generic;
using System.Linq;

namespace ComparisonDemo
{
    // ========== KLASY DANYCH ==========
    
    public class Person : IComparable<Person>, IEquatable<Person>
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
        
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        // Naturalne porównanie - po nazwie
        public int CompareTo(Person? other)
        {
            if (other is null) return 1;
            return this.Name.CompareTo(other.Name);
        }
        
        public bool Equals(Person? other)
        {
            if (other is null) return false;
            return this.Name == other.Name && this.Age == other.Age;
        }
        
        public override bool Equals(object? obj) => Equals(obj as Person);
        public override int GetHashCode() => HashCode.Combine(Name, Age);
        public override string ToString() => $"{Name} ({Age})";
    }
    
    // ========== COMPARERS ==========
    
    public class PersonAgeComparer : IComparer<Person>
    {
        public int Compare(Person? x, Person? y)
        {
            if (x is null || y is null) return 0;
            return x.Age.CompareTo(y.Age);
        }
    }
    
    public class PersonNameDescendingComparer : IComparer<Person>
    {
        public int Compare(Person? x, Person? y)
        {
            if (x is null || y is null) return 0;
            return y.Name.CompareTo(x.Name);  // Odwrócone!
        }
    }
    
    // ========== GENERIC COMPARER ==========
    
    public static class Comparers
    {
        public static IComparer<T> Create<T, TKey>(Func<T, TKey> keySelector)
            where TKey : IComparable<TKey>
        {
            return new GenericComparer<T, TKey>(keySelector);
        }
        
        private class GenericComparer<T, TKey> : IComparer<T>
            where TKey : IComparable<TKey>
        {
            private Func<T, TKey> keySelector;
            
            public GenericComparer(Func<T, TKey> keySelector)
            {
                this.keySelector = keySelector;
            }
            
            public int Compare(T? x, T? y)
            {
                if (x is null || y is null) return 0;
                return keySelector(x).CompareTo(keySelector(y));
            }
        }
    }
    
    // ========== ENTRY POINT ==========
    
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     Interfejsy Porównania - Demonstracja                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        DemonstrateIComparable();
        DemonstrateIComparer();
        DemonstrateLambdaComparison();
        DemonstrateGenericComparer();
    }
    
    private static void DemonstrateIComparable()
    {
        Console.WriteLine("\n📌 IComparable<T> - Naturalne Porównanie\n");
        
        var people = new List<Person>
        {
            new("Charlie", 30),
            new("Alice", 25),
            new("Bob", 35)
        };
        
        Console.WriteLine("Przed sortowaniem:");
        foreach (var p in people)
            Console.WriteLine($"  {p}");
        
        people.Sort();  // Używa CompareTo - sortuje po nazwie
        
        Console.WriteLine("\nPo sortowaniu (po nazwie):");
        foreach (var p in people)
            Console.WriteLine($"  {p}");
    }
    
    private static void DemonstrateIComparer()
    {
        Console.WriteLine("\n📌 IComparer<T> - Elastyczne Porównanie\n");
        
        var people = new List<Person>
        {
            new("Charlie", 30),
            new("Alice", 25),
            new("Bob", 35)
        };
        
        Console.WriteLine("Sortuj po wieku (rosnąco):");
        people.Sort(new PersonAgeComparer());
        foreach (var p in people)
            Console.WriteLine($"  {p}");
        
        Console.WriteLine("\nSortuj po nazwie (malejąco):");
        people.Sort(new PersonNameDescendingComparer());
        foreach (var p in people)
            Console.WriteLine($"  {p}");
    }
    
    private static void DemonstrateLambdaComparison()
    {
        Console.WriteLine("\n📌 Lambda Comparison\n");
        
        var people = new List<Person>
        {
            new("Charlie", 30),
            new("Alice", 25),
            new("Bob", 35)
        };
        
        Console.WriteLine("Sortuj po wieku (lambda):");
        people.Sort((x, y) => x.Age.CompareTo(y.Age));
        foreach (var p in people)
            Console.WriteLine($"  {p}");
        
        Console.WriteLine("\nSortuj po wieku (malejąco):");
        people.Sort((x, y) => y.Age.CompareTo(x.Age));
        foreach (var p in people)
            Console.WriteLine($"  {p}");
    }
    
    private static void DemonstrateGenericComparer()
    {
        Console.WriteLine("\n📌 Generic Comparer Helper\n");
        
        var people = new List<Person>
        {
            new("Charlie", 30),
            new("Alice", 25),
            new("Bob", 35)
        };
        
        Console.WriteLine("Sortuj po nazwie (generic comparer):");
        people.Sort(Comparers.Create<Person, string>(p => p.Name));
        foreach (var p in people)
            Console.WriteLine($"  {p}");
        
        Console.WriteLine("\nSortuj po wieku (generic comparer):");
        people.Sort(Comparers.Create<Person, int>(p => p.Age));
        foreach (var p in people)
            Console.WriteLine($"  {p}");
    }
}
