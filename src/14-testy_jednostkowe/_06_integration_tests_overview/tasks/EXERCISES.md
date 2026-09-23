# Ćwiczenia - Zarys Testów Integracyjnych

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 6.1: Klasyfikacja Testów
**Cel:** Odróżnić test jednostkowy od integracyjnego.

Dla poniższych testów określ, czy są jednostkowe czy integracyjne i uzasadnij jednym zdaniem:
1. Test wywołujący `OrderService.CalculateTotal()` z dwoma mockami (`Mock<IOrderRepository>`, `Mock<IDiscountService>`)
2. Test zapisujący plik JSON na dysk i odczytujący go z powrotem
3. Test wysyłający żądanie HTTP `GET /api/products` przez `WebApplicationFactory`

---

### Ćwiczenie 6.2: Test Integracyjny z Katalogiem Tymczasowym
**Cel:** Napisać własny prosty test integracyjny z realnym I/O.

Zaimplementuj `TextFileLogger` z metodą `Log(string message)` dopisującą linię do pliku. Napisz test integracyjny, który loguje 3 wiadomości do tymczasowego pliku i weryfikuje, że plik zawiera dokładnie 3 linie (`File.ReadAllLines`). Pamiętaj o posprzątaniu pliku w bloku `finally`.

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 6.3: Ten Sam Kod, Dwa Testy
**Cel:** Napisać zarówno test jednostkowy, jak i integracyjny dla tej samej funkcjonalności.

Mając interfejs `ICacheStore` z metodami `Set(string key, string value)` i `Get(string key)`:
1. Napisz test **jednostkowy** dla klasy korzystającej z `ICacheStore` (np. `ProductPriceCache`), mockując `ICacheStore`
2. Zaimplementuj `InMemoryCacheStore : ICacheStore` (prawdziwy `Dictionary<string, string>`) i napisz dla niej test **integracyjny**, weryfikujący, że dane faktycznie się zapisują i odczytują

Porównaj w komentarzu: co wykrywa pierwszy test, a czego nie wykryje (i odwrotnie)?

---

### Ćwiczenie 6.4: Analiza Kompromisów
**Cel:** Zrozumieć trade-offy między EF Core InMemory a Testcontainers.

Napisz krótką notatkę (5-8 zdań) odpowiadającą na pytania:
- Dlaczego test z EF Core InMemory Provider może **przejść**, mimo że ten sam kod **zawiedzie** na prawdziwym SQL Server?
- Kiedy warto zaakceptować to ryzyko w zamian za szybkość testów?
- Kiedy Testcontainers jest niezbędny mimo wolniejszego wykonania?

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 6.5: Projekt Zestawu Testów dla Repozytorium Plikowego
**Cel:** Zaprojektować kompletny zestaw testów integracyjnych.

Dla `FileOrderRepository` z tego tematu zaprojektuj (i zaimplementuj) dodatkowe testy integracyjne pokrywające:
- Nadpisanie istniejącego pliku nowymi danymi (`Save` wywołane dwukrotnie)
- Zachowanie przy uszkodzonym/niepoprawnym JSON w pliku (co się dzieje? czy powinien być rzucony czytelny wyjątek?)
- Współbieżny zapis z dwóch wątków do tego samego pliku (opisz w komentarzu potencjalne ryzyko, implementacja opcjonalna)

**Podpowiedź:** To ćwiczenie pokazuje, że testy integracyjne odkrywają problemy (race conditions, błędna obsługa błędów), których żaden mock nigdy by nie wykrył.
