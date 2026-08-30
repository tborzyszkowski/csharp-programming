using System;
using System.Linq;
using Xunit;

namespace StaticMethods;

public class StringHelper
{
    public static bool IsNullOrEmpty(string? str) => string.IsNullOrEmpty(str);
    
    public static string Reverse(string str)
    {
        char[] chars = str.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
    
    public static int CountVowels(string str)
    {
        int count = 0;
        foreach (char c in str.ToLower())
        {
            if ("aeiou".Contains(c))
                count++;
        }
        return count;
    }
}

public class MathHelper
{
    public static double Square(double x) => x * x;
    public static double Cube(double x) => x * x * x;
    public static int Factorial(int n)
    {
        if (n <= 1) return 1;
        return n * Factorial(n - 1);
    }
}

public class ValidationHelper
{
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrEmpty(email))
            return false;
        
        return email.Contains("@") && email.Contains(".");
    }
    
    public static bool IsValidPhoneNumber(string? phone)
    {
        if (string.IsNullOrEmpty(phone))
            return false;
        
        return phone.Length >= 9 && phone.All(char.IsDigit);
    }
}

public class DataConverter
{
    public static int StringToInt(string? str)
    {
        return int.TryParse(str, out int result) ? result : 0;
    }
    
    public static double CelsiusToFahrenheit(double celsius)
    {
        return (celsius * 9 / 5) + 32;
    }
    
    public static double KilometersToMiles(double km)
    {
        return km * 0.621371;
    }
}

public class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    private Person() { }
    
    private Person(string first, string last)
    {
        FirstName = first;
        LastName = last;
    }
    
    // Factory Method
    public static Person CreateFromFullName(string fullName)
    {
        var parts = fullName.Split(' ');
        return new Person(parts[0], parts.Length > 1 ? parts[1] : "");
    }
    
    // Factory Method
    public static Person CreateAdmin()
    {
        return new Person("Admin", "User");
    }
}

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== METODY STATYCZNE (STATIC METHODS) ===\n");
        
        // 1. String Helper
        Console.WriteLine("1. STRING HELPER:");
        Console.WriteLine($"   IsNullOrEmpty(\"test\"): {StringHelper.IsNullOrEmpty("test")}");
        Console.WriteLine($"   IsNullOrEmpty(null): {StringHelper.IsNullOrEmpty(null)}");
        Console.WriteLine($"   Reverse(\"hello\"): {StringHelper.Reverse("hello")}");
        Console.WriteLine($"   CountVowels(\"programming\"): {StringHelper.CountVowels("programming")}");
        Console.WriteLine();
        
        // 2. Math Helper
        Console.WriteLine("2. MATH HELPER:");
        Console.WriteLine($"   Square(5): {MathHelper.Square(5)}");
        Console.WriteLine($"   Cube(3): {MathHelper.Cube(3)}");
        Console.WriteLine($"   Factorial(5): {MathHelper.Factorial(5)}");
        Console.WriteLine();
        
        // 3. Validation Helper
        Console.WriteLine("3. VALIDATION HELPER:");
        Console.WriteLine($"   IsValidEmail(\"user@example.com\"): {ValidationHelper.IsValidEmail("user@example.com")}");
        Console.WriteLine($"   IsValidEmail(\"invalid\"): {ValidationHelper.IsValidEmail("invalid")}");
        Console.WriteLine($"   IsValidPhoneNumber(\"123456789\"): {ValidationHelper.IsValidPhoneNumber("123456789")}");
        Console.WriteLine();
        
        // 4. Data Converter
        Console.WriteLine("4. DATA CONVERTER:");
        Console.WriteLine($"   StringToInt(\"42\"): {DataConverter.StringToInt("42")}");
        Console.WriteLine($"   CelsiusToFahrenheit(25): {DataConverter.CelsiusToFahrenheit(25)}");
        Console.WriteLine($"   KilometersToMiles(100): {DataConverter.KilometersToMiles(100):F2}");
        Console.WriteLine();
        
        // 5. Factory Methods
        Console.WriteLine("5. FACTORY METHODS:");
        var person1 = Person.CreateFromFullName("John Doe");
        var person2 = Person.CreateAdmin();
        Console.WriteLine($"   person1: {person1.FirstName} {person1.LastName}");
        Console.WriteLine($"   person2: {person2.FirstName} {person2.LastName}");
    }
}

public class StaticMethodsTests
{
    [Fact]
    public void StringHelper_IsNullOrEmpty_ReturnsTrueForNull()
    {
        Assert.True(StringHelper.IsNullOrEmpty(null));
        Assert.True(StringHelper.IsNullOrEmpty(""));
        Assert.False(StringHelper.IsNullOrEmpty("test"));
    }
    
    [Fact]
    public void StringHelper_Reverse_ReversesString()
    {
        Assert.Equal("olleh", StringHelper.Reverse("hello"));
        Assert.Equal("dlrow", StringHelper.Reverse("world"));
    }
    
    [Fact]
    public void StringHelper_CountVowels_CountsCorrectly()
    {
        Assert.Equal(2, StringHelper.CountVowels("hello"));
        Assert.Equal(3, StringHelper.CountVowels("programming"));
    }
    
    [Fact]
    public void MathHelper_Square_CalculatesCorrectly()
    {
        Assert.Equal(25, MathHelper.Square(5));
        Assert.Equal(4, MathHelper.Square(2));
    }
    
    [Fact]
    public void DataConverter_CelsiusToFahrenheit_ConvertsCorrectly()
    {
        Assert.Equal(77, DataConverter.CelsiusToFahrenheit(25));
        Assert.Equal(32, DataConverter.CelsiusToFahrenheit(0));
    }
    
    [Fact]
    public void Person_CreateFromFullName_ParsesCorrectly()
    {
        var person = Person.CreateFromFullName("John Doe");
        
        Assert.Equal("John", person.FirstName);
        Assert.Equal("Doe", person.LastName);
    }
    
    [Fact]
    public void Person_CreateAdmin_ReturnsAdminUser()
    {
        var admin = Person.CreateAdmin();
        
        Assert.Equal("Admin", admin.FirstName);
        Assert.Equal("User", admin.LastName);
    }
}
