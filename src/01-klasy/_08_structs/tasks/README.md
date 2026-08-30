# Zadania - Struktury (Value Types)

## 📝 Zadanie 1: Color struct

```csharp
public struct Color
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
}

var red = new Color(255, 0, 0);
var red2 = red;
```

---

## 📝 Zadanie 2: Money struct

Stwórz strukturę `Money`:
- Pola: `decimal Amount`, `string Currency`
- Metody: `Add()`, `Subtract()`, `IsValid()`
- Operator: `+`, `-`

---

## 📝 Zadanie 3: DateTime wrapper

Utwórz `Date` struct (value type) z:
- Konstruktorem (year, month, day)
- Metodą GetDayOfWeek()
- Operatorem porównania (==, !=)

---

## ✅ Zadanie 1 - Rozwiązanie: Color Struct

### Kod

```csharp
// STRUKTURA - Value Type
public struct Color
{
    public byte Red { get; private set; }
    public byte Green { get; private set; }
    public byte Blue { get; private set; }
    
    public Color(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
    
    // Operator na RGB
    public static Color FromHex(string hex)
    {
        if (hex.StartsWith("#"))
            hex = hex.Substring(1);
        
        byte r = Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(4, 2), 16);
        
        return new Color(r, g, b);
    }
    
    public string ToHex() => $"#{Red:X2}{Green:X2}{Blue:X2}";
    
    public double GetBrightness() => (Red + Green + Blue) / 3.0 / 255.0;
    
    public override string ToString() => $"RGB({Red}, {Green}, {Blue}) - {ToHex()}";
    
    public override bool Equals(object? obj)
    {
        if (obj is not Color other) return false;
        return Red == other.Red && Green == other.Green && Blue == other.Blue;
    }
    
    public override int GetHashCode() => HashCode.Combine(Red, Green, Blue);
}

// Test
var red = new Color(255, 0, 0);
var red2 = red;  // KOPIA wartości (bo to struct)

red2 = new Color(200, 0, 0);  // Zmiana red2 nie wpływa na red

Console.WriteLine($"red: {red}");
Console.WriteLine($"red2: {red2}");

var blue = Color.FromHex("#0000FF");
Console.WriteLine($"blue: {blue}");
Console.WriteLine($"brightness: {blue.GetBrightness():F2}");
```

### Wyjaśnienie

- **struct = Value Type**: Zmienne przechowują wartość bezpośrednio
- Przypisanie `red2 = red` tworzy **KOPIĘ** wartości
- Zmiana `red2` **nie wpływa** na `red`
- Przydatne dla: punkty, kolory, daty, małe obiekty
- **Szybsze** niż klasy (brak alokacji na heap)

---

## ✅ Zadanie 2 - Rozwiązanie: Money Struct

### Kod

```csharp
public struct Money : IEquatable<Money>
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
    
    public Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount nie może być ujemny");
        if (string.IsNullOrEmpty(currency) || currency.Length != 3)
            throw new ArgumentException("Currency musi być kodem 3-literowym (np. USD)");
        
        Amount = amount;
        Currency = currency;
    }
    
    // Metody
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Nie można dodawać różnych walut");
        return new Money(Amount + other.Amount, Currency);
    }
    
    public Money Subtract(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Nie można odejmować różnych walut");
        
        decimal result = Amount - other.Amount;
        if (result < 0)
            throw new InvalidOperationException("Wynik byłby ujemny");
        
        return new Money(result, Currency);
    }
    
    public bool IsValid() => Amount >= 0 && !string.IsNullOrEmpty(Currency);
    
    // Operatory
    public static Money operator +(Money a, Money b) => a.Add(b);
    public static Money operator -(Money a, Money b) => a.Subtract(b);
    
    public bool Equals(Money other) => Amount == other.Amount && Currency == other.Currency;
    
    public override string ToString() => $"{Amount:F2} {Currency}";
}

// Test
var salary = new Money(5000, "USD");
var bonus = new Money(1000, "USD");

Console.WriteLine($"Pensja: {salary}");
Console.WriteLine($"Bonus: {bonus}");
Console.WriteLine($"Razem: {salary + bonus}");

var spent = new Money(500, "USD");
var remaining = salary - spent;
Console.WriteLine($"Po wydatkach: {remaining}");
```

### Wyjaśnienie

