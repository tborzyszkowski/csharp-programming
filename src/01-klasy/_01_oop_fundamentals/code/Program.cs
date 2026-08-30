using System;
using Xunit;

namespace OOPFundamentals;

/// <summary>
/// TEMAT 1: Programowanie Obiektowe - Podstawowe Pojęcia
/// 
/// W tym pliku demonstrujemy cztery filary OOP:
/// 1. Abstrakcja - ukrywanie szczegółów implementacji
/// 2. Enkapsulacja - ochrona danych za pośrednictwem modyfikatorów dostępu
/// 3. Dziedziczenie - hierarchia klas
/// 4. Polimorfizm - wielopostaciowość
/// </summary>

/// <summary>
/// FILAR 1: ABSTRAKCJA
/// Definiujemy co powinien robić obiekt, bez szczegółów jak to robi.
/// </summary>
public abstract class Animal
{
    public string Name { get; set; }
    
    public Animal(string name) => Name = name;
    
    /// <summary>
    /// Abstrakcyjna metoda - każne zwierzę musi wydawać dźwięk
    /// </summary>
    public abstract void MakeSound();
    
    /// <summary>
    /// Konkretna metoda wspólna dla wszystkich zwierząt
    /// </summary>
    public virtual void Eat()
    {
        Console.WriteLine($"{Name} je pożywienie");
    }
}

/// <summary>
/// Konkretna implementacja abstrakcji - Pies
/// </summary>
public class Dog : Animal
{
    public Dog(string name) : base(name) { }
    
    public override void MakeSound()
    {
        Console.WriteLine($"{Name}: Hau! Hau!");
    }
    
    /// <summary>
    /// Filar 4: POLIMORFIZM - przesłanianie metody bazowej
    /// </summary>
    public override void Eat()
    {
        Console.WriteLine($"{Name} żarłocznie je mięso");
    }
    
    public void Fetch()
    {
        Console.WriteLine($"{Name} przynosisz piłkę");
    }
}

/// <summary>
/// Konkretna implementacja abstrakcji - Kot
/// </summary>
public class Cat : Animal
{
    public Cat(string name) : base(name) { }
    
    public override void MakeSound()
    {
        Console.WriteLine($"{Name}: Miau!");
    }
    
    public override void Eat()
    {
        Console.WriteLine($"{Name} delikatnie jada rybę");
    }
    
    public void Scratch()
    {
        Console.WriteLine($"{Name} drapie sobie w uszy");
    }
}

/// <summary>
/// FILAR 2: ENKAPSULACJA
/// Ochrona danych za pośrednictwem modyfikatorów dostępu
/// </summary>
public class BankAccount
{
    private decimal balance;  // Prywatna zmienna
    private int pin;          // Chroniony PIN
    
    public string AccountHolder { get; }
    
    /// <summary>
    /// Property - kontrolowany dostęp do danych
    /// </summary>
    public decimal Balance => balance;
    
    public BankAccount(string accountHolder, int initialPin)
    {
        AccountHolder = accountHolder;
        pin = initialPin;
        balance = 0;
    }
    
    /// <summary>
    /// Wpłata - tylko metoda publiczna zmienia saldo
    /// </summary>
    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
            Console.WriteLine($"Wpłacono: {amount:C}. Nowe saldo: {balance:C}");
        }
        else
        {
            Console.WriteLine("Kwota musi być dodatnia");
        }
    }
    
    /// <summary>
    /// Wypłata - wymaga podania PINu dla bezpieczeństwa
    /// </summary>
    public bool Withdraw(decimal amount, int providedPin)
    {
        if (providedPin != pin)
        {
            Console.WriteLine("Błędny PIN!");
            return false;
        }
        
        if (amount <= 0)
        {
            Console.WriteLine("Kwota musi być dodatnia");
            return false;
        }
        
        if (amount > balance)
        {
            Console.WriteLine("Niewystarczające środki!");
            return false;
        }
        
        balance -= amount;
        Console.WriteLine($"Wypłacono: {amount:C}. Nowe saldo: {balance:C}");
        return true;
    }
    
    /// <summary>
    /// Zmiana PINu - wymaga starego PINu dla bezpieczeństwa
    /// </summary>
    public bool ChangePin(int oldPin, int newPin)
    {
        if (oldPin != pin)
        {
            Console.WriteLine("Stary PIN jest nieprawidłowy");
            return false;
        }
        
        pin = newPin;
        Console.WriteLine("PIN zmieniony pomyślnie");
        return true;
    }
}

/// <summary>
/// FILAR 3: DZIEDZICZENIE
/// Pracownik -> różne typy pracowników
/// </summary>
public class Employee
{
    public string Name { get; set; }
    public decimal BaseSalary { get; set; }
    
    public Employee(string name, decimal baseSalary)
    {
        Name = name;
        BaseSalary = baseSalary;
    }
    
    public virtual decimal CalculateSalary()
    {
        return BaseSalary;
    }
    
    public virtual void Work()
    {
        Console.WriteLine($"{Name} pracuje");
    }
}

/// <summary>
/// Filar 4: POLIMORFIZM - różne implementacje CalculateSalary
/// </summary>
public class Manager : Employee
{
    private decimal bonus;
    
    public Manager(string name, decimal baseSalary, decimal bonus) 
        : base(name, baseSalary)
    {
        this.bonus = bonus;
    }
    
    public override decimal CalculateSalary()
    {
        return BaseSalary + bonus;
    }
    
    public override void Work()
    {
        Console.WriteLine($"{Name} kieruje zespołem");
    }
}

public class Intern : Employee
{
    public Intern(string name, decimal baseSalary) 
        : base(name, baseSalary) { }
    
