using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductCode { get; set; }
        public string ReleaseDate { get; set; }
        public int CategoryId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public int StockQty { get; set; }
        public string Description { get; set; }        
        public decimal StarRating { get; set; }
        // public string ImageUrl { get; set; }
        public List<Image> Images { get; set; }
    }
}
