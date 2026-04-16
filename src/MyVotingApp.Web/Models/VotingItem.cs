namespace MyVotingApp.Web.Models;

/// <summary>
/// Represents a voteable item that users can cast votes for.
/// </summary>
public class VotingItem
{
    /// <summary>
    /// Unique identifier for the voting item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Display name shown to voters.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Optional description providing additional context about the item.
    /// </summary>
    public string? Description { get; set; }
}
