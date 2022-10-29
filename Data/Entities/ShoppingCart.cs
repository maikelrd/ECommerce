using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        // public User User { get; set; }
        //public int UserId { get; set; }
       // public UsersEcommerce User { get; set; }
        public string UserId { get; set; }
        public List<ProductShoppingCart> ProductsInShoppingCart { get; set; }
    }
}
