# Refleksja i Atrybuty w C#

## 📚 Wprowadzenie

Moduł kompleksowo omawia **refleksję (reflection)** i **atrybuty (attributes)** – mechanizmy introspekcji i metaprogramowania w C#, pozwalające na dynamiczne odkrywanie i modyfikowanie zachowania programu w czasie działania (runtime).

Od podstaw refleksji, przez tworzenie własnych atrybutów, aż po zaawansowane techniki takie jak Expression Trees i systemy pluginów.

---

## 📋 Tematy (10 wykładów)

| # | Temat | Opis |
|---|-------|------|
| 1 | [Refleksja – Wprowadzenie i Historia](_01_reflection_intro/README.md) | System.Reflection, typeof, GetType, workflow refleksji |
| 2 | [Badanie Typów – Type Inspection](_02_type_inspection/README.md) | GetProperties, GetMethods, GetFields, MemberInfo, BindingFlags |
| 3 | [System.Activator](_03_system_activator/README.md) | Dynamiczne tworzenie instancji, CreateInstance, factory patterns |
| 4 | [Własne Atrybuty – Custom Attributes (Część 1)](_04_custom_attributes/README.md) | AttributeUsage, AttributeTargets, walidacja |
| 5 | [Zaawansowane Atrybuty – Custom Attributes (Część 2)](_05_advanced_attributes/README.md) | Hierarchie, kompozycja, warunkowość, złożone wzorce |
| 6 | [Czytanie Atrybutów](_06_reading_attributes/README.md) | GetCustomAttribute(s), sprawdzanie istnienia, ekstrakcja metadanych |
| 7 | [Wbudowane Atrybuty .NET](_07_builtin_attributes/README.md) | [Obsolete], [Serializable], [Conditional], [Flags], DataAnnotations |
| 8 | [Plugin System](_08_plugin_system/README.md) | Assembly.LoadFrom/LoadFile, dynamiczne ładowanie DLL, bezpieczeństwo |
| 9 | [Expression Trees & Dynamic](_09_expression_trees_dynamic/README.md) | Expression\<T\>, słowo kluczowe dynamic, wydajność vs refleksja |
| 10 | [Performance & Best Practices](_10_performance_best_practices/README.md) | Benchmarking, cache'owanie, Source Generators vs refleksja, AOT |

---

## 🎯 Topologia Nauki

```
Temat 1: Refleksja – Wprowadzenie (fundament)
    ↓
Temat 2: Badanie Typów (odkrywanie struktury)
    ↓
Temat 3: System.Activator (dynamiczne tworzenie)
    ↓
Temat 4: Własne Atrybuty – cz. 1 (podstawy)
    ↓
Temat 5: Zaawansowane Atrybuty – cz. 2
    ↓
Temat 6: Czytanie Atrybutów
    ↓
Temat 7: Wbudowane Atrybuty .NET
    ↓
Temat 8: Plugin System (praktyczne zastosowanie)
    ↓
Temat 9: Expression Trees & Dynamic (zaawansowane)
    ↓
Temat 10: Performance & Best Practices (podsumowanie)
```

---

## 🚀 Wymagania

- .NET SDK 9.0 lub nowszy
- Visual Studio Code (lub Visual Studio Community)
- Znajomość interfejsów i abstrakcji (Moduł 7)

## 📦 Uruchamianie Przykładów

```bash
cd _01_reflection_intro/code
dotnet run
dotnet test
```

Każdy temat zawiera `README.md`, `code/` (Program.cs + testy), `diagrams/` (Mermaid) i `tasks/` (ćwiczenia z rozwiązaniami).
