# Zadania - Tworzenie i Korzystanie z Obiektów

## 📝 Zadanie 1: Kolekcja książek

Utwórz klasę `Book` i system do zarządzania kolekcją książek.

### Wymagania

```csharp
public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }

    public Book(string title, string author, int year) { /* ... */ }
}

var books = new List<Book>
{
    new("...", "...", 2024),
    ...
};
```

- Utwórz 5 książek (kolekcja + target-typed `new`)
- Wypisz informacje o każdej
- Porównaj dwie książki (`Equals` kontra `ReferenceEquals`)

---

## 📝 Zadanie 2: Porównanie referencji

Utwórz test demonstrujący różnicę między `ReferenceEquals` a `Equals`.

### Wymagania

- Stwórz 2 obiekty Person z identyczną zawartością
- Stwórz referencję do pierwszego
- Sprawdź ReferenceEquals dla każdej pary
- Sprawdź Equals dla każdej pary

---

## 📝 Zadanie 3: Referencje w metodach

Zaimplementuj i przetestuj trzy metody przyjmujące `Person`:

1. `Birthday(Person p)` - zwiększa `Age` o 1
2. `Reset(Person p)` - przypisuje do parametru `new Person { Name = "", Age = 0 }`
3. `ResetByRef(ref Person p)` - jak wyżej, ale z modyfikatorem `ref`

Dla każdej z nich przewidź (zanim uruchomisz kod), czy zmienna wołającego się zmieni, i uzasadnij to
rysunkiem zmiennych i obiektów. Zweryfikuj przewidywania testami xUnit.

---

## ✅ Zadanie 1 - Rozwiązanie: Kolekcja Książek

### Kod

```csharp
public class Book
{
    public string Title { get; }
    public string Author { get; }
    public int Year { get; }
    
    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Book other) return false;
        return Title == other.Title && 
               Author == other.Author && 
               Year == other.Year;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Author, Year);
    }
    
    public override string ToString() => $"{Title} - {Author} ({Year})";
}

// W Main():
var books = new List<Book>
{
    new Book("Quo Vadis", "Henryk Sienkiewicz", 1896),
    new Book("Pan Tadeusz", "Adam Mickiewicz", 1834),
    new Book("Lalka", "Bolesław Prus", 1890),
    new Book("Krzyżacy", "Henryk Sienkiewicz", 1900),
    new Book("Pani Bovary", "Gustave Flaubert", 1857)
};

Console.WriteLine("📚 Kolekcja książek:");
foreach (var book in books)
{
    Console.WriteLine($"  {book}");
}

// Porównanie
var book1 = new Book("Quo Vadis", "Henryk Sienkiewicz", 1896);
var book2 = new Book("Quo Vadis", "Henryk Sienkiewicz", 1896);
var book3 = book1;

Console.WriteLine($"\nbook1.Equals(book2): {book1.Equals(book2)}");     // true
Console.WriteLine($"ReferenceEquals(book1, book2): {ReferenceEquals(book1, book2)}"); // false
Console.WriteLine($"ReferenceEquals(book1, book3): {ReferenceEquals(book1, book3)}");  // true
```

### Wyjaśnienie

- Klasa `Book` ma **3 właściwości tylko do odczytu** (`{ get; }`) ustawiane w konstruktorze – obiekt jest niezmienny
- Override `Equals()` porównuje zawartość (tytuł, autora, rok)
- Override `GetHashCode()` jest konieczny, gdy nadpisujemy `Equals()` (te same pola w obu metodach)
- **5 książek** tworzymy w inicjalizatorze kolekcji `List<Book>`
- Demonstracja różnicy między `Equals()` (zawartość) a `ReferenceEquals()` (identyczność)
- Ponieważ `Book` jest niezmienna, jej `GetHashCode()` nie zmieni się w czasie życia obiektu – to bezpieczne
  do użycia jako klucz w `Dictionary` / element `HashSet`

---

## ✅ Zadanie 2 - Rozwiązanie: ReferenceEquals vs Equals

### Kod

