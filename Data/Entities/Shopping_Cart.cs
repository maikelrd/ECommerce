using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Shopping_Cart
    {
        public int ShoppingCartId { get; set; }        
        public string UserEmail { get; set; }

         public List<ProductInShoppingCart> ProductsInShoppingCart { get; set; }
        //public ProductInShoppingCart[] ProductsInShoppingCart { get; set; }

    }
}
