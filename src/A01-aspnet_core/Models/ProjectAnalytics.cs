namespace ProjectManager.Models;

/// <summary>
/// ViewModel dla statystyk i analiz projektu
/// Zawiera obliczone metryki
/// </summary>
public class ProjectAnalytics
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int InProgressTasks { get; set; }
    public int ToDoTasks { get; set; }
    public int OverdueTasksCount { get; set; }
    
    public int CompletionPercentage { get; set; }
    
    // Średni priorytet zadań
    public double AveragePriority { get; set; }
    
    // Zadania pogrupowane po priorytecie
    public Dictionary<int, int> TasksByPriority { get; set; } = new();
    
    // Zadania pogrupowane po statusie
    public Dictionary<TaskStatus, int> TasksByStatus { get; set; } = new();
    
    public DateTime? EarliestDueDate { get; set; }
    public DateTime? LatestDueDate { get; set; }
}

/// <summary>
/// ViewModel dla widoku szczegółów projektu
/// Łączy projekt z analizą
/// </summary>
public class ProjectDetailsViewModel
{
    public Project Project { get; set; } = new();
    public ProjectAnalytics Analytics { get; set; } = new();
    public List<Task> UpcomingTasks { get; set; } = new();
    public List<Task> OverdueTasks { get; set; } = new();
}
