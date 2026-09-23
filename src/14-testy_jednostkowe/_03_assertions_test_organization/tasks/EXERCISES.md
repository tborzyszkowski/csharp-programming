# Ćwiczenia - Asercje, Organizacja i Parametryzacja Testów

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 3.1: Wybór Właściwej Asercji
**Cel:** Dopasować asercję do sytuacji.

Dla poniższych sprawdzeń wskaż najbardziej opisową asercję xUnit (`Assert.Equal`, `Assert.Empty`, `Assert.Contains`, `Assert.Throws`, `Assert.Null` itd.):
1. Lista `orders` powinna być pusta po anulowaniu wszystkich zamówień
2. Lista `products` powinna zawierać produkt o nazwie `"Laptop"`
3. Wywołanie `repository.Find(-1)` powinno rzucić `ArgumentException`
4. Zmienna `user` powinna być `null` po usunięciu konta

---

### Ćwiczenie 3.2: Konwersja Testów na `[Theory]`
**Cel:** Wyeliminować powielanie kodu testowego.

Masz 4 osobne testy `[Fact]` sprawdzające `PriceCategory.Classify(decimal price)` (zwraca `"Tani"`, `"Średni"`, `"Drogi"`). Przekształć je w jeden test `[Theory]` z `[InlineData]`.

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 3.3: `MemberData` dla Złożonych Obiektów
**Cel:** Przetestować metodę przyjmującą obiekt, korzystając z `MemberData`.

Zaimplementuj `ShippingCalculator.CalculateCost(Order order)`, gdzie `Order` ma właściwości `Weight` (kg) i `Distance` (km). Reguła: koszt = `Weight * 2 + Distance * 0.5`.

Napisz statyczną właściwość `OrderScenarios` zwracającą co najmniej 3 różne kombinacje `Order` + oczekiwany koszt, i użyj jej w `[MemberData]`.

---

### Ćwiczenie 3.4: Testowanie Kolekcji
**Cel:** Wykorzystać asercje kolekcji.

Zaimplementuj `UniqueNumberFilter.RemoveDuplicates(int[] numbers)`. Napisz testy sprawdzające za pomocą `Assert.Equal` (porównanie całych kolekcji), `Assert.Contains` i `Assert.DoesNotContain`, że:
- Duplikaty są usuwane
- Kolejność pierwszego wystąpienia jest zachowana
- Pusta tablica wejściowa daje pustą tablicę wyjściową (`Assert.Empty`)

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 3.5: Klasa Testowa dla Wielu Powiązanych Metod
**Cel:** Zaprojektować dobrze zorganizowaną klasę testową.

Zaimplementuj klasę `ShoppingCart` z metodami `AddItem`, `RemoveItem`, `GetTotal`, `Clear`. Napisz `ShoppingCartTests` z osobną grupą testów (wyraźnie skomentowaną) dla każdej metody, stosując konsekwentnie:
- Konwencję `Metoda_Scenariusz_Wynik`
- `[Theory]` tam, gdzie ma to sens (np. `GetTotal` dla różnych zestawów produktów)
- Przynajmniej jeden test `Assert.Throws` (np. `RemoveItem` nieistniejącego produktu)

**Podpowiedź:** Dobra organizacja testów ułatwia nawigację w dużych plikach – rozważ regiony/komentarze grupujące testy per metoda.
