namespace ProjectManager.Models;

/// <summary>
/// Status projektu - aktywny, wstrzymany lub ukończony
/// </summary>
public enum ProjectStatus
{
    Active,
    OnHold,
    Completed
}

/// <summary>
/// Status zadania - do zrobienia, w toku, ukończone
/// </summary>
public enum TaskStatus
{
    ToDo,
    InProgress,
    Done
}
