using PriceEngine.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PriceEngine.Models
{
    public class FullPrice : IFullPrice
    {
        [Key]
        [Required]
        [Display(Name = "productNumber")]
        public string ProductNumber { get; set; }

        [Required]
        [Display(Name = "productName")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "requestedUnits")]
        public int RequestedUnits { get; set; }

        [Required]
        [Display(Name = "totalCost")]
        public decimal? TotalCost { get; set; }
    }
}
