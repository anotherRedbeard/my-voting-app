using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyVotingApp.Web.Models;
using MyVotingApp.Web.Services;

namespace MyVotingApp.Web.Pages;

public class IndexModel : PageModel
{
    private readonly IVoteService _voteService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IVoteService voteService, ILogger<IndexModel> logger)
    {
        _voteService = voteService;
        _logger = logger;
    }

    public IReadOnlyList<VotingItem> Items { get; private set; } = [];

    public IReadOnlyList<VoteResult> Results { get; private set; } = [];

    public bool ShowResults { get; private set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        Items = await _voteService.GetItemsAsync();
    }

    public async Task<IActionResult> OnPostAsync(int itemId)
    {
        var success = await _voteService.VoteAsync(itemId);

        if (!success)
        {
            _logger.LogWarning("Vote attempt for invalid item id {ItemId}", itemId);
            ErrorMessage = "Invalid item selected. Please choose a valid option.";
            return RedirectToPage();
        }

        _logger.LogInformation("Vote recorded for item {ItemId}", itemId);
        Items = await _voteService.GetItemsAsync();
        Results = await _voteService.GetResultsAsync();
        ShowResults = true;
        return Page();
    }
}
