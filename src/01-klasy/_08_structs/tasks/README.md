# Zadania - Struktury (Value Types)

## 📝 Zadanie 1: Color struct

Zaimplementuj **niezmienną** strukturę `Color` (`readonly struct`) z konstruktorem `Color(byte red, byte green, byte blue)`,
metodą `FromHex(string)`, `ToHex()` i równością wartościową:

```csharp
public readonly struct Color
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
}

var red = new Color(255, 0, 0);
var red2 = red;                 // kopia
var darker = red.WithRed(200);  // zmiana = nowa wartość, oryginalna bez zmian
```

Odpowiedz: dlaczego przy niezmiennej strukturze programista nie może zaobserwować różnicy między „kopią”
a „wspólną referencją”? Jaki problem rozwiązuje ta właściwość?

---

## 📝 Zadanie 2: Money struct

Stwórz strukturę `Money`:
- Właściwości: `decimal Amount`, `string Currency` (3-literowy kod)
- Metody: `Add()`, `Subtract()`, `IsValid()`
- Operatory: `+`, `-`, `==`, `!=` (z `Equals` i `GetHashCode`)
- Zastanów się, co oznacza `default(Money)` i jak `IsValid()` pomaga w tej sytuacji

---

## 📝 Zadanie 3: Date wrapper

Utwórz `Date` struct (value type) z:
- Konstruktorem (year, month, day) z pełną walidacją (np. 30 lutego jest niepoprawne)
- Metodą GetDayOfWeek()
- Operatorami porównania (`==`, `!=`, `<`, `>`, `<=`, `>=`)
- Porównaj swoje rozwiązanie z wbudowanym `DateOnly` – kiedy nie ma sensu pisać własnej struktury?

---

## ✅ Zadanie 1 - Rozwiązanie: Color Struct

### Kod

```csharp
// STRUKTURA NIEZMIENNA - Value Type
public readonly struct Color : IEquatable<Color>
{
    public byte Red { get; }
    public byte Green { get; }
    public byte Blue { get; }
    
    public Color(byte red, byte green, byte blue)
    {
        Red = red;
        Green = green;
        Blue = blue;
    }
    
    // "Zmiana" = zwrócenie nowej wartości
    public Color WithRed(byte red) => new(red, Green, Blue);
    
    // Fabryka: tworzy kolor z zapisu "#RRGGBB"
    public static Color FromHex(string hex)
    {
        if (hex.StartsWith('#'))
            hex = hex.Substring(1);
        if (hex.Length != 6)
            throw new ArgumentException("Oczekiwano 6 cyfr szesnastkowych (RRGGBB)", nameof(hex));
        
        byte r = Convert.ToByte(hex.Substring(0, 2), 16);
        byte g = Convert.ToByte(hex.Substring(2, 2), 16);
        byte b = Convert.ToByte(hex.Substring(4, 2), 16);
        
        return new Color(r, g, b);
    }
    
    public string ToHex() => $"#{Red:X2}{Green:X2}{Blue:X2}";
    
    // Przybliżona jasność (0..1) – średnia składowych
    public double GetBrightness() => (Red + Green + Blue) / 3.0 / 255.0;
    
    public override string ToString() => $"RGB({Red}, {Green}, {Blue}) - {ToHex()}";
    
    public bool Equals(Color other) => Red == other.Red && Green == other.Green && Blue == other.Blue;
    public override bool Equals(object? obj) => obj is Color other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Red, Green, Blue);
    
    public static bool operator ==(Color left, Color right) => left.Equals(right);
    public static bool operator !=(Color left, Color right) => !left.Equals(right);
}

// Test
var red = new Color(255, 0, 0);
var red2 = red;              // KOPIA wartości (bo to struct)
var darker = red.WithRed(200);   // nowa wartość - oryginalna bez zmian

Console.WriteLine($"red:    {red}");
Console.WriteLine($"red2:   {red2}");
Console.WriteLine($"darker: {darker}");
Console.WriteLine($"red == red2: {red == red2}");          // true - równość wartościowa

var blue = Color.FromHex("#0000FF");
Console.WriteLine($"blue: {blue}");
Console.WriteLine($"brightness: {blue.GetBrightness():F2}");
```

