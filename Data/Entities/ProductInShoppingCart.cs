using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ProductInShoppingCart
    {
        public Product ProductShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
