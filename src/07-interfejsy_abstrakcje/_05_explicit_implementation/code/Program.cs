using System;
using System.Collections.Generic;
using Xunit;

namespace ExplicitImplementation;

// ============================================================================
// CZĘŚĆ 1: Implicit vs Explicit Implementation
// ============================================================================

public interface IShapable
{
    void Draw();
    string Name { get; }
}

public interface IAnimatable
{
    void Draw();  // ⚠️ Same method name as IShapable!
    void Animate();
}

/// <summary>
/// ✅ Implicit Implementation - methods are accessible directly
/// Only possible if method signatures don't conflict
/// </summary>
public class SimpleShape : IShapable
{
    public string Name => "Simple Shape";
    
    public void Draw()
    {
        Console.WriteLine("Drawing simple shape");
    }
}

/// <summary>
/// ❌ Problem: Two interfaces with same method name (Draw)
/// Solution: Use EXPLICIT implementation for one (or both)
/// </summary>
public class AnimatedShape : IShapable, IAnimatable
{
    public string Name => "Animated Shape";
    
    // ✅ Implicit - for IShapable
    public void Draw()
    {
        Console.WriteLine("Drawing shape");
    }
    
    // ✅ Explicit - for IAnimatable (qualified name: IAnimatable.Draw)
    void IAnimatable.Draw()
    {
        Console.WriteLine("Animating shape");
    }
    
    public void Animate()
    {
        Console.WriteLine("Animating...");
    }
}

// ============================================================================
// CZĘŚĆ 2: Accessing Explicit Implementation
// ============================================================================

/// <summary>
/// Explicit implementation is ONLY accessible through interface type
/// </summary>
public class PaymentMethod : IShapable, IAnimatable
{
    public string Name => "Payment";
    
    public void Draw()  // IShapable.Draw - implicit
    {
        Console.WriteLine("[Payment] Drawing payment UI");
    }
    
    void IAnimatable.Draw()  // IAnimatable.Draw - explicit
    {
        Console.WriteLine("[Animation] Drawing payment animation");
    }
    
    public void Animate()
    {
        Console.WriteLine("[Animation] Animating payment process");
    }
}

// ============================================================================
// CZĘŚĆ 3: Hiding Methods - Explicit for Privacy
// ============================================================================

public interface IPublicApi
{
    void PublicMethod();
    void UtilityMethod();
}

public interface IInternalApi
{
    void UtilityMethod();
}

/// <summary>
/// Use explicit implementation to hide internal methods
/// </summary>
public class ApiImpl : IPublicApi, IInternalApi
{
    // ✅ Public - accessible directly
    public void PublicMethod()
    {
        Console.WriteLine("Public API method");
    }
    
    // ✅ Explicit for IPublicApi - hidden from direct access
    void IPublicApi.UtilityMethod()
    {
        Console.WriteLine("Utility (hidden) - for IPublicApi");
    }
    
    // ✅ Explicit for IInternalApi - hidden from direct access
    void IInternalApi.UtilityMethod()
    {
        Console.WriteLine("Utility (hidden) - for IInternalApi");
    }
}

// ============================================================================
// CZĘŚĆ 4: Real-World Example - Repository Pattern
// ============================================================================

public interface IReadOnlyRepository
{
    T? GetById<T>(int id) where T : class;
    List<T> GetAll<T>() where T : class;
}

public interface IWriteRepository
{
    void Add<T>(T item) where T : class;
    void Delete<T>(T item) where T : class;
}

/// <summary>
/// Separate read and write operations
/// Some callers might only need read access
/// </summary>
public class FullRepository : IReadOnlyRepository, IWriteRepository
{
    private List<object> _data = new();
    
    // ✅ Read operations - implicit (public API)
    public T? GetById<T>(int id) where T : class
    {
        Console.WriteLine($"Getting by ID: {id}");
        return null;
    }
    
    public List<T> GetAll<T>() where T : class
    {
        Console.WriteLine("Getting all items");
        return new();
    }
    
    // ✅ Write operations - implicit (public API)
    public void Add<T>(T item) where T : class
    {
        _data.Add(item);
        Console.WriteLine($"Added item of type {typeof(T).Name}");
    }
    
    public void Delete<T>(T item) where T : class
    {
        _data.Remove(item);
        Console.WriteLine($"Deleted item of type {typeof(T).Name}");
    }
}

// ============================================================================
// CZĘŚĆ 5: Different Implementations Per Interface
// ============================================================================

public interface IValidator
{
    bool Validate();
    void ShowError(string error);
}

public interface ILogger
{
    void Log(string message);
    void ShowError(string error);  // ⚠️ Same name!
}

