# Temat 10: Większy Przykład - Vector3D System

## 🎯 Cel

Kompleksowy system dem onstrujący wszystkie typy przeciażeń operatorów w praktyce.

---

## 📖 Projekt: Geometria 3D

System reprezentujący i manipulujący wektorami w 3D:

```csharp
// Operatory binarne: + (dodanie), - (odejmowanie), * (iloczyn)
var v1 = new Vector3D(1, 2, 3);
var v2 = new Vector3D(4, 5, 6);
var sum = v1 + v2;           // [5, 7, 9]
var scaled = v1 * 2.0;       // [2, 4, 6]

// Operatory unarne: - (negacja), ! (czy zero?)
var neg = -v1;               // [-1, -2, -3]
if (!neg) { }                // False, wektor jest zerowy

// Operatory relacyjne: ==, !=, <, > (po długości)
bool equal = v1 == v2;       // False
bool shorter = v1 < v2;      // True (v1 jest krótszy)

// Operatory konwersji: explicit/implicit
Vector3D v = (1.5, 2.5, 3.5);  // Niejawna z tuple
double[] arr = (double[])v;     // Jawna na array
```

---

## 🏗️ Komponenty Systemu

### Vector3D

```csharp
public record Vector3D(double X, double Y, double Z)
{
    // Unary
    public static Vector3D operator +(Vector3D v) => v;
    public static Vector3D operator -(Vector3D v) => new(-v.X, -v.Y, -v.Z);
    public static bool operator !(Vector3D v) => Magnitude == 0;

    // Binary arithmetic
    public static Vector3D operator +(Vector3D a, Vector3D b) 
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3D operator -(Vector3D a, Vector3D b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vector3D operator *(Vector3D v, double scalar)
        => new(v.X * scalar, v.Y * scalar, v.Z * scalar);
    public static Vector3D operator /(Vector3D v, double scalar)
        => new(v.X / scalar, v.Y / scalar, v.Z / scalar);

    // Binary dot product
    public static double operator |(Vector3D a, Vector3D b)
        => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    // Cross product (binary operator)
    public static Vector3D operator &(Vector3D a, Vector3D b)
        => new(a.Y*b.Z - a.Z*b.Y, a.Z*b.X - a.X*b.Z, a.X*b.Y - a.Y*b.X);

    // Relational
    public static bool operator ==(Vector3D a, Vector3D b)
        => a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    public static bool operator !=(Vector3D a, Vector3D b) => !(a == b);
    public static bool operator <(Vector3D a, Vector3D b)
        => a.Magnitude < b.Magnitude;
    public static bool operator >(Vector3D a, Vector3D b)
        => a.Magnitude > b.Magnitude;

    // Conversions
    public static implicit operator Vector3D((double x, double y, double z) t)
        => new(t.x, t.y, t.z);
    public static explicit operator double[](Vector3D v)
        => new[] { v.X, v.Y, v.Z };

    public double Magnitude => Math.Sqrt(X*X + Y*Y + Z*Z);
    public Vector3D Normalized => this / Magnitude;
}
```

---

## 💡 Zastosowania

1. **Grafika komputerowa** - pozycje, kierunki wektorów
2. **Fizyka** - siły, prędkości, przyspieszenia
3. **Algebra liniowa** - operacje na wektorach
4. **Gry video** - geometria świata

---

## 🔗 Referencje

- [Vector Mathematics](https://en.wikipedia.org/wiki/Vector_(mathematics_and_physics))
- [Dot Product](https://en.wikipedia.org/wiki/Dot_product)
- [Cross Product](https://en.wikipedia.org/wiki/Cross_product)

---

## ➡️ Koniec Kursu!

Ukończyłeś pełny kurs na temat przeciażania operatorów w C#! 🎉
