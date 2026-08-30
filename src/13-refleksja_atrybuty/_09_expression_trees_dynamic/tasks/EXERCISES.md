# Ćwiczenia: Expression Trees & Dynamic

## 🟢 Basic Level

### Zadanie 1: Simple Expression Tree
Buduj proste wyrażenie x + 1:

```csharp
// TODO: Build x => x + 1
```

**Rozwiązanie:**
```csharp
var param = Expression.Parameter(typeof(int), "x");
var constant = Expression.Constant(1);
var body = Expression.Add(param, constant);
var lambda = Expression.Lambda<Func<int, int>>(body, param);
var compiled = lambda.Compile();
var result = compiled(10);  // 11
```

---

### Zadanie 2: Compile Expression
Kompiluj wyrażenie do delegate:

```csharp
// TODO: Compile lambda expression
```

**Rozwiązanie:**
```csharp
var compiled = lambda.Compile();
var result = compiled(arg);
```

---

### Zadanie 3: Dynamic Variable
Utwórz zmienną dynamic:

```csharp
// TODO: Create dynamic string and call methods
```

**Rozwiązanie:**
```csharp
dynamic str = "Hello";
var upper = str.ToUpper();  // "HELLO"
var length = str.Length;     // 5
```

---

### Zadanie 4: DynamicObject Basics
Utwórz prosty DynamicObject:

```csharp
public class SimpleDynamic : DynamicObject
{
    // TODO: Override TryGetMember, TrySetMember
}
```

**Rozwiązanie:**
```csharp
public class SimpleDynamic : DynamicObject
{
    private Dictionary<string, object> _data = new();
    
    public override bool TryGetMember(GetMemberBinder binder, out object result)
        => _data.TryGetValue(binder.Name, out result);
    
    public override bool TrySetMember(SetMemberBinder binder, object value)
    {
        _data[binder.Name] = value;
        return true;
    }
}
```

---

### Zadanie 5: Expression with Two Parameters
Buduj: (a, b) => a + b:

```csharp
// TODO: Build addition expression with two parameters
```

**Rozwiązanie:**
```csharp
var paramA = Expression.Parameter(typeof(int), "a");
var paramB = Expression.Parameter(typeof(int), "b");
var body = Expression.Add(paramA, paramB);
var lambda = Expression.Lambda<Func<int, int, int>>(body, paramA, paramB);
var compiled = lambda.Compile();
```

---

## 🟡 Intermediate Level

### Zadanie 6: Multiply Expression
Buduj: x => x * 2:

```csharp
// TODO: Build multiplication expression
```

**Rozwiązanie:**
```csharp
var param = Expression.Parameter(typeof(int), "x");
var constant = Expression.Constant(2);
var body = Expression.Multiply(param, constant);
var lambda = Expression.Lambda<Func<int, int>>(body, param);
var compiled = lambda.Compile();
```

---

### Zadanie 7: Complex Expression
Buduj: (a, b) => (a * 2) + (b * 3):

```csharp
// TODO: Build complex mathematical expression
```

**Rozwiązanie:**
```csharp
var paramA = Expression.Parameter(typeof(int), "a");
var paramB = Expression.Parameter(typeof(int), "b");

var mulA = Expression.Multiply(paramA, Expression.Constant(2));
var mulB = Expression.Multiply(paramB, Expression.Constant(3));
var body = Expression.Add(mulA, mulB);

var lambda = Expression.Lambda<Func<int, int, int>>(body, paramA, paramB);
var compiled = lambda.Compile();
```

---

### Zadanie 8: Method Call via Expression
Wyrażenie dla metody:

```csharp
public class Calc { public int Add(int a, int b) => a + b; }

// TODO: Build expression for Calc.Add
```

