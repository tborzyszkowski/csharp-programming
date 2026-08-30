#!/usr/bin/env dotnet-script
// Demonstracja IEnumerable i IEnumerator

using System;
using System.Collections;
using System.Collections.Generic;

namespace EnumerableDemo
{
    // ========== IMPLEMENTACJA RĘCZNA ==========
    
    public class ManualList<T> : IEnumerable<T>
    {
        private T[] items;
        private int count;
        
        public ManualList()
        {
            items = new T[10];
            count = 0;
        }
        
        public void Add(T item)
        {
            if (count == items.Length)
                Array.Resize(ref items, items.Length * 2);
            items[count++] = item;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return new ManualListEnumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        private class ManualListEnumerator : IEnumerator<T>
        {
            private ManualList<T> list;
            private int index = -1;
            
            public ManualListEnumerator(ManualList<T> list)
            {
                this.list = list;
            }
            
            public T Current => list.items[index];
            object IEnumerator.Current => Current!;
            
            public bool MoveNext()
            {
                index++;
                return index < list.count;
            }
            
            public void Reset() => index = -1;
            public void Dispose() { }
        }
        
        public override string ToString() => $"ManualList<{typeof(T).Name}> with {count} items";
    }
    
    // ========== IMPLEMENTACJA Z YIELD ==========
    
    public class YieldList<T> : IEnumerable<T>
    {
        private T[] items;
        private int count;
        
        public YieldList()
        {
            items = new T[10];
            count = 0;
        }
        
        public void Add(T item)
        {
            if (count == items.Length)
                Array.Resize(ref items, items.Length * 2);
            items[count++] = item;
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return items[i];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public override string ToString() => $"YieldList<{typeof(T).Name}> with {count} items";
    }
    
    // ========== RANGE ITERATOR ==========
    
    public class Range : IEnumerable<int>
    {
        private int start;
        private int end;
        
        public Range(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = start; i < end; i++)
            {
                yield return i;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public override string ToString() => $"Range({start}..{end})";
    }
    
    // ========== FIBONACCI SEQUENCE ==========
    
    public class Fibonacci : IEnumerable<long>
    {
        private int count;
        
        public Fibonacci(int count)
        {
            if (count < 0)
                throw new ArgumentException("Count must be non-negative", nameof(count));
            this.count = count;
        }
        
        public IEnumerator<long> GetEnumerator()
        {
            long a = 0, b = 1;
            for (int i = 0; i < count; i++)
            {
                yield return a;
                (a, b) = (b, a + b);
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    // ========== REVERSE ITERATOR ==========
    
    public class ReverseIterator<T> : IEnumerable<T>
    {
        private T[] items;
        
        public ReverseIterator(T[] items)
        {
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = items.Length - 1; i >= 0; i--)
            {
                yield return items[i];
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    // ========== FILTERED ITERATOR ==========
    
    public class FilteredList<T> : IEnumerable<T>
    {
        private List<T> items;
        private Func<T, bool> predicate;
        
        public FilteredList(IEnumerable<T> source, Func<T, bool> predicate)
        {
            this.items = new List<T>(source);
            this.predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in items)
            {
                if (predicate(item))
                    yield return item;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    // ========== INFINITE SEQUENCE ==========
    
    public class InfiniteSequence : IEnumerable<int>
    {
        private int start;
        private int step;
        
        public InfiniteSequence(int start = 0, int step = 1)
        {
            this.start = start;
            this.step = step;
        }
        
        public IEnumerator<int> GetEnumerator()
        {
            int current = start;
            while (true)
            {
                yield return current;
                current += step;
            }
        }
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
    // ========== MAIN PROGRAM ==========
    
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     IEnumerable i IEnumerator - Demonstracja              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        DemonstrateManualList();
        DemonstrateYieldList();
        DemonstrateRange();
        DemonstrateFibonacci();
        DemonstrateReverseIterator();
        DemonstrateFilteredList();
        DemonstrateInfiniteSequence();
    }
    
    private static void DemonstrateManualList()
    {
        Console.WriteLine("\n📌 MANUAL LIST (Ręczna Implementacja Enumeratora)\n");
        
        var list = new ManualList<int>();
        list.Add(10);
        list.Add(20);
        list.Add(30);
        
        Console.WriteLine(list);
        Console.WriteLine("Iteracja:");
        foreach (var item in list)
        {
            Console.WriteLine($"  {item}");
        }
    }
    
    private static void DemonstrateYieldList()
    {
        Console.WriteLine("\n📌 YIELD LIST (Z yield return)\n");
        
        var list = new YieldList<string>();
        list.Add("Alice");
        list.Add("Bob");
        list.Add("Charlie");
        
        Console.WriteLine(list);
        Console.WriteLine("Iteracja:");
        foreach (var item in list)
        {
            Console.WriteLine($"  {item}");
        }
    }
    
    private static void DemonstrateRange()
    {
        Console.WriteLine("\n📌 RANGE (Niestandardowy Zakres)\n");
        
        var range = new Range(1, 6);
        Console.WriteLine(range);
        Console.WriteLine("Iteracja:");
        foreach (var item in range)
        {
            Console.WriteLine($"  {item}");
        }
    }
    
    private static void DemonstrateFibonacci()
    {
        Console.WriteLine("\n📌 FIBONACCI (Sekwencja Fibonacciego)\n");
        
        var fib = new Fibonacci(8);
        Console.WriteLine("Pierwsze 8 liczb Fibonacciego:");
        foreach (var num in fib)
        {
            Console.Write($"  {num}");
        }
        Console.WriteLine();
    }
    
    private static void DemonstrateReverseIterator()
    {
        Console.WriteLine("\n📌 REVERSE ITERATOR (Odwrotna Iteracja)\n");
        
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var reversed = new ReverseIterator<int>(numbers);
        Console.WriteLine("Tablica: [1, 2, 3, 4, 5]");
        Console.WriteLine("Odwrócona iteracja:");
        foreach (var item in reversed)
        {
            Console.Write($"  {item}");
        }
        Console.WriteLine();
    }
    
    private static void DemonstrateFilteredList()
    {
        Console.WriteLine("\n📌 FILTERED LIST (Filtrowana Iteracja)\n");
        
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var evenNumbers = new FilteredList<int>(numbers, x => x % 2 == 0);
        
        Console.WriteLine("Liczby: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]");
        Console.WriteLine("Liczby parzyste:");
        foreach (var item in evenNumbers)
        {
            Console.Write($"  {item}");
        }
        Console.WriteLine();
    }
    
    private static void DemonstrateInfiniteSequence()
    {
        Console.WriteLine("\n📌 INFINITE SEQUENCE (Nieskończona Sekwencja - Pierwsze 5)\n");
        
        var infinite = new InfiniteSequence(start: 10, step: 5);
        Console.WriteLine("Sekwencja: 10, 15, 20, 25, ... (nieskończona)");
        Console.WriteLine("Pierwsze 5 elementów:");
        
        int count = 0;
        foreach (var item in infinite)
        {
            if (count++ >= 5)
                break;
            Console.Write($"  {item}");
        }
        Console.WriteLine();
    }
}
