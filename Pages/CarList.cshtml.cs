using Razor_Test.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Razor_Test.AdditionalEntities;
using Microsoft.EntityFrameworkCore;

namespace Razor_Test.Pages
{
    public class CarListModel : PageModel
    {
        [BindProperty]
        public List<Offer> Offers { get; set; }
        [BindProperty(SupportsGet = true)] //unsafe method
        public SearchViewModel SearchViewModel { get; set; }
        [BindProperty]
        public Profile CurrentProfile { get; set; }

        private RazorAutomarketDbContext _dbContext;

        public CarListModel(RazorAutomarketDbContext dbContext)
        {
            CurrentProfile = ProfileTracker.CurrentProfile;
            _dbContext = dbContext;

            Offers = _dbContext.Offers
                .Include(o => o.CarModel)
                .Include(o => o.PeopleViewed)
                .ToList();
        }
        
        public void OnGet()
        {
            Offers = Offers.Where(offer => { return (offer.CarModel.Cost <= SearchViewModel.FinalCost && offer.CarModel.Cost >= SearchViewModel.StartCost); }).ToList();
            if (SearchViewModel.Country != "ALL")
                Offers = Offers.Where(offer => offer.CarModel.Country == SearchViewModel.Country).ToList();
            if (SearchViewModel.CarType != -1)
                Offers = Offers.Where(offer => offer.CarModel.Type == SearchViewModel.CarType).ToList();
        }

        public void OnGetMostPopularOffers()
        {
            var max = _dbContext.Offers
                .Select(o => o.PeopleViewed)
                .Max(x => x.Count);

            var minBorder = max - 2 > 0 ? max - 2 : 1;

            Offers = _dbContext.Offers
                .Where(o => o.PeopleViewed.Count >= minBorder &&
                            o.PeopleViewed.Count <= max)
                .ToList();
        }

       public void OnGetMostRecentModels()
       {
            var currentYear = DateTime.Now.Year;
            var yearCounter = currentYear;
            List<Offer> mostRecentOffers = null;

            while (mostRecentOffers == null || mostRecentOffers.Count == 0)
            {
                if (yearCounter == currentYear - 5)
                    break;
                mostRecentOffers = _dbContext.Offers
                .Where(o => o.CarModel.Year == yearCounter)
                .ToList();

                yearCounter--;
            }

            Offers = mostRecentOffers;
       }

        public IActionResult GetOfferInfo(int id)
        {
            var offer = Offers
                .Where(x => x.Id == id)
                .FirstOrDefault();

            return offer == null ? RedirectToPage("CarPage", offer) : NotFound();
        }
    }
}