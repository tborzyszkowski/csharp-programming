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
Car starts: Silnik benzynowy rusza...
Motorcycle starts: Silnik 2-cylindrowy warczy!
Truck starts: Diesel włącza się z hukiem!

Car honks: Pip! Pip!
Motorcycle honks: Brrrrr!
Truck honks: HUUUUU!
```

### 🎯 Wskazówki

- Używaj abstract class dla Vehicle
- Każda klasa pochodna musi zaimplementować wszystkie abstrakcyjne metody
- Przetestuj każdy pojazd w kolekcji `Vehicle[]`

---

## 📝 Zadanie 2: System biblioteki z enkapsulacją

### Opis

Stwórz system zarządzania bibliotekę, gdzie książki są chronione przed niewłaściwymi operacjami.

### Wymagania

1. **Klasa Book**:
   - `private` pole `availableCopies` (liczba dostępnych kopii)
   - `public` property `Title`, `Author`
   - `public` property `AvailableCopies` (tylko do odczytu)
   - `public` method `Borrow()` - zmniejsza kopie (jeśli > 0)
   - `public` method `Return()` - zwiększa kopie

2. **Klasa Library**:
   - Przechowuje książki w kolekcji
   - `public` method `AddBook(Book)`
   - `public` method `BorrowBook(string title)`
   - `public` method `ReturnBook(string title)`
   - `public` method `GetBookInfo(string title)`

3. **Walidacja**:
   - Nie można pożyczyć książki jeśli kopie < 1
   - Można zwrócić maksymalnie 5 kopii na raz

### 🎯 Wskazówki

- Używaj enkapsulacji do ochrony `availableCopies`
- Każda operacja powinna wypisać komunikat o powodzeniu/błędzie
- Przechowuj książki w `Dictionary<string, Book>`

---

## 📝 Zadanie 3: Hierarchia pracowników

### Opis

Stwórz system payroll dla różnych typów pracowników z polimorfizmem.

### Wymagania

1. **Klasa Employee** (bazowa):
   - `string Name`, `decimal BaseSalary`, `int EmployeeId`
   - `virtual decimal CalculateSalary()`
   - `virtual void PrintDetails()`

2. **Klasy pochodne** (każda wylicza pensję inaczej):
   - **FullTimeEmployee**: BaseSalary + Heath Insurance (200)
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

- Używaj `Employee[]` lub `List<Employee>`
- Polimorfizm: każdy typ pracownika inaczej liczy pensję
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
        if (increase > 0)
        {
            speed += increase;
            Console.WriteLine($"Speed increased to {speed} km/h");
        }
    }
    
    public void Brake(int decrease)
    {
        if (decrease > 0 && speed - decrease >= 0)
        {
            speed -= decrease;
            Console.WriteLine($"Speed decreased to {speed} km/h");
        }
        else if (decrease > speed)
        {
            speed = 0;
            Console.WriteLine("Vehicle stopped!");
        }
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
    
    public string Title { get; set; }
    public string Author { get; set; }
    public int AvailableCopies => availableCopies;
    
    public Book(string title, string author, int copies)
    {
        Title = title;
        Author = author;
        availableCopies = copies;
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
        availableCopies++;
        return true;
    }
}

public class Library
{
    private Dictionary<string, Book> books;
    
    public Library()
    {
        books = new Dictionary<string, Book>();
    }
    
    public void AddBook(Book book)
    {
        if (!books.ContainsKey(book.Title))
        {
            books.Add(book.Title, book);
            Console.WriteLine($"✓ '{book.Title}' dodana do biblioteki");
        }
    }
    
    public bool BorrowBook(string title)
    {
        if (books.ContainsKey(title) && books[title].Borrow())
        {
            Console.WriteLine($"✓ '{title}' pożyczona. Zostało: {books[title].AvailableCopies}");
            return true;
        }
        Console.WriteLine($"✗ '{title}' niedostępna!");
        return false;
    }
    
    public bool ReturnBook(string title)
    {
        if (books.ContainsKey(title))
        {
            books[title].Return();
            Console.WriteLine($"✓ '{title}' zwrócona. Dostępnych: {books[title].AvailableCopies}");
            return true;
        }
        return false;
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
        library.BorrowBook("Clean Code");  // Błąd
        
        library.ReturnBook("Clean Code");
        library.BorrowBook("Clean Code");
    }
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
    public string Name { get; set; }
    public decimal BaseSalary { get; set; }
    public int EmployeeId { get; set; }
    
    public Employee(string name, decimal salary, int id)
    {
        Name = name;
        BaseSalary = salary;
        EmployeeId = id;
    }
    
    public virtual decimal CalculateSalary() => BaseSalary;
    
    public virtual void PrintDetails()
    {
        Console.WriteLine($"{Name:20} {CalculateSalary():10:C}");
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

public class Manager : Employee
{
    private decimal bonus;
    
    public Manager(string name, decimal salary, decimal bonus, int id)
        : base(name, salary, id) => this.bonus = bonus;
    
    public override decimal CalculateSalary()
        => BaseSalary + bonus;
}

public class PayrollSystem
{
    private List<Employee> employees;
    
    public PayrollSystem() => employees = new List<Employee>();
    
    public void AddEmployee(Employee emp)
        => employees.Add(emp);
    
    public decimal CalculateTotalPayroll()
        => employees.Sum(e => e.CalculateSalary());
    
    public void PrintPayroll()
    {
        Console.WriteLine("=== PAYROLL SYSTEM ===\n");
        foreach (var emp in employees)
            emp.PrintDetails();
        Console.WriteLine($"\nTOTAL: {CalculateTotalPayroll():C}");
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
        
        payroll.PrintPayroll();
    }
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

