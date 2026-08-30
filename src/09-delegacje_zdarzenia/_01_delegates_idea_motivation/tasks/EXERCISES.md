# Ćwiczenia - Delegacje: Idea i Motywacja

## Poziomy Trudności
- 🟢 **Basic** - Grundowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Wzorce i architektura

---

## 🟢 BASIC LEVEL

### Ćwiczenie 1.1: Prosty Callback
**Cel:** Zrozumieć podstawowy mechanizm callback'a

Zdefiniuj delegację `StringAction` (przyjmuje string, nic nie zwraca) i funkcję `PrintMessage(string message, StringAction action)` która:
1. Wypisuje "Executing action..."
2. Wykonuje przekazaną akcję z wiadomością
3. Wypisuje "Done!"

Użyj jej z trzema różnymi implementacjami:
- Drukuj wiadomość normalnie
- Drukuj wiadomość wielkimi literami
- Drukuj wiadomość z przedrostkiem "LOG: "

**Podpowiedź:** Delegacja powinna wyglądać jak:
```csharp
public delegate void StringAction(string message);
```

**Oczekiwany output:**
```
Executing action...
Hello World
Done!

Executing action...
HELLO WORLD
Done!

Executing action...
LOG: Hello World
Done!
```

---

### Ćwiczenie 1.2: Late Binding - Kalkulator
**Cel:** Zrozumieć late binding - runtime decyzja co wykonać

Zdefiniuj delegację `MathOp` (przyjmuje dwie liczby, zwraca liczbę).

Utwórz funkcję `Calculate(int a, int b, MathOp operation, string opName)` która:
1. Wypisuje "Calculating..."
2. Wykonuje operację
3. Wypisuje wynik z nazwą operacji

Napisz program, który dla liczb 12 i 4 wykonuje:
- Dodawanie
- Mnożenie
- Dzielenie (ostrożnie!)
- Potęgowanie

Wynik:
```
Calculating Add...
Result: 16

Calculating Multiply...
Result: 48

Calculating Divide...
Result: 3

Calculating Power...
Result: 20736
```

---

### Ćwiczenie 1.3: Filtrowanie Listy
**Cel:** Zastosuj delegację do filtrowania

Zdefiniuj delegację `IntPredicate` (przyjmuje int, zwraca bool).

Utwórz funkcję `FilterNumbers(int[] numbers, IntPredicate predicate)` która zwraca nową tablicę z liczbami spełniającymi predykat.

Filtry (implementacje):
- Parzyste liczby
- Liczby większe niż 10
- Liczby podzielne przez 3

Dane testowe: `{ 5, 10, 15, 20, 25, 30, 35, 40 }`

Oczekiwane wyniki:
- Parzyste: `{ 10, 20, 30, 40 }`
- > 10: `{ 15, 20, 25, 30, 35, 40 }`
- Podzielne przez 3: `{ 15, 30 }`

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 2.1: Strategy Pattern - Sortowanie
**Cel:** Implementuj Strategy Pattern z delegacjami

Utwórz system sortowania liczb z różnymi strategiami:

```csharp
public delegate int SortStrategy(int a, int b);
```

Strategie:
1. **Rosnąco** - tradycyjne
2. **Malejąco** - odwrotnie
3. **Po wartości bezwzględnej** - |a| vs |b|
4. **Liczby parzyste najpierw** - parzyste przed nieparzystymi

Funkcja `SortWithStrategy(int[] numbers, SortStrategy strategy)` zwraca posortowaną tablicę.

Dane: `{ -5, 3, -2, 8, 1, -9, 4 }`

**Oczekiwane wyniki:**
```
Ascending: -9 -5 -2 1 3 4 8
Descending: 8 4 3 1 -2 -5 -9
By Absolute Value: 1 -2 3 4 -5 8 -9
Even First: 8 4 -2 -9 -5 3 1
```

---

### Ćwiczenie 2.2: Multicast Delegates - Event Logger
**Cel:** Pracuj z multicast delegates

Zdefiniuj:
```csharp
public delegate void LogAction(string message, DateTime timestamp);
```

Utwórz funkcję `LogEvent(string eventName, LogAction loggers)` która:
1. Tworzy timestamp
2. Wykonuje wszystkie zarejestrowane loggery

Loggery:
- Console Logger - wypisuje `[CONSOLE] timestamp: message`
- File Logger (symulacja) - wypisuje `[FILE] timestamp: message`
- Alert Logger - jeśli message zawiera "ERROR", wypisuje `[ALERT!!!] message`

Uruchom łańcuch (chain):
```csharp
LogAction loggers = null;
loggers += ConsoleLogger;
loggers += FileLogger;
loggers += AlertLogger;

LogEvent("System started", loggers);
LogEvent("ERROR: Connection failed", loggers);
```

---

