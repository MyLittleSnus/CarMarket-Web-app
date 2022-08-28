using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Razor_Test.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public Profile? Customer { get; set; }

        public string Code { get; set; }
        public int TotalPrice { get; set; }
        public bool IsApproved { get; set; }

        [Required]
        public string CarManufacturer { get; set; }
        [Required]
        public string CarModel { get; set; }
        [Required]
        public string SeatNumber { get; set; }
        [Required]
        public string EngineVolume { get; set; }
        [Required]
        public string GearBox { get; set; }
        [Required]
        public string Light { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string PaymentType { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}