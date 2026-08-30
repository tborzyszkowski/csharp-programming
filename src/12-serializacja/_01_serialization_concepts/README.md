# Temat 1: Serialization Concepts - Fundamenty

## 📚 Definicja Serializacji

**Serializacja** = proces konwersji **obiektu** (w pamięci RAM) na **sekwencję bajtów** (stream).

**Deserializacja** = proces odwrotny - odtworzenie obiektu z sekwencji bajtów.

```csharp
// Obiekt w pamięci (RAM)
var person = new Person { Name = "Alice", Age = 30 };

// Serializacja (RAM → Bytes)
byte[] data = Serialize(person);

// Przechowywanie / Transmisja
File.WriteAllBytes("person.dat", data);
Network.Send(data);

// Deserializacja (Bytes → RAM)
var restored = Deserialize<Person>(data);
```

---

## 🌳 Pojęcie Object Graph

### Co to jest Object Graph?

**Graf obiektów** = struktura wszystkich obiektów i ich referencji w pamięci.

**Przykład struktury:**

```
┌─────────────────────────┐
│      Person             │
│  Name: "Alice"          │
│  Age: 30                │
│  Address: ────────┐     │
└─────────────────┼─────┘
                  │
        ┌─────────▼────────────┐
        │    Address           │
        │  Street: "Main St"   │
        │  City: "NYC"         │
        │  Country: ──────┐    │
        └─────────────────┼────┘
                          │
                  ┌───────▼──────────┐
                  │  Country         │
                  │  Code: "US"      │
                  │  Name: "USA"     │
                  └──────────────────┘
```

### Algorytm Serializacji: Depth-First Search (DFS)

**Przechodzenie obiektu (grafu) krok po kroku:**

```
1. Zacznij od obiektu głównego (Person)
2. Przejdź głębiej (Person.Address)
3. Przejdź głębiej (Address.Country)
4. Osiągnięty koniec - cofnij się
5. Przejdź do następnych pól
6. Powtórz aż do końca grafu
```

**Kod konceptualny:**

```csharp
void SerializeDFS(object obj, Stream stream)
{
    if (obj == null) return;
    
    // Odwiedź bieżący obiekt
    WriteObject(stream, obj);
    
    // Przejdź do każdego pola referencyjnego
    foreach (var field in obj.GetType().GetFields())
    {
        if (IsReferenceType(field.FieldType))
        {
            var value = field.GetValue(obj);
            SerializeDFS(value, stream);  // Rekursywnie
        }
    }
}
```

---

## 🤔 Dlaczego Serializacja?

### 1. **Persistence** - Zapis stanu na dysk

```csharp
// Zapis stanu aplikacji
var game = new GameState { Score = 1000, Level = 5 };
serializer.Save("game.save", game);

// Wznowienie gry
var restored = serializer.Load("game.save");
```

### 2. **Network Communication** - Przesyłanie przez sieć

```csharp
// Klient wysyła żądanie do serwera
var request = new ApiRequest { UserId = 123 };
byte[] data = JsonSerializer.SerializeToUtf8Bytes(request);
await httpClient.PostAsync("/api/data", new ByteArrayContent(data));
```

### 3. **Caching** - Szybki dostęp do danych

```csharp
// Serializuj skomplikowany obiekt do cache'u
var complexData = LoadComplexData();
cache.Set("data", Serialize(complexData));

// Szybkie pobranie
var cached = Deserialize(cache.Get("data"));
```

### 4. **Integration** - Współpraca systemów

```
System A (C#)
    ↓ serialize to JSON
{ "name": "Alice", "age": 30 }
    ↓
System B (Python/Java/JavaScript)
    ↓ deserialize from JSON
    ↓ process data
```

### 5. **Backup & Recovery** - Odtworzenie stanu

```csharp
// Backup
var state = GetCurrentState();
backupStorage.Save(Serialize(state));

// Recovery
var restored = Deserialize(backupStorage.Restore());
```

---

## 📊 Typy Danych: Co Serializować?

### Proste Typy (Primitives)
```csharp
int, double, string, bool, DateTime
// Serializacja: bezpośrednia konwersja na bajty
```

### Złożone Typy (Complex)
```csharp
public class Person
{
    public string Name { get; set; }        // Primitive
    public int Age { get; set; }            // Primitive
    public Address Address { get; set; }    // Complex (reference)
}
```

### Kolekcje
```csharp
public class Team
{
    public List<Person> Members { get; set; }  // Collection
    public Dictionary<string, int> Scores { get; set; }
}
```

### Grafy Cykliczne ⚠️
```csharp
public class Node
{
    public string Value { get; set; }
    public Node? Next { get; set; }    // Może wskazywać na self!
    public Node? Previous { get; set; } // Może tworzyć cykl!
}

// Exemple cyklu:
// Node A → Node B → Node C → Node A (CYKL!)
```

---

## 🔄 Proces Serializacji Krok po Kroku

### Faza 1: Przygotowanie
```csharp
var person = new Person 
{ 
    Name = "Alice",
    Address = new Address { City = "NYC" }
};
```

### Faza 2: Przechodzenie Grafu (DFS)
```
1. Odwiedź Person
   ├─ Odwiedź Name ("Alice")
   ├─ Odwiedź Age (30)
   └─ Odwiedź Address
       ├─ Odwiedź Street ("Main St")
       ├─ Odwiedź City ("NYC")
       └─ Odwiedź Country
           └─ ...
```

