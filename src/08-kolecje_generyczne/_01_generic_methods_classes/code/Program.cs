#!/usr/bin/env dotnet-script
// Demonstracja Metod i Klas Generycznych

using System;
using System.Collections.Generic;
using System.Linq;

namespace GenericMethods
{
    // ========== METODY GENERYCZNE ==========
    
    public class GenericMethods
    {
        /// <summary>
        /// Metoda generyczna zwracająca pierwszy element tablicy
        /// </summary>
        public static T GetFirst<T>(T[] array)
        {
            return array.Length > 0 ? array[0] : default(T)!;
        }
        
        /// <summary>
        /// Metoda generyczna zamieniająca dwa elementy
        /// </summary>
        public static void Swap<T>(ref T a, ref T b)
        {
            (a, b) = (b, a);  // Używam dekonstrukcji (C# 7+)
        }
        
        /// <summary>
        /// Metoda generyczna konwertująca typ
        /// </summary>
        public static bool TryConvert<TInput, TOutput>(TInput input, out TOutput? result)
        {
            result = default(TOutput);
            
            try
            {
                if (input is TOutput converted)
                {
                    result = converted;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Metoda generyczna znajdujące indeks elementu
        /// </summary>
        public static int FindIndex<T>(T[] array, T item) where T : IEquatable<T>
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].Equals(item))
                    return i;
            }
            return -1;
        }
    }
    
    // ========== KLASY GENERYCZNE ==========
    
    /// <summary>
    /// Generyczny Stack (stos) - LIFO struktura danych
    /// </summary>
    public class Stack<T>
    {
        private T[] items;
        private int count;
        
        public Stack(int capacity = 10)
        {
            items = new T[capacity];
            count = 0;
        }
        
        public void Push(T item)
        {
            if (count == items.Length)
                Resize();
            items[count++] = item;
        }
        
        public T Pop()
        {
            if (count == 0)
                throw new InvalidOperationException("Stack is empty");
            return items[--count];
        }
        
        public T Peek()
        {
            if (count == 0)
                throw new InvalidOperationException("Stack is empty");
            return items[count - 1];
        }
        
        public bool IsEmpty => count == 0;
        public int Count => count;
        
        private void Resize()
        {
            T[] newItems = new T[items.Length * 2];
            Array.Copy(items, newItems, count);
            items = newItems;
        }
        
        public override string ToString()
        {
            return $"Stack<{typeof(T).Name}> with {count} items";
        }
    }
    
    /// <summary>
    /// Generyczny Repository - przechowuje obiekty
    /// </summary>
    public class Repository<T> where T : class
    {
        private List<T> items = new();
        
        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            items.Add(item);
        }
        
        public void Remove(T item) => items.Remove(item);
        
        public IEnumerable<T> GetAll() => items.AsReadOnly();
        
        public T? Get(Func<T, bool> predicate) => items.FirstOrDefault(predicate);
        
        public int Count => items.Count;
    }
    
    /// <summary>
    /// Generyczna klasa para (Pair) - przechowuje dwie wartości
    /// </summary>
    public class Pair<TFirst, TSecond>
    {
        public TFirst First { get; set; }
        public TSecond Second { get; set; }
        
        public Pair(TFirst first, TSecond second)
        {
            First = first;
            Second = second;
        }
        
        public override string ToString() => $"Pair({First}, {Second})";
    }
    
    /// <summary>
    /// Generyczna klasa Container z możliwością transformacji
    /// </summary>
    public class Container<T>
    {
        public T Value { get; private set; }
        
        public Container(T value)
        {
            Value = value;
        }
        
        /// <summary>
        /// Transformuje zawartość do innego typu
        /// </summary>
        public Container<TResult> Map<TResult>(Func<T, TResult> transform)
        {
            return new Container<TResult>(transform(Value));
        }
        
        /// <summary>
        /// Filtruje wartość
        /// </summary>
        public Container<T>? Filter(Func<T, bool> predicate)
        {
            return predicate(Value) ? this : null;
        }
        
        public override string ToString() => $"Container<{typeof(T).Name}>({Value})";
    }
    
    // ========== WARIANCJA TYPÓW ==========
    
    // Kowariantność - out
    public interface IProducer<out T>
    {
        T Produce();
    }
    
    public class DogProducer : IProducer<Dog>
    {
        public Dog Produce() => new Dog("Buddy", 5);
    }
    
    // Kontrawariantność - in
    public interface IConsumer<in T>
    {
        void Consume(T item);
    }
    
    public class AnimalConsumer : IConsumer<Animal>
    {
        public void Consume(Animal animal)
        {
            Console.WriteLine($"Consuming: {animal.GetDescription()}");
        }
    }
    
    // ========== KLASY HELPER ==========
    
    public class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        
        public Animal(string name, int age)
        {
            Name = name;
            Age = age;
        }
        
        public virtual string GetDescription() => $"{Name} (age {Age})";
    }
    
    public class Dog : Animal
    {
        public Dog(string name, int age) : base(name, age) { }
        
        public override string GetDescription() => $"🐕 Dog: {base.GetDescription()}";
    }
    
    public class Cat : Animal
    {
        public Cat(string name, int age) : base(name, age) { }
        
        public override string GetDescription() => $"🐈 Cat: {base.GetDescription()}";
    }
    
    // ========== ENTRY POINT ==========
    
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     Metody i Klasy Generyczne - Demonstracja              ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        DemonstrateGenericMethods();
        DemonstrateStack();
        DemonstrateRepository();
        DemonstratePair();
        DemonstrateContainer();
        DemonstrateVariance();
    }
    
    private static void DemonstrateGenericMethods()
    {
        Console.WriteLine("\n📌 METODY GENERYCZNE\n");
        
        // GetFirst
        int[] numbers = { 1, 2, 3, 4, 5 };
        var firstNum = GenericMethods.GetFirst(numbers);
        Console.WriteLine($"First number: {firstNum}");
        
        string[] names = { "Alice", "Bob", "Charlie" };
        var firstName = GenericMethods.GetFirst(names);
        Console.WriteLine($"First name: {firstName}");
        
        // Swap
        int a = 10, b = 20;
        Console.WriteLine($"\nBefore swap: a={a}, b={b}");
        GenericMethods.Swap(ref a, ref b);
        Console.WriteLine($"After swap: a={a}, b={b}");
        
        // FindIndex
        int index = GenericMethods.FindIndex(numbers, 3);
        Console.WriteLine($"\nIndex of 3: {index}");
    }
    
    private static void DemonstrateStack()
    {
        Console.WriteLine("\n\n📌 STOS (STACK<T>)\n");
        
        Stack<int> stack = new();
        
        Console.WriteLine("Pushing: 10, 20, 30");
        stack.Push(10);
        stack.Push(20);
        stack.Push(30);
        
        Console.WriteLine($"Stack state: {stack}");
        Console.WriteLine($"Peek: {stack.Peek()}");
        
        Console.WriteLine("\nPopping:");
        while (!stack.IsEmpty)
        {
            Console.WriteLine($"  Pop: {stack.Pop()}");
        }
    }
    
    private static void DemonstrateRepository()
    {
        Console.WriteLine("\n\n📌 REPOSITORY<T>\n");
        
        Repository<Dog> dogRepository = new();
        
        dogRepository.Add(new Dog("Buddy", 5));
        dogRepository.Add(new Dog("Max", 3));
        dogRepository.Add(new Dog("Luna", 7));
        
        Console.WriteLine($"Total dogs: {dogRepository.Count}");
        
        Console.WriteLine("\nAll dogs:");
        foreach (var dog in dogRepository.GetAll())
        {
            Console.WriteLine($"  - {dog.GetDescription()}");
        }
        
        var maxDog = dogRepository.Get(d => d.Name == "Max");
        Console.WriteLine($"\nFound: {maxDog?.GetDescription()}");
    }
    
    private static void DemonstratePair()
    {
        Console.WriteLine("\n\n📌 PAIR<T, U>\n");
        
        var stringIntPair = new Pair<string, int>("Age", 25);
        Console.WriteLine($"Pair 1: {stringIntPair}");
        
        var animalDogPair = new Pair<string, Dog>("Buddy", new Dog("Buddy", 5));
        Console.WriteLine($"Pair 2: {animalDogPair}");
    }
    
    private static void DemonstrateContainer()
    {
        Console.WriteLine("\n\n📌 CONTAINER<T> - TRANSFORMACJE\n");
        
        var container = new Container<int>(42);
        Console.WriteLine($"Original: {container}");
        
        // Transformacja int -> string
        var stringContainer = container.Map(x => $"Value is {x}");
        Console.WriteLine($"After Map: {stringContainer}");
        
        // Transformacja string -> int
        var lenContainer = container.Map(x => x.ToString().Length);
        Console.WriteLine($"After Map to length: {lenContainer}");
        
        // Filtrowanie
        var filtered = container.Filter(x => x > 40);
        Console.WriteLine($"Filtered (>40): {filtered?.Value}");
        
        filtered = container.Filter(x => x < 40);
        Console.WriteLine($"Filtered (<40): {filtered?.Value ?? "null"}");
    }
    
    private static void DemonstrateVariance()
    {
        Console.WriteLine("\n\n📌 WARIANCJA TYPÓW\n");
        
        // Kowariantność - out
        Console.WriteLine("Kowariantność (out):");
        IProducer<Dog> dogProducer = new DogProducer();
        IProducer<Animal> animalProducer = dogProducer;  // ✅ Działa!
        var dog = animalProducer.Produce();
        Console.WriteLine($"  Produced: {dog.GetDescription()}");
        
        // Kontrawariantność - in
        Console.WriteLine("\nKontrawariantność (in):");
        IConsumer<Animal> animalConsumer = new AnimalConsumer();
        IConsumer<Dog> dogConsumer = animalConsumer;  // ✅ Działa!
        dogConsumer.Consume(new Dog("Buddy", 3));
    }
}
