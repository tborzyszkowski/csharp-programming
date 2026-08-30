using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ObjectMethods;

// ============================================================================
// CZĘŚĆ 1: ToString()
// ============================================================================

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // ❌ Bez override - zwraca "ObjectMethods.Person"
    
    // ✅ Z override - user-friendly output
    public override string ToString()
    {
        return $"Person: {Name}, Age {Age}";
    }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public override string ToString()
    {
        return $"[{Id}] {Name} - {Price:C}";
    }
}

public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    
    public override string ToString()
    {
        return $"{Street}, {City} {ZipCode}";
    }
}

// ============================================================================
// CZĘŚĆ 2: Equals() bez przesłaniania (Reference Equality)
// ============================================================================

public class SimpleUser
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Bez override - domyślnie porównuje referencje
}

// ============================================================================
// CZĘŚĆ 3: Equals() z przesłanianiem (Value Equality)
// ============================================================================

public class User : IEquatable<User>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    // ✅ Porównaj wartości, nie referencje
    public override bool Equals(object? obj)
    {
        return Equals(obj as User);
    }
    
    public bool Equals(User? other)
    {
        return other != null &&
               Id == other.Id &&
               Name == other.Name &&
               Email == other.Email;
    }
    
    // ✅ GetHashCode razem z Equals
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Email);
    }
    
    public override string ToString()
    {
        return $"User#{Id}: {Name} ({Email})";
    }
}

// ============================================================================
// CZĘŚĆ 4: GetHashCode() - Dla Kolekcji
// ============================================================================

public class CartItem : IEquatable<CartItem>
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    
    public override bool Equals(object? obj)
    {
        return Equals(obj as CartItem);
    }
    
    public bool Equals(CartItem? other)
    {
        // Uważamy dwa CartItem równe jeśli mają ten sam ProductId
        return other != null && ProductId == other.ProductId;
    }
    
    public override int GetHashCode()
    {
        return ProductId.GetHashCode();
    }
    
    public override string ToString()
    {
        return $"CartItem#{ProductId}: {ProductName} x{Quantity}";
    }
}

// ============================================================================
// CZĘŚĆ 5: Real-World - Comparison Methods
// ============================================================================

public class Employee : IEquatable<Employee>
{
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }
    
    public override bool Equals(object? obj)
    {
        return Equals(obj as Employee);
    }
    
    public bool Equals(Employee? other)
    {
        return other != null && EmployeeId == other.EmployeeId;
    }
    
    public override int GetHashCode()
    {
        return EmployeeId.GetHashCode();
    }
    
    public override string ToString()
    {
        return $"[{EmployeeId}] {Name} - {Department} (${Salary:F2})";
    }
}

// ============================================================================
// PROGRAM GŁÓWNY I TESTY
// ============================================================================

public class Program
{
    public static void Main()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  TEMAT 4: METODY KLASY OBJECT                             ║");
        Console.WriteLine("║  ToString, Equals, GetHashCode                            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");
        
        // ====================================================================
        Console.WriteLine("SCENARIUSZ 1: ToString() - Reprezentacja Tekstowa\n");
        
        var person = new Person { Name = "Alice", Age = 28 };
        Console.WriteLine($"Person: {person}");
        Console.WriteLine($"Person.ToString(): {person.ToString()}");
        
        Console.WriteLine("\n--- Kolekcja ---");
        var people = new List<Person>
        {
            new() { Name = "Bob", Age = 35 },
            new() { Name = "Carol", Age = 42 },
            new() { Name = "David", Age = 28 }
        };
        
        foreach (var p in people)
        {
            Console.WriteLine($"  {p}");  // Automatycznie wołuje ToString()
        }
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 2: Equals() - Porównanie Równości\n");
        
        Console.WriteLine("--- WITHOUT Override (Reference Equality) ---");
        var user1 = new SimpleUser { Id = 1, Name = "John" };
        var user2 = new SimpleUser { Id = 1, Name = "John" };
        var user3 = user1;
        
        Console.WriteLine($"user1 == user2: {user1 == user2}");  // false (different refs)
        Console.WriteLine($"user1 == user3: {user1 == user3}");  // true (same ref)
        Console.WriteLine($"user1.Equals(user2): {user1.Equals(user2)}");  // false
        
        Console.WriteLine("\n--- WITH Override (Value Equality) ---");
        var userA = new User { Id = 1, Name = "John", Email = "john@example.com" };
        var userB = new User { Id = 1, Name = "John", Email = "john@example.com" };
        var userC = new User { Id = 2, Name = "Jane", Email = "jane@example.com" };
        
        Console.WriteLine($"userA.Equals(userB): {userA.Equals(userB)}");  // true (same values)
        Console.WriteLine($"userA.Equals(userC): {userA.Equals(userC)}");  // false (different ID)
        Console.WriteLine($"userA == userB: {userA == userB}");  // false (reference equality still)
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 3: GetHashCode() - Dla Kolekcji\n");
        
        Console.WriteLine("--- HashSet bez GetHashCode Override ---");
        var simpleUsers = new HashSet<SimpleUser>
        {
            new() { Id = 1, Name = "User1" },
            new() { Id = 1, Name = "User1" }  // Duplikat, ale...
        };
        Console.WriteLine($"Count (bez override): {simpleUsers.Count}");  // 2 (oba się dodały!)
        
