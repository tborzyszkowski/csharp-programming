# Zadania: Abstract Classes

## Zadanie 1: Payment System (Abstract)

Stwórz:
- `Payment(abstract Process())`
- `CreditCard : Payment`
- `Bitcoin : Payment`

## Zadanie 2: Abstract Property

Stwórz abstract class z abstract property i implementuj w pochodnej.

## Zadanie 3: Mix Abstract i Virtual

Stwórz abstract class z:
- Abstract member
- Virtual member (z implementacją)
- Regular member

---

## Rozwiązania

### Zadanie 1
```csharp
public abstract class Payment
{
    public abstract void Process();
}

public class CreditCard : Payment
{
    public override void Process() => Console.WriteLine("Card processing");
}

public class Bitcoin : Payment
{
    public override void Process() => Console.WriteLine("Crypto processing");
}
```
