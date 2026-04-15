using MyVotingApp.Web.Models;

namespace MyVotingApp.Tests;

public class VoteResultTests
{
    [Fact]
    public void VoteResult_DefaultVoteCount_IsZero()
    {
        var result = new VoteResult
        {
            Item = new VotingItem { Id = 1, Name = "Option A" }
        };

        Assert.Equal(0, result.VoteCount);
    }

    [Fact]
    public void VoteResult_CanTrackVoteCount()
    {
        var result = new VoteResult
        {
            Item = new VotingItem { Id = 1, Name = "Option A" },
            VoteCount = 42
        };

        Assert.Equal(42, result.VoteCount);
        Assert.Equal("Option A", result.Item.Name);
    }
}