```csharp
public class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Person other) return false;
        return Name == other.Name && Age == other.Age;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Age);
    }
    
    public override string ToString() => $"{Name}, {Age} lat";
}

// Test porównania
var person1 = new Person { Name = "Jan", Age = 30 };
var person2 = new Person { Name = "Jan", Age = 30 };  // Inna instancja, ta sama zawartość
var person3 = person1;                                  // Referencja do person1

Console.WriteLine("\n⚖️  ReferenceEquals vs Equals");
Console.WriteLine($"person1: {person1}");
Console.WriteLine($"person2: {person2}");
Console.WriteLine($"person3: {person3}");

Console.WriteLine("\nReferenceEquals (czy to ten sam obiekt?):");
Console.WriteLine($"  ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}");  // false
Console.WriteLine($"  ReferenceEquals(person1, person3): {ReferenceEquals(person1, person3)}");  // true

Console.WriteLine("\nEquals (czy mają tę samą zawartość?):");
Console.WriteLine($"  person1.Equals(person2): {person1.Equals(person2)}");  // true
Console.WriteLine($"  person1.Equals(person3): {person1.Equals(person3)}");  // true

Console.WriteLine("\nOperator == (dla klas = ReferenceEquals):");
Console.WriteLine($"  person1 == person2: {person1 == person2}");  // false
Console.WriteLine($"  person1 == person3: {person1 == person3}");  // true
```

### Wyjaśnienie

- **ReferenceEquals**: Sprawdza czy obie zmienne wskazują na **ten sam obiekt** w pamięci
  - `person1` i `person2` to różne obiekty → `false`
  - `person1` i `person3` to ta sama instancja → `true`
  
- **Equals()**: Sprawdza czy obiekty mają **tę samą zawartość**
  - Override porównuje pola (Name, Age)
  - Zarówno `person1.Equals(person2)` i `person1.Equals(person3)` → `true`
  
- **Operator ==**: Dla klas domyślnie sprawdza referencję (jak ReferenceEquals)
  - Możemy go przeciążyć (razem z `!=`), aby porównywał zawartość (jak Equals)

---

## ✅ Zadanie 3 - Rozwiązanie: Referencje w metodach

### Kod

```csharp
public static class PersonOperations
{
    // Modyfikuje OBIEKT, na który wskazuje kopia referencji - zmiana widoczna u wołającego
    public static void Birthday(Person p) => p.Age++;

    // Przypisuje nową wartość do LOKALNEJ kopii referencji - wołający tego nie zobaczy
    public static void Reset(Person p) => p = new Person { Name = "", Age = 0 };

    // Z ref metoda dostaje samą zmienną wołającego, więc może ją przepiąć na inny obiekt
    public static void ResetByRef(ref Person p) => p = new Person { Name = "", Age = 0 };
}

[Fact]
public void Birthday_ModifiesCallersObject()
{
    var person = new Person { Name = "Jan", Age = 30 };

    PersonOperations.Birthday(person);

    Assert.Equal(31, person.Age);
}

[Fact]
public void Reset_DoesNotChangeCallersVariable()
{
    var person = new Person { Name = "Jan", Age = 30 };
    var original = person;

    PersonOperations.Reset(person);

    Assert.Same(original, person);   // nadal ten sam obiekt
    Assert.Equal("Jan", person.Name);
}

[Fact]
public void ResetByRef_ReplacesCallersVariable()
{
    var person = new Person { Name = "Jan", Age = 30 };
    var original = person;

    PersonOperations.ResetByRef(ref person);

    Assert.NotSame(original, person);
    Assert.Equal(0, person.Age);
    Assert.Equal("Jan", original.Name);   // stary obiekt nietknięty
}
```

### Wyjaśnienie

- W C# argumenty przekazuje się **przez wartość**. Dla typów referencyjnych wartością jest *referencja*,
  więc metoda dostaje jej kopię: obie wskazują na ten sam obiekt.
- `Birthday` zmienia stan wspólnego obiektu – widać to u wołającego.
- `Reset` zmienia tylko to, na co wskazuje lokalny parametr – zmienna wołającego pozostaje bez zmian.
- `ref` przekazuje alias samej zmiennej wołającego, dlatego podmiana obiektu jest widoczna na zewnątrz.

---

## ✅ Rozwiązania

Patrz plik `Program.cs` - zawiera pełne demonstracje wszystkich koncepcji.

