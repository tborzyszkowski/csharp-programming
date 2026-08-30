# Zadania - IEnumerable i IEnumerator

## 📚 Poziom: Podstawowy

### Zadanie 1: Custom List Iterator

Implementuj klaseę `CustomList<T>` z `GetEnumerator()` używając `yield return`.

### Zadanie 2: Infinite Sequence

Stwórz `CountingSequence` które zwraca liczby od 1 do nieskończoności (z limitem dla testów).

### Zadanie 3: Filter Iterator

Implementuj `FilteredEnumerable<T>` która filtruje elementy na podstawie predykatu.

---

## 📚 Poziom: Średniozaawansowany

### Zadanie 4: Lazy Evaluation Pipeline

Stwórz pipeline przetwarzający dane leniwie:

```csharp
public class TransformPipeline<T, TResult> : IEnumerable<TResult>
{
    public IEnumerator<TResult> GetEnumerator()
    {
        // Implementacja
    }
}
```

### Zadanie 5: Chunked Iterator

Implementuj iterator zwracający elementy w grupach (chunks):

```csharp
public class ChunkedEnumerable<T> : IEnumerable<T[]>
{
    // GetEnumerator zwraca T[] - tablice o określonym rozmiarze
}
```

---

## 📚 Poziom: Zaawansowany

### Zadanie 6: Parallel Iterator (Poznawcze)

Zaimplementuj `ParallelEnumerable<T>` która pozwala na równoległy dostęp do elementów.

---

**Następnie:** [4. Interfejsy Porównania]../_04_comparison_interfaces/
