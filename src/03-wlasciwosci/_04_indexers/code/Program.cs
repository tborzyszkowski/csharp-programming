using System;
using System.Collections.Generic;
using Xunit;

namespace Indexers;

// ============ SIMPLE INDEXER ============
public class SimpleCollection
{
    private string[] items = new string[3];
    
    public string this[int index]
    {
        get { return items[index]; }
        set { items[index] = value; }
    }
}

// ============ VALIDATED INDEXER ============
public class ValidatedCollection
{
    private List<string> items = new();
    
    public string this[int index]
    {
        get
        {
            if (index < 0 || index >= items.Count)
                throw new IndexOutOfRangeException();
            return items[index];
        }
        set
        {
            if (index < 0)
                throw new IndexOutOfRangeException();
            while (items.Count <= index)
                items.Add(string.Empty);
            items[index] = value;
        }
    }
    
    public void Add(string item) => items.Add(item);
}

// ============ STRING INDEXER ============
public class Person
{
    private string _name = string.Empty;
    private int _age;
    private string _email = string.Empty;
    
    public object? this[string propertyName]
    {
        get => propertyName switch
        {
            "name" => _name,
            "age" => _age,
            "email" => _email,
            _ => null
        };
        set
        {
            switch (propertyName)
            {
                case "name":
                    _name = (string?)value ?? string.Empty;
                    break;
                case "age":
                    _age = (int?)value ?? 0;
                    break;
                case "email":
                    _email = (string?)value ?? string.Empty;
                    break;
            }
        }
    }
}

// ============ MULTIDIMENSIONAL INDEXER ============
public class Matrix
{
    private int[,] data = new int[3, 3];
    
    public int this[int row, int col]
    {
        get { return data[row, col]; }
        set { data[row, col] = value; }
    }
}

// ============ DICTIONARY-LIKE INDEXER ============
public class Phonebook
{
    private Dictionary<string, string> entries = new();
    
    public string? this[string name]
    {
        get => entries.ContainsKey(name) ? entries[name] : null;
        set
        {
            if (value != null)
                entries[name] = value;
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 4: INDEKSATORY ===\n");
        
        Console.WriteLine("1. PROSTY INDEKSATOR:");
        var collection = new SimpleCollection();
        collection[0] = "Apple";
        collection[1] = "Banana";
        collection[2] = "Cherry";
        Console.WriteLine($"collection[0]: {collection[0]}");
        Console.WriteLine();
        
        Console.WriteLine("2. WALIDOWANY INDEKSATOR:");
        var validated = new ValidatedCollection();
        validated.Add("First");
        validated[1] = "Second";
        Console.WriteLine($"validated[1]: {validated[1]}");
        try
        {
            var _ = validated[100];
        }
        catch (IndexOutOfRangeException)
        {
            Console.WriteLine("Exception: Index out of range");
        }
        Console.WriteLine();
        
        Console.WriteLine("3. INDEKSATOR STRING:");
        var person = new Person();
        person["name"] = "John";
        person["age"] = 30;
        person["email"] = "john@example.com";
        Console.WriteLine($"name: {person["name"]}");
        Console.WriteLine($"age: {person["age"]}");
        Console.WriteLine();
        
        Console.WriteLine("4. INDEKSATOR WIELOWYMIAROWY:");
        var matrix = new Matrix();
        matrix[0, 0] = 1;
        matrix[0, 1] = 2;
        matrix[1, 0] = 3;
        matrix[1, 1] = 4;
        Console.WriteLine($"matrix[0, 0]: {matrix[0, 0]}");
        Console.WriteLine($"matrix[1, 1]: {matrix[1, 1]}");
        Console.WriteLine();
        
        Console.WriteLine("5. PHONEBOOK (DICTIONARY-LIKE):");
        var phonebook = new Phonebook();
        phonebook["Alice"] = "123-456-789";
        phonebook["Bob"] = "987-654-321";
        Console.WriteLine($"Alice: {phonebook["Alice"]}");
        Console.WriteLine($"Charlie (not found): {phonebook["Charlie"] ?? "N/A"}");
    }
}

public class IndexersTests
{
    [Fact]
    public void SimpleIndexer_StoresAndRetrieves()
    {
        var collection = new SimpleCollection();
        collection[0] = "Test";
        
        Assert.Equal("Test", collection[0]);
    }
    
    [Fact]
    public void ValidatedIndexer_ThrowsOnOutOfRange()
    {
        var collection = new ValidatedCollection();
        collection.Add("First");
        
        Assert.Throws<IndexOutOfRangeException>(() => _ = collection[100]);
    }
    
    [Fact]
    public void StringIndexer_AccessesByPropertyName()
    {
        var person = new Person();
        person["name"] = "John";
        person["age"] = 30;
        
        Assert.Equal("John", person["name"]);
        Assert.Equal(30, person["age"]);
    }
    
    [Fact]
    public void MultidimensionalIndexer_Works()
    {
        var matrix = new Matrix();
        matrix[0, 0] = 5;
        matrix[2, 2] = 9;
        
        Assert.Equal(5, matrix[0, 0]);
        Assert.Equal(9, matrix[2, 2]);
    }
    
    [Fact]
    public void PhonebookIndexer_StoresAndRetrieves()
    {
        var phonebook = new Phonebook();
        phonebook["Alice"] = "123-456";
        
        Assert.Equal("123-456", phonebook["Alice"]);
        Assert.Null(phonebook["Unknown"]);
    }
}
