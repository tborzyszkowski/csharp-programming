# Destruktory a kod zarządzany i niezarządzany

## 🎯 Cel rozdziału

Zrozumienie różnicy między pamięcią zarządzaną przez .NET a zasobami systemowymi, poznanie ograniczeń garbage collectora oraz sposobów deterministycznego zwalniania zasobów przez `IDisposable` i `using`.

## 📚 Spis treści

1. [Zarządzane vs niezarządzane zasoby](#zarządzane-vs-niezarządzane)
2. [Garbage Collection](#garbage-collection)
3. [IDisposable i Dispose](#idisposable-i-dispose)
4. [Using Statement](#using-statement)

---

## Zarządzane vs niezarządzane

### Pamięć zarządzana

Obiekty utworzone przez środowisko .NET (np. `string`, `List<T>` i własne klasy) znajdują się na stercie zarządzanej. Garbage collector (GC) odzyskuje pamięć obiektów, do których program nie ma już osiągalnych odwołań. Zwykle nie trzeba zwalniać tej pamięci ręcznie.

To nie znaczy, że GC zwalnia każdy zasób używany przez obiekt natychmiast. Obiekt .NET może być tylko **opakowaniem** zasobu systemowego: na przykład `FileStream` jest obiektem zarządzanym, który korzysta z uchwytu pliku udostępnianego przez system operacyjny.

### Zasoby niezarządzane

Zasób niezarządzany jest przydzielany poza zwykłą stertą .NET. Przykłady to uchwyty plików i gniazd, uchwyty systemu operacyjnego, pamięć zaalokowana przez kod natywny oraz zasoby pozyskane przez P/Invoke. Takie zasoby mają ograniczoną liczbę i mogą pozostać zajęte, nawet gdy obiekt .NET przestaje być używany.

Najczęściej aplikacja nie pracuje bezpośrednio z surowym zasobem niezarządzanym. Używa zarządzanej klasy, takiej jak `FileStream`, `StreamReader` czy klient bazy danych, która ten zasób opakowuje i udostępnia metodę `Dispose`.

```csharp
using var stream = File.OpenRead("file.txt");
// Odczyt z pliku...
// Przy wyjściu z zakresu stream.Dispose() zamknie strumień i jego uchwyt.
```

Samo porzucenie zmiennej nie jest równoznaczne z natychmiastowym zamknięciem pliku. W tym przykładzie `FileStream` jest zarządzanym obiektem, ale jego uchwyt pliku wymaga jawnego zwolnienia.

---

## Garbage Collection

GC śledzi osiągalność obiektów, a nie to, czy ich zasoby systemowe powinny już zostać zwolnione. Gdy obiekt przestaje być osiągalny, GC może odzyskać zajmowaną przez niego pamięć przy kolejnym przebiegu. Nie gwarantuje jednak, **kiedy** to nastąpi.

Wymuszenie `GC.Collect()` nie jest właściwym sposobem zamykania pliku ani innego zasobu: nie zapewnia terminowego zwolnienia, może pogorszyć wydajność i nie zastępuje `Dispose`.

Finalizer (składnia `~NazwaKlasy()`) jest wywoływany przez GC w nieokreślonym momencie, o ile obiekt z finalizerem nie został wcześniej jawnie zwolniony. Nie powinien być podstawowym mechanizmem sprzątania: do jego uruchomienia może dojść dopiero po pewnym czasie, a kolejność finalizacji obiektów nie jest gwarantowana.

Finalizer ma sens głównie jako zabezpieczenie dla typu, który **bezpośrednio posiada** zasób niezarządzany. W typowym kodzie aplikacyjnym należy korzystać z `SafeHandle` lub istniejącej klasy implementującej `IDisposable`, zamiast samodzielnie pisać finalizer.

---

## IDisposable i Dispose

`IDisposable` udostępnia metodę `Dispose()`, dzięki której kod korzystający z obiektu może jawnie i deterministycznie zakończyć jego używanie. Zasada podziału odpowiedzialności jest następująca:

- kod, który **korzysta** z obiektu `IDisposable`, wywołuje `Dispose` (najprościej przez `using`);
- typ, który **posiada** inne obiekty `IDisposable`, zwalnia je w swoim `Dispose`;
- implementację `IDisposable` dodaje się wtedy, gdy typ jest właścicielem zasobu wymagającego jawnego zwolnienia, a nie tylko dlatego, że ma finalizer lub zarządza zwykłymi obiektami .NET.

Prosty wzorzec dla klasy będącej właścicielem innego zarządzanego zasobu wygląda tak:

```csharp
public sealed class LogReader : IDisposable
{
    private readonly StreamReader reader;

    public LogReader(string path) => reader = new StreamReader(path);

    public string? ReadLine() => reader.ReadLine();

    public void Dispose() => reader.Dispose();
}
```

`LogReader` nie potrzebuje własnego finalizera: uchwyt pliku jest zarządzany przez `StreamReader`, który sam odpowiada za jego zwolnienie. Jeśli typ implementujący `IDisposable` jest właścicielem kilku zasobów, powinien zwolnić wszystkie; należy też zdecydować, czy po `Dispose` dalsze użycie obiektu ma zgłaszać `ObjectDisposedException`.

### Wzorzec z zasobem niezarządzanym

Własny finalizer jest potrzebny tylko wtedy, gdy typ bezpośrednio posiada zasób niezarządzany i nie jest on już bezpiecznie opakowany przez `SafeHandle`. Klasyczny wzorzec bazowy rozróżnia sprzątanie jawne (`disposing: true`) od awaryjnej finalizacji (`disposing: false`):

```csharp
public class NativeOwner : IDisposable
{
    private bool disposed;

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
        {
            // Zwolnij posiadane obiekty zarządzane IDisposable.
        }

        // Zwolnij bezpośrednio posiadany zasób niezarządzany.
        disposed = true;
    }

    ~NativeOwner() => Dispose(disposing: false);
}
```

Przy wywołaniu przez kod `Dispose(true)` zwalnia się zarówno własne zasoby niezarządzane, jak i posiadane obiekty zarządzane `IDisposable`. Finalizer wywołuje `Dispose(false)`, więc nie należy w tej ścieżce polegać na innych obiektach zarządzanych — ich stan lub kolejność finalizacji mogą być nieznane. `GC.SuppressFinalize` zapobiega późniejszemu uruchomieniu finalizera po poprawnym, jawnym `Dispose`.

Powyższy wzorzec jest ilustracją i wymaga uzupełnienia o rzeczywistą obsługę natywnego zasobu. Jeżeli można użyć `SafeHandle`, należy preferować go: przejmuje on odpowiedzialność za bezpieczne zwolnienie uchwytu i zwykle pozwala uniknąć własnego finalizera.

---

## Using Statement

`using` gwarantuje wywołanie `Dispose`, również wtedy, gdy kod w bloku zakończy się wyjątkiem.

### Blok using

```csharp
using (var reader = new StreamReader("file.txt"))
{
    Console.WriteLine(reader.ReadLine());
} // reader.Dispose() zostanie wywołane także przy wyjątku w bloku.
```

### Deklaracja using (C# 8.0+)

```csharp
using var reader = new StreamReader("file.txt");
Console.WriteLine(reader.ReadLine());
// reader.Dispose() przy wyjściu z bieżącego zakresu.
```

Deklaracja `using var` zwalnia zasób na końcu zakresu, a nie po pojedynczej instrukcji. Wybierz zakres, który odpowiada temu, jak długo zasób ma być potrzebny.

---

## Best Practices

✅ Używaj `using` dla każdego obiektu `IDisposable`, którego jesteś właścicielem.

✅ Implementuj `IDisposable`, gdy typ posiada zasób wymagający jawnego zwolnienia (w tym inne obiekty `IDisposable`).

✅ Preferuj istniejące typy zarządzające zasobami i `SafeHandle`; nie dodawaj finalizera bez potrzeby.

✅ Udostępniaj `Dispose` jako bezpieczne do wielokrotnego wywołania i po zwolnieniu chroń metody, które nie powinny już działać.

❌ Nie używaj finalizera ani `GC.Collect()` jako zwykłego sposobu zamykania plików lub połączeń.

---

## Co demonstruje przykład

`code/Program.cs` pokazuje kontrakt `IDisposable`, wywołanie `Dispose` przez obie formy `using` oraz odrzucenie operacji po zwolnieniu obiektu. `FileResource` jest celowo uproszczoną symulacją: zapisuje komunikaty o otwieraniu i zamykaniu zasobu, ale nie otwiera rzeczywistego pliku ani natywnego uchwytu. Przykłady z `FileStream` i `StreamReader` powyżej pokazują rzeczywisty przypadek użycia.

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
