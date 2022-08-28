using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;

namespace Razor_Test.Pages
{
    public class AuntheticationModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Password field cannot be empty")]
        public string Password { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Email field cannot be empty")]
        public string Email { get; set; }

        private RazorAutomarketDbContext _dbContext;
        private CryptographyHandler _cryptographyHandler;

        public AuntheticationModel(RazorAutomarketDbContext dbContext, CryptographyHandler cryptographyHandler)
        {
            _dbContext = dbContext;
            _cryptographyHandler = cryptographyHandler;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            var hashedPassword = _cryptographyHandler.GetSha256Hash(Password);
            var profile = _dbContext.Profiles
                .Where(prof => prof.Password == hashedPassword && prof.Email == Email)
                .Cast<Profile>()
                .FirstOrDefault();

            if (profile == null)
                return RedirectToPage("Error");

            ProfileTracker.TrackProfile(profile);
            ViewData["Username"] = profile.Username;

            return RedirectToPage("Index", profile);
        }
    }
}