### Ćwiczenie 2.3: Asynchroniczny Callback (Symulacja)
**Cel:** Symuluj async operacje z callback'ami

```csharp
public delegate void OperationCallback(string result, bool success);
```

Utwórz metodę `PerformAsyncOperation(string operationName, int delayMs, OperationCallback callback)` która:
1. Wypisuje "Starting: operationName..."
2. Czeka delayMs milisekund
3. Czasami się nie udaje (random 30% szansy)
4. Wywoła callback z wynikiem

Użyj z co najmniej 3 różnymi callback'ami:
- Success callback - wypisuje "✓ Operation succeeded"
- Error callback - wypisuje "✗ Operation failed"
- Mixed callback - obsługuje obie sytuacje

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 3.1: Pipeline Architecture z Delegacjami
**Cel:** Zbuduj pipeline przetwarzania danych

```csharp
public record Item(string Name, int Value);

public delegate Item DataProcessor(Item item);
```

Utwórz pipeline gdzie każdy krok to delegacja:
1. **Validation Filter** - sprawdza czy Value > 0
2. **Transformation** - mnożyć Value × 2
3. **Enrichment** - dodaj prefix do Name
4. **Logging** - loguj każdy krok

Przykład:
```csharp
Item item = new("Product", 50);

// Krok 1: Validate
item = validate(item);  // OK, Value > 0

// Krok 2: Transform
item = transform(item);  // Value becomes 100

// Krok 3: Enrich
item = enrich(item);     // Name becomes "[PROCESSED] Product"

// Krok 4: Log
log(item);               // Log final state
```

---

### Ćwiczenie 3.2: Observable Pattern (Simplified)
**Cel:** Implementuj uproszczony Observable pattern

Zbuduj system powiadomień gdzie:
- Wiele "obserwatorów" może się zarejestrowć na zmiany
- Zmiany liczby w rejestrze wyzwalają powiadomienia
- Każdy obserwator ma inną akcję

```csharp
public delegate void Observer(int oldValue, int newValue);

public class Observable
{
    private int value;
    private Observer? observers;
    
    public int Value
    {
        get => value;
        set
        {
            int oldValue = this.value;
            this.value = value;
            observers?.Invoke(oldValue, value);  // Notify all observers
        }
    }
    
    public void Subscribe(Observer observer) => observers += observer;
    public void Unsubscribe(Observer observer) => observers -= observer;
}
```

Obserwatory:
- Logger - loguje zmiany
- Validator - sprawdza czy zmiana jest logiczna (różnica < 50)
- Persister - "zapisuje" do pliku (symulacja)

---

### Ćwiczenie 3.3: Command Pattern z Delegacjami
**Cel:** Implementuj Command Pattern (undo/redo)

```csharp
public record Command(string Name, Action Execute, Action Undo);

public class CommandQueue
{
    private Stack<Command> executedCommands = new();
    private Stack<Command> undoneCommands = new();
    
    public void Execute(Command cmd) { ... }
    public void Undo() { ... }
    public void Redo() { ... }
    public void PrintHistory() { ... }
}
```

Komendy:
- **Add** - dodaj liczbę do listy
- **Remove** - usuń z listy
- **Clear** - wyczyść listę
- **Sort** - posortuj listę

Funkcjonalność:
```csharp
var queue = new CommandQueue();

// Wykonaj komendy
queue.Execute(new Command("Add 5", ...));
queue.Execute(new Command("Add 10", ...));
queue.Execute(new Command("Sort", ...));

// Undo
queue.Undo();  // Cofa Sort
queue.Undo();  // Cofa Add 10

// Redo
queue.Redo();  // Przywraca Add 10
```

---

## 💡 Wskazówki

### Basic Level
- Zacznij od prostych string delegacji
- Wypisuj każdy krok do konsoli dla debuggingu
- Testuj z przesadnymi danymi

### Intermediate Level
- Użyj LINQ (OrderBy, Where) gdzie potrzeba
- Testuj edge case'i (puste tablice, null, itp.)
- Obserwuj konsekwencje multicast

### Advanced Level
- Myśl o memory leaks przy unsubscribe
- Obserwuj kolejność wykonania
- Testuj race conditions w async scenariuszach

---

## 🎯 Kryteria Oceny

- ✅ Kod kompiluje się bez błędów
- ✅ Logika działa zgodnie z opisem
- ✅ Obsługuje edge case'i
- ✅ Kod jest czytelny i skomentowany
- ✅ Wyniki outputu zgadzają się z oczekiwaniami

---

## 📝 Przesyłanie Rozwiązań

Każde ćwiczenie powinno zawierać:
1. Kod źródłowy
2. Wynik uruchomienia (output)
3. Krótkie wyjaśnienie podejścia (w komentarzach)
4. (Optional) Refleksję na temat alternatywnych rozwiązań
