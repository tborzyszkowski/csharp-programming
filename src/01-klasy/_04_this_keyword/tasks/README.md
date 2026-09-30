# Zadania - this

## 📝 Zadanie 1: Builder pattern

Utwórz klasę `SqlQuery` z fluent API korzystającą z `this`:

```csharp
var query = new SqlQuery()
    .Select("Name", "Age")
    .From("Users")
    .Where("Age > 18")
    .Where("City = 'Kraków'")     // kolejne Where łączymy przez AND
    .OrderBy("Name");

Console.WriteLine(query.Build());
// SELECT Name, Age FROM Users WHERE Age > 18 AND City = 'Kraków' ORDER BY Name
```

---

## 📝 Zadanie 2: Łańcuch konstruktorów

Utwórz klasę `Address` z właściwościami `Street`, `City`, `ZipCode`, `Country` (tylko do odczytu) i konstruktorami:

- `Address(string street, string city)` – kod pocztowy `"00-000"`, kraj `"Polska"`
- `Address(string street, string city, string zipCode)` – kraj `"Polska"`
- `Address(string street, string city, string zipCode, string country)` – jedyny konstruktor z logiką
  (walidacja: żaden argument nie może być pusty, w przeciwnym razie `ArgumentException`)

Konstruktory 1 i 2 mają **wyłącznie delegują** do konstruktora 3 przez `this(...)`.

---

## 📝 Zadanie 3: Znajdź błąd

Poniższa klasa kompiluje się, ale test `Assert.Equal("Jan", person.Name)` nie przechodzi.
Znajdź przyczynę, popraw kod na **dwa sposoby** (z `this.` i bez niego) i wyjaśnij, jaki komunikat
kompilatora powinien był Cię ostrzec.

```csharp
public class Person
{
    private string name;
    private int age;

    public Person(string name, int age)
    {
        name = name;
        age = age;
    }

    public string Name => name;
    public int Age => age;
}
```

---

## ✅ Zadanie 1 - Rozwiązanie: SqlQuery Builder Pattern

### Kod

```csharp
public class SqlQuery
{
    private string[] columns = Array.Empty<string>();
    private string? table;
    private readonly List<string> conditions = new();
    private string[] orderColumns = Array.Empty<string>();

    public SqlQuery Select(params string[] columns)
    {
        this.columns = columns;   // this. rozróżnia pole od parametru
        return this;              // Zwraca bieżący obiekt
    }

    public SqlQuery From(string table)
    {
        this.table = table;
        return this;              // Umożliwia łańcuchowanie
    }

    public SqlQuery Where(string condition)
    {
        conditions.Add(condition);   // kolejne wywołania się kumulują
        return this;
    }

    public SqlQuery OrderBy(params string[] columns)
    {
        orderColumns = columns;
        return this;
    }

    public string Build()
    {
        if (columns.Length == 0 || table is null)
            throw new InvalidOperationException("Zapytanie wymaga Select(...) i From(...)");

        var sql = $"SELECT {string.Join(", ", columns)} FROM {table}";

        if (conditions.Count > 0)
            sql += " WHERE " + string.Join(" AND ", conditions);

        if (orderColumns.Length > 0)
            sql += " ORDER BY " + string.Join(", ", orderColumns);

        return sql;
    }

    public override string ToString() => Build();
}

// W Main():
var query1 = new SqlQuery()
    .Select("Name", "Age")
    .From("Users")
    .Where("Age > 18")
    .OrderBy("Name");

Console.WriteLine(query1.Build());
// SELECT Name, Age FROM Users WHERE Age > 18 ORDER BY Name

var query2 = new SqlQuery()
    .Select("*")
    .From("Orders")
    .Where("Total > 100")
    .Where("Status = 'Pending'")
    .OrderBy("CreatedDate");

Console.WriteLine(query2.Build());
// SELECT * FROM Orders WHERE Total > 100 AND Status = 'Pending' ORDER BY CreatedDate
```

### Wyjaśnienie

- Każda metoda konfigurująca zwraca `this` (referencję do bieżącego obiektu) – to umożliwia
  **łańcuchowanie metod** (fluent API)
- `Select`, `From`, `Where`, `OrderBy` można wywoływać w dowolnej kolejności, bo dopiero `Build()` składa wynik
- **Zaleta**: Kod jest czytelny i ekspresyjny
- **Wzorzec**: Builder / method chaining

