using System;
using Xunit;

namespace ConstructorChaining;

/// <summary>
/// Temat 2: Łańcuchowe Wywołanie Konstruktorów (this)
/// Demonstracja konstruktora łańcuchowego i unikania duplikacji kodu
/// </summary>

// ============ 1. KONSTRUKTOR ŁAŃCUCHOWY - PROSTY PRZYKŁAD ============

public class Point
{
    public double X { get; }
    public double Y { get; }
    
    /// <summary>
    /// Konstruktor dla punktu na osi X
    /// </summary>
    public Point(double x) : this(x, 0)
    {
        Console.WriteLine("[Point] Constructor with 1 param");
    }
    
    /// <summary>
    /// Konstruktor pełny (główny)
    /// </summary>
    public Point(double x, double y)
    {
        if (double.IsNaN(x) || double.IsNaN(y))
            throw new ArgumentException("Coordinates cannot be NaN");
        
        X = x;
        Y = y;
        Console.WriteLine("[Point] Main constructor with 2 params");
    }
    
    public double Distance() => Math.Sqrt(X * X + Y * Y);
    
    public override string ToString() => $"({X}, {Y})";
}

// ============ 2. WIELOKROTNE ŁAŃCUCHOWANIE ============

public class Employee
{
    public string Name { get; }
    public string Department { get; }
    public decimal Salary { get; }
    public DateTime HireDate { get; }
    
    // Konstruktor 1: tylko imię
    public Employee(string name) 
        : this(name, "HR")
    {
        Console.WriteLine("[Employee] Constructor 1 (name only)");
    }
    
    // Konstruktor 2: imię + dział
    public Employee(string name, string department)
        : this(name, department, 3000)
    {
        Console.WriteLine("[Employee] Constructor 2 (name + dept)");
    }
    
    // Konstruktor 3: imię + dział + pensja
    public Employee(string name, string department, decimal salary)
        : this(name, department, salary, DateTime.Now)
    {
        Console.WriteLine("[Employee] Constructor 3 (name + dept + salary)");
    }
    
    // Konstruktor 4: GŁÓWNY - wszystkie pola
    public Employee(string name, string department, decimal salary, DateTime hireDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");
        
        if (salary < 0)
            throw new ArgumentException("Salary cannot be negative");
        
        Name = name;
        Department = department;
        Salary = salary;
        HireDate = hireDate;
        
        Console.WriteLine("[Employee] Main constructor with all params");
    }
    
    public override string ToString() => $"{Name} ({Department}) - ${Salary}";
}

// ============ 3. KONSTRUKTOR ŁAŃCUCHOWY Z WARTOŚCIAMI DOMYŚLNYMI ============

public class DatabaseConfig
{
    public string Server { get; }
    public string Database { get; }
    public int Port { get; }
    public string Username { get; }
    public string Password { get; }
    public bool UseEncryption { get; }
    
    // Domyślna konfiguracja
    public DatabaseConfig()
        : this("localhost") { }
    
    // Z serwerem
    public DatabaseConfig(string server)
        : this(server, "mydb") { }
    
    // Z serwerem i bazą
    public DatabaseConfig(string server, string database)
        : this(server, database, 5432) { }
    
    // Z portem
    public DatabaseConfig(string server, string database, int port)
        : this(server, database, port, "admin") { }
    
    // Z username
    public DatabaseConfig(string server, string database, int port, string username)
        : this(server, database, port, username, "password") { }
    
    // Z password
    public DatabaseConfig(string server, string database, int port, string username, string password)
        : this(server, database, port, username, password, true) { }
    
    // GŁÓWNY - pełna konfiguracja
    public DatabaseConfig(string server, string database, int port, string username, 
                         string password, bool useEncryption)
    {
        if (port <= 0 || port > 65535)
            throw new ArgumentException("Invalid port");
        
        Server = server;
        Database = database;
        Port = port;
        Username = username;
        Password = password;
        UseEncryption = useEncryption;
    }
    
    public string GetConnectionString()
        => $"Server={Server};Database={Database};Port={Port};User={Username};Encrypt={UseEncryption}";
    
    public override string ToString() => GetConnectionString();
}

// ============ 4. KONSTRUKTOR ŁAŃCUCHOWY - ZAAWANSOWANY (BUILDER PATTERN) ============

public class HttpRequestBuilder
{
    public string Url { get; }
    public string Method { get; }
    public Dictionary<string, string> Headers { get; }
    public string? Body { get; }
    
    // Domyślny - GET
    public HttpRequestBuilder(string url)
        : this(url, "GET") { }
    
    // Z metodą HTTP
    public HttpRequestBuilder(string url, string method)
        : this(url, method, new Dictionary<string, string>()) { }
    
    // Z nagłówkami
    public HttpRequestBuilder(string url, string method, Dictionary<string, string> headers)
        : this(url, method, headers, null) { }
    
    // GŁÓWNY - z ciałem
    public HttpRequestBuilder(string url, string method, Dictionary<string, string> headers, string? body)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be empty");
        
