using System;
using Xunit;

namespace DesignPatterns;

// ============ SINGLETON - Tworzenie ============
public class Logger
{
    private static Logger? instance = null;
    
    private Logger() { }
    
    public static Logger GetInstance()
    {
        instance ??= new Logger();
        return instance;
    }
    
    public void Log(string msg) => Console.WriteLine($"[LOG] {msg}");
}

// ============ FACTORY - Tworzenie ============
public abstract class Shape
{
    public abstract void Draw();
}

public class Circle : Shape
{
    public override void Draw() => Console.WriteLine("Drawing circle");
}

public class Square : Shape
{
    public override void Draw() => Console.WriteLine("Drawing square");
}

public class ShapeFactory
{
    public static Shape CreateShape(string type)
    {
        return type.ToLower() switch
        {
            "circle" => new Circle(),
            "square" => new Square(),
            _ => throw new ArgumentException("Unknown shape")
        };
    }
}

// ============ DECORATOR - Struktura ============
public interface IComponent
{
    void Operation();
}

public class ConcreteComponent : IComponent
{
    public void Operation() => Console.WriteLine("Basic operation");
}

public class Decorator : IComponent
{
    protected IComponent component;
    
    public Decorator(IComponent component) => this.component = component;
    
    public virtual void Operation() => component.Operation();
}

public class DecoratorA : Decorator
{
    public DecoratorA(IComponent component) : base(component) { }
    
    public override void Operation()
    {
        base.Operation();
        Console.WriteLine("  + Feature A");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 7: WZORCE PROJEKTOWE ===\n");
        
        Console.WriteLine("1. SINGLETON:");
        var log1 = Logger.GetInstance();
        var log2 = Logger.GetInstance();
        log1.Log("Same instance? " + ReferenceEquals(log1, log2));
        Console.WriteLine();
        
        Console.WriteLine("2. FACTORY:");
        var circle = ShapeFactory.CreateShape("circle");
        var square = ShapeFactory.CreateShape("square");
        circle.Draw();
        square.Draw();
        Console.WriteLine();
        
        Console.WriteLine("3. DECORATOR:");
        IComponent comp = new ConcreteComponent();
        var decorated = new DecoratorA(comp);
        decorated.Operation();
    }
}

public class DesignPatternTests
{
    [Fact]
    public void Singleton_ReturnsSameInstance()
    {
        var log1 = Logger.GetInstance();
        var log2 = Logger.GetInstance();
        
        Assert.True(ReferenceEquals(log1, log2));
    }
    
    [Fact]
    public void Factory_CreatesCorrectShape()
    {
        var circle = ShapeFactory.CreateShape("circle");
        var square = ShapeFactory.CreateShape("square");
        
        Assert.IsType<Circle>(circle);
        Assert.IsType<Square>(square);
    }
}
