# Zadania - Ograniczenia Typów Generycznych

## 📚 Poziom: Podstawowy

### Zadanie 1: Metoda `GetMin` z Ograniczeniem

Napisz metodę `GetMin<T>` która zwraca minimum z dwóch wartości. Metoda powinna implementować `IComparable<T>`.

```csharp
public static T GetMin<T>(T a, T b) where T : IComparable<T>
{
    // TODO: Implementacja
}

// Test
Assert.Equal(5, GetMin(10, 5));
Assert.Equal("apple", GetMin("apple", "zebra"));
```

### Zadanie 2: Klasa `Validator` z Ograniczeniem Interfejsu

Stwórz interfejs `IValidatable` i klasę `Validator<T>` która waliduje obiekty.

```csharp
public interface IValidatable
{
    bool Validate();
    string ErrorMessage { get; }
}

public class Validator<T> where T : IValidatable
{
    // TODO: Implementacja - Validate(T item)
}
```

### Zadanie 3: Factory Pattern

Zaimplementuj klasy `ServiceFactory<T>` z ograniczeniem `new()`:

```csharp
public class ServiceFactory<T> where T : new()
{
    public T CreateService() => new T();
}

var userServiceFactory = new ServiceFactory<UserService>();
var service = userServiceFactory.CreateService();
```

---

## 📚 Poziom: Średniozaawansowany

### Zadanie 4: Repository z Wieloma Ograniczeniami

Stwórz repozytorium dla typów implementujących `IEntity` i `IValidatable`:

```csharp
public interface IEntity
{
    int Id { get; set; }
}

public interface IValidatable
{
    bool IsValid();
}

public class GenericRepository<T> where T : class, IEntity, IValidatable, new()
{
    // Implementacja: Add, Get, GetAll, Update, Delete
}
```

### Zadanie 5: Value Type Container

Stwórz klasę `StronglyTypedValue<T>` tylko dla typów wartościowych:

```csharp
public class StronglyTypedValue<T> where T : struct
{
    private T value;
    
    public StronglyTypedValue(T initialValue) => value = initialValue;
    
    public T Value => value;
    public T GetOrDefault(T defaultValue) => Equals(value, default(T)) ? defaultValue : value;
}
```

### Zadanie 6: Cache Specificy Type

```csharp
public class ReferenceTypeCache<T> where T : class
{
    private Dictionary<int, T> cache = new();
    
    public void Set(int key, T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));
        cache[key] = value;
    }
    
    public T? Get(int key) => cache.TryGetValue(key, out var value) ? value : null;
}
```

---

## 📚 Poziom: Zaawansowany

### Zadanie 7: Dependency Resolver

Stwórz `DependencyResolver<T>` z więcej ograniczeniami:

```csharp
public class DependencyResolver<T> where T : class, new()
{
    private Dictionary<Type, object> dependencies = new();
    
    public T Resolve()
    {
        var instance = new T();
        // Inject properties
        return instance;
    }
}
```

### Zadanie 8: Comparer Adapter

Stwórz `KeyedComparer<T, TKey>` do porównywania po kluczu:

```csharp
public class KeyedComparer<T, TKey> : IComparer<T> 
    where T : class 
    where TKey : IComparable<TKey>
{
    private readonly Func<T, TKey> keySelector;
    
    public KeyedComparer(Func<T, TKey> keySelector)
    {
        this.keySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
    }
    
    public int Compare(T? x, T? y)
    {
        if (x is null || y is null) return 0;
        return keySelector(x).CompareTo(keySelector(y));
    }
}
```

### Zadanie 9: Builder z Ograniczeniami

Stwórz `FluentBuilder<T>` z wieloma ograniczeniami:

```csharp
public class FluentBuilder<T> where T : class, IEntity, new()
{
    private T instance = new T();
    
    public FluentBuilder<T> WithId(int id)
    {
        instance.Id = id;
        return this;
    }
    
    public T Build() => instance;
}
```

---

## 🎯 Wyzwanie Dodatkowe

### Zadanie 10: Generic Converter z Walidacją

```csharp
public interface IConvertible<T>
{
    T Convert();
}

public class TypedConverter<TFrom, TTo> 
    where TFrom : class, IConvertible<TTo>, new()
    where TTo : class, new()
{
    public TTo Convert(TFrom source)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        return source.Convert();
    }
}
```

---

## 📋 Tabela Porównania Ograniczeń

| Ograniczenie | Przykład | Zastosowanie |
|-------------|----------|--------------|
| `where T : IComparable<T>` | Sorting, comparisons | Porównanie wartości |
| `where T : class` | Reference types only | Nullable types |
| `where T : struct` | Value types only | Non-nullable types |
| `where T : new()` | Parameterless constructor | Dependency injection |
| `where T : BaseClass` | Inheritance hierarchy | Base functionality access |
| `where T : Interface` | Interface contract | Specific methods available |
| `where T : notnull` (C# 8+) | Non-null types | Null-safety guaranteed |

---

**Następnie:** Przejdź do tematu [3. IEnumerable i IEnumerator]../_03_ienumerable_ienumerator/
