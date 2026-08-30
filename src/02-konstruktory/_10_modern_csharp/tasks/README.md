# Zadania - Nowoczesne C#

## 📝 Zadanie 1: Record dla DTO

Stwórz `UserDto` jako record z Name, Email, CreatedAt.

```csharp
public record UserDto(string Name, string Email, DateTime CreatedAt);

var user1 = new UserDto("John", "john@example.com", DateTime.Now);
var user2 = user1 with { Name = "Jane" };
```

## 📝 Zadanie 2: Configuration z init properties

Stwórz `DatabaseConfig` z init-only properties dla hosta, portu, bazy danych.

## 📝 Zadanie 3: Primary Constructor Service

Stwórz `UserService(IUserRepository repo, ILogger logger)` z primary constructor.

---

## ✅ Rozwiązania w `code/Program.cs`
