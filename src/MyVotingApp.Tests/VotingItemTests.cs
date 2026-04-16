using MyVotingApp.Web.Models;

namespace MyVotingApp.Tests;

public class VotingItemTests
{
    [Fact]
    public void VotingItem_CanBeCreated_WithRequiredFields()
    {
        var item = new VotingItem { Id = 1, Name = "Option A" };

        Assert.Equal(1, item.Id);
        Assert.Equal("Option A", item.Name);
        Assert.Null(item.Description);
    }

    [Fact]
    public void VotingItem_CanHaveOptionalDescription()
    {
        var item = new VotingItem
        {
            Id = 2,
            Name = "Option B",
            Description = "A detailed description"
        };

        Assert.Equal("A detailed description", item.Description);
    }
}
