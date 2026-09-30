# Zadania - Modyfikatory Dostępu

## 📝 Zadanie 1: Klasa User

Zaimplementuj klasę `User` zgodnie z poniższym szkicem:

```csharp
public class User
{
    private string password;
    private DateTime lastLogin;
    
    public string Username { get; }
    public string Email { get; private set; }
    
    public bool Login(string inputPassword)
    {
        // publiczna metoda korzystająca z prywatnej VerifyPassword
    }
}
```

- `password`: private (nigdy nie udostępniany na zewnątrz)
- `lastLogin`: private (odczyt przez metodę `GetLastLogin()`)
- `Username`: public, tylko do odczytu (ustawiany w konstruktorze)
- `Email`: public odczyt, private zapis (zmiana tylko przez `ChangeEmail`)
- Po **3 nieudanych logowaniach z rzędu** konto jest blokowane i **żadne** dalsze logowanie (także z poprawnym
  hasłem) nie może się udać

## 📝 Zadanie 2: Klasa pochodna i `protected`

Rozszerz `User` o klasę `AdminUser : User`, której konto jest blokowane dopiero po **5** nieudanych logowaniach
(zwykły użytkownik – po 3). Pomyśl, które składowe klasy bazowej muszą być `protected`, a które mogą pozostać
`private`. Uzasadnij wybór każdego modyfikatora.

---

## ✅ Rozwiązanie: Klasa User z Modyfikatorami

### Kod

```csharp
public class User
{
    // PRIVATE - tylko wewnątrz klasy
    private string password;
    private DateTime lastLogin;
    private int loginAttempts;
    
    // PROTECTED - klasa pochodna może zmienić regułę blokady, ale nie dotyka stanu (loginAttempts)
    protected virtual int MaxLoginAttempts => 3;
    
    // PUBLIC READ-ONLY - ustawiane tylko w konstruktorze, potem się nie zmieniają
    public string Username { get; }
    
    // PUBLIC READ, PRIVATE WRITE - czytane publicznie, zmieniane tylko wewnątrz klasy
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
    
    public User(string username, string email, string password)
    {
        Username = username;
        Email = email;
        this.password = password;   // UWAGA: uproszczenie dydaktyczne - patrz niżej
        lastLogin = DateTime.MinValue;
        IsActive = true;
    }
    
    // PUBLIC metody
    public bool Login(string inputPassword)
    {
        if (!IsActive)
            return false;              // zablokowane konto nie może się zalogować nawet poprawnym hasłem
        
        if (VerifyPassword(inputPassword))
        {
            lastLogin = DateTime.Now;
            loginAttempts = 0;
            return true;
        }
        
        loginAttempts++;
        if (loginAttempts >= MaxLoginAttempts)
        {
            IsActive = false;          // blokada konta
            Console.WriteLine($"⚠️  Konto zablokowane po {MaxLoginAttempts} nieudanych próbach");
        }
        return false;
    }
    
    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail) || !newEmail.Contains('@'))
            throw new ArgumentException("Niepoprawny adres e-mail", nameof(newEmail));
        Email = newEmail;   // OK - private set
    }
    
    public DateTime GetLastLogin() => lastLogin;   // tylko odczyt, bez możliwości zapisu z zewnątrz
    
    // PRIVATE metoda pomocnicza - szczegół implementacji
    private bool VerifyPassword(string inputPassword) => password == inputPassword;
    
    public override string ToString() => $"{Username} ({Email})";
}

public class AdminUser : User
{
    public AdminUser(string username, string email, string password)
        : base(username, email, password) { }
    
    // protected override: zmiana reguły bez dostępu do prywatnego licznika prób
    protected override int MaxLoginAttempts => 5;
}

// W Main():
Console.WriteLine("🔐 Modyfikatory Dostępu - Enkapsulacja");
Console.WriteLine("──────────────────────────────────────\n");

var user = new User("jkowalski", "jan@example.com", "password123");

Console.WriteLine($"Użytkownik: {user}");
Console.WriteLine($"Username (public read-only): {user.Username}");
Console.WriteLine($"Email (public r, private w): {user.Email}");
Console.WriteLine($"IsActive: {user.IsActive}");

Console.WriteLine("\nLogowanie:");
bool success = user.Login("wrongPassword");
Console.WriteLine($"Wynik: {success}");

success = user.Login("password123");
Console.WriteLine($"Wynik: {success}");

Console.WriteLine($"\nOstatnie logowanie: {user.GetLastLogin()}");

// Te linie byłyby błędem kompilacji:
// user.password = "new";         // Błąd - private
// user.lastLogin = DateTime.Now; // Błąd - private
// user.Username = "nowy";        // Błąd - read-only
// user.VerifyPassword("x");      // Błąd - private
```

