# Temat 1: Delegacje - Idea i Motywacja

## 🎯 Cel Tematu

W tym temacie poznasz **WHY** delegacje istnieją - zrozumiesz ideę stojącą za nimi, zanim nauczysz się ich składni.

### Słowa Kluczowe
- Late binding (wiązanie opóźnione)
- Type safety
- Callback mechanism
- Callback-based programming
- Function pointers

---

## 📖 Wprowadzenie: Czym są Delegacje?

### Problem: Jak Przekazać Logikę do Funkcji?

Wyobraź sobie, że masz funkcję, która powinna **wykonać coś na liście liczb**. Ale co dokładnie? Może:
- Drukować każdą liczbę
- Sumować je
- Filtrować parzyste
- Coś innego?

### Rozwiązanie Bez Delegacji (Tradycyjne Podejście)

```csharp
// Musisz napisać osobną funkcję dla każdego scenariusza
public void PrintNumbers(int[] numbers)
{
    foreach (var n in numbers)
        Console.WriteLine(n);
}

public void SumNumbers(int[] numbers)
{
    int sum = 0;
    foreach (var n in numbers)
        sum += n;
    Console.WriteLine($"Suma: {sum}");
}

public void PrintEvenNumbers(int[] numbers)
{
    foreach (var n in numbers)
    {
        if (n % 2 == 0)
            Console.WriteLine(n);
    }
}
```

**Problem:** Duża powtarzalność kodu! ❌

### Rozwiązanie z Delegacjami (Elastyczne Podejście)

```csharp
// Deklarujemy delegacje - typ reprezentujący "funkcję"
public delegate void NumberAction(int number);

// Jedna funkcja, którą przejawiamy logikę jako delegację
public void ProcessNumbers(int[] numbers, NumberAction action)
{
    foreach (var n in numbers)
        action(n);  // Wykonujemy przekazoną logikę
}

// Teraz możemy użyć tej samej funkcji z różnymi zachowaniami
int[] data = { 1, 2, 3, 4, 5 };

ProcessNumbers(data, n => Console.WriteLine(n));           // Print
ProcessNumbers(data, n => {                                // Custom logic
    if (n % 2 == 0) Console.WriteLine($"Even: {n}");
});
```

**Przewaga:** Jeden kod, wiele zachowań! ✅

---

## 🔗 Delegacje jako "Type-Safe Function Pointers"

### Czym są Function Pointers?

W języku C można używać wskaźników do funkcji:

```c
// C - wskaźnik do funkcji
int (*operation)(int, int) = &add;  // Wskaż na funkcję add
int result = operation(5, 3);        // Wywołaj przez wskaźnik
```

**Problem w C:** Brak bezpieczeństwa typów! 😟

### Delegacje - "Function Pointers" z Type Safety

Delegacje w C# to to samo co wskaźniki do funkcji, ale **z pełnym typem bezpieczeństwa**:

```csharp
// C# - delegacja (type-safe function pointer)
public delegate int BinaryOperation(int a, int b);

BinaryOperation operation = (x, y) => x + y;  // Type-checked
int result = operation(5, 3);                 // Sprawdzony typ

// Compiler błąd - parametry się nie zgadzają!
// BinaryOperation wrong = n => Console.WriteLine(n);  // ❌ ERROR!
```

---

## 📚 Koncepty: Late Binding

### Co to Jest Late Binding?

**Early Binding** (tradycyjnie):
```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
}

// Wiadome na etapie kompilacji - co będzie wykonane
Calculator calc = new Calculator();
int result = calc.Add(5, 3);  // ZNAMY metodę na etapie kompilacji
```

**Late Binding** (z delegacjami):
```csharp
public delegate int Operation(int a, int b);

Operation myOperation = (x, y) => x + y;  // Funkcja wybrana w RUNTIME!

// Operacja mogła być wybrana na podstawie danych z użytkownika
if (userWantsAdd)
    myOperation = (x, y) => x + y;
else
    myOperation = (x, y) => x * y;

int result = myOperation(5, 3);  // Który będzie wykonany? Zależy od runtime!
```

### Kiedy Potrzebujemy Late Binding?

1. **Strategy Pattern** - wybranie algorytmu w runtime
2. **Observer Pattern** - powiadomienia dla wielu obserwatorów
3. **Event Handling** - reagowanie na działania użytkownika
4. **Dependency Injection** - wstrzykiwanie logiki biznesowej
5. **Asynchroniczne Operacje** - callback po ukończeniu operacji

---

## 🏛️ Architektura i Wzorce

### Wzorzec: Strategy Pattern z Delegacjami

```csharp
public class DataProcessor
{
    public delegate void ProcessingStrategy(int[] data);
    
    // Różne strategie przetwarzania
    public static void PrintAll(int[] data)
        => data.ForEach(n => Console.WriteLine(n));
    
    public static void PrintEven(int[] data)
        => data.Where(n => n % 2 == 0)
               .ForEach(n => Console.WriteLine(n));
    
    public static void PrintSum(int[] data)
        => Console.WriteLine($"Sum: {data.Sum()}");
    
    // Procesator akceptuje strategię
    public void Process(int[] data, ProcessingStrategy strategy)
    {
        Console.WriteLine("Processing with selected strategy...");
        strategy(data);
    }
}

// Użycie
var processor = new DataProcessor();
int[] numbers = { 1, 2, 3, 4, 5 };

processor.Process(numbers, DataProcessor.PrintAll);    // Druk wszystkich
processor.Process(numbers, DataProcessor.PrintEven);   // Druk parzystych
processor.Process(numbers, DataProcessor.PrintSum);    // Suma
```

