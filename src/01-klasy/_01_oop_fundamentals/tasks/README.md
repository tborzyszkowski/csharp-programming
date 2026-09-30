# Zadania - Programowanie Obiektowe - Podstawowe Pojęcia

## 📝 Zadanie 1: Klasa pojazdu z polimorfizmem

### Opis

Utwórz system zarządzania pojazami, który demonstruje wszystkie cztery filary OOP.

### Wymagania

1. **Abstrakcja**: Stwórz abstrakcyjną klasę `Vehicle` z metodami:
   - `abstract void Start()`
   - `abstract void Stop()`
   - `virtual void Honk()` - zwraca dźwięk

2. **Enkapsulacja**: Klasa powinna mieć:
   - Prywatne pole `speed` (int)
   - Publiczną property `Speed` (tylko do odczytu)
   - Metodę `Accelerate(int increase)` i `Brake(int decrease)` która zmienia speed

3. **Dziedziczenie**: Utwórz 3 klasy pochodne:
   - `Car` - rodzinny samochód
   - `Motorcycle` - motocykl
   - `Truck` - ciężarówka

4. **Polimorfizm**: Każdy pojazd powinien:
   - Inaczej uruchomić się `Start()`
   - Inaczej się zatrzymać `Stop()`
   - Inaczej zatrąbić `Honk()`

### Przykład wyjścia

```
Toyota car: Silnik benzynowy rusza...
Harley motorcycle: Silnik 2-cylindrowy warczy!
Volvo truck: Diesel włącza się z hukiem!

Toyota car: Pip! Pip!
Harley motorcycle: Brrrrr!
Volvo truck: HUUUUU!
```

### 🎯 Wskazówki

- Używaj abstract class dla Vehicle
- Każda klasa pochodna musi zaimplementować wszystkie abstrakcyjne metody
- Przetestuj każdy pojazd w kolekcji `Vehicle[]`
- `Brake` nie może sprowadzić prędkości poniżej 0

---

## 📝 Zadanie 2: System biblioteki z enkapsulacją

### Opis

Stwórz system zarządzania biblioteką, gdzie książki są chronione przed niewłaściwymi operacjami.

### Wymagania

1. **Klasa Book**:
   - `private` pola `availableCopies` (liczba dostępnych kopii) i `totalCopies` (ile egzemplarzy ma biblioteka)
   - `public` property `Title`, `Author`
   - `public` property `AvailableCopies` (tylko do odczytu)
   - `public` method `Borrow()` - zmniejsza kopie (jeśli > 0), zwraca `bool`
   - `public` method `Return()` - zwiększa kopie, zwraca `bool`

2. **Klasa Library**:
   - Przechowuje książki w kolekcji
   - `public` method `AddBook(Book)`
   - `public` method `BorrowBook(string title)`
   - `public` method `ReturnBook(string title)`
   - `public` method `GetBookInfo(string title)` - zwraca opis (`string`) lub informację, że książki brak

3. **Walidacja**:
   - Nie można pożyczyć książki, jeśli kopie < 1
   - Nie można zwrócić więcej egzemplarzy niż biblioteka posiada w sumie (`availableCopies <= totalCopies`)

### 🎯 Wskazówki

- Używaj enkapsulacji do ochrony `availableCopies`
- Każda operacja powinna wypisać komunikat o powodzeniu/błędzie
- Przechowuj książki w `Dictionary<string, Book>`; do wyszukiwania użyj `TryGetValue`

---

## 📝 Zadanie 3: Hierarchia pracowników

### Opis

Stwórz system payroll dla różnych typów pracowników z polimorfizmem.

### Wymagania

1. **Klasa Employee** (bazowa, abstrakcyjna):
   - `string Name`, `decimal BaseSalary`, `int EmployeeId`
   - `virtual decimal CalculateSalary()`
   - `virtual void PrintDetails()`

2. **Klasy pochodne** (każda wylicza pensję inaczej):
   - **FullTimeEmployee**: BaseSalary + Health Insurance (200)
   - **PartTimeEmployee**: BaseSalary * 0.5 (pracuje pół etatu)
   - **Contractor**: BaseSalary * 1.1 (brak benefitów +10%)
   - **Manager**: BaseSalary + Bonus (parametr)

