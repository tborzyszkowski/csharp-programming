using System;
using Xunit;

namespace PartialClasses;

// Część 1: Definicja podstawowa
public partial class Employee
{
    private string name;
    private int id;
    private decimal salary;
    
    public string Name => name;
    public int Id => id;
    public decimal Salary => salary;
    
    public Employee(string name, int id, decimal salary)
    {
        this.name = name;
        this.id = id;
        this.salary = salary;
    }
}

// Część 2: Logika biznesowa
public partial class Employee
{
    public void GiveRaise(decimal amount)
    {
        if (amount > 0)
            salary += amount;
    }
    
    public bool IsHighEarner() => salary >= 5000;
}

// Część 3: Walidacja
public partial class Employee
{
    public bool IsValid() => !string.IsNullOrEmpty(name) && salary >= 0;
    
    public override string ToString() => $"{name} (ID:{id}) - {salary:C}";
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  KLASY CZĘŚCIOWE (PARTIAL CLASSES)                    ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        var emp = new Employee("Jan Kowalski", 123, 3000);
        Console.WriteLine($"Pracownik: {emp}");
        Console.WriteLine($"Ważny: {emp.IsValid()}");
        
        emp.GiveRaise(500);
        Console.WriteLine($"Po podwyżce: {emp}");
        Console.WriteLine($"High Earner: {emp.IsHighEarner()}");
    }
}

/// ============================================
/// TESTY
/// ============================================

public class PartialClassesTests
{
    [Fact]
    public void PartialClass_CombinesAllParts()
    {
        var emp = new Employee("Anna", 456, 4000);
        
        Assert.Equal("Anna", emp.Name);
        Assert.Equal(456, emp.Id);
        Assert.Equal(4000, emp.Salary);
    }
    
    [Fact]
    public void PartialClass_AllMethodsWork()
    {
        var emp = new Employee("John", 789, 3000);
        
        Assert.True(emp.IsValid());
        emp.GiveRaise(1000);
        Assert.Equal(4000, emp.Salary);
    }
}
