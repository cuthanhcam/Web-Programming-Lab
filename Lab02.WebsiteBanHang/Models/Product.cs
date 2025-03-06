using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab02.WebsiteBanHang.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required, StringLength(100)]
        public string? Name { get; set; }

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        public int CategoryId { get; set; } // Foreign key

        public string? ImageUrl { get; set; } // Ảnh chính

        public List<string>? ImageUrls { get; set; } = new List<string>(); // Ảnh bổ sung
    }
}
