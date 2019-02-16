using System;
using System.ComponentModel.DataAnnotations;

namespace BimManufact.WebApi.Models
{
    public class Product : ProductBase
    {
        [Required]
        public Guid AuditCreatedBy { get; set; }

        [Required]
        public DateTime AuditCreatedDate { get; set; }

        [Required]
        public Guid AuditLastModifiedBy { get; set; }

        [Required]
        public DateTime AuditLastModifiedDate { get; set; }
    }
}