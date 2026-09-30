using System;
using System.Collections.Generic;
using System.Linq;
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

// <<interface>> IPayable   (UML: relacja realizacji  Employee ..|> IPayable)
public interface IPayable
{
    decimal GetMonthlyPay();
}

// Dziedziczenie (UML: Employee --|> Person) + realizacja interfejsu (Employee ..|> IPayable)
public class Employee : Person, IPayable
{
    private decimal salary;
    
    public Employee(string name, int age, decimal salary) : base(name, age)
    {
        this.salary = salary;
    }
    
    public decimal Salary => salary;
    
    public decimal GetMonthlyPay() => salary;
    
    public override string ToString() => $"{base.ToString()} - {salary:C}";
}

// Agregacja (UML: Department o-- Employee): dział "ma" pracowników,
// ale pracownicy istnieją niezależnie od działu (mogą zostać przeniesieni).
public class Department
{
    private readonly List<Employee> employees = new();
    
    public string Name { get; }
    public IReadOnlyList<Employee> Employees => employees;
    
    public Department(string name) => Name = name;
    
    public void Add(Employee employee) => employees.Add(employee);
    
    // Zależność (UML: Department ..> IPayable) - korzysta z interfejsu tylko w metodzie
    public decimal TotalPayroll() => employees.Sum(e => ((IPayable)e).GetMonthlyPay());
}

// Kompozycja (UML: Car *-- Engine): Engine jest tworzony przez Car i nie ma sensu bez niego.
public class Engine
{
    public int Horsepower { get; }
    public Engine(int horsepower) => Horsepower = horsepower;
}

public class Car
{
    private readonly Engine engine;
    
    public Car(int horsepower) => engine = new Engine(horsepower);   // część ma żywotę właściciela
    
    public int Horsepower => engine.Horsepower;
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  UML - MODELOWANIE SYSTEMÓW                           ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        var person = new Person("Jan", 30);
        var emp = new Employee("Maria", 28, 3000);
        
        Console.WriteLine($"Person: {person}");
        Console.WriteLine($"Employee: {emp}");

        var department = new Department("IT");
        department.Add(emp);
        department.Add(new Employee("Piotr", 35, 4500));
        Console.WriteLine($"\nDział {department.Name}: {department.Employees.Count} pracowników, suma wypłat {department.TotalPayroll():C}");

        var car = new Car(150);
        Console.WriteLine($"Samochód z silnikiem {car.Horsepower} KM (kompozycja)");
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

    [Fact]
    public void Employee_RealizesInterface()
    {
        IPayable payable = new Employee("John", 30, 5000);

        Assert.Equal(5000, payable.GetMonthlyPay());
    }

    [Fact]
    public void Department_Aggregation_EmployeesOutliveDepartment()
    {
        var employee = new Employee("Anna", 28, 4000);
        var department = new Department("HR");
        department.Add(employee);

        Assert.Single(department.Employees);
        Assert.Equal(4000, department.TotalPayroll());

        // Obiekt Employee istnieje nadal, niezależnie od Department
        Assert.Equal("Anna", employee.Name);
    }

    [Fact]
    public void Car_Composition_CreatesOwnEngine()
    {
        var car = new Car(120);

        Assert.Equal(120, car.Horsepower);
    }
}
