# Ćwiczenia - Dobre Praktyki i Sugestywne Przykłady

## Poziomy Trudności
- 🟢 **Basic** - Podstawowe zrozumienie koncepcji
- 🟡 **Intermediate** - Praktyczne stosowanie
- 🔴 **Advanced** - Złożone scenariusze

---

## 🟢 BASIC LEVEL

### Ćwiczenie 7.1: Rozpoznaj Anti-Pattern
**Cel:** Nauczyć się rozpoznawać problematyczne testy.

Dla poniższych fragmentów wskaż, który anti-pattern (Fragile Test, Mystery Guest, Test Spaghetti) występuje i zaproponuj poprawkę:

```csharp
[Fact]
public void UserTest()
{
    var config = ConfigLoader.LoadDefault(); // skąd te dane?
    var user = new User(config.DefaultName);
    Assert.Equal("Admin", user.Name);
}
```

```csharp
[Fact]
public void ProcessOrder_Wszystko()
{
    var result1 = service.Validate(order);
    var result2 = service.CalculatePrice(order);
    var result3 = service.Ship(order);
    Assert.True(result1);
    Assert.Equal(100m, result2);
    Assert.True(result3);
}
```

---

### Ćwiczenie 7.2: Popraw Nazwy Testów
**Cel:** Zastosować konwencję nazewnictwa z tego modułu.

Popraw poniższe nazwy testów tak, aby stosowały konwencję `Metoda_Scenariusz_OczekiwanyWynik`:
- `TestLogin()`
- `SprawdzWalidacjeEmaila()`
- `Test_Order()`

---

## 🟡 INTERMEDIATE LEVEL

### Ćwiczenie 7.3: Checklist Code Review
**Cel:** Zastosować checklistę z tego tematu do prawdziwego kodu.

Weź dowolny test napisany w Temacie 1-6 tego modułu (Twój własny lub z `code/Program.cs`) i przejdź przez checklistę z sekcji "Checklist Przed Code Review". Zapisz w komentarzu, czy test spełnia wszystkie punkty, a jeśli nie – jak byś go poprawił.

---

### Ćwiczenie 7.4: Refaktoryzacja Testu Spaghetti
**Cel:** Rozbić jeden zbyt duży test na kilka mniejszych.

Poniższy test sprawdza zbyt wiele naraz. Podziel go na 3-4 osobne testy z dobrymi nazwami:

```csharp
[Fact]
public void ShoppingCart_DzialaPoprawnie()
{
    var cart = new ShoppingCart();
    cart.AddItem("Laptop", 3000m);
    cart.AddItem("Mysz", 100m);
    Assert.Equal(2, cart.ItemCount);
    Assert.Equal(3100m, cart.GetTotal());
    cart.RemoveItem("Mysz");
    Assert.Equal(1, cart.ItemCount);
    cart.Clear();
    Assert.Equal(0, cart.ItemCount);
}
```

---

## 🔴 ADVANCED LEVEL

### Ćwiczenie 7.5: Audyt Jakości Całego Modułu
**Cel:** Zastosować całą wiedzę z modułu do krytycznej oceny.

Przejrzyj testy napisane w Tematach 1-6 tego modułu (`code/Program.cs` każdego tematu) i sporządź krótki raport (10-15 zdań) odpowiadający na:
- Czy któryś test narusza zasadę Independent (np. dzieli stan z innym testem)?
- Czy nazewnictwo we wszystkich tematach jest spójne z konwencją `Metoda_Scenariusz_Wynik`?
- Który temat najlepiej ilustruje zasadę Fast, a który (test integracyjny z Tematu 6) świadomie ją łamie – i dlaczego to uzasadnione?

### Ćwiczenie 7.6: Konfiguracja Code Coverage
**Cel:** Uruchomić i zinterpretować raport pokrycia kodu.

Uruchom `dotnet test --collect:"XPlat Code Coverage"` dla kodu z tego tematu. Znajdź wygenerowany plik `coverage.cobertura.xml`, zidentyfikuj który procent linii `OrderProcessor` został pokryty, i zaproponuj (bez konieczności implementacji) dodatkowy test zwiększający pokrycie o scenariusz obecnie nietestowany.