- **Encapsulacja danych**: `Amount` i `Currency` razem to **logiczna jednostka**
- **Operatory +/-**: Naturalny zapis (`salary + bonus`)
- **IEquatable<Money>**: Porównanie między wartościami
- Jeśli `Currency` się nie zgadza, wyrzucamy **wyjątek** (fail-fast)

---

## ✅ Zadanie 3 - Rozwiązanie: Date Struct

### Kod

```csharp
public struct Date : IEquatable<Date>, IComparable<Date>
{
    public int Year { get; private set; }
    public int Month { get; private set; }
    public int Day { get; private set; }
    
    public Date(int year, int month, int day)
    {
        if (year < 1 || month < 1 || month > 12 || day < 1 || day > 31)
            throw new ArgumentException("Nieprawidłowa data");
        
        Year = year;
        Month = month;
        Day = day;
    }
    
    // Konwersja
    public static Date Today() => new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
    
    public DateTime ToDateTime() => new(Year, Month, Day);
    
    public DayOfWeek GetDayOfWeek() => ToDateTime().DayOfWeek;
    
    public int GetDaysSinceEpoch() => (int)(ToDateTime() - new DateTime(1970, 1, 1)).TotalDays;
    
    public string GetDayOfWeekName() => GetDayOfWeek().ToString();
    
    public bool IsLeapYear() => (Year % 4 == 0 && Year % 100 != 0) || (Year % 400 == 0);
    
    // Porównania
    public bool Equals(Date other) => Year == other.Year && Month == other.Month && Day == other.Day;
    
    public int CompareTo(Date other)
    {
        if (Year != other.Year) return Year.CompareTo(other.Year);
        if (Month != other.Month) return Month.CompareTo(other.Month);
        return Day.CompareTo(other.Day);
    }
    
    // Operatory
    public static bool operator ==(Date a, Date b) => a.Equals(b);
    public static bool operator !=(Date a, Date b) => !a.Equals(b);
    public static bool operator <(Date a, Date b) => a.CompareTo(b) < 0;
    public static bool operator >(Date a, Date b) => a.CompareTo(b) > 0;
    
    public override string ToString() => $"{Year:D4}-{Month:D2}-{Day:D2}";
}

// Test
var birthday = new Date(1990, 5, 15);
Console.WriteLine($"Data urodzin: {birthday}");
Console.WriteLine($"Dzień tygodnia: {birthday.GetDayOfWeekName()}");

var today = Date.Today();
Console.WriteLine($"Dzisiaj: {today}");

Console.WriteLine($"birthday == birthday: {birthday == birthday}");
Console.WriteLine($"birthday < today: {birthday < today}");
Console.WriteLine($"Rok 1990 to rok przestępny: {birthday.IsLeapYear()}");
```

### Wyjaśnienie

- **Value Type**: Wartości nie referencje
- **IComparable**: Możliwość porównywania dat
- **Operatory**: `==`, `<`, `>` dla naturalnych porównań
- **Konwersje**: `ToDateTime()` - interop z System.DateTime
- **Logika biznesowa**: Year leap checks, day of week calculations

---

## 🧪 Testy

```csharp
[Fact]
public void Struct_Color_IsValueType()
{
    var color1 = new Color(255, 0, 0);
    var color2 = color1;
    color2 = new Color(0, 255, 0);
    
    Assert.NotEqual(color1, color2);
}

[Fact]
public void Struct_Money_OperatorWorks()
{
    var money1 = new Money(100, "USD");
    var money2 = new Money(50, "USD");
    var result = money1 + money2;
    
    Assert.Equal(150, result.Amount);
}

[Fact]
public void Struct_Date_Comparison()
{
    var date1 = new Date(2024, 1, 1);
    var date2 = new Date(2024, 1, 2);
    
    Assert.True(date1 < date2);
    Assert.False(date1 == date2);
}
```

---

## 📚 Zasoby Edukacyjne

**Pojęcia kluczowe**:
- **Struktury = Value Types**: Przechowują dane bezpośrednio
- **Klasy = Reference Types**: Przechowują referencje
- Struktury pasują do: small data objects (Point, Color, Money, Date)
- Performance: Struktury szybsze (stack vs heap)
- Ostrożnie z mutacją: Nie zmienia oryginału

**YouTube - Structs vs Classes in C#**:
- https://www.youtube.com/results?search_query=C%23+structs+vs+classes+tutorial

**Microsoft Docs**:
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