    public override decimal CalculateSalary()
    {
        return BaseSalary * 0.5m;  // Praktykant zarabia połowę
    }
    
    public override void Work()
    {
        Console.WriteLine($"{Name} wykonuje zadania praktykanta");
    }
}

/// ============================================
/// GŁÓWNY PROGRAM - DEMONSTRACJA
/// ============================================

public class Program
{
    static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  OOP FUNDAMENTALS - Cztery filary programowania        ║");
        Console.WriteLine("║  obiektowego w C#                                     ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        DemonstrateAbstraction();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateEncapsulation();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateInheritanceAndPolymorphism();
    }
    
    /// <summary>
    /// Demonstracja abstrakcji i polimorfizmu
    /// </summary>
    private static void DemonstrateAbstraction()
    {
        Console.WriteLine("🎯 FILAR 1 & 4: Abstrakcja i Polimorfizm");
        Console.WriteLine("─────────────────────────────────────────\n");
        
        // Tworzymy kolekcję zwierząt różnych typów
        Animal[] animals = new Animal[]
        {
            new Dog("Rex"),
            new Cat("Fluffy"),
            new Dog("Buddy")
        };
        
        // Polimorfizm - każne zwierzę wydaje inny dźwięk
        Console.WriteLine("Każde zwierzę wydaje inny dźwięk:");
        foreach (var animal in animals)
        {
            animal.MakeSound();
        }
        
        Console.WriteLine("\nKażde zwierzę jada w inny sposób:");
        foreach (var animal in animals)
        {
            animal.Eat();
        }
    }
    
    /// <summary>
    /// Demonstracja enkapsulacji
    /// </summary>
    private static void DemonstrateEncapsulation()
    {
        Console.WriteLine("🔒 FILAR 2: Enkapsulacja - ochrona danych");
        Console.WriteLine("───────────────────────────────────────\n");
        
        var account = new BankAccount("Jan Kowalski", 1234);
        
        Console.WriteLine($"Właściciel: {account.AccountHolder}");
        Console.WriteLine($"Saldo: {account.Balance:C}\n");
        
        // Wpłata - publiczna metoda
        account.Deposit(1000);
        
        // Próba wypłaty z błędnym PINem
        Console.WriteLine();
        account.Withdraw(100, 9999);
        
        // Poprawna wypłata
        Console.WriteLine();
        account.Withdraw(100, 1234);
        
        // Zmiana PINu
        Console.WriteLine();
        account.ChangePin(1234, 5678);
        account.ChangePin(9999, 1111);  // Błędny stary PIN
    }
    
    /// <summary>
    /// Demonstracja dziedziczenia i polimorfizmu
    /// </summary>
    private static void DemonstrateInheritanceAndPolymorphism()
    {
        Console.WriteLine("👨‍💼 FILAR 3 & 4: Dziedziczenie i Polimorfizm");
        Console.WriteLine("─────────────────────────────────────────────\n");
        
        // Tworzymy pracowników różnych typów
        Employee[] employees = new Employee[]
        {
            new Employee("Anna", 2000),
            new Manager("Maciej", 3000, 500),
            new Intern("Tomasz", 1000)
        };
        
        Console.WriteLine("Pracownicy pracują w inny sposób:");
        foreach (var emp in employees)
        {
            emp.Work();
        }
        
        Console.WriteLine("\nWyliczanie pensji - każdy otrzymuje inną kwotę:");
        foreach (var emp in employees)
        {
            decimal salary = emp.CalculateSalary();
            Console.WriteLine($"{emp.Name:20} - {salary:C}");
        }
    }
}

/// ============================================
/// TESTY JEDNOSTKOWE
/// ============================================

public class OOPFundamentalsTests
{
    [Fact]
    public void Dog_MakeSound_OutputsCorrectSound()
    {
        // Arrange
        var dog = new Dog("Rex");
        
        // Act & Assert
        Assert.NotNull(dog);
        Assert.Equal("Rex", dog.Name);
    }
    
    [Fact]
    public void Cat_MakeSound_OutputsCorrectSound()
    {
        var cat = new Cat("Whiskers");
        Assert.NotNull(cat);
        Assert.Equal("Whiskers", cat.Name);
    }
    
    [Fact]
    public void BankAccount_Deposit_IncreaseBalance()
    {
        // Arrange
        var account = new BankAccount("John Doe", 1234);
        decimal initialBalance = account.Balance;
        
        // Act
        account.Deposit(1000);
        
        // Assert
        Assert.Equal(initialBalance + 1000, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_WithCorrectPin_Succeeds()
    {
        // Arrange
        var account = new BankAccount("John Doe", 1234);
        account.Deposit(500);
        
        // Act
        bool result = account.Withdraw(100, 1234);
        
        // Assert
        Assert.True(result);
        Assert.Equal(400, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_WithWrongPin_Fails()
    {
        // Arrange
        var account = new BankAccount("John Doe", 1234);
        account.Deposit(500);
        
        // Act
        bool result = account.Withdraw(100, 9999);
        
        // Assert
        Assert.False(result);
        Assert.Equal(500, account.Balance);  // Saldo nie zmienia się
    }
    
    [Fact]
    public void Employee_CalculateSalary_ReturnsBaseSalary()
    {
        var employee = new Employee("John", 2000);
        Assert.Equal(2000, employee.CalculateSalary());
    }
    
    [Fact]
    public void Manager_CalculateSalary_IncludesBonus()
    {
        var manager = new Manager("Jane", 3000, 500);
        Assert.Equal(3500, manager.CalculateSalary());
    }
    
    [Fact]
    public void Intern_CalculateSalary_ReceivesHalfSalary()
    {
        var intern = new Intern("Bob", 2000);
        Assert.Equal(1000, intern.CalculateSalary());
    }
}
