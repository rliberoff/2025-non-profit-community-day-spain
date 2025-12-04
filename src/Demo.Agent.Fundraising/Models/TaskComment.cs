namespace Demo.Agent.Fundraising.Models;

/// <summary>
/// Represents a comment or note added to a task for collaboration.
/// </summary>
/// <param name="Id">Unique identifier for the comment (GUID).</param>
/// <param name="TaskId">ID of the task this comment belongs to.</param>
/// <param name="Text">Content of the comment.</param>
/// <param name="CreatedAt">Date and time when the comment was created (UTC).</param>
public record TaskComment(
    string Id,
    string TaskId,
    string Text,
    DateTime CreatedAt
);