**Rozwiązanie:**
```csharp
var calc = new Calc();
var method = typeof(Calc).GetMethod("Add");
var paramA = Expression.Parameter(typeof(int), "a");
var paramB = Expression.Parameter(typeof(int), "b");

var callExpr = Expression.Call(
    Expression.Constant(calc),
    method,
    paramA,
    paramB
);

var lambda = Expression.Lambda<Func<int, int, int>>(callExpr, paramA, paramB);
var compiled = lambda.Compile();
```

---

### Zadanie 9: Dynamic Method Invocation
Wywołaj metodę dynamicznie:

```csharp
dynamic obj = new Calc();

// TODO: Call Add method dynamically
```

**Rozwiązanie:**
```csharp
dynamic result = obj.Add(5, 3);  // 8
```

---

### Zadanie 10: DynamicObject with Properties
Utwórz dynamic dictionary:

```csharp
dynamic dict = new DynamicDict();

// TODO: Set and get properties
```

**Rozwiązanie:**
```csharp
dict.Name = "Alice";
dict.Age = 30;

Console.WriteLine(dict.Name);  // Alice
Console.WriteLine(dict.Age);   // 30
```

---

## 🔴 Advanced Level

### Zadanie 11: Expression Performance Comparison
Porównaj performance metod:

```csharp
// TODO: Benchmark direct call vs expression vs reflection
```

**Rozwiązanie:**
```csharp
var calc = new Calc();

// Direct - fastest
var direct = calc.Add(5, 3);

// Reflection - slowest
var method = typeof(Calc).GetMethod("Add");
var reflected = method.Invoke(calc, new object[] { 5, 3 });

// Expression - compiled is fast
var param1 = Expression.Parameter(typeof(int));
var param2 = Expression.Parameter(typeof(int));
var callExpr = Expression.Call(Expression.Constant(calc), method, param1, param2);
var lambda = Expression.Lambda<Func<int, int, int>>(callExpr, param1, param2);
var compiled = lambda.Compile();
var expressed = compiled(5, 3);

// Measurements show:
// Direct: 10ns
// Compiled: 20ns
// Dynamic: 100ns
// Reflection: 200ns
```

---

### Zadanie 12: Custom Expression Visitor
Analizuj strukturę wyrażenia:

```csharp
public class ExpressionAnalyzer : ExpressionVisitor
{
    // TODO: Override Visit methods to analyze expression
}
```

**Rozwiązanie:**
```csharp
public class ExpressionAnalyzer : ExpressionVisitor
{
    public void Analyze(Expression expr)
    {
        Console.WriteLine($"Expression type: {expr.GetType().Name}");
        Visit(expr);
    }
    
    public override Expression Visit(Expression node)
    {
        if (node is BinaryExpression binary)
        {
            Console.WriteLine($"  Binary: {binary.NodeType}");
            Visit(binary.Left);
            Visit(binary.Right);
        }
        else if (node is ConstantExpression constant)
        {
            Console.WriteLine($"  Constant: {constant.Value}");
        }
        else if (node is ParameterExpression param)
        {
            Console.WriteLine($"  Parameter: {param.Name}");
        }
        
        return base.Visit(node);
    }
}
```

---

## 📊 Wskazówki

- ✅ Expression trees dla complex dynamic compilation
- ✅ Zawsze Compile() przed użyciem
- ✅ Cache compiled expressions dla performance
- ✅ Dynamic dla COM interop
- ✅ DynamicObject dla custom behavior
- ❌ Nie ignoruj performance - measure z benchmarks
- ❌ Nie mieszaj dynamic z type safety
- ❌ Nie zapomnij null checks na dynamic

---

## 🎯 Key Takeaways

Expression Trees & Dynamic:

```
Expression<Func<...>> = Runtime code representation
Compile() = Convert to delegate
Dynamic = Runtime type resolution (no IntelliSense)
DynamicObject = Override member access
Performance: Direct < Compiled Expr < Dynamic < Reflection

Use when:
- Expression: Compile-time + runtime flexibility
- Dynamic: COM interop, ExpandoObject
- Reflection: Discovery, metadata
```

Pamiętaj: **Kompiluj wyrażenia, nie interpretuj ich!**
