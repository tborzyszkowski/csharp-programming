using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Dynamic;

Console.OutputEncoding = Encoding.UTF8;

PrintHeader();

// Example 1: Expression Trees
RunExample1();

// Example 2: Dynamic Keyword
RunExample2();

// Example 3: DynamicObject
RunExample3();

// Example 4: Performance Comparison
RunExample4();

PrintFooter();

void RunExample1()
{
    Console.WriteLine("\n=== EXAMPLE 1: Expression Trees ===");
    
    // Build: x => x + 5
    var param = Expression.Parameter(typeof(int), "x");
    var constant = Expression.Constant(5);
    var body = Expression.Add(param, constant);
    var lambda = Expression.Lambda<Func<int, int>>(body, param);
    
    var compiled = lambda.Compile();
    var result = compiled(10);
    
    Console.WriteLine($"  Expression: x => x + 5");
    Console.WriteLine($"  Result for x=10: {result}");
}

void RunExample2()
{
    Console.WriteLine("\n=== EXAMPLE 2: Dynamic Keyword ===");
    
    dynamic value = "Hello World";
    
    // Dynamic method call
    var upper = value.ToUpper();
    var length = value.Length;
    
    Console.WriteLine($"  Value: {value}");
    Console.WriteLine($"  Upper: {upper}");
    Console.WriteLine($"  Length: {length}");
}

void RunExample3()
{
    Console.WriteLine("\n=== EXAMPLE 3: DynamicObject ===");
    
    dynamic dict = new DynamicDict();
    
    dict.Name = "Alice";
    dict.Age = 30;
    dict.City = "Warsaw";
    
    Console.WriteLine($"  Dynamic dictionary:");
    Console.WriteLine($"    Name: {dict.Name}");
    Console.WriteLine($"    Age: {dict.Age}");
    Console.WriteLine($"    City: {dict.City}");
}

void RunExample4()
{
    Console.WriteLine("\n=== EXAMPLE 4: Performance Comparison ===");
    
    var calc = new MathCalc();
    var method = typeof(MathCalc).GetMethod("Add");
    
    // Expression tree
    var p1 = Expression.Parameter(typeof(int));
    var p2 = Expression.Parameter(typeof(int));
    var callExpr = Expression.Call(Expression.Constant(calc), method, p1, p2);
    var lambda = Expression.Lambda<Func<int, int, int>>(callExpr, p1, p2);
    var compiled = lambda.Compile();
    
    Console.WriteLine("  Performance (relative):\n");
    Console.WriteLine("    Direct call:      ⚡⚡⚡ Fastest");
    Console.WriteLine("    Compiled expr:    ⚡⚡  Fast");
    Console.WriteLine("    Dynamic:          🐌 Medium");
    Console.WriteLine("    Reflection:       🐢 Slowest");
}

void PrintHeader()
{
    Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   EXPRESSION TREES & DYNAMIC                                       ║");
    Console.WriteLine("║   Building Expressions, Dynamic Calls, DynamicObject               ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

void PrintFooter()
{
    Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
    Console.WriteLine("║   ✓ Expression Trees & Dynamic Examples Completed                 ║");
    Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
}

// ============================================================================
// HELPER CLASSES
// ============================================================================

public class MathCalc
{
    public int Add(int a, int b) => a + b;
}

public class DynamicDict : DynamicObject
{
    private Dictionary<string, object> _values = new();
    
    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        return _values.TryGetValue(binder.Name, out result);
    }
    
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        _values[binder.Name] = value;
        return true;
    }
}
