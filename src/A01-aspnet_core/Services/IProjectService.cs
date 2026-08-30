using ProjectManager.Models;

namespace ProjectManager.Services;

/// <summary>
/// Interfejs usługi projektów
/// Definiuje operacje CRUD i analityczne
/// </summary>
public interface IProjectService
{
    // CRUD Operations
    Task<List<Project>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(int id);
    Task CreateProjectAsync(Project project);
    Task UpdateProjectAsync(Project project);
    Task DeleteProjectAsync(int id);

    // Analytics
    Task<ProjectAnalytics> GetProjectAnalyticsAsync(int projectId);
    Task<List<ProjectAnalytics>> GetAllProjectsAnalyticsAsync();

    // Filtering & Sorting
    Task<List<Project>> SearchProjectsAsync(string searchTerm);
    Task<List<Project>> GetProjectsBySortAsync(string sortBy);
}

/// <summary>
/// Implementacja usługi projektów z LINQ
/// Zawiera wszystkie operacje biznesowe
/// </summary>
public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Pobiera wszystkie projekty z ich zadaniami
    /// </summary>
    public async Task<List<Project>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }

    /// <summary>
    /// Pobiera projekt po ID z jego zadaniami
    /// </summary>
    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Tworzy nowy projekt
    /// </summary>
    public async Task CreateProjectAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Aktualizuje projekt
    /// </summary>
    public async Task UpdateProjectAsync(Project project)
    {
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Usuwa projekt i jego zadania (cascade)
    /// </summary>
    public async Task DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Pobiera analitykę dla konkretnego projektu
    /// Zawiera liczby zadań, procent ukończenia, itp.
    /// </summary>
    public async Task<ProjectAnalytics> GetProjectAnalyticsAsync(int projectId)
    {
        var project = await GetProjectByIdAsync(projectId);
        if (project == null)
            return new ProjectAnalytics();

        var tasks = project.Tasks;

        // Obliczanie statystyk z LINQ
        var analytics = new ProjectAnalytics
        {
            ProjectId = projectId,
            ProjectName = project.Name,
            TotalTasks = tasks.Count,
            CompletedTasks = tasks.Count(t => t.Status == TaskStatus.Done),
            InProgressTasks = tasks.Count(t => t.Status == TaskStatus.InProgress),
            ToDoTasks = tasks.Count(t => t.Status == TaskStatus.ToDo),
            OverdueTasksCount = tasks.Count(t => 
                t.DueDate < DateTime.Now && t.Status != TaskStatus.Done),
            
            CompletionPercentage = tasks.Count > 0 
                ? (tasks.Count(t => t.Status == TaskStatus.Done) * 100) / tasks.Count 
                : 0,
            
            AveragePriority = tasks.Any() 
                ? Math.Round(tasks.Average(t => t.Priority), 2) 
                : 0,
            
            // GroupBy - zadania pogrupowane po priorytecie
            TasksByPriority = tasks
                .GroupBy(t => t.Priority)
                .OrderByDescending(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Count()),
            
            // GroupBy - zadania pogrupowane po statusie
            TasksByStatus = tasks
                .GroupBy(t => t.Status)
                .ToDictionary(g => g.Key, g => g.Count()),
            
            EarliestDueDate = tasks.Any() ? tasks.Min(t => t.DueDate) : null,
            LatestDueDate = tasks.Any() ? tasks.Max(t => t.DueDate) : null
        };

        return analytics;
    }

    /// <summary>
    /// Pobiera analitykę dla wszystkich projektów
    /// </summary>
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

    /// <summary>
    /// Wyszukuje projekty po nazwie (case-insensitive)
    /// Używa LINQ Where
    /// </summary>
    public async Task<List<Project>> SearchProjectsAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllProjectsAsync();

        var projects = await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();

        return projects;
    }

    /// <summary>
    /// Sortuje projekty według wybranego kryterium
    /// Demonstracja różnych sposobów sortowania z LINQ
    /// </summary>
    public async Task<List<Project>> GetProjectsBySortAsync(string sortBy)
    {
        var projects = await _context.Projects
            .Include(p => p.Tasks)
            .ToListAsync();

        return sortBy switch
        {
            "name" => projects
                .OrderBy(p => p.Name)
                .ToList(),
            
            "date" => projects
                .OrderByDescending(p => p.StartDate)
                .ToList(),
            
            "progress" => projects
                .OrderByDescending(p => p.ProgressPercentage)
                .ToList(),
            
            "tasks" => projects
                .OrderByDescending(p => p.Tasks.Count)
                .ToList(),
            
            "completion" => projects
                .OrderByDescending(p => p.CompletedTasks)
                .ToList(),
            
            "status-active" => projects
                .Where(p => p.Status == ProjectStatus.Active)
                .OrderByDescending(p => p.StartDate)
                .ToList(),
            
            _ => projects
                .OrderByDescending(p => p.StartDate)
                .ToList()
        };
    }
}
