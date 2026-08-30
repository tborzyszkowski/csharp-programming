using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdapterFactory;

// ============================================================================
// CZĘŚĆ 1: Target Interface (Nasz Unified Interface)
// ============================================================================

/// <summary>
/// Unified payment gateway interface
/// </summary>
public abstract class PaymentGateway
{
    public string Name { get; set; }
    public string TransactionId { get; set; }
    
    public abstract void Authorize(decimal amount);
    public abstract void Capture();
    public abstract void Refund();
}

// ============================================================================
// CZĘŚĆ 2: External Payment Systems (Co Adaptujemy)
// ============================================================================

// Stripe API
public interface IStripeClient
{
    string Charge(decimal amount, string token);
}

public class StripeClient : IStripeClient
{
    public string Charge(decimal amount, string token)
    {
        return $"stripe_txn_{Guid.NewGuid().ToString()[..8]}";
    }
}

// PayPal API
public interface IPayPalClient
{
    string Execute(PayPalTransaction transaction);
}

public class PayPalTransaction
{
    public decimal Amount { get; set; }
}

public class PayPalClient : IPayPalClient
{
    public string Execute(PayPalTransaction transaction)
    {
        return $"paypal_txn_{Guid.NewGuid().ToString()[..8]}";
    }
}

// Legacy Payment System
public interface ILegacyPaymentSystem
{
    string ProcessTransaction(int customerId, decimal amount, string method);
}

public class LegacyPaymentSystem : ILegacyPaymentSystem
{
    public string ProcessTransaction(int customerId, decimal amount, string method)
    {
        return $"legacy_txn_{Guid.NewGuid().ToString()[..8]}";
    }
}

// ============================================================================
// CZĘŚĆ 3: Adapters (Konwersja Interfejsów)
// ============================================================================

/// <summary>
/// Adapter - Stripe to PaymentGateway
/// </summary>
public class StripeAdapter : PaymentGateway
{
    private readonly IStripeClient _stripe;
    private bool _authorized = false;
    private bool _captured = false;
    
    public StripeAdapter(IStripeClient? stripe = null)
    {
        Name = "💳 Stripe";
        _stripe = stripe ?? new StripeClient();
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"[{Name}] Authorizing ${amount}...");
        TransactionId = _stripe.Charge(amount, "tok_visa");
        _authorized = true;
        Console.WriteLine($"[{Name}] Authorization successful: {TransactionId}");
    }
    
    public override void Capture()
    {
        if (!_authorized)
            throw new InvalidOperationException("Must authorize before capture");
        
        Console.WriteLine($"[{Name}] Capturing transaction...");
        _captured = true;
        Console.WriteLine($"[{Name}] Capture successful");
    }
    
    public override void Refund()
    {
        Console.WriteLine($"[{Name}] Refunding {TransactionId}...");
        Console.WriteLine($"[{Name}] Refund processed");
    }
}

/// <summary>
/// Adapter - PayPal to PaymentGateway
/// </summary>
public class PayPalAdapter : PaymentGateway
{
    private readonly IPayPalClient _paypal;
    private bool _authorized = false;
    
    public PayPalAdapter(IPayPalClient? paypal = null)
    {
        Name = "🔵 PayPal";
        _paypal = paypal ?? new PayPalClient();
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"[{Name}] Authorizing ${amount}...");
        var transaction = new PayPalTransaction { Amount = amount };
        TransactionId = _paypal.Execute(transaction);
        _authorized = true;
        Console.WriteLine($"[{Name}] Authorization successful: {TransactionId}");
    }
    
    public override void Capture()
    {
        if (!_authorized)
            throw new InvalidOperationException("Must authorize before capture");
        
        Console.WriteLine($"[{Name}] Capturing transaction...");
        Console.WriteLine($"[{Name}] Capture successful");
    }
    
    public override void Refund()
    {
        Console.WriteLine($"[{Name}] Refunding {TransactionId}...");
        Console.WriteLine($"[{Name}] Refund processed");
    }
}

/// <summary>
/// Adapter - Legacy System to PaymentGateway
/// </summary>
public class LegacyAdapter : PaymentGateway
{
    private readonly ILegacyPaymentSystem _legacy;
    
