using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;

namespace Razor_Test.Pages
{
	public class ProfilePageModel : PageModel
    {
        [BindProperty]
        public string ChangedPassword { get; set; }
        [BindProperty]
        public string ChangedUsername { get; set; }
        [BindProperty]
        public Profile CurrentProfile { get; set; }
        [BindProperty]
        public IFormFile FormFile { get; set; }

        IWebHostEnvironment _webHostEnv;
        RazorAutomarketDbContext _dbContext;

        public ProfilePageModel(
            IWebHostEnvironment webEnv,
            RazorAutomarketDbContext dbContext)
        {
            CurrentProfile = ProfileTracker.CurrentProfile;
            _webHostEnv = webEnv;
            _dbContext = dbContext;
            Helper.ResourcesFolderName = "ProfileResources";
        }

        public IActionResult OnPostLogOut()
        {
            ProfileTracker.CurrentProfile = new Profile() { Username = "Account" };

            return RedirectToPage("Index");
        }

        public IActionResult OnPost()
        {
            CurrentProfile = ProfileTracker.CurrentProfile;

            if (FormFile != null)
            {
                Helper.SavePhoto(FormFile, _webHostEnv, CurrentProfile.Id);
                CurrentProfile.ImagePath = Helper.GetEntityPhotoRelativePath(CurrentProfile.Id);
            }

            var usernameAlreadyTaken = _dbContext.Profiles.Any(p => p.Username == ChangedUsername);

            if (ChangedUsername != CurrentProfile.Username && !usernameAlreadyTaken)
                CurrentProfile.Username = ChangedUsername;

            _dbContext.Update(CurrentProfile);
            _dbContext.SaveChanges();

            return RedirectToPage("ProfilePage");
        }
    }
}