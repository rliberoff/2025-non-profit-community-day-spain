using AgentFundraising.Models;

namespace AgentFundraising.Validators;

/// <summary>
/// Validator for comment-related operations.
/// </summary>
public static class CommentValidator
{
    /// <summary>
    /// Validates comment data before creation.
    /// </summary>
    /// <param name="text">Comment text.</param>
    /// <param name="taskId">ID of the associated task.</param>
    /// <param name="state">Agent state.</param>
    /// <returns>Error message if validation fails, null if valid.</returns>
    public static string? ValidateComment(string text, string taskId, AgentState state)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return "El texto del comentario no puede estar vacío.";
        }

        if (text.Length < 3)
        {
            return "El comentario debe tener al menos 3 caracteres.";
        }

        if (text.Length > 1000)
        {
            return "El comentario no puede exceder 1000 caracteres.";
        }

        // Validate that the task exists
        var taskError = TaskValidator.ValidateTaskExists(taskId, state);
        if (taskError != null)
        {
            return taskError;
        }

        return null; // Validation successful
    }
}