### Faza 3: Kodowanie
```
Format: [Type][Field1][Field2][...][References]
Output: 01 41 6C 69 63 65 1E 04 4E 59 43 ...
        (Person)(A)(l)(i)(c)(e)(30)(NYC)
```

### Faza 4: Zapis
```csharp
// Do pliku
File.WriteAllBytes("person.dat", encodedData);

// Do sieci
networkStream.Write(encodedData, 0, encodedData.Length);

// Do pamięci
memoryStream.Write(encodedData, 0, encodedData.Length);
```

---

## 💾 Formaty Serializacji

### Porównanie

| Aspekt | XML | Binary | JSON | Protocol Buffers |
|--------|-----|--------|------|------------------|
| **Format** | Text | Binary | Text | Binary |
| **Human Readable** | ✅ Yes | ❌ No | ✅ Yes | ❌ No |
| **Rozmiar** | ⚠️ Large | ✅ Small | 📊 Medium | ✅ Very Small |
| **Szybkość** | ❌ Slow | ✅ Fast | 📊 Medium | ✅ Very Fast |
| **Wersjonowanie** | ⚠️ Difficult | ⚠️ Difficult | ✅ Easy | ✅ Very Easy |
| **Typizacja** | ❌ No | ✅ Yes | ⚠️ Loose | ✅ Strong |

---

## ⚙️ Jak Serializer Pracuje?

### Reflection-Based (Tradycyjny)
```csharp
// Serializator musi reflektować typ w runtime
public class ReflectionSerializer
{
    public byte[] Serialize<T>(T obj)
    {
        var type = obj.GetType();
        var fields = type.GetFields();  // Reflection!
        
        foreach (var field in fields)
        {
            var value = field.GetValue(obj);
            WriteValue(value);
        }
    }
}
```

**Zalety:** Elastyczność, obsługa dowolnych typów  
**Wady:** Wolne, duże overhead reflection'u

### Source Generator-Based (Nowoczesne)
```csharp
// Kod generowany w compile-time (C# 9+)
public class GeneratedSerializer
{
    // Kompilator generuje to automatycznie
    public byte[] Serialize(Person person)
    {
        // Brak reflection, czysty kod
        Write(person.Name);
        Write(person.Age);
    }
}
```

**Zalety:** Szybkie, brak reflection, typesafe  
**Wady:** Ograniczony do znanych typów

---

## 🔗 Obiekt vs Referencja

### Problem: Duplikacja

```csharp
public class Company
{
    public List<Employee> Employees { get; set; }
}

var alice = new Employee { Name = "Alice" };
var company = new Company 
{ 
    Employees = new() { alice, alice, alice }  // Ta sama osoba 3x
};

// Naiwna serializacja: duplikuje "Alice" 3 razy ❌
// Inteligentna serializacja: zapisuje referencję ✅
```

### Rozwiązanie: Referencyjne Kodowanie

```
[Company]
[Employees List (3 items)]
  [Employee: Alice] ← Zapisz pełnie
  [Reference: ^1] ← Wskaż na poprzednią
  [Reference: ^2] ← Wskaż na poprzednią
```

---

## ⚠️ Challenges

### 1. Cykliczne Referencje
```csharp
public class Node
{
    public Node? Next { get; set; }
}

var a = new Node();
var b = new Node();
a.Next = b;
b.Next = a;  // CYKL!

// Serializator musi śledzić odwiedzone obiekty
```

### 2. Polimorfizm
```csharp
public class Person { }
public class Employee : Person { }

var people = new List<Person> 
{ 
    new Person(),
    new Employee()  // Jaki typ zapisać?
};

// Serializator musi zapisać info o typie
```

### 3. Prywatne Pola
```csharp
public class BankAccount
{
    private decimal balance;  // Czy serializować?
}

// Zależy od strategii i security
```

### 4. Cykliczne Zależności
```csharp
public class Company
{
    public List<Employee> Employees { get; set; }
}

public class Employee
{
    public Company Employer { get; set; }  // Wskazuje z powrotem!
}

// Risk: nieskończona pętla
```

---

## 🎯 Strategie

### Whitelist Strategy
```csharp
[Serializable]
public class Person
{
    [SerializableAttribute]
    public string Name { get; set; }
    
    [NonSerializedAttribute]  // ← Pomiń to pole
    public string Password { get; set; }
}
```

### Cycle Detection
```csharp
public class SmartSerializer
{
    private HashSet<object> visited = new();
    
    private void Serialize(object obj)
    {
        if (visited.Contains(obj))
            return;  // Już odwiedziłem - pomiń
        
        visited.Add(obj);
        // Dalej serializuj...
    }
}
```

---

## 📚 Referencje

- [Microsoft: Serialization in .NET](https://learn.microsoft.com/en-us/dotnet/standard/serialization/)
- [Object Graphs Explained](https://en.wikipedia.org/wiki/Object_graph)
- [Depth-First Search Algorithm](https://en.wikipedia.org/wiki/Depth-first_search)
- [Serialization Best Practices](https://learn.microsoft.com/en-us/dotnet/standard/serialization/serialization-guidelines)

---

## ✅ Summary

**Serializacja** to niezbędna technika konwersji obiektów na bajty.

**Kluczowe koncepty:**
- Object graph = struktura obiektu z wszystkimi referencjami
- DFS/BFS = algorytm przechodzenia grafu
- Cycle detection = obsługa referencji zwrotnych
- Format selection = XML vs Binary vs JSON
- Trade-offs = readable vs compact vs fast

**Następny temat:** Historia serializacji w .NET
