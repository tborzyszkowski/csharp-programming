using Microsoft.AspNetCore.Mvc;
using ProjectManager.Models;
using ProjectManager.Services;

namespace ProjectManager.Controllers;

/// <summary>
/// Kontroler dla zarządzania projektami
/// HTTP GET/POST dla CRUD operacji
/// </summary>
[Route("projects")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// GET: /projects - Lista wszystkich projektów
    /// Obsługuje filtrowanie i sortowanie
    /// </summary>
    [HttpGet("")]
    [HttpGet("index")]
    public async Task<IActionResult> Index(string? searchTerm, string? sortBy)
    {
        List<Project> projects;

        // Filtrowanie
        if (!string.IsNullOrEmpty(searchTerm))
        {
            projects = await _projectService.SearchProjectsAsync(searchTerm);
        }
        else
        {
            projects = await _projectService.GetAllProjectsAsync();
        }

        // Sortowanie
        if (!string.IsNullOrEmpty(sortBy))
        {
            projects = await _projectService.GetProjectsBySortAsync(sortBy);
        }

        ViewData["CurrentSort"] = sortBy;
        ViewData["SearchTerm"] = searchTerm;

        return View(projects);
    }

    /// <summary>
    /// GET: /projects/create - Formularz tworzenia nowego projektu
    /// </summary>
    [HttpGet("create")]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// POST: /projects/create - Zapisuje nowy projekt
    /// Walidacja danych, wyświetlanie błędów
    /// </summary>
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        // Walidacja z DataAnnotations
        if (!ModelState.IsValid)
        {
            return View(project);
        }

        // Custom walidacja - EndDate nie może być wcześniej niż StartDate
        if (project.EndDate.HasValue && project.EndDate < project.StartDate)
        {
            ModelState.AddModelError("EndDate", 
                "Data zakończenia musi być Later niż data rozpoczęcia");
            return View(project);
        }

        await _projectService.CreateProjectAsync(project);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// GET: /projects/{id} - Szczegóły projektu z analitiką
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
            return NotFound();

        var analytics = await _projectService.GetProjectAnalyticsAsync(id);

        var viewModel = new ProjectDetailsViewModel
        {
            Project = project,
            Analytics = analytics,
            UpcomingTasks = project.Tasks
                .Where(t => t.DueDate >= DateTime.Now && t.Status != TaskStatus.Done)
                .OrderBy(t => t.DueDate)
                .ToList(),
            OverdueTasks = project.Tasks
                .Where(t => t.DueDate < DateTime.Now && t.Status != TaskStatus.Done)
                .OrderByDescending(t => t.DueDate)
                .ToList()
        };

        return View(viewModel);
    }

    /// <summary>
    /// GET: /projects/edit/{id} - Formularz edycji projektu
    /// </summary>
    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
            return NotFound();

        return View(project);
    }

    /// <summary>
    /// POST: /projects/edit/{id} - Zapisuje zmiany w projekcie
    /// </summary>
    [HttpPost("edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Project project)
    {
        if (id != project.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            return View(project);
        }

        // Custom walidacja
        if (project.EndDate.HasValue && project.EndDate < project.StartDate)
        {
            ModelState.AddModelError("EndDate", 
                "Data zakończenia musi być Later niż data rozpoczęcia");
            return View(project);
        }

        try
        {
            await _projectService.UpdateProjectAsync(project);
        }
        catch (Exception)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Details), new { id });
    }

    /// <summary>
    /// POST: /projects/delete/{id} - Usuwa projekt
    /// </summary>
    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
            return NotFound();

        await _projectService.DeleteProjectAsync(id);
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// GET: /projects/analytics - Dashboard analityk wszystkich projektów
    /// </summary>
    [HttpGet("analytics")]
    public async Task<IActionResult> Analytics()
    {
        var allAnalytics = await _projectService.GetAllProjectsAnalyticsAsync();
        return View(allAnalytics);
    }
}
