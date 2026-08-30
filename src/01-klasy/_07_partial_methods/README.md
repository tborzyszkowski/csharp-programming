# Metody Częściowe (Partial Methods)

## 🎯 Cel

Deklaracja metody w jednej części, implementacja w drugiej.

---

## 🚀 Uruchomienie

```bash
cd code/
dotnet run
dotnet test
```

## Syntaktyka

```csharp
public partial class User
{
    // Deklaracja (część 1)
    partial void OnUserCreated();
    
    public User(string name)
    {
        Name = name;
        OnUserCreated();  // Wywołanie
    }
}

public partial class User
{
    // Implementacja (część 2)
    partial void OnUserCreated()
    {
        Console.WriteLine("Użytkownik utworzony!");
    }
}
```

## Zasady

✅ `partial` metoda może być bez implementacji  
✅ Jeśli brak implementacji, kompilator ją opuszcza  
✅ Parametry zwracane: `void` lub `ref`  
✅ Nie mogą być `virtual`, `abstract`, `override`  

---

## 📖 Referencje

[Microsoft Docs - Partial Methods](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes#partial-methods)

