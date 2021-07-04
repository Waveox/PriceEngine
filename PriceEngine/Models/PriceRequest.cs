using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PriceEngine.Models
{
    public class PriceRequest
    {
        [Required]
        [FromQuery(Name = "productNumber")]
        public string ProductNumber { get; set; }

        [Required]
        [FromQuery(Name = "units")]
        public int Units { get; set; }
    }
}