### Diagram: Delegacje jako Proxy do Metod

```
┌─────────────────────────────────────────────────┐
│  Delegacja = "Type-Safe Reference to Method"    │
│  Przechowuje: Obiekt + Metoda                   │
└─────────────────────────────────────────────────┘
        ↓
    ┌───────────┐
    │ delegate  │
    │  void     │
    │ Operation │  ← Deklaracja typuuję
    │ (int, int)│
    └───────────┘
        ↓
┌──────────────────────┬──────────────────────┐
│  (x, y) => x + y     │  (x, y) => x * y     │ ← Różne implementacje
│  (Add)               │  (Multiply)          │    mogą być przydzielane
└──────────────────────┴──────────────────────┘
        ↓
    ┌────────────────┐
    │ Wywoła metodę  │
    │ w runtime      │  ← Late Binding
    └────────────────┘
```

---

## 💡 Porównanie: Delegacje vs Inne Podejścia

### Podejście 1: Odziedziczenie (Stara Droga)

```csharp
public abstract class NumberProcessor
{
    public abstract void Process(int number);
    
    public void ProcessAll(int[] numbers)
    {
        foreach (var n in numbers)
            Process(n);
    }
}

public class PrintProcessor : NumberProcessor
{
    public override void Process(int number)
        => Console.WriteLine(number);
}

public class SumProcessor : NumberProcessor
{
    private int sum = 0;
    
    public override void Process(int number)
        => sum += number;
}

// Użycie: Musisz tworzyć nowe klasy!
new PrintProcessor().ProcessAll(numbers);
```

**Problemy:** 😞
- Wiele klas
- Złożona hierarchia
- Hard to maintain

### Podejście 2: Delegacje (Nowoczesna Droga)

```csharp
public delegate void NumberProcessor(int number);

public void ProcessNumbers(int[] numbers, NumberProcessor processor)
{
    foreach (var n in numbers)
        processor(n);
}

// Użycie: Prosta, elegancka!
ProcessNumbers(numbers, n => Console.WriteLine(n));
ProcessNumbers(numbers, n => sum += n);
```

**Zalety:** ✨
- Mniej kodu
- Bardziej elastyczne
- Łatwiejsze do testowania

---

## 🔐 Type Safety w Delegacjach

### Co To Znaczy "Type Safe"?

```csharp
// Deklarujemy, jakie parametry i zwracaną wartość przechowuje delegacja
public delegate bool IntPredicate(int value);

// ✅ Poprawne - typy się zgadzają
IntPredicate isEven = n => n % 2 == 0;

// ❌ BŁĄD KOMPILACJI - zwracana wartość się nie zgadza
// IntPredicate wrong1 = n => { Console.WriteLine(n); };  // Zwraca void, oczekujesz bool

// ❌ BŁĄD KOMPILACJI - typy parametrów się nie zgadzają
// IntPredicate wrong2 = (string s) => true;  // Oczekujesz int, a tu string

// ❌ BŁĄD KOMPILACJI - liczba parametrów się nie zgadza
// IntPredicate wrong3 = (a, b) => a > b;  // Oczekujesz 1 parametru, a tu 2
```

**Bezpieczeństwo:** Compiler sprawdzi dla nas! 🛡️

---

## 🎬 Real-World Przykład: Asynchroniczny Callback

### Bez Delegacji (Trudne)

```csharp
// Jak powiedzieć metodzie co zrobić po ukończeniu operacji?
// Trzeba by odziedziczać od klasy specjalnej... Zamieszanie!
```

### Z Delegacjami (Proste)

```csharp
public class FileDownloader
{
    // Delegacja do callbacku
    public delegate void DownloadCompleted(string result);
    
    public void Download(string url, DownloadCompleted onComplete)
    {
        // Symulacja pobierania
        Thread.Sleep(1000);
        string data = $"Content from {url}";
        
        // Wywoła callback kiedy gotowe
        onComplete(data);
    }
}

// Użycie
var downloader = new FileDownloader();
downloader.Download("http://example.com", result => 
{
    Console.WriteLine($"Download complete: {result}");
});
```

---

## 🚀 Podsumowanie: Dlaczego Delegacje?

| Aspekt | Bez Delegacji | Z Delegacjami |
|--------|---------------|---------------|
| **Elastyczność** | Niski | Wysoki ✅ |
| **Ilość Kodu** | Duża | Mała ✅ |
| **Type Safety** | Zł | Doskonały ✅ |
| **Łatwość Testów** | Trudna | Łatwa ✅ |
| **Curvy** | Proste | Potwornie? |

---

## 📚 Referencje

### Dokumentacja
- [Delegates - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/delegates)
- [Late Binding - MSDN](https://learn.microsoft.com/en-us/dotnet/fundamentals/reflection/reflection)

### Artykuły
- [Understanding Delegates in C#](https://www.tutorialsteacher.com/csharp/csharp-delegates)
- [Delegates and Events in .NET](https://www.codeproject.com/Articles/11155/Delegates-and-Events-in-NET)

---

## ➡️ Następny Krok

Teraz, gdy rozumiesz **WHY** - przechodzimy do **HOW** w Temacie 2: **Delegacje - Definicja i Składnia**

W następnym temacie nauczysz się:
- Jak deklarować delegacje
- Jak je instancjować
- Jak je wywoływać
- Multicast delegacje
