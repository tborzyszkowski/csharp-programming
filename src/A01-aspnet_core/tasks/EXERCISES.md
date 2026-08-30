# 📝 EXERCISES - A01: ASP.NET Core Project Manager

## Wstęp

Ćwiczenia są podzielone na trzy poziomy trudności:
- **🟢 Beginner** - Podstawowe umiejętności
- **🟡 Intermediate** - Średniozaawansowane
- **🔴 Advanced** - Zaawansowane koncepty

Każde ćwiczenie zawiera:
1. **Opis problemu**
2. **Wskazówki dotyczące rozwiązania**
3. **Kod startowy**
4. **Kryteria sukcesu**
5. **Bonus challenges**

---

## 🟢 LEVEL BEGINNER

### ✅ Ćwiczenie 1: Tworzenie Projektu z Walidacją

**Cel:** Zaimplementować formularz tworzenia projektu z pełną walidacją danych

**Opis Problemu:**
Użytkownik powinien móc utworzyć nowy projekt, ale aplikacja musi sprawdzić:
- Nazwa projektu: 3-100 znaków
- Data zakończenia > data rozpoczęcia
- Postęp: 0-100%

**Wskazówki:**
1. Przeanalizuj `DataAnnotations` w `Project.cs`
2. Patrz na `ProjectsController.Create()`
3. Sprawdź walidację w `Views/Projects/Create.cshtml`

**Kod Startowy:**
```csharp
// TODO: Uzupełnij walidację
public class Project
{
    // [Required]
    // [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
    
    // [Range(0, 100)]
    public int ProgressPercentage { get; set; }
}
```

**Kryteria Sukcesu:**
- ✅ Forma wyświetla błędy walidacji
- ✅ Nie można zapisać projektu z nazwą < 3 znaków
- ✅ Nie można zapisać jeśli EndDate < StartDate
- ✅ Custom komunikat o błędzie jest wyświetlany

**Bonus Challenge:**
Dodaj custom validator dla pola Description (max 500 znaków)

---

### ✅ Ćwiczenie 2: Filtrowanie Projektów Po Statusie

**Cel:** Dodać dropdown do filtrowania projektów po statusie (Active/OnHold/Completed)

**Opis Problemu:**
Na stronie Index powinien być dropdown umożliwiający wybór:
- Wszystkie
- Aktywne
- Wstrzymane
- Ukończone

**Wskazówki:**
1. Zmodyfikuj `ProjectsController.Index()`
2. Dodaj parametr `ProjectStatus? status`
3. Filtruj: `projects = projects.Where(p => p.Status == status).ToList()`
4. Zaktualizuj widok `Index.cshtml`

**Kod Startowy:**
```csharp
[HttpGet("")]
public async Task<IActionResult> Index(
    string? searchTerm, 
    string? sortBy,
    ProjectStatus? status = null)  // ← Dodaj
{
    var projects = await _projectService.GetAllProjectsAsync();
    
    if (!string.IsNullOrEmpty(searchTerm))
        projects = await _projectService.SearchProjectsAsync(searchTerm);
    
    // TODO: Dodaj filtrowanie po statusie
    
    return View(projects);
}
```

**HTML w formularzu:**
```html
<select name="status" class="form-select">
    <option value="">Wszystkie</option>
    <option value="Active">Aktywne</option>
    <option value="OnHold">Wstrzymane</option>
    <option value="Completed">Ukończone</option>
</select>
```

**Kryteria Sukcesu:**
- ✅ Dropdown działa
- ✅ Filtruje projekty po statusie
- ✅ Kombinacja search + sort + status działa

---

### ✅ Ćwiczenie 3: Wyświetlanie Listy Zadań

**Cel:** Na stronie Details wyświetlić tabelę wszystkich zadań projektu

**Opis Problemu:**
Przejście do szczegółów projektu powinno pokazać:
1. Dane projektu
2. Analityka (liczby zadań)
3. **Tabela wszystkich zadań** z:
   - Tytułem
   - Priorytetem (kolorowe badge)
   - Statusem
   - Terminem (z ostrzeżeniem jeśli przeterminowane)

