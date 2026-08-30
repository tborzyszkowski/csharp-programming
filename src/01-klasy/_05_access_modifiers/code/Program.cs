using System;
using Xunit;

namespace AccessModifiers;

/// <summary>
/// TEMAT 5: Modyfikatory Dostępu
/// Demonstracja public, private, protected, internal
/// </summary>

public class BankAccount
{
    private string accountNumber;
    private decimal balance;
    private int pin;
    
    public string AccountHolder { get; private set; }
    public string AccountNumber => accountNumber;
    public decimal Balance => balance;
    
    public BankAccount(string accountHolder, string accountNumber, int pin)
    {
        AccountHolder = accountHolder;
        this.accountNumber = accountNumber;
        this.pin = pin;
        this.balance = 0;
    }
    
    public void Deposit(decimal amount)
    {
        if (ValidateAmount(amount))
        {
            balance += amount;
            Console.WriteLine($"✓ Wpłacono {amount:C}");
        }
    }
    
    public bool Withdraw(decimal amount, int providedPin)
    {
        if (!VerifyPin(providedPin))
        {
            Console.WriteLine("✗ Błędny PIN!");
            return false;
        }
        
        if (!ValidateAmount(amount) || amount > balance)
        {
            Console.WriteLine("✗ Nie można wypłacić!");
            return false;
        }
        
        balance -= amount;
        Console.WriteLine($"✓ Wypłacono {amount:C}");
        return true;
    }
    
    // Metody prywatne - pomocnicze
    private bool VerifyPin(int provided Pin) => provided Pin == pin;
    private bool ValidateAmount(decimal amount) => amount > 0;
}

public class Document
{
    public string Title { get; set; }
    protected string Content { get; set; }
    private string Metadata { get; set; }
    
    public Document(string title)
    {
        Title = title;
        Content = "";
        Metadata = "";
    }
    
    public virtual void Print()
    {
        Console.WriteLine($"Dokument: {Title}");
        Console.WriteLine($"Zawartość: {Content}");
    }
    
    protected void AddMetadata(string meta)
    {
        Metadata = meta;
    }
}

public class SecretDocument : Document
{
    public SecretDocument(string title) : base(title) { }
    
    public override void Print()
    {
        Console.WriteLine($"TAJNE: {Title}");
        Console.WriteLine($"Zawartość: {Content}");  // OK - protected
    }
    
    public void AddClassification(string level)
    {
        AddMetadata($"Poziom: {level}");  // OK - protected
    }
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  MODYFIKATORY DOSTĘPU                                 ║");
        Console.WriteLine("║  public, private, protected, internal                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        DemonstrateBankAccount();
        Console.WriteLine("\n" + new string('─', 60) + "\n");
        
        DemonstrateInheritance();
    }
    
    private static void DemonstrateBankAccount()
    {
        Console.WriteLine("🏦 Modyfikatory Dostępu - BankAccount");
        Console.WriteLine("─────────────────────────────────────\n");
        
        var account = new BankAccount("Jan Kowalski", "PL1234567890", 1234);
        
        Console.WriteLine($"Właściciel (public): {account.AccountHolder}");
        Console.WriteLine($"Numer konta (public property): {account.AccountNumber}");
        Console.WriteLine($"Saldo (public property): {account.Balance}\n");
        
        account.Deposit(1000);
        account.Withdraw(300, 1234);
        account.Withdraw(200, 9999);
        
        // Poniższe byłyby błędami kompilacji:
        // account.balance = -1000;  // Błąd - private
        // account.pin = 9999;       // Błąd - private
        // account.VerifyPin(1234);  // Błąd - private
    }
    
    private static void DemonstrateInheritance()
    {
        Console.WriteLine("📄 Protected - Dostęp w Klasach Pochodnych");
        Console.WriteLine("──────────────────────────────────────────\n");
        
        var doc = new SecretDocument("Tajne Sprawozdanie");
        doc.Content = "To jest tajne!";
        doc.AddClassification("TOP SECRET");
        
        doc.Print();
        
        // Poniższe byłyby błędami:
        // doc.Content = "...";       // Błąd - protected (widoczne dla pochodnych, nie publiczne)
        // doc.AddMetadata("...");    // Błąd - private (niedostępne z zewnątrz)
    }
}

/// ============================================
/// TESTY
/// ============================================

public class AccessModifiersTests
{
    [Fact]
    public void BankAccount_PublicProperties_AreAccessible()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        
        Assert.Equal("John Doe", account.AccountHolder);
        Assert.Equal("123456", account.AccountNumber);
        Assert.Equal(0, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Deposit_IncreasesBalance()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        account.Deposit(1000);
        
        Assert.Equal(1000, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_WithCorrectPin_Succeeds()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        account.Deposit(1000);
        
        bool result = account.Withdraw(500, 1234);
        
        Assert.True(result);
        Assert.Equal(500, account.Balance);
    }
    
    [Fact]
    public void BankAccount_Withdraw_WithWrongPin_Fails()
    {
        var account = new BankAccount("John Doe", "123456", 1234);
        account.Deposit(1000);
        
        bool result = account.Withdraw(500, 9999);
        
        Assert.False(result);
        Assert.Equal(1000, account.Balance);
    }
}
