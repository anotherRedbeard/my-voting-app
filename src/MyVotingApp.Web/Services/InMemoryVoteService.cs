using System.Collections.Concurrent;
using MyVotingApp.Web.Models;

namespace MyVotingApp.Web.Services;

/// <summary>
/// In-memory implementation of <see cref="IVoteService"/> for the MVP.
/// Uses static seed data and a thread-safe dictionary to track vote counts.
/// </summary>
public class InMemoryVoteService : IVoteService
{
    private readonly List<VotingItem> _items;
    private readonly ConcurrentDictionary<int, int> _votes;

    public InMemoryVoteService()
    {
        _items = new List<VotingItem>
        {
            new() { Id = 1, Name = "Pizza", Description = "Classic cheese or pepperoni pizza" },
            new() { Id = 2, Name = "Tacos", Description = "Soft or hard shell tacos" },
            new() { Id = 3, Name = "Sushi", Description = "Fresh rolls and nigiri" },
        };

        _votes = new ConcurrentDictionary<int, int>(
            _items.Select(i => new KeyValuePair<int, int>(i.Id, 0))
        );
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<VotingItem>> GetItemsAsync()
    {
        return Task.FromResult<IReadOnlyList<VotingItem>>(_items.AsReadOnly());
    }

    /// <inheritdoc />
    public Task<bool> VoteAsync(int itemId)
    {
        if (!_votes.ContainsKey(itemId))
        {
            return Task.FromResult(false);
        }

        _votes.AddOrUpdate(itemId, 1, (_, current) => current + 1);
        return Task.FromResult(true);
    }

    /// <inheritdoc />
    public Task<IReadOnlyList<VoteResult>> GetResultsAsync()
    {
        var results = _items.Select(item => new VoteResult
        {
            Item = item,
            VoteCount = _votes.GetValueOrDefault(item.Id, 0)
        }).ToList();

        return Task.FromResult<IReadOnlyList<VoteResult>>(results.AsReadOnly());
    }
}
