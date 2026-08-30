using System;
using Xunit;

namespace PropertiesVsFields;

// ============ POLA (Fields) - PROBLEM ============
public class PersonWithFields
{
    public string name = string.Empty;
    public int age;  // Brak walidacji!
}

// ============ WŁAŚCIWOŚCI (Properties) - ROZWIĄZANIE ============
public class Person
{
    private string _name = string.Empty;
    private int _age;
    
    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value >= 0 && value <= 150)
                _age = value;
        }
    }
}

// ============ ENKAPSULACJA ============
public class BankAccount
{
    private decimal _balance = 0;
    
    public decimal Balance
    {
        get { return _balance; }
        private set { _balance = value; }  // Prywatny setter
    }
    
    public void Deposit(decimal amount)
    {
        if (amount > 0)
            _balance += amount;
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

// ============ READ-ONLY PROPERTY ============
public class User
{
    private int _id;
    
    public int Id
    {
        get { return _id; }
    }
    
    public User(int id)
    {
        _id = id;
    }
}

// ============ WRITE-ONLY (RZADKIE) ============
public class Password
{
    private string _hash = string.Empty;
    
    public string PasswordHash
    {
        set { _hash = SimpleHash(value); }
    }
    
    public bool VerifyPassword(string password)
    {
        return _hash == SimpleHash(password);
    }
    
    private static string SimpleHash(string input)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(input));
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 1: WŁAŚCIWOŚCI vs POLA ===\n");
        
        Console.WriteLine("1. POLA - PROBLEM:");
        var personFields = new PersonWithFields();
        personFields.age = -5;  // BŁĄD - bez walidacji!
        Console.WriteLine($"Age: {personFields.age}");
        Console.WriteLine();
        
        Console.WriteLine("2. WŁAŚCIWOŚCI - ROZWIĄZANIE:");
        var person = new Person();
        person.Age = 30;
        Console.WriteLine($"Name: {person.Name}, Age: {person.Age}");
        person.Age = -5;  // Setter zignaruje!
        Console.WriteLine($"Age (after invalid set): {person.Age}");
        Console.WriteLine();
        
        Console.WriteLine("3. ENKAPSULACJA:");
        var account = new BankAccount();
        account.Deposit(100);
        Console.WriteLine($"Balance: {account.Balance}");
        account.Withdraw(30);
        Console.WriteLine($"After withdrawal: {account.Balance}");
        // account.Balance = -1000;  // BŁĄD - setter prywatny!
        Console.WriteLine();
        
        Console.WriteLine("4. READ-ONLY PROPERTY:");
        var user = new User(42);
        Console.WriteLine($"User ID: {user.Id}");
        // user.Id = 100;  // BŁĄD - brak setter!
        Console.WriteLine();
        
        Console.WriteLine("5. WRITE-ONLY + VERIFY:");
        var pwd = new Password();
        pwd.PasswordHash = "tajne123";
        Console.WriteLine($"Password valid: {pwd.VerifyPassword("tajne123")}");
        Console.WriteLine($"Password invalid: {pwd.VerifyPassword("zla")}");
    }
}

public class PropertiesVsFieldsTests
{
    [Fact]
    public void Property_AllowsValidValue()
    {
        var person = new Person();
        person.Age = 30;
        
        Assert.Equal(30, person.Age);
    }
    
    [Fact]
    public void Property_IgnoresInvalidValue()
    {
        var person = new Person();
        person.Age = 30;
        person.Age = -5;  // Invalid
        
        Assert.Equal(30, person.Age);  // Unchanged
    }
    
    [Fact]
    public void BankAccount_DepositIncreases()
    {
        var account = new BankAccount();
        account.Deposit(100);
        
        Assert.Equal(100, account.Balance);
    }
    
    [Fact]
    public void BankAccount_CannotDirectlyModifyBalance()
    {
        var account = new BankAccount();
        account.Deposit(100);
        
        // account.Balance = -1000;  // Compilation error - private setter
        Assert.Equal(100, account.Balance);
    }
    
    [Fact]
    public void ReadOnlyProperty_CannotSet()
    {
        var user = new User(42);
        
        // user.Id = 100;  // Compilation error
        Assert.Equal(42, user.Id);
    }
    
    [Fact]
    public void Password_VerifyWorks()
    {
        var pwd = new Password();
        pwd.PasswordHash = "test123";
        
        Assert.True(pwd.VerifyPassword("test123"));
        Assert.False(pwd.VerifyPassword("wrong"));
    }
}
