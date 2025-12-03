namespace AgentFundraising.Models;

/// <summary>
/// In-memory state manager for the fundraising agent.
/// Stores all campaigns, tasks, comments, and users in dictionaries.
/// </summary>
public class AgentState
{
    public Dictionary<string, Campaign> Campaigns { get; } = new();
    public Dictionary<string, CampaignTask> Tasks { get; } = new();
    public Dictionary<string, TaskComment> Comments { get; } = new();
    public Dictionary<string, User> Users { get; } = new();

    public AgentState()
    {
        // Pre-populate with 3 predefined users
        Users["u1-coordinadora"] = new User("u1-coordinadora", "Ana García", UserRole.Coordinator);
        Users["u2-voluntario"] = new User("u2-voluntario", "Carlos Ruiz", UserRole.Volunteer);
        Users["u3-voluntario"] = new User("u3-voluntario", "María López", UserRole.Volunteer);
    }
}
