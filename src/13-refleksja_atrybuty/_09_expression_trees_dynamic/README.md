# Temat 9: Expression Trees & Dynamic - Advanced Reflection

## 🌳 Expression Trees - Buduj Wyrażenia Dynamicznie

**Expression Tree** = reprezentacja kodu jako obiektu (AST).

```csharp
// Normal lambda - compiled to IL
Func<int, int, int> add = (a, b) => a + b;

// Expression tree - compiled at runtime
Expression<Func<int, int, int>> addExpr = (a, b) => a + b;

// Compile to delegate
Func<int, int, int> compiled = addExpr.Compile();
var result = compiled(5, 3);  // 8
```

---

## 🏗️ Building Expressions Manually

```csharp
// Build: x => x + 1

var parameter = Expression.Parameter(typeof(int), "x");
var constant = Expression.Constant(1);
var body = Expression.Add(parameter, constant);

var lambda = Expression.Lambda<Func<int, int>>(body, parameter);
var compiled = lambda.Compile();

var result = compiled(10);  // 11
```

### Complex Expression

```csharp
// Build: (a, b) => (a * 2) + (b * 3)

var paramA = Expression.Parameter(typeof(int), "a");
var paramB = Expression.Parameter(typeof(int), "b");

var mulA = Expression.Multiply(paramA, Expression.Constant(2));
var mulB = Expression.Multiply(paramB, Expression.Constant(3));
var add = Expression.Add(mulA, mulB);

var lambda = Expression.Lambda<Func<int, int, int>>(add, paramA, paramB);
var compiled = lambda.Compile();

var result = compiled(5, 10);  // 5*2 + 10*3 = 40
```

---

## ⚡ Comparison: Reflection vs Expression Trees vs Dynamic

```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
}

// Method 1: Direct call (FASTEST)
var calc = new Calculator();
var result1 = calc.Add(5, 3);  // ~10ns

// Method 2: Reflection (SLOWEST)
var method = typeof(Calculator).GetMethod("Add");
var result2 = method.Invoke(calc, new object[] { 5, 3 });  // ~200ns

// Method 3: Expression Tree (Compiled = FAST)
var param1 = Expression.Parameter(typeof(int));
var param2 = Expression.Parameter(typeof(int));
var callExpr = Expression.Call(Expression.Constant(calc), method, param1, param2);
var lambda = Expression.Lambda<Func<int, int, int>>(callExpr, param1, param2);
var compiled = lambda.Compile();
var result3 = compiled(5, 3);  // ~20ns (after compilation)

// Method 4: Dynamic (MEDIUM)
dynamic dynCalc = calc;
var result4 = dynCalc.Add(5, 3);  // ~100ns
```

---

## 🎭 Dynamic Keyword - Runtime Type Checking

```csharp
// Compile-time: type checking
string name = "Alice";
int length = name.Length;  // OK - knows string has Length

// Runtime: no compile-time checking
dynamic dynValue = "Alice";
dynamic dynLength = dynValue.Length;  // OK - assumes has Length

// Problem: typo not caught until runtime
dynamic badAccess = dynValue.InvalidProperty;  // Crashes at runtime!
```

### Dynamic Method Calls

```csharp
dynamic obj = new Calculator();
var result = obj.Add(5, 3);  // Method call resolved at runtime

dynamic dynObj = "Hello";
var upper = dynObj.ToUpper();  // String method works

// But no IntelliSense! Risky!
```

---

## 🔍 DynamicObject - Create Custom Dynamic Types

```csharp
public class DynamicDictionary : DynamicObject
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

// Usage
dynamic dict = new DynamicDictionary();
dict.Name = "Alice";
dict.Age = 30;

Console.WriteLine($"{dict.Name} is {dict.Age}");  // Alice is 30
```

---

## 🔗 COM Interop - Office/Automation

```csharp
// Classic COM Interop pattern
dynamic excelApp = Activator.CreateInstance(Type.GetTypeFromProgID("Excel.Application"));

excelApp.Visible = true;
var workbook = excelApp.Workbooks.Add();
var worksheet = workbook.Sheets[1];

worksheet.Cells[1, 1] = "Hello Excel";
worksheet.Cells[1, 1].Font.Bold = true;

workbook.Save();
excelApp.Quit();

// Without dynamic - MASSIVE boilerplate:
// ((Range)worksheet.Cells[1, 1]).Value = "Hello";
// ((Font)((Range)worksheet.Cells[1, 1]).Font).Bold = true;
```

---

## 🎯 When to Use Each

| Technika | Użyteczna gdy | Performance |
|----------|---------------|-------------|
| Direct Call | typ znany compile-time | ⚡⚡⚡ FASTEST |
| Expression Tree | potrzebna performance + dynamizm | ⚡⚡ FAST (after compile) |
| Reflection | discovery, metadane | 🐢 SLOW |
| Dynamic | COM interop, ExpandoObject | 🐌 MEDIUM |

---

## 📊 Benchmarking

```csharp
using BenchmarkDotNet.Attributes;
using System.Reflection;
using System.Linq.Expressions;

[MemoryDiagnoser]
public class ReflectionBenchmark
{
    private Calculator _calc = new();
    private MethodInfo _method;
    private Func<int, int, int> _compiled;
    
    [GlobalSetup]
    public void Setup()
    {
        _method = typeof(Calculator).GetMethod("Add");
        
        // Compile expression
        var p1 = Expression.Parameter(typeof(int));
        var p2 = Expression.Parameter(typeof(int));
        var call = Expression.Call(Expression.Constant(_calc), _method, p1, p2);
        var lambda = Expression.Lambda<Func<int, int, int>>(call, p1, p2);
        _compiled = lambda.Compile();
    }
    
    [Benchmark]
    public int DirectCall() => _calc.Add(5, 3);
    
    [Benchmark]
    public object ReflectionCall() => _method.Invoke(_calc, new object[] { 5, 3 });
    
    [Benchmark]
    public int CompiledExpression() => _compiled(5, 3);
}

// Results:
// DirectCall: 10 ns (baseline)
// CompiledExpression: 20 ns (2x slower)
// ReflectionCall: 200 ns (20x slower)
```

---

## 🎨 Visitor Pattern with Expression Trees

```csharp
// Traverse and analyze expression
public class ExpressionPrinter : ExpressionVisitor
{
    public override Expression Visit(Expression node)
    {
        if (node is BinaryExpression binary)
        {
            Console.WriteLine($"Binary: {binary.NodeType}");
            Visit(binary.Left);
            Visit(binary.Right);
        }
        else if (node is ConstantExpression constant)
        {
            Console.WriteLine($"Constant: {constant.Value}");
        }
        
        return base.Visit(node);
    }
}

// Usage
var expr = (Expression<Func<int, int, int>>)((a, b) => (a * 2) + (b * 3));
var printer = new ExpressionPrinter();
printer.Visit(expr);
```

---

## 📚 Summary

**Advanced Reflection Techniques:**

- **Expression Trees** = Kompiluj kod dynamicznie w runtime
- **Dynamic** = Runtime type checking (risky!)
- **DynamicObject** = Utwórz custom dynamic types
- **COM Interop** = Office/Legacy system integration
- **Benchmarking** = Mierz performance
- **Expression Visitor** = Analizuj expression tree

**Performance Hierarchy:**
1. Direct call ~10ns (best)
2. Compiled expression ~20ns (good)
3. Dynamic ~100ns (medium)
4. Reflection ~200ns (slowest)

---

## 🎯 Następny Temat

Temat 10: Performance & Best Practices - Caching, AOT, Source Generators, Real-world ORM
