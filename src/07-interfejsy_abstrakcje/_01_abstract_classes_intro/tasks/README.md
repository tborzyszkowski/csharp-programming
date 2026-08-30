# 📝 Zadania do Topic 1: Klasy Abstrakcyjne - Wstęp

## Zadanie 1: Rozszerz Animal Hierarchy

### Problem
Masz podstawową hierarchię klas:
```csharp
abstract class AnimalBase
{
    public string Name { get; set; }
    public abstract void Speak();
    public virtual void Sleep() { }
}
```

Masz klasy: `Dog` i `Cat`, ale potrzebujesz dodać nowe zwierzęta.

### Wymagania
1. **Dodaj nową klasę**: `Bird` dziedziczącą z `AnimalBase`
2. **Implementuj `Speak()`**: Bird powinien wypisać "chirp chirp"
3. **Override `Sleep()`**: Dodaj własną implementację "Bird is resting on branch"
4. **Dodaj nową metodę abstrakcyjną**: `string GetHabitat()` (środowisko)
5. **Implementuj w Dog, Cat, Bird**: Każdy zwierz powinien zwrócić swoje środowisko
   - Dog: "Home with humans"
   - Cat: "Indoor apartment"
   - Bird: "Tree"

### Starter Code
```csharp
public class Bird : AnimalBase
{
    public Bird(string name) => Name = name;
    
    // TODO: Implementuj Speak()
    // TODO: Override Sleep()
    // TODO: Implementuj GetHabitat()
}
```

### Oczekiwany Rezultat
```csharp
var bird = new Bird("Tweety");
bird.Speak();         // Tweety: chirp chirp
bird.Sleep();         // Bird is resting on branch
Console.WriteLine(bird.GetHabitat()); // Tree
```

### Test Xunit
```csharp
[Fact]
public void BirdImplementsAnimalContract()
{
    var bird = new Bird("Tweety");
    AnimalBase animal = bird;  // Polimorfizm
    
    // Powinno działać
    animal.Speak();
    animal.Sleep();
    var habitat = bird.GetHabitat();
    
    Assert.Equal("Tree", habitat);
}
```

---

## Zadanie 2: Payment Method Hierarchy

### Problem
System płatności ma tylko `CreditCardPayment` i `PayPalPayment`.

### Wymagania
1. **Dodaj nowy typ płatności**: `BankTransferPayment : PaymentMethodBase`
2. **Implementuj wymagane metody**:
   - `Authorize(decimal amount)`: Wypisz "Authorizing bank transfer for $X"
   - `Charge(decimal amount)`: Wypisz "Charging account for $X via bank"
   - `PrintReceipt()`: Wypisz "Bank Transfer Receipt"
3. **Dodaj walidację**: IBAN musi mieć minimum 15 znaków
4. **Dodaj właściwość**: `string IBAN { get; set; }`

### Starter Code
```csharp
public class BankTransferPayment : PaymentMethodBase
{
    public string IBAN { get; set; }
    
    public BankTransferPayment(string iban)
    {
        if (iban.Length < 15)
            throw new ArgumentException("Invalid IBAN");
        IBAN = iban;
    }
    
    // TODO: Implementuj Authorize, Charge, PrintReceipt
}
```

### Oczekiwany Rezultat
```csharp
var payment = new BankTransferPayment("NL91ABNA0417164300");
payment.Authorize(500m);      // Authorizing bank transfer...
payment.Charge(500m);         // Charging account...
payment.PrintReceipt();       // Bank Transfer Receipt

// Walidacja
var invalid = new BankTransferPayment("short");  // Throws!
```

---

## Rozwiązanie: Zadanie 1

```csharp
public class Bird : AnimalBase
{
    public Bird(string name) => Name = name;
    
    public abstract string GetHabitat();  // ← UWAGA: Dodaj do AnimalBase!
    
    public override void Speak()
    {
        Console.WriteLine($"{Name}: chirp chirp");
    }
    
    public override void Sleep()
    {
        Console.WriteLine("Bird is resting on branch");
    }
    
    public override string GetHabitat() => "Tree";
}

// W AnimalBase, dodaj:
abstract class AnimalBase
{
    // ... istniejące elementy ...
    public abstract string GetHabitat();
}
```

---

## Rozwiązanie: Zadanie 2

```csharp
public class BankTransferPayment : PaymentMethodBase
{
    public string IBAN { get; set; }
    
    public BankTransferPayment(string iban)
    {
        if (iban.Length < 15)
            throw new ArgumentException("Invalid IBAN length");
        IBAN = iban;
    }
    
    public override void Authorize(decimal amount)
    {
        Console.WriteLine($"Authorizing bank transfer for ${amount} to {IBAN}");
    }
    
    public override void Charge(decimal amount)
    {
        Console.WriteLine($"Charging account {IBAN} for ${amount} via bank");
    }
    
    public override void PrintReceipt(decimal amount, string status)
    {
        Console.WriteLine($"═════════════════════════════════════");
        Console.WriteLine($"Bank Transfer Receipt");
        Console.WriteLine($"IBAN: {IBAN}");
        Console.WriteLine($"Amount: ${amount}");
        Console.WriteLine($"Status: {status}");
        Console.WriteLine($"═════════════════════════════════════");
    }
}
```

---

## Dodatkowe Challenges (Advanced)

### Challenge 1: Abstract Property
Dodaj abstrakcyjną właściwość do `AnimalBase`:
```csharp
public abstract int LifeSpanYears { get; }
```
Implementuj w Dog (15), Cat (18), Bird (20).

### Challenge 2: Protected Abstract Method
Dodaj `protected abstract string GetBreedInfo()` - nie widoczna publicznie, ale wymagana w podklasach.

### Challenge 3: Sealed Override
W `Dog`, dodaj `sealed override void Bark()` aby inne klasy nie mogły zmienić implementacji.

---

*Czas do ukończenia: 20-30 minut*  
*Poziom: Intermediate*  
*Koncepty: Abstract classes, inheritance, polymorphism*
