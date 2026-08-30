using Microsoft.EntityFrameworkCore;
using ProjectManager.Models;

namespace ProjectManager.Data;

/// <summary>
/// Entity Framework Core DbContext dla aplikacji ProjectManager
/// Zarządza modelami Project i Task
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja relacji Project -> Tasks (1:Wiele)
        modelBuilder.Entity<Task>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indeksy dla poprawy performance
        modelBuilder.Entity<Project>()
            .HasIndex(p => p.Status);

        modelBuilder.Entity<Task>()
            .HasIndex(t => t.ProjectId);

        modelBuilder.Entity<Task>()
            .HasIndex(t => t.Status);

        // Seed data - przykładowe dane
        SeedInitialData(modelBuilder);
    }

    /// <summary>
    /// Wstawia przykładowe dane przy pierwszej migracji
    /// </summary>
    private static void SeedInitialData(ModelBuilder modelBuilder)
    {
        // Przykładowy projekt
        var project1 = new Project
        {
            Id = 1,
            Name = "System CRM",
            Description = "Nowy system zarządzania relacjami z klientami",
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 6, 30),
            ProgressPercentage = 65,
            Status = ProjectStatus.Active
        };

        var project2 = new Project
        {
            Id = 2,
            Name = "Modernizacja API",
            Description = "Aktualizacja API do wersji 2.0",
            StartDate = new DateTime(2024, 2, 15),
            EndDate = null,
            ProgressPercentage = 40,
            Status = ProjectStatus.Active
        };

        modelBuilder.Entity<Project>().HasData(project1, project2);

        // Przykładowe zadania dla projektu 1
        var tasks1 = new List<Task>
        {
            new Task
            {
                Id = 1,
                Title = "Analiza wymagań",
                Description = "Zebranie i analiza wymagań od klientów",
                Priority = 9,
                Status = TaskStatus.Done,
                DueDate = new DateTime(2024, 1, 15),
                ProjectId = 1
            },
            new Task
            {
                Id = 2,
                Title = "Design bazy danych",
                Description = "Projektowanie struktury bazy danych",
                Priority = 10,
                Status = TaskStatus.Done,
                DueDate = new DateTime(2024, 1, 30),
                ProjectId = 1
            },
            new Task
            {
                Id = 3,
                Title = "Implementacja API",
                Description = "Napisanie REST API dla systemu",
                Priority = 8,
                Status = TaskStatus.InProgress,
                DueDate = new DateTime(2024, 3, 15),
                ProjectId = 1
            },
            new Task
            {
                Id = 4,
                Title = "Testy jednostkowe",
                Description = "Napisanie testów dla logiki biznesowej",
                Priority = 7,
                Status = TaskStatus.ToDo,
                DueDate = new DateTime(2024, 4, 1),
                ProjectId = 1
            },
            new Task
            {
                Id = 5,
                Title = "Deploy na produkcję",
                Description = "Wdrożenie systemu na serwer produkcyjny",
                Priority = 10,
                Status = TaskStatus.ToDo,
                DueDate = new DateTime(2024, 6, 30),
                ProjectId = 1
            }
        };

        // Przykładowe zadania dla projektu 2
        var tasks2 = new List<Task>
        {
            new Task
            {
                Id = 6,
                Title = "Review starego API",
                Description = "Analiza obecnego API v1.0",
                Priority = 8,
                Status = TaskStatus.Done,
                DueDate = new DateTime(2024, 2, 20),
                ProjectId = 2
            },
            new Task
            {
                Id = 7,
                Title = "Projektowanie API v2.0",
                Description = "Nowa architektura REST API",
                Priority = 9,
                Status = TaskStatus.InProgress,
                DueDate = new DateTime(2024, 4, 1),
                ProjectId = 2
            }
        };

        modelBuilder.Entity<Task>().HasData(tasks1.Concat(tasks2));
    }
}