3. **Klasa PayrollSystem**:
   - Przechowuje pracowników
   - `CalculateTotalPayroll()` - suma wszystkich pensji
   - `PrintPayroll()` - wypisuje szczegóły dla każdego
   - `GetEmployeesSalaryAbove(decimal amount)` - pracownicy zarabiający ponad X

### Przykład

```
=== PAYROLL SYSTEM ===
John (Full-time):     2200.00
Jane (Manager):       3500.00
Bob (Part-time):      1000.00
Alice (Contractor):   1100.00
─────────────────────
TOTAL:                7800.00
```

### 🎯 Wskazówki

- Używaj `List<Employee>`
- Polimorfizm: każdy typ pracownika inaczej liczy pensję
- Do sformatowania kolumn użyj wyrównania w interpolacji: `{Name,-20}` (lewe) i `{value,10:C}` (prawe)
- Przetestuj przy użyciu xUnit

---

## ✅ Zadanie 1 - Rozwiązanie

```csharp
using System;
using System.Collections.Generic;

public abstract class Vehicle
{
    private int speed;
    
    public int Speed => speed;
    public string Brand { get; set; }
    
    public Vehicle(string brand)
    {
        Brand = brand;
        speed = 0;
    }
    
    public abstract void Start();
    public abstract void Stop();
    
    public virtual void Honk()
    {
        Console.WriteLine("Generic horn sound");
    }
    
    public void Accelerate(int increase)
    {
        if (increase <= 0) return;
        
        speed += increase;
        Console.WriteLine($"Speed increased to {speed} km/h");
    }
    
    public void Brake(int decrease)
    {
        if (decrease <= 0) return;
        
        speed = Math.Max(0, speed - decrease);   // prędkość nigdy nie spada poniżej 0
        Console.WriteLine(speed == 0 ? "Vehicle stopped!" : $"Speed decreased to {speed} km/h");
    }
}

public class Car : Vehicle
{
    public Car(string brand) : base(brand) { }
    
    public override void Start()
    {
        Console.WriteLine($"{Brand} car: Silnik benzynowy rusza...");
    }
    
    public override void Stop()
    {
        Console.WriteLine($"{Brand} car: Hamulce hydrauliczne!");
    }
    
    public override void Honk()
    {
        Console.WriteLine($"{Brand} car: Pip! Pip!");
    }
}

public class Motorcycle : Vehicle
{
    public Motorcycle(string brand) : base(brand) { }
    
    public override void Start()
    {
        Console.WriteLine($"{Brand} motorcycle: Silnik 2-cylindrowy warczy!");
    }
    
    public override void Stop()
    {
        Console.WriteLine($"{Brand} motorcycle: Hamulce tarczowe!");
    }
    
    public override void Honk()
    {
        Console.WriteLine($"{Brand} motorcycle: Brrrrr!");
    }
}

public class Truck : Vehicle
{
    public Truck(string brand) : base(brand) { }
    
    public override void Start()
    {
        Console.WriteLine($"{Brand} truck: Diesel włącza się z hukiem!");
    }
    
    public override void Stop()
    {
        Console.WriteLine($"{Brand} truck: Powietrzne hamulce!");
    }
    
    public override void Honk()
    {
        Console.WriteLine($"{Brand} truck: HUUUUU!");
    }
}

// DEMO
public class Program
{
    public static void Main()
    {
        Vehicle[] vehicles = new Vehicle[]
        {
            new Car("Toyota"),
            new Motorcycle("Harley"),
            new Truck("Volvo")
        };
        
        Console.WriteLine("=== ALL VEHICLES START ===\n");
        foreach (var v in vehicles)
            v.Start();
        
        Console.WriteLine("\n=== ALL VEHICLES HONK ===\n");
        foreach (var v in vehicles)
            v.Honk();
        
        Console.WriteLine("\n=== ALL VEHICLES STOP ===\n");
        foreach (var v in vehicles)
            v.Stop();
    }
}
```

---

## ✅ Zadanie 2 - Rozwiązanie

