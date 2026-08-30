# Walidacja w Właściwościach

## 🎯 Cel rozdziału

Zrozumienie technik walidacji w setterach właściwości - backing fields, throw exceptions, side effects.

## 📚 Spis treści

1. [Walidacja w Setterze](#walidacja-w-setterze)
2. [Backing Field Pattern](#backing-field-pattern)
3. [Exceptions vs Ignorowanie](#exceptions-vs-ignorowanie)
4. [Property Changed Events](#property-changed-events)

---

## Walidacja w Setterze

Setter to idealne miejsce na **walidację i logikę**:

```csharp
public class Person
{
    private int _age;
    
    public int Age
    {
        get { return _age; }
        set
        {
            if (value >= 0 && value <= 150)
                _age = value;
            else
                throw new ArgumentException("Age must be 0-150");
        }
    }
}

var person = new Person();
person.Age = 30;  // OK
person.Age = -5;  // Exception!
```

---

## Backing Field Pattern

### Standard Pattern

```csharp
public class BankAccount
{
    private decimal _balance;
    
    public decimal Balance
    {
        get { return _balance; }
        private set { _balance = value; }
    }
    
    public bool Deposit(decimal amount)
    {
        if (amount > 0)
        {
            _balance += amount;
            return true;
        }
        return false;
    }
}
```

---

## Exceptions vs Ignorowanie

### Throw Exception

```csharp
public class StrictValidation
{
    private int _age;
    
    public int Age
    {
        set
        {
            if (value < 0)
                throw new ArgumentException("Age cannot be negative");
            _age = value;
        }
    }
}

var obj = new StrictValidation();
obj.Age = -5;  // Exception!
```

### Silent Ignore

```csharp
public class SilentValidation
{
    private int _age;
    
    public int Age
    {
        set
        {
            if (value >= 0)
                _age = value;
            // Else: ignoruj
        }
    }
}

var obj = new SilentValidation();
obj.Age = -5;  // Brak wyjątku, wartość się nie zmienia
```

**Kiedy użyć?**
- **Exception**: Gdy bład jest krytyczny
- **Ignore**: Gdy chcesz "soft fail"

---

## Property Changed Events

```csharp
public class Person
{
    private string _name = string.Empty;
    
    public event Action<string, string>? PropertyChanged;
    
    public string Name
    {
        get { return _name; }
        set
        {
            if (value != _name)
            {
                PropertyChanged?.Invoke("Name", value);
                _name = value;
            }
        }
    }
}

var person = new Person();
person.PropertyChanged += (prop, newValue) =>
    Console.WriteLine($"{prop} changed to {newValue}");

person.Name = "John";  // Event fires
```

---

## Best Practices

✅ **Walidacja blisko danych** - w setterze

✅ **Fail fast** - rzuć exception dla błędów krytycznych

✅ **Consistency** - loguj/trigger events przy zmianach

✅ **Dokumentuj** - jakie wartości są akceptowane

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