    public LegacyAdapter(ILegacyPaymentSystem? legacy = null)
    {
        Name = "🔧 Legacy System";
        _legacy = legacy ?? new LegacyPaymentSystem();
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"[{Name}] Authorizing ${amount}...");
        TransactionId = _legacy.ProcessTransaction(12345, amount, "CC");
        Console.WriteLine($"[{Name}] Authorization successful: {TransactionId}");
    }
    
    public override void Capture()
    {
        Console.WriteLine($"[{Name}] Capturing transaction...");
        Console.WriteLine($"[{Name}] Capture successful");
    }
    
    public override void Refund()
    {
        Console.WriteLine($"[{Name}] Refunding {TransactionId}...");
        Console.WriteLine($"[{Name}] Refund processed");
    }
}

/// <summary>
/// Fake adapter for testing
/// </summary>
public class FakeGateway : PaymentGateway
{
    public FakeGateway()
    {
        Name = "🧪 Fake (Testing)";
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"[{Name}] Fake authorization: ${amount}");
        TransactionId = $"fake_txn_{Guid.NewGuid().ToString()[..8]}";
    }
    
    public override void Capture()
    {
        Console.WriteLine($"[{Name}] Fake capture");
    }
    
    public override void Refund()
    {
        Console.WriteLine($"[{Name}] Fake refund");
    }
}

// ============================================================================
// CZĘŚĆ 4: Factory (Centralne Tworzenie)
// ============================================================================

/// <summary>
/// Simple Factory Pattern
/// </summary>
public class PaymentGatewayFactory
{
    public static PaymentGateway Create(string providerName)
    {
        return providerName.ToLower() switch
        {
            "stripe" => new StripeAdapter(),
            "paypal" => new PayPalAdapter(),
            "legacy" => new LegacyAdapter(),
            "fake" or "test" => new FakeGateway(),
            _ => throw new ArgumentException($"Unknown provider: {providerName}")
        };
    }
}

// ============================================================================
// CZĘŚĆ 5: Business Logic (Unified Interface Usage)
// ============================================================================

public class Order
{
    public string OrderId { get; set; }
    public decimal Total { get; set; }
    public string PaymentProvider { get; set; }
}

public class OrderProcessor
{
    private readonly PaymentGatewayFactory _factory;
    
    public OrderProcessor()
    {
        _factory = new PaymentGatewayFactory();
    }
    
    public bool ProcessOrder(Order order)
    {
        try
        {
            Console.WriteLine($"\n📦 Processing order {order.OrderId}...");
            Console.WriteLine($"   Amount: ${order.Total}");
            Console.WriteLine($"   Provider: {order.PaymentProvider}\n");
            
            // Factory creates the right adapter
            var gateway = PaymentGatewayFactory.Create(order.PaymentProvider);
            
            // Unified interface - works the same for all!
            gateway.Authorize(order.Total);
            gateway.Capture();
            
            Console.WriteLine($"\n✅ Order {order.OrderId} completed successfully\n");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Order processing failed: {ex.Message}\n");
            return false;
        }
    }
}