```csharp
using System;
using System.Collections.Generic;

public class Book
{
    private int availableCopies;
    private readonly int totalCopies;
    
    public string Title { get; }
    public string Author { get; }
    public int AvailableCopies => availableCopies;
    
    public Book(string title, string author, int copies)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(copies);
        
        Title = title;
        Author = author;
        availableCopies = copies;
        totalCopies = copies;
    }
    
    public bool Borrow()
    {
        if (availableCopies > 0)
        {
            availableCopies--;
            return true;
        }
        return false;
    }
    
    public bool Return()
    {
        // Nie można oddać więcej egzemplarzy, niż biblioteka posiada
        if (availableCopies < totalCopies)
        {
            availableCopies++;
            return true;
        }
        return false;
    }
    
    public override string ToString() => $"'{Title}' - {Author} (dostępne: {availableCopies}/{totalCopies})";
}

public class Library
{
    private readonly Dictionary<string, Book> books = new();
    
    public void AddBook(Book book)
    {
        if (books.TryAdd(book.Title, book))
            Console.WriteLine($"✓ '{book.Title}' dodana do biblioteki");
        else
            Console.WriteLine($"✗ '{book.Title}' już istnieje w bibliotece");
    }
    
    public bool BorrowBook(string title)
    {
        if (books.TryGetValue(title, out var book) && book.Borrow())
        {
            Console.WriteLine($"✓ '{title}' pożyczona. Zostało: {book.AvailableCopies}");
            return true;
        }
        Console.WriteLine($"✗ '{title}' niedostępna!");
        return false;
    }
    
    public bool ReturnBook(string title)
    {
        if (books.TryGetValue(title, out var book) && book.Return())
        {
            Console.WriteLine($"✓ '{title}' zwrócona. Dostępnych: {book.AvailableCopies}");
            return true;
        }
        Console.WriteLine($"✗ Nie można zwrócić '{title}'");
        return false;
    }
    
    public string GetBookInfo(string title)
    {
        return books.TryGetValue(title, out var book) ? book.ToString() : $"Brak książki '{title}'";
    }
}

// DEMO
public class Program
{
    public static void Main()
    {
        var library = new Library();
        
        library.AddBook(new Book("Clean Code", "Robert Martin", 3));
        library.AddBook(new Book("Design Patterns", "Gang of Four", 2));
        
        library.BorrowBook("Clean Code");
        library.BorrowBook("Clean Code");
        library.BorrowBook("Clean Code");
        library.BorrowBook("Clean Code");  // Błąd - brak kopii
        
        library.ReturnBook("Clean Code");
        library.BorrowBook("Clean Code");
        
        library.ReturnBook("Design Patterns");   // Błąd - wszystkie egzemplarze już w bibliotece
        
        Console.WriteLine(library.GetBookInfo("Clean Code"));
    }
}
```

### Testy

```csharp
[Fact]
public void Borrow_WhenNoCopies_ReturnsFalse()
{
    var book = new Book("A", "B", 1);
    Assert.True(book.Borrow());
    Assert.False(book.Borrow());
    Assert.Equal(0, book.AvailableCopies);
}

[Fact]
public void Return_CannotExceedTotalCopies()
{
    var book = new Book("A", "B", 2);
    Assert.False(book.Return());            // wszystkie egzemplarze już w bibliotece
    book.Borrow();
    Assert.True(book.Return());
    Assert.Equal(2, book.AvailableCopies);
}

[Fact]
public void GetBookInfo_UnknownTitle_ReturnsMessage()
{
    Assert.Contains("Brak", new Library().GetBookInfo("Nie ma"));
}
```

---

## ✅ Zadanie 3 - Rozwiązanie

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Employee
{
    public string Name { get; }
    public decimal BaseSalary { get; }
    public int EmployeeId { get; }
    
    protected Employee(string name, decimal salary, int id)
    {
        Name = name;
        BaseSalary = salary;
        EmployeeId = id;
    }
    
    public virtual decimal CalculateSalary() => BaseSalary;
    
    public virtual void PrintDetails()
    {
        // {Name,-20} = wyrównanie do lewej w 20 znakach; {x,10:C} = wyrównanie do prawej w 10 znakach + format walutowy
        Console.WriteLine($"{Name,-20} {CalculateSalary(),10:C}");
    }
}