        Url = url;
        Method = method;
        Headers = headers;
        Body = body;
    }
    
    public HttpRequestBuilder AddHeader(string key, string value)
    {
        Headers[key] = value;
        return this;
    }
    
    public override string ToString() => $"{Method} {Url} ({Headers.Count} headers)";
}

// ============ MAIN PROGRAM ============

public class Program
{
    public static void Main()
    {
        Console.WriteLine("=== TEMAT 2: ŁAŃCUCHOWE WYWOŁANIE KONSTRUKTORÓW ===\n");
        
        // 1. Point - 2 konstruktory
        Console.WriteLine("1. PROSTY ŁAŃCUCH - Point");
        var p1 = new Point(3);
        var p2 = new Point(3, 4);
        Console.WriteLine($"p1: {p1}, distance: {p1.Distance()}");
        Console.WriteLine($"p2: {p2}, distance: {p2.Distance()}");
        Console.WriteLine();
        
        // 2. Employee - wielokrotny łańcuch
        Console.WriteLine("2. WIELOKROTNY ŁAŃCUCH - Employee");
        var emp1 = new Employee("Jan");
        var emp2 = new Employee("Maria", "IT");
        var emp3 = new Employee("Piotr", "Finance", 5500);
        var emp4 = new Employee("Anna", "HR", 4200, DateTime.Now.AddYears(-2));
        Console.WriteLine(emp1);
        Console.WriteLine(emp2);
        Console.WriteLine(emp3);
        Console.WriteLine(emp4);
        Console.WriteLine();
        
        // 3. Database Config - domyślne wartości
        Console.WriteLine("3. WARTOŚCI DOMYŚLNE - DatabaseConfig");
        var cfg1 = new DatabaseConfig();
        var cfg2 = new DatabaseConfig("db.example.com");
        var cfg3 = new DatabaseConfig("db.example.com", "shop", 3306, "root", "secret");
        Console.WriteLine($"cfg1: {cfg1}");
        Console.WriteLine($"cfg2: {cfg2}");
        Console.WriteLine($"cfg3: {cfg3}");
        Console.WriteLine();
        
        // 4. HTTP Request - fluent API
        Console.WriteLine("4. FLUENT API - HttpRequestBuilder");
        var req = new HttpRequestBuilder("https://api.example.com/users")
            .AddHeader("Authorization", "Bearer token123")
            .AddHeader("Content-Type", "application/json");
        Console.WriteLine(req);
        Console.WriteLine();
    }
}

// ============ TESTY XUNIT ============

public class ConstructorChainingTests
{
    [Fact]
    public void Point_WithOneParam_SetsYtoZero()
    {
        var point = new Point(5);
        
        Assert.Equal(5, point.X);
        Assert.Equal(0, point.Y);
    }
    
    [Fact]
    public void Point_WithTwoParams_SetsBoth()
    {
        var point = new Point(3, 4);
        
        Assert.Equal(3, point.X);
        Assert.Equal(4, point.Y);
    }
    
    [Fact]
    public void Point_Distance_CalculatedCorrectly()
    {
        var point = new Point(3, 4);
        
        Assert.Equal(5, point.Distance());
    }
    
    [Fact]
    public void Point_WithNaN_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Point(double.NaN, 5));
    }
    
    [Fact]
    public void Employee_WithNameOnly_UsesDefaults()
    {
        var emp = new Employee("John");
        
        Assert.Equal("John", emp.Name);
        Assert.Equal("HR", emp.Department);
        Assert.Equal(3000, emp.Salary);
    }
    
    [Fact]
    public void Employee_WithFullParams_InitializesAll()
    {
        var hireDate = new DateTime(2020, 1, 1);
        var emp = new Employee("Jane", "IT", 5000, hireDate);
        
        Assert.Equal("Jane", emp.Name);
        Assert.Equal("IT", emp.Department);
        Assert.Equal(5000, emp.Salary);
        Assert.Equal(hireDate, emp.HireDate);
    }
    
    [Fact]
    public void Employee_WithInvalidName_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Employee(""));
    }
    
    [Fact]
    public void Employee_WithNegativeSalary_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => new Employee("John", "IT", -100));
    }
    
    [Fact]
    public void DatabaseConfig_Default_UsesLocalhost()
    {
        var config = new DatabaseConfig();
        
        Assert.Equal("localhost", config.Server);
        Assert.Equal("mydb", config.Database);
        Assert.Equal(5432, config.Port);
    }
    
    [Fact]
    public void DatabaseConfig_CustomPort_ValidatesRange()
    {
        Assert.Throws<ArgumentException>(() => new DatabaseConfig("localhost", "db", 99999));
        Assert.Throws<ArgumentException>(() => new DatabaseConfig("localhost", "db", 0));
    }
    
    [Fact]
    public void HttpRequest_AddHeader_BuildsCorrectly()
    {
        var req = new HttpRequestBuilder("https://api.example.com")
            .AddHeader("Auth", "token")
            .AddHeader("Type", "json");
        
        Assert.Equal("https://api.example.com", req.Url);
        Assert.Equal("GET", req.Method);
        Assert.Equal(2, req.Headers.Count);
    }
}
