using System;
using Xunit;

namespace AccessModifiers;

public class BankAccount
{
    // Public - dostęp wszędzie
    public string AccountNumber { get; set; } = "";
    
    // Private - dostęp TYLKO tutaj
    private double balance = 0;
    
    // Protected - dostęp w klasach pochodnych
    protected bool IsActive { get; set; } = true;
    
    // Internal - dostęp w całym assembly
    internal string Bank { get; set; } = "";
    
    public double GetBalance()
    {
        return balance;
    }
    
    public void Deposit(double amount)
    {
        if (amount > 0 && IsActive)
            balance += amount;
    }
    
    protected void DeductFee(double fee)
    {
        if (balance >= fee)
            balance -= fee;
    }
}

public class StudentAccount : BankAccount
{
    public void ApplyDiscount()
    {
        // ✅ OK - protected
        Console.WriteLine($"Account status: {IsActive}");
        
        // ✅ OK - protected method
        DeductFee(5.0);
        
        // ❌ Nie można - private
        // Console.WriteLine(balance);
    }
}

public class Vehicle
{
    public string Make { get; set; } = "";
    
    private double fuelTank = 50.0;
    protected int MaxSpeed { get; set; } = 100;
    internal string VIN { get; set; } = "";
}

public class Car : Vehicle
{
    public void DisplayInfo()
    {
        // ✅ OK - public
        Console.WriteLine($"Make: {Make}");
        
        // ✅ OK - protected
        Console.WriteLine($"MaxSpeed: {MaxSpeed}");
        
        // ✅ OK - internal
        Console.WriteLine($"VIN: {VIN}");
        
        // ❌ Nie można - private
        // Console.WriteLine(fuelTank);
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== MODYFIKATORY DOSTĘPU ===\n");
        
        Console.WriteLine("1. Public Access:");
        var account = new BankAccount 
        { 
            AccountNumber = "123456",
            Bank = "PKO"
        };
        Console.WriteLine($"   Account: {account.AccountNumber}");
        Console.WriteLine();
        
        Console.WriteLine("2. Private Field (hidden):");
        account.Deposit(1000);
        Console.WriteLine($"   Balance (via GetBalance): {account.GetBalance()}");
        Console.WriteLine();
        
        Console.WriteLine("3. Protected in Derived Class:");
        var student = new StudentAccount();
        student.AccountNumber = "STU001";
        student.Deposit(500);
        student.ApplyDiscount();
        Console.WriteLine();
        
        Console.WriteLine("4. C# vs Java - protected difference:");
        var car = new Car { Make = "Toyota", VIN = "ABC123" };
        car.DisplayInfo();
        Console.WriteLine("   Note: In C#, protected only accessible in derived classes");
        Console.WriteLine("   In Java, protected also accessible in same package");
    }
}

public class AccessModifiersTests
{
    [Fact]
    public void PublicProperty_CanBeAccessedExternally()
    {
        var account = new BankAccount { AccountNumber = "123" };
        
        Assert.Equal("123", account.AccountNumber);
    }
    
    [Fact]
    public void PrivateField_CannotBeAccessedDirectly()
    {
        var account = new BankAccount();
        
        // balance is private - cannot access directly
        // account.balance = 1000;  // Compile error
        
        account.Deposit(1000);
        Assert.Equal(1000, account.GetBalance());
    }
    
    [Fact]
    public void ProtectedMember_AccessibleInDerivedClass()
    {
        var student = new StudentAccount();
        student.Deposit(500);
        
        // ✅ This works - StudentAccount can access protected
        student.ApplyDiscount();  // Should not throw
        Assert.NotNull(student);
    }
    
    [Fact]
    public void ProtectedProperty_NotAccessibleExternally()
    {
        var account = new BankAccount();
        
        // account.IsActive = false;  // Compile error - protected
        Assert.NotNull(account);
    }
    
    [Fact]
    public void DerivedClass_InheritsPublicAndProtected()
    {
        var car = new Car { Make = "Toyota", VIN = "123" };
        
        Assert.Equal("Toyota", car.Make);
        Assert.Equal("123", car.VIN);
    }
    
    [Fact]
    public void Protected_OnlyInDerived_Not_External()
    {
        var vehicle = new Vehicle { Make = "Honda" };
        
        // vehicle.MaxSpeed = 150;  // Compile error - protected
        Assert.NotNull(vehicle);
    }
}
