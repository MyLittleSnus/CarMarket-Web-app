using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Razor_Test.Pages
{
    public class MainCarPageModel : PageModel
    {
        [BindProperty]
        public Offer? Offer { get; set; }
        [BindProperty]
        public CompleteCarModel CompleteCarModel { get; set; }
        [BindProperty]
        public OrderModel NewOrder { get; set; }
        [BindProperty]
        public bool InvokeCodeMessage { get; set; }
        public Profile CurrentProfile { get; set; }

        private RazorAutomarketDbContext _dbContext;
        private SettingsMaster _settingsMaster;

        public MainCarPageModel(
            RazorAutomarketDbContext razorAutomarketDbContext,
            SettingsMaster settingsMaster)
        {
            _dbContext = razorAutomarketDbContext;
            _settingsMaster = settingsMaster;
            CurrentProfile = ProfileTracker.CurrentProfile;
        }

        public void OnGet(int id)
        {
            Offer = _dbContext.Offers
                .Where(o => o.Id == id)
                .Include(o => o.CarModel)
                .Include(o => o.PeopleViewed)
                .FirstOrDefault();

            var dbprofile = _dbContext.Profiles
                .Where(p => p.Id == CurrentProfile.Id)
                .Include(p => p.OffersVisited)
                .FirstOrDefault();

            if (Offer.PeopleViewed == null)
                Offer.PeopleViewed = new();

            if(dbprofile != null)
            {
                if (!Offer.PeopleViewed.Contains(dbprofile))
                {
                    Offer.PeopleViewed.Add(dbprofile);
                    dbprofile.OffersVisited.Add(Offer);
                }
            }

            _dbContext.SaveChanges();

            var settings = _settingsMaster.GetSettings(Offer.CarModel.Manufacturer, Offer.CarModel.Model, Offer.CarModel.Year);

            CompleteCarModel = new()
            {
                CarViewModel = Offer.CarModel,
                AvailableGearBoxes = settings.AvailableGearBoxes.ToList(),
                EngineVolumes = settings.MinMaxEngineVolume.ToList(),
                LightTypes = settings.AvailableLight.ToList(),
                MinSeats = int.Parse(settings.AvailableSeatNumbers[0]),
                MaxSeats = int.Parse(settings.AvailableSeatNumbers[settings.AvailableSeatNumbers.Length - 1]),
            };
        }

        public void OnPostCreateOrder(int id)
        {
            InvokeCodeMessage = true;

            NewOrder.Code = Convert.ToString(new Random().Next(100000000, 999999999));
            NewOrder.Customer = _dbContext.Profiles
                .Where(p => p.Id == CurrentProfile.Id)
                .FirstOrDefault();
            NewOrder.CreatedAt = DateTime.Now;
            NewOrder.IsApproved = false;
            _dbContext.Add(NewOrder);
            _dbContext.SaveChanges();

            OnGet(id);
        }

        public void OnPostToggle()
        {
            InvokeCodeMessage = false;
        }
    }
}