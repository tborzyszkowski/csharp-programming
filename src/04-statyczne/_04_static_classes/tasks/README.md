# Zadania: Klasy Statyczne

## Zadanie 1: FileHelper

Stwórz statyczną klasę `FileHelper` z metodami:
- `GetFileName(path)` - zwraca nazwę pliku
- `GetExtension(path)` - zwraca rozszerzenie
- `CombinePath(folder, file)` - łączy ścieżkę

## Zadanie 2: DateHelper

Stwórz statyczną klasę `DateHelper` z metodami:
- `GetAge(birthDate)` - oblicza wiek
- `IsLeapYear(year)` - sprawdza rok przestępny
- `DaysBetween(d1, d2)` - dni między datami

---

## Rozwiązania

### Zadanie 1
```csharp
public static class FileHelper
{
    public static string GetFileName(string path) => Path.GetFileName(path);
    public static string GetExtension(string path) => Path.GetExtension(path);
    public static string CombinePath(string folder, string file) => Path.Combine(folder, file);
}
```
