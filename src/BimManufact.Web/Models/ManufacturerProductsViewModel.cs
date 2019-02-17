using System.ComponentModel.DataAnnotations;

namespace BimManufact.Web.Models
{
    public class ManufacturerProductsViewModel
    {
        public ManufacturerViewModel Manufacturer { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int ProductId { get; set; }
    }
}