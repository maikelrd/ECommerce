using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Model
{
    public class ProductInShoppingCartModel
    {
        public Product ProductShoppingCart { get; set; }
        public int Quantity { get; set; }
    }
}
