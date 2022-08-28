using Razor_Test.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Razor_Test.AdditionalEntities;

namespace Razor_Test.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    [BindProperty]
    public SearchViewModel SearchViewModel { get; set; }
    public Profile CurrentProfile { get; set; }

    private RazorAutomarketDbContext _dbContext;

    public IndexModel(ILogger<IndexModel> logger, RazorAutomarketDbContext dbContext)
    {
        _logger = logger;
        CurrentProfile = ProfileTracker.CurrentProfile;
        _dbContext = dbContext;
        //_dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        //MockDBModel.FillDbContext(_dbContext);
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("CarList", SearchViewModel);
    }
}