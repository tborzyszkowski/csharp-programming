# Temat 6: Default Interface Members - Nowoczesne C# (8+)

## 📌 Cel Tematu

Zrozumienie **Default Interface Members** - funkcjonalności dostępnej od C# 8.0, która pozwala na **implementację kodu w interfejsach**.

## 🚀 Co Nowego w C# 8.0+?

Tradycyjnie interfejsy służyły głównie do deklarowania kontraktu:

```csharp
public interface IPaymentMethod
{
    void Pay(decimal amount);      // Bez ciała: implementacja jest wymagana
}
```

**Teraz** (C# 8.0+), możesz mieć implementację!

```csharp
public interface IPaymentMethod
{
    void Pay(decimal amount);
    
    // Domyślna implementacja od C# 8
    public string GetDescription() => "Standardowa metoda płatności";
}
```

`Pay` nadal jest wymaganym kontraktem - każda klasa implementująca
`IPaymentMethod` musi ją dostarczyć. `GetDescription` ma ciało, więc klasa może
z niego skorzystać bez dodawania własnej implementacji.

### Jak wywoływana jest metoda domyślna?

Metoda domyślna należy do interfejsu. Dlatego wywołujemy ją przez zmienną typu
interfejsu, a nie automatycznie przez zmienną konkretnej klasy:

```csharp
public class BasicPayment : IPaymentMethod
{
    public void Pay(decimal amount) { }
}

IPaymentMethod payment = new BasicPayment();
Console.WriteLine(payment.GetDescription()); // "Standardowa metoda płatności"

// BasicPayment concretePayment = new BasicPayment();
// concretePayment.GetDescription(); // Błąd: ta metoda nie jest członkiem klasy BasicPayment
```

Jeśli klasa zadeklaruje publiczną metodę o tej samej sygnaturze, wywołanie
przez interfejs użyje implementacji klasy zamiast domyślnej:

```csharp
public class CardPayment : IPaymentMethod
{
    public void Pay(decimal amount) { }
    public string GetDescription() => "Płatność kartą";
}

IPaymentMethod payment = new CardPayment();
Console.WriteLine(payment.GetDescription()); // "Płatność kartą"
```

To nadal jest wywołanie polimorficzne przez interfejs: wybór implementacji
następuje na podstawie typu obiektu. Nie jest to metoda odziedziczona przez
klasę tak jak metoda z klasy bazowej. Różni się też od metody rozszerzającej:
metoda rozszerzająca jest zwykłą metodą statyczną wybieraną na podstawie typu
widocznego w miejscu wywołania, a nie domyślną implementacją kontraktu interfejsu.

---

## 🎯 Kiedy to Jest Przydatne?

### Problem: Dodawanie Nowej Metody do Interface'u

Wyobraź sobie starą aplikację z 50 klasami implementującymi interface:

```csharp
public interface ILogger
{
    void Log(string message);
}

// 50 klas:
public class ConsoleLogger : ILogger
{
    public void Log(string message) { /* dotychczasowe logowanie */ }
}
public class FileLogger : ILogger
{
    public void Log(string message) { /* dotychczasowe logowanie */ }
}
public class DatabaseLogger : ILogger
{
    public void Log(string message) { /* dotychczasowe logowanie */ }
}
// ... i 47 więcej
```

Teraz musisz dodać nową metodę `LogError()`.

### Rozwiązanie 1: Dodaj do Interface'u

```csharp
public interface ILogger
{
    void Log(string message);
    void LogError(string error);  // ❌ BREAK: Wszystkie 50 klas teraz się nie kompilują!
}
```

**Rezultat**: Musisz zaktualizować wszystkie 50 klas! 💔

### Rozwiązanie 2: Default Implementation (C# 8.0+)

```csharp
public interface ILogger
{
    void Log(string message);
    
    // ✅ NEW: Default implementation - domyślne zachowanie
    public void LogError(string error)
    {
        Log($"ERROR: {error}");  // Domyślnie loguje jako zwykły Log
    }
}

// Dotychczasowe klasy nie muszą dopisywać LogError().
ILogger logger = new ConsoleLogger();
logger.LogError("Nie udało się zapisać danych");
```

Domyślna metoda pozwala ewoluować interfejsowi bez wymuszania zmian w każdej
istniejącej klasie implementującej. Klasa musi jednak nadal implementować
wcześniejsze metody bez ciała, takie jak `Log`. Nowa metoda musi też mieć
zachowanie sensowne dla wszystkich implementacji, które z niego skorzystają.
Sama możliwość kompilacji nie gwarantuje, że to zachowanie jest właściwe.

---

## 💡 Praktyczne Zastosowania

### 1. Backward Compatibility

```csharp
public interface IDataRepository<T>
    where T : class
{
    T? GetById(int id);
    
    // Domyślna metoda składa się z operacji wymaganej przez kontrakt
    public bool Exists(int id) => GetById(id) is not null;
}

Stara implementacja, która już ma `GetById`, może od razu użyć `Exists`.
Może też udostępnić własną implementację `Exists`, np. zoptymalizowaną pod bazę
danych. Domyślne `GetAll() => new List<T>()` byłoby niebezpiecznym przykładem:
mogłoby zwracać pustą listę mimo tego, że repozytorium zawiera dane.

Podobna pułapka występuje w demonstracyjnym `IRepository<T>` w pliku
`code/Program.cs`: domyślne `Count()` zwraca `0`. `UserRepository` dostarcza
własną implementację, ale inny niepusty magazyn danych bez nadpisania tej
metody raportowałby błędny wynik. To przykład technicznego fallbacku, a nie
dobrego domyślnego zachowania dla produkcyjnego repozytorium.
```

### 2. Metody oparte na innych członkach interfejsu

```csharp
public interface ICalculator
{
    int Add(int a, int b);
    int Subtract(int a, int b);
    
    // ✅ Default: Combine other methods
    public int DuplicateResult(int a, int b)
    {
        return Add(a, b) * 2;  // Używa innych metod interface'u!
    }
}
```

### 3. Logging/Validation Wrapper

```csharp
public interface IService
{
    void DoWork(string input);
    
    // ✅ Default: Add logging
    public void DoWorkWithLogging(string input)
    {
        Console.WriteLine($"[LOG] Starting work with '{input}'");
        DoWork(input);
        Console.WriteLine("[LOG] Work completed");
    }
}

public class MyService : IService
{
    public void DoWork(string input)
    {
        Console.WriteLine($"Actually doing work: {input}");
    }
}

// Użycie
IService service = new MyService();
service.DoWorkWithLogging("test");  // Dodane logowanie bez zmiany klasy!
```

---

## 🔐 Modyfikatory dostępu w interfejsach (C# 8+)

Od C# 8 składowe interfejsu mogą mieć modyfikatory dostępu (`public`, `private`, `protected`, `internal` i ich kombinacje). Składowe bez modyfikatora są publiczne.

```csharp
public interface IAdvancedLogging
{
    // Public - dostępny przez referencję interfejsu
    public void PublicLog(string message)
    {
        Console.WriteLine($"[PUBLIC] {FormatMessage(message)}");
    }
    
    // Private - pomocnik dostępny tylko wewnątrz tego interfejsu
    private string FormatMessage(string message) => $"Formatted: {message}";

    // Protected - dostępny dla interfejsów pochodnych
    protected void ProtectedLog(string message)
    {
        Console.WriteLine($"[PROTECTED] {message}");
    }
    
    // Internal - dostęp w assembly
    internal void InternalMethod() { }
}

public interface IExtendedLogging : IAdvancedLogging
{
    // ✅ interfejs pochodny może wołać składową protected interfejsu bazowego
    public void LogViaDerivedInterface(string message) => ProtectedLog(message);
}

public class AdvancedLogger : IExtendedLogging
{
    public void Use()
    {
        // ProtectedLog("...");   // ❌ klasa NIE widzi składowej protected interfejsu
        // FormatMessage("...");  // ❌ private - niedostępne poza interfejsem
        ((IAdvancedLogging)this).PublicLog("ok");   // ✅ przez referencję interfejsu
    }
}
```

> Składowa `protected` interfejsu jest przeznaczona dla **interfejsów pochodnych**, a nie dla klas, które go implementują. Klasa może taką składową zaimplementować (przesłonić), ale nie może jej bezpośrednio wywołać.

---

## 📊 Diagram: Default Interface Members Evolution

```mermaid
graph LR
    subgraph "C# 7.0 i wcześniej"
        A["interface ILogger<br/>+ Log"]
        B["Nowa metoda?<br/>BREAK"]
        C["50 klas<br/>do naprawy"]
        A --> B --> C
    end
    
    subgraph "C# 8.0+"
        D["interface ILogger<br/>+ Log<br/>+ LogError default"]
        E["Nowa metoda<br/>+ default impl"]
        F["50 klas<br/>Bez zmian!"]
        D --> E --> F
    end
    
    style B fill:#ffcccc
    style E fill:#ccffcc
```

---

## ⚡ Składowe statyczne w interfejsach (C# 8+) i `static abstract` (C# 11+)

```csharp
public interface IConverter
{
    // C# 8+: składowe statyczne - dostęp bez instancji
    static int ConversionFactor = 1000;   // uwaga: publiczne pole statyczne to współdzielony stan globalny
    
    static string Format(int value)
    {
        return $"{value / ConversionFactor}";
    }
    
    // Instance member
    int Convert(int input);
}

public class MyConverter : IConverter
{
    public int Convert(int input)
    {
        return input * IConverter.ConversionFactor;
    }
}

// Użycie
Console.WriteLine(IConverter.Format(5000));  // Bezpośrednio z interfejsu: "5"
```

Składowe statyczne **nie są dziedziczone** przez implementujące klasy - odwołujesz się do nich przez nazwę interfejsu (`IConverter.Format`).

### `static abstract` (C# 11+)

Od C# 11 interfejs może wymagać, by implementujący typ miał określoną składową **statyczną** (m.in. operatory). Dzięki temu metody generyczne mogą wołać statyczne składowe `T`:

```csharp
public interface IParsableValue<TSelf> where TSelf : IParsableValue<TSelf>
{
    static abstract TSelf Parse(string text);
}

public readonly record struct Meters(double Value) : IParsableValue<Meters>
{
    public static Meters Parse(string text) =>
        new(double.Parse(text, CultureInfo.InvariantCulture));
}

public static T Read<T>(string text) where T : IParsableValue<T> => T.Parse(text);

var distance = Read<Meters>("42.5");   // T.Parse rozwiązane statycznie, bez refleksji
```

To fundament mechanizmu *generic math* w .NET 7+ (np. `INumber<T>`, `IParsable<T>`).

Warto rozróżnić te dwa rodzaje składowych:

- `IConverter.Format(...)` jest konkretną metodą statyczną interfejsu. Wywołujesz
  ją przez nazwę interfejsu; nie zależy od obiektu i nie jest wybierana
  polimorficznie.
- `static abstract TSelf Parse(...)` nie ma implementacji w interfejsie.
  Wymaga implementacji statycznej w typie, a ograniczenie generyczne pozwala
  metodzie takiej jak `Read<T>` wywołać `T.Parse(...)` bez tworzenia instancji.

To różni się od domyślnej metody instancyjnej: ta ma ciało i działa na konkretnej
referencji interfejsu, natomiast `static abstract` opisuje wymaganie wobec typu.

---

## ⚔️ Dwie domyślne implementacje tej samej metody

```csharp
public interface IGreeterA { string Hello() => "A"; }
public interface IGreeterB { string Hello() => "B"; }

// ❌ Bez własnej implementacji kompilator zgłosi błąd - brak "najbardziej szczegółowej" implementacji
// public class Broken : IGreeterA, IGreeterB { }

// ✅ Klasa wskazuje własną implementację
public class BothGreeter : IGreeterA, IGreeterB
{
    public string Hello() => "Both";
}

// ✅ Interfejs pochodny może zastąpić domyślną implementację interfejsu bazowego
public interface ILoudGreeter : IGreeterA
{
    string IGreeterA.Hello() => "LOUD";   // jawna implementacja interfejsu
}

public class LoudGreeter : ILoudGreeter { }

IGreeterA g = new LoudGreeter();
Console.WriteLine(g.Hello());   // "LOUD"
```

Zasada: wywoływana jest **najbardziej szczegółowa** implementacja. Jeśli nie da się jej jednoznacznie wskazać (jak w `Broken`), musisz rozstrzygnąć konflikt sam.

W praktyce:

1. Gdy tylko jeden z interfejsów dostarcza domyślną implementację, można z niej
   skorzystać.
2. Gdy klasa dostarcza własną metodę `Hello`, wywołania przez oba interfejsy
   korzystają z implementacji klasy.
3. Gdy dwa niezależne interfejsy dostarczają różne implementacje, kompilator nie
   zgaduje, którą wybrać - klasa musi rozstrzygnąć konflikt.
4. Interfejs pochodny może wskazać bardziej szczegółową implementację dla
   członka interfejsu bazowego, jak `ILoudGreeter` dla `IGreeterA.Hello`.

---

## 🎓 Zasady - która wersja języka wprowadziła co

| Funkcja | C# 7.3 i wcześniej | C# 8 | C# 11 |
|---------|:---:|:---:|:---:|
| Deklaracje metod/właściwości w interfejsie | ✅ | ✅ | ✅ |
| Domyślne implementacje (metody, właściwości, indeksatory, zdarzenia) | ❌ | ✅ | ✅ |
| Modyfikatory dostępu (`private`, `protected`, `internal`…) w interfejsie | ❌ | ✅ | ✅ |
| Składowe statyczne z implementacją (metody, pola) | ❌ | ✅ | ✅ |
| `static abstract` / `static virtual` | ❌ | ❌ | ✅ |
| Pola instancji (stan) w interfejsie | ❌ | ❌ | ❌ |

Interfejsy **nadal nie mogą przechowywać stanu instancji** (pola, auto-właściwości z polem pomocniczym). Domyślne implementacje mogą wyłącznie korzystać z pozostałych składowych interfejsu.

---

## 📁 Kod Demonstracyjny

Znajduje się w katalogu `code/`:
- `Program.cs` - Default interface members w praktyce

Uruchomienie:
```bash
cd code
dotnet run
```

---

## 🔗 Referencje

- [Default Interface Members (C# 8.0)](https://learn.microsoft.com/en-us/dotnet/csharp/tutorials/default-interface-members-versions)
- [Interfaces - Microsoft Learn](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces)
- [Static Abstract Members in Interfaces (C# 11)](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-11#static-abstract-members-in-interfaces)

## ⚠️ Ograniczenia

Domyślna implementacja jest członkiem interfejsu, a nie zwykłą metodą klasy.
Wywołanie `logger.LogInfo(...)` wymaga więc referencji typu `ILogger`, chyba że
klasa sama zdefiniuje publiczną metodę o tej nazwie. Default interface members
nie zastępują klasy abstrakcyjnej ani mechanizmu DI; powinny zawierać małe,
stabilne zachowania domyślne i nie ukrywać istotnych zależności.

Dodatkowo:
- **Wymagają wsparcia runtime**: sama wersja języka kompilatora nie wystarczy.
  Potrzebny jest runtime obsługujący tę funkcję (np. .NET Core 3.0+ lub .NET 5+);
  nie jest ona obsługiwana przez .NET Framework.
- **Kompatybilność nie oznacza poprawności dowolnego zachowania**: stary
  implementator może skorzystać z nowej metody bez zmian, ale jej domyślna
  logika musi być poprawna dla jego przypadku.
- **Dodanie domyślnej implementacji może zmienić rozstrzyganie metod**:
  w hierarchii interfejsów może pojawić się bardziej szczegółowa implementacja
  albo konflikt, którego wcześniej nie było.
- **Nie ma stanu instancji** - domyślna implementacja nie może mieć własnych pól.

---

## 🚀 Wnioski

- **Default implementations** = bezpieczniejsza ewolucja interfejsów
- **Modyfikatory dostępu** = prywatne metody pomocnicze i składowe `protected` dla interfejsów pochodnych
- **Składowe statyczne** = funkcje pomocnicze związane z interfejsem; `static abstract` = kontrakt na składowe statyczne typu
- **Backward compatibility** = zwykle nie łamiesz istniejącego kodu, ale zachowaj ostrożność

---

*Nowoczesne C# daje nam nowe możliwości projektowania interfejsów!*

## 🧭 Nawigacja

| Przejdź do | Link |
|---|---|
| Poprzedni temat | [Temat 5: Adapter + Factory](../_05_adapter_factory/README.md) |
| Moduł | [06-polimorfizm](../README.md) |
| Następny moduł | [07-interfejsy_abstrakcje](../../07-interfejsy_abstrakcje/README.md) |
