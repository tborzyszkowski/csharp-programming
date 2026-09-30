# Słowo Kluczowe this

## 🎯 Cel

Zrozumienie słowa kluczowego `this` - referencji do bieżącego obiektu.

---

## 🚀 Jak pracować z tym tematem

### Uruchomienie kodu

```bash
cd code/

# Demonstracja tego słowa kluczowego
dotnet run

# Testy
dotnet test
```

### Zadania dla studentów

[📝 ZADANIA](tasks/README.md) – Praktyka z `this`:
- Constructor chaining
- Fluent API
- Odróżnianie pól od parametrów

---

## Zastosowania

### 1. Odróżnienie pól od parametrów

```csharp
public class Person
{
    private string name;
    private int age;
    
    public Person(string name, int age)
    {
        this.name = name;  // this.name = pole klasy, name = parametr
        this.age = age;    // this.age = pole klasy, age = parametr
    }
}
```

Parametr o tej samej nazwie co pole **przesłania** pole. Bez `this` przypisanie `name = name;` dotyczy
parametru samego do siebie – kompilator ostrzega (CS1717), a pole zostaje niezainicjowane.

> W stylu z prefiksem `_` (`_name = name;`) `this` nie jest tu potrzebne – patrz konwencje w temacie 2.

### 2. Łańcuch konstruktorów (Constructor Chaining)

```csharp
public class Person
{
    private string name;
    private int age;
    private string city;
    
    public Person() : this("Unknown", 0, "Unknown") { }
    
    public Person(string name) : this(name, 0, "Unknown") { }
    
    // Konstruktor "główny" - jedyne miejsce z logiką inicjalizacji
    public Person(string name, int age, string city)
    {
        this.name = name;
        this.age = age;
        this.city = city;
    }
}
```

`: this(...)` woła inny konstruktor **tej samej klasy** i wykonuje się **przed** ciałem bieżącego
konstruktora. (`: base(...)` woła konstruktor klasy bazowej – w jednym konstruktorze można użyć tylko jednego z nich.)
Dzięki łańcuchowaniu logika inicjalizacji nie jest powielana.

### 3. Zwracanie bieżącego obiektu (Fluent API)

```csharp
public class TextBuilder   // (nie nazywamy go StringBuilder, żeby nie mylić z System.Text.StringBuilder)
{
    private string content = "";
    
    public TextBuilder Append(string text)
    {
        content += text;
        return this;  // Zwraca bieżący obiekt
    }
    
    public TextBuilder AppendLine()
    {
        content += Environment.NewLine;
        return this;
    }
    
    public string Build() => content;
}

// Użycie - fluent API
var text = new TextBuilder()
    .Append("Hello")
    .Append(" ")
    .Append("World")
    .AppendLine()
    .Build();
```

> Wersja w `Program.cs` skleja napisy operatorem `+=`, co tworzy nowy `string` przy każdym wywołaniu.
> To celowe uproszczenie dydaktyczne; w realnym kodzie do składania tekstu służy `System.Text.StringBuilder`
> – który zresztą też zwraca `this`, więc można go łańcuchować dokładnie tak samo.

### 4. Przekazywanie referencji do metody

```csharp
public class Person
{
    public string Name { get; }
    
    public Person(string name) => Name = name;
    
    public void Introduce(Action<Person> action)
    {
        action(this);  // Przekazuje bieżący obiekt
    }
}

var person = new Person("Jan");
person.Introduce(p => Console.WriteLine($"Osoba: {p.Name}"));
```

Tak samo obiekt może zarejestrować się w innej strukturze: `registry.Register(this);`.
Nie rób tego w konstruktorze, jeśli obiekt nie jest jeszcze w pełni zainicjowany (*this escape*).

### 5. Inne zastosowania słowa `this`

```csharp
// a) Indeksator: obiekt indeksowany jak tablica
public class Playlist
{
    private readonly List<string> songs = new();
    public string this[int index] => songs[index];
}

// b) Metoda rozszerzająca: 'this' przy pierwszym parametrze metody statycznej
public static class StringExtensions
{
    public static bool IsBlank(this string s) => string.IsNullOrWhiteSpace(s);
}
// użycie: "  ".IsBlank()
```

(Oba mechanizmy poznasz dokładniej w modułach o właściwościach i metodach statycznych.)

### Ograniczenia

- `this` nie istnieje w metodach i konstruktorach **statycznych** – nie ma wtedy żadnego „bieżącego obiektu”.
- W klasie nie można przypisać do `this` (`this = ...` jest błędem); w strukturach `this` jest zmienną typu `ref`.
- `this` jest domyślnie dopisywane przez kompilator przy każdym odwołaniu do składowej instancji,
  więc jawnie piszemy je tylko tam, gdzie coś rozstrzyga (przesłonięcie, zwrócenie, przekazanie).

## Podsumowanie

- `this` wskazuje na bieżący obiekt (instancję), na którym wykonywana jest metoda
- Rozróżnia pola od parametrów o tej samej nazwie
- `: this(...)` umożliwia łańcuchowanie konstruktorów
- `return this` daje fluent API
- Nie istnieje w kontekście statycznym

---

## 📖 Referencje

[Microsoft Docs - this](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/this)

