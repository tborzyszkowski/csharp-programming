using System;
using System.Collections.Generic;

namespace RealWorldExample;

/// <summary>
/// Vector3D - Complete system demonstrating operator overloading
/// </summary>
public record Vector3D(double X, double Y, double Z)
{
    // ==================== UNARY OPERATORS ====================
    
    /// Unary plus
    public static Vector3D operator +(Vector3D v) => v;
    
    /// Unary minus (negation)
    public static Vector3D operator -(Vector3D v) 
        => new(-v.X, -v.Y, -v.Z);
    
    /// Logical NOT - true if vector is zero
    public static bool operator !(Vector3D v)
        => Math.Abs(v.Magnitude) < double.Epsilon;

    // ==================== BINARY ARITHMETIC ====================
    
    /// Vector addition
    public static Vector3D operator +(Vector3D a, Vector3D b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    
    /// Vector subtraction
    public static Vector3D operator -(Vector3D a, Vector3D b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    
    /// Scalar multiplication
    public static Vector3D operator *(Vector3D v, double scalar)
        => new(v.X * scalar, v.Y * scalar, v.Z * scalar);
    
    public static Vector3D operator *(double scalar, Vector3D v)
        => v * scalar;
    
    /// Scalar division
    public static Vector3D operator /(Vector3D v, double scalar)
    {
        if (Math.Abs(scalar) < double.Epsilon)
            throw new ArgumentException("Cannot divide by zero");
        return new(v.X / scalar, v.Y / scalar, v.Z / scalar);
    }

    // ==================== SPECIAL OPERATORS ====================
    
    /// Dot product (using | operator)
    public static double operator |(Vector3D a, Vector3D b)
        => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    
    /// Cross product (using & operator)
    public static Vector3D operator &(Vector3D a, Vector3D b)
        => new(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );

    // ==================== RELATIONAL ====================
    
    /// Equality
    public static bool operator ==(Vector3D a, Vector3D b)
        => Math.Abs(a.X - b.X) < double.Epsilon &&
           Math.Abs(a.Y - b.Y) < double.Epsilon &&
           Math.Abs(a.Z - b.Z) < double.Epsilon;
    
    /// Inequality
    public static bool operator !=(Vector3D a, Vector3D b)
        => !(a == b);
    
    /// Less than (by magnitude)
    public static bool operator <(Vector3D a, Vector3D b)
        => a.Magnitude < b.Magnitude;
    
    /// Greater than
    public static bool operator >(Vector3D a, Vector3D b)
        => a.Magnitude > b.Magnitude;
    
    /// Less than or equal
    public static bool operator <=(Vector3D a, Vector3D b)
        => a.Magnitude <= b.Magnitude;
    
    /// Greater than or equal
    public static bool operator >=(Vector3D a, Vector3D b)
        => a.Magnitude >= b.Magnitude;

    // ==================== CONVERSIONS ====================
    
    /// Implicit conversion from tuple
    public static implicit operator Vector3D((double x, double y, double z) t)
        => new(t.x, t.y, t.z);
    
    /// Explicit conversion to array
    public static explicit operator double[](Vector3D v)
        => new[] { v.X, v.Y, v.Z };
    
    /// Explicit conversion to list
    public static explicit operator List<double>(Vector3D v)
        => new List<double> { v.X, v.Y, v.Z };

    // ==================== PROPERTIES ====================
    
    /// Magnitude (length) of vector
    public double Magnitude => Math.Sqrt(X*X + Y*Y + Z*Z);
    
    /// Unit vector in same direction
    public Vector3D Normalized
    {
        get
        {
            var mag = Magnitude;
            if (mag < double.Epsilon) return new(0, 0, 0);
            return this / mag;
        }
    }

    public override string ToString() 
        => $"[{X:F2}, {Y:F2}, {Z:F2}] (|v| = {Magnitude:F2})";
}

// ==================== MAIN PROGRAM ====================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   REAL-WORLD EXAMPLE: VECTOR3D SYSTEM                     ║");
        Console.WriteLine("║   Complete Operator Overloading Demonstration             ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");

        // ===== UNARY OPERATORS =====
        Console.WriteLine("\n=== UNARY OPERATORS ===");
        
        var v1 = new Vector3D(1, 2, 3);
        Console.WriteLine($"  v1 = {v1}");
        
        var v2 = +v1;
        Console.WriteLine($"  +v1 = {v2}");
        
        var v3 = -v1;
        Console.WriteLine($"  -v1 = {v3}");
        
        var zero = new Vector3D(0, 0, 0);
        Console.WriteLine($"  !zero (is zero?) = {!zero}");
        Console.WriteLine($"  !v1 (is zero?) = {!v1}");

        // ===== BINARY ARITHMETIC =====
        Console.WriteLine("\n=== BINARY ARITHMETIC ===");
        
        var u1 = new Vector3D(1, 0, 0);
        var u2 = new Vector3D(0, 1, 0);
        
        Console.WriteLine($"  u1 = {u1}");
        Console.WriteLine($"  u2 = {u2}");
        
        var sum = u1 + u2;
        Console.WriteLine($"  u1 + u2 = {sum}");
        
        var diff = u1 - u2;
        Console.WriteLine($"  u1 - u2 = {diff}");
        
        var scaled = u1 * 3.5;
        Console.WriteLine($"  u1 * 3.5 = {scaled}");
        
        var divided = scaled / 2.0;
        Console.WriteLine($"  scaled / 2.0 = {divided}");

        // ===== SPECIAL OPERATORS =====
        Console.WriteLine("\n=== SPECIAL OPERATORS ===");
        
        var a = new Vector3D(1, 0, 0);
        var b = new Vector3D(0, 1, 0);
        
        double dot = a | b;  // Dot product
        Console.WriteLine($"  Dot product (a | b) = {dot}");
        
        var cross = a & b;   // Cross product
        Console.WriteLine($"  Cross product (a & b) = {cross}");

        // ===== RELATIONAL =====
        Console.WriteLine("\n=== RELATIONAL OPERATORS ===");
        
        var v4 = new Vector3D(3, 4, 0);      // Magnitude = 5
        var v5 = new Vector3D(1, 0, 0);      // Magnitude = 1
        var v6 = new Vector3D(3, 4, 0);      // Magnitude = 5
        
        Console.WriteLine($"  v4 = {v4}");
        Console.WriteLine($"  v5 = {v5}");
        Console.WriteLine($"  v6 = {v6}");
        
        Console.WriteLine($"  v4 == v6: {v4 == v6}");
        Console.WriteLine($"  v4 != v5: {v4 != v5}");
        Console.WriteLine($"  v5 < v4: {v5 < v4}");
        Console.WriteLine($"  v4 > v5: {v4 > v5}");

        // ===== CONVERSIONS =====
        Console.WriteLine("\n=== CONVERSION OPERATORS ===");
        
        Vector3D v7 = (2.0, 3.0, 4.0);  // Implicit from tuple
        Console.WriteLine($"  From tuple (2, 3, 4): {v7}");
        
        double[] arr = (double[])v7;    // Explicit to array
        Console.WriteLine($"  To array: [{string.Join(", ", arr)}]");
        
        var list = (List<double>)v7;    // Explicit to list
        Console.WriteLine($"  To list: [{string.Join(", ", list)}]");

        // ===== PRACTICAL EXAMPLE =====
        Console.WriteLine("\n=== PRACTICAL EXAMPLE: Physics ===");
        
        // Force vectors
        var gravity = new Vector3D(0, -9.81, 0);
        var wind = new Vector3D(2.5, 0, 0);
        var buoyancy = new Vector3D(0, 5.0, 0);
        
        var netForce = gravity + wind + buoyancy;
        Console.WriteLine($"  Gravity: {gravity}");
        Console.WriteLine($"  Wind: {wind}");
        Console.WriteLine($"  Buoyancy: {buoyancy}");
        Console.WriteLine($"  Net Force: {netForce}");

        // Normalize direction
        var direction = new Vector3D(3, 4, 0);
        var normalized = direction.Normalized;
        Console.WriteLine($"\n  Direction: {direction}");
        Console.WriteLine($"  Normalized: {normalized}");

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║   ✓ Real-World Example Complete                           ║");
        Console.WriteLine("║   All Operator Categories Demonstrated!                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
    }
}
