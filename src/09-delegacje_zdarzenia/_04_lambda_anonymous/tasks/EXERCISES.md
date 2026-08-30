# Ćwiczenia - Lambda i Anonymous Methods

## 🟢 BASIC

### Ćw 4.1: Lambda Hello World
```csharp
Func<string, string> greet = name => $"Hello, {name}!";
Console.WriteLine(greet("World"));
```

### Ćw 4.2: LINQ Where
```csharp
var numbers = new[] { 1, 2, 3, 4, 5 };
var evens = numbers.Where(n => n % 2 == 0);
```

### Ćw 4.3: Select Transform
```csharp
var names = new[] { "alice", "bob", "charlie" };
var upper = names.Select(n => n.ToUpper());
```

## 🟡 INTERMEDIATE

### Ćw 4.4: Filter-Map-Reduce
Napisz jedno-linijkowy LINQ query:
- Filtruj liczby > 10
- Pomnóż przez 2
- Sumuj rezultat

### Ćw 4.5: Closure
```csharp
int bonus = 100;
Func<int, int> addBonus = salary => salary + bonus;
```

### Ćw 4.6: GroupBy Students
Pogrupuj studentów po ocenie z lambdą

## 🔴 ADVANCED

### Ćw 4.7: Complex LINQ Query
Użyj Select, Where, OrderBy, GroupBy, Take w jednym query
