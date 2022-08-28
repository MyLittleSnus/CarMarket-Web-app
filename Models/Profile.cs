using System;
using System.ComponentModel.DataAnnotations;

namespace Razor_Test.Models
{
	public class Profile
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string? ImagePath { get; set; }
		public DateTime CreatedAt { get; set; }
		public bool IsAdmin { get; set; } = false;

		public List<Offer>? OffersVisited { get; set; }
	}
}