using System.ComponentModel.DataAnnotations;

namespace BimManufact.Web.Models
{
    public class ManufacturerRequestViewModel
    {
        public int ManufacturerId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}