using System;
using Xunit;

namespace UML;

// Klasy demonstrujące UML

public class Person
{
    private string name;
    private int age;
    
    public Person(string name, int age)
    {
        this.name = name;
        this.age = age;
    }
    
    public string Name => name;
    public int Age => age;
    
    public override string ToString() => $"{name}, {age}";
}

public class Employee : Person
{
    private decimal salary;
    
    public Employee(string name, int age, decimal salary) : base(name, age)
    {
        this.salary = salary;
    }
    
    public decimal Salary => salary;
    
    public override string ToString() => $"{base.ToString()} - {salary:C}";
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  UML - MODELOWANIE SYSTEMÓW                           ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        var person = new Person("Jan", 30);
        var emp = new Employee("Maria", 28, 3000);
        
        Console.WriteLine($"Person: {person}");
        Console.WriteLine($"Employee: {emp}");
    }
}

/// ============================================
/// TESTY
/// ============================================

public class UMLTests
{
    [Fact]
    public void Person_HasNameAndAge()
    {
        var person = new Person("Anna", 25);
        Assert.Equal("Anna", person.Name);
        Assert.Equal(25, person.Age);
    }
    
    [Fact]
    public void Employee_InheritsFromPerson()
    {
        var emp = new Employee("John", 30, 5000);
        Assert.Equal("John", emp.Name);
        Assert.Equal(30, emp.Age);
        Assert.Equal(5000, emp.Salary);
    }
}
