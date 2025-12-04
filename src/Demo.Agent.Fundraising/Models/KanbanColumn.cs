namespace Demo.Agent.Fundraising.Models;

/// <summary>
/// Represents the columns of a Kanban board for task workflow management.
/// </summary>
public enum KanbanColumn
{
    /// <summary>
    /// Task is new or pending (Por Hacer).
    /// </summary>
    ToDo,

    /// <summary>
    /// Task is currently being worked on (En Progreso).
    /// </summary>
    InProgress,

    /// <summary>
    /// Task is completed (Completado).
    /// </summary>
    Done
}
