using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace VirtualMethodsExample;

// ============================================================================
// CZĘŚĆ 1: Payment Gateway System
// ============================================================================

/// <summary>
/// Abstrakcyjna klasa bazowa dla wszystkich metod płatności
/// </summary>
public abstract class PaymentMethod
{
    public string Name { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
    
    // Virtual methods - każda płatność implementuje inaczej
    public virtual void Authorize()
    {
        Console.WriteLine($"[{Name}] Authorizing payment...");
    }
    
    public virtual void ProcessPayment()
    {
        Authorize();
        Console.WriteLine($"[{Name}] Processing...");
        TransactionId = Guid.NewGuid().ToString()[..8];
        Status = PaymentStatus.Completed;
    }
    
    public virtual void GenerateReceipt()
    {
        Console.WriteLine($"[{Name}] Receipt: {TransactionId} - {Status}");
    }
    
    public virtual void Refund()
    {
        Console.WriteLine($"[{Name}] Refunding transaction {TransactionId}...");
        Status = PaymentStatus.Refunded;
    }
}

public enum PaymentStatus { Pending, Completed, Failed, Refunded }

/// <summary>
/// Implementacja dla płatności kartą kredytową
/// </summary>
public class CreditCardPayment : PaymentMethod
{
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public string ExpiryDate { get; set; }
    
    public CreditCardPayment()
    {
        Name = "💳 Credit Card";
        CardNumber = "****-****-****-1234";
        CVV = "***";
    }
    
    public override void Authorize()
    {
        Console.WriteLine($"[{Name}] Validating card {CardNumber}...");
        Console.WriteLine($"[{Name}] Checking CVV and expiry date...");
        Console.WriteLine($"[{Name}] Card authorized ✓");
    }
    
    public override void ProcessPayment()
    {
        base.ProcessPayment();
        Console.WriteLine($"[{Name}] Charging {CardNumber}...");
    }
    
    public override void GenerateReceipt()
    {
        Console.WriteLine($"\n📄 Credit Card Receipt:");
        Console.WriteLine($"   Transaction ID: {TransactionId}");
        Console.WriteLine($"   Card: {CardNumber}");
        Console.WriteLine($"   Status: {Status}");
    }
}

/// <summary>
/// Implementacja dla PayPal
/// </summary>
public class PayPalPayment : PaymentMethod
{
    public string Email { get; set; }
    
    public PayPalPayment()
    {
        Name = "🔵 PayPal";
        Email = "user@example.com";
    }
    
    public override void Authorize()
    {
        Console.WriteLine($"[{Name}] Authenticating with {Email}...");
        Console.WriteLine($"[{Name}] OAuth2 flow started...");
        Console.WriteLine($"[{Name}] User logged in ✓");
    }
    
    public override void ProcessPayment()
    {
        base.ProcessPayment();
        Console.WriteLine($"[{Name}] Transferring funds from PayPal account...");
    }
    
    public override void GenerateReceipt()
    {
        Console.WriteLine($"\n📄 PayPal Receipt:");
        Console.WriteLine($"   Transaction ID: {TransactionId}");
        Console.WriteLine($"   Email: {Email}");
        Console.WriteLine($"   Status: {Status}");
    }
}

/// <summary>
/// Implementacja dla Bitcoina
/// </summary>
public class BitcoinPayment : PaymentMethod
{
    public string WalletAddress { get; set; }
    
    public BitcoinPayment()
    {
        Name = "₿ Bitcoin";
        WalletAddress = "1A1z7agoat...";
    }
    
    public override void Authorize()
    {
        Console.WriteLine($"[{Name}] Verifying wallet {WalletAddress}...");
        Console.WriteLine($"[{Name}] Checking blockchain balance...");
        Console.WriteLine($"[{Name}] Wallet verified ✓");
    }
    
    public override void ProcessPayment()
    {
        base.ProcessPayment();
        Console.WriteLine($"[{Name}] Creating blockchain transaction...");
        Console.WriteLine($"[{Name}] Broadcasting to network...");
    }
    
    public override void GenerateReceipt()
    {
        Console.WriteLine($"\n📄 Bitcoin Receipt:");
        Console.WriteLine($"   Transaction ID: {TransactionId}");
        Console.WriteLine($"   Wallet: {WalletAddress}");
        Console.WriteLine($"   Status: {Status}");
    }
    
