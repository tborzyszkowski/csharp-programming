using System;
using System.Linq;
using Xunit;

namespace ExtensionMethods;

public static class StringExtensions
{
    public static string Capitalize(this string str)
    {
        if (string.IsNullOrEmpty(str)) return str;
        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
    
    public static bool IsValidEmail(this string str)
    {
        return !string.IsNullOrEmpty(str) && str.Contains("@");
    }
    
    public static int CountWords(this string str)
    {
        return string.IsNullOrEmpty(str) ? 0 : str.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

public static class IntExtensions
{
    public static bool IsEven(this int num) => num % 2 == 0;
    public static bool IsOdd(this int num) => num % 2 != 0;
    public static int Square(this int num) => num * num;
    public static int Cube(this int num) => num * num * num;
}

public static class EnumerableExtensions
{
    public static T? GetRandomElement<T>(this System.Collections.Generic.IEnumerable<T> list)
    {
        var arr = list.ToArray();
        if (arr.Length == 0) return default;
        return arr[new Random().Next(arr.Length)];
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== METODY ROZSZERZAJĄCE ===\n");
        
        Console.WriteLine("1. String Extensions:");
        string text = "hello world";
        Console.WriteLine($"   \"{text}\".Capitalize(): \"{text.Capitalize()}\"");
        Console.WriteLine($"   \"{text}\".IsValidEmail(): {text.IsValidEmail()}");
        Console.WriteLine($"   \"{text}\".CountWords(): {text.CountWords()}");
        Console.WriteLine();
        
        Console.WriteLine("2. Int Extensions:");
        int num = 5;
        Console.WriteLine($"   {num}.IsEven(): {num.IsEven()}");
        Console.WriteLine($"   {num}.IsOdd(): {num.IsOdd()}");
        Console.WriteLine($"   {num}.Square(): {num.Square()}");
        Console.WriteLine($"   {num}.Cube(): {num.Cube()}");
        Console.WriteLine();
        
        Console.WriteLine("3. Enumerable Extensions:");
        var numbers = new[] { 1, 2, 3, 4, 5 };
        var random = numbers.GetRandomElement();
        Console.WriteLine($"   Random element: {random}");
    }
}

public class ExtensionMethodsTests
{
    [Fact]
    public void StringExtension_Capitalize_WorksCorrectly()
    {
        Assert.Equal("Hello", "hello".Capitalize());
        Assert.Equal("World", "WORLD".Capitalize());
    }
    
    [Fact]
    public void StringExtension_IsValidEmail_ChecksAt()
    {
        Assert.True("user@example.com".IsValidEmail());
        Assert.False("invalid".IsValidEmail());
    }
    
    [Fact]
    public void StringExtension_CountWords_CountsCorrectly()
    {
        Assert.Equal(2, "hello world".CountWords());
        Assert.Equal(3, "one two three".CountWords());
    }
    
    [Fact]
    public void IntExtension_IsEven_WorksCorrectly()
    {
        Assert.True(4.IsEven());
        Assert.False(5.IsEven());
    }
    
    [Fact]
    public void IntExtension_Square_CalculatesCorrectly()
    {
        Assert.Equal(25, 5.Square());
        Assert.Equal(16, 4.Square());
    }
}
