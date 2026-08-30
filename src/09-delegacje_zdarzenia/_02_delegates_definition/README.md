# Temat 2: Delegacje - Definicja i Składnia

## 🎯 Cel Tematu

Nauczysz się **HOW** - jak deklarować, instancjować i wywoływać delegacje. Przejdziemy od teorii do praktyki.

### Słowa Kluczowe
- Deklaracja delegacji (`delegate` keyword)
- Instancjowanie delegacji
- Odwoływanie (`Invoke`)
- Multicast (`+=`, `-=`)
- Variance (in, out)

---

## 📖 Deklaracja Delegacji

### Składnia Podstawowa

```csharp
// Deklaracja delegacji
public delegate <return-type> <delegate-name>(<parameters>);

// Przykłady
public delegate void NotifyDelegate(string message);           // Bez zwracanej wartości
public delegate int MathDelegate(int a, int b);                // Ze zwracaną wartością
public delegate bool FilterDelegate(string text);              // Zwraca bool
public delegate void AsyncDelegate(string url, int timeout);   // Wiele parametrów
```

### Delegacja w Namespace

```csharp
namespace MyApp
{
    // Delegacja na poziomie namespace - widoczna wszędzie
    public delegate void EventHandler(string message);
    
    public class MyClass
    {
        // Delegacja wewnątrz klasy - tylko tutaj widoczna
        public delegate void InnerDelegate(int value);
    }
}
```

### Accessibility Modifiers

```csharp
public delegate void PublicDelegate(int x);        // Widoczna wszędzie
private delegate void PrivateDelegate(int x);      // Tylko w tej klasie
internal delegate void InternalDelegate(int x);    // Tylko w tym assembly
protected delegate void ProtectedDelegate(int x);  // W klasie i w klasach pochodnych
```

---

## 🔧 Instancjowanie Delegacji

### Metoda 1: Przypisanie Nazwanej Metody

```csharp
public class Calculator
{
    public delegate int MathOp(int a, int b);
    
    // Metoda z odpowiednim podpisem
    public static int Add(int a, int b) => a + b;
    public static int Multiply(int a, int b) => a * b;
    
    public static void Main()
    {
        // Przypiś istniejącą metodę
        MathOp op1 = Add;
        MathOp op2 = Multiply;
        
        Console.WriteLine($"5 + 3 = {op1(5, 3)}");      // Output: 8
        Console.WriteLine($"5 * 3 = {op2(5, 3)}");      // Output: 15
    }
}
```

### Metoda 2: Anonimowa Metoda

```csharp
public delegate int MathOp(int a, int b);

MathOp operation = delegate(int x, int y)
{
    Console.WriteLine($"Executing with {x} and {y}");
    return x + y;
};

Console.WriteLine(operation(5, 3));  // Output: Executing with 5 and 3 / 8
```

### Metoda 3: Wyrażenie Lambda

```csharp
public delegate int MathOp(int a, int b);

// Jedno-linijkowa lambda
MathOp add = (x, y) => x + y;

// Wielolinijkowa lambda
MathOp subtract = (x, y) =>
{
    int result = x - y;
    Console.WriteLine($"Subtraction result: {result}");
    return result;
};

Console.WriteLine(add(10, 5));          // 15
Console.WriteLine(subtract(10, 5));     // 5
```

### Porównanie Metod

```csharp
public delegate int Op(int a, int b);

// 1. Metoda nazwana
public static int Named(int x, int y) => x + y;
Op m1 = Named;

// 2. Anonimowa metoda
Op m2 = delegate(int x, int y) { return x + y; };

// 3. Lambda (najnowoczesniej)
Op m3 = (x, y) => x + y;

// Wszystkie działają identycznie!
Console.WriteLine(m1(5, 3));  // 8
Console.WriteLine(m2(5, 3));  // 8
Console.WriteLine(m3(5, 3));  // 8
```

---

## 📞 Odwoływanie Delegacji

### Metoda 1: Bezpośrednie Wywołanie

```csharp
public delegate void Greeting(string name);

Greeting greet = name => Console.WriteLine($"Hello, {name}!");

// Bezpośrednie wywołanie
greet("Alice");
```

### Metoda 2: Metoda `Invoke()`

```csharp
public delegate void Greeting(string name);

Greeting greet = name => Console.WriteLine($"Hello, {name}!");

// Jawne użycie Invoke
greet.Invoke("Bob");  // Tożsame z greet("Bob")
```

### Metoda 3: Sprawdzenie Null

```csharp
public delegate void Notify(string message);

Notify? notifier = null;  // Nullable delegate

// ❌ Błąd - NullReferenceException
// notifier("Error!");

// ✅ Bezpieczne
notifier?.Invoke("Message");  // Nic się nie stanie jeśli null

// Alternatywa
if (notifier != null)
{
    notifier("Message");
}
```

---

## 🔗 Multicast Delegates

### Łączenie Delegacji - Operator `+=`

