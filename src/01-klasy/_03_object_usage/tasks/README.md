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
}

var books = new List<Book>
{
    new() { Title = "...", Author = "...", Year = 2024 },
    ...
};
```

- Utwórz 5 książek za pomocą inicjalizatora
- Wypisz informacje o każdej
- Porównaj dwie książki (equals)

---

## 📝 Zadanie 2: Porównanie referencji

Utwórz test demonstrujący różnicę między `ReferenceEquals` a `Equals`.

### Wymagania

- Stwórz 2 obiekty Person z identyczną zawartością
- Stwórz referencję do pierwszego
- Sprawdź ReferenceEquals dla każdej pary
- Sprawdź Equals dla każdej pary

---

## ✅ Zadanie 1 - Rozwiązanie: Kolekcja Książek

### Kod

```csharp
public class Book
{
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int Year { get; private set; }
    
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
Console.WriteLine($"ReferenceEquals(book1, book2): {ReferenceEquals(book1, book2)}")); // false
Console.WriteLine($"ReferenceEquals(book1, book3): {ReferenceEquals(book1, book3)}");  // true
```

### Wyjaśnienie

- Klasa `Book` ma **3 read-only properties** (dobrze dla immutable design)
- Override `Equals()` porównuje zawartość (tytuł, autora, rok)
- Override `GetHashCode()` jest konieczny gdy overridujemy `Equals()`
- **3 książki** tworzone ze zmiennych (pattern-matching)
- Demonstracja różnicy między `Equals()` (zawartość) a `ReferenceEquals()` (identyczność)

---

## ✅ Zadanie 2 - Rozwiązanie: ReferenceEquals vs Equals

### Kod

```csharp
public class Person
{
    public string Name { get; set; }
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
var person2 = new Person { Name = "Jan", Age = 30 };  // Inna instancja, ta zawartość
var person3 = person1;                                  // Referencja do person1

Console.WriteLine("\n⚖️  ReferenceEquals vs Equals");
Console.WriteLine($"person1: {person1}");
Console.WriteLine($"person2: {person2}");
Console.WriteLine($"person3: {person3}");

Console.WriteLine("\nReferenceEquals (czy tensamobjekt?):");
Console.WriteLine($"  ReferenceEquals(person1, person2): {ReferenceEquals(person1, person2)}");  // false
Console.WriteLine($"  ReferenceEquals(person1, person3): {ReferenceEquals(person1, person3)}");  // true

Console.WriteLine("\nEquals (czy tanazawartość?):");
Console.WriteLine($"  person1.Equals(person2): {person1.Equals(person2)}");  // true
Console.WriteLine($"  person1.Equals(person3): {person1.Equals(person3)}");  // true

Console.WriteLine("\nOperator == (dla klas = ReferenceEquals):");
Console.WriteLine($"  person1 == person2: {person1 == person2}");  // false
Console.WriteLine($"  person1 == person3: {person1 == person3}");  // true
```

### Wyjaśnienie

- **ReferenceEquals**: Sprawdza czy obie zmienne wskazują na **tensamobjekt** w pamięci
  - `person1` i `person2` to różne obiekty → `false`
  - `person1` i `person3` to ta sama instancja → `true`
  
- **Equals()**: Sprawdza czy obiekty mają **tę samą zawartość**
  - Override porównuje pola (Name, Age)
  - Zarówno `person1.Equals(person2)` i `person1.Equals(person3)` → `true`
  
- **Operator ==**: Dla klas domyślnie sprawdza referencję (jak ReferenceEquals)
  - Możemy override aby porównywać zawartość (jak Equals)

---

## ✅ Rozwiązania

Patrz plik `Program.cs` - zawiera pełne demonstracje wszystkich koncepcji.

