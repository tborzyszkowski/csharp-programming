# Rozwiązania - Metody i Klasy Generyczne

## Zadanie 1: Metoda `FindMax<T>` ✅

```csharp
public static T FindMax<T>(T[] array) where T : IComparable<T>
{
    if (array == null || array.Length == 0)
        throw new ArgumentException("Array cannot be null or empty", nameof(array));
    
    T max = array[0];
    for (int i = 1; i < array.Length; i++)
    {
        if (array[i].CompareTo(max) > 0)
            max = array[i];
    }
    return max;
}
```

**Wyjaśnienie:**
- `where T : IComparable<T>` - gwarantuje, że typ T może być porównywany
- `CompareTo()` zwraca: < 0 (mniejszy), 0 (równy), > 0 (większy)
- Walidacja na początku zapobiega błędom

---

## Zadanie 2: Klasa `Pair<T>` ✅

```csharp
public class Pair<T> : IEquatable<Pair<T>>
{
    public T First { get; set; }
    public T Second { get; set; }
    
    public Pair(T first, T second)
    {
        First = first;
        Second = second;
    }
    
    public void Swap() => (First, Second) = (Second, First);
    
    public override string ToString() => $"Pair({First}, {Second})";
    
    public override bool Equals(object? obj) => Equals(obj as Pair<T>);
    
    public bool Equals(Pair<T>? other)
    {
        if (other is null) return false;
        return EqualityComparer<T>.Default.Equals(First, other.First) &&
               EqualityComparer<T>.Default.Equals(Second, other.Second);
    }
    
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 31 + First?.GetHashCode() ?? 0;
            hash = hash * 31 + Second?.GetHashCode() ?? 0;
            return hash;
        }
    }
}
```

**Wyjaśnienie:**
- `IEquatable<T>` - dla efektywnego porównywania bez boxing
- `EqualityComparer<T>.Default` - obsługuje null dla typów referencyjnych
- `GetHashCode()` - używa prime numbers (17, 31) dla dobrej dystrybucji

---

## Zadanie 3: Klasa `Queue<T>` ✅

```csharp
public class Queue<T>
{
    private T[] items;
    private int head;  // Indeks pierwszego elementu
    private int tail;  // Indeks dla nowego elementu
    private int count;
    
    public Queue(int capacity = 10)
    {
        items = new T[capacity];
        head = 0;
        tail = 0;
        count = 0;
    }
    
    public void Enqueue(T item)
    {
        if (count == items.Length)
            Resize();
        
        items[tail] = item;
        tail = (tail + 1) % items.Length;
        count++;
    }
    
    public T Dequeue()
    {
        if (count == 0)
            throw new InvalidOperationException("Queue is empty");
        
        T item = items[head];
        head = (head + 1) % items.Length;
        count--;
        return item;
    }
    
    public T Peek()
    {
        if (count == 0)
            throw new InvalidOperationException("Queue is empty");
        return items[head];
    }
    
    public bool IsEmpty => count == 0;
    public int Count => count;
    
    private void Resize()
    {
        T[] newItems = new T[items.Length * 2];
        for (int i = 0; i < count; i++)
        {
            newItems[i] = items[(head + i) % items.Length];
        }
        items = newItems;
        head = 0;
        tail = count;
    }
}
```

**Wyjaśnienie:**
- Circular buffer - efektywnie wykorzystuje pamięć
- `(head + i) % items.Length` - obsługuje "zawinięcie" w tablicy
- `Resize()` - kopiuje elementy w poprawnej kolejności

---

## Zadanie 4: Cache Generyczny ✅

```csharp
public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int maxSize;
    private readonly Dictionary<TKey, TValue> data = new();
    private readonly LinkedList<TKey> order = new();  // Kolejność dodania
    
    public Cache(int maxSize)
    {
        if (maxSize <= 0)
            throw new ArgumentException("Max size must be positive", nameof(maxSize));
        this.maxSize = maxSize;
    }
    
    public void Set(TKey key, TValue value)
    {
        if (data.ContainsKey(key))
        {
            data[key] = value;
            order.Remove(order.First(n => n.Value.Equals(key)));
            order.AddLast(key);
        }
        else
        {
            if (data.Count >= maxSize)
            {
                var oldKey = order.First!.Value;
                order.RemoveFirst();
                data.Remove(oldKey);
            }
            data[key] = value;
            order.AddLast(key);
        }
    }
    
    public TValue? Get(TKey key)
    {
        if (data.TryGetValue(key, out var value))
            return value;
        return default;
    }
    
    public void Remove(TKey key)
    {
        if (data.Remove(key))
        {
            var node = order.First(n => n.Equals(key));
            order.Remove(node);
        }
    }
    
    public void Clear()
    {
        data.Clear();
        order.Clear();
    }
    
    public int Count => data.Count;
}
```

