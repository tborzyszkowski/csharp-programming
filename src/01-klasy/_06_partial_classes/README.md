# Klasy Częściowe (Partial Classes)

## 🎯 Cel

Rozbicie definicji klasy na wiele plików.

---

## 🚀 Uruchomienie

```bash
cd code/
dotnet run
dotnet test
```

## Syntaktyka

```csharp
// Plik 1: Person.cs
public partial class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Plik 2: Person.Data.cs
public partial class Person
{
    public void LoadFromDatabase() { }
    public void SaveToDatabase() { }
}

// Plik 3: Person.Validation.cs
public partial class Person
{
    public bool IsValid() => !string.IsNullOrEmpty(Name) && Age >= 0;
}
```

## Zastosowania

- Rozdzielenie logiki biznesowej od generowanego kodu
- Generated code (WinForms, ASP.NET)
- Bardzo duże klasy (rozdzielenie odpowiedzialności)
- Współpraca wielu programistów

## Zasady

✅ Wszystkie części mają tę samą nazwę i namespace  
✅ Wszystkie części mogą mieć różne modyfikatory dostępu  
✅ W kompilacji traktowana jako jedna klasa  

---

## 📖 Referencje

[Microsoft Docs - Partial Classes](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/classes#partial-class-definitions)