### Wyjaśnienie

- **struct = Value Type**: zmienne przechowują wartość bezpośrednio, a przypisanie `red2 = red` tworzy **kopię**
- `readonly struct` gwarantuje, że nic nie zmieni wartości po utworzeniu – wtedy kopiowanie jest całkowicie
  bezpieczne, a przypadkowe zmiany „na kopii” (klasyczny błąd ze zmiennymi strukturami) są niemożliwe
- Kolor jest dobrym kandydatem na strukturę: ma 3 bajty, reprezentuje pojedynczą wartość, jest niezmienny
- Gdy definiujesz `==`, **zawsze** przesłaniaj też `Equals(object)` i `GetHashCode()` (kompilator ostrzega o braku)
- W praktyce podobną strukturę napiszesz jednym wierszem: `public readonly record struct Color(byte Red, byte Green, byte Blue);`

---

## ✅ Zadanie 2 - Rozwiązanie: Money Struct

### Kod

```csharp
public readonly struct Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }
    
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
    public override bool Equals(object? obj) => obj is Money other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
    
    public static bool operator ==(Money a, Money b) => a.Equals(b);
    public static bool operator !=(Money a, Money b) => !a.Equals(b);
    
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

- **Enkapsulacja danych**: `Amount` i `Currency` razem to **logiczna jednostka**
- **Operatory +/-**: naturalny zapis (`salary + bonus`)
- **IEquatable<Money>** + `Equals(object)` + `GetHashCode()` + `==`/`!=`: spójna równość wartościowa
- Jeśli `Currency` się nie zgadza, wyrzucamy **wyjątek** (fail-fast)
- **Pułapka:** `default(Money)` (oraz `new Money[5]`) omija konstruktor – ma `Currency == null`. Struktura
  nie może więc zagwarantować swoich niezmienników, dlatego przyda się metoda `IsValid()`.
  Wskazówka: dla typów z ważnymi niezmiennikami często lepsza jest klasa albo `record`.

---

## ✅ Zadanie 3 - Rozwiązanie: Date Struct

### Kod

```csharp
public readonly struct Date : IEquatable<Date>, IComparable<Date>
{
    public int Year { get; }
    public int Month { get; }
    public int Day { get; }
    
    public Date(int year, int month, int day)
    {
        // Walidacja pełna: dzień musi istnieć w danym miesiącu (uwzględnia lata przestępne)
        if (year < 1 || year > 9999 || month < 1 || month > 12
            || day < 1 || day > DateTime.DaysInMonth(year, month))
            throw new ArgumentException("Nieprawidłowa data");
        
        Year = year;
        Month = month;
        Day = day;
    }
    
    // Konwersja
    public static Date Today()
    {
        var today = DateTime.Today;   // jedno wywołanie - bez ryzyka różnych dni przy północy
        return new(today.Year, today.Month, today.Day);
    }
    
    public DateTime ToDateTime() => new(Year, Month, Day);
    
    public DayOfWeek GetDayOfWeek() => ToDateTime().DayOfWeek;
    
    public string GetDayOfWeekName() => GetDayOfWeek().ToString();
    
    public bool IsLeapYear() => DateTime.IsLeapYear(Year);
    
    // Porównania
    public bool Equals(Date other) => Year == other.Year && Month == other.Month && Day == other.Day;
    public override bool Equals(object? obj) => obj is Date other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Year, Month, Day);
    
    public int CompareTo(Date other)
    {
        if (Year != other.Year) return Year.CompareTo(other.Year);
        if (Month != other.Month) return Month.CompareTo(other.Month);
        return Day.CompareTo(other.Day);
    }
    
    // Operatory (<, > wymagają pary; dodajemy też <=, >=)
    public static bool operator ==(Date a, Date b) => a.Equals(b);
    public static bool operator !=(Date a, Date b) => !a.Equals(b);
    public static bool operator <(Date a, Date b) => a.CompareTo(b) < 0;
    public static bool operator >(Date a, Date b) => a.CompareTo(b) > 0;
    public static bool operator <=(Date a, Date b) => a.CompareTo(b) <= 0;
    public static bool operator >=(Date a, Date b) => a.CompareTo(b) >= 0;
    
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
Console.WriteLine($"Rok 1990 to rok przestępny: {birthday.IsLeapYear()}");   // False
```

### Wyjaśnienie

- **Value Type**: wartości, nie referencje; struktura niezmienna (`readonly struct`)
- **IComparable**: możliwość sortowania i porównywania dat
- **Operatory**: `==`, `!=`, `<`, `>`, `<=`, `>=` – oraz `Equals`/`GetHashCode` zgodne z `==`
- **Konwersje**: `ToDateTime()` - interop z System.DateTime
- Walidacja dnia przez `DateTime.DaysInMonth` odrzuca np. 30 lutego (prosty warunek `day <= 31` by je przepuścił)
- **W praktyce** nie piszemy własnych typów daty: użyj wbudowanego **`DateOnly`** (.NET 6+), który robi to wszystko i więcej.
  To zadanie służy do ćwiczenia składni struktur i operatorów.

---

## 🧪 Testy

```csharp
[Fact]
public void Struct_Color_CopyIsIndependentOfOriginal()
{
    var color1 = new Color(255, 0, 0);
    var color2 = color1;              // kopia
    var changed = color2.WithRed(0);  // "zmiana" daje nową wartość

    Assert.Equal(color1, color2);     // kopia jest równa oryginałowi (równość wartościowa)
    Assert.Equal(255, color1.Red);    // oryginalna wartość nie została zmieniona
    Assert.NotEqual(color1, changed);
}

