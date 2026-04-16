namespace MyVotingApp.Web.Models;

/// <summary>
/// Represents the aggregated vote total for a single voting item.
/// Used in the MVP to return current vote counts without tracking individual votes.
/// </summary>
public class VoteResult
{
    /// <summary>
    /// The voting item this result belongs to.
    /// </summary>
    public required VotingItem Item { get; set; }

    /// <summary>
    /// Total number of votes cast for this item.
    /// </summary>
    public int VoteCount { get; set; }
}
