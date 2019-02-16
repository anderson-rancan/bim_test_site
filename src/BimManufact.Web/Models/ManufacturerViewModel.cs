using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BimManufact.Web.Models
{
    public class ManufacturerViewModel
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
    }
}