**Wskazówki:**
Już to zrobiliśmy! Patrz: `Views/Projects/Details.cshtml` na koncu

```razor
@if (Model.Project.Tasks.Count == 0)
{
    <div class="alert alert-info">Brak zadań</div>
}
else
{
    <table class="table">
        <thead>
            <tr>
                <th>Tytuł</th>
                <th>Priorytet</th>
                <th>Status</th>
                <th>Termin</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var task in Model.Project.Tasks)
            {
                <tr>
                    <td>@task.Title</td>
                    <td>
                        <span class="badge bg-@(task.Priority > 7 ? "danger" : "info")">
                            P@task.Priority
                        </span>
                    </td>
                    <td>
                        <span class="badge bg-@(task.Status == TaskStatus.Done ? "success" : "warning")">
                            @task.Status
                        </span>
                    </td>
                    <td>
                        @if (task.IsOverdue)
                        {
                            <strong class="text-danger">@task.DueDate.ToString("dd.MM.yyyy")</strong>
                        }
                        else
                        {
                            <span>@task.DueDate.ToString("dd.MM.yyyy")</span>
                        }
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
```

**Kryteria Sukcesu:**
- ✅ Tabela wyświetla wszystkie zadania
- ✅ Kolory pokazują priorytet
- ✅ Przeterminowane zadania mają ostrzeżenie

---

### ✅ Ćwiczenie 4: Dodawanie Projektu do Ulubionych

**Cel:** Dodać checkbox "Ulubiony" przy każdym projekcie

**Opis Problemu:**
1. Dodaj pole `bool IsFavorite` do modelu `Project`
2. Utwórz migrację EF Core
3. Pokaż checkbox w formularzu Create/Edit
4. Na liście Index pokaż gwiazdkę ⭐ przy ulubionych

**Kod Startowy:**

```csharp
// 1. Models/Project.cs
public class Project
{
    // ...
    
    [Display(Name = "Ulubiony")]
    public bool IsFavorite { get; set; } = false;
}

// 2. Migracja
// dotnet ef migrations add AddIsFavoriteToProjects
// dotnet ef database update

// 3. Create.cshtml
<div class="form-check">
    <input asp-for="IsFavorite" type="checkbox" class="form-check-input">
    <label asp-for="IsFavorite" class="form-check-label">
        Dodaj do ulubionych
    </label>
</div>

// 4. Index.cshtml
@if (project.IsFavorite)
{
    <span class="text-warning">⭐</span>
}
```

**Kryteria Sukcesu:**
- ✅ Pole dodane do bazy danych
- ✅ Checkbox w formularzach
- ✅ Gwiazdka wyświetla się na liście
- ✅ Dane są zapisywane i ładowane

**Bonus Challenge:**
Dodaj filtr "Tylko ulubione" na stronie Index

---

## 🟡 LEVEL INTERMEDIATE

### ✅ Ćwiczenie 5: Wyszukiwanie Zaawansowane

**Cel:** Zaimplementować wyszukiwanie nie tylko po nazwie, ale i po opisie + data

**Opis Problemu:**
Użytkownik chce wyszukiwać projekty po:
- Nazwie (już zrobione)
- Opisie (NOWE)
- Zakresie dat (NOWE)

**Wskazówki:**
1. Zmodyfikuj `IProjectService` - dodaj nową metodę
2. W kontrolerze obsługuj dodatkowe parametry

