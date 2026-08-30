# Zadania: Funkcje Wirtualne - Wstęp

## 📝 Zadanie 1: Hierarchia Pojazdów

Stwórz hierarchię klas reprezentującą pojazdy:

### Wymagania:
1. Stwórz klasę bazową `Pojazd` z:
   - Właściwościami: `Marka`, `Model`, `RokProdukcji`
   - Wirtualną metodą `Wyswietl()` wypisującą informacje
   - Wirtualną metodą `PotrzebnyPaliwoPO100km()` zwracającą double

2. Stwórz klasy pochodne:
   - `Samochod` - pali 7L/100km
   - `Motocykl` - pali 3L/100km
   - `Ciężarówka` - pali 12L/100km

3. Stwórz metodę `ObliczKosztPrzejazdu(Pojazd pojazd, double km, double cenaPaliwa)`
   - Powinna działać na KAŻDYM pojazd

### Przykład Użycia:
```csharp
List<Pojazd> pojazdy = new()
{
    new Samochod { Marka = "Toyota", Model = "Camry", RokProdukcji = 2022 },
    new Motocykl { Marka = "Harley", Model = "Street 750", RokProdukcji = 2023 },
    new Ciężarówka { Marka = "Volvo", Model = "FH16", RokProdukcji = 2021 }
};

foreach (var pojazd in pojazdy)
{
    pojazd.Wyswietl();
    decimal koszt = ObliczKosztPrzejazdu(pojazd, 1000, 5.50); // 1000km, 5.50 za litr
    Console.WriteLine($"Koszt przejazdu: {koszt:C}\n");
}
```

### Rozwiązanie:

<details>
<summary>Kliknij aby zobaczyć rozwiązanie</summary>

```csharp
public abstract class Pojazd
{
    public string Marka { get; set; }
    public string Model { get; set; }
    public int RokProdukcji { get; set; }
    
    public virtual void Wyswietl()
    {
        Console.WriteLine($"{Marka} {Model} ({RokProdukcji})");
    }
    
    public abstract double PotrzebnyPaliwoPO100km();
}

public class Samochod : Pojazd
{
    public override double PotrzebnyPaliwoPO100km() => 7.0;
}

public class Motocykl : Pojazd
{
    public override double PotrzebnyPaliwoPO100km() => 3.0;
}

public class Ciężarówka : Pojazd
{
    public override double PotrzebnyPaliwoPO100km() => 12.0;
}

public static decimal ObliczKosztPrzejazdu(Pojazd pojazd, double km, double cenaPaliwa)
{
    double litry = (km / 100) * pojazd.PotrzebnyPaliwoPO100km();
    return (decimal)litry * (decimal)cenaPaliwa;
}
```

</details>

---

## 📝 Zadanie 2: System Pracowników

Stwórz system zarządzania pracownikami:

### Wymagania:
1. Stwórz klasę bazową `Pracownik` z:
   - Właściwościami: `Imię`, `Nazwisko`, `Stanowisko`
   - Wirtualną metodą `ObliczPensję()` zwracającą decimal
   - Wirtualną metodą `WyswietlInfo()` wypisującą dane

2. Stwórz klasy pochodne:
   - `Programista` - pensja = 5000 + 100 za każdy rok doświadczenia
   - `Manager` - pensja = 6000 + bonus 500 na każdego pracownika
   - `Specjalista` - pensja = 4500 + 50 za każdą certyfikację

3. Stwórz metodę `WyswietlRaport(List<Pracownik> pracownicy)`
   - Powinna wyświetlić wszystkich pracowników
   - Podsumować łączne wynagrodzenia

### Rozwiązanie:

<details>
<summary>Kliknij aby zobaczyć rozwiązanie</summary>

