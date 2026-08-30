# Zadania: Sealed Classes

## Zadanie 1: Sealed Final Implementation

Stwórz:
- `Payment(abstract Process())`
- `CreditCard : Payment`
- `sealed class FinalCreditCard : CreditCard`

Pokaż że FinalCreditCard nie może być dziedziczona.

## Zadanie 2: Sealed Override

Stwórz metodę z virtual, potem sealed override, pokaż że nie można dalej override'ować.

## Zadanie 3: Sealed Class

Stwórz sealed class - nie można jej dziedziczć ale można instancjonować.

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
    public override void Process() => Console.WriteLine("Card pay");
}

public sealed class FinalCreditCard : CreditCard
{
    // Nie można dalej dziedziczć
}

// ❌ BŁĄD
// public class Custom : FinalCreditCard { }
```