[Fact]
public void Struct_Color_FromHex_RoundTrips()
{
    Assert.Equal("#0000FF", Color.FromHex("#0000FF").ToHex());
    Assert.Throws<ArgumentException>(() => Color.FromHex("#FFF"));
}

[Fact]
public void Struct_Money_OperatorWorks()
{
    var money1 = new Money(100, "USD");
    var money2 = new Money(50, "USD");
    var result = money1 + money2;
    
    Assert.Equal(150, result.Amount);
    Assert.True(result == new Money(150, "USD"));
}

[Fact]
public void Struct_Money_DifferentCurrencies_Throw()
{
    Assert.Throws<InvalidOperationException>(() => new Money(1, "USD") + new Money(1, "EUR"));
}

[Fact]
public void Struct_Money_Default_IsNotValid()
{
    Assert.False(default(Money).IsValid());   // Currency == null, konstruktor nie został wywołany
}

[Fact]
public void Struct_Date_Comparison()
{
    var date1 = new Date(2024, 1, 1);
    var date2 = new Date(2024, 1, 2);
    
    Assert.True(date1 < date2);
    Assert.True(date1 <= new Date(2024, 1, 1));
    Assert.False(date1 == date2);
    Assert.True(date1 != date2);
}

[Fact]
public void Struct_Date_InvalidDay_Throws()
{
    Assert.Throws<ArgumentException>(() => new Date(2023, 2, 29));   // 2023 nie jest przestępny
    Assert.Throws<ArgumentException>(() => new Date(2024, 4, 31));   // kwiecień ma 30 dni
    Assert.Equal("2024-02-29", new Date(2024, 2, 29).ToString());    // 2024 jest przestępny
}

[Fact]
public void Struct_Date_UsableAsDictionaryKey()
{
    var dict = new Dictionary<Date, string> { [new Date(2024, 1, 1)] = "Nowy Rok" };

    Assert.Equal("Nowy Rok", dict[new Date(2024, 1, 1)]);   // działa dzięki GetHashCode/Equals
}
```

---

## 📚 Zasoby Edukacyjne

**Pojęcia kluczowe**:
- **Struktury = Value Types**: przypisanie kopiuje wartość
- **Klasy = Reference Types**: przypisanie kopiuje referencję
- Struktury pasują do małych, niezmiennych obiektów reprezentujących pojedynczą wartość (Point, Color, Money, Date)
- Struktury **nie są automatycznie szybsze** – duże struktury są kosztowne w kopiowaniu, a boxing alokuje pamięć
- Preferuj `readonly struct` / `readonly record struct`; ostrożnie ze zmiennymi strukturami (zmiana kopii nie zmienia oryginału)

**Microsoft Docs**:
- https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
