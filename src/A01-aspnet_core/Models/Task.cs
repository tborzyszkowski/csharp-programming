using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Models;

/// <summary>
/// Model zadania - zależne od projektu (relacja N:1)
/// </summary>
public class Task
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tytuł zadania jest wymagany")]
    [StringLength(200, MinimumLength = 3,
        ErrorMessage = "Tytuł musi mieć od 3 do 200 znaków")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000, ErrorMessage = "Opis nie może być dłuższy niż 1000 znaków")]
    public string Description { get; set; } = string.Empty;

    [Range(1, 10, ErrorMessage = "Priorytet musi być między 1 (najniższy) a 10 (najwyższy)")]
    [Display(Name = "Priorytet")]
    public int Priority { get; set; } = 5;

    [Display(Name = "Status Zadania")]
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;

    [DataType(DataType.Date)]
    [Display(Name = "Termin")]
    public DateTime DueDate { get; set; }

    // Foreign Key
    [ForeignKey("Project")]
    public int ProjectId { get; set; }

    // Navigation property
    public Project? Project { get; set; }

    // Computed properties
    public bool IsOverdue => DueDate < DateTime.Now && Status != TaskStatus.Done;
    public bool IsCompleted => Status == TaskStatus.Done;
    public bool IsHighPriority => Priority >= 8;
}
