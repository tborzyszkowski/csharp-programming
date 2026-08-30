# Ćwiczenia: I/O Operations

## 🟢 Basic

### 1. Read and Write File Async
Napisz kod który:
- Asynchronicznie zapisuje tekst do pliku
- Asynchronicznie czyta plik
- Wypisuje zawartość

**Wymagania:** Użyj `File.WriteAllTextAsync` i `File.ReadAllTextAsync`

---

### 2. Simple HTTP Request
Pobierz dane z publicznego API używając `HttpClient.GetStringAsync`

**URL:** https://jsonplaceholder.typicode.com/posts/1

---

## 🟡 Intermediate

### 3. Concurrent File Operations
Napisz kod który równocześnie:
- Tworzy 3 pliki (file1.txt, file2.txt, file3.txt)
- Czyta wszystkie 3 pliki
- Wypisuje rozmiar każdego

**Wymagania:** Użyj `Task.WhenAll`

---

### 4. Multiple HTTP Requests
Pobierz dane z 3 postów równocześnie:

```
https://jsonplaceholder.typicode.com/posts/1
https://jsonplaceholder.typicode.com/posts/2
https://jsonplaceholder.typicode.com/posts/3
```

Zmierz łączny czas (powinno być ~1 sekunda, nie 3)

---

## 🔴 Advanced

### 5. Streaming Large File
Pobierz duży plik asynchronicznie używając `GetAsync` z `ReadAsStreamAsync`

---

### 6. Retry on Failure
Implementuj retry logic:
- Jeśli HTTP request nie powiódł się, spróbuj ponownie (max 3 razy)
- Zwróć dane jeśli udał się którykolwiek request

---

## ✅ Rozwiązanie: Example 3

```csharp
public async Task ConcurrentFilesAsync()
{
    var files = new[] { "f1.txt", "f2.txt", "f3.txt" };
    var content = "test";
    
    // Write all
    await Task.WhenAll(files.Select(f => 
        File.WriteAllTextAsync(f, content)));
    
    // Read all
    var results = await Task.WhenAll(files.Select(f => 
        File.ReadAllTextAsync(f)));
}
```
