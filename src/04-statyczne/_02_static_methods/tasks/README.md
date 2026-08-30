# Zadania: Metody Statyczne

## Zadanie 1: Calc Helper

Stwórz klasę `CalcHelper` z metodami statycznymi do obliczeń:
- `Add(a, b)` - dodawanie
- `Subtract(a, b)` - odejmowanie
- `Multiply(a, b)` - mnożenie
- `Divide(a, b)` - dzielenie (zwraca 0 jeśli b == 0)

## Zadanie 2: Validation

Stwórz klasę `Validator` z metodami do walidacji:
- `IsValidAge(int age)` - zwraca true jeśli 0-150
- `IsValidName(string name)` - zwraca true jeśli długość > 2
- `IsValidPassword(string pwd)` - zwraca true jeśli długość > 8

## Zadanie 3: Logger Service

Stwórz klasę `LoggerService` z metodami:
- `Info(message)` - loguje info
- `Warning(message)` - loguje warning
- `Error(message)` - loguje error
- `GetLogCount()` - zwraca ilość wpisów

Wskazówka: Użyj statycznego pola do przechowywania logów.

---

## Rozwiązania

### Zadanie 1
```csharp
public static class CalcHelper
{
    public static double Add(double a, double b) => a + b;
    public static double Subtract(double a, double b) => a - b;
    public static double Multiply(double a, double b) => a * b;
    public static double Divide(double a, double b) => b != 0 ? a / b : 0;
}
```

### Zadanie 2
```csharp
public static class Validator
{
    public static bool IsValidAge(int age) => age >= 0 && age <= 150;
    public static bool IsValidName(string name) => !string.IsNullOrEmpty(name) && name.Length > 2;
    public static bool IsValidPassword(string pwd) => !string.IsNullOrEmpty(pwd) && pwd.Length > 8;
}
```

### Zadanie 3
```csharp
public static class LoggerService
{
    private static List<string> logs = new();
    
    public static void Info(string message) => logs.Add($"[INFO] {message}");
    public static void Warning(string message) => logs.Add($"[WARN] {message}");
    public static void Error(string message) => logs.Add($"[ERR] {message}");
    public static int GetLogCount() => logs.Count;
}
```