**Kod Startowy:**
```csharp
// Services/IProjectService.cs
public interface IProjectService
{
    // ...
    
    // TODO: Dodaj metodę
    Task<List<Project>> AdvancedSearchAsync(
        string? searchTerm,
        DateTime? startDateFrom,
        DateTime? startDateTo);
}

// Implementation w ProjectService
public async Task<List<Project>> AdvancedSearchAsync(
    string? searchTerm,
    DateTime? startDateFrom,
    DateTime? startDateTo)
{
    var query = _context.Projects
        .Include(p => p.Tasks)
        .AsQueryable();
    
    // TODO: Dodaj filtry
    // Jeśli searchTerm - Where po Name + Description
    // Jeśli startDateFrom - Where >= StartDate
    // Jeśli startDateTo - Where <= StartDate
    
    return await query.ToListAsync();
}

// W kontrolerze
[HttpGet("")]
public async Task<IActionResult> Index(
    string? searchTerm,
    string? sortBy,
    DateTime? startDateFrom,
    DateTime? startDateTo)
{
    var projects = await _projectService.AdvancedSearchAsync(
        searchTerm, 
        startDateFrom, 
        startDateTo);
    
    // ...
}
```

**HTML forma:**
```html
<form method="get">
    <input type="text" name="searchTerm" placeholder="Szukaj...">
    <input type="date" name="startDateFrom" placeholder="Od daty">
    <input type="date" name="startDateTo" placeholder="Do daty">
    <button type="submit">Szukaj</button>
</form>
```

**Kryteria Sukcesu:**
- ✅ Wyszukiwanie po tekście działa
- ✅ Filtrowanie po datach działa
- ✅ Kombinacja obydwu działa
- ✅ Zapamiętane wartości w formie (sticky values)

---

### ✅ Ćwiczenie 6: Sortowanie Wielowarstwowe

**Cel:** Sortowanie po primär + sekundär kryterium (np. po statusie, potem po nazwie)

**Opis Problemu:**
Dodaj możliwość takich sortowań:
- Po nazwie → potem po dacie
- Po postępie malejąco → potem po nazwie
- Po liczbie zadań → potem po dacie

**Wskazówki:**
Użyj `ThenBy()` i `ThenByDescending()`:

```csharp
public async Task<List<Project>> GetProjectsByComplexSortAsync(string sortBy)
{
    var query = _context.Projects
        .Include(p => p.Tasks)
        .AsQueryable();
    
    return sortBy switch
    {
        "name" => await query
            .OrderBy(p => p.Name)
            .ThenByDescending(p => p.StartDate)  // ← Drugie sortowanie
            .ToListAsync(),
        
        "progress" => await query
            .OrderByDescending(p => p.ProgressPercentage)
            .ThenBy(p => p.Name)
            .ToListAsync(),
        
        "tasks" => await query
            .OrderByDescending(p => p.Tasks.Count)
            .ThenBy(p => p.Name)
            .ToListAsync(),
        
        _ => await query.OrderBy(p => p.Name).ToListAsync()
    };
}
```

**Kryteria Sukcesu:**
- ✅ Wielowarstwowe sortowanie działa
- ✅ Stabilne (powtarzalne wyniki)
- ✅ Intuitywne dla użytkownika

---

### ✅ Ćwiczenie 7: CRUD Kontroler dla Zadań

**Cel:** Stworzyć `TasksController` z pełnymi operacjami CRUD dla zadań

**Opis Problemu:**
Należy dodać nowy kontroler do zarządzania zadaniami:

```csharp
[Route("tasks")]
public class TasksController : Controller
{
    private readonly IProjectService _projectService;
    // ... DI
    
    // GET /tasks/create?projectId=1
    [HttpGet("create")]
    public async Task<IActionResult> Create(int projectId)
    {
        // TODO: Wyświetl formularz
    }
    
    // POST /tasks/create
    [HttpPost("create")]
    public async Task<IActionResult> Create(Task task)
    {
        // TODO: Walidacja i zapis
        // Potem redirect do Details projektu
    }
    
    // GET /tasks/{id}/edit
    [HttpGet("{id}/edit")]
    public async Task<IActionResult> Edit(int id)
    {
        // TODO: Pobierz zadanie i wyświetl formularz
    }
    
    // POST /tasks/{id}/edit
    [HttpPost("{id}/edit")]
    public async Task<IActionResult> Edit(int id, Task task)
    {
        // TODO: Walidacja i aktualizacja
    }
    
    // POST /tasks/{id}/delete
    [HttpPost("{id}/delete")]
    public async Task<IActionResult> Delete(int id)
    {
        // TODO: Usunięcie i redirect
    }
}
```

