using System;
using Xunit;

namespace InitializationOrder;

public class Logger
{
    private static int logCount = 0;
    
    public static void Log(string msg) => Console.WriteLine($"[LOG] {msg}");
    public static int LogCount => logCount;
    public static void Reset() => logCount = 0;
}

public class Example1
{
    // Krok 1: Pola inicjalizują się NAJPIERW
    public string Name { get; set; } = (Logger.Log("Field init: Name"), "DefaultName").Item2;
    public int Value { get; set; }
    
    // Krok 2: Konstruktor uruchamia się DRUGI
    public Example1(int value)
    {
        Logger.Log("Constructor start");
        Value = value;
        Logger.Log("Constructor end");
    }
}

public class Example2
{
    // Inicjalizatory pól - wykonują się w kolejności
    public string Field1 { get; set; } = (Logger.Log("Init Field1"), "Value1").Item2;
    public string Field2 { get; set; } = (Logger.Log("Init Field2"), "Value2").Item2;
    
    public Example2()
    {
        Logger.Log("Constructor");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 4: KOLEJNOŚĆ INICJALIZACJI ===\n");
        
        Console.WriteLine("1. POLA INICJALIZUJĄ SIĘ PRZED KONSTRUKTOREM:");
        var ex1 = new Example1(42);
        Console.WriteLine();
        
        Console.WriteLine("2. POLA INICJALIZUJĄ SIĘ W KOLEJNOŚCI DEKLARACJI:");
        Logger.Reset();
        var ex2 = new Example2();
    }
}

public class InitializationOrderTests
{
    [Fact]
    public void FieldsInitializeBeforeConstructor()
    {
        Logger.Reset();
        var ex = new Example1(10);
        
        Assert.Equal("DefaultName", ex.Name);
        Assert.Equal(10, ex.Value);
    }
}
