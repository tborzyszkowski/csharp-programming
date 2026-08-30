using System;
using Xunit;

namespace PartialMethods;

// Część 1: Deklaracja
public partial class User
{
    private string name;
    
    partial void OnUserCreated();
    partial void OnNameChanged();
    
    public User(string name)
    {
        this.name = name;
        OnUserCreated();
    }
    
    public void ChangeName(string newName)
    {
        name = newName;
        OnNameChanged();
    }
    
    public string Name => name;
}

// Część 2: Implementacja
public partial class User
{
    partial void OnUserCreated()
    {
        Console.WriteLine($"✓ Użytkownik '{name}' utworzony");
    }
    
    partial void OnNameChanged()
    {
        Console.WriteLine($"✓ Nazwa zmieniona na '{name}'");
    }
}

/// ============================================
/// DEMONSTRACJA
/// ============================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  METODY CZĘŚCIOWE (PARTIAL METHODS)                   ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");
        
        var user = new User("Jan");
        user.ChangeName("Maria");
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
    }
    
    [Fact]
    public void PartialMethod_OnNameChanged()
    {
        var user = new User("Jan");
        user.ChangeName("Piotr");
        Assert.Equal("Piotr", user.Name);
    }
}
