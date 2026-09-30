using System;
using Xunit;

namespace PartialClasses;

// Klasa Employee jest rozłożona na trzy pliki w tym katalogu:
//   Employee.cs            - dane i konstruktor
//   Employee.Business.cs   - logika biznesowa
//   Employee.Validation.cs - walidacja i prezentacja
// Kompilator łączy je w jeden typ, więc Program widzi jedną klasę Employee.

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
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

    [Fact]
    public void PartialClass_NonPositiveRaise_IsIgnored()
    {
        var emp = new Employee("John", 789, 3000);

        emp.GiveRaise(-100);

        Assert.Equal(3000, emp.Salary);
    }

    [Fact]
    public void PartialClass_IsHighEarner_UsesThresholdFromBusinessPart()
    {
        Assert.False(new Employee("A", 1, 4999).IsHighEarner());
        Assert.True(new Employee("B", 2, 5000).IsHighEarner());
    }

    [Fact]
    public void PartialClass_AllPartsFormSingleType()
    {
        // Metody z różnych plików należą do tego samego typu w czasie wykonania
        Assert.NotNull(typeof(Employee).GetMethod("GiveRaise"));   // Employee.Business.cs
        Assert.NotNull(typeof(Employee).GetMethod("IsValid"));     // Employee.Validation.cs
        Assert.NotNull(typeof(Employee).GetProperty("Salary"));    // Employee.cs
    }
}
