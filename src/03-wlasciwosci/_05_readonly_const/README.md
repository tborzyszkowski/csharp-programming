# Pola Readonly vs Stałe (Const)

## 🎯 Cel rozdziału

Zrozumienie różnicy między `readonly` i `const` - kiedy używać każdej z nich.

## 📚 Spis treści

1. [Const - Stałe Kompilacji](#const---stałe-kompilacji)
2. [Readonly - Stałe Runtime](#readonly---stałe-runtime)
3. [Porównanie](#porównanie)
4. [Kiedy Użyć Każdej](#kiedy-użyć-każdej)

---

## Const - Stałe Kompilacji

**Const** jest stałą **czasu kompilacji** - wartość musi być znana w momencie kompilacji:

```csharp
public class MathConstants
{
    public const double PI = 3.14159265359;
    public const int DAYS_IN_WEEK = 7;
    public const string APP_VERSION = "1.0.0";
}

// Wartość jest wkompilowana w kod
Console.WriteLine(MathConstants.PI);  // 3.14159265359
```

**Właściwości Const:**
- Tylko wartości prymitywne (int, string, double, bool, etc.)
- Statyczne implicitnie
- Wartość musi być **znana w kodzie źródłowym**
- Wkompilowana w IL code (inlined)
- Nie można tworzyć referencji (brak adresu)

---

## Readonly - Stałe Runtime

**Readonly** jest stałą **czasu wykonania** - wartość może być ustawiona w konstruktorze:

```csharp
public class Configuration
{
    public readonly string ConfigPath;
    public readonly int MaxRetries;
    
    public Configuration(string path, int retries)
    {
        ConfigPath = path;  // Można ustawić w konstruktorze
        MaxRetries = retries;
    }
}

var config = new Configuration("C:\\app.json", 3);
// config.ConfigPath = "D:\\other.json";  // BŁĄD - readonly
```

**Właściwości Readonly:**
- Dowolny typ (klasy, struktury, obiekty)
- Może być instancja lub statyczne
- Ustawianie tylko w konstruktorze (lub inicjalizatorze)
- Nie można modyfikować po konstrukcji

---

## Porównanie

| Cecha | Const | Readonly |
|-------|-------|----------|
| Czas Ustalenia | Kompilacja | Runtime |
| Typy | Prymitywy | Dowolne |
| Statyczne | Zawsze | Opcjonalne |
| Inicjalizacja | Kod | Konstruktor |
| Modyfikacja | Niemożliwa | Niemożliwa (po ctor) |
| Wydajność | Inlined | Dereference |
| Serializacja | N/A | Tak |

---

## Kiedy Użyć Każdej

### Użyj CONST gdy:
- Wartość jest **znana wcześnie** (PI, DAYS_IN_WEEK)
- Prymitywny typ
- Nigdy się nie zmienia

### Użyj READONLY gdy:
- Wartość z **konfiguracji** (plik, baza danych)
- Obiekty (listy, słowniki, obiekty biznesowe)
- Trzeba ustawić w konstruktorze

---

## Best Practices

✅ **Const** dla stałych matematycznych i tekstowych

✅ **Readonly** dla konfiguracji i obiektów

✅ **Static readonly** dla singleton'ów (zamiast konstruktora)

✅ **Dokumentuj** dlaczego wartość jest const/readonly

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
