# A01: ASP.NET Core - System Zarządzania Projektami

## 📋 Spis Treści

- [Przegląd](#przegląd)
- [Struktura Projektu](#struktura-projektu)
- [Szybki Start](#szybki-start)
- [Główne Komponenty](#główne-komponenty)
- [LINQ Queries](#linq-queries)
- [Funkcjonalności](#funkcjonalności)
- [Architektura](#architektura)
- [Status Modułu](#status-modułu)

---

## Przegląd

**A01** to zaawansowany projekt edukacyjny prezentujący kompletną aplikację ASP.NET Core:

- **Projekt**: System zarządzania projektami i zadaniami
- **Architektura**: MVC + Services + Entity Framework Core
- **Baza Danych**: SQL Server z relacją 1:N (Project → Tasks)
- **Zaawansowane Elementy**: LINQ, Razor views, Blazor components, DataAnnotations validation
- **Cel**: Zrozumienie nowoczesnego stacku .NET

### Key Features

✅ **CRUD Operacje** - Tworzenie, czytanie, aktualizacja, usuwanie  
✅ **LINQ Queries** - Include, Where, OrderBy, GroupBy, Select  
✅ **Walidacja** - DataAnnotations na modelu  
✅ **MVC Routing** - HTTP endpoints z parametrami  
✅ **Razor Templates** - Dynamic HTML generation  
✅ **Blazor Components** - Interactive UI w C#  
✅ **Performance** - Eager loading, indeksy bazy  
✅ **Analytics** - Statystyki i raporty z GroupBy  

---

## Struktura Projektu

```
A01-aspnet_core/
│
├── Models/                          # Encje i ViewModels
│   ├── Enums.cs                    # ProjectStatus, TaskStatus
│   ├── Project.cs                  # Główna encja (1:N relationship)
│   ├── Task.cs                     # Zależna encja (N:1 relationship)
│   └── ProjectAnalytics.cs         # ViewModel dla statystyk
│
├── Data/                           # Data Access Layer (EF Core)
│   └── ApplicationDbContext.cs    # DbSet<Project>, DbSet<Task>
│                                   # Relacje, indeksy, seed data
│
├── Services/                       # Business Logic Layer
│   └── IProjectService.cs         # Interface + Implementation
│                                   # LINQ queries, aggregations
│
├── Controllers/                    # HTTP Layer (MVC)
│   └── ProjectsController.cs      # 7 actions: Index, Create, Details,
│                                   # Edit, Delete, Analytics
│
├── Views/                          # Razor Templates
│   ├── Projects/
│   │   ├── Index.cshtml           # Lista + search + sort
│   │   ├── Create.cshtml          # Formularz tworzenia
│   │   ├── Edit.cshtml            # Formularz edycji
│   │   ├── Details.cshtml         # Szczegóły + analityka
│   │   └── Analytics.cshtml       # Dashboard wszystkich projektów
│   └── Shared/
│       ├── _Layout.cshtml         # Master layout
│       └── Error.cshtml           # Error page
│
├── Components/                     # Blazor Server Components
│   ├── ProjectDashboard.razor     # Interactive dashboard
│   └── TaskEditor.razor           # Task management component
│
├── wwwroot/                        # Static Files
│   ├── css/                       # Stylesheets
│   └── js/                        # JavaScript
│
├── Program.cs                      # Startup configuration
├── appsettings.json               # Configuration (DB connection)
└── ProjectManager.csproj          # Project file + NuGet packages
```

---

## Szybki Start

### Wymagania
- .NET 9.0 SDK
- SQL Server (Express lub LocalDB)
- Visual Studio Code / Visual Studio 2022

### Instalacja

```bash
# 1. Przejdź do folderu
cd src/A01-aspnet_core

# 2. Restore packages
dotnet restore

# 3. Utwórz bazę danych
dotnet ef database update

# 4. Uruchom aplikację
dotnet run

# 5. Otwórz przeglądarkę
# https://localhost:7123
```

### Seed Data

Aplikacja automatycznie tworzy przykładowe dane:
- 2 projekty (System CRM, Modernizacja API)
- 7 zadań z różnymi statusami i priorytetami

---

## Główne Komponenty

### 1. Models (Encje)

```csharp
// ProjectStatus enum
public enum ProjectStatus { Active, OnHold, Completed }

// Project - główna encja
public class Project
{
    public int Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }
    
    [Range(0, 100)]
    public int ProgressPercentage { get; set; }
    
    public ProjectStatus Status { get; set; }
    
    // 1:N relationship
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}

// Task - zależna od Project
public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    [Range(1, 10)]
    public int Priority { get; set; }
    
    // Foreign Key
    public int ProjectId { get; set; }
    public Project? Project { get; set; }  // Navigation
}
```

### 2. DbContext (Data Access)

```csharp
public class ApplicationDbContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relacja 1:N z cascade delete
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Indeksy dla performance
        modelBuilder.Entity<Project>()
            .HasIndex(p => p.Status);
        
        // Seed data
        SeedInitialData(modelBuilder);
    }
}
```

### 3. Services (Business Logic)

```csharp
public interface IProjectService
{
    // CRUD
    Task<List<Project>> GetAllProjectsAsync();
    Task CreateProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);
    
    // Analytics
    Task<ProjectAnalytics> GetProjectAnalyticsAsync(int projectId);
    
    // Filtering & Sorting
    Task<List<Project>> SearchProjectsAsync(string searchTerm);
    Task<List<Project>> GetProjectsBySortAsync(string sortBy);
}

// Implementacja z LINQ
public class ProjectService : IProjectService
{
    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Tasks)  // Eager loading
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }
    
    public async Task<ProjectAnalytics> GetProjectAnalyticsAsync(int projectId)
    {
        var project = await GetProjectByIdAsync(projectId);
        var tasks = project.Tasks;
        
        return new ProjectAnalytics
        {
            TotalTasks = tasks.Count,
            CompletedTasks = tasks.Count(t => t.Status == TaskStatus.Done),
            TasksByPriority = tasks
                .GroupBy(t => t.Priority)
                .ToDictionary(g => g.Key, g => g.Count()),
            AveragePriority = tasks.Average(t => t.Priority)
        };
    }
}
```

### 4. Controllers (HTTP Layer)

```csharp
[Route("projects")]
public class ProjectsController : Controller
{
    // GET /projects
    [HttpGet("")]
    public async Task<IActionResult> Index(
        string? searchTerm, 
        string? sortBy)
    {
        List<Project> projects;
        
        if (!string.IsNullOrEmpty(searchTerm))
            projects = await _projectService.SearchProjectsAsync(searchTerm);
        else
            projects = await _projectService.GetAllProjectsAsync();
        
        if (!string.IsNullOrEmpty(sortBy))
            projects = await _projectService.GetProjectsBySortAsync(sortBy);
        
        return View(projects);
    }
    
    // GET /projects/create
    // POST /projects/create
    // GET /projects/{id}
    // GET /projects/edit/{id}
    // POST /projects/edit/{id}
    // POST /projects/delete/{id}
    // GET /projects/analytics
}
```

---

## LINQ Queries

### Podstawowe Query Patterns

```csharp
// 1. Filtrowanie (Where)
var activeProjects = projects
    .Where(p => p.Status == ProjectStatus.Active)
    .ToList();

// 2. Sortowanie (OrderBy/OrderByDescending)
var sorted = projects
    .OrderByDescending(p => p.StartDate)
    .ThenBy(p => p.Name)
    .ToList();

// 3. Selekcja (Select)
var names = projects
    .Select(p => p.Name)
    .ToList();

// 4. Grupowanie (GroupBy)
var tasksByStatus = tasks
    .GroupBy(t => t.Status)
    .ToDictionary(g => g.Key, g => g.Count());

// 5. Agregacja
int totalTasks = projects.Sum(p => p.Tasks.Count);
double avgProgress = projects.Average(p => p.ProgressPercentage);
var oldest = projects.Min(p => p.StartDate);

// 6. Eager Loading (Include)
var projectsWithTasks = projects
    .Include(p => p.Tasks)
    .ToListAsync();

// 7. Złożone Query
var report = projects
    .Where(p => p.Status == ProjectStatus.Active)
    .OrderByDescending(p => p.ProgressPercentage)
    .Select(p => new 
    {
        p.Name,
        p.ProgressPercentage,
        TaskCount = p.Tasks.Count,
        CompletedCount = p.Tasks.Count(t => t.Status == TaskStatus.Done)
    })
    .ToList();
```

### EF Core LINQ-to-SQL

```csharp
// Asynchronous queries
var projects = await _context.Projects
    .Include(p => p.Tasks)
    .Where(p => p.Status == ProjectStatus.Active)
    .OrderByDescending(p => p.ProgressPercentage)
    .ToListAsync();

// Query execution (ToListAsync triggers SQL execution)
// SELECT p.* FROM Projects AS p
// WHERE p.Status = 'Active'
// ORDER BY p.ProgressPercentage DESC
// INNER JOIN Tasks AS t ON p.Id = t.ProjectId
```

---

## Funkcjonalności

### 📊 Dashboard
- Wyświetlanie wszystkich projektów z postępiem
- Progress bars z kolorami (zielony > 75%, żółty > 50%, pomarańczowy < 50%)
- Status badges (Active/OnHold/Completed)
- Szybkie statystyki (liczba zadań, postęp)

### 🔍 Wyszukiwanie i Sortowanie
- Wyszukiwanie po nazwie/opisie (case-insensitive)
- Sortowanie po: nazwie, dacie, postępie, liczbie zadań, statusie
- Kombinacja search + sort w jednym zapytaniu

### ➕ Tworzenie/Edycja
- Formularz z walidacją DataAnnotations
- Komunikaty o błędach
- Osadzony scheduling (date pickers)
- Progress slider (0-100%)

### 📋 Szczegóły i Analityka
- Informacje o projekcie
- Tabela wszystkich zadań z priorytetami
- Analytics cards:
  - Wszystkie zadania
  - Ukończone zadania
  - W toku
  - Przeterminowane
- Procent ukończenia
- Ostrzeżenia o przeterminowanych zadaniach

### 🗑️ Usuwanie
- Cascade delete (usunięcie projektu → usunięcie zadań)
- Potwierdzenie przed usunięciem

### 📊 Analytics Dashboard
- Podsumowanie wszystkich projektów
- Tabela ze statystykami
- Szczegółowe karty analiz
- Sorting projektów po postępie

---

## Architektura

### Warstwy aplikacji

```
┌─────────────────────────────────┐
│  Views (Razor + Bootstrap)      │ ← Presentation Layer
├─────────────────────────────────┤
│  Controllers (MVC)              │ ← API/Handler Layer
├─────────────────────────────────┤
│  Services + LINQ                │ ← Business Logic Layer
├─────────────────────────────────┤
│  DbContext (EF Core)            │ ← Data Access Layer
├─────────────────────────────────┤
│  SQL Server Database            │ ← Persistence Layer
└─────────────────────────────────┘
```

### Design Patterns

1. **MVC Pattern** - Separation of concerns
2. **Repository Pattern** - DbContext acts as repository
3. **Dependency Injection** - Loose coupling via DI container
4. **Service Layer** - Business logic centralization
5. **DataAnnotations** - Declarative validation
6. **Async/Await** - Non-blocking I/O

### Database Relations

```
Project (1) ──────────── (N) Task
  │
  ├── Id (PK)
  ├── Name
  ├── Description
  ├── StartDate
  ├── EndDate
  ├── ProgressPercentage
  ├── Status
  └── Tasks[] (Navigation)

Task
  │
  ├── Id (PK)
  ├── Title
  ├── Description
  ├── Priority (1-10)
  ├── Status
  ├── DueDate
  ├── ProjectId (FK)
  └── Project (Navigation)
```

---

## Status Modułu

### ✅ Zaimplementowane

| Komponent | Status | Plik |
|-----------|--------|------|
| Models | ✅ 100% | Models/*.cs |
| DbContext | ✅ 100% | Data/ApplicationDbContext.cs |
| Services | ✅ 100% | Services/IProjectService.cs |
| Controllers | ✅ 100% | Controllers/ProjectsController.cs |
| Views | ✅ 100% | Views/Projects/*.cshtml |
| Components | ✅ 100% | Components/*.razor |
| Program.cs | ✅ 100% | Program.cs |
| README | ✅ 100% | README.md |
| Exercises | ✅ 100% | tasks/EXERCISES.md |
| Diagrams | ✅ 100% | diagrams/diagrams.md |

### 📊 Statystyki Plików

- **Plik Models**: 4 pliki (Enums, Project, Task, Analytics)
- **Pliki Views**: 5 widoków Razor + 2 layout files
- **Pliki Components**: 2 komponenty Blazor
- **Pliki Services**: 1 service + interface
- **Pliki Controllers**: 1 kontroler (7 actions)
- **Dokumentacja**: 3 pliki (README, EXERCISES, diagrams)
- **Konfiguracja**: Program.cs, .csproj, appsettings.json
- **Total**: ~40 plików

### 🎯 Obejmowane Koncepty

- ✅ ASP.NET Core fundamentals
- ✅ MVC Pattern (Model-View-Controller)
- ✅ Dependency Injection
- ✅ Entity Framework Core
- ✅ LINQ (Language Integrated Query)
- ✅ DataAnnotations Validation
- ✅ Razor Templating
- ✅ Blazor Components
- ✅ HTTP Routing
- ✅ Async/Await
- ✅ Database Relationships (1:N)
- ✅ Database Indexing
- ✅ Seed Data

### 📚 Ćwiczenia

- **12 ćwiczeń** podzielonych na 3 poziomy:
  - 4 ćwiczenia poziom Beginner 🟢
  - 4 ćwiczenia poziom Intermediate 🟡
  - 4 ćwiczenia poziom Advanced 🔴

### 🎨 Wizualizacje

- **14 diagramów** Mermaid obejmujących:
  - Request flow
  - Database schema
  - DI container
  - EF Core pattern
  - LINQ execution
  - Razor rendering
  - Blazor lifecycle
  - Architecture layers
  - Performance optimization
  - Full request cycle

---

## Następne Kroki

1. **Wdrożenie TasksController** - Pełny CRUD dla Tasks
2. **API Endpoints** - Web API zamiast MVC
3. **Autentykacja** - ASP.NET Core Identity
4. **Unit Tests** - xUnit + Moq
5. **Performance** - Caching, optimization
6. **Deploy** - Azure/AWS

---

## Zasoby

- 📖 [Microsoft Docs - ASP.NET Core](https://learn.microsoft.com/aspnet/core/)
- 📖 [Microsoft Docs - Entity Framework Core](https://learn.microsoft.com/ef/core/)
- 📖 [Microsoft Docs - LINQ](https://learn.microsoft.com/dotnet/csharp/linq/)
- 📖 [Microsoft Docs - Razor](https://learn.microsoft.com/aspnet/core/mvc/views/razor/)
- 📖 [Microsoft Docs - Blazor](https://learn.microsoft.com/aspnet/core/blazor/)

---

**Status**: ✅ COMPLETED (100% implementacji)  
**Data Ukończenia**: 2025-08-31  
**Wersja**: 1.0.0  
**Framework**: .NET 9.0  
