# Ćwiczenia - Predefiniowane Delegacje Generyczne

## 🟢 BASIC

### Ćw 3.1: Action - Print Numbers
Użyj `Action<int>` aby wydrukować liczby 1-10 z `List.ForEach`

### Ćw 3.2: Func - Transform Strings
```csharp
Func<string, int> wordCount = ...
Func<string, string> toUpper = ...
```

### Ćw 3.3: Predicate - Filter
```csharp
Predicate<int> isEven = n => n % 2 == 0;
List<int> numbers = new { 1, 2, 3, 4, 5, 6 };
List<int> evens = numbers.FindAll(isEven);
```

## 🟡 INTERMEDIATE

### Ćw 3.4: Pipeline z Func
Stwórz pipeline przetwarzania tekstu z 3+ transformacjami

### Ćw 3.5: FilteredCollection
Zbuduj klasę `FilteredCollection<T>` z Action/Func/Predicate

### Ćw 3.6: LINQ Query
Użyj Where (Predicate), Select (Func), ForEach (Action) w jednym Query

## 🔴 ADVANCED

### Ćw 3.7: Composition
Stwórz `Compose` funkcję która łączy dwie `Func`:
```csharp
var toUpper = (string s) => s.ToUpper();
var addBrackets = (string s) => $"[{s}]";
var composed = Compose(addBrackets, toUpper);
Console.WriteLine(composed("hello"));  // [HELLO]
```
