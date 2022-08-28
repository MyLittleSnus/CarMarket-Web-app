using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Test.Models;
using Razor_Test.AdditionalEntities;

namespace Razor_Test.Pages
{
    public class AdminUserInfoModel : PageModel
    {
        [BindProperty]
        public Profile? UserProfile { get; set; }
        [BindProperty]
        public Profile CurrentProfile { get; set; }

        private RazorAutomarketDbContext _dbContext;
        public AdminUserInfoModel(RazorAutomarketDbContext dbContext)
        {
            _dbContext = dbContext;
            CurrentProfile = ProfileTracker.CurrentProfile;
        }

        public void OnGet(int id)
        {
            UserProfile = _dbContext.Profiles
                .Where(p => p.Id == id)
                .Include(p => p.OffersVisited)
                .FirstOrDefault();
        }
    }
}
