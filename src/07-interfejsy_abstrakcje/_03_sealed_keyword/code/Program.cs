using System;
using System.Collections.Generic;
using Xunit;

namespace SealedKeyword;

// ============================================================================
// CZĘŚĆ 1: Sealed Classes - Niemożna dziedziczyć
// ============================================================================

/// <summary>
/// Klasa abstrakcyjna - można dziedziczyć
/// </summary>
public abstract class PaymentBase
{
    public abstract void Process(decimal amount);
}

/// <summary>
/// ✅ Klasa konkretna - można dziedziczyć
/// </summary>
public class CreditCardPayment : PaymentBase
{
    public override void Process(decimal amount)
    {
        Console.WriteLine($"Processing: ${amount}");
    }
}

/// <summary>
/// ❌ SEALED klasa - NIEMOŻNA dziedziczyć
/// Użyj: gdy chcesz zabronić dalszego dziedziczenia
/// </summary>
public sealed class SpecialOffer
{
    public decimal Discount { get; set; }
    
    public SpecialOffer(decimal discount)
    {
        Discount = discount;
    }
}

// ❌ To by się nie kompilowało:
// public class SuperSpecialOffer : SpecialOffer { }  // Compiler Error!

// ============================================================================
// CZĘŚĆ 2: Sealed Methods - Nie można override'ować
// ============================================================================

public abstract class DocumentBase
{
    // Wirtualna metoda - można override'ować
    public virtual void Save()
    {
        Console.WriteLine("Saving document...");
    }
    
    // Wirtualna metoda - można override'ować
    public virtual void Validate()
    {
        Console.WriteLine("Validating document...");
    }
}

public class PdfDocument : DocumentBase
{
    // ✅ Override metody Save
    public override void Save()
    {
        Console.WriteLine("Saving as PDF...");
    }
    
    // ✅ SEALED override - dalsze klasy nie mogą override'ować
    public sealed override void Validate()
    {
        Console.WriteLine("PDF validation (sealed - cannot be overridden)");
    }
}

// ✅ Można dziedziczyć z PdfDocument
public class EncryptedPdfDocument : PdfDocument
{
    // ✅ Można override'ować Save (nie jest sealed)
    public override void Save()
    {
        Console.WriteLine("Saving encrypted PDF...");
    }
    
    // ❌ To by się nie kompilowało:
    // public override void Validate() { }  // Compiler Error! Validate jest sealed
}

// ============================================================================
// CZĘŚĆ 3: Sealed Properties (C# 8.0+)
// ============================================================================

public abstract class ConfigBase
{
    public abstract string AppName { get; }
}

public class AppConfig : ConfigBase
{
    // ✅ SEALED property - nie można override'ować w klasach pochodnych
    public sealed override string AppName => "MyApp";
}

// ============================================================================
// CZĘŚĆ 4: Real-World Example - Security
// ============================================================================

public abstract class CryptoBase
{
    public virtual string Encrypt(string data)
    {
        return $"[ENCRYPTED] {data}";
    }
    
    // Ta metoda jest critical - security risk!
    public virtual void ResetKey()
    {
        Console.WriteLine("Key reset");
    }
}

public class AesEncryption : CryptoBase
{
    public override string Encrypt(string data)
    {
        return $"[AES] {data}";
    }
    
    // ✅ SEALED - nie może być override'owana w podklasach
    // Chroni ważną operację bezpieczeństwa
    public sealed override void ResetKey()
    {
        Console.WriteLine("AES Key reset (security-critical operation)");
    }
}

// ✅ Można dziedziczyć z AesEncryption
public class SpecialAesEncryption : AesEncryption
{
    public override string Encrypt(string data)
    {
        return $"[SPECIAL-AES] {data}";
    }
    
    // ❌ Nie można override'ować ResetKey (sealed)
}

// ============================================================================
// CZĘŚĆ 5: Sealed vs Abstract
// ============================================================================

// ✅ SEALED - nie można dziedziczyć (kończy hierarchię)
public sealed class FinalImplementation
{
    public void DoSomething()
    {
        Console.WriteLine("Doing something...");
    }
}

// ❌ ABSTRACT - nie można instantiować (musi być dziedziczenie)
public abstract class TemplateForInheritance
{
    public abstract void DoSomething();
}

// ============================================================================
// CZĘŚĆ 6: Sealed vs Sealed Class
// ============================================================================

// ✅ SEALED na klasie - cała klasa jest sealed
public sealed class SingletonLike
{
    private static SingletonLike? _instance;
    
    private SingletonLike() { }
    
    public static SingletonLike Instance
    {
        get => _instance ??= new SingletonLike();
    }
}

// ❌ Niemożna:
// public class DerivedFromSingleton : SingletonLike { }  // Error!

// ============================================================================
// CZĘŚĆ 7: Performance Consideration
// ============================================================================

public abstract class VirtualCalls
{
    // Virtual method - runtime dispatch (small performance cost)
    public virtual void PerformAction()
    {
        Console.WriteLine("Action");
    }
}

public sealed class PerformantImplementation : VirtualCalls
{
    // ✅ SEALED class - compiler MAY optimize virtual calls
    public override void PerformAction()
    {
        Console.WriteLine("Performant action");
    }
}

// ============================================================================
// TESTS
// ============================================================================

public class SealedKeywordTests
{
    [Fact]
    public void CannotInheritFromSealedClass()
    {
        // ✅ Can instantiate sealed class
        var offer = new SpecialOffer(0.2m);
        Assert.Equal(0.2m, offer.Discount);
        
        // ❌ Cannot inherit from sealed class (compiler error)
        // public class SuperOffer : SpecialOffer { }
    }
    
    [Fact]
    public void CannotOverrideSealedMethod()
    {
        // ✅ PdfDocument overrides Validate as sealed
        var pdf = new PdfDocument();
        pdf.Validate();
        
        // ✅ EncryptedPdf can inherit and override Save
        var encrypted = new EncryptedPdfDocument();
        encrypted.Save();
        
        // ❌ EncryptedPdf cannot override Validate (sealed)
    }
    
    [Fact]
    public void SealedMethodInHierarchy()
    {
        var pdf = new PdfDocument();
        var encrypted = new EncryptedPdfDocument();
        
        // Both have Validate method, but:
        // - pdf.Validate() uses PdfDocument implementation
        // - encrypted.Validate() also uses PdfDocument implementation (cannot override)
        
        pdf.Validate();
        encrypted.Validate();
        
        Assert.NotNull(pdf);
        Assert.NotNull(encrypted);
    }
    
    [Fact]
    public void SealedSecurityCriticalMethods()
    {
        var crypto = new AesEncryption();
        crypto.Encrypt("secret");
        crypto.ResetKey();
        
        var special = new SpecialAesEncryption();
        special.Encrypt("data");
        special.ResetKey();  // Uses AesEncryption's implementation (sealed)
        
        Assert.NotNull(special);
    }
    
    [Fact]
    public void PolymorphismWithSealedMembers()
    {
        var encrypted = new EncryptedPdfDocument();
        encrypted.Save();
        encrypted.Validate();  // Sealed - uses PdfDocument implementation
        
        // The type is known at compile time
        DocumentBase doc = encrypted;
        doc.Save();      // Calls EncryptedPdfDocument.Save
        doc.Validate();  // Calls PdfDocument.Validate (sealed)
        
        Assert.NotNull(doc);
    }
    
    [Fact]
    public void SealedClassCanBeInstantiated()
    {
        var singleton = SingletonLike.Instance;
        var singleton2 = SingletonLike.Instance;
        
        Assert.Same(singleton, singleton2);
    }
}
