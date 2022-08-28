using Microsoft.EntityFrameworkCore;

namespace Razor_Test.Models
{
	public class RazorAutomarketDbContext : DbContext
	{
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Offer> Offers { get; set; }
		public DbSet<CarViewModel> Cars { get; set; }
		public DbSet<OrderModel> Orders { get; set; }

		public RazorAutomarketDbContext(DbContextOptions options) : base(options) { }
	}
}