// ============================================================================
// PROGRAM GŁÓWNY I TESTY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TEMAT 5: ADAPTER + FACTORY PATTERN                        ║");
        Console.WriteLine("║  Payment Gateway Integration                               ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        
        // ====================================================================
        Console.WriteLine("\nPROBLEM: Różne interfejsy dla każdego payment provider\n");
        Console.WriteLine("ROZWIĄZANIE: Adapter + Factory\n");
        
        // ====================================================================
        Console.WriteLine("SCENARIUSZ 1: Pojedynczy Adapter");
        
        var stripeAdapter = new StripeAdapter();
        stripeAdapter.Authorize(99.99m);
        stripeAdapter.Capture();
        stripeAdapter.Refund();
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 2: Factory Pattern - Różne Providery");
        
        var providers = new[] { "Stripe", "PayPal", "Legacy" };
        
        foreach (var provider in providers)
        {
            Console.WriteLine($"\n--- Creating {provider} Gateway ---");
            var gateway = PaymentGatewayFactory.Create(provider);
            gateway.Authorize(50m);
            gateway.Capture();
        }
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 3: Business Logic - Unified Interface");
        
        var processor = new OrderProcessor();
        
        var orders = new[]
        {
            new Order { OrderId = "ORD-001", Total = 99.99m, PaymentProvider = "Stripe" },
            new Order { OrderId = "ORD-002", Total = 149.99m, PaymentProvider = "PayPal" },
            new Order { OrderId = "ORD-003", Total = 199.99m, PaymentProvider = "Legacy" },
            new Order { OrderId = "ORD-004", Total = 299.99m, PaymentProvider = "Test" }
        };
        
        foreach (var order in orders)
        {
            processor.ProcessOrder(order);
        }
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 4: Type Safety - All Gateways Implement Same Interface");
        
        var gateways = new List<PaymentGateway>
        {
            new StripeAdapter(),
            new PayPalAdapter(),
            new LegacyAdapter(),
            new FakeGateway()
        };
        
        Console.WriteLine("\nAuthorizing $100 on all gateways:");
        foreach (var gateway in gateways)
        {
            gateway.Authorize(100m);
            Console.WriteLine($"  TransactionId: {gateway.TransactionId}");
        }
        
        Console.WriteLine("\n✅ Demonstracja ukończona");
    }
}

// ============================================================================
// TESTY XUNIT
// ============================================================================

public class AdapterFactoryTests
{
    [Fact]
    public void StripeAdapter_Authorize_SetsTransactionId()
    {
        var adapter = new StripeAdapter(new FakeStripeClient());
        adapter.Authorize(50m);
        
        Assert.NotEmpty(adapter.TransactionId);
    }
    
    [Fact]
    public void PayPalAdapter_Authorize_Works()
    {
        var adapter = new PayPalAdapter(new FakePayPalClient());
        adapter.Authorize(50m);
        
        Assert.NotEmpty(adapter.TransactionId);
    }
    
    [Fact]
    public void Factory_Create_ReturnsCorrectType()
    {
        var stripe = PaymentGatewayFactory.Create("stripe");
        var paypal = PaymentGatewayFactory.Create("paypal");
        var legacy = PaymentGatewayFactory.Create("legacy");
        
        Assert.IsType<StripeAdapter>(stripe);
        Assert.IsType<PayPalAdapter>(paypal);
        Assert.IsType<LegacyAdapter>(legacy);
    }
    
    [Fact]
    public void Factory_UnknownProvider_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
            PaymentGatewayFactory.Create("unknown_provider")
        );
    }
    
    [Fact]
    public void Adapter_Capture_RequiresAuthorization()
    {
        var adapter = new StripeAdapter();
        
        Assert.Throws<InvalidOperationException>(() => adapter.Capture());
    }
    
    [Fact]
    public void OrderProcessor_ProcessOrder_WithStripe()
    {
        var processor = new OrderProcessor();
        var order = new Order
        {
            OrderId = "TEST-001",
            Total = 50m,
            PaymentProvider = "stripe"
        };
        
        var result = processor.ProcessOrder(order);
        Assert.True(result);
    }
    
    [Fact]
    public void Polymorphism_AllAdapters_ImplementInterface()
    {
        var adapters = new List<PaymentGateway>
        {
            new StripeAdapter(),
            new PayPalAdapter(),
            new LegacyAdapter()
        };
        
        foreach (var adapter in adapters)
        {
            adapter.Authorize(50m);
            Assert.NotEmpty(adapter.TransactionId);
        }
    }
}

// ============================================================================
// FAKE IMPLEMENTATIONS FOR TESTING
// ============================================================================

public class FakeStripeClient : IStripeClient
{
    public string Charge(decimal amount, string token)
    {
        return $"fake_stripe_{Guid.NewGuid().ToString()[..8]}";
    }
}

public class FakePayPalClient : IPayPalClient
{
    public string Execute(PayPalTransaction transaction)
    {
        return $"fake_paypal_{Guid.NewGuid().ToString()[..8]}";
    }
}