    public override void Refund()
    {
        Console.WriteLine($"[{Name}] ⚠️  Bitcoin transactions cannot be refunded (blockchain immutable)");
        Status = PaymentStatus.Failed;
    }
}

/// <summary>
/// Implementacja dla BNPL (Buy Now Pay Later)
/// </summary>
public class BNPLPayment : PaymentMethod
{
    public decimal MonthlyPayment { get; set; }
    public int Installments { get; set; }
    
    public BNPLPayment()
    {
        Name = "📅 BNPL (Buy Now Pay Later)";
        Installments = 4;
    }
    
    public override void Authorize()
    {
        Console.WriteLine($"[{Name}] Checking credit score...");
        Console.WriteLine($"[{Name}] Approving 4 installments...");
        Console.WriteLine($"[{Name}] Credit approved ✓");
    }
    
    public override void ProcessPayment()
    {
        base.ProcessPayment();
        Console.WriteLine($"[{Name}] Scheduling {Installments} monthly payments...");
    }
    
    public override void GenerateReceipt()
    {
        Console.WriteLine($"\n📄 BNPL Receipt:");
        Console.WriteLine($"   Transaction ID: {TransactionId}");
        Console.WriteLine($"   Installments: {Installments}");
        Console.WriteLine($"   Monthly: {MonthlyPayment:C}");
        Console.WriteLine($"   Status: {Status}");
    }
}

// ============================================================================
// CZĘŚĆ 2: Order Processing System
// ============================================================================

public class Item
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    
    public decimal Total => Price * Quantity;
}

public class Order
{
    public string OrderId { get; set; }
    public List<Item> Items { get; set; }
    public DateTime OrderDate { get; set; }
    
    public decimal Total => Items.Sum(i => i.Total);
}

/// <summary>
/// Procesor zamówień - współpracuje z KAŻDĄ metodą płatności!
/// </summary>
public class OrderProcessor
{
    public bool ProcessOrder(Order order, PaymentMethod payment)
    {
        try
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║ Order: {order.OrderId}                                       ║");
            Console.WriteLine($"║ Total: {order.Total:C}                                          ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine($"\n📦 Items in order:");
            foreach (var item in order.Items)
            {
                Console.WriteLine($"   - {item.Name}: {item.Quantity}x {item.Price:C} = {item.Total:C}");
            }
            
            Console.WriteLine($"\n💳 Processing with {payment.Name}...\n");
            
            // Virtual dispatch - każda płatność robi coś innego!
            payment.ProcessPayment();
            payment.GenerateReceipt();
            
            Console.WriteLine("\n✅ Order processed successfully!\n");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Error: {ex.Message}\n");
            return false;
        }
    }
    
    public void RefundOrder(PaymentMethod payment)
    {
        Console.WriteLine("\n🔄 Initiating refund...\n");
        payment.Refund();
        Console.WriteLine();
    }
}

// ============================================================================
// CZĘŚĆ 3: Payment Strategy System
// ============================================================================

public abstract class DiscountStrategy
{
    public abstract decimal ApplyDiscount(decimal amount);
}

public class NoDiscount : DiscountStrategy
{
    public override decimal ApplyDiscount(decimal amount) => amount;
}

public class CreditCardDiscount : DiscountStrategy
{
    public override decimal ApplyDiscount(decimal amount) => amount * 0.98m;  // 2% off
}

public class PayPalDiscount : DiscountStrategy
{
    public override decimal ApplyDiscount(decimal amount) => amount * 0.95m;  // 5% off
}

public class BNPLDiscount : DiscountStrategy
{
    public override decimal ApplyDiscount(decimal amount) => amount;  // No discount, fees instead
}

