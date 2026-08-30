# Zadania: Virtual Methods i Polymorphism

## Zadanie 1: Payment System

Stwórz:
- `Payment(abstract Process())`
- `Visa : Payment`
- `MasterCard : Payment`
- `Bitcoin : Payment`

Polimorficznie przetwórz listę payments.

## Zadanie 2: Type Checking

Mając listę `List<Animal>`, użyj `is` i `as` do obsługi różnych typów.

## Zadanie 3: Virtual Properties

Stwórz klasę z virtual property i override w klasie pochodnej.

---

## Rozwiązania

### Zadanie 1
```csharp
public abstract class Payment
{
    public abstract void Process();
}

public class Visa : Payment
{
    public override void Process() => Console.WriteLine("Visa processing");
}

public class MasterCard : Payment
{
    public override void Process() => Console.WriteLine("MasterCard processing");
}

List<Payment> payments = new() { new Visa(), new MasterCard() };
foreach (var p in payments) p.Process();
```
