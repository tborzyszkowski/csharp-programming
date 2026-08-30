# Zadania - Łańcuchowe Wywołanie Konstruktorów

## 📝 Zadanie 1: Klasa Date z konstruktorami łańcuchowymi

### Opis

Stwórz klasę `Date` reprezentującą datę z łańcuchowymi konstruktorami:
- Konstruktor domyślny - dzisiejsza data
- Konstruktor z rokiem - dziś, ale dany rok
- Konstruktor pełny - rok, miesiąc, dzień
- Wszystkie konstruktory połączone łańcuchem `this()`

### Wymagania

- Walidacja daty w głównym konstruktorze
- Właściwości `Year`, `Month`, `Day` (readonly)
- Metoda `IsLeapYear()`
- Metoda `DaysInMonth()`

### Przykład

```csharp
var d1 = new Date();              // Dzisiejsza data
var d2 = new Date(2024);          // 2024, dzisiaj (miesiąc/dzień)
var d3 = new Date(2024, 12, 25);  // 25.12.2024
```

### ✅ Rozwiązanie

```csharp
public class Date
{
    public int Year { get; }
    public int Month { get; }
    public int Day { get; }
    
    public Date() : this(DateTime.Now.Year) { }
    
    public Date(int year) 
        : this(year, DateTime.Now.Month, DateTime.Now.Day) { }
    
    public Date(int year, int month, int day)
    {
        if (year < 1 || year > 9999)
            throw new ArgumentException("Year out of range");
        if (month < 1 || month > 12)
            throw new ArgumentException("Month out of range");
        if (day < 1 || day > DateTime.DaysInMonth(year, month))
            throw new ArgumentException("Day out of range");
        
        Year = year;
        Month = month;
        Day = day;
    }
    
    public bool IsLeapYear() => DateTime.IsLeapYear(Year);
    
    public int DaysInMonth() => DateTime.DaysInMonth(Year, Month);
    
    public override string ToString() => $"{Day:00}.{Month:00}.{Year}";
}
```

---

## 📝 Zadanie 2: Klasa Vehicle z wielokrotnym łańcuchowaniem

### Opis

Stwórz klasę `Vehicle` z co najmniej 4 konstruktorami:
- Tylko marka
- Marka + model
- Marka + model + rok produkcji
- Marka + model + rok + przebieg

### Wymagania

- Wszystkie konstruktory połączone `this()`
- Walidacja przeb iegu (nie ujemny)
- Metoda `GetAge()` - ile lat ma pojazd
- Domyślne wartości w `this()` dla pól opcjonalnych

### Przykład

```csharp
var v1 = new Vehicle("BMW");
var v2 = new Vehicle("BMW", "X5");
var v3 = new Vehicle("BMW", "X5", 2020);
var v4 = new Vehicle("BMW", "X5", 2020, 45000);
```

### ✅ Rozwiązanie

```csharp
public class Vehicle
{
    public string Brand { get; }
    public string Model { get; }
    public int Year { get; }
    public int Mileage { get; }
    
    public Vehicle(string brand)
        : this(brand, "Unknown") { }
    
    public Vehicle(string brand, string model)
        : this(brand, model, DateTime.Now.Year) { }
    
    public Vehicle(string brand, string model, int year)
        : this(brand, model, year, 0) { }
    
    public Vehicle(string brand, string model, int year, int mileage)
    {
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand cannot be empty");
        if (mileage < 0)
            throw new ArgumentException("Mileage cannot be negative");
        
        Brand = brand;
        Model = model;
        Year = year;
        Mileage = mileage;
    }
    
    public int GetAge() => DateTime.Now.Year - Year;
    
    public override string ToString() 
        => $"{Brand} {Model} ({Year}) - {Mileage} km";
}
```

---

## 📝 Zadanie 3: Klasa Book z konfiguracją domyślną

### Opis

Stwórz klasę `Book` z następującymi konstruktorami (wszystkie łańcuchowe):
- `Book(title)` - tylko tytuł, autor nieznany, rok bieżący
- `Book(title, author)` - tytuł i autor, rok bieżący
- `Book(title, author, year)` - wszystkie pola
- `Book(title, author, year, pages)` - + liczba stron

### Wymagania

- Walidacja wszystkich pól w głównym konstruktorze
- Właściwości readonly
- Metoda `GetInfo()` zwracająca sformatowany string
- Rok nie może być w przyszłości

### Przykład

```csharp
var b1 = new Book("1984");
var b2 = new Book("1984", "George Orwell");
var b3 = new Book("1984", "George Orwell", 1949);
var b4 = new Book("1984", "George Orwell", 1949, 328);
```

### ✅ Rozwiązanie

```csharp
public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }
    public int Pages { get; }
    
    public Book(string title)
        : this(title, "Unknown") { }
    
    public Book(string title, string author)
        : this(title, author, DateTime.Now.Year) { }
    
    public Book(string title, string author, int year)
        : this(title, author, year, 0) { }
    
    public Book(string title, string author, int year, int pages)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title required");
        if (year > DateTime.Now.Year)
            throw new ArgumentException("Year cannot be in future");
        if (pages < 0)
            throw new ArgumentException("Pages cannot be negative");
        
        Title = title;
        Author = author;
        Year = year;
        Pages = pages;
    }
    
    public string GetInfo() 
        => $"{Title} by {Author} ({Year}), {Pages} pages";
    
    public override string ToString() => GetInfo();
}
```

---

## 📚 Testy do implementacji

```csharp
[Fact]
public void Date_DefaultConstructor_ReturnsTodayDate()
{
    var date = new Date();
    var today = DateTime.Now;
    
    Assert.Equal(today.Year, date.Year);
    Assert.Equal(today.Month, date.Month);
}

[Fact]
public void Vehicle_Constructor_ChainsProperly()
{
    var v = new Vehicle("Tesla", "Model 3", 2023, 15000);
    
    Assert.Equal("Tesla", v.Brand);
    Assert.Equal("Model 3", v.Model);
    Assert.Equal(2023, v.Year);
    Assert.Equal(15000, v.Mileage);
}

[Fact]
public void Book_ValidatesFutureYear()
{
    var futureYear = DateTime.Now.Year + 1;
    
    Assert.Throws<ArgumentException>(() 
        => new Book("Title", "Author", futureYear));
}
```

---

## 💡 Wnioski

Po wykonaniu tych zadań powinieneś umieć:

- ✅ Projektować hierarchię konstruktorów
- ✅ Unikać duplikacji kodu w `this()`
- ✅ Umieszczać walidację w głównym konstruktorze
- ✅ Używać wartości domyślnych w łańcuchu
- ✅ Tworzyć elastyczne API z wieloma konstruktorami
