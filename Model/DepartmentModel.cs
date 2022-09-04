using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class DepartmentModel
    {
        public int DepartmentId { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        public List<Category> Categories { get; set; }
    }
}
