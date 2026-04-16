using MyVotingApp.Web.Models;

namespace MyVotingApp.Web.Services;

/// <summary>
/// Abstraction for voting operations. Implementations may use in-memory storage,
/// a database, or any other persistence mechanism.
/// </summary>
public interface IVoteService
{
    /// <summary>
    /// Returns all available voting items.
    /// </summary>
    Task<IReadOnlyList<VotingItem>> GetItemsAsync();

    /// <summary>
    /// Records a vote for the specified item.
    /// </summary>
    /// <param name="itemId">The id of the item to vote for.</param>
    /// <returns>True if the vote was recorded; false if the item id is invalid.</returns>
    Task<bool> VoteAsync(int itemId);

    /// <summary>
    /// Returns aggregated vote results for all items.
    /// </summary>
    Task<IReadOnlyList<VoteResult>> GetResultsAsync();
}
