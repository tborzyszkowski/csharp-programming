using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AbstractMethods;

// ============================================================================
// CZĘŚĆ 1: Abstract vs Virtual - Kluczowe Różnice
// ============================================================================

/// <summary>
/// Metoda ABSTRAKCYJNA: brak implementacji, MUSI być override'owana
/// </summary>
public abstract class PaymentBase
{
    // ❌ Abstrakcyjna metoda - brak ciała
    public abstract void Process(decimal amount);
    
    // ✅ Wirtualna metoda - ma implementację, może być override'owana
    public virtual void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public class CreditCardPayment : PaymentBase
{
    // ✅ MUSI implementować Process (abstrakcyjna)
    public override void Process(decimal amount)
    {
        Console.WriteLine($"Processing credit card: ${amount}");
    }
    
    // 📝 Może override'ować Log (wirtualna) lub nie
    public override void Log(string message)
    {
        Console.WriteLine($"[CARD] {message}");
    }
}

public class BankTransferPayment : PaymentBase
{
    // ✅ MUSI implementować Process (abstrakcyjna)
    public override void Process(decimal amount)
    {
        Console.WriteLine($"Processing bank transfer: ${amount}");
    }
    
    // Nie override'ujemy Log - używamy domyślnej implementacji
}

// ============================================================================
// CZĘŚĆ 2: Łańcuch Abstrakcji (Chain of Abstractions)
// ============================================================================

public abstract class DocumentBase
{
    // Całkowicie abstrakcyjna metoda
    public abstract void Open();
    public abstract void Save();
    public abstract void Close();
    
    // Abstrakcyjna metoda z wartością zwracaną
    public abstract string GetContent();
    
    // Abstrakcyjna metoda z parametrami
    public abstract void FindAndReplace(string find, string replace);
}

public class PdfDocument : DocumentBase
{
    private string _content = "PDF content";
    
    public override void Open()
    {
        Console.WriteLine("Opening PDF with Adobe Reader...");
    }
    
    public override void Save()
    {
        Console.WriteLine("Saving PDF...");
    }
    
    public override void Close()
    {
        Console.WriteLine("Closing PDF...");
    }
    
    public override string GetContent() => _content;
    
    public override void FindAndReplace(string find, string replace)
    {
        _content = _content.Replace(find, replace);
        Console.WriteLine($"Replaced '{find}' with '{replace}'");
    }
}

public class WordDocument : DocumentBase
{
    private string _content = "Word document content";
    
    public override void Open()
    {
        Console.WriteLine("Opening Word document...");
    }
    
    public override void Save()
    {
        Console.WriteLine("Saving Word document...");
    }
    
    public override void Close()
    {
        Console.WriteLine("Closing Word document...");
    }
    
    public override string GetContent() => _content;
    
    public override void FindAndReplace(string find, string replace)
    {
        _content = _content.Replace(find, replace);
        Console.WriteLine($"Replaced '{find}' with '{replace}'");
    }
}

// ============================================================================
// CZĘŚĆ 3: Ograniczenia Metod Abstrakcyjnych
// ============================================================================

public abstract class RepositoryBase<T>
{
    // ❌ Abstrakcyjna metoda nie może mieć implementacji
    // public abstract void Add(T item) { } // Compiler error!
    
    // ✅ Poprawnie - tylko sygnatura
    public abstract void Add(T item);
    public abstract void Remove(T item);
    public abstract T? GetById(int id);
    public abstract List<T> GetAll();
    
    // ✅ Metody z access modifiers
    protected abstract void ValidateItem(T item);
    
    // ✅ Może zawierać properties
    public abstract int Count { get; }
}

public class UserRepository : RepositoryBase<string>
{
    private List<string> _users = new();
    
    public override void Add(string item)
    {
        ValidateItem(item);
        _users.Add(item);
    }
    
    public override void Remove(string item) => _users.Remove(item);
    
    public override string? GetById(int id) 
        => id >= 0 && id < _users.Count ? _users[id] : null;
    
    public override List<string> GetAll() => new(_users);
    
