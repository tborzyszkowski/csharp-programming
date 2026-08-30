# Zadania - Definicja Klasy w Języku C#

## 📝 Zadanie 1: Klasa Prostokąt

### Opis

Utwórz klasę `Rectangle` reprezentującą prostokąt na płaszczyźnie.

### Wymagania

1. **Pola prywatne**:
   - `width` (double) - szerokość
   - `height` (double) - wysokość

2. **Konstruktory**:
   - Bezparametrowy: szerokość=1, wysokość=1
   - Z parametrami: `Rectangle(double width, double height)`

3. **Właściwości**:
   - `Width` i `Height` (get/set z walidacją - muszą być > 0)
   - `Area` (get - pole, read-only)
   - `Perimeter` (get - obwód, read-only)

4. **Metody**:
   - `Resize(double newWidth, double newHeight)` - zmienia wymiary
   - `ToString()` - zwraca "[width x height]"

### Testy

```csharp
var rect = new Rectangle(5, 3);
Assert.Equal(15, rect.Area);      // 5 * 3
Assert.Equal(16, rect.Perimeter); // 2*(5+3)

rect.Resize(4, 4);
Assert.Equal(16, rect.Area);      // 4 * 4
```

---

## 📝 Zadanie 2: Klasa NumerCiągu (NumericSequence)

### Opis

Utwórz klasę reprezentującą ciąg arytmetyczny.

### Wymagania

1. **Pola prywatne**:
   - `firstTerm` (double) - pierwszy wyraz
   - `difference` (double) - różnica ciągu
   - `length` (int) - liczba wyrazów

2. **Konstruktor**:
   - `NumericSequence(double first, double diff, int len)`

3. **Właściwości**:
   - `FirstTerm`, `Difference`, `Length` (read-only)
   - `Sum` - suma wszystkich wyrazów (read-only, obliczana)

4. **Metody**:
   - `GetTerm(int n)` - zwraca n-ty wyraz (1-indexed)
   - `IsIncreasing()` - czy ciąg rosnący
   - `ToString()` - "First: X, Diff: Y, Length: Z"

### Testy

```csharp
var seq = new NumericSequence(2, 3, 5);  // 2, 5, 8, 11, 14
Assert.Equal(2, seq.GetTerm(1));
Assert.Equal(5, seq.GetTerm(2));
Assert.Equal(14, seq.GetTerm(5));
Assert.Equal(40, seq.Sum);  // 2+5+8+11+14
Assert.True(seq.IsIncreasing());
```

---

## 📝 Zadanie 3: Klasa Pracownik z Walidacją

### Opis

Utwórz klasę `Employee` z kompleksową walidacją danych.

### Wymagania

1. **Pola prywatne**:
   - `_id` (int) - unikalny identyfikator (auto-incrementing)
   - `_name` (string)
   - `_salary` (decimal)
   - `_department` (string)

2. **Statyczne pole**:
   - `_nextId` - śledzenie ID

3. **Konstruktor**:
   - `Employee(string name, decimal salary, string department)`
   - Auto-generowanie ID

4. **Właściwości**:
   - `Id` (read-only)
   - `Name` (get/set, nie może być pusty)
   - `Salary` (get/set, musi być >= 0, musi być >= 1500)
   - `Department` (get/set)

5. **Metody**:
   - `GiveRaise(decimal percent)` - zwiększa pensję o X%
   - `IsHighEarner()` - zwraca true jeśli zarabia >= 5000
   - `GetEmploymentDetails()` - zwraca sformatowany tekst

### Walidacja

- Nazwa: nie może być null ani pusta
- Salary: musi być >= 1500
- Department: nie może być null

### Testy

```csharp
var emp = new Employee("John", 3000, "IT");
Assert.Equal(1, emp.Id);
Assert.Equal("John", emp.Name);
Assert.Equal(3000, emp.Salary);

emp.GiveRaise(10);  // +10%
Assert.Equal(3300, emp.Salary);

var emp2 = new Employee("Jane", 5000, "HR");
Assert.Equal(2, emp2.Id);
Assert.True(emp2.IsHighEarner());
```

---

## ✅ Zadanie 1 - Rozwiązanie

```csharp
public class Rectangle
{
    private double width;
    private double height;
    
    public Rectangle()
    {
        width = 1;
        height = 1;
    }
    
    public Rectangle(double width, double height)
    {
        this.width = width > 0 ? width : 1;
        this.height = height > 0 ? height : 1;
    }
    
    public double Width
    {
        get { return width; }
        set { width = value > 0 ? value : width; }
    }
    
    public double Height
    {
        get { return height; }
        set { height = value > 0 ? value : height; }
    }
    
    public double Area => width * height;
    public double Perimeter => 2 * (width + height);
    
    public void Resize(double newWidth, double newHeight)
    {
        if (newWidth > 0 && newHeight > 0)
        {
            width = newWidth;
            height = newHeight;
        }
    }
    
    public override string ToString() => $"[{width} x {height}]";
}
```

---

## ✅ Zadanie 2 - Rozwiązanie

```csharp
public class NumericSequence
{
    private double firstTerm;
    private double difference;
    private int length;
    
    public NumericSequence(double first, double diff, int len)
    {
        firstTerm = first;
        difference = diff;
        length = len > 0 ? len : 1;
    }
    
    public double FirstTerm => firstTerm;
    public double Difference => difference;
    public int Length => length;
    
    public double Sum
    {
        get
        {
            // Suma ciągu arytmetycznego: S = n/2 * (2a + (n-1)d)
            return (length / 2.0) * (2 * firstTerm + (length - 1) * difference);
        }
    }
    
    public double GetTerm(int n)
    {
        if (n < 1 || n > length) return 0;
        return firstTerm + (n - 1) * difference;
    }
    
    public bool IsIncreasing() => difference > 0;
    
    public override string ToString()
    {
        return $"First: {firstTerm}, Diff: {difference}, Length: {length}";
    }
}
```

---

## ✅ Zadanie 3 - Rozwiązanie

```csharp
public class Employee
{
    private int id;
    private string name;
    private decimal salary;
    private string department;
    private static int nextId = 1;
    private const decimal MinSalary = 1500;
    
    public Employee(string name, decimal salary, string department)
    {
        id = nextId++;
        Name = name;
        Salary = salary;
        Department = department;
    }
    
    public int Id => id;
    
    public string Name
    {
        get { return name; }
        set { name = !string.IsNullOrEmpty(value) ? value : name; }
    }
    
    public decimal Salary
    {
        get { return salary; }
        set { salary = value >= MinSalary ? value : salary; }
    }
    
    public string Department
    {
        get { return department; }
        set { department = !string.IsNullOrEmpty(value) ? value : department; }
    }
    
    public void GiveRaise(decimal percent)
    {
        if (percent > 0)
            Salary = salary * (1 + percent / 100);
    }
    
    public bool IsHighEarner() => salary >= 5000;
    
    public string GetEmploymentDetails()
    {
        return $"ID: {id}, Name: {name}, Salary: {salary:C}, " +
               $"Department: {department}, High Earner: {IsHighEarner()}";
    }
}
```

---

## 🎓 Refleksja

1. **Dlaczego walidacja w setterach jest ważna?** Jakie problemy mogą się pojawić bez niej?
2. **Statyczne pola**: Kiedy ich używać i dlaczego mogą być niebezpieczne?
3. **Read-only właściwości**: Kiedy ich używać zamiast zwykłych pól?

