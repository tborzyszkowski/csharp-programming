# Temat 3: I/O Operations 📡

## Wprowadzenie

Asynchroniczna obsługa I/O (Input/Output): pliki, sieć, baza danych. I/O jest naturalnym kandydatem do async - thread nie czeka na dane, pracuje nad innymi zadaniami.

---

## 📖 Dlaczego Async dla I/O?

**Synchronicznie (❌ zły):**
```csharp
var data = File.ReadAllText("file.txt");  // BLOKUJE thread!
var response = client.GetStringAsync(url).Result;  // DEADLOCK!
```

**Asynchronicznie (✅ dobrze):**
```csharp
var data = await File.ReadAllTextAsync("file.txt");  // Thread wolny!
var response = await client.GetStringAsync(url);     // Czeka bez blokowania
```

---

## 💻 Przykłady

### File I/O

```csharp
public async Task ReadFileAsync()
{
    var content = await File.ReadAllTextAsync("data.txt");
    Console.WriteLine(content);
}

public async Task WriteFileAsync(string data)
{
    await File.WriteAllTextAsync("output.txt", data);
}
```

### HttpClient

```csharp
private readonly HttpClient _client = new();

public async Task<string> FetchDataAsync(string url)
{
    var response = await _client.GetAsync(url);
    return await response.Content.ReadAsStringAsync();
}
```

### Multiple I/O Operations (Concurrent)

```csharp
public async Task FetchMultipleAsync(params string[] urls)
{
    var tasks = urls.Select(url => _client.GetStringAsync(url)).ToList();
    var results = await Task.WhenAll(tasks);
    return results;
}
```

---

## 🎯 Profity

✅ **Nie blokuje thread** - thread może pracować nad innymi zadaniami  
✅ **Lepsze resource utilization** - mniej threadów potrzebnych  
✅ **Lepsza skalabilność** - można obsłużyć więcej równoczesnych operacji  
✅ **Responsywna aplikacja** - UI nie "zamraza się"  

---

## 🔗 Referencje

- [HttpClient Best Practices](https://learn.microsoft.com/en-us/dotnet/fundamentals/networking/http/httpclient)
- [File.ReadAllTextAsync](https://learn.microsoft.com/en-us/dotnet/api/system.io.file.readalltextasync)