> ⚠️ **Bezpieczeństwo:** ten builder skleja tekst z argumentów, więc jest **podatny na SQL injection**,
> jeśli do `Where(...)` trafią dane od użytkownika (np. `Where("Name = '" + input + "'")`). W prawdziwej aplikacji
> zapytania buduje się z **parametrami** (`@name` + `SqlParameter`) lub przez ORM (Entity Framework, LINQ).
> To zadanie służy wyłącznie do ćwiczenia `this` – nie używaj go do realnych zapytań.

### Testy

```csharp
[Fact]
public void SelectAndFrom_BuildsCorrectQuery()
{
    var query = new SqlQuery()
        .Select("Name", "Email")
        .From("Users")
        .Build();

    Assert.Equal("SELECT Name, Email FROM Users", query);
}

[Fact]
public void FullQuery_BuildsCompleteStatement()
{
    var query = new SqlQuery()
        .Select("*")
        .From("Products")
        .Where("Price > 100")
        .OrderBy("Name")
        .Build();

    Assert.Equal("SELECT * FROM Products WHERE Price > 100 ORDER BY Name", query);
}

[Fact]
public void MultipleWhere_AreCombinedWithAnd()
{
    var query = new SqlQuery()
        .Select("*")
        .From("Orders")
        .Where("Total > 100")
        .Where("Status = 'Pending'")
        .Build();

    Assert.Equal("SELECT * FROM Orders WHERE Total > 100 AND Status = 'Pending'", query);
}

[Fact]
public void Methods_ReturnTheSameInstance()
{
    var builder = new SqlQuery();

    Assert.Same(builder, builder.Select("*"));
    Assert.Same(builder, builder.From("T"));
}

[Fact]
public void Build_WithoutFrom_Throws()
{
    Assert.Throws<InvalidOperationException>(() => new SqlQuery().Select("*").Build());
}
```

---

## ✅ Zadanie 2 - Rozwiązanie: Address

```csharp
public class Address
{
    public string Street { get; }
    public string City { get; }
    public string ZipCode { get; }
    public string Country { get; }

    // 1. Deleguje do konstruktora głównego
    public Address(string street, string city)
        : this(street, city, "00-000") { }

    // 2. Deleguje do konstruktora głównego
    public Address(string street, string city, string zipCode)
        : this(street, city, zipCode, "Polska") { }

    // 3. Jedyne miejsce z logiką inicjalizacji
    public Address(string street, string city, string zipCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street)) throw new ArgumentException("Ulica jest wymagana", nameof(street));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("Miasto jest wymagane", nameof(city));
        if (string.IsNullOrWhiteSpace(zipCode)) throw new ArgumentException("Kod pocztowy jest wymagany", nameof(zipCode));
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Kraj jest wymagany", nameof(country));

        Street = street;
        City = city;
        ZipCode = zipCode;
        Country = country;
    }
}

[Fact]
public void ShortConstructor_UsesDefaults()
{
    var address = new Address("Długa 1", "Gdańsk");

    Assert.Equal("00-000", address.ZipCode);
    Assert.Equal("Polska", address.Country);
}

[Fact]
public void AllConstructors_GoThroughValidation()
{
    // Walidacja działa także dla skróconych konstruktorów - dzięki this(...)
    Assert.Throws<ArgumentException>(() => new Address("", "Gdańsk"));
}
```

**Wyjaśnienie:** walidacja znajduje się w jednym miejscu, więc nie da się utworzyć niepoprawnego
obiektu żadnym konstruktorem. Konstruktor wywołany przez `this(...)` wykonuje się w całości (z ciałem)
**przed** ciałem konstruktora, który go wywołał.

---

## ✅ Zadanie 3 - Rozwiązanie: Znajdź błąd

**Przyczyna:** parametry `name` i `age` **przesłaniają** pola o tych samych nazwach. Instrukcja
`name = name;` przypisuje parametr do niego samego, a pole `this.name` zostaje `null`
(dla `age` – `0`). Kompilator zgłasza ostrzeżenie **CS1717: Assignment made to same variable**.

**Poprawka 1 – z `this.`:**

```csharp
public Person(string name, int age)
{
    this.name = name;
    this.age = age;
}
```

**Poprawka 2 – bez `this.` (prefiks `_` w polach):**

```csharp
private string _name;
private int _age;

public Person(string name, int age)
{
    _name = name;
    _age = age;
}
```

**Wniosek:** ostrzeżenia kompilatora (zwłaszcza CS1717, CS8618) warto traktować jak błędy – w projekcie można
to wymusić przez `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.