**Kryteria Sukcesu:**
- ✅ Dodawanie nowych zadań działa
- ✅ Edycja istniejących zadań działa
- ✅ Usuwanie zadań działa
- ✅ Walidacja DataAnnotations działa
- ✅ Po akcji redirect do projektu

**Bonus Challenge:**
Dodaj możliwość zmiany statusu zadania (ToDo → InProgress → Done) za pomocą przycisków

---

### ✅ Ćwiczenie 8: Dashboard Analytics

**Cel:** Stworzyć widok pokazujący statystyki wszystkich projektów

**Opis Problemu:**
Użytkownik chce zobaczyć:
1. Łączna liczba projektów
2. Łączna liczba zadań
3. Liczba ukończonych zadań
4. Tabela z analitiką każdego projektu

**Wskazówki:**
Już to jest w `Views/Projects/Analytics.cshtml`!

Analiza kodu:
```csharp
// Controller
[HttpGet("analytics")]
public async Task<IActionResult> Analytics()
{
    var allAnalytics = await _projectService.GetAllProjectsAnalyticsAsync();
    return View(allAnalytics);
}

// Service
public async Task<List<ProjectAnalytics>> GetAllProjectsAnalyticsAsync()
{
    var projects = await GetAllProjectsAsync();
    var analyticsList = new List<ProjectAnalytics>();
    
    foreach (var project in projects)
    {
        var analytics = await GetProjectAnalyticsAsync(project.Id);
        analyticsList.Add(analytics);
    }
    
    return analyticsList;
}
```

**Kryteria Sukcesu:**
- ✅ Widok się wyświetla
- ✅ Statystyki są poprawne
- ✅ Tabela pokazuje dane
- ✅ Progress bary działają

---

## 🔴 LEVEL ADVANCED

### ✅ Ćwiczenie 9: Reporting z LINQ GroupBy

**Cel:** Wygenerować raport projektów pogrupowanych po statusie

**Opis Problemu:**
Stworzyć raport pokazujący:
- Projekty pogrupowane po statusie
- Dla każdej grupy: liczba projektów, średni postęp, łączna liczba zadań

**Kod Startowy:**
```csharp
// Services/IProjectService.cs
public interface IProjectService
{
    // ...
    Task<Dictionary<ProjectStatus, ProjectGroupAnalytics>> 
        GetProjectsGroupedByStatusAsync();
}

// Model dla grupy
public class ProjectGroupAnalytics
{
    public int ProjectCount { get; set; }
    public double AverageProgress { get; set; }
    public int TotalTasks { get; set; }
    public List<Project> Projects { get; set; }
}

// Implementacja
public async Task<Dictionary<ProjectStatus, ProjectGroupAnalytics>> 
    GetProjectsGroupedByStatusAsync()
{
    var projects = await GetAllProjectsAsync();
    
    // TODO: GroupBy(p => p.Status)
    // Dla każdej grupy:
    // - ProjectCount = g.Count()
    // - AverageProgress = g.Average(p => p.ProgressPercentage)
    // - TotalTasks = g.Sum(p => p.Tasks.Count)
    // - Projects = g.ToList()
}
```

**Raport View:**
```html
@model Dictionary<ProjectStatus, ProjectGroupAnalytics>

<div class="row">
    @foreach (var group in Model)
    {
        <div class="col-md-4">
            <div class="card">
                <h5>@group.Key.ToString()</h5>
                <p>Projektów: @group.Value.ProjectCount</p>
                <p>Średni postęp: @group.Value.AverageProgress.ToString("F2")%</p>
                <p>Całkowite zadania: @group.Value.TotalTasks</p>
            </div>
        </div>
    }
</div>
```

