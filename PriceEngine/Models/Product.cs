using PriceEngine.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PriceEngine.Models
{
    public class Product : IProduct
    {
        [Key]
        [Required]
        [Display(Name = "productNumber")]
        public string ProductNumber { get; set; }

        [Required]
        [Display(Name = "productName")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "cartonSize")]
        public int CartonSize { get; set; }

        [Required]
        [Display(Name = "boxFullCost")]
        public decimal? BoxFullCost { get; set; }

        [Required]
        [Display(Name = "singleUnitMarkupPercentage")]
        public int SingleUnitMarkupPercentage { get; set; }
    }
}
