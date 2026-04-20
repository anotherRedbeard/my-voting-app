using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging.Abstractions;
using MyVotingApp.Web.Pages;
using MyVotingApp.Web.Services;

namespace MyVotingApp.Tests;

public class IndexModelTests
{
    private static IndexModel CreateModel()
    {
        var service = new InMemoryVoteService();
        var logger = NullLogger<IndexModel>.Instance;
        var model = new IndexModel(service, logger);

        var httpContext = new DefaultHttpContext();
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
        var modelMetadataProvider = new EmptyModelMetadataProvider();
        var viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
        var tempData = new TempDataDictionary(httpContext, new FakeTempDataProvider());

        model.PageContext = new PageContext(actionContext)
        {
            ViewData = viewData
        };
        model.TempData = tempData;

        return model;
    }

    [Fact]
    public async Task OnGetAsync_PopulatesItems()
    {
        var model = CreateModel();

        await model.OnGetAsync();

        Assert.Equal(3, model.Items.Count);
        Assert.False(model.ShowResults);
    }

    [Fact]
    public async Task OnPostAsync_ValidItemId_ReturnsPageResultWithResults()
    {
        var model = CreateModel();

        var result = await model.OnPostAsync(1);

        Assert.IsType<PageResult>(result);
        Assert.True(model.ShowResults);
        Assert.Equal(3, model.Results.Count);
        var pizzaResult = Assert.Single(model.Results, r => r.Item.Name == "Pizza");
        Assert.Equal(1, pizzaResult.VoteCount);
    }

    [Fact]
    public async Task OnPostAsync_InvalidItemId_RedirectsWithErrorMessage()
    {
        var model = CreateModel();

        var result = await model.OnPostAsync(999);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.False(model.ShowResults);
        Assert.Equal("Invalid item selected. Please choose a valid option.", model.ErrorMessage);
    }

    [Fact]
    public async Task OnPostAsync_ValidItemId_PopulatesItemsAndResults()
    {
        var model = CreateModel();

        await model.OnPostAsync(2);

        Assert.Equal(3, model.Items.Count);
        Assert.Equal(3, model.Results.Count);

        var tacosResult = Assert.Single(model.Results, r => r.Item.Name == "Tacos");
        Assert.Equal(1, tacosResult.VoteCount);

        // Other items should have 0 votes
        var pizzaResult = Assert.Single(model.Results, r => r.Item.Name == "Pizza");
        Assert.Equal(0, pizzaResult.VoteCount);
    }

    [Fact]
    public async Task OnPostAsync_MultipleVotes_AccumulatesTotalsCorrectly()
    {
        var model = CreateModel();

        await model.OnPostAsync(1); // Pizza: 1
        await model.OnPostAsync(1); // Pizza: 2
        await model.OnPostAsync(2); // Tacos: 1

        Assert.True(model.ShowResults);
        Assert.Equal(3, model.Results.Count);

        var pizzaResult = Assert.Single(model.Results, r => r.Item.Name == "Pizza");
        Assert.Equal(2, pizzaResult.VoteCount);

        var tacosResult = Assert.Single(model.Results, r => r.Item.Name == "Tacos");
        Assert.Equal(1, tacosResult.VoteCount);

        var sushiResult = Assert.Single(model.Results, r => r.Item.Name == "Sushi");
        Assert.Equal(0, sushiResult.VoteCount);
    }

    [Fact]
    public async Task OnPostAsync_InvalidThenValid_RecoversProperly()
    {
        var model = CreateModel();

        // Invalid vote should redirect
        var invalidResult = await model.OnPostAsync(999);
        Assert.IsType<RedirectToPageResult>(invalidResult);
        Assert.False(model.ShowResults);

        // Valid vote should succeed and show results
        var validResult = await model.OnPostAsync(1);
        Assert.IsType<PageResult>(validResult);
        Assert.True(model.ShowResults);

        var pizzaResult = Assert.Single(model.Results, r => r.Item.Name == "Pizza");
        Assert.Equal(1, pizzaResult.VoteCount);
    }

    /// <summary>
    /// Simple fake ITempDataProvider for unit testing.
    /// </summary>
    private class FakeTempDataProvider : ITempDataProvider
    {
        private Dictionary<string, object?> _data = new();

        public IDictionary<string, object?> LoadTempData(HttpContext context) => _data;

        public void SaveTempData(HttpContext context, IDictionary<string, object?> values)
        {
            _data = new Dictionary<string, object?>(values);
        }
    }
}