**Kryteria Sukcesu:**
- ✅ GroupBy pracuje poprawnie
- ✅ Agregacja (Count, Average, Sum) działa
- ✅ Raport wyświetla dane
- ✅ Liczby są poprawne

---

### ✅ Ćwiczenie 10: Performance Optimization - N+1 Problem

**Cel:** Zidentyfikować i naprawić N+1 queries problem

**Opis Problemu:**
Poniższy kod wykonuje wiele zapytań:

```csharp
// ❌ WOLNE - N+1 queries
var projects = await _context.Projects.ToListAsync();  // 1 query
foreach (var project in projects)
{
    var tasks = project.Tasks;  // 1 query per project
}
```

**Rozwiązanie - Eager Loading:**

```csharp
// ✅ SZYBKO - 2 queries
var projects = await _context.Projects
    .Include(p => p.Tasks)  // ← Eager loading
    .ToListAsync();
```

**Wyzwanie:**
Przejrzyj kod w usłudze i kontrolerze, znajdź wszystkie N+1 problemy, naprawi je Include()

**Porada:**
W `ProjectsController.Index()` powinno być Include:

```csharp
// ❌ Przed
var projects = await _projectService.GetAllProjectsAsync();
// Jeśli GetAllProjectsAsync nie ma Include, to N+1

// ✅ Po
var projects = await _context.Projects
    .Include(p => p.Tasks)
    .OrderByDescending(p => p.StartDate)
    .ToListAsync();
```

**Kryteria Sukcesu:**
- ✅ Include() używany we wszystkich местах
- ✅ Brak N+1 queries
- ✅ Aplikacja jest szybsza

---

### ✅ Ćwiczenie 11: Caching Implementation

**Cel:** Dodać memory caching dla poprawy performance

**Opis Problemu:**
Każde żądanie do Index zmusza aplikację na ponowne kwerendy bazy danych.
Rozwiązanie: Cache wyniki przez 5 minut

**Kod Startowy:**

```csharp
// Dodaj do Program.cs
builder.Services.AddMemoryCache();

// Wrapper service z cachingiem
public class CachedProjectService : IProjectService
{
    private readonly IProjectService _innerService;
    private readonly IMemoryCache _cache;
    private const string ALL_PROJECTS_CACHE_KEY = "all_projects";
    private const int CACHE_DURATION_MINUTES = 5;
    
    public CachedProjectService(
        IProjectService innerService,
        IMemoryCache cache)
    {
        _innerService = innerService;
        _cache = cache;
    }
    
    public async Task<List<Project>> GetAllProjectsAsync()
    {
        // TODO: Sprawdź czy jest w cache
        if (_cache.TryGetValue(ALL_PROJECTS_CACHE_KEY, out List<Project>? cached))
        {
            return cached;
        }
        
        // Jeśli nie - pobierz
        var projects = await _innerService.GetAllProjectsAsync();
        
        // TODO: Zapisz do cache na 5 minut
        _cache.Set(ALL_PROJECTS_CACHE_KEY, projects, 
            TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
        
        return projects;
    }
    
    // TODO: Invalidate cache przy Create/Update/Delete
    public async Task CreateProjectAsync(Project project)
    {
        await _innerService.CreateProjectAsync(project);
        _cache.Remove(ALL_PROJECTS_CACHE_KEY);  // ← Clear cache
    }
}
```

**DI Registration:**
```csharp
builder.Services.AddScoped<ProjectService>();
builder.Services.Decorate<IProjectService, CachedProjectService>();
// lub ręcznie
builder.Services.AddScoped<IProjectService>(provider =>
    new CachedProjectService(
        new ProjectService(provider.GetRequiredService<ApplicationDbContext>()),
        provider.GetRequiredService<IMemoryCache>()
    )
);
```

**Kryteria Sukcesu:**
- ✅ Cache implementowany
- ✅ TTL 5 minut
- ✅ Cache invalidation przy zmianie danych
- ✅ Performance test - drugi load szybszy