```csharp
public delegate void Logger(string message);

void ConsoleLog(string msg) => Console.WriteLine($"[CONSOLE] {msg}");
void FileLog(string msg) => Console.WriteLine($"[FILE] {msg}");

Logger loggers = null!;

// Dodaj pierwszy logger
loggers += ConsoleLog;

// Dodaj kolejny logger
loggers += FileLog;

// Teraz zawiera oba loggers
loggers("System started");
// Output:
// [CONSOLE] System started
// [FILE] System started
```

### Usuwanie Delegacji - Operator `-=`

```csharp
public delegate void Logger(string message);

Logger log = null!;
log += m => Console.WriteLine($"Log1: {m}");
log += m => Console.WriteLine($"Log2: {m}");
log += m => Console.WriteLine($"Log3: {m}");

Console.WriteLine("=== All Loggers ===");
log("Event happened");

// Usuń drugiego loggera
log -= m => Console.WriteLine($"Log2: {m}");

Console.WriteLine("\n=== After Removing Log2 ===");
log("Another event");
// Note: Może nie zadziałać idealnie - lambda bez referencji trudna do usunięcia!
```

### Prawidłowy Multicast

```csharp
public delegate void Notify(string msg);

// Zmienne dla referencji
void Handler1(string msg) => Console.WriteLine($"Handler 1: {msg}");
void Handler2(string msg) => Console.WriteLine($"Handler 2: {msg}");
void Handler3(string msg) => Console.WriteLine($"Handler 3: {msg}");

Notify notifier = null!;
notifier += Handler1;
notifier += Handler2;
notifier += Handler3;

notifier("Event 1");
// Handler 1: Event 1
// Handler 2: Event 1
// Handler 3: Event 1

// Usuń drugiego handlera
notifier -= Handler2;

notifier("Event 2");
// Handler 1: Event 2
// Handler 3: Event 2 (Handler 2 missing)
```

### Kolejność Wykonania

```csharp
public delegate int Operation(int a, int b);

Operation ops = null!;

ops += (x, y) => { Console.WriteLine("First"); return x + y; };
ops += (x, y) => { Console.WriteLine("Second"); return x * y; };
ops += (x, y) => { Console.WriteLine("Third"); return x - y; };

int result = ops(5, 3);

// Output:
// First
// Second
// Third
// Result (ostatnia zwrócona wartość): 2 (5 - 3)
```

---

## 🔀 Variance w Delegacjach

### Covariance (Out)

```csharp
public delegate Animal GetAnimal();  // Zwraca Animal

public class Dog : Animal { }

// Covariance: Możliwe przypisanie Dog do Animal
GetAnimal getAnimal = () => new Dog();

// Dlaczego bezpieczne? Dog IS-A Animal
```

### Contravariance (In)

```csharp
public delegate void ProcessAnimal(Animal animal);

public class Dog : Animal { }

// Contravariance: Możliwe przypisanie ProcessDog do ProcessAnimal
ProcessAnimal process = (Animal a) => Console.WriteLine("Processing animal");

// Dlaczego bezpieczne? Możesz przetwarzać bardziej specjalistycznie
```

---

## 🎬 Real-World Patterns

### Pattern: Event Handler

```csharp
// Standard .NET event handler
public delegate void EventHandler(object? sender, EventArgs e);

public class Button
{
    private EventHandler? onClick;
    
    public event EventHandler? OnClick
    {
        add => onClick += value;
        remove => onClick -= value;
    }
    
    public void Click()
    {
        onClick?.Invoke(this, EventArgs.Empty);
    }
}
```

### Pattern: Func vs Action

```csharp
// Action - procedura, nie zwraca wartości
public delegate void Action();                    // Brak parametrów
public delegate void Action<T>(T obj);            // 1 parametr
public delegate void Action<T1, T2>(T1 x, T2 y); // 2 parametry

// Func - funkcja, zwraca wartość
public delegate TResult Func<out TResult>();                      // Brak parametrów
public delegate TResult Func<in T, out TResult>(T obj);            // 1 parametr
public delegate TResult Func<in T1, in T2, out TResult>(T1 x, T2 y); // 2 parametry
```

---

## 📊 Podsumowanie: Delegacje Step-by-Step

```
1. DEKLARACJA
   public delegate <return-type> <name>(<params>);

2. INSTANCJOWANIE
   - Metoda nazwana: delegate_var = MethodName;
   - Anonimowa: delegate_var = delegate(params) { };
   - Lambda: delegate_var = (params) => expression;

3. ODWOŁYWANIE
   - Bezpośrednie: delegate_var(args);
   - Invoke: delegate_var.Invoke(args);
   - Bezpieczne: delegate_var?.Invoke(args);

4. MULTICAST
   - Dodaj: delegate_var += handler;
   - Usuń: delegate_var -= handler;
   - Wszystkie odpalane sekwencyjnie
```

---

## 🔗 Referencje

- [Delegates - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/delegates)
- [Action, Func, Predicate](https://learn.microsoft.com/en-us/dotnet/api/system.action)
- [Covariance and Contravariance](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/functional/delegates-type-safety)

---

## ➡️ Następny Krok

Temat 3: **Predefiniowane Delegacje Generyczne** - poznasz `Action<T>` i `Func<T>`
