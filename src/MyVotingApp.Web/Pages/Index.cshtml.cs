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

    public async Task OnGetAsync()
    {
        Items = await _voteService.GetItemsAsync();
    }
}
