using MyVotingApp.Web.Services;

namespace MyVotingApp.Tests;

public class InMemoryVoteServiceTests
{
    [Fact]
    public async Task GetItemsAsync_ReturnsSeedItems()
    {
        var service = new InMemoryVoteService();

        var items = await service.GetItemsAsync();

        Assert.Equal(3, items.Count);
        Assert.Contains(items, i => i.Name == "Pizza");
        Assert.Contains(items, i => i.Name == "Tacos");
        Assert.Contains(items, i => i.Name == "Sushi");
    }

    [Fact]
    public async Task VoteAsync_ValidItemId_ReturnsTrue()
    {
        var service = new InMemoryVoteService();

        var result = await service.VoteAsync(1);

        Assert.True(result);
    }

    [Fact]
    public async Task VoteAsync_InvalidItemId_ReturnsFalse()
    {
        var service = new InMemoryVoteService();

        var result = await service.VoteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task GetResultsAsync_InitialCounts_AreZero()
    {
        var service = new InMemoryVoteService();

        var results = await service.GetResultsAsync();

        Assert.Equal(3, results.Count);
        Assert.All(results, r => Assert.Equal(0, r.VoteCount));
    }

    [Fact]
    public async Task GetResultsAsync_AfterVoting_ReflectsUpdatedTotals()
    {
        var service = new InMemoryVoteService();

        await service.VoteAsync(1);
        await service.VoteAsync(1);
        await service.VoteAsync(2);

        var results = await service.GetResultsAsync();

        var pizzaResult = Assert.Single(results, r => r.Item.Name == "Pizza");
        Assert.Equal(2, pizzaResult.VoteCount);

        var tacosResult = Assert.Single(results, r => r.Item.Name == "Tacos");
        Assert.Equal(1, tacosResult.VoteCount);

        var sushiResult = Assert.Single(results, r => r.Item.Name == "Sushi");
        Assert.Equal(0, sushiResult.VoteCount);
    }

    [Fact]
    public async Task VoteAsync_MultipleVotesSameItem_AccumulatesCorrectly()
    {
        var service = new InMemoryVoteService();

        for (int i = 0; i < 5; i++)
        {
            await service.VoteAsync(3);
        }

        var results = await service.GetResultsAsync();
        var sushiResult = Assert.Single(results, r => r.Item.Id == 3);
        Assert.Equal(5, sushiResult.VoteCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    public async Task VoteAsync_EdgeCaseIds_ReturnsFalse(int invalidId)
    {
        var service = new InMemoryVoteService();

        var result = await service.VoteAsync(invalidId);

        Assert.False(result);
    }

    [Fact]
    public async Task VoteAsync_ConcurrentVotes_AccumulateCorrectly()
    {
        var service = new InMemoryVoteService();
        const int votesPerItem = 100;

        var tasks = Enumerable.Range(0, votesPerItem)
            .SelectMany(_ => new[]
            {
                service.VoteAsync(1),
                service.VoteAsync(2),
                service.VoteAsync(3)
            });

        var results = await Task.WhenAll(tasks);

        Assert.All(results, r => Assert.True(r));

        var voteResults = await service.GetResultsAsync();
        Assert.All(voteResults, r => Assert.Equal(votesPerItem, r.VoteCount));
    }

    [Fact]
    public async Task GetResultsAsync_AfterVoting_MapsItemsToCorrectCounts()
    {
        var service = new InMemoryVoteService();

        await service.VoteAsync(1); // Pizza: 1
        await service.VoteAsync(2); // Tacos: 1
        await service.VoteAsync(2); // Tacos: 2
        await service.VoteAsync(3); // Sushi: 1
        await service.VoteAsync(3); // Sushi: 2
        await service.VoteAsync(3); // Sushi: 3

        var results = await service.GetResultsAsync();

        Assert.Equal(3, results.Count);
        Assert.Equal(1, results.Single(r => r.Item.Id == 1).VoteCount);
        Assert.Equal(2, results.Single(r => r.Item.Id == 2).VoteCount);
        Assert.Equal(3, results.Single(r => r.Item.Id == 3).VoteCount);
    }

    [Fact]
    public async Task VoteAsync_InvalidId_DoesNotAffectValidCounts()
    {
        var service = new InMemoryVoteService();

        await service.VoteAsync(1);
        await service.VoteAsync(999);

        var results = await service.GetResultsAsync();

        var pizzaResult = Assert.Single(results, r => r.Item.Id == 1);
        Assert.Equal(1, pizzaResult.VoteCount);

        // Ensure no phantom entries were created
        Assert.Equal(3, results.Count);
    }
}
