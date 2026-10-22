# Właściwości vs Pola Klasy

## 🎯 Cel rozdziału

Zrozumienie różnicy między **polami** a **właściwościami**, oraz dlaczego właściwości są fundamentem **encapsulation** w OOP.

## 📚 Spis treści

1. [Pola (Fields)](#pola-fields)
2. [Właściwości (Properties)](#właściwości-properties)
3. [Encapsulation](#encapsulation)
4. [Gettery i Settery](#gettery-i-settery)
5. [Porównanie](#porównanie)

---

## Pola (Fields)

**Pole** to zwykła zmienna składowa klasy:

```csharp
public class Person
{
    public string name;  // Pole - bezpośredni dostęp
    public int age;
}

var person = new Person();
person.name = "John";  // Bezpośrednia zmiana
person.age = -5;       // Bez walidacji - PROBLEM!
```

**Problemy:**
- ❌ Brak walidacji - możesz ustawić `age = -5`
- ❌ Nie możesz logować/śledzić zmian
- ❌ Nie możesz obliczyć wartości dynamicznie
- ❌ Brak enkapsulacji

---

## Właściwości (Properties)

**Właściwość** to sterowana metoda dostępu do danych z `get` i `set` accessorami:

```csharp
public class Person
{
    private string _name = string.Empty;  // Prywatne pole
    
    public string Name   // Publiczna właściwość
    {
        get { return _name; }
        set { _name = value; }
    }
}

var person = new Person();
person.Name = "John";  // Używa set accessor
var name = person.Name;  // Używa get accessor
```

**Zalety:**
- ✅ Walidacja w setterze
- ✅ Logowanie zmian
- ✅ Obliczenia dynamiczne
- ✅ Enkapsulacja

Właściwość nie musi przechowywać wartości w polu. Może ją obliczać, delegować do innego obiektu albo udostępniać różne poziomy dostępu:

```csharp
public class Rectangle
{
    public double Width { get; init; }
    public double Height { get; init; }
    public double Area => Width * Height; // właściwość obliczana
}
```

Kod korzystający z `Area` nie musi wiedzieć, czy wartość jest zapisana, czy wyliczana. To właśnie odróżnia stabilny interfejs właściwości od bezpośredniego publicznego pola.

---

## Encapsulation

Ukrywanie wewnętrznych szczegółów implementacji:

```csharp
public class BankAccount
{
    private decimal balance = 0;  // Ukryte pole
    
    public decimal Balance
    {
        get { return balance; }
        private set { balance = value; }  // Tylko wewnętrznie
    }
    
    public void Deposit(decimal amount)
    {
        if (amount > 0)
            Balance = balance + amount;
    }
}

var account = new BankAccount();
account.Deposit(100);
Console.WriteLine(account.Balance);  // 100
// account.Balance = -1000;  // BŁĄD - setter jest prywatny!
```

---

## Gettery i Settery

### Read-only (tylko getter)

```csharp
public class User
{
    private int id;
    
    public int Id
    {
        get { return id; }
        // Bez setter!
    }
}

var user = new User();
var userId = user.Id;  // OK
// user.Id = 5;  // BŁĄD - nie ma setter!
```

### Write-only (tylko setter)

```csharp
public class Password
{
    private string hash = string.Empty;
    
    public string PasswordHash
    {
        set { hash = SHA256(value); }  // Haszowanie
        // Bez getter!
    }
}

var pwd = new Password();
pwd.PasswordHash = "tajne123";  // OK
// var h = pwd.PasswordHash;  // BŁĄD - nie ma getter!
```

---

## Porównanie

| Cecha | Pole | Właściwość |
|-------|------|-----------|
| Dostęp | Bezpośredni | Kontrolowany |
| Walidacja | Nie | Tak |
| Logowanie | Nie | Możliwe |
| Wirtualne | Nie | Tak |
| Interfejsy | Nie | Tak |
| Serializacja | Obowiązkowe | Preferowane |

---

## Best Practices

✅ **Zawsze używaj właściwości** zamiast publicznych pól

✅ **Pola prywatne** → **Właściwości publiczne**

✅ **Walidacja w setterze**, nie w konstruktorze

✅ **Asymetryczne accessory** (np. `public get; private set;`)

> **Uwaga:** właściwość nie jest automatycznie lepsza od pola w każdej sytuacji. Pola zwykle pozostają prywatne, a publiczne właściwości tworzą część API klasy. Zmiana publicznego pola na właściwość może wpływać na serializację, refleksję i kompatybilność binarną, dlatego warto zdecydować o publicznym API świadomie.

---

## 🚀 Jak pracować

```bash
cd code/
dotnet run
dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)

## 🧭 Nawigacja

[← Moduł 03: właściwości i indeksatory](../README.md) · Następny: [Auto Properties →](../_02_auto_properties/README.md)
