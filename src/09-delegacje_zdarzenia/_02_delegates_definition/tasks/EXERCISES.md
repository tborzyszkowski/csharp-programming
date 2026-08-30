# Ćwiczenia - Delegacje: Definicja i Składnia

## 🟢 BASIC LEVEL

### Ćw 2.1: Deklaracja i Instancjowanie
Zdefiniuj delegację `StringOperation` (string → int) i stwórz 3 implementacje:
- Zwraca długość stringa
- Zwraca liczbę spacji
- Zwraca liczbę wielkich liter

Test: `"Hello World"` powinno dać 11, 1, 2

### Ćw 2.2: Anonimowa i Lambda
Porównaj te same funkcje z Ćw 2.1 ale:
- Jedna jako metoda nazwana
- Druga jako anonymous method
- Trzecia jako lambda

### Ćw 2.3: Multicast - Logger
```csharp
delegate void Logger(string level, string msg);
```
Stwórz 3 loggery i łącz je z `+=`

## 🟡 INTERMEDIATE LEVEL

### Ćw 2.4: Closure Challenge
Utwórz listę delegacji w pętli gdzie każda "pamięta" swoją wartość:
```csharp
var operations = new List<Func<int, int>>();
for (int multiplier = 1; multiplier <= 5; multiplier++)
{
    operations.Add(x => x * multiplier);  // Closure!
}
// operations[0](10) = 10
// operations[1](10) = 20
// operations[2](10) = 30
```

### Ćw 2.5: Chain Pattern
Stwórz chain przetwarzania tekstu gdzie każdy procesor modify wynik:
```csharp
delegate string TextProcessor(string text);
TextProcessor pipeline = null!;
pipeline += Trim;
pipeline += ToLower;
pipeline += AddPrefix;
```

## 🔴 ADVANCED LEVEL

### Ćw 2.6: Generic Delegate Array
```csharp
public delegate T Converter<T>(T input);

// Utwórz array converters dla int, string, double
// Każdy ma różne operacje
```

### Ćw 2.7: Safe Multicast Handler
Zabezpiecz multicast delegatę przed:
- NullReferenceException
- Wyjątkami z poszczególnych handlerów
