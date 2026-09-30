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
   - `Width` i `Height` (get/set z walidacją - muszą być > 0; w przeciwnym razie `ArgumentOutOfRangeException`)
   - `Area` (get - pole, read-only)
   - `Perimeter` (get - obwód, read-only)

4. **Metody**:
   - `Resize(double newWidth, double newHeight)` - zmienia wymiary (ta sama walidacja)
   - `ToString()` - zwraca "[width x height]"

### Testy

```csharp
var rect = new Rectangle(5, 3);
Assert.Equal(15, rect.Area);      // 5 * 3
Assert.Equal(16, rect.Perimeter); // 2*(5+3)

rect.Resize(4, 4);
Assert.Equal(16, rect.Area);      // 4 * 4

Assert.Throws<ArgumentOutOfRangeException>(() => new Rectangle(0, 5));
Assert.Throws<ArgumentOutOfRangeException>(() => rect.Width = -1);
```

---

## 📝 Zadanie 2: Ciąg arytmetyczny (`NumericSequence`)

### Opis

Utwórz klasę reprezentującą ciąg arytmetyczny.

### Wymagania

1. **Pola prywatne**:
   - `firstTerm` (double) - pierwszy wyraz
   - `difference` (double) - różnica ciągu
   - `length` (int) - liczba wyrazów

2. **Konstruktor**:
   - `NumericSequence(double first, double diff, int len)` - `len` musi być >= 1 (`ArgumentOutOfRangeException`)

3. **Właściwości**:
   - `FirstTerm`, `Difference`, `Length` (read-only)
   - `Sum` - suma wszystkich wyrazów (read-only, obliczana)

4. **Metody**:
   - `GetTerm(int n)` - zwraca n-ty wyraz (1-indexed); dla `n` spoza zakresu `1..Length` rzuca `ArgumentOutOfRangeException`
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
   - `Name` (get/set, nie może być null ani pusty)
   - `Salary` (get/set, musi być >= 1500)
   - `Department` (get/set, nie może być null ani pusty)

5. **Metody**:
   - `GiveRaise(decimal percent)` - zwiększa pensję o X%
   - `IsHighEarner()` - zwraca true jeśli zarabia >= 5000
   - `GetEmploymentDetails()` - zwraca sformatowany tekst

### Walidacja

- Niepoprawna wartość (także w konstruktorze) => `ArgumentException` / `ArgumentOutOfRangeException`.
  Dzięki temu nie da się utworzyć obiektu w niepoprawnym stanie.

### Testy

```csharp
var emp = new Employee("John", 3000, "IT");
Assert.Equal("John", emp.Name);
Assert.Equal(3000, emp.Salary);

emp.GiveRaise(10);  // +10%
Assert.Equal(3300, emp.Salary);

var emp2 = new Employee("Jane", 5000, "HR");
Assert.Equal(emp.Id + 1, emp2.Id);   // pole statyczne - porównujemy względnie!
Assert.True(emp2.IsHighEarner());

Assert.Throws<ArgumentException>(() => new Employee("", 3000, "IT"));
Assert.Throws<ArgumentOutOfRangeException>(() => new Employee("Ann", 1000, "IT"));
```

> **Uwaga:** licznik `_nextId` jest wspólny dla wszystkich testów, więc test nie może zakładać,
> że pierwszy utworzony obiekt ma `Id == 1` (kolejność testów nie jest gwarantowana).

---

## ✅ Zadanie 1 - Rozwiązanie

```csharp
public class Rectangle
{
    private double _width;
    private double _height;
    
    public Rectangle() : this(1, 1) { }
    
    public Rectangle(double width, double height)
    {
        Width = width;     // przez właściwość - walidacja w jednym miejscu
        Height = height;
    }
    
    public double Width
    {
        get => _width;
        set => _width = value > 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), "Szerokość musi być dodatnia");
    }
    
    public double Height
    {
        get => _height;
        set => _height = value > 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), "Wysokość musi być dodatnia");
    }
    
    public double Area => _width * _height;
    public double Perimeter => 2 * (_width + _height);
    
    public void Resize(double newWidth, double newHeight)
    {
        // Najpierw walidujemy OBA wymiary, żeby nie zostawić obiektu w połowie zmienionym
        if (newWidth <= 0) throw new ArgumentOutOfRangeException(nameof(newWidth));
        if (newHeight <= 0) throw new ArgumentOutOfRangeException(nameof(newHeight));
        
        _width = newWidth;
        _height = newHeight;
    }
    
    public override string ToString() => $"[{_width} x {_height}]";
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
        if (len < 1)
            throw new ArgumentOutOfRangeException(nameof(len), "Ciąg musi mieć co najmniej jeden wyraz");
        
        firstTerm = first;
        difference = diff;
        length = len;
    }
    
    public double FirstTerm => firstTerm;
    public double Difference => difference;
    public int Length => length;
    
    // Suma ciągu arytmetycznego: S = n/2 * (2a + (n-1)d)
    public double Sum => (length / 2.0) * (2 * firstTerm + (length - 1) * difference);
    
    public double GetTerm(int n)
    {
        if (n < 1 || n > length)
            throw new ArgumentOutOfRangeException(nameof(n), $"n musi należeć do zakresu 1..{length}");
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
    private const decimal MinSalary = 1500;
    private static int _nextId = 1;
    
    private readonly int _id;
    private string _name = "";
    private decimal _salary;
    private string _department = "";
    
    public Employee(string name, decimal salary, string department)
    {
        // Przez właściwości - każda niepoprawna wartość przerywa tworzenie obiektu wyjątkiem
        Name = name;
        Salary = salary;
        Department = department;
        
        // Id nadajemy dopiero po udanej walidacji, żeby nie "marnować" numerów
        _id = Interlocked.Increment(ref _nextId) - 1;   // atomowo, bezpieczne dla wielu wątków
    }
    
    public int Id => _id;
    
    public string Name
    {
        get => _name;
        set => _name = !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException("Imię nie może być puste", nameof(value));
    }
    
    public decimal Salary
    {
        get => _salary;
        set => _salary = value >= MinSalary
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), $"Pensja musi być >= {MinSalary}");
    }
    
    public string Department
    {
        get => _department;
        set => _department = !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException("Dział nie może być pusty", nameof(value));
    }
    
    public void GiveRaise(decimal percent)
    {
        if (percent <= 0)
            throw new ArgumentOutOfRangeException(nameof(percent), "Podwyżka musi być dodatnia");
        Salary = _salary * (1 + percent / 100);
    }
    
    public bool IsHighEarner() => _salary >= 5000;
    
    public string GetEmploymentDetails()
    {
        return $"ID: {_id}, Name: {_name}, Salary: {_salary:C}, " +
               $"Department: {_department}, High Earner: {IsHighEarner()}";
    }
}
```

---

## 🎓 Refleksja

1. **Dlaczego walidacja w setterach jest ważna?** Jakie problemy mogą się pojawić bez niej?
2. **Wyjątek czy po cichu zignorowana wartość?** Porównaj oba podejścia - które łatwiej debugować?
3. **Statyczne pola**: Kiedy ich używać i dlaczego mogą być niebezpieczne (stan wspólny, wielowątkowość, testy)?
4. **Read-only właściwości**: Kiedy ich używać zamiast zwykłych pól?
5. Dlaczego w konstruktorze `Employee` przypisujemy wartości przez właściwości, a nie bezpośrednio do pól?

