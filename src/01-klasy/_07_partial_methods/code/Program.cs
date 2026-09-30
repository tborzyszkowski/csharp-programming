using System;
using System.Collections.Generic;
using Xunit;

namespace PartialMethods;

// Część 1: Deklaracje (np. kod wygenerowany przez narzędzie)
public partial class User
{
    private string name;

    public List<string> Events { get; } = new();

    // Klasyczne metody częściowe: niejawnie private, zwracają void.
    // Implementacja jest opcjonalna - bez niej kompilator usuwa wywołanie.
    partial void OnUserCreated();
    partial void OnNameChanged();
    partial void OnNameChanging(string newName);   // celowo BEZ implementacji

    // Rozszerzona metoda częściowa (C# 9): modyfikator dostępu i wartość zwracana
    // => implementacja jest OBOWIĄZKOWA.
    public partial bool CanChangeName(string newName);

    public User(string name)
    {
        this.name = name;
        OnUserCreated();
    }

    public bool ChangeName(string newName)
    {
        if (!CanChangeName(newName))
            return false;

        OnNameChanging(newName);   // wywołanie zniknie z kodu IL - brak implementacji
        name = newName;
        OnNameChanged();
        return true;
    }

    public string Name => name;
}

// Część 2: Implementacja (kod napisany ręcznie)
public partial class User
{
    partial void OnUserCreated()
    {
        Events.Add("created");
        Console.WriteLine($"✓ Użytkownik '{name}' utworzony");
    }

    partial void OnNameChanged()
    {
        Events.Add("renamed");
        Console.WriteLine($"✓ Nazwa zmieniona na '{name}'");
    }

    public partial bool CanChangeName(string newName) => !string.IsNullOrWhiteSpace(newName);
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  METODY CZĘŚCIOWE (PARTIAL METHODS)                   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        var user = new User("Jan");
        user.ChangeName("Maria");

        bool changed = user.ChangeName("   ");  // odrzucone przez CanChangeName
        Console.WriteLine($"Zmiana na pustą nazwę udana? {changed}; nazwa: {user.Name}");
    }
}

/// ============================================
/// TESTY
/// ============================================

public class PartialMethodsTests
{
    [Fact]
    public void PartialMethod_CallsImplementation()
    {
        var user = new User("Anna");

        Assert.Equal("Anna", user.Name);
        Assert.Equal(new[] { "created" }, user.Events);   // hook OnUserCreated został wywołany
    }
    
    [Fact]
    public void PartialMethod_OnNameChanged()
    {
        var user = new User("Jan");

        Assert.True(user.ChangeName("Piotr"));

        Assert.Equal("Piotr", user.Name);
        Assert.Equal(new[] { "created", "renamed" }, user.Events);
    }

    [Fact]
    public void ExtendedPartialMethod_CanRejectChange()
    {
        var user = new User("Jan");

        Assert.False(user.ChangeName("  "));

        Assert.Equal("Jan", user.Name);
        Assert.DoesNotContain("renamed", user.Events);
    }
}
