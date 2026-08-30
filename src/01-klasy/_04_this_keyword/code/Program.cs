using System;
using Xunit;

namespace ThisKeyword;

/// <summary>
/// TEMAT 4: Słowo Kluczowe this
/// Demonstruje zastosowania this w C#
/// </summary>

// Przykład 1: Rozróżnienie pól od parametrów
public class Person
{
    private string name;
    private int age;
    
    public Person(string name, int age)
    {
        this.name = name;  // this.name = pole klasy
        this.age = age;    // age = parametr konstruktora
    }
    
    public void UpdateInfo(string name, int age)
    {
        this.name = name;  // Rozróżnia pole od parametru
        this.age = age;
    }
    
    public string Name => name;
    public int Age => age;
    
    public override string ToString() => $"{name}, {age}";
}

// Przykład 2: Łańcuch konstruktorów
public class Student
{
    private string name;
    private int id;
    private double gpa;
    
    public Student() : this("Unknown", 0, 0.0) { }
    
    public Student(string name) : this(name, 0, 0.0) { }
    
    public Student(string name, int id) : this(name, id, 0.0) { }
    
    public Student(string name, int id, double gpa)
    {
        this.name = name;
        this.id = id;
        this.gpa = gpa;
    }
    
    public string Name => name;
    public int Id => id;
    public double GPA => gpa;
    
    public override string ToString() => $"Student({name}, ID:{id}, GPA:{gpa:F2})";
}

// Przykład 3: Fluent API - zwracanie this
public class StringBuilder
{
    private string content = "";
    
    public StringBuilder Append(string text)
    {
        content += text;
        return this;  // Zwraca bieżący obiekt
    }
    
    public StringBuilder AppendLine(string text = "")
    {
        content += text + Environment.NewLine;
        return this;  // Pozwala na łańcuchowanie
    }
    
    public string Build() => content;
    
    public override string ToString() => content;
}

// Przykład 4: Metoda przyjmująca delegate
public class Employee
{
    public string Name { get; set; }
    public decimal Salary { get; set; }
    
    public Employee(string name, decimal salary)
    {
        Name = name;
        Salary = salary;
    }
    
    public void ProcessWithDelegate(Action<Employee> action)
    {
        action(this);  // Przekazuje bieżący obiekt
    }
    
    public override string ToString() => $"{Name}: {Salary:C}";
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  SŁOWO KLUCZOWE this                                  ║");
        Console.WriteLine("║  Referencja do bieżącego obiektu                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        DemonstrateFI eldVsParameter();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateConstructorChaining();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateFluentAPI();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateDelegateCallback();
    }
    
    private static void DemonstrateFI eldVsParameter()
    {
        Console.WriteLine("🔹 this - Rozróżnienie Pól od Parametrów");
        Console.WriteLine("──────────────────────────────────────────\n");
        
        var person = new Person("Jan", 30);
        Console.WriteLine($"Utworzony: {person}");
        
        person.UpdateInfo("Maria", 28);
        Console.WriteLine($"Po aktualizacji: {person}");
    }
    
    private static void DemonstrateConstructorChaining()
    {
        Console.WriteLine("🔗 this - Łańcuch Konstruktorów");
        Console.WriteLine("───────────────────────────────\n");
        
        var student1 = new Student();
        Console.WriteLine($"Student(): {student1}");
        
        var student2 = new Student("Anna");
        Console.WriteLine($"Student(name): {student2}");
        
        var student3 = new Student("Piotr", 123);
        Console.WriteLine($"Student(name, id): {student3}");
        
        var student4 = new Student("Marta", 456, 3.8);
        Console.WriteLine($"Student(name, id, gpa): {student4}");
    }
    
    private static void DemonstrateFluentAPI()
    {
        Console.WriteLine("⛓️  this - Fluent API (Łańcuchowanie Metod)");
        Console.WriteLine("──────────────────────────────────────────\n");
        
        var sb = new StringBuilder()
            .Append("Cześć ")
            .Append("Świecie")
            .AppendLine("!")
            .AppendLine("To jest fluent API")
            .Append("Każda metoda zwraca this");
        
        Console.WriteLine("Wynik:");
        Console.WriteLine(sb.Build());
    }
    
    private static void DemonstrateDelegateCallback()
    {
        Console.WriteLine("📞 this - Przekazywanie Referencji do Metody");
        Console.WriteLine("──────────────────────────────────────────────\n");
        
        var emp = new Employee("Jan Kowalski", 3000);
        
        Console.WriteLine("Pracownik:");
        emp.ProcessWithDelegate(e => Console.WriteLine($"  {e}"));
        
        Console.WriteLine("\nPodwyżka 20%:");
        emp.ProcessWithDelegate(e =>
        {
            e.Salary *= 1.2m;
            Console.WriteLine($"  Nowa pensja: {e.Salary:C}");
        });
    }
}

/// ============================================
/// TESTY
/// ============================================

public class ThisKeywordTests
{
    [Fact]
    public void ThisDisambiguatesFieldFromParameter()
    {
        var person = new Person("Jan", 30);
        Assert.Equal("Jan", person.Name);
        Assert.Equal(30, person.Age);
    }
    
    [Fact]
    public void ConstructorChainingInitializesAllFields()
    {
        var student = new Student("Anna", 123, 3.8);
        Assert.Equal("Anna", student.Name);
        Assert.Equal(123, student.Id);
        Assert.Equal(3.8, student.GPA);
    }
    
    [Fact]
    public void DefaultConstructorUsesChaining()
    {
        var student = new Student();
        Assert.Equal("Unknown", student.Name);
        Assert.Equal(0, student.Id);
        Assert.Equal(0.0, student.GPA);
    }
    
    [Fact]
    public void FluentAPIReturnsThis()
    {
        var result = new StringBuilder()
            .Append("Hello")
            .Append(" ")
            .Append("World")
            .Build();
        
        Assert.Equal("Hello World", result);
    }
}
