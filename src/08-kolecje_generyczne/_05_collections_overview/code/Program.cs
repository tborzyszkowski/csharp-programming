#!/usr/bin/env dotnet-script
// Demonstracja Kolekcji

using System;
using System.Collections.Generic;

namespace CollectionsDemo
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              Przegląd Kolekcji - Demonstracja             ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        DemonstrateList();
        DemonstrateDictionary();
        DemonstrateHashSet();
        DemonstrateQueue();
        DemonstrateStack();
        DemonstrateLinkedList();
        DemonstrateSortedSet();
    }
    
    private static void DemonstrateList()
    {
        Console.WriteLine("\n📌 List<T> - Dynamiczny Array\n");
        
        var list = new List<int> { 1, 2, 3 };
        list.Add(4);
        list.Insert(0, 0);
        
        Console.WriteLine("Lista: [" + string.Join(", ", list) + "]");
        Console.WriteLine($"Count: {list.Count}");
    }
    
    private static void DemonstrateDictionary()
    {
        Console.WriteLine("\n📌 Dictionary<K,V> - Klucz-Wartość\n");
        
        var dict = new Dictionary<string, int>
        {
            ["Alice"] = 25,
            ["Bob"] = 30,
            ["Charlie"] = 35
        };
        
        foreach (var kvp in dict)
            Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
    }
    
    private static void DemonstrateHashSet()
    {
        Console.WriteLine("\n📌 HashSet<T> - Unikalne Elementy\n");
        
        var set = new HashSet<int> { 1, 2, 3, 2, 1 };
        
        Console.WriteLine("Set (1, 2, 3, 2, 1): [" + string.Join(", ", set) + "]");
        Console.WriteLine($"Contains 2: {set.Contains(2)}");
    }
    
    private static void DemonstrateQueue()
    {
        Console.WriteLine("\n📌 Queue<T> - FIFO\n");
        
        var queue = new Queue<int>();
        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);
        
        Console.WriteLine("Dequeue: " + queue.Dequeue());
        Console.WriteLine("Peek: " + queue.Peek());
    }
    
    private static void DemonstrateStack()
    {
        Console.WriteLine("\n📌 Stack<T> - LIFO\n");
        
        var stack = new Stack<int>();
        stack.Push(1);
        stack.Push(2);
        stack.Push(3);
        
        Console.WriteLine("Pop: " + stack.Pop());
        Console.WriteLine("Peek: " + stack.Peek());
    }
    
    private static void DemonstrateLinkedList()
    {
        Console.WriteLine("\n📌 LinkedList<T> - Powiązana Lista\n");
        
        var linkedList = new LinkedList<int>();
        linkedList.AddLast(1);
        linkedList.AddLast(2);
        linkedList.AddFirst(0);
        
        Console.WriteLine("List: [" + string.Join(", ", linkedList) + "]");
    }
    
    private static void DemonstrateSortedSet()
    {
        Console.WriteLine("\n📌 SortedSet<T> - Posortowany Zbiór\n");
        
        var sortedSet = new SortedSet<int> { 3, 1, 2 };
        
        Console.WriteLine("Set (3, 1, 2): [" + string.Join(", ", sortedSet) + "]");
    }
}
