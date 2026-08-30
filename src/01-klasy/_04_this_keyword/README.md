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
        this.name = name;  // this.name = pole klasy
        this.age = age;    // age = parametr
    }
}
```

### 2. Łańcuch konstruktorów (Constructor Chaining)

```csharp
public class Person
{
    private string name;
    private int age;
    private string city;
    
    public Person() : this("Unknown", 0, "Unknown") { }
    
    public Person(string name) : this(name, 0, "Unknown") { }
    
    public Person(string name, int age, string city)
    {
        this.name = name;
        this.age = age;
        this.city = city;
    }
}
```

### 3. Zwracanie bieżącego obiektu (Fluent API)

```csharp
public class StringBuilder
{
    private string content = "";
    
    public StringBuilder Append(string text)
    {
        content += text;
        return this;  // Zwraca bieżący obiekt
    }
    
    public StringBuilder AppendLine()
    {
        content += Environment.NewLine;
        return this;
    }
}

// Użycie - fluent API
var sb = new StringBuilder()
    .Append("Hello")
    .Append(" ")
    .Append("World")
    .AppendLine();
```

### 4. Przekazywanie referencji do metody

```csharp
public class Person
{
    public void Introduce(Action<Person> action)
    {
        action(this);  // Przekazuje bieżący obiekt
    }
}

var person = new Person("Jan");
person.Introduce(p => Console.WriteLine($"Osoba: {p}"));
```

## Podsumowanie

- `this` wskazuje na bieżący obiekt
- Rozróżnia pola od parametrów
- Umożliwia łańcuchowanie konstruktorów
- Zwraca bieżący obiekt dla fluent API

---

## 📖 Referencje

[Microsoft Docs - this](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/this)

