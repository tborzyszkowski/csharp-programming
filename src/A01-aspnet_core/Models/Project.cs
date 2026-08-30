using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models;

/// <summary>
/// Model projektu - główna encja aplikacji
/// Relacja 1:Wiele z Task
/// </summary>
public class Project
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa projektu jest wymagana")]
    [StringLength(100, MinimumLength = 3, 
        ErrorMessage = "Nazwa musi mieć od 3 do 100 znaków")]
    public string Name { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Opis nie może być dłuższy niż 500 znaków")]
    public string Description { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "Data Rozpoczęcia")]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Data Zakończenia")]
    public DateTime? EndDate { get; set; }

    [Range(0, 100, ErrorMessage = "Postęp musi być między 0 a 100%")]
    [Display(Name = "Postęp (%)")]
    public int ProgressPercentage { get; set; }

    [Display(Name = "Status Projektu")]
    public ProjectStatus Status { get; set; } = ProjectStatus.Active;

    // Navigation property - relacja 1:Wiele
    public ICollection<Task> Tasks { get; set; } = new List<Task>();

    // Computed properties
    public int TotalTasks => Tasks.Count;
    public int CompletedTasks => Tasks.Count(t => t.Status == TaskStatus.Done);
    public int OverdueTasksCount => Tasks.Count(t => 
        t.DueDate < DateTime.Now && t.Status != TaskStatus.Done);
    
    public bool IsActive => Status == ProjectStatus.Active;
    public bool IsOverdue => EndDate.HasValue && EndDate < DateTime.Now && 
        Status != ProjectStatus.Completed;
}
