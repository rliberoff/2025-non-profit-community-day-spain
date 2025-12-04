namespace Demo.Agent.Fundraising.Models;

public record ChatResponse
{
    public required string Message { get; init; }
    public DateTime Timestamp { get; init; }
}