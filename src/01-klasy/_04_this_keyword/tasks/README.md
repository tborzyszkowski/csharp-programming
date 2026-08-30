# Zadania - this

## 📝 Zadanie: Builder pattern

Utwórz klasę `SqlQuery` z fluent API korzystającą z `this`:

```csharp
var query = new SqlQuery()
    .Select("Name", "Age")
    .From("Users")
    .Where("Age > 18")
    .OrderBy("Name");

Console.WriteLine(query.Build());
// SELECT Name, Age FROM Users WHERE Age > 18 ORDER BY Name
```

---

## ✅ Rozwiązanie: SqlQuery Builder Pattern

### Kod

```csharp
public class SqlQuery
{
    private string? selectPart;
    private string? fromPart;
    private string? wherePart;
    private string? orderByPart;
    
    public SqlQuery Select(params string[] columns)
    {
        selectPart = "SELECT " + string.Join(", ", columns);
        return this;  // Zwraca bieżący obiekt
    }
    
    public SqlQuery From(string table)
    {
        fromPart = " FROM " + table;
        return this;  // Umożliwia łańcuchowanie
    }
    
    public SqlQuery Where(string condition)
    {
        wherePart = " WHERE " + condition;
        return this;
    }
    
    public SqlQuery OrderBy(params string[] columns)
    {
        orderByPart = " ORDER BY " + string.Join(", ", columns);
        return this;
    }
    
    public string Build()
    {
        var query = selectPart ?? "";
        query += fromPart ?? "";
        query += wherePart ?? "";
        query += orderByPart ?? "";
        return query.Trim();
    }
    
    public override string ToString() => Build();
}

// W Main():
Console.WriteLine("🔨 SQL Query Builder Pattern (Fluent API)");
Console.WriteLine("──────────────────────────────────────────\n");

var query1 = new SqlQuery()
    .Select("Name", "Age")
    .From("Users")
    .Where("Age > 18")
    .OrderBy("Name");

Console.WriteLine("Query 1:");
Console.WriteLine($"  {query1.Build()}");

var query2 = new SqlQuery()
    .Select("*")
    .From("Orders")
    .Where("Total > 100")
    .Where("Status = 'Pending'")
    .OrderBy("CreatedDate");

Console.WriteLine("\nQuery 2:");
Console.WriteLine($"  {query2.Build()}");
```

### Wyjaśnienie

- Każda metoda zwraca `this` (referencję do bieżącego obiektu)
- To umożliwia **łańcuchowanie metod** (fluent API)
- Metody mogą być wywołane w dowolnej kolejności
- **Zaleta**: Kod jest czytelny i ekspresyjny
- **Wzorzec**: Builder pattern / Method chaining

### Testy

```csharp
[Fact]
public void SelectAndFrom_BuildsCorrectQuery()
{
    var query = new SqlQuery()
        .Select("Name", "Email")
        .From("Users")
        .Build();
    
    Assert.Equal("SELECT Name, Email FROM Users", query);
}

[Fact]
public void FullQuery_BuildsCompleteStatement()
{
    var query = new SqlQuery()
        .Select("*")
        .From("Products")
        .Where("Price > 100")
        .OrderBy("Name")
        .Build();
    
    Assert.Equal(
        "SELECT * FROM Products WHERE Price > 100 ORDER BY Name", 
        query
    );
}
```

