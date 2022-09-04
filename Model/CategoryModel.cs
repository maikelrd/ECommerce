using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public int DepartmentId { get; set; }
        public List<Product> Products { get; set; }
    }
}