        Console.WriteLine("\n--- HashSet z GetHashCode Override ---");
        var cart = new HashSet<CartItem>
        {
            new() { ProductId = 1, ProductName = "Laptop", Quantity = 1 },
            new() { ProductId = 1, ProductName = "Laptop", Quantity = 2 }  // Duplikat?
        };
        Console.WriteLine($"Count (z override): {cart.Count}");  // 1 (duplikat usunięty!)
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 4: GetHashCode() Consistency\n");
        
        var userX = new User { Id = 1, Name = "X", Email = "x@test.com" };
        var userY = new User { Id = 1, Name = "X", Email = "x@test.com" };
        
        Console.WriteLine($"userX.Equals(userY): {userX.Equals(userY)}");
        Console.WriteLine($"userX.GetHashCode(): {userX.GetHashCode()}");
        Console.WriteLine($"userY.GetHashCode(): {userY.GetHashCode()}");
        Console.WriteLine($"Hashes equal: {userX.GetHashCode() == userY.GetHashCode()}");  // Must be true!
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 5: Real-World - Employee System\n");
        
        var employees = new HashSet<Employee>
        {
            new() { EmployeeId = 1, Name = "Alice", Department = "IT", Salary = 5000 },
            new() { EmployeeId = 2, Name = "Bob", Department = "HR", Salary = 4500 },
            new() { EmployeeId = 1, Name = "Alice", Department = "IT", Salary = 5500 }  // Duplikat ID
        };
        
        Console.WriteLine($"Unique employees: {employees.Count}");
        foreach (var emp in employees)
        {
            Console.WriteLine($"  {emp}");
        }
        
        // ====================================================================
        Console.WriteLine("\n\nSCENARIUSZ 6: Dictionary z Custom Objects\n");
        
        var userDict = new Dictionary<User, string>
        {
            { new User { Id = 1, Name = "John", Email = "john@test.com" }, "Active" },
            { new User { Id = 2, Name = "Jane", Email = "jane@test.com" }, "Inactive" }
        };
        
        Console.WriteLine($"Dictionary entries: {userDict.Count}");
        foreach (var kvp in userDict)
        {
            Console.WriteLine($"  {kvp.Key} → {kvp.Value}");
        }
        
        Console.WriteLine("\n✅ Demonstracja ukończona");
    }
}

// ============================================================================
// TESTY XUNIT
// ============================================================================

public class ObjectMethodsTests
{
    [Fact]
    public void ToString_Person_ReturnsFormattedString()
    {
        var person = new Person { Name = "John", Age = 30 };
        var str = person.ToString();
        
        Assert.Contains("John", str);
        Assert.Contains("30", str);
    }
    
    [Fact]
    public void ToString_Product_ReturnsIdNamePrice()
    {
        var product = new Product { Id = 1, Name = "Laptop", Price = 999.99m };
        var str = product.ToString();
        
        Assert.Contains("1", str);
        Assert.Contains("Laptop", str);
    }
    
    [Fact]
    public void Equals_SimpleUser_ComparesReferences()
    {
        var user1 = new SimpleUser { Id = 1, Name = "John" };
        var user2 = new SimpleUser { Id = 1, Name = "John" };
        
        // Without override, Equals compares references
        Assert.NotEqual(user1, user2);
    }
    
    [Fact]
    public void Equals_User_ComparesValues()
    {
        var user1 = new User { Id = 1, Name = "John", Email = "john@test.com" };
        var user2 = new User { Id = 1, Name = "John", Email = "john@test.com" };
        
        // With override, Equals compares values
        Assert.Equal(user1, user2);
    }
    
    [Fact]
    public void GetHashCode_SameValues_SameHash()
    {
        var user1 = new User { Id = 1, Name = "John", Email = "john@test.com" };
        var user2 = new User { Id = 1, Name = "John", Email = "john@test.com" };
        
        Assert.Equal(user1.GetHashCode(), user2.GetHashCode());
    }
    
    [Fact]
    public void GetHashCode_DifferentValues_DifferentHash()
    {
        var user1 = new User { Id = 1, Name = "John", Email = "john@test.com" };
        var user2 = new User { Id = 2, Name = "Jane", Email = "jane@test.com" };
        
        Assert.NotEqual(user1.GetHashCode(), user2.GetHashCode());
    }
    
    [Fact]
    public void HashSet_WithGetHashCode_RemovesDuplicates()
    {
        var items = new HashSet<CartItem>
        {
            new() { ProductId = 1, ProductName = "Laptop", Quantity = 1 },
            new() { ProductId = 1, ProductName = "Laptop", Quantity = 2 },
            new() { ProductId = 2, ProductName = "Mouse", Quantity = 1 }
        };
        
        // Should have 2 items (duplicate ProductId=1 removed)
        Assert.Equal(2, items.Count);
    }
    
    [Fact]
    public void Dictionary_WithCustomKey_Works()
    {
        var dict = new Dictionary<User, string>
        {
            { new User { Id = 1, Name = "John", Email = "john@test.com" }, "Status1" }
        };
        
        var key = new User { Id = 1, Name = "John", Email = "john@test.com" };
        
        // Should find the value because Equals and GetHashCode work
        Assert.True(dict.TryGetValue(key, out var status));
        Assert.Equal("Status1", status);
    }
}
