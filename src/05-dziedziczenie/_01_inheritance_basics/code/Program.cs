using System;
using System.Collections.Generic;
using Xunit;

namespace InheritanceBasics;

// Klasa bazowa
public class Animal
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    
    public void Eat() => Console.WriteLine($"{Name} is eating");
    public void Sleep() => Console.WriteLine($"{Name} is sleeping");
}

// Klasa pochodna
public class Dog : Animal
{
    public void Bark() => Console.WriteLine($"{Name} says: Woof!");
}

public class Cat : Animal
{
    public void Meow() => Console.WriteLine($"{Name} says: Meow!");
}

public class Bird : Animal
{
    public void Sing() => Console.WriteLine($"{Name} is singing");
}

// Łańcuch dziedziczenia
public class Mammal : Animal
{
    public bool IsWarmBlooded { get; set; } = true;
}

public class Puppy : Dog
{
    public void Play() => Console.WriteLine($"Little {Name} is playing");
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== PODSTAWOWE POJĘCIA DZIEDZICZENIA ===\n");
        
        Console.WriteLine("1. Klasa pochodna dziedziczy z bazowej:");
        var dog = new Dog { Name = "Rex", Age = 3 };
        dog.Eat();     // Z Animal
        dog.Sleep();   // Z Animal
        dog.Bark();    // Własna metoda
        Console.WriteLine();
        
        Console.WriteLine("2. Polimorfizm - wiele typów zwierząt:");
        List<Animal> animals = new()
        {
            new Dog { Name = "Rex" },
            new Cat { Name = "Whiskers" },
            new Bird { Name = "Tweety" }
        };
        
        foreach (var animal in animals)
        {
            animal.Eat();
        }
        Console.WriteLine();
        
        Console.WriteLine("3. Łańcuch dziedziczenia:");
        var puppy = new Puppy { Name = "Buddy", Age = 1 };
        puppy.Eat();      // Z Animal
        puppy.Sleep();    // Z Animal
        puppy.Bark();     // Z Dog
        puppy.Play();     // Własna metoda
        Console.WriteLine();
        
        Console.WriteLine("4. Typ is-a relacja:");
        Console.WriteLine($"dog is Animal: {dog is Animal}");
        Console.WriteLine($"dog is Dog: {dog is Dog}");
        Console.WriteLine($"dog is Cat: {dog is Cat}");
    }
}

public class InheritanceBasicsTests
{
    [Fact]
    public void Dog_Inherits_From_Animal()
    {
        var dog = new Dog { Name = "Rex" };
        
        Assert.IsAssignableFrom<Animal>(dog);
    }
    
    [Fact]
    public void Dog_HasInheritedProperties()
    {
        var dog = new Dog { Name = "Rex", Age = 3 };
        
        Assert.Equal("Rex", dog.Name);
        Assert.Equal(3, dog.Age);
    }
    
    [Fact]
    public void DerivedClass_CanCallBaseClassMethods()
    {
        var dog = new Dog { Name = "Rex" };
        
        // Should not throw
        dog.Eat();
        dog.Sleep();
    }
    
    [Fact]
    public void Puppy_InheritsFromDog_And_Animal()
    {
        var puppy = new Puppy { Name = "Buddy" };
        
        Assert.IsAssignableFrom<Dog>(puppy);
        Assert.IsAssignableFrom<Animal>(puppy);
    }
    
    [Fact]
    public void IsOperator_ChecksInheritance()
    {
        var dog = new Dog { Name = "Rex" };
        var cat = new Cat { Name = "Whiskers" };
        
        Assert.True(dog is Animal);
        Assert.True(dog is Dog);
        Assert.False(dog is Cat);
        Assert.False(cat is Dog);
    }
}
