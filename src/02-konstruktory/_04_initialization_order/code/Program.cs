using System;
using System.Collections.Generic;
using Xunit;

namespace InitializationOrder;

/// <summary>
/// Zapisuje kolejność zdarzeń inicjalizacji (do wypisania na konsoli i do sprawdzenia w testach).
/// </summary>
public static class Trace
{
    public static List<string> Log { get; } = new();
    
    public static string Write(string message)
    {
        Log.Add(message);
        Console.WriteLine($"  {message}");
        return message;
    }
    
    public static void Reset() => Log.Clear();
}

// ============ 1. POLA -> KONSTRUKTOR ============

public class Example1
{
    // Inicjalizatory pól i właściwości wykonują się PRZED ciałem konstruktora, w kolejności zapisu
    private string field = Trace.Write("Example1: field initializer");
    public string Name { get; set; } = Trace.Write("Example1: property initializer");
    public int Value { get; set; }
    
    public Example1(int value)
    {
        Trace.Write("Example1: constructor body (start)");
        Value = value;
        Trace.Write("Example1: constructor body (end)");
    }
}

// ============ 2. INICJALIZATOR OBIEKTU PO KONSTRUKTORZE ============

public class Example2
{
    public string Title { get; set; } = "";
    
    private int priority;
    public int Priority
    {
        get => priority;
        set { Trace.Write($"Example2: setter Priority = {value}"); priority = value; }
    }
    
    public Example2() => Trace.Write("Example2: constructor body");
}

// ============ 3. ŁAŃCUCH KONSTRUKTORÓW I POLA ============

public class Example3
{
    // W łańcuchu this(...) inicjalizatory pól wykonują się TYLKO RAZ - w konstruktorze, który nie deleguje dalej
    private string field = Trace.Write("Example3: field initializer");
    
    public Example3() : this(1) => Trace.Write("Example3: ctor()");
    public Example3(int x) => Trace.Write("Example3: ctor(int) - main");
}

// ============ 4. DZIEDZICZENIE ============

public class BaseClass
{
    private string baseField = Trace.Write("BaseClass: field initializer");
    
    public BaseClass()
    {
        Trace.Write("BaseClass: constructor body");
        Describe();   // wywołanie metody wirtualnej z konstruktora - NIEBEZPIECZNE
    }
    
    public virtual void Describe() => Trace.Write("BaseClass.Describe");
}

public class DerivedClass : BaseClass
{
    private string derivedField = Trace.Write("DerivedClass: field initializer");
    private string? assignedInBody;
    
    public DerivedClass()
    {
        Trace.Write("DerivedClass: constructor body (start)");
        assignedInBody = "set in constructor body";
        Trace.Write("DerivedClass: constructor body (end)");
    }
    
    // Wywołane z konstruktora klasy bazowej, zanim ciało konstruktora pochodnego zdążyło się wykonać!
    public override void Describe() =>
        Trace.Write($"DerivedClass.Describe: derivedField={(derivedField is null ? "null" : "set")}, assignedInBody={assignedInBody ?? "null"}");
}

// ============ 5. STATYCZNE vs INSTANCJI ============

public class StaticDemo
{
    private static readonly string staticField = Trace.Write("StaticDemo: static field initializer");
    private readonly string instanceField = Trace.Write("StaticDemo: instance field initializer");
    
    static StaticDemo() => Trace.Write("StaticDemo: static constructor");
    
    public StaticDemo() => Trace.Write("StaticDemo: instance constructor");
    
    public static void Touch() => Trace.Write("StaticDemo: Touch()");
}

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== TEMAT 4: KOLEJNOŚĆ INICJALIZACJI ===\n");
        
        Console.WriteLine("1. POLA I WŁAŚCIWOŚCI PRZED KONSTRUKTOREM:");
        _ = new Example1(42);
        Console.WriteLine();
        
        Console.WriteLine("2. INICJALIZATOR OBIEKTU PO KONSTRUKTORZE:");
        _ = new Example2 { Title = "Demo", Priority = 5 };
        Console.WriteLine();
        
        Console.WriteLine("3. ŁAŃCUCH this(...): INICJALIZATOR POLA TYLKO RAZ:");
        _ = new Example3();
        Console.WriteLine();
        
        Console.WriteLine("4. DZIEDZICZENIE (pola pochodnej -> pola bazowej -> ctor bazowej -> ctor pochodnej):");
        _ = new DerivedClass();
        Console.WriteLine();
        
        Console.WriteLine("5. STATYCZNE PRZED INSTANCJI (pierwsze użycie klasy):");
        StaticDemo.Touch();
        _ = new StaticDemo();
        _ = new StaticDemo();   // konstruktor statyczny nie powtarza się
    }
}

public class InitializationOrderTests
{
    [Fact]
    public void FieldsAndPropertiesInitialize_BeforeConstructorBody_InDeclarationOrder()
    {
        Trace.Reset();
        
        var ex = new Example1(10);
        
        Assert.Equal(new[]
        {
            "Example1: field initializer",
            "Example1: property initializer",
            "Example1: constructor body (start)",
            "Example1: constructor body (end)"
        }, Trace.Log);
        Assert.Equal(10, ex.Value);
    }
    
    [Fact]
    public void ObjectInitializer_RunsAfterConstructor()
    {
        Trace.Reset();
        
        _ = new Example2 { Priority = 7 };
        
        Assert.Equal(new[]
        {
            "Example2: constructor body",
            "Example2: setter Priority = 7"
        }, Trace.Log);
    }
    
    [Fact]
    public void ChainedConstructors_FieldInitializerRunsOnce_MainBodyFirst()
    {
        Trace.Reset();
        
        _ = new Example3();
        
        Assert.Equal(new[]
        {
            "Example3: field initializer",
            "Example3: ctor(int) - main",
            "Example3: ctor()"
        }, Trace.Log);
    }
    
    [Fact]
    public void Inheritance_DerivedFieldInitializersRunBeforeBaseConstructor()
    {
        Trace.Reset();
        
        _ = new DerivedClass();
        
        Assert.Equal(new[]
        {
            "DerivedClass: field initializer",     // 1. pola klasy POCHODNEJ
            "BaseClass: field initializer",        // 2. pola klasy bazowej
            "BaseClass: constructor body",         // 3. konstruktor bazowej...
            "DerivedClass.Describe: derivedField=set, assignedInBody=null",   // ...wywołuje przesłoniętą metodę
            "DerivedClass: constructor body (start)",   // 4. dopiero teraz konstruktor pochodnej
            "DerivedClass: constructor body (end)"
        }, Trace.Log);
    }
    
    [Fact]
    public void StaticMembers_InitializeOnceBeforeFirstInstance()
    {
        Trace.Reset();
        
        _ = new StaticDemo();
        _ = new StaticDemo();
        
        Assert.Equal(new[]
        {
            "StaticDemo: static field initializer",
            "StaticDemo: static constructor",
            "StaticDemo: instance field initializer",
            "StaticDemo: instance constructor",
            "StaticDemo: instance field initializer",
            "StaticDemo: instance constructor"
        }, Trace.Log);
    }
}
