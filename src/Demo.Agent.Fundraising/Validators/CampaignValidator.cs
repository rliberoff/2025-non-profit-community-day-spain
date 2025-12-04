using Demo.Agent.Fundraising.Models;

namespace Demo.Agent.Fundraising.Validators;

/// <summary>
/// Validator for campaign-related operations.
/// </summary>
public static class CampaignValidator
{
    /// <summary>
    /// Validates campaign data before creation or modification.
    /// </summary>
    /// <param name="name">Campaign name.</param>
    /// <param name="goalEuros">Financial goal in euros.</param>
    /// <returns>Error message if validation fails, null if valid.</returns>
    public static string? ValidateCampaign(string name, decimal goalEuros)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "El nombre de la campaña no puede estar vacío.";
        }

        if (name.Length < 3)
        {
            return "El nombre de la campaña debe tener al menos 3 caracteres.";
        }

        if (name.Length > 100)
        {
            return "El nombre de la campaña no puede exceder 100 caracteres.";
        }

        if (goalEuros <= 0)
        {
            return "La meta financiera debe ser mayor que cero.";
        }

        if (goalEuros > 1_000_000)
        {
            return "La meta financiera no puede exceder 1,000,000 euros.";
        }

        return null; // Validation successful
    }

    /// <summary>
    /// Validates that a campaign exists in the state.
    /// </summary>
    /// <param name="campaignId">ID of the campaign to validate.</param>
    /// <param name="state">Agent state.</param>
    /// <returns>Error message if validation fails, null if valid.</returns>
    public static string? ValidateCampaignExists(string campaignId, AgentState state)
    {
        if (string.IsNullOrWhiteSpace(campaignId))
        {
            return "El ID de la campaña no puede estar vacío.";
        }

        if (!state.Campaigns.ContainsKey(campaignId))
        {
            return $"Campaña '{campaignId}' no encontrada. Campañas disponibles: {string.Join(", ", state.Campaigns.Keys)}";
        }

        return null; // Validation successful
    }
}
