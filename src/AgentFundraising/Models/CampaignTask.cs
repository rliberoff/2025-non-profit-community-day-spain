namespace AgentFundraising.Models;

/// <summary>
/// Represents a task within a fundraising campaign, tracked on a Kanban board.
/// </summary>
/// <param name="Id">Unique identifier for the task (GUID).</param>
/// <param name="CampaignId">ID of the campaign this task belongs to.</param>
/// <param name="Description">Description of the task.</param>
/// <param name="Column">Current Kanban column (ToDo, InProgress, Done).</param>
/// <param name="AssignedTo">ID of the user assigned to this task (optional).</param>
/// <param name="CommentIds">List of comment IDs associated with this task.</param>
/// <param name="CreatedAt">Date and time when the task was created (UTC).</param>
public record CampaignTask(
    string Id,
    string CampaignId,
    string Description,
    KanbanColumn Column,
    string? AssignedTo,
    List<string>? CommentIds,
    DateTime CreatedAt
);
