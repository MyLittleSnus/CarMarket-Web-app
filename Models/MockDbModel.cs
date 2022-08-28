using System;
namespace Razor_Test.Models
{
	public static class MockDbModel
	{
		public static List<Offer> Offers { get; set; }
		public static List<CarViewModel> Cars { get; set; }

		public static void FillDbContext(RazorAutomarketDbContext dbContext)
        {
			dbContext.AddRange(Offers);
			dbContext.AddRange(Cars);
        }
	}
}