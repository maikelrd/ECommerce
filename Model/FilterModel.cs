using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class FilterModel
    {
        public ProductModel[] Products { get; set; }
        public int count { get; set; }
    }
}
