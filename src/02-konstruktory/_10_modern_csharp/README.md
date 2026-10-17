# Nowoczesne C#: Records, Init Properties, Primary Constructors

## 🎯 Cel rozdziału

Zrozumienie wybranych cech C# 9 i nowszych, które ułatwiają modelowanie danych oraz inicjalizację obiektów. Najważniejsze jest nie tylko poznanie krótszej składni, ale też zrozumienie, jak wpływa ona na równość, mutowalność i API typu.

## 📚 Spis treści

1. [Records (C# 9+)](#records-c-9)
2. [Init-only Properties (C# 9+)](#init-only-properties-c-9)
3. [Primary Constructors (C# 12+)](#primary-constructors-c-12)
4. [Wybór typu i składni](#wybór-typu-i-składni)

---

## Records (C# 9+)

`record` to deklaracja typu, która dostarcza zachowanie przydatne dla modeli danych. Najczęściej używany `record class` jest typem **referencyjnym**. C# udostępnia też `record struct`, który jest typem **wartościowym**.

W przeciwieństwie do zwykłej klasy, record domyślnie porównuje dane, a nie to, czy dwie zmienne wskazują dokładnie ten sam obiekt. To nadal nie oznacza, że każdy record jest automatycznie niezmienny: można dodać do niego właściwości z `set`, a obiekty przechowywane we właściwościach mogą być mutowalne.

### Co otrzymujemy w record

- Równość wartościowa: porównywane są wartości składowych recordu; dla `record class` porównanie uwzględnia również zgodność typu recordu.
- Nadpisane `ToString()`, które wypisuje nazwę typu i jego publiczne właściwości.
- Dla recordów pozycyjnych: właściwości pozycyjne i metodę `Deconstruct`.
- Obsługę wyrażenia `with`, które tworzy nowy obiekt z wybranymi zmienionymi wartościami.

`GetHashCode()` jest zgodny z równością. Dlatego nie należy zmieniać danych wpływających na równość obiektu, gdy jest on kluczem w `Dictionary` lub elementem `HashSet`.

```csharp
// Record pozycyjny (C# 9+)
public record Person(string Name, int Age);

var first = new Person("Jan", 30);
var sameData = new Person("Jan", 30);

Console.WriteLine(first == sameData); // true: dane są równe
Console.WriteLine(ReferenceEquals(first, sameData)); // false: to różne obiekty
Console.WriteLine(first); // Person { Name = Jan, Age = 30 }

var (name, age) = first; // Deconstruct
```

W powyższym przykładzie parametry pozycyjne stają się właściwościami `init`-only. Można więc ustawić je podczas tworzenia obiektu, ale nie przypisać ponownie po zakończeniu inicjalizacji.

```csharp
public record Point(double X, double Y);

var point = new Point(3, 4);
Console.WriteLine(point.X); // 3
```

### `with` i płytka kopia

```csharp
var person1 = new Person("Jan", 30);
var person2 = person1 with { Age = 31 };

Console.WriteLine(person1); // Person { Name = Jan, Age = 30 }
Console.WriteLine(person2); // Person { Name = Jan, Age = 31 }
```

`with` nie zmienia istniejącego obiektu. Dla `record class` tworzy nową instancję, kopiuje wartości składowych i stosuje wskazane zmiany. Jest to **płytka kopia**: jeżeli właściwość zawiera referencję do mutowalnej listy lub innego obiektu, obie instancje mogą nadal wskazywać na ten sam obiekt wewnętrzny.

```csharp
public record Team(string Name, List<string> Members);

var team1 = new Team("A", new List<string> { "Ola" });
var team2 = team1 with { Name = "B" };
team2.Members.Add("Jan");
// team1.Members również zawiera "Jan": lista nie została skopiowana.
```

Używaj niemutowalnych składowych lub twórz ich kopie osobno, jeśli potrzebna jest głęboka izolacja.

---

## Init-only Properties (C# 9+)

Właściwość z akcesorem `init` można przypisać podczas tworzenia obiektu: w konstruktorze, inicjalizatorze obiektu lub inicjalizatorze `with`. Po zakończeniu inicjalizacji nie można przypisać jej nowej wartości zwykłym kodem.

```csharp
public class Person
{
    public required string Name { get; init; }
    public int Age { get; init; }
}

var person = new Person { Name = "Jan", Age = 30 };
// person.Name = "Anna"; // Błąd kompilacji: inicjalizacja jest zakończona.
```

`init` ogranicza ponowne przypisanie właściwości, ale samo w sobie nie czyni całego obiektu głęboko niezmiennym. Referencja do mutowalnego obiektu może nadal pozwalać na zmianę jego zawartości.

Modyfikator `required` wymaga, aby kod tworzący obiekt podał wartość wymaganej właściwości lub pola. Jest to kontrola kompilatora, a nie automatyczna walidacja danych w czasie działania; wartość może być na przykład `null`, jeśli typ i konfiguracja projektu na to pozwalają. Właściwość `required` musi mieć możliwość ustawienia w miejscu tworzenia, na przykład przez `init` lub `set`.

`init` jest przydatne dla konfiguracji i modeli, których stan ma być ustalony przy tworzeniu. Jeśli stan powinien zmieniać się przez cały czas życia obiektu, użyj `set` albo jawnych metod zmieniających stan.

---

## Primary Constructors (C# 12+)

Primary constructor (konstruktor główny) pozwala umieścić parametry konstruktora w nagłówku deklaracji klasy lub struktury. Skraca przekazywanie zależności i danych wejściowych, ale zachowuje się inaczej w klasie niż pozycyjny record.

```csharp
// Zwykły konstruktor
public class Person
{
    public string Name { get; }

    public Person(string name)
    {
        Name = name;
    }
}

// C# 12+: parametry konstruktora głównego
public class Person(string name)
{
    public string Name { get; } = name;
}
```

Ważne: dla zwykłej klasy parametry `name` nie stają się automatycznie publicznymi właściwościami. Są dostępne w treści typu; jeśli odwołuje się do nich kod instancji, kompilator przechowuje potrzebną wartość. Właściwość `Name` w przykładzie jest jawnie deklarowana.

Primary constructor nie jest tym samym co tradycyjny konstruktor z blokiem ciała. Można użyć parametrów w inicjalizatorach pól i właściwości oraz w metodach, ale nie można dodać własnego bloku instrukcji konstruktora głównego. Weryfikację lub dodatkową logikę można umieścić w inicjalizatorze albo konstruktorze z ciałem, który deleguje do konstruktora głównego.

### Przekazywanie zależności

```csharp
public interface IUserRepository
{
    string? FindName(int id);
}

public class UserService(IUserRepository repository)
{
    public string? GetName(int id) => repository.FindName(id);
}
```

Tutaj `repository` jest parametrem konstruktora, a nie publiczną właściwością. To dobre rozwiązanie dla zależności, które mają być używane przez implementację, ale nie muszą stanowić publicznej części API.

---

## Wybór typu i składni

| Potrzeba | Typ lub składnia | Uwagi |
|---|---|---|
| Encja z tożsamością i zmiennym stanem | `class` | Domyślna równość tożsamościowa; stan i równość można zdefiniować samodzielnie. |
| Model danych porównywany według zawartości | `record class` | Typ referencyjny z równością wartościową; nie gwarantuje głębokiej niezmienności. |
| Mały typ wartościowy porównywany według zawartości | `record struct` | Typ wartościowy; kopiuje się przy przypisaniu. Wybieraj go świadomie, uwzględniając semantykę wartości. |
| Ustawienie wartości przy tworzeniu zwykłej klasy | `init` | Blokuje ponowne przypisanie po inicjalizacji, nie unieruchamia obiektów wskazywanych przez właściwość. |
| Krótszy konstruktor klasy lub struktury | Primary constructor | Parametry nie stają się automatycznie publicznymi właściwościami. |

Zwykła klasa i record class są typami referencyjnymi; record nie zmienia obiektu w typ wartościowy. Z kolei `record struct` jest typem wartościowym i jego przypisanie kopiuje wartość.

---

## Best Practices

✅ Używaj recordów dla modeli danych, dla których porównanie według wartości jest pożądane (np. DTO lub value objects).

✅ Używaj `init` do ustalania stanu przy tworzeniu obiektu; rozważ `required`, gdy właściwość musi być jawnie podana.

✅ Używaj primary constructors, gdy parametry konstruktora są proste i skrót poprawia czytelność.

✅ Pamiętaj, że `with` wykonuje płytką kopię; zagnieżdżone obiekty nie są automatycznie klonowane.

❌ Nie zakładaj, że record jest automatycznie głęboko niezmienny albo że `with` odizoluje mutowalne składowe.

❌ Nie traktuj parametrów primary constructor zwykłej klasy jak automatycznie wygenerowanych właściwości.

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