```csharp
public abstract class Pracownik
{
    public string Imię { get; set; }
    public string Nazwisko { get; set; }
    public string Stanowisko { get; set; }
    
    public virtual decimal ObliczPensję()
    {
        return 3000m;  // Domyślna pensja
    }
    
    public virtual void WyswietlInfo()
    {
        Console.WriteLine($"{Imię} {Nazwisko} - {Stanowisko}: {ObliczPensję():C}");
    }
}

public class Programista : Pracownik
{
    public int LatDoświadczenia { get; set; }
    
    public override decimal ObliczPensję()
    {
        return 5000m + (LatDoświadczenia * 100m);
    }
}

public class Manager : Pracownik
{
    public int LiczbaPoddanych { get; set; }
    
    public override decimal ObliczPensję()
    {
        return 6000m + (LiczbaPoddanych * 500m);
    }
}

public class Specjalista : Pracownik
{
    public int Certyfikacje { get; set; }
    
    public override decimal ObliczPensję()
    {
        return 4500m + (Certyfikacje * 50m);
    }
}

public static void WyswietlRaport(List<Pracownik> pracownicy)
{
    Console.WriteLine("╔════════════════════════════════════════╗");
    Console.WriteLine("║  RAPORT WYNAGRODZEŃ                    ║");
    Console.WriteLine("╚════════════════════════════════════════╝\n");
    
    decimal total = 0m;
    foreach (var pracownik in pracownicy)
    {
        pracownik.WyswietlInfo();
        total += pracownik.ObliczPensję();
    }
    
    Console.WriteLine($"\nLączne wynagrodzenie: {total:C}");
}
```

</details>

---

## 📝 Zadanie 3: System Notyfikacji

Stwórz system do wysyłania notyfikacji:

### Wymagania:
1. Stwórz klasę bazową `Notifikacja` z:
   - Właściwościami: `Temat`, `Wiadomość`, `Odbiorca`
   - Wirtualną metodą `Wyślij()` 
   - Wirtualną metodą `WyswietlPodgląd()`

2. Stwórz klasy pochodne:
   - `EmailNotifikacja` - wyświetla format "Email do: recipient"
   - `SMSNotifikacja` - wyświetla format "SMS do: recipient"
   - `PushNotifikacja` - wyświetla format "Push notification na device"

3. Stwórz system kolejki:
   ```csharp
   WyslijWszystkieNotyfikacje(List<Notifikacja> notyfikacje)
   ```

### Rozwiązanie:

<details>
<summary>Kliknij aby zobaczyć rozwiązanie</summary>

```csharp
public abstract class Notifikacja
{
    public string Temat { get; set; }
    public string Wiadomość { get; set; }
    public string Odbiorca { get; set; }
    
    public virtual void Wyślij()
    {
        Console.WriteLine($"Wysyłam: {Temat}");
    }
    
    public virtual void WyswietlPodgląd()
    {
        Console.WriteLine($"Temat: {Temat}\nWiadomość: {Wiadomość}");
    }
}

public class EmailNotifikacja : Notifikacja
{
    public override void Wyślij()
    {
        Console.WriteLine($"📧 Email do: {Odbiorca}");
        Console.WriteLine($"   Temat: {Temat}");
    }
}

public class SMSNotifikacja : Notifikacja
{
    public override void Wyślij()
    {
        Console.WriteLine($"📱 SMS do: {Odbiorca}");
        Console.WriteLine($"   {Wiadomość}");
    }
}

public class PushNotifikacja : Notifikacja
{
    public override void Wyślij()
    {
        Console.WriteLine($"🔔 Push notification");
        Console.WriteLine($"   Użytkownik: {Odbiorca}");
        Console.WriteLine($"   {Temat}: {Wiadomość}");
    }
}

public static void WyslijWszystkieNotyfikacje(List<Notifikacja> notyfikacje)
{
    foreach (var notyfikacja in notyfikacje)
    {
        notyfikacja.Wyślij();
        Console.WriteLine();
    }
}
```

</details>

---

## 🎓 Koncepty do Zrozumienia

Po rozwiązaniu zadań powinieneś rozumieć:

- [ ] Kiedy używać `virtual`
- [ ] Kiedy używać `override`
- [ ] Jak działa late binding (wiązanie w runtime)
- [ ] Dlaczego polimorfizm jest ważny
- [ ] Jak napisać kod, który pracuje z hierarchiami klas

---

## 📚 Dodatkowe Wyzwania (Challenge)

1. **Łańcuch odpowiedzialności**: Dodaj do systemu notyfikacji system retry'owania jeśli wysyłanie nie udało się
2. **Strategie płatności**: Rozszerz system płatności o składankę - może się dzielić między wiele metod
3. **Logging**: Dodaj wirtualną metodę `LogEvent()` do każdej klasy

---

*Pamiętaj: Polimorfizm pozwala ci pisać kod, który działa z nieznanymiami Ci typami, o ile implementują właściwy interfejs!*
