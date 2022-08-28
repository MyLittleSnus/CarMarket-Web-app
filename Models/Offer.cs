using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor_Test.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Немає короткого опису")]
        public string ShortDescription { get; set; }
        [Required(ErrorMessage = "Немає повного опису")]
        public string FullDescription { get; set; }

        [ForeignKey("CarId")]
        public CarViewModel CarModel { get; set; }

        public List<Profile>? PeopleViewed { get; set; }
    }
}