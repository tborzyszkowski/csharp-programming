# Temat 2: Przykład - Funkcje Wirtualne w Praktyce

## 📌 Cel Tematu

Rzeczywisty przykład systemu e-commerce z różnymi metodami płatności - pokazanie **polimorfizmu** w działaniu na praktycznym case'u biznesowym.

## 🎯 Problem Biznesowy

Sklep internetowy musi obsługiwać wiele metod płatności:
- Karty kredytowe
- PayPal
- Przelewy bankowe
- Kryptowaluty
- BNPL (Buy Now Pay Later)

**Bez polimorfizmu**: Ogromny switch statement

```csharp
switch(paymentType)
{
    case "CreditCard":
        // 50 linii kodu...
        break;
    case "PayPal":
        // 50 linii kodu...
        break;
    case "Bitcoin":
        // 50 linii kodu...
        break;
    // ... i tak dalej
}
```

**Z polimorfizmem**: Jeden kod działa dla wszystkich!

```csharp
PaymentMethod payment = GetPaymentMethod(type);
payment.ProcessPayment();
payment.GenerateReceipt();
```

---

## 🏗️ Architektura Systemu

```
┌─────────────────────────────────┐
│   PaymentProcessor (klient)     │
│   - ProcessOrder()              │
│   - Nie wie jakiej płatności!   │
└────────────────┬────────────────┘
                 │
          (virtual dispatch)
                 │
    ┌────────────┴────────────┬──────────────┐
    │                         │              │
┌───▼────────┐    ┌──────────▼──┐    ┌─────▼──────┐
│ CreditCard │    │    PayPal   │    │   Bitcoin  │
│ - Validate │    │ - Login OAuth│   │ - Wallet  │
│ - Charge   │    │ - Transfer  │    │ - Sign Tx │
└────────────┘    └─────────────┘    └────────────┘
```

---

## 💳 Implementacja: Payment Gateway

### Klasa Bazowa

W kodzie demonstracyjnym klasa bazowa używa metod `virtual` z **domyślną implementacją** (każda płatność może ją rozszerzyć lub zastąpić). Klasa jest `abstract`, więc nie można utworzyć "gołej" płatności:

```csharp
public abstract class PaymentMethod
{
    public string Name { get; set; } = "";
    public string TransactionId { get; set; } = "";
    public PaymentStatus Status { get; set; }

    public virtual void Authorize() { /* domyślna autoryzacja */ }

    public virtual void ProcessPayment()
    {
        Authorize();                       // wywołanie wirtualne - zadziała wersja potomka
        // ...
        Status = PaymentStatus.Completed;
    }

    public virtual void GenerateReceipt() { /* domyślny paragon */ }
    public virtual void Refund() { /* domyślny zwrot */ }
}
```

Gdyby każdy typ płatności *musiał* dostarczyć własną logikę (brak sensownego zachowania domyślnego), użylibyśmy `abstract` zamiast `virtual` - patrz moduł 05, temat 6.

### Konkretne Implementacje

```csharp
public class CreditCardPayment : PaymentMethod
{
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    
    public override void Authorize()
    {
        Console.WriteLine("✓ Validating card...");
        // Logika walidacji
    }
    
    public override void ProcessPayment()
    {
        Authorize();
        Console.WriteLine("💳 Charging credit card...");
        TransactionId = Guid.NewGuid().ToString();
    }
    
    public override void GenerateReceipt()
    {
        Console.WriteLine($"Credit Card Receipt: {TransactionId}");
    }
}
```

---

## 🏪 Order Processing System

```csharp
public class Order
{
    public string OrderId { get; set; } = "";
    public List<Item> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.Total);   // wyliczane z pozycji
}

public class OrderProcessor
{
    // Uniwersalny kod - pracuje z KAŻDĄ metodą płatności!
    public bool ProcessOrder(Order order, PaymentMethod payment)
    {
        try
        {
            Console.WriteLine($"Processing order {order.OrderId}...");
            
            payment.ProcessPayment();  // Late binding - każdy robi coś innego!
            payment.GenerateReceipt();
            
            Console.WriteLine("✓ Order completed successfully\n");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error: {ex.Message}\n");
            return false;
        }
    }
}
```

> **Uwaga:** `catch (Exception)` z wypisaniem komunikatu to uproszczenie na potrzeby demonstracji. W kodzie produkcyjnym łap konkretne wyjątki domenowe, loguj je i nie ukrywaj błędów płatności - błąd "połknięty" w tym miejscu oznaczałby, że klient nie dowie się, czy został obciążony.

---

## 📊 Diagram: E-Commerce Payment Flow

