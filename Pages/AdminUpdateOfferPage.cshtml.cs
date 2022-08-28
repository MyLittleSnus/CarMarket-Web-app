using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;

namespace Razor_Test.Pages
{
    public class AdminUpdateOfferPageModel : PageModel
    {
        [BindProperty]
        public bool HasV2 { get; set; }
        [BindProperty]
        public bool HasV4 { get; set; }
        [BindProperty]
        public bool HasV6 { get; set; }
        [BindProperty]
        public bool HasV8 { get; set; }
        [BindProperty]
        public bool HasV10 { get; set; }

        [BindProperty]
        public bool HasHalogen { get; set; }
        [BindProperty]
        public bool HasLED { get; set; }
        [BindProperty]
        public bool HasXenon { get; set; }

        [BindProperty]
        public bool HasManual { get; set; }
        [BindProperty]
        public bool HasAuto { get; set; }

        [BindProperty]
        public Offer? Offer { get; set; }
        [BindProperty]
        public IFormFile OfferImage { get; set; }
        [BindProperty]
        public CompleteCarModel CompleteCarModel { get; set; }

        private IWebHostEnvironment _webHost;
        private RazorAutomarketDbContext _dbContext { get; set; }
        private SettingsMaster _settingsMaster { get; set; }

        public AdminUpdateOfferPageModel(RazorAutomarketDbContext dbContext, IWebHostEnvironment webHost, SettingsMaster settingsMaster)
        {
            _dbContext = dbContext;
            _webHost = webHost;
            _settingsMaster = settingsMaster;
        }

        public void OnGet(int id)
        {
            Offer = _dbContext.Offers
                .Where(o => o.Id == id)
                .Include(o => o.CarModel)
                .FirstOrDefault();

            var settings = _settingsMaster.GetSettings(Offer.CarModel.Manufacturer, Offer.CarModel.Model, Offer.CarModel.Year);

            CompleteCarModel = new()
            {
                AvailableGearBoxes = settings.AvailableGearBoxes.ToList(),
                EngineVolumes = settings.MinMaxEngineVolume.ToList(),
                LightTypes = settings.AvailableLight.ToList(),
                MinSeats = int.Parse(settings.AvailableSeatNumbers[0]),
                MaxSeats = int.Parse(settings.AvailableSeatNumbers[1]),
                CarViewModel = Offer.CarModel
            };

            HasV2 = CompleteCarModel.EngineVolumes.Contains("V2");
            HasV4 = CompleteCarModel.EngineVolumes.Contains("V4");
            HasV6 = CompleteCarModel.EngineVolumes.Contains("V6");
            HasV8 = CompleteCarModel.EngineVolumes.Contains("V8");
            HasV10 = CompleteCarModel.EngineVolumes.Contains("V10");

            HasHalogen = CompleteCarModel.LightTypes.Contains("Halogen");
            HasLED = CompleteCarModel.LightTypes.Contains("LED");
            HasXenon = CompleteCarModel.LightTypes.Contains("Xenon");

            HasAuto = CompleteCarModel.AvailableGearBoxes.Contains("AUTO");
            HasManual = CompleteCarModel.AvailableGearBoxes.Contains("MANUAL");
        }

        public IActionResult OnPostUpdateOffer(string[] engines, string[] lights, string[] gearBoxes, string minSeat, string maxSeat, int id, int carId)
        {
            var settings = (engines, new string[] {minSeat, maxSeat}, lights, gearBoxes);
            _settingsMaster.UpdateSettings(
                Offer.CarModel.Manufacturer,
                Offer.CarModel.Model,
                Offer.CarModel.Year,
                settings);

            Offer.Id = id;
            Offer.CarModel.Id = carId;

            if (OfferImage != null)
            {
                Helper.ResourcesFolderName = "CarResources";
                Helper.SavePhoto(OfferImage, _webHost, Offer.Id);
                Offer.CarModel.ImagePath = Helper.GetEntityPhotoRelativePath(Offer.Id);
            }

            _dbContext.Update(Offer);
            _dbContext.SaveChanges();

            return RedirectToPage("AdminUpdateOfferPage", new { id = Offer.Id });
        }
    }
}