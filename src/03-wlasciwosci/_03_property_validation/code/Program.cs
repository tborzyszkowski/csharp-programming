using System;
using Xunit;

namespace PropertyValidation;

// ============ THROW EXCEPTION ============
public class StrictPerson
{
    private int _age;
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentException("Age must be 0-150");
            _age = value;
        }
    }
}

// ============ SILENT IGNORE ============
public class SilentPerson
{
    private int _age;
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value >= 0 && value <= 150)
                _age = value;
            // Else: ignoruj
        }
    }
}

// ============ BACKING FIELD PATTERN ============
public class BankAccount
{
    private decimal _balance;
    
    public decimal Balance
    {
        get { return _balance; }
        private set { _balance = value; }
    }
    
    public bool Deposit(decimal amount)
    {
        if (amount > 0)
        {
            _balance += amount;
            return true;
        }
        return false;
    }
    
    public bool Withdraw(decimal amount)
    {
        if (amount > 0 && amount <= _balance)
        {
            _balance -= amount;
            return true;
        }
        return false;
    }
}

// ============ PROPERTY CHANGED EVENT ============
public class Person
{
    private string _name = string.Empty;
    
    public event Action<string, string>? PropertyChanged;
    
    public string Name
    {
        get { return _name; }
        set
        {
            if (value != _name)
            {
                PropertyChanged?.Invoke("Name", value);
                _name = value;
            }
        }
    }
}

// ============ EMAIL VALIDATION ============
public class User
{
    private string _email = string.Empty;
    
    public string Email
    {
        get { return _email; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty");
            if (!value.Contains("@"))
                throw new ArgumentException("Email must contain @");
            _email = value;
        }
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 3: WALIDACJA WŁAŚCIWOŚCI ===\n");
        
        Console.WriteLine("1. THROW EXCEPTION:");
        var strictPerson = new StrictPerson();
        strictPerson.Age = 30;
        Console.WriteLine($"Age: {strictPerson.Age}");
        try
        {
            strictPerson.Age = -5;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        Console.WriteLine();
        
        Console.WriteLine("2. SILENT IGNORE:");
        var silentPerson = new SilentPerson();
        silentPerson.Age = 30;
        Console.WriteLine($"Age: {silentPerson.Age}");
        silentPerson.Age = -5;  // Brak wyjątku
        Console.WriteLine($"Age after invalid set: {silentPerson.Age}");
        Console.WriteLine();
        
        Console.WriteLine("3. BACKING FIELD:");
        var account = new BankAccount();
        account.Deposit(100);
        Console.WriteLine($"Balance: {account.Balance}");
        account.Withdraw(30);
        Console.WriteLine($"After withdrawal: {account.Balance}");
        Console.WriteLine();
        
        Console.WriteLine("4. PROPERTY CHANGED EVENT:");
        var person = new Person();
        person.PropertyChanged += (prop, newValue) =>
            Console.WriteLine($"  Event: {prop} changed to {newValue}");
        person.Name = "John";
        person.Name = "Jane";
        Console.WriteLine();
        
        Console.WriteLine("5. EMAIL VALIDATION:");
        var user = new User();
        try
        {
            user.Email = "invalid";
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
        user.Email = "john@example.com";
        Console.WriteLine($"Valid email: {user.Email}");
    }
}

public class PropertyValidationTests
{
    [Fact]
    public void StrictValidation_ThrowsException()
    {
        var person = new StrictPerson();
        
        Assert.Throws<ArgumentException>(() => person.Age = -5);
    }
    
    [Fact]
    public void SilentValidation_IgnoresInvalid()
    {
        var person = new SilentPerson();
        person.Age = 30;
        person.Age = -5;
        
        Assert.Equal(30, person.Age);
    }
    
    [Fact]
    public void BankAccount_Deposit()
    {
        var account = new BankAccount();
        var result = account.Deposit(100);
        
        Assert.True(result);
        Assert.Equal(100, account.Balance);
    }
    
    [Fact]
    public void BankAccount_CannotWithdrawMore()
    {
        var account = new BankAccount();
        account.Deposit(100);
        var result = account.Withdraw(200);
        
        Assert.False(result);
        Assert.Equal(100, account.Balance);
    }
    
    [Fact]
    public void PropertyChanged_FiresEvent()
    {
        var person = new Person();
        string? changedProp = null;
        
        person.PropertyChanged += (prop, val) => changedProp = prop;
        person.Name = "John";
        
        Assert.Equal("Name", changedProp);
    }
    
    [Fact]
    public void EmailValidation_RequiresAt()
    {
        var user = new User();
        
        Assert.Throws<ArgumentException>(() => user.Email = "invalid");
        Assert.Throws<ArgumentException>(() => user.Email = "");
    }
}
