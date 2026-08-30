# Temat 6: Testing Async Code 🧪

Pisanie testów dla async metod w xUnit.

```csharp
[Fact]
public async Task GetData_ReturnsData()
{
    var result = await _service.GetDataAsync();
    Assert.NotNull(result);
}
```