### Wyjaśnienie Modyfikatorów

| Modyfikator | Gdzie widać | Przykład |
|-------------|------------|----------|
| **public** | Wszędzie | `Username { get; }` - czytać można wszędzie |
| **private** | Tylko wewnątrz klasy | `password`, `VerifyPassword()` - szczegóły ukryte przed światem |
| **protected** | Klasa i klasy pochodne | `MaxLoginAttempts` - punkt rozszerzenia dla `AdminUser` |
| **get-only** (`{ get; }`) | Wartość ustawiana tylko w konstruktorze | `Username` - nie zmienia się nigdy |
| **private set** | Publiczny odczyt, zapis tylko wewnątrz klasy | `Email`, `IsActive` - zmieniane tylko przez metody klasy |

> ⚠️ **Hasła w prawdziwej aplikacji.** Przechowywanie hasła jako zwykłego `string` i porównywanie przez `==`
> to uproszczenie dydaktyczne. Prawdziwy system **nigdy** nie przechowuje hasła jawnie: zapisuje się jego
> *hash* wyliczony wolnym, solonym algorytmem (PBKDF2, bcrypt, Argon2 – w ASP.NET Core `PasswordHasher<T>`),
> a porównanie wykonuje się w stałym czasie (np. `CryptographicOperations.FixedTimeEquals`). Modyfikatory
> dostępu chronią przed błędami programisty, **nie** przed kimś, kto ma dostęp do pamięci lub bazy danych.

### Testy

```csharp
[Fact]
public void LoginWithCorrectPassword_ReturnsTrue()
{
    var user = new User("test", "test@example.com", "pass123");
    Assert.True(user.Login("pass123"));
}

[Fact]
public void LoginWithWrongPassword_ReturnsFalse()
{
    var user = new User("test", "test@example.com", "pass123");
    Assert.False(user.Login("wrongpass"));
}

[Fact]
public void ThreeFailedLogins_BlocksAccount()
{
    var user = new User("test", "test@example.com", "pass123");
    
    user.Login("wrong1");
    user.Login("wrong2");
    user.Login("wrong3");
    
    Assert.False(user.IsActive);
}

[Fact]
public void BlockedAccount_CannotLoginEvenWithCorrectPassword()
{
    var user = new User("test", "test@example.com", "pass123");
    user.Login("x"); user.Login("x"); user.Login("x");   // blokada
    
    Assert.False(user.Login("pass123"));
}

[Fact]
public void SuccessfulLogin_ResetsFailedAttemptsCounter()
{
    var user = new User("test", "test@example.com", "pass123");
    user.Login("x"); user.Login("x");
    user.Login("pass123");          // poprawne logowanie zeruje licznik
    user.Login("x"); user.Login("x");
    
    Assert.True(user.IsActive);     // w sumie 4 porażki, ale nie 3 z rzędu
}

[Fact]
public void AdminUser_AllowsFiveAttempts()
{
    var admin = new AdminUser("root", "root@example.com", "pass123");
    
    for (int i = 0; i < 4; i++) admin.Login("bad");
    Assert.True(admin.IsActive);
    
    admin.Login("bad");             // piąta nieudana próba
    Assert.False(admin.IsActive);
}

[Fact]
public void ChangeEmail_InvalidAddress_Throws()
{
    var user = new User("test", "test@example.com", "pass123");
    
    Assert.Throws<ArgumentException>(() => user.ChangeEmail("not-an-email"));
    Assert.Equal("test@example.com", user.Email);
}
```

