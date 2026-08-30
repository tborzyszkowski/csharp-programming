using System;
using Xunit;

namespace StaticClasses;

public static class MathHelper
{
    public const double PI = 3.14159;
    public const double E = 2.71828;
    
    public static double CircleArea(double radius) => PI * radius * radius;
    public static double CircleCircumference(double radius) => 2 * PI * radius;
    public static int Max(int a, int b) => a > b ? a : b;
    public static int Min(int a, int b) => a < b ? a : b;
}

public static class StringHelper
{
    public static string Reverse(string str)
    {
        char[] chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
    
    public static bool IsPalindrome(string str)
    {
        return str == Reverse(str);
    }
}

public static class ValidationHelper
{
    public static bool IsEmail(string? email)
    {
        return !string.IsNullOrEmpty(email) && email.Contains("@");
    }
    
    public static bool IsPhoneNumber(string? phone)
    {
        return !string.IsNullOrEmpty(phone) && phone.Length >= 9;
    }
}

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("=== KLASY STATYCZNE ===\n");
        
        Console.WriteLine("1. MathHelper:");
        Console.WriteLine($"   CircleArea(5): {MathHelper.CircleArea(5):F2}");
        Console.WriteLine($"   CircleCircumference(5): {MathHelper.CircleCircumference(5):F2}");
        Console.WriteLine($"   Max(10, 20): {MathHelper.Max(10, 20)}");
        Console.WriteLine();
        
        Console.WriteLine("2. StringHelper:");
        Console.WriteLine($"   Reverse(\"hello\"): {StringHelper.Reverse("hello")}");
        Console.WriteLine($"   IsPalindrome(\"racecar\"): {StringHelper.IsPalindrome("racecar")}");
        Console.WriteLine();
        
        Console.WriteLine("3. ValidationHelper:");
        Console.WriteLine($"   IsEmail(\"user@example.com\"): {ValidationHelper.IsEmail("user@example.com")}");
        Console.WriteLine($"   IsPhoneNumber(\"123456789\"): {ValidationHelper.IsPhoneNumber("123456789")}");
        Console.WriteLine();
        
        Console.WriteLine("4. Nie można instancjonować!");
        // new MathHelper();  // BŁĄD!
    }
}

public class StaticClassesTests
{
    [Fact]
    public void MathHelper_CalculatesCircleArea()
    {
        Assert.True(MathHelper.CircleArea(5) > 78 && MathHelper.CircleArea(5) < 79);
    }
    
    [Fact]
    public void StringHelper_ReversesCorrectly()
    {
        Assert.Equal("olleh", StringHelper.Reverse("hello"));
    }
    
    [Fact]
    public void StringHelper_DetectsPalindrome()
    {
        Assert.True(StringHelper.IsPalindrome("racecar"));
        Assert.False(StringHelper.IsPalindrome("hello"));
    }
    
    [Fact]
    public void ValidationHelper_ValidatesEmail()
    {
        Assert.True(ValidationHelper.IsEmail("user@example.com"));
        Assert.False(ValidationHelper.IsEmail("invalid"));
    }
    
    [Fact]
    public void ValidationHelper_ValidatesPhone()
    {
        Assert.True(ValidationHelper.IsPhoneNumber("123456789"));
        Assert.False(ValidationHelper.IsPhoneNumber("123"));
    }
}
