namespace Demo.Agent.Fundraising.Models;

/// <summary>
/// Represents a user (team member) in the fundraising system.
/// </summary>
/// <param name="Id">Unique identifier for the user.</param>
/// <param name="Name">Full name of the user.</param>
/// <param name="Role">User's role (Coordinator or Volunteer).</param>
public record User(
    string Id,
    string Name,
    UserRole Role
);