---

### ✅ Ćwiczenie 12: Unit Tests

**Cel:** Napisać unit testy dla `ProjectService` (używając xUnit i Moq)

**Setup:**

```bash
dotnet add package xunit
dotnet add package Moq
dotnet add package Microsoft.EntityFrameworkCore.InMemory
```

**Kod Startowy:**

```csharp
// ProjectServiceTests.cs
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Data;
using ProjectManager.Models;
using ProjectManager.Services;

public class ProjectServiceTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        return new ApplicationDbContext(options);
    }
    
    [Fact]
    public async Task GetAllProjectsAsync_ReturnsAllProjects()
    {
        // Arrange
        var dbContext = GetInMemoryDbContext();
        var project1 = new Project { Id = 1, Name = "Project 1" };
        var project2 = new Project { Id = 2, Name = "Project 2" };
        
        dbContext.Projects.AddRange(project1, project2);
        await dbContext.SaveChangesAsync();
        
        var service = new ProjectService(dbContext);
        
        // Act
        var result = await service.GetAllProjectsAsync();
        
        // Assert
        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task CreateProjectAsync_AddsProjectToDatabase()
    {
        // TODO: Test
    }
    
    [Fact]
    public async Task SearchProjectsAsync_FiltersByName()
    {
        // TODO: Test
    }
    
    [Fact]
    public async Task GetProjectAnalyticsAsync_ReturnsCorrectStatistics()
    {
        // TODO: Test
        // - Total tasks
        // - Completed tasks
        // - Overdue tasks
    }
}
```

**Uruchomienie testów:**
```bash
dotnet test
```

**Kryteria Sukcesu:**
- ✅ 4+ testy napisane
- ✅ Wszystkie testy przechodzą
- ✅ Testy testują logikę business

---

## 🎯 Challenge: Integracyjny Projekt

### Kompleksowe Wyzwanie: "Productivity Dashboard"

**Zadanie:**
Stworzyć zaawansowany dashboard, który:

1. **Główna Strona:**
   - Ostatnio aktywne projekty (LINQ: OrderByDescending)
   - Przeterminowane zadania (LINQ: Where IsOverdue)
   - Statystyki (GroupBy, Sum, Average)

2. **Filtry Zaawansowane:**
   - Zakres dat
   - Status projektu
   - Priorytet zadań
   - Tekst wyszukiwania

3. **Sorting:**
   - Po zdacie malejąco
   - Po postępie malejąco
   - Po liczbie zadań

4. **Performance:**
   - Caching wyników
   - Indeksy bazy danych
   - Include() do eager loadingu

5. **Testy:**
   - Unit testy dla Service
   - Integration testy dla Controller

6. **UI/UX:**
   - Responsive design (Bootstrap)
   - Loading indicators
   - Error messages

**Bonus Points:**
- Eksport do CSV
- API endpoint (Web API)
- Real-time updates (SignalR)
- Mobile app (Blazor Mobile)

---

## Podsumowanie Ćwiczeń

| # | Nazwa | Temat | Poziom |
|---|-------|-------|--------|
| 1 | Walidacja | DataAnnotations | 🟢 |
| 2 | Filtrowanie | LINQ Where | 🟢 |
| 3 | Wyświetlanie | Razor templating | 🟢 |
| 4 | Ulubione | EF Migrations | 🟢 |
| 5 | Advanced Search | LINQ Query | 🟡 |
| 6 | Multi-sort | LINQ ThenBy | 🟡 |
| 7 | Task CRUD | Controller | 🟡 |
| 8 | Analytics | Views & Data | 🟡 |
| 9 | Reporting | LINQ GroupBy | 🔴 |
| 10 | N+1 Problem | Optimization | 🔴 |
| 11 | Caching | Performance | 🔴 |
| 12 | Unit Tests | Testing | 🔴 |

Powodzenia! 🚀
