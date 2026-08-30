# Temat 3: Predefiniowane Delegacje Generyczne

## 🎯 Cel Tematu

Zamiast definiować własne delegacje, nauczysz się używać predefiniowanych delegacji generycznych .NET: `Action<T>`, `Func<T, TResult>` i `Predicate<T>`.

### Słowa Kluczowe
- `Action<T>` - procedura (brak zwracanej wartości)
- `Func<T, TResult>` - funkcja (ze zwracaną wartością)
- `Predicate<T>` - predykat logiczny (zwraca bool)
- Variance (in/out)

---

## 📖 Action<T> - Procedury

### Co To Jest Action?

`Action<T>` to delegacja, która:
- Przyjmuje parametr typu `T`
- **Nie zwraca wartości** (void)
- Wykonuje operację/efekt uboczny

### Podstawowe Użycie

```csharp
// Deklaracja (już wbudowana, nie musisz definiować)
Action<string> greet = name => Console.WriteLine($"Hello, {name}!");

greet("Alice");  // Output: Hello, Alice!

// Wiele parametrów
Action<int, int> add = (a, b) => Console.WriteLine($"{a} + {b} = {a + b}");
add(5, 3);  // Output: 5 + 3 = 8

// Brak parametrów
Action log = () => Console.WriteLine("Event logged");
log();  // Output: Event logged
```

### Praktyczne Przykłady

```csharp
// Przykład 1: Iteracja z Action
var numbers = new List<int> { 1, 2, 3, 4, 5 };
numbers.ForEach(n => Console.WriteLine(n));

// Przykład 2: Callback
public void ProcessWithCallback(string data, Action<string> onComplete)
{
    Console.WriteLine($"Processing: {data}");
    Thread.Sleep(500);
    onComplete($"Completed: {data}");
}

ProcessWithCallback("file.txt", result => Console.WriteLine($"✓ {result}"));

// Przykład 3: Multicast Action
Action<string> loggers = null!;
loggers += msg => Console.WriteLine($"[CONSOLE] {msg}");
loggers += msg => Console.WriteLine($"[FILE] {msg}");
loggers("Important message");
```

---

## 🔧 Func<T, TResult> - Funkcje

### Co To Jest Func?

`Func<T, TResult>` to delegacja, która:
- Przyjmuje parametr typu `T`
- **Zwraca wartość** typu `TResult`
- Wykonuje operację i produkuje wynik

### Podstawowe Użycie

```csharp
// Jeden parametr
Func<int, int> square = x => x * x;
Console.WriteLine(square(5));  // Output: 25

// Wiele parametrów (ostatni to return type!)
Func<int, int, int> add = (a, b) => a + b;
Console.WriteLine(add(10, 5));  // Output: 15

// Parametr i return value
Func<string, int> wordCount = text => text.Split(' ').Length;
Console.WriteLine(wordCount("Hello world C#"));  // Output: 3

// Zwracanie struktury
Func<int, (int square, int cube)> powers = x => (x * x, x * x * x);
var result = powers(3);
Console.WriteLine($"Square: {result.square}, Cube: {result.cube}");  // Output: 9, 27
```

### LINQ i Func

```csharp
// Func w LINQ
Func<int, bool> isEven = n => n % 2 == 0;
var numbers = new[] { 1, 2, 3, 4, 5, 6 };
var evenNumbers = numbers.Where(isEven);

// Func jako transformer
Func<int, string> format = n => $"Number: {n}";
var formatted = numbers.Select(format);

// Łańcuchowanie Func
Func<string, int> toInt = s => int.Parse(s);
Func<int, int> double_val = x => x * 2;
Func<int, string> format_result = n => $"Result: {n}";

string input = "5";
int parsed = toInt(input);
int doubled = double_val(parsed);
string final = format_result(doubled);
Console.WriteLine(final);  // Output: Result: 10
```

---

## ✔️ Predicate<T> - Predykaty Logiczne

### Co To Jest Predicate?

`Predicate<T>` to delegacja, która:
- Przyjmuje parametr typu `T`
- **Zwraca bool** - true/false
- Testuje warunek

