﻿using System.ComponentModel.DataAnnotations;

namespace BimManufact.WebApi.Models
{
    public class Manufacturer
    {
        public int ManufacturerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}