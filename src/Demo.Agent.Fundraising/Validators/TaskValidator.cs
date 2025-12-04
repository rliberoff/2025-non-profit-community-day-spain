using Demo.Agent.Fundraising.Models;

namespace Demo.Agent.Fundraising.Validators;

/// <summary>
/// Validator for task-related operations.
/// </summary>
public static class TaskValidator
{
    /// <summary>
    /// Validates task data before creation.
    /// </summary>
    /// <param name="description">Task description.</param>
    /// <param name="campaignId">ID of the associated campaign.</param>
    /// <param name="assignedTo">ID of the assigned user (optional).</param>
    /// <param name="state">Agent state.</param>
    /// <returns>Error message if validation fails, null if valid.</returns>
    public static string? ValidateTask(
        string description,
        string campaignId,
        string? assignedTo,
        AgentState state)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return "La descripción de la tarea no puede estar vacía.";
        }

        if (description.Length < 5)
        {
            return "La descripción de la tarea debe tener al menos 5 caracteres.";
        }

        if (description.Length > 500)
        {
            return "La descripción de la tarea no puede exceder 500 caracteres.";
        }

        // Validate that the campaign exists
        var campaignError = CampaignValidator.ValidateCampaignExists(campaignId, state);
        if (campaignError != null)
        {
            return campaignError;
        }

        // Validate assigned user if provided
        if (!string.IsNullOrWhiteSpace(assignedTo))
        {
            if (!state.Users.ContainsKey(assignedTo))
            {
                return $"No se encontró el usuario con ID '{assignedTo}'. Usuarios válidos: {string.Join(", ", state.Users.Keys)}.";
            }
        }

        return null; // Validation successful
    }

    /// <summary>
    /// Validates moving a task to a new column.
    /// </summary>
    /// <param name="taskId">ID of the task to move.</param>
    /// <param name="newColumn">Destination column.</param>
    /// <param name="state">Agent state.</param>
    /// <returns>Error message if validation fails, null if valid.</returns>
    public static string? ValidateTaskMove(
        string taskId,
        KanbanColumn newColumn,
        AgentState state)
    {
        if (string.IsNullOrWhiteSpace(taskId))
        {
            return "El ID de la tarea no puede estar vacío.";
        }

        if (!state.Tasks.ContainsKey(taskId))
        {
            return $"No se encontró la tarea con ID '{taskId}'.";
        }

        var task = state.Tasks[taskId];

        // All transitions are valid according to spec
        // We only verify it's not already in the destination column
        if (task.Column == newColumn)
        {
            return $"La tarea ya se encuentra en la columna '{newColumn}'.";
        }

        return null; // Validation successful
    }

    /// <summary>
    /// Validates that a task exists in the state.
    /// </summary>
    /// <param name="taskId">ID of the task to validate.</param>
    /// <param name="state">Agent state.</param>
    /// <returns>Error message if it doesn't exist, null if it exists.</returns>
    public static string? ValidateTaskExists(string taskId, AgentState state)
    {
        if (string.IsNullOrWhiteSpace(taskId))
        {
            return "El ID de la tarea no puede estar vacío.";
        }

        if (!state.Tasks.ContainsKey(taskId))
        {
            return $"No se encontró la tarea con ID '{taskId}'.";
        }

        return null; // Validation successful
    }
}
