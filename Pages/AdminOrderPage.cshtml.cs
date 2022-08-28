using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Razor_Test.AdditionalEntities;
using Razor_Test.Models;

namespace Razor_Test.Pages
{
    public class AdminOrderPageModel : PageModel
    {
        [BindProperty]
        public OrderModel? Order { get; set; }
        [BindProperty]
        public CompleteCarModel CompleteCarModel { get; set; }
        [BindProperty]
        public Profile CurrentProfile { get; set; }

        private RazorAutomarketDbContext _dbContext;
        private SettingsMaster _settingsMaster;

        public AdminOrderPageModel(RazorAutomarketDbContext dbContext, SettingsMaster settingsMaster)
        {
            _dbContext = dbContext;
            _settingsMaster = settingsMaster;
            CurrentProfile = ProfileTracker.CurrentProfile;
        }

        private void MouldOrderById(int id)
        {
            Order = _dbContext.Orders
               .Where(o => o.Id == id)
               .Include(o => o.Customer)
               .FirstOrDefault();

            var settings = _settingsMaster.GetSettings(Order.CarManufacturer, Order.CarModel, Order.Year);
            var carModel = _dbContext.Cars
                .Where(c =>
            c.Manufacturer == Order.CarManufacturer &&
            c.Model == Order.CarModel &&
            c.Year == Order.Year)
                .FirstOrDefault();

            CompleteCarModel = new()
            {
                CarViewModel = carModel,
                AvailableGearBoxes = settings.AvailableGearBoxes.ToList(),
                EngineVolumes = settings.MinMaxEngineVolume.ToList(),
                LightTypes = settings.AvailableLight.ToList(),
                MinSeats = int.Parse(settings.AvailableSeatNumbers[0]),
                MaxSeats = int.Parse(settings.AvailableSeatNumbers[settings.AvailableSeatNumbers.Length - 1]),
            };
        }

        public void OnGet(int Id)
        {
            MouldOrderById(Id);
        }

        public IActionResult OnPostApproveOrder(int Id)
        {
            MouldOrderById(Id);
            AssignCustomerToOrder();
            Order.IsApproved = true;

            _dbContext.Update(Order);
            _dbContext.SaveChanges();

            return RedirectToPage("AdminOrderPage", new { Id = Order.Id });
        }

        public IActionResult OnPostUpdateOrder()
        {
            AssignCustomerToOrder();
            ModelState.Clear();

            ViewData["SavedId"] = Order.Id;
            return RedirectToAction("AdminOrderPage", new { Id = Order.Id });

            _dbContext.Update(Order);
            _dbContext.SaveChanges();

            return RedirectToPage("AdminOrderPage", new { Id = Order.Id });
        }

        public IActionResult OnPostDeleteOrder(int id)
        {
            var orderToDelete = _dbContext.Orders
                .Where(x => x.Id == id)
                .FirstOrDefault();
            _dbContext.Remove(orderToDelete);
            _dbContext.SaveChanges();
            return RedirectToPage("AdminPage");
        }

        private void AssignCustomerToOrder()
        {
            var customer = _dbContext.Orders
               .Where(o => o.Id == Order.Id)
               .Include(o => o.Customer)
               .Select(o => o.Customer)
               .FirstOrDefault();

            Order.Customer = customer;
        }
    }
}