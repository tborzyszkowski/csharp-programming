# Indeksatory - Tablica Asocjacyjna w Klasie

## 🎯 Cel rozdziału

Zrozumienie indeksatorów (`indexers`) - właściwości które przyjmują parametry i umożliwiają dostęp jak do tablicy/słownika.

## 📚 Spis treści

1. [Co to Indeksator?](#co-to-indeksator)
2. [Składnia](#składnia)
3. [Indeksatory z String](#indeksatory-z-string)
4. [Wielowymiarowe Indeksatory](#wielowymiarowe-indeksatory)

---

## Co to Indeksator?

**Indeksator** pozwala na dostęp do obiektu jak do tablicy:

```csharp
// Tablica
int[] arr = new int[3];
arr[0] = 10;  // Dostęp po indeksie

// Klasa z indeksatorem
public class MyList
{
    private List<int> items = new();
    
    public int this[int index]  // Indeksator!
    {
        get { return items[index]; }
        set { items[index] = value; }
    }
}

var list = new MyList();
list[0] = 10;  // To samo co tablica!
```

---

## Składnia

### Podstawowa

```csharp
public class SimpleCollection
{
    private string[] items = new string[3];
    
    public string this[int index]
    {
        get { return items[index]; }
        set { items[index] = value; }
    }
}

var collection = new SimpleCollection();
collection[0] = "Hello";
Console.WriteLine(collection[0]);  // Hello
```

### Z Walidacją

```csharp
public class ValidatedCollection
{
    private string[] items = new string[3];
    
    public string this[int index]
    {
        get
        {
            if (index < 0 || index >= items.Length)
                throw new IndexOutOfRangeException();
            return items[index];
        }
        set
        {
            if (index < 0 || index >= items.Length)
                throw new IndexOutOfRangeException();
            items[index] = value;
        }
    }
}
```

---

## Indeksatory z String

```csharp
public class Person
{
    private string name = string.Empty;
    private int age;
    private string email = string.Empty;
    
    public object? this[string propertyName]
    {
        get => propertyName switch
        {
            "name" => name,
            "age" => age,
            "email" => email,
            _ => null
        };
        set
        {
            switch (propertyName)
            {
                case "name":
                    name = (string?)value ?? string.Empty;
                    break;
                case "age":
                    age = (int?)value ?? 0;
                    break;
                case "email":
                    email = (string?)value ?? string.Empty;
                    break;
            }
        }
    }
}

var person = new Person();
person["name"] = "John";
person["age"] = 30;
Console.WriteLine(person["name"]);  // John
```

---

## Wielowymiarowe Indeksatory

```csharp
public class Matrix
{
    private int[,] data = new int[3, 3];
    
    public int this[int row, int col]
    {
        get { return data[row, col]; }
        set { data[row, col] = value; }
    }
}

var matrix = new Matrix();
matrix[0, 0] = 1;
matrix[0, 1] = 2;
Console.WriteLine(matrix[0, 0]);  // 1
```

---

## Best Practices

✅ Indeksatory dla **sekwencyjnych danych** (tablica, lista)

✅ Indeksatory string dla **Named access** (właściwości)

✅ **Waliduj indeks** - rzuć exception jeśli poza zakresem

✅ **Dokumentuj** - jakie indeksy są akceptowane

---

## 🚀 Jak pracować

```bash
cd code/
rtk dotnet run
rtk dotnet test
```

**Przejdź do**: [Zadania do samodzielnego wykonania](tasks/README.md)
