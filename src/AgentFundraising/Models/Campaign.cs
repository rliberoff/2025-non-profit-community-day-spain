namespace AgentFundraising.Models;

/// <summary>
/// Represents a fundraising campaign with a financial goal.
/// </summary>
/// <param name="Id">Unique identifier for the campaign (GUID).</param>
/// <param name="Name">Descriptive name of the campaign.</param>
/// <param name="GoalEuros">Financial goal in euros (must be greater than zero).</param>
/// <param name="CreatedAt">Date and time when the campaign was created (UTC).</param>
/// <param name="TaskIds">List of task IDs associated with this campaign.</param>
public record Campaign(
    string Id,
    string Name,
    decimal GoalEuros,
    DateTime CreatedAt,
    List<string>? TaskIds = null
);