// ============================================================================
// PROGRAM GŁÓWNY I TESTY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TEMAT 2: FUNKCJE WIRTUALNE W PRAKTYCE                     ║");
        Console.WriteLine("║  E-Commerce Payment System                                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        // Stwórz zamówienie
        var order = new Order
        {
            OrderId = "ORD-2024-001",
            OrderDate = DateTime.Now,
            Items = new()
            {
                new Item { Name = "Laptop", Price = 1299.99m, Quantity = 1 },
                new Item { Name = "Mouse", Price = 29.99m, Quantity = 2 },
                new Item { Name = "Keyboard", Price = 79.99m, Quantity = 1 }
            }
        };
        
        var processor = new OrderProcessor();
        
        // ====================================================================
        Console.WriteLine("SCENARIUSZ 1: Płatność Kartą Kredytową\n");
        var creditCard = new CreditCardPayment();
        processor.ProcessOrder(order, creditCard);
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 2: Płatność PayPal\n");
        var paypal = new PayPalPayment();
        processor.ProcessOrder(order, paypal);
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 3: Płatność Bitcoin\n");
        var bitcoin = new BitcoinPayment();
        processor.ProcessOrder(order, bitcoin);
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 4: Płatność BNPL\n");
        var bnpl = new BNPLPayment { MonthlyPayment = order.Total / 4 };
        processor.ProcessOrder(order, bnpl);
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 5: Refund\n");
        processor.RefundOrder(creditCard);
        
        // ====================================================================
        Console.WriteLine("\nSCENARIUSZ 6: Discount Strategies\n");
        Console.WriteLine("Testing pricing strategies:");
        var strategies = new List<(string name, DiscountStrategy strategy)>
        {
            ("No Discount", new NoDiscount()),
            ("Credit Card (2% off)", new CreditCardDiscount()),
            ("PayPal (5% off)", new PayPalDiscount()),
            ("BNPL (no discount)", new BNPLDiscount())
        };
        
        foreach (var (name, strategy) in strategies)
        {
            var finalPrice = strategy.ApplyDiscount(order.Total);
            Console.WriteLine($"{name}: {order.Total:C} → {finalPrice:C}");
        }
        
        Console.WriteLine("\n✅ Demonstracja ukończona");
    }
}

// ============================================================================
// TESTY XUNIT
// ============================================================================

public class VirtualMethodsExampleTests
{
    [Fact]
    public void CreditCard_ProcessPayment_SetsTransactionId()
    {
        var payment = new CreditCardPayment();
        payment.ProcessPayment();
        
        Assert.NotEmpty(payment.TransactionId);
        Assert.Equal(PaymentStatus.Completed, payment.Status);
    }
    
    [Fact]
    public void PayPal_ProcessPayment_Succeeds()
    {
        var payment = new PayPalPayment();
        payment.ProcessPayment();
        
        Assert.NotEmpty(payment.TransactionId);
        Assert.Equal(PaymentStatus.Completed, payment.Status);
    }
    
    [Fact]
    public void Bitcoin_Refund_CannotRefund()
    {
        var payment = new BitcoinPayment();
        payment.ProcessPayment();
        payment.Refund();
        
        // Bitcoin nie może być refundowany
        Assert.NotEqual(PaymentStatus.Refunded, payment.Status);
    }
    
    [Fact]
    public void Order_CalculatesTotal_Correctly()
    {
        var order = new Order
        {
            Items = new()
            {
                new Item { Name = "Item1", Price = 100m, Quantity = 2 },
                new Item { Name = "Item2", Price = 50m, Quantity = 1 }
            }
        };
        
        Assert.Equal(250m, order.Total);
    }
    
    [Fact]
    public void OrderProcessor_ProcessOrder_WithMultiplePayments()
    {
        var order = new Order
        {
            OrderId = "TEST-001",
            Items = new() { new Item { Name = "Test", Price = 100m, Quantity = 1 } }
        };
        
        var processor = new OrderProcessor();
        var creditCard = new CreditCardPayment();
        
        var result = processor.ProcessOrder(order, creditCard);
        
        Assert.True(result);
        Assert.Equal(PaymentStatus.Completed, creditCard.Status);
    }
    
    [Fact]
    public void Polymorphism_MultiplePaymentMethods_AllWork()
    {
        List<PaymentMethod> payments = new()
        {
            new CreditCardPayment(),
            new PayPalPayment(),
            new BitcoinPayment(),
            new BNPLPayment()
        };
        
        foreach (var payment in payments)
        {
            payment.ProcessPayment();
            Assert.NotEmpty(payment.TransactionId);
            Assert.NotEqual(PaymentStatus.Pending, payment.Status);
        }
    }
    
    [Fact]
    public void DiscountStrategy_AppliesCorrectly()
    {
        decimal amount = 100m;
        
        var noDiscount = new NoDiscount();
        var creditCardDiscount = new CreditCardDiscount();
        var paypalDiscount = new PayPalDiscount();
        
        Assert.Equal(100m, noDiscount.ApplyDiscount(amount));
        Assert.Equal(98m, creditCardDiscount.ApplyDiscount(amount));
        Assert.Equal(95m, paypalDiscount.ApplyDiscount(amount));
    }
}
