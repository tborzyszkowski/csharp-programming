# Zadania - Metody i Klasy Generyczne

## 📚 Poziom: Podstawowy

### Zadanie 1: Metoda Generyczna `FindMax`

**Opis:** Napisz generyczną metodę `FindMax<T>` która zwraca maksymalny element z tablicy. Metoda powinna:
- Zaakceptować tablicę dowolnego typu implementującego `IComparable<T>`
- Rzucić wyjątek `ArgumentException` jeśli tablica jest pusta
- Zwrócić maksymalny element

**Szablon:**
```csharp
public static T FindMax<T>(T[] array) where T : IComparable<T>
{
    // TODO: Implementacja
}

// Testy
int[] numbers = { 3, 1, 4, 1, 5, 9, 2, 6 };
Assert.Equal(9, FindMax(numbers));

string[] words = { "apple", "zebra", "banana" };
Assert.Equal("zebra", FindMax(words));
```

### Zadanie 2: Generyczna Klasa `Pair<T>`

**Opis:** Zaimplementuj generyczną klasę `Pair<T>` przechowującą dwie wartości tego samego typu. Klasa powinna posiadać:
- Konstruktor
- Właściwości First i Second
- Metodę `Swap()` zamieniającą wartości
- Przesłonięcie `ToString()`
- Przesłonięcie `Equals()` i `GetHashCode()`

**Testy:**
```csharp
var pair = new Pair<int>(10, 20);
Assert.Equal(10, pair.First);
Assert.Equal(20, pair.Second);

pair.Swap();
Assert.Equal(20, pair.First);
Assert.Equal(10, pair.Second);

var pair2 = new Pair<int>(10, 20);
Assert.Equal(pair, pair2);
```

### Zadanie 3: Generyczna Klasa `Queue<T>`

**Opis:** Zaimplementuj generyczną kolejkę (FIFO - First In First Out). Klasa powinna mieć:
- `Enqueue(T item)` - dodaj element na koniec
- `Dequeue()` - usuń i zwróć element z przodu
- `Peek()` - zwróć element z przodu bez usuwania
- `Count` - liczba elementów
- `IsEmpty` - czy pusta

**Testy:**
```csharp
Queue<int> queue = new();
queue.Enqueue(1);
queue.Enqueue(2);
queue.Enqueue(3);

Assert.Equal(1, queue.Dequeue());
Assert.Equal(2, queue.Dequeue());
Assert.Equal(3, queue.Count);
```

---

## 📚 Poziom: Średniozaawansowany

### Zadanie 4: Cache Generyczny

**Opis:** Napisz generyczną klasę `Cache<TKey, TValue>` implementującą prosty cache z limitem wielkości:
- `Set(TKey key, TValue value)` - dodaj/zaktualizuj wartość
- `Get(TKey key)` - pobierz wartość (zwróć `default` jeśli nie ma)
- `Remove(TKey key)` - usuń wartość
- `Clear()` - wyczyść cache
- Limit wielkości - gdy przekroczony, usuń najstarszy element (FIFO)
- Właściwość `Count`

**Testy:**
```csharp
Cache<string, int> cache = new(2);  // limit 2 elementy
cache.Set("a", 1);
cache.Set("b", 2);
cache.Set("c", 3);  // Powinno usunąć "a"

Assert.False(cache.Get("a").HasValue);  // Zwraca Optional<int>
Assert.Equal(2, cache.Get("b").Value);
Assert.Equal(3, cache.Get("c").Value);
```

### Zadanie 5: Generyczny Comparator

**Opis:** Napisz generyczną klasę `PropertyComparer<T, TProp>` umożliwiającą porównywanie obiektów po określonej właściwości:
- Konstruktor przyjmuje `Func<T, TProp>` selector
- Implementuje `IComparer<T>`
- Umożliwia sortowanie listy obiektów po wybranej właściwości

**Testy:**
```csharp
var people = new List<Person>
{
    new("Alice", 30),
    new("Bob", 25),
    new("Charlie", 35)
};

var nameComparer = new PropertyComparer<Person, string>(p => p.Name);
people.Sort(nameComparer);
Assert.Equal("Alice", people[0].Name);

var ageComparer = new PropertyComparer<Person, int>(p => p.Age);
people.Sort(ageComparer);
Assert.Equal(25, people[0].Age);
```

### Zadanie 6: Observable<T>

**Opis:** Napisz generyczną klasę `Observable<T>` obserwatora wartości (pattern Observer):
- Właściwość `Value` z getter i setter
- Metoda `Subscribe(Action<T> observer)` - rejestracja obserwatora
- Metoda `Unsubscribe(Action<T> observer)` - wyrejestrowanie obserwatora
- Setter powinien powiadamiać wszystkich obserwatorów o zmianie wartości

**Testy:**
```csharp
var observable = new Observable<int>(0);
var values = new List<int>();

observable.Subscribe(v => values.Add(v));
observable.Value = 10;
observable.Value = 20;

Assert.Equal(new[] { 10, 20 }, values);
```

---