    public override int Count => _users.Count;
    
    protected override void ValidateItem(string item)
    {
        if (string.IsNullOrEmpty(item))
            throw new ArgumentException("User cannot be empty");
    }
}

// ============================================================================
// CZĘŚĆ 4: Abstract vs Virtual - Porównanie Tabel
// ============================================================================

public abstract class VehicleAbstractStyle
{
    // Abstrakcyjna - nie ma domyślnej implementacji
    public abstract void Start();
    
    // Wirtualna - ma domyślną implementację
    public virtual void Horn()
    {
        Console.WriteLine("Beep beep!");
    }
}

public abstract class VehicleVirtualStyle
{
    // Wirtualna - ma domyślną implementację
    public virtual void Start()
    {
        Console.WriteLine("Starting engine...");
    }
    
    // Wirtualna - ma domyślną implementację
    public virtual void Horn()
    {
        Console.WriteLine("Beep beep!");
    }
}

// ============================================================================
// CZĘŚĆ 5: Interfejsy vs Abstrakcyjne Klasy (preview)
// ============================================================================

// Interfejs - tylko kontrakt
public interface IStorable
{
    void Save();
    void Load();
}

// Klasa abstrakcyjna - może mieć stan i implementacje
public abstract class DocumentWithState
{
    protected string _fileName = "";
    protected DateTime _lastModified = DateTime.Now;
    
    public abstract void Save();
    public abstract void Load();
    
    public virtual void PrintInfo()
    {
        Console.WriteLine($"File: {_fileName}, Modified: {_lastModified}");
    }
}

// ============================================================================
// TESTS
// ============================================================================

public class AbstractMethodsTests
{
    [Fact]
    public void AbstractMethodMustBeImplemented()
    {
        // ❌ Cannot do:
        // var payment = new PaymentBase();  // Compiler error
        
        // ✅ Must instantiate concrete class
        PaymentBase payment = new CreditCardPayment();
        payment.Process(100m);
        
        Assert.NotNull(payment);
    }
    
    [Fact]
    public void OverridingAbstractMethod()
    {
        var card = new CreditCardPayment();
        var bank = new BankTransferPayment();
        
        // Both implement Process differently
        List<PaymentBase> payments = new() { card, bank };
        
        foreach (var payment in payments)
        {
            payment.Process(50m);
        }
        
        Assert.Equal(2, payments.Count);
    }
    
    [Fact]
    public void OverridingVirtualMethod()
    {
        var card = new CreditCardPayment();
        var bank = new BankTransferPayment();
        
        // Card overrides Log, Bank uses default
        card.Log("Card payment test");     // Custom log
        bank.Log("Bank payment test");     // Default log
        
        Assert.NotNull(card);
        Assert.NotNull(bank);
    }
    
    [Fact]
    public void DocumentChainOfAbstraction()
    {
        List<DocumentBase> documents = new()
        {
            new PdfDocument(),
            new WordDocument()
        };
        
        foreach (var doc in documents)
        {
            doc.Open();
            var content = doc.GetContent();
            doc.FindAndReplace("content", "TEXT");
            doc.Save();
            doc.Close();
        }
        
        Assert.Equal(2, documents.Count);
    }
    
    [Fact]
    public void RepositoryPatternWithAbstractMethods()
    {
        RepositoryBase<string> repo = new UserRepository();
        
        repo.Add("Alice");
        repo.Add("Bob");
        
        Assert.Equal(2, repo.Count);
        Assert.Equal("Alice", repo.GetById(0));
    }
    
    [Fact]
    public void AbstractPropertyImplementation()
    {
        UserRepository repo = new UserRepository();
        
        repo.Add("User1");
        repo.Add("User2");
        repo.Add("User3");
        
        Assert.Equal(3, repo.Count);
    }
    
    [Fact]
    public void PolymorphisticBehavior()
    {
        DocumentBase pdf = new PdfDocument();
        DocumentBase word = new WordDocument();
        
        // Same method call, different implementations
        pdf.Open();      // Opens with Adobe
        word.Open();     // Opens with Word
        
}
