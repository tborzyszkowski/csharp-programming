# Temat 4: Wyrażenia Lambda i Metody Anonimowe

## 🎯 Cel Tematu

Nauczysz się pisać kod inline z wyrażeniami lambda i metodami anonimowymi - najnowoczesnymi sposobami pracy z delegacjami.

---

## 📖 Metody Anonimowe (Starsza Składnia)

### Co To Jest Anonymous Method?

```csharp
public delegate int Calculate(int a, int b);

// Metoda anonimowa
Calculate calc = delegate(int x, int y)
{
    int result = x + y;
    return result;
};

Console.WriteLine(calc(5, 3));  // 8
```

### Kiedy Używać?

- Mały kod inline
- Nie powtarzany w innym miejscu
- Alternatywa do lambdy

---

## ⚡ Wyrażenia Lambda (Nowoczesna Składnia)

### Podstawowa Składnia

```csharp
// Jedno-linijkowa
(parameters) => expression;

// Wielolinijkowa
(parameters) => {
    statements;
    return value;
};

// Bez parametrów
() => value;

// Jeden parametr (nawiasy opcjonalne)
x => x * 2;
(x) => x * 2;  // Identyczne
```

### Przykłady

```csharp
// Proste transformacje
Func<int, int> square = x => x * x;
Func<string, int> length = s => s.Length;
Func<double, double> negate = x => -x;

// Warunki
Func<int, bool> isEven = n => n % 2 == 0;
Func<string, bool> isEmpty = s => string.IsNullOrWhiteSpace(s);

// Wielolinijkowe
Func<int, int> complexCalc = x => {
    int doubled = x * 2;
    int result = doubled + 10;
    return result;
};

// Bez wartości zwracanej (Action)
Action<string> greet = name => Console.WriteLine($"Hello, {name}!");
```

### Closure - Przechwytywanie Zmiennych

```csharp
int multiplier = 10;

// Lambda "pamięta" multiplier
Func<int, int> multiply = x => x * multiplier;

Console.WriteLine(multiply(5));  // 50

// Zmiana multiplier
multiplier = 20;
Console.WriteLine(multiply(5));  // 100 (używa nowej wartości!)
```

---

## 🔗 Lambda w LINQ

### Gdzie (Where)

```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var evens = numbers.Where(n => n % 2 == 0);
```

### Select (Transformacja)

```csharp
var names = new[] { "alice", "bob", "charlie" };
var upper = names.Select(n => n.ToUpper());
```

### OrderBy

```csharp
var people = new[] { 
    new { Name = "Bob", Age = 25 },
    new { Name = "Alice", Age = 30 }
};
var sorted = people.OrderBy(p => p.Age);
```

### GroupBy

```csharp
var students = new[] {
    new { Name = "Alice", Grade = 'A' },
    new { Name = "Bob", Grade = 'B' },
    new { Name = "Charlie", Grade = 'A' }
};

var grouped = students.GroupBy(s => s.Grade);
foreach (var group in grouped)
{
    Console.WriteLine($"Grade {group.Key}: {group.Count()} students");
}
```

---

## 🎬 Real-World Patterns

### Pattern: Filter-Map-Reduce

```csharp
var numbers = Enumerable.Range(1, 100);

int result = numbers
    .Where(n => n % 2 == 0)                    // Filter: parzyste
    .Select(n => n * 2)                        // Map: podwój
    .Aggregate(0, (sum, n) => sum + n);        // Reduce: sumuj

Console.WriteLine($"Sum of doubled even numbers: {result}");
```

### Pattern: Event Handler

```csharp
button.Click += (sender, e) =>
{
    MessageBox.Show("Button clicked!");
};

// Równoważnie:
void OnButtonClick(object? sender, EventArgs e)
{
    MessageBox.Show("Button clicked!");
}
button.Click += OnButtonClick;
```

### Pattern: Predicate Chain

```csharp
var isValidUser = (User u) => 
    !string.IsNullOrEmpty(u.Name) &&
    u.Age >= 18 &&
    !u.IsBlocked;
```

---

## 📊 Porównanie: Lambda vs Anonymous vs Named

```csharp
delegate void Notify(string msg);

// 1. Metoda nazwana
public static void LogNotification(string msg)
{
    Console.WriteLine($"[LOG] {msg}");
}
Notify n1 = LogNotification;

// 2. Metoda anonimowa
Notify n2 = delegate(string msg) {
    Console.WriteLine($"[LOG] {msg}");
};

// 3. Lambda (najnowoczesniej)
Notify n3 = msg => Console.WriteLine($"[LOG] {msg}");

// Wszystkie identyczne!
n1("test");
n2("test");
n3("test");
```

---

## 🔗 Referencje

- [Lambda Expressions - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions)
- [Anonymous Methods - Microsoft Docs](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/delegates-and-lambdas)

---

## ➡️ Następny Krok

Temat 5: **Zdarzenia - Fundamenty** - praktyczne użycie delegacji