## 📚 Poziom: Zaawansowany

### Zadanie 7: Tree<T> - Drzewo Generyczne

**Opis:** Zaimplementuj generyczną strukturę drzewa binarnego:
- Klasa `TreeNode<T>` z właściwościami: `Value`, `Left`, `Right`
- Klasa `BinaryTree<T>`
- Metody: `Insert(T value)`, `Contains(T value)`, `Traverse()` (in-order)
- Dla liczb: automatyczne sortowanie (lewa < rodzic < prawa)
- Dla innych typów: wymagaj `IComparable<T>`

**Testy:**
```csharp
var tree = new BinaryTree<int>();
tree.Insert(5);
tree.Insert(3);
tree.Insert(7);
tree.Insert(1);
tree.Insert(9);

var result = tree.Traverse();  // In-order
Assert.Equal(new[] { 1, 3, 5, 7, 9 }, result);
Assert.True(tree.Contains(7));
```

### Zadanie 8: MultiMap<TKey, TValue>

**Opis:** Zaimplementuj generyczną mapę umożliwiającą przechowywanie wielu wartości dla jednego klucza:
- `Add(TKey key, TValue value)` - dodaj wartość dla klucza
- `Get(TKey key)` - zwróć `IEnumerable<TValue>` dla klucza
- `Remove(TKey key, TValue value)` - usuń konkretną wartość
- `RemoveAll(TKey key)` - usuń wszystkie wartości dla klucza
- `ContainsKey(TKey key)` - czy klucz istnieje
- `Keys` - wszystkie klucze

**Testy:**
```csharp
var multiMap = new MultiMap<string, int>();
multiMap.Add("key", 1);
multiMap.Add("key", 2);
multiMap.Add("key", 3);

var values = multiMap.Get("key");
Assert.Equal(3, values.Count());
Assert.Contains(2, values);
```

### Zadanie 9: Result<T, TError>

**Opis:** Zaimplementuj generyczną klasę `Result<T, TError>` reprezentującą wynik operacji (sukces/błąd):
- `Success(T value)` - static factory dla sukcesu
- `Failure(TError error)` - static factory dla błędu
- `IsSuccess` - czy operacja się powiodła
- `Map<TResult>(Func<T, TResult>)` - transformacja wartości w succesie
- `Bind<TResult>(Func<T, Result<TResult, TError>>)` - łańcuchowanie operacji
- `Match<TResult>(Func<T, TResult>, Func<TError, TResult>)` - pattern matching

**Testy:**
```csharp
Result<int, string> success = Result<int, string>.Success(42);
Assert.True(success.IsSuccess);

var doubled = success.Map(x => x * 2);
Assert.Equal(84, doubled.Unwrap());

Result<int, string> failure = Result<int, string>.Failure("Error");
Assert.False(failure.IsSuccess);

string message = failure.Match(
    success => $"Success: {success}",
    error => $"Error: {error}"
);
Assert.Equal("Error: Error", message);
```

---

## 🎯 Wyzwanie Dodatkowe

### Zadanie 10: Generyczny Lambda Expression Builder

**Opis:** Napisz klasę `ExpressionBuilder<T>` ułatwiającą budowanie zapytań lambda:
- `Where(Func<T, bool> predicate)` - filtruj
- `Select<TResult>(Func<T, TResult> selector)` - transformuj
- `Build()` - zwróć skompilowaną funkcję
- Możliwość łańcuchowania operacji

**Testy:**
```csharp
var builder = new ExpressionBuilder<int>();
var func = builder
    .Where(x => x > 5)
    .Select(x => x * 2)
    .Build();

var data = new[] { 1, 2, 6, 7, 10 };
var result = data.Where(func);  // lub customowa metoda
Assert.Equal(new[] { 12, 14, 20 }, result);
```

---

## 💡 Wskazówki

### Dla Zadań 1-3:
- Używaj `where T : IComparable<T>` do porównywania
- Pamiętaj o walidacji (null, empty arrays)
- Testuj z różnymi typami (int, string, double)

### Dla Zadań 4-6:
- Zastanów się nad wątkami (thread-safety) jeśli chcesz
- Verwendj `Dictionary<TKey, TValue>` wewnętrznie
- Pamiętaj o `GetHashCode()` dla słowników

### Dla Zadań 7-10:
- Stosuj rekursję ostrożnie (stos)
- Implementuj `IEnumerable<T>` gdzie to sensowne
- Rozważ expression trees dla zaawansowanych scenariuszy

---

## 📝 Kryteria Oceny

| Kryterium | Pkt |
|-----------|-----|
| Poprawność kodu | 40% |
| Testy jednostkowe (minimum 5 per zadanie) | 30% |
| Czytelność i dokumentacja | 20% |
| Efektywność (brak zbędnych alokacji) | 10% |

---

## 🔗 Referencje do Kodu

Patrz folder `code/` w tym katalogu:
- `Program.cs` - przykłady implementacji
- `ProgramTests.cs` - testy jednostkowe

Uruchom: `dotnet run` lub `dotnet test`
