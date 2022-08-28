using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;
using Razor_Test.Extensions;

namespace Razor_Test.Pages
{
    public class AdminPageModel : PageModel
    {
        class ValidationBox
        {
            [Required]
            [MinLength(1, ErrorMessage = "Двигун не обран")]
            public string[] Engines { get; set; }
            [Required]
            [MinLength(1, ErrorMessage = "Світло не обрано")]
            public string[] Lights { get; set; }
            [Required]
            [MinLength(1, ErrorMessage = "Коробка не обрана")]
            public string[] GearBoxes { get; set; }
            [Required(ErrorMessage = "Мінімальна кількість місць не обрана")]
            public string MinSeat { get; set; }
            [Required(ErrorMessage = "Максимальна кількість місць не обрана")]
            public string MaxSeat { get; set; }
        }

        [BindProperty]
        public List<Offer> Offers { get; set; }
        [BindProperty]
        public NewOfferWithImageBox NewEntity { get; set; }
        [BindProperty]
        public Profile CurrentProfile { get; set; }
        [BindProperty]
        public CompleteCarModel CompleteCarModel { get; set; }
        [BindProperty]
        public List<Profile> AllUsers { get; set; }
        [BindProperty]
        public List<Profile> Buyers { get; set; }
        [BindProperty]
        public List<OrderModel> AllOrders { get; set; }

        private List<string> errors = new();
        private List<string> FaultedProperties = new();

        private RazorAutomarketDbContext _dbContext;
        private IWebHostEnvironment _webEnv;
        private SettingsMaster _settingsMaster;

        public AdminPageModel(RazorAutomarketDbContext dbContext, IWebHostEnvironment env, SettingsMaster settingsMaster)
        {
            _dbContext = dbContext;
            _webEnv = env;
            CurrentProfile = ProfileTracker.CurrentProfile;
            _settingsMaster = settingsMaster;

            Offers = _dbContext.Offers
                .Include(o => o.CarModel)
                .ToList();
            AllOrders = _dbContext.Orders
                .Include(o => o.Customer)
                .ToList();
            Buyers = AllOrders
                .Select(o => o.Customer)
                .DistinctBy(c => c.Username)
                .ToList();
            AllUsers = _dbContext.Profiles.ToList();
        }

        private List<string> GetErrorList()
        {
            var errors = ModelState.Values
                   .Where(v => v.ValidationState == ModelValidationState.Invalid)
                   .Select(v => v.Errors)
                   .ToList();

            var errorMessages = new List<string>();

            foreach (var collection in errors)
            {
                foreach (var item in collection)
                {
                    errorMessages.Add(item.ErrorMessage);
                }
            }

            return errorMessages;
        }

        public IActionResult OnPostSaveOffer(string[] engines, string[] lights, string[] gearBoxes, string minSeat, string maxSeat, int id)
        {
            ModelState.Clear();

            if (!TryValidateModel(new ValidationBox() { Engines = engines, GearBoxes = gearBoxes, Lights = lights, MaxSeat = maxSeat, MinSeat = minSeat}))
                errors = errors.Union(GetErrorList()).ToList();

            if (!TryValidateModel(NewEntity.Offer))
                errors = errors.Union(GetErrorList()).ToList();

            if (errors.Count > 0)
            {
                TempData.Set("FaultedProperties", FaultedProperties);
                TempData.Set("Errors", errors);
                return RedirectToPage("ErrorDataPage", new { errors = errors });
            }

            NewEntity.Offer.Id = id;

            var carModelSettings = new KeyValuePair<
            (string Manufacturer, string Name, int year),
            (string[] MinMaxEngineVolume, string[] AvailableSeatNumbers, string[] AvailableLight, string[] AvailableGearBoxes)>
            ((NewEntity.Offer.CarModel.Manufacturer, NewEntity.Offer.CarModel.Model, NewEntity.Offer.CarModel.Year),
            (engines, new string[] { minSeat, maxSeat }, lights, gearBoxes));

            _settingsMaster.AddSettings(carModelSettings);

            Helper.ResourcesFolderName = "CarResources";
            Helper.SavePhoto(NewEntity.File, _webEnv, NewEntity.Offer.Id);
            NewEntity.Offer.CarModel.ImagePath
                = Helper.GetEntityPhotoRelativePath(NewEntity.Offer.CarModel.Id);

            _dbContext.Add(NewEntity.Offer);
            _dbContext.SaveChanges();
            Offers = _dbContext.Offers.Include(o => o.CarModel).ToList();

            return Page();
        }

        public void OnGetDeleteOffer(int id)
        {
            var offer = Offers.Where(x => x.Id == id).FirstOrDefault();
            var car = offer.CarModel;

            _dbContext.RemoveRange(offer, car);
            _dbContext.SaveChanges();

            _settingsMaster.RemoveSettings(car.Manufacturer, car.Model, car.Year);
            
            Offers = _dbContext.Offers.Include(o => o.CarModel).ToList();
        }

        public void OnPostRedirectToForm(int id)
        {
            RedirectToPage("AdminOrderPage", id);
        }
    }
}