using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor_Test.Pages
{
    public class ErrorDataPageModel : PageModel
    {
        [BindProperty]
        public List<string> Errors { get; set; }

        public void OnGet(List<string> errors)
        {
            Errors = errors;
        }
    }
}