public class FullTimeEmployee : Employee
{
    private const decimal HealthInsurance = 200;
    
    public FullTimeEmployee(string name, decimal salary, int id)
        : base(name, salary, id) { }
    
    public override decimal CalculateSalary()
        => BaseSalary + HealthInsurance;
}

public class PartTimeEmployee : Employee
{
    public PartTimeEmployee(string name, decimal salary, int id)
        : base(name, salary, id) { }
    
    public override decimal CalculateSalary()
        => BaseSalary * 0.5m;
}

public class Contractor : Employee
{
    public Contractor(string name, decimal salary, int id)
        : base(name, salary, id) { }
    
    public override decimal CalculateSalary()
        => BaseSalary * 1.1m;   // brak benefitów => +10%
}

public class Manager : Employee
{
    private readonly decimal bonus;
    
    public Manager(string name, decimal salary, decimal bonus, int id)
        : base(name, salary, id) => this.bonus = bonus;
    
    public override decimal CalculateSalary()
        => BaseSalary + bonus;
}

public class PayrollSystem
{
    private readonly List<Employee> employees = new();
    
    public void AddEmployee(Employee emp)
        => employees.Add(emp);
    
    public decimal CalculateTotalPayroll()
        => employees.Sum(e => e.CalculateSalary());
    
    public IEnumerable<Employee> GetEmployeesSalaryAbove(decimal amount)
        => employees.Where(e => e.CalculateSalary() > amount);
    
    public void PrintPayroll()
    {
        Console.WriteLine("=== PAYROLL SYSTEM ===\n");
        foreach (var emp in employees)
            emp.PrintDetails();   // polimorfizm: każdy typ pracownika liczy pensję po swojemu
        Console.WriteLine($"\n{"TOTAL:",-20} {CalculateTotalPayroll(),10:C}");
    }
}

// DEMO
public class Program
{
    public static void Main()
    {
        var payroll = new PayrollSystem();
        
        payroll.AddEmployee(new FullTimeEmployee("John", 2000, 1));
        payroll.AddEmployee(new Manager("Jane", 3000, 500, 2));
        payroll.AddEmployee(new PartTimeEmployee("Bob", 2000, 3));
        payroll.AddEmployee(new Contractor("Alice", 1000, 4));
        
        payroll.PrintPayroll();
    }
}
```

### Testy

```csharp
[Fact]
public void TotalPayroll_SumsAllEmployeeTypes()
{
    var payroll = new PayrollSystem();
    payroll.AddEmployee(new FullTimeEmployee("John", 2000, 1));   // 2200
    payroll.AddEmployee(new Manager("Jane", 3000, 500, 2));       // 3500
    payroll.AddEmployee(new PartTimeEmployee("Bob", 2000, 3));    // 1000
    payroll.AddEmployee(new Contractor("Alice", 1000, 4));        // 1100

    Assert.Equal(7800m, payroll.CalculateTotalPayroll());
}

[Fact]
public void GetEmployeesSalaryAbove_FiltersByCalculatedSalary()
{
    var payroll = new PayrollSystem();
    payroll.AddEmployee(new FullTimeEmployee("John", 2000, 1));   // 2200
    payroll.AddEmployee(new PartTimeEmployee("Bob", 2000, 3));    // 1000

    var result = payroll.GetEmployeesSalaryAbove(2000).ToList();

    Assert.Single(result);
    Assert.Equal("John", result[0].Name);
}
```

---

## 🎓 Refleksja i dyskusja

Po wykonaniu zadań, zastanów się nad:

1. **Dlaczego abstrakcja jest ważna?** Jak zmiana implementacji wpłynęłaby na kod klienta?
2. **Enkapsulacja**: Gdyby pola były publiczne, jakie problemy mogłyby się pojawić?
3. **Polimorfizm**: Jak byłby kod bez CalculateSalary() w każdej klasie?
4. **Dziedziczenie**: Jakie powtórzenia kodu eliminuje?

---

## 📚 Materiały dodatkowe

- [Microsoft C# OOP](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/)
- [SOLID Principles](https://en.wikipedia.org/wiki/SOLID)
- [Design Patterns - OOP](https://refactoring.guru/design-patterns/oop)

