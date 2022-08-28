using System.ComponentModel.DataAnnotations;

namespace Razor_Test.Models
{
	public class CarViewModel
	{
		[MaxLength(20, ErrorMessage = "Назва виробника більна аніж  20 символів!")]
		[Required(ErrorMessage = "Не вказан виробник!")]
		public string? Manufacturer { get; set; }
		[MaxLength(20, ErrorMessage = "Назва моделі більна аніж 20 символів!")]
		[Required(ErrorMessage = "Не вказана модель!")]
		public string? Model { get; set; }
		[MaxLength(20, ErrorMessage = "Назва країни більна аніж  20 символів!")]
		[Required(ErrorMessage = "Не вказана країна!")]
		public string? Country { get; set; }
		[MaxLength(40)]
		public string? ImagePath { get; set; }

		[Key]
		public int Id { get; set; }
		[Required(ErrorMessage = "Не вказана ціна авто!")]
		public int Cost { get; set; }
		[Required(ErrorMessage = "Не вказан рік випуску авто!")]
		public int Year { get; set; }
		public int Type { get; set; }
	}
}