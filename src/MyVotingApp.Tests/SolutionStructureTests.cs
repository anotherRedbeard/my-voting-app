namespace MyVotingApp.Tests;

public class SolutionStructureTests
{
    [Fact]
    public void WebProject_IsReferenceable()
    {
        // Verify the web project assembly can be loaded by the test project
        var assembly = typeof(Program).Assembly;
        Assert.NotNull(assembly);
        Assert.Equal("MyVotingApp.Web", assembly.GetName().Name);
    }
}