```mermaid
graph LR
    A["Order Received<br/>Order processor"] 
    B["Select Payment Method<br/>User chooses"]
    C["PaymentMethod payment<br/>Variable assignment"]
    D{Runtime Type?}
    E["CreditCard<br/>Authorize & Charge"]
    F["PayPal<br/>OAuth Login"]
    G["Bitcoin<br/>Blockchain"]
    H["Generate Receipt<br/>Virtual dispatch"]
    I["Order Complete"]
    
    A --> B
    B --> C
    C --> D
    D -->|CreditCard| E
    D -->|PayPal| F
    D -->|Bitcoin| G
    E --> H
    F --> H
    G --> H
    H --> I
    
    style D fill:#ffcccc
    style C fill:#ccffcc
    style H fill:#ccffff
```

---

## 🔐 Bezpieczeństwo: Chained Authorization (szkic, nie ma go w kodzie demonstracyjnym)

```csharp
public interface IValidator
{
    bool IsValid(PaymentMethod payment, Order order);
}

public class SecurePaymentProcessor
{
    private readonly IReadOnlyList<IValidator> validators;

    public SecurePaymentProcessor(IReadOnlyList<IValidator> validators)
        => this.validators = validators;
    
    public bool Process(PaymentMethod payment, Order order)
    {
        // Każdy walidator może zawetować płatność (polimorfizm przez interfejs)
        foreach (var validator in validators)
        {
            if (!validator.IsValid(payment, order))
                return false;
        }
        
        payment.ProcessPayment();
        return true;
    }
}
```

---

## 📦 Real-World: E-Commerce Platforms

### Architektura w stylu platform e-commerce

```
User → Order → PaymentGateway
                    ├─ Stripe
                    ├─ Square
                    ├─ Authorize.net
                    ├─ PayPal
                    └─ Custom Integration
```

Wspólnym kontraktem jest typ abstrakcyjny lub interfejs (w tym temacie rolę kontraktu pełni klasa bazowa `PaymentMethod`, w [temacie 5](../_05_adapter_factory/README.md) - `PaymentGateway`). Każda implementacja ma:
- Inną walidację
- Inną komunikację z API
- Inne opłaty
- Inną szybkość

Ale dla kodu e-commerce - nie ma różnicy!

---

## 💰 Bonus Features: Pricing Strategies

```csharp
public abstract class PricingStrategy
{
    public abstract decimal ApplyDiscount(decimal amount);
}

public class CreditCardPrice : PricingStrategy
{
    public override decimal ApplyDiscount(decimal amount)
    {
        return amount * 0.98m;  // 2% discount
    }
}

public class PayPalPrice : PricingStrategy
{
    public override decimal ApplyDiscount(decimal amount)
    {
        return amount * 0.95m;  // 5% discount (jak w kodzie demonstracyjnym)
    }
}

// Użycie:
decimal finalPrice = strategy.ApplyDiscount(100m);  // wywołanie wirtualne
```

To drugi, niezależny przykład polimorfizmu: strategia rabatu jest osobną hierarchią (wzorzec Strategy), wybieraną np. według metody płatności.

---

## ⚡ Performance Considerations

| Aspekt | Wpływ |
|--------|-------|
| **Wywołanie virtual** | Może mieć niewielki koszt względem wywołania bezpośredniego |
| **Lookup metody** | Szczegół implementacyjny CLR, niewymagający ręcznej optymalizacji |
| **Inlining** | JIT może devirtualizować i inline'ować znane wywołania |

**Konkluzja**: nie zakładaj konkretnej liczby nanosekund bez benchmarku. W kodzie biznesowym ważniejsza jest poprawna abstrakcja; optymalizuj dopiero po pomiarze.

---

## 🎓 Cele uczenia

Po tym temacie powinieneś:
- [ ] Zrozumieć praktyczne zastosowanie polimorfizmu
- [ ] Wiedzieć, jak projektować hierarchie klas
- [ ] Umieć pracować z wirtualnymi metodami
- [ ] Rozumieć, dlaczego uniwersalny kod (np. procesor zamówień) nie musi znać konkretnych typów płatności

---

## 📚 Referencje

- [Patterns of Enterprise Application Architecture (Fowler)](https://martinfowler.com/eaaCatalog/)
- [Stripe API Design](https://stripe.com/docs/api)
- [Strategy Pattern](https://refactoring.guru/design-patterns/strategy)

---

## 📁 Kod Demonstracyjny

Znajduje się w katalogu `code/`:
- `Program.cs` - Pełny e-commerce payment system

Uruchomienie:
```bash
cd code
dotnet run
dotnet test
```

---

## 🧭 Nawigacja

| Przejdź do | Link |
|---|---|
| Poprzedni temat | [Temat 1: Funkcje wirtualne](../_01_virtual_methods_intro/README.md) |
| Moduł | [06-polimorfizm](../README.md) |
| Następny temat | [Temat 3: Virtual vs New](../_03_virtual_vs_new/README.md) |