/// <summary>
/// Different behavior for ShowError depending on which interface is used
/// </summary>
public class ValidationService : IValidator, ILogger
{
    // ✅ Different implementations for same method name
    
    // IValidator.ShowError
    void IValidator.ShowError(string error)
    {
        Console.WriteLine($"[VALIDATION ERROR] {error}");
    }
    
    // ILogger.ShowError
    void ILogger.ShowError(string error)
    {
        Console.WriteLine($"[LOG ERROR] {error}");
    }
    
    public bool Validate()
    {
        Console.WriteLine("Validating...");
        return true;
    }
    
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

// ============================================================================
// CZĘŚĆ 6: Accessing Explicit Members
// ============================================================================

public class ExplicitAccessDemo
{
    public static void Demo()
    {
        var shape = new AnimatedShape();
        var payment = new PaymentMethod();
        var service = new ValidationService();
        
        // ✅ Can access implicit methods directly
        shape.Draw();  // Uses IShapable.Draw
        
        // ❌ Cannot access explicit methods directly
        // shape.IAnimatable.Draw();  // Compiler error - can't use dot notation
        
        // ✅ Must cast to interface type to access explicit methods
        ((IAnimatable)shape).Draw();  // Calls IAnimatable.Draw
        
        Console.WriteLine();
        
        // ✅ Different interface types show different methods
        IShapable shapable = payment;
        IAnimatable animatable = payment;
        
        shapable.Draw();           // Calls implicit IShapable.Draw
        animatable.Draw();         // Calls explicit IAnimatable.Draw
        
        Console.WriteLine();
        
        // ✅ Interface-based access shows appropriate methods
        IValidator validator = service;
        ILogger logger = service;
        
        validator.Validate();
        ((IValidator)service).ShowError("Validation failed");
        ((ILogger)service).ShowError("Log error occurred");
    }
}

// ============================================================================
// TESTS
// ============================================================================

public class ExplicitImplementationTests
{
    [Fact]
    public void ExplicitImplementationIsNotDirectlyAccessible()
    {
        var shape = new AnimatedShape();
        
        // ✅ Can call implicit method
        shape.Draw();
        
        // ❌ Cannot call explicit method directly
        // shape.IAnimatable.Draw();  // Compiler error
        
        // ✅ Must cast to interface type
        var animatable = (IAnimatable)shape;
        animatable.Draw();  // Calls explicit IAnimatable.Draw
        
        Assert.NotNull(shape);
    }
    
    [Fact]
    public void SameMethodNameMultipleInterfaces()
    {
        var payment = new PaymentMethod();
        
        // Implicit - IShapable.Draw
        payment.Draw();
        
        // Explicit - IAnimatable.Draw
        IAnimatable animatable = payment;
        animatable.Draw();
        
        Assert.NotNull(payment);
    }
    
    [Fact]
    public void ExplicitForHiding()
    {
        var api = new ApiImpl();
        
        // ✅ Can access public method
        api.PublicMethod();
        
        // ❌ Cannot access UtilityMethod directly
        // api.UtilityMethod();  // Compiler error
        
        // ✅ Only accessible through interface
        ((IPublicApi)api).UtilityMethod();
        ((IInternalApi)api).UtilityMethod();
        
        Assert.NotNull(api);
    }
    
    [Fact]
    public void DifferentImplementationsPerInterface()
    {
        var service = new ValidationService();
        
        // ✅ Different implementations for same method name
        IValidator validator = service;
        ILogger logger = service;
        
        validator.Validate();
        validator.ShowError("Validation failed");  // [VALIDATION ERROR]
        
        logger.Log("Message");
        logger.ShowError("Error");  // [LOG ERROR]
        
        Assert.NotNull(service);
    }
    
    [Fact]
    public void CastingToAccessExplicitMembers()
    {
        var shape = new AnimatedShape();
        
        IShapable s = shape;
        IAnimatable a = shape;
        
        s.Draw();  // IShapable version
        a.Draw();  // IAnimatable version
        
        Assert.Equal("Animated Shape", shape.Name);
    }
    
    [Fact]
    public void RepositoryPattern()
    {
        var repo = new FullRepository();
        
        // Use as read-only
        IReadOnlyRepository readOnly = repo;
        readOnly.GetAll<string>();
        
        // Use as write-enabled
        IWriteRepository writeOnly = repo;
        writeOnly.Add<string>("item");
        
        // Use as full
        var full = repo;
        full.Add<string>("item2");
        full.GetAll<string>();
        
        Assert.NotNull(repo);
    }
}
