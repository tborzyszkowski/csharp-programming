# Struktury – Słowo Kluczowe struct

## 🎯 Cel

Zrozumienie różnic między klasami a strukturami oraz świadomy wybór między nimi.

---

## 🚀 Uruchomienie

```bash
cd code/
dotnet run
dotnet test
```

## Klasa vs Struktura

| Aspekt | Klasa | Struktura |
|--------|-------|----------|
| Rodzaj typu | Referencyjny (reference type) | Wartościowy (value type) |
| Zmienna zawiera | Referencję do obiektu na stercie | Samą wartość (pola) |
| Przypisanie `b = a` | Kopiuje referencję (jeden obiekt) | Kopiuje całą wartość (dwa niezależne egzemplarze) |
| Gdzie leży pamięć | Obiekt na stercie (heap), sprząta go GC | Tam, gdzie zadeklarowano zmienną: stos, pole obiektu lub element tablicy |
| Konstruktor bezparametrowy | Kompilator dodaje go, dopóki nie zdefiniujesz innego konstruktora | Zawsze istnieje „domyślna” wartość (`default` – wszystkie pola wyzerowane); od C# 10 możesz zdefiniować własny |
| Dziedziczenie | Może dziedziczyć po innej klasie i być dziedziczona | Nie dziedziczy po innych strukturach/klasach (pośrednio po `System.ValueType`) i nie można z niej dziedziczyć; **może implementować interfejsy** |
| `null` | Zmienna może mieć wartość `null` | Nie, chyba że `Nullable<T>` (np. `int?`, `Point?`) |
| Domyślna równość (`Equals`) | Referencje (ten sam obiekt?) | Wartości pól |
| Finalizator (`~T`) | Tak | Nie |

> **Mit:** „Struktury są szybsze, bo leżą na stosie”. Struktura zadeklarowana jako pole klasy leży na stercie razem z nią,
> a duża struktura jest **kosztowna w kopiowaniu** przy każdym przypisaniu i przekazaniu do metody. Rzutowanie struktury
> na `object` lub interfejs powoduje *boxing* – alokację na stercie. O wyborze decyduje semantyka, nie mikro-optymalizacja.

## Przykład

```csharp
// KLASA - Reference type
public class PointClass
{
    public int X { get; set; }
    public int Y { get; set; }
}

// STRUKTURA - Value type
public struct PointStruct
{
    public int X { get; set; }
    public int Y { get; set; }
}

var c1 = new PointClass { X = 1, Y = 2 };
var c2 = c1;          // ten sam obiekt
c2.X = 100;
Console.WriteLine(c1.X);   // 100

var s1 = new PointStruct { X = 1, Y = 2 };
var s2 = s1;          // kopia
s2.X = 100;
Console.WriteLine(s1.X);   // 1
```

## Kiedy używać `struct`, a kiedy `class`

Zalecenia Microsoft (Framework Design Guidelines): rozważ `struct`, jeśli typ **jednocześnie**:

✅ logicznie reprezentuje **pojedynczą wartość** (punkt, kolor, kwota, data)  
✅ jest **mały** (zwykle ≤ 16 bajtów)  
✅ jest **niezmienny** (immutable)  
✅ nie będzie często rzutowany na `object` (boxing)  

W pozostałych przypadkach (logika biznesowa, duży stan, dziedziczenie, tożsamość obiektu) używaj `class`.

### Struktury niezmienne – zalecany styl

```csharp
// readonly struct: kompilator gwarantuje brak mutacji (i unika zbędnych kopii obronnych)
public readonly struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency) { Amount = amount; Currency = currency; }

    public Money WithAmount(decimal amount) => new(amount, Currency);   // zwracamy NOWĄ wartość
}

// record struct: równość wartościowa, ToString i wyrażenie `with` za darmo
public readonly record struct Vector2D(double X, double Y);

var v = new Vector2D(1, 2);
var w = v with { X = 5 };            // kopia ze zmianą X
Console.WriteLine(v == new Vector2D(1, 2));   // true
```

### Pułapka: zmienne struktury

```csharp
public class Circle
{
    public Point Center { get; set; }   // Point to zmienna struktura
}

var circle = new Circle();
// circle.Center.X = 10;   // BŁĄD KOMPILACJI CS1612: getter zwraca KOPIĘ, zmiana byłaby zgubiona
circle.Center = new Point(10, 0);       // poprawnie: podmieniamy całą wartość
```

Dlatego zmienne struktury (`{ get; set; }`) w `Program.cs` służą wyłącznie celom demonstracyjnym;
w realnym kodzie wybieraj `readonly struct` / `readonly record struct`.

### Przekazywanie do metod

```csharp
static void Move(Point p)       => p.X += 100;   // dostaje KOPIĘ - wołający nie zobaczy zmiany
static void Move(ref Point p)   => p.X += 100;   // pracuje na oryginale
static void Print(in Point p)   => Console.WriteLine(p);   // przekazanie bez kopiowania, tylko do odczytu
```

### `default` i niezainicjowane struktury

Każda struktura ma wartość domyślną (`default(Point)` = pola wyzerowane) i tablica `new Point[10]` zawiera 10
takich wartości bez wywoływania konstruktora. Dlatego **nie da się zagwarantować niezmienników** (np. „waluta nie jest
pusta”) w strukturze – obiekt `default(Money)` ma `Currency == null`. To kolejny powód, by struktur używać do prostych danych.

---

## 📖 Referencje

- [Microsoft Docs - Structs](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct)
- [Choosing Between Class and Struct](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/choosing-between-class-and-struct)