**Wyjaśnienie:**
- `LinkedList<TKey>` - śledzi kolejność dodania (FIFO)
- Usunięcie najstarszego elementu gdy cache jest pełny
- `where TKey : notnull` - wymagaj wartościowych kluczy (C# 11)

---

## Zadanie 5: PropertyComparer<T, TProp> ✅

```csharp
public class PropertyComparer<T, TProp> : IComparer<T> 
    where TProp : IComparable<TProp>
{
    private readonly Func<T, TProp> selector;
    private readonly bool descending;
    
    public PropertyComparer(Func<T, TProp> selector, bool descending = false)
    {
        this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        this.descending = descending;
    }
    
    public int Compare(T? x, T? y)
    {
        if (x is null || y is null)
            return 0;
        
        TProp propX = selector(x);
        TProp propY = selector(y);
        
        int result = propX.CompareTo(propY);
        return descending ? -result : result;
    }
}

// Użycie
public record Person(string Name, int Age);

var people = new List<Person>
{
    new("Charlie", 35),
    new("Alice", 30),
    new("Bob", 25)
};

var nameComparer = new PropertyComparer<Person, string>(p => p.Name);
people.Sort(nameComparer);
// Teraz: Alice, Bob, Charlie
```

**Wyjaśnienie:**
- `Func<T, TProp>` - lambda expression selektujący właściwość
- `descending` - parametr dla sortowania malejącego
- `IComparable<TProp>` - wymaga, aby TProp był porównywalny

---

## Zadanie 6: Observable<T> ✅

```csharp
public class Observable<T>
{
    private T value;
    private readonly List<Action<T>> observers = new();
    
    public T Value
    {
        get => value;
        set
        {
            this.value = value;
            Notify(value);
        }
    }
    
    public Observable(T initialValue)
    {
        value = initialValue;
    }
    
    public void Subscribe(Action<T> observer)
    {
        if (observer != null)
            observers.Add(observer);
    }
    
    public void Unsubscribe(Action<T> observer)
    {
        observers.Remove(observer);
    }
    
    private void Notify(T newValue)
    {
        foreach (var observer in observers)
        {
            observer(newValue);
        }
    }
}

// Użycie
var observable = new Observable<int>(0);
var changes = new List<int>();

observable.Subscribe(v => changes.Add(v));
observable.Value = 10;
observable.Value = 20;

// changes: [10, 20]
```

**Wyjaśnienie:**
- Observer Pattern - subscribers są powiadamiani o zmianach
- `Notify()` - wołana gdy Value się zmienia
- `Unsubscribe()` - czyszczenie pamieci

---

## Zadanie 7: BinaryTree<T> ✅

```csharp
public class TreeNode<T>
{
    public T Value { get; set; }
    public TreeNode<T>? Left { get; set; }
    public TreeNode<T>? Right { get; set; }
    
    public TreeNode(T value)
    {
        Value = value;
    }
}

public class BinaryTree<T> where T : IComparable<T>
{
    public TreeNode<T>? Root { get; private set; }
    
    public void Insert(T value)
    {
        if (Root is null)
        {
            Root = new TreeNode<T>(value);
        }
        else
        {
            InsertRecursive(Root, value);
        }
    }
    
    private void InsertRecursive(TreeNode<T> node, T value)
    {
        if (value.CompareTo(node.Value) < 0)
        {
            if (node.Left is null)
                node.Left = new TreeNode<T>(value);
            else
                InsertRecursive(node.Left, value);
        }
        else
        {
            if (node.Right is null)
                node.Right = new TreeNode<T>(value);
            else
                InsertRecursive(node.Right, value);
        }
    }
    
    public bool Contains(T value) => ContainsRecursive(Root, value);
    
    private bool ContainsRecursive(TreeNode<T>? node, T value)
    {
        if (node is null) return false;
        
        int cmp = value.CompareTo(node.Value);
        if (cmp == 0) return true;
        if (cmp < 0) return ContainsRecursive(node.Left, value);
        return ContainsRecursive(node.Right, value);
    }
    
    public List<T> Traverse()
    {
        var result = new List<T>();
        TraverseInOrder(Root, result);
        return result;
    }
    
    private void TraverseInOrder(TreeNode<T>? node, List<T> result)
    {
        if (node is null) return;
        TraverseInOrder(node.Left, result);
        result.Add(node.Value);
        TraverseInOrder(node.Right, result);
    }
}

// Testy
var tree = new BinaryTree<int>();
tree.Insert(5);
tree.Insert(3);
tree.Insert(7);
tree.Insert(1);
tree.Insert(9);

var inOrder = tree.Traverse();  // [1, 3, 5, 7, 9]
Assert.True(tree.Contains(7));
```

**Wyjaśnienie:**
- BST (Binary Search Tree) - lewa < rodzic < prawa
- In-order traversal - zwraca posortowane elementy
- `CompareTo()` do lokalizacji właściwego miejsca

---

## Zadanie 8: MultiMap<TKey, TValue> ✅

```csharp
public class MultiMap<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, List<TValue>> data = new();
    
    public void Add(TKey key, TValue value)
    {
        if (!data.ContainsKey(key))
            data[key] = new List<TValue>();
        data[key].Add(value);
    }
    
    public IEnumerable<TValue> Get(TKey key)
    {
        if (data.TryGetValue(key, out var values))
            return values.AsReadOnly();
        return Enumerable.Empty<TValue>();
    }
    
    public void Remove(TKey key, TValue value)
    {
        if (data.TryGetValue(key, out var values))
            values.Remove(value);
    }
    
    public void RemoveAll(TKey key) => data.Remove(key);
    
    public bool ContainsKey(TKey key) => data.ContainsKey(key);
    
    public IEnumerable<TKey> Keys => data.Keys;
}

// Użycie
var multiMap = new MultiMap<string, int>();
multiMap.Add("key", 1);
multiMap.Add("key", 2);
multiMap.Add("key", 3);

var values = multiMap.Get("key");  // [1, 2, 3]
```

**Wyjaśnienie:**
- `Dictionary<TKey, List<TValue>>` - wewnętrzna struktura
- `AsReadOnly()` - zapobiega modyfikacji z zewnątrz
- `Enumerable.Empty<TValue>()` - bezpieczny dla nieistniejących kluczy

---

## Zadanie 9: Result<T, TError> ✅

```csharp
public class Result<T, TError>
{
    public bool IsSuccess { get; }
    public T? SuccessValue { get; }
    public TError? ErrorValue { get; }
    
    private Result(bool isSuccess, T? successValue, TError? errorValue)
    {
        IsSuccess = isSuccess;
        SuccessValue = successValue;
        ErrorValue = errorValue;
    }
    
    public static Result<T, TError> Success(T value) => 
        new(true, value, default);
    
    public static Result<T, TError> Failure(TError error) => 
        new(false, default, error);
    
    public Result<TResult, TError> Map<TResult>(Func<T, TResult> transform)
    {
        return IsSuccess 
            ? Result<TResult, TError>.Success(transform(SuccessValue!))
            : Result<TResult, TError>.Failure(ErrorValue!);
    }
    
    public Result<TResult, TError> Bind<TResult>(
        Func<T, Result<TResult, TError>> transform)
    {
        return IsSuccess 
            ? transform(SuccessValue!)
            : Result<TResult, TError>.Failure(ErrorValue!);
    }
    
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<TError, TResult> onFailure)
    {
        return IsSuccess 
            ? onSuccess(SuccessValue!)
            : onFailure(ErrorValue!);
    }
    
    public T Unwrap()
    {
        if (!IsSuccess)
            throw new InvalidOperationException($"Result is failure: {ErrorValue}");
        return SuccessValue!;
    }
}

// Użycie
Result<int, string> success = Result<int, string>.Success(42);
var doubled = success.Map(x => x * 2);  // Success(84)

Result<int, string> failure = Result<int, string>.Failure("Error");
string message = failure.Match(
    s => $"Success: {s}",
    e => $"Error: {e}"
);  // "Error: Error"
```

**Wyjaśnienie:**
- Railway-Oriented Programming - reprezentacja sukcesu/porażki
- `Map()` - transformacja w succesie
- `Bind()` - łańcuchowanie operacji zwracających Result
- `Match()` - pattern matching na wyniku

---

## Zadanie 10: ExpressionBuilder<T> ✅

```csharp
public class ExpressionBuilder<T>
{
    private List<Func<T, bool>> whereConditions = new();
    private Func<T, T>? selectTransform = null;
    
    public ExpressionBuilder<T> Where(Func<T, bool> predicate)
    {
        whereConditions.Add(predicate);
        return this;
    }
    
    public ExpressionBuilder<T> Select(Func<T, T> transform)
    {
        selectTransform = transform;
        return this;
    }
    
    public Func<T, bool> Build()
    {
        return item =>
        {
            // Sprawdź wszystkie Where warunki
            foreach (var condition in whereConditions)
            {
                if (!condition(item))
                    return false;
            }
            return true;
        };
    }
    
    public IEnumerable<T> Apply(IEnumerable<T> data)
    {
        var predicate = Build();
        foreach (var item in data)
        {
            if (predicate(item))
            {
                var result = selectTransform != null ? selectTransform(item) : item;
                yield return result;
            }
        }
    }
}

// Użycie
var builder = new ExpressionBuilder<int>();
var filtered = builder
    .Where(x => x > 5)
    .Where(x => x < 100)
    .Select(x => x * 2)
    .Apply(new[] { 1, 6, 10, 50, 200 });

// Wynik: 12, 20, 100
```

**Wyjaśnienie:**
- Fluent API - `.Method().Method().Build()`
- `whereConditions` - lista warunków (AND)
- `Apply()` - generator - leniva ewaluacja (lazy evaluation)

---

## 📚 Podsumowanie

Wszystkie rozwiązania demonstrują:
- ✅ Poprawne użycie ograniczeń generyk (`where T :`)
- ✅ Bezpieczeństwo typów
- ✅ Efektywne struktury danych
- ✅ Wzorce projektowe (Observer, Factory, etc.)
- ✅ Functional programming (Map, Bind, Match)

**Następnie:** Przejdź do tematu [2. Ograniczenia Typów Generycznych]../_02_generic_constraints/
