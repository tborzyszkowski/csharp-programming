# Zadania - Modyfikatory Dostępu

## 📝 Zadanie: Klasa User

```csharp
public class User
{
    private string password;
    private DateTime lastLogin;
    
    public string Username { get; }
    public string Email { get; private set; }
    
    public bool ValidatePassword(string inputPassword)
    {
        // private metoda
    }
}
```

- Password: private
- LastLogin: private
- Username: public read-only
- Email: public read, private write

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
    
    // PUBLIC READ-ONLY - zaraz po konstruktorze się nie zmienia
    public string Username { get; }
    
    // PUBLIC READ, PRIVATE WRITE - czytane publicznie, zmieniane tylko wewnątrz
    public string Email { get; private set; }
    public bool IsActive { get; private set; }
    
    public User(string username, string email, string password)
    {
        Username = username;      // Ustawiane w konstruktorze
        Email = email;
        this.password = password; // private - nie można zmienić z zewnątrz
        this.lastLogin = DateTime.MinValue;
        this.loginAttempts = 0;
        IsActive = true;
    }
    
    // PUBLIC metody
    public bool Login(string inputPassword)
    {
        if (VerifyPassword(inputPassword))
        {
            lastLogin = DateTime.Now;
            loginAttempts = 0;
            return true;
        }
        
        loginAttempts++;
        if (loginAttempts >= 3)
        {
            IsActive = false;  // Blokada konta
            Console.WriteLine("⚠️  Konto zablokowane po 3 nieudanych próbach");
        }
        return false;
    }
    
    public void ChangeEmail(string newEmail)
    {
        Email = newEmail;  // OK - private set
    }
    
    public DateTime GetLastLogin() => lastLogin;  // Zamiast property
    
    // PRIVATE metody - pomocnicze
    private bool VerifyPassword(string inputPassword)
    {
        return password == inputPassword;  // Uproszczone - w rzeczywistości byłoby hashing
    }
    
    private void LogActivity(string action)
    {
        Console.WriteLine($"[{DateTime.Now}] {Username}: {action}");
    }
    
    public override string ToString() => $"{Username} ({Email})";
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
| **public** | Wszędzie | `Email { get; private set; }` - czytać można wszędzie |
| **private** | Tylko wewnątrz klasy | `password` - bezpieczne przechowywanie |
| **read-only** | Ustawić można tylko w konstruktorze | `Username` - nie zmienia się nigdy |
| **private set** | Setter dostępny tylko wewnątrz | `Email` - zmieniane tylko metodą |

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
public void UsernameIsReadOnly_CannotChange()
{
    var user = new User("test", "test@example.com", "pass123");
    // user.Username = "newname";  // Nie skompiluje się!
    Assert.Equal("test", user.Username);
}
```

