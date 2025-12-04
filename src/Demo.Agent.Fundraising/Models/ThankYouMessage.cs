namespace Demo.Agent.Fundraising.Models;

/// <summary>
/// Represents an AI-generated thank-you message for a donor.
/// </summary>
/// <param name="DonorName">Name of the donor to thank.</param>
/// <param name="CampaignName">Name of the campaign the donation supports.</param>
/// <param name="GeneratedText">AI-generated thank-you message text in Spanish.</param>
/// <param name="AmountEuros">Optional donation amount in euros.</param>
public record ThankYouMessage(
    string DonorName,
    string CampaignName,
    string GeneratedText,
    decimal? AmountEuros = null
);
