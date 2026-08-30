# Zadania: Modyfikatory Dostępu

## Zadanie 1: Encapsulation w Klasie Bazowej

Stwórz klasę `Employee` z:
- `public` property: Name, Salary (ale setter private!)
- `protected` method: CalculateBonus()
- `private` field: taxRate

Potem stwórz `Manager : Employee` i wywołaj protected method.

## Zadanie 2: Protected vs Public

Stwórz hierarchię:
- `Person` - protected property: SSN (Social Security Number)
- `Employee : Person` - publiczny access do SSN via property

Pokaż różnicę między protected i public.

## Zadanie 3: Porównanie C# i Java

Napisz kod pokazujący czemu poniższy kod byłby OK w Javie, ale nie w C#:

```csharp
public class Base { protected void Method() { } }
public class Derived : Base { }
public class Other { 
    void Test() { 
        new Derived().Method();  // OK w Java, ❌ w C#
    } 
}
```

---

## Rozwiązania

### Zadanie 1
```csharp
public class Employee
{
    public string Name { get; set; } = "";
    public double Salary { get; private set; }
    private double taxRate = 0.2;
    
    protected double CalculateBonus()
    {
        return Salary * 0.1;
    }
    
    public void GiveSalary(double amount)
    {
        Salary = amount;
    }
}

public class Manager : Employee
{
    public double GetBonusAmount()
    {
        return CalculateBonus();  // ✅ OK - protected
    }
}
```

### Zadanie 2
```csharp
public class Person
{
    protected string SSN { get; set; } = "";  // Protected - tylko pochodne
}

public class Employee : Person
{
    public string GetSSN() => SSN;  // ✅ OK
}

public class Other
{
    void Test()
    {
        var emp = new Employee();
        // emp.SSN = "123-45-6789";  // ❌ BŁĄD - protected
    }
}
```
