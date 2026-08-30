using System;
using Xunit;

namespace Destructors;

public class SimpleResource
{
    private string name;
    
    public SimpleResource(string name)
    {
        this.name = name;
        Console.WriteLine($"[CTOR] {name} created");
    }
    
    ~SimpleResource()
    {
        Console.WriteLine($"[DTOR] {name} destroyed");
    }
}

public class ManagedResource : IDisposable
{
    private string name;
    private bool disposed = false;
    
    public ManagedResource(string name)
    {
        this.name = name;
        Console.WriteLine($"[CTOR] {name} created");
    }
    
    public void DoWork()
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(ManagedResource));
        
        Console.WriteLine($"[WORK] {name} working");
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Console.WriteLine($"[DISPOSE] {name} - managed resources");
            }
            Console.WriteLine($"[DISPOSE] {name} - unmanaged resources");
            disposed = true;
        }
    }
    
    ~ManagedResource()
    {
        Dispose(false);
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 6: DESTRUKTORY ===\n");
        
        Console.WriteLine("1. SIMPLE DESTRUCTOR:");
        {
            var res = new SimpleResource("Resource1");
        }  // Wychodzi ze scope - ale destruktor uruchomi się PÓŹNIEJ
        Console.WriteLine();
        
        Console.WriteLine("2. IDISPOSABLE PATTERN:");
        using (var res = new ManagedResource("Resource2"))
        {
            res.DoWork();
        }  // Tutaj Dispose() uruchomi się NATYCHMIAST
        Console.WriteLine();
        
        Console.WriteLine("3. USING DECLARATION (C# 8+):");
        using var res3 = new ManagedResource("Resource3");
        res3.DoWork();
        // Tutaj Dispose() się uruchomi
        Console.WriteLine();
    }
}

public class DestructorTests
{
    [Fact]
    public void ManagedResource_Dispose_DisposesManagedResources()
    {
        var res = new ManagedResource("Test");
        res.DoWork();  // OK
        
        res.Dispose();
        
        Assert.Throws<ObjectDisposedException>(() => res.DoWork());
    }
}