### Podstawowe Użycie

```csharp
// Predicate dla int
Predicate<int> isPositive = n => n > 0;
Console.WriteLine(isPositive(5));   // true
Console.WriteLine(isPositive(-5));  // false

// Predicate dla string
Predicate<string> isEmpty = s => string.IsNullOrWhiteSpace(s);
Console.WriteLine(isEmpty(""));      // true
Console.WriteLine(isEmpty("hello"));  // false

// Complex predicate
Predicate<int> isValid = n => n > 0 && n < 100;
Console.WriteLine(isValid(50));   // true
Console.WriteLine(isValid(150));  // false
```

### Predicate w LINQ

```csharp
// List.FindAll
var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
Predicate<int> isEven = n => n % 2 == 0;
var evens = numbers.FindAll(isEven);  // { 2, 4, 6, 8, 10 }

// Alternatywa z LINQ
var evensLinq = numbers.Where(n => n % 2 == 0);

// FindIndex
int index = numbers.FindIndex(n => n > 5);  // 5 (index of 6)
```

---

## 📊 Porównanie: Action vs Func vs Predicate

| Typ | Zwraca | Używaj Dla | Przykład |
|-----|--------|-----------|---------|
| **Action<T>** | void | Efekty uboczne, operacje | `n => Console.WriteLine(n)` |
| **Func<T, TResult>** | TResult | Transformacje, obliczenia | `x => x * 2` |
| **Predicate<T>** | bool | Warunki testowe | `n => n > 0` |

### Praktyczny Przykład

```csharp
List<int> data = new { 1, 2, 3, 4, 5 };

// Action - wypisz
Action<int> printer = n => Console.WriteLine(n);
data.ForEach(printer);

// Func - podwój
Func<int, int> doubler = n => n * 2;
var doubled = data.Select(doubler);

// Predicate - filtruj parzyste
Predicate<int> isEven = n => n % 2 == 0;
var evens = data.FindAll(isEven);
```

---

## 🎬 Real-World Scenariusze

### Scenario 1: Data Processing Pipeline

```csharp
public class DataProcessor
{
    private List<Func<string, string>> transformations = new();
    
    public void AddTransformation(Func<string, string> transformer)
    {
        transformations.Add(transformer);
    }
    
    public string Process(string input)
    {
        return transformations.Aggregate(input, (current, transform) => transform(current));
    }
}

var processor = new DataProcessor();
processor.AddTransformation(s => s.ToUpper());
processor.AddTransformation(s => s.Trim());
processor.AddTransformation(s => $"[{s}]");

string result = processor.Process("  hello world  ");
Console.WriteLine(result);  // Output: [HELLO WORLD]
```

### Scenario 2: Filtered Collection Processing

```csharp
public class FilteredCollection<T>
{
    private List<T> items = new();
    
    public void Add(T item) => items.Add(item);
    
    public List<T> GetFiltered(Predicate<T> filter) => items.FindAll(filter);
    
    public void ProcessAll(Action<T> action)
    {
        items.ForEach(action);
    }
    
    public List<R> TransformAll<R>(Func<T, R> transformer)
    {
        return items.Select(transformer).ToList();
    }
}

var products = new FilteredCollection<(string name, int price)>();
products.Add(("Laptop", 1000));
products.Add(("Mouse", 20));
products.Add(("Monitor", 300));

var expensive = products.GetFiltered(p => p.price > 100);
products.ProcessAll(p => Console.WriteLine($"{p.name}: ${p.price}"));
var names = products.TransformAll(p => p.name);
```

---

## 🔗 Referencje

- [Action (System)](https://learn.microsoft.com/en-us/dotnet/api/system.action)
- [Func (System)](https://learn.microsoft.com/en-us/dotnet/api/system.func-2)
- [Predicate (System)](https://learn.microsoft.com/en-us/dotnet/api/system.predicate-1)

---

## ➡️ Następny Krok

Temat 4: **Wyrażenia Lambda i Metody Anonimowe** - pogłębienie pracy z lambdami
