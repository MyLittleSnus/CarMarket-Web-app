using Razor_Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Test.AdditionalEntities;
using System.ComponentModel.DataAnnotations;

namespace Razor_Test.Pages
{
	public class AuthorizationModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Email cannot be empty")]
        public string Email { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Username cannot be empty")]
        public string Username { get; set; }
        [BindProperty]
        [MinLength(4, ErrorMessage = "Password length must be at least 4 characters")]
        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }

        private RazorAutomarketDbContext _dbContext;
        private CryptographyHandler _cryptograpyHandler;

        public AuthorizationModel(RazorAutomarketDbContext dbContext, CryptographyHandler cryptographyHandler)
        {
            _dbContext = dbContext;
            _cryptograpyHandler = cryptographyHandler;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var profile = new Profile();
            var hashedPasword = _cryptograpyHandler.GetSha256Hash(Password);
            profile.Email = Email;
            profile.Password = hashedPasword;
            profile.Username = Username;
            profile.CreatedAt = DateTime.Now;
            profile.ImagePath = "default.png";

            var alreadyUser = _dbContext.Profiles
                .Select(x => x.Email == Email || x.Username == Username)
                .FirstOrDefault();

            if (alreadyUser)
                RedirectToPage("Error");

            ProfileTracker.TrackProfile(profile);

            _dbContext.Add(profile);
            _dbContext.SaveChanges();

            ViewData["Username"] = profile.Username;

            return RedirectToPage("index", profile);
        }